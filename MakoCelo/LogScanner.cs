using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using MakoCelo.Model;
using MakoCelo.Model.RelicApi;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Tracer.NLog;

namespace MakoCelo
{
    public class LogScanner
    {
        private readonly frmMain _frmMain;
        private readonly LogFileParser _logFileParser;

        public event EventHandler MatchFound;
        private readonly HttpClient _httpClient = new();
        private readonly JsonSerializer _jsonSerializer = new();

        public LogScanner(frmMain frmMain)
        {
            _frmMain = frmMain;
            _logFileParser = new LogFileParser();
        }

        protected virtual void OnMatchFound(EventArgs e)
        {
            EventHandler handler = MatchFound;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public Match StartScanningLogFile(Match previousMatch)
        {
            var matchFound = _logFileParser.ParsePlayersFromGameLog(_frmMain.PATH_Game);

            if (matchFound.IsMatchFound() && (previousMatch == null || matchFound.Id != previousMatch.Id))
            {
                matchFound.IsNewMatch = true;
                OnMatchFound(EventArgs.Empty);

                GetGroupedStatsFromRelicApi(matchFound);

                var allTeams = matchFound.AlliesPlayers.SelectMany(x => x.Teams).Where(x => x.Players.Count <= (int)matchFound.GameMode);

                var teamGrouping = allTeams.GroupBy(x => x.Id).Where(x => x.Count() == x.First().Players.Count).OrderByDescending(y => y.Count()).FirstOrDefault();

                if (teamGrouping != null)
                {
                    var team = teamGrouping.First();
                    foreach (var matchFoundAlliesPlayer in matchFound.AlliesPlayers)
                    {
                        if (team.Players.Any(x => x.RelicId == matchFoundAlliesPlayer.RelicId))
                        {

                            matchFoundAlliesPlayer.CurrentTeam = team;
                            matchFoundAlliesPlayer.CurrentTeamStats =
                                team.TeamStats.First(x => x.Side == Side.Allies);
                        }

                    }

                }

                var axsTeams = matchFound.AxisPlayers.SelectMany(x => x.Teams).Where(x => x.Players.Count <= (int)matchFound.GameMode);

                teamGrouping = axsTeams.GroupBy(x => x.Id).Where(x => x.Count() == x.First().Players.Count).OrderByDescending(y => y.Count()).FirstOrDefault();

                if (teamGrouping != null)
                {
                    var team = teamGrouping.First();
                    foreach (var matchFoundAlliesPlayer in matchFound.AxisPlayers)
                    {
                        if (team.Players.Any(x => x.RelicId == matchFoundAlliesPlayer.RelicId))
                        {
                            matchFoundAlliesPlayer.CurrentTeam = team;
                            matchFoundAlliesPlayer.CurrentTeamStats =
                                team.TeamStats.First(x => x.Side == Side.Axis);
                        }

                    }

                }
                
            }


            // R4.30 Clean up our GUI user indicators.
            _frmMain.Cursor = Cursors.Default;
            _frmMain.lbStatus.Text = "Ready";
            Application.DoEvents();

            return matchFound;
        }

        private void GetGroupedStatsFromRelicApi(Match matchFound)
        {
            var rawResp = "";
            try
            {

                var response = DownloadRawStats(matchFound.Players);

                for (var t = 1; t <= matchFound.Players.Count; t++)
                {
                    var currentPlayer = matchFound.Players[t - 1];

                    var currentPlayerData = response.StatGroups.First(x => x.Type == 1 && x.Members.Any(x => x.ProfileId == currentPlayer.RelicId)).Members.First();
                    currentPlayer.StatGroupId = currentPlayerData.PersonalStatGroupId;
                    currentPlayer.Country = new Country
                    {
                        Code = currentPlayerData.Country,
                        Name = Country_GetName(currentPlayerData.Country)
                    };

                    for (int i = 0; i < _frmMain.RelDataLeaderId.GetUpperBound(0); i++) //backward compatibility
                    {
                        for (int j = 0; j < _frmMain.RelDataLeaderId.GetUpperBound(1); j++) //backward compatibility
                        {
                            var leaderBoardPlayerData = response.LeaderBoardStats.FirstOrDefault(x => currentPlayerData.PersonalStatGroupId == x.StatGroupId && x.LeaderBoardId == _frmMain.RelDataLeaderId[i, j]);
                            if (leaderBoardPlayerData != null)
                            {
                                var personalStats = new PersonalStats
                                {
                                    Faction = (Faction)i,
                                    GameMode = (GameMode)j,
                                    Losses = leaderBoardPlayerData.Losses,
                                    Wins = leaderBoardPlayerData.Wins,
                                    Rank = Convert.ToInt32(leaderBoardPlayerData.Rank) == -1 ? 0 : Convert.ToInt32(leaderBoardPlayerData.Rank), //backward compatibility,
                                    RankLevel = leaderBoardPlayerData.RankLevel,
                                    TotalPlayers = leaderBoardPlayerData.RankTotal
                                };

                                currentPlayer.PersonalStats.Add(personalStats);
                                if (currentPlayer.CurrentFaction == personalStats.Faction)
                                    currentPlayer.CurrentPersonalStats = personalStats;

                            }
                        }
                    }

                    currentPlayer.Teams = response.StatGroups.Where(statGroup =>
                            statGroup.Type != 1 && statGroup.Members.Any(member => member.ProfileId == currentPlayer.RelicId))
                        .Select((statGroup, i) =>
                        {
                            return new Team
                            {
                                Id = statGroup.Id,
                                Players = statGroup.Members.Select(y => new TeamMember
                                {
                                    Name = y.Alias,
                                    RelicId = y.ProfileId
                                }).ToList(),
                                TeamStats = response.LeaderBoardStats
                                    .Where(leaderBoard => leaderBoard.StatGroupId == statGroup.Id).Take(2)
                                    .Select(leaderBoard => new TeamStats
                                    {
                                        Side = leaderBoard.LeaderBoardId is "20" or "22" or "24"
                                            ? Side.Axis
                                            : Side.Allies,
                                        Rank = leaderBoard.Rank,
                                        RankLevel = leaderBoard.RankLevel,
                                        Wins = leaderBoard.Wins,
                                        Losses = leaderBoard.Losses,
                                        TotalTeams = leaderBoard.RankTotal
                                    }).ToList()
                            };
                        }).ToList();

                }


            }
            catch (Exception)
            {
                // R4.41 Added logging and color change.
                _frmMain.LbError1.Text = "RID error";
                _frmMain.LbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID " +
                                          Information.Err().Description);
            }


        }

        private Response DownloadRawStats(List<Player> players)
        {
            using var responseMessage = Task.Run(() => _httpClient.GetAsync(
                "https://coh2-api.reliclink.com/community/leaderboard/GetPersonalStat?title=coh2&profile_ids=[" +
                 string.Join(",", players.Select(x => x.RelicId).ToArray()) + "]")).Result; // TODO: make whole app Async :)

            if (responseMessage.IsSuccessStatusCode)
            {
                var content = Task.Run(() => responseMessage.Content.ReadAsStreamAsync()).Result;

                using StreamReader sr = new StreamReader(content);
                using JsonReader reader = new JsonTextReader(sr);

                var deserializeObject = _jsonSerializer.Deserialize<Response>(reader);

                if (deserializeObject != null && deserializeObject.Result.Message == "SUCCESS")
                {
                    return deserializeObject;
                }

                if (deserializeObject != null)
                {
                    Log.Error(
                        $"{DateAndTime.Now.ToLongTimeString()} - Get RID - Error in Relic API, status code: {deserializeObject.Result.Code}, Message: {deserializeObject.Result.Message} ");

                }
            }
            else
            {
                Log.Error(
                    $"{DateAndTime.Now.ToLongTimeString()} - Get RID -  Error getting response, status code: {responseMessage.StatusCode}, Reason: {responseMessage.ReasonPhrase} ");

            }

            _frmMain.LbError1.Text = "RID error";
            _frmMain.LbError1.BackColor = Color.FromArgb(255, 255, 0, 0);

            return null;
        }

        private string Country_GetName(string ca)
        {
            var tName = "";
            for (int t = 1, loopTo = _frmMain.CountryCount1; t <= loopTo; t++)
                if ((ca ?? "") == (_frmMain.CountryAbbr[t] ?? ""))
                {
                    tName = _frmMain.CountryName[t];
                    break;
                }

            return tName;
        }
    }
}