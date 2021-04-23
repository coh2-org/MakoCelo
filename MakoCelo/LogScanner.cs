using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using MakoCelo.Model;
using MakoCelo.Model.RelicApi;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace MakoCelo
{
    public class LogScanner
    {
        private readonly frmMain _frmMain;
        private readonly LogFileParser _logFileParser;
        private Match _previousMatch;
        public event EventHandler MatchFound;
        private readonly HttpClient _httpClient = new();
        private readonly JsonSerializer _jsonSerializer = new();

        public LogScanner(frmMain frmMain)
        {
            _frmMain = frmMain;
            _logFileParser = new LogFileParser(frmMain);
        }

        protected virtual void OnMatchFound(EventArgs e)
        {
            EventHandler handler = MatchFound;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void StartScanningLogFile()
        {
            // R4.00 Read the RELIC log file and get the match stats.
            // R4.00 Stats come in two sections. Each has a Relic ID #.
            // R4.00 Match the two sections using the Relic ID #.
            // R4.50 Relic broke the file so now there is only one section.
            var tPlrRank = new string[10]; // R3.10 Added Storage to place old vals on screen if no new onesa are found.
            var tPlrName = new string[10];
            var tPlrSteam = new string[10];
            var tPlrRid = new string[10];
            var tPlrFact = new string[10];
            var tPlrTWin = new int[10];
            var tPlrTLoss = new int[10];
            var tPlrTeam = new int[10];
            var tPlrCountry = new string[10];
            var tPlrCountryName = new string[10];
            var tempTl = new clsGlobal.t_TeamList();
            var tTeamList = new clsGlobal.t_TeamList[10, 1001];
            var tTeamListCnt = new int[10];
            var tPlrRankAll = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrRankWin = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrRankLoss = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrRankPerc = new string[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrElo = new string[10];
            var tPlrLvl = new string[10];
            var tPlrGlvl = new int[10];


            // This will be replaced with object returned to form so there will be no need to buffer previous data at this point (it will be done after search)
            #region ToRemoveAfterMigration
            // R3.10 Clear the current match and find new data below. 
            for (var t = 1; t <= 8; t++)
            {
                tPlrRank[t] = _frmMain.PlrRank[t];
                _frmMain.PlrRank[t] = "---";
                tPlrName[t] = _frmMain.PlrName[t];
                _frmMain.PlrName[t] = "";
                tPlrSteam[t] = _frmMain.PlrSteam[t];
                _frmMain.PlrSteam[t] = "";
                tPlrRid[t] = _frmMain.PlrRID[t];
                _frmMain.PlrRID[t] = "";
                tPlrFact[t] = _frmMain.PlrFact[t];
                _frmMain.PlrFact[t] = "";
                tPlrTeam[t] = _frmMain.PlrTeam[t];
                _frmMain.PlrTeam[t] = 0;
                tPlrTWin[t] = _frmMain.PlrTWin[t];
                _frmMain.PlrTWin[t] = 0;
                tPlrTLoss[t] = _frmMain.PlrTLoss[t];
                _frmMain.PlrTLoss[t] = 0;
                tPlrCountry[t] = _frmMain.PlrCountry[t];
                _frmMain.PlrCountry[t] = "";
                tPlrCountryName[t] = _frmMain.PlrCountryName[t];
                _frmMain.PlrCountryName[t] = "";
                for (var t2 = 1; t2 <= 5; t2++)
                    for (var t3 = 1; t3 <= 4; t3++)
                    {
                        tPlrRankAll[t, t2, t3] = _frmMain.PlrRankALL[t, t2, t3];
                        _frmMain.PlrRankALL[t, t2, t3] = 0;
                        tPlrRankWin[t, t2, t3] = _frmMain.PlrRankWin[t, t2, t3];
                        _frmMain.PlrRankWin[t, t2, t3] = 0;
                        tPlrRankLoss[t, t2, t3] = _frmMain.PlrRankLoss[t, t2, t3];
                        _frmMain.PlrRankLoss[t, t2, t3] = 0;
                        tPlrRankPerc[t, t2, t3] = _frmMain.PlrRankPerc[t, t2, t3];
                        _frmMain.PlrRankPerc[t, t2, t3] = "";
                    }

                tTeamListCnt[t] = _frmMain.TeamListCnt[t];
                _frmMain.TeamListCnt[t] = 0;
                for (int t2 = 1, loopTo1 = _frmMain.TeamList.GetUpperBound(1); t2 <= loopTo1; t2++)
                {
                    tTeamList[t, t2] = _frmMain.TeamList[t, t2];
                    _frmMain.TeamList[t, t2] = tempTl;
                }

                tPlrElo[t] = _frmMain.PlrELO[t];
                _frmMain.PlrELO[t] = "";
                tPlrLvl[t] = _frmMain.PlrLVL[t];
                _frmMain.PlrLVL[t] = "";
                tPlrGlvl[t] = _frmMain.PlrGLVL[t];
                _frmMain.PlrGLVL[t] = 0;

                // R4.30 Store PUBLIC copy for LAST MATCH ability.
                _frmMain.PlrRank_Buffer[t] = tPlrRank[t];
                _frmMain.PlrName_Buffer[t] = tPlrName[t];
                _frmMain.PlrSteam_Buffer[t] = tPlrSteam[t];
                _frmMain.PlrRID_Buffer[t] = tPlrRid[t];
                _frmMain.PlrFact_Buffer[t] = tPlrFact[t];
                _frmMain.PlrTeam_Buffer[t] = tPlrTeam[t];
                _frmMain.PlrTWin_Buffer[t] = tPlrTWin[t];
                _frmMain.PlrTLoss_Buffer[t] = tPlrTLoss[t];
                _frmMain.PlrCountry_Buffer[t] = tPlrCountry[t]; // R4.45 Added.
                _frmMain.PlrCountryName_Buffer[t] = tPlrCountryName[t]; // R4.45 Added.
                for (var t2 = 1; t2 <= 5; t2++)
                    for (var t3 = 1; t3 <= 4; t3++)
                    {
                        _frmMain.PlrRankALL_Buffer[t, t2, t3] = tPlrRankAll[t, t2, t3];
                        _frmMain.PlrRankWin_Buffer[t, t2, t3] = tPlrRankWin[t, t2, t3];
                        _frmMain.PlrRankLoss_Buffer[t, t2, t3] = tPlrRankLoss[t, t2, t3];
                        _frmMain.PlrRankPerc_Buffer[t, t2, t3] = tPlrRankPerc[t, t2, t3];
                    }

                _frmMain.TeamListCnt_Buffer[t] = tTeamListCnt[t];
                for (int t2 = 1, loopTo2 = _frmMain.TeamList_Buffer.GetUpperBound(1); t2 <= loopTo2; t2++) _frmMain.TeamList_Buffer[t, t2] = tTeamList[t, t2];

                _frmMain.PlrELO_Buffer[t] = tPlrElo[t];
                _frmMain.PlrLVL_Buffer[t] = tPlrLvl[t];
                _frmMain.PlrGLVL_Buffer[t] = tPlrGlvl[t];
            }

            #endregion
            var matchFound = _logFileParser.ParsePlayersFromGameLog(tempTl);
            #region ToRemoveAfterMigration
            if (!matchFound.IsMatchFound()) //backward compatibility - won't be needed if we use _previousMatch
                for (var t = 1; t <= 8; t++)
                {
                    _frmMain.PlrRank[t] = tPlrRank[t];
                    _frmMain.PlrName[t] = tPlrName[t];
                    _frmMain.PlrSteam[t] = tPlrSteam[t];
                    _frmMain.PlrRID[t] = tPlrRid[t];
                    _frmMain.PlrFact[t] = tPlrFact[t];
                    _frmMain.PlrTeam[t] = tPlrTeam[t];
                    _frmMain.PlrTWin[t] = tPlrTWin[t];
                    _frmMain.PlrTLoss[t] = tPlrTLoss[t];
                    _frmMain.PlrCountry[t] = tPlrCountry[t]; // R4.45 Added.
                    _frmMain.PlrCountryName[t] = tPlrCountryName[t]; // R4.46 Added.
                    for (var t2 = 1; t2 <= 5; t2++)
                        for (var t3 = 1; t3 <= 4; t3++)
                        {
                            _frmMain.PlrRankALL[t, t2, t3] = tPlrRankAll[t, t2, t3];
                            _frmMain.PlrRankWin[t, t2, t3] = tPlrRankWin[t, t2, t3];
                            _frmMain.PlrRankLoss[t, t2, t3] = tPlrRankLoss[t, t2, t3];
                            _frmMain.PlrRankPerc[t, t2, t3] = tPlrRankPerc[t, t2, t3];
                        }

                    _frmMain.TeamListCnt[t] = tTeamListCnt[t];
                    for (int t2 = 1, loopTo4 = _frmMain.TeamList.GetUpperBound(1); t2 <= loopTo4; t2++) _frmMain.TeamList[t, t2] = tTeamList[t, t2];

                    _frmMain.PlrELO[t] = tPlrElo[t];
                    _frmMain.PlrLVL[t] = tPlrLvl[t];
                }
            #endregion ToRemoveAfterMigration

            if (matchFound.IsMatchFound() && (_previousMatch == null || matchFound.Id != _previousMatch.Id))
            {
                _previousMatch = matchFound;

                // R4.30 Reset the ELO cycle mode.
                _frmMain.RankDisplayMode = 0;
                STATS_StoreLast(); //backward compatibility - won't be needed if we use _previousMatch

                OnMatchFound(EventArgs.Empty);

                GetGroupedStatsFromRelicApi(matchFound);
                
                _frmMain.lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - Complete.");
                
                // R4.34 See if we should search the net for team ranks.
                if (_frmMain.chkGetTeams.Checked)
                {
                    var allTeams = matchFound.AlliesPlayers.SelectMany(x => x.Teams).Where(x => x.Players.Count <= (int)matchFound.GameMode);

                    var teamGrouping = allTeams.GroupBy(x => x.Id).Where(x => x.Count() == x.First().Players.Count).OrderByDescending(y => y.Count()).FirstOrDefault();

                    if (teamGrouping != null)
                    {
                        var team = teamGrouping.First();
                        foreach (var matchFoundAlliesPlayer in matchFound.AlliesPlayers)
                        {
                            if ( team.Players.Any(x => x.RelicId == matchFoundAlliesPlayer.RelicId))
                            {
                                matchFoundAlliesPlayer.CurrentTeam = team;
                                
                            }
                            
                        }

                    }

                    

                    var allaTeams = matchFound.AxisPlayers.SelectMany(x => x.Teams).Where(x => x.Players.Count <= (int)matchFound.GameMode);

                    var sorteda = allaTeams.GroupBy(x => x.Id).Where(x => x.Count() == x.First().Players.Count).OrderByDescending(y => y.Count()).ToList();

                    // R4.34 Set ranks if players are a team.
                    STAT_GetCheckForTeam();
                    _frmMain.lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - TEAM Check - Completed.");
                }

                // R4.34 Text-to-Speech the ranks.
                if (_frmMain.chkSpeech.Checked) STATS_ReadAloud();

                // R4.50 Force the STATS image redraw.
                _frmMain.MainBuffer_Valid = false;

                // R4.34 Draw the updated ranks.
                _frmMain.Gfx.GFX_DrawStats();
            }

            // R5.00 Overlay
            if (_frmMain.AutoScanEnabled && matchFound.IsMatchFound() && _frmMain._chkToggleOverlay.Checked) _frmMain._overlay.Run(_frmMain.PlrName, _frmMain.PlrRank);
            // R4.30 Clean up our GUI user indicators.
            _frmMain.Cursor = Cursors.Default;
            _frmMain.lbStatus.Text = "Ready";
            Application.DoEvents();
        }

        private void GetGroupedStatsFromRelicApi(Match matchFound)
        {
            var rawResp = "";
            try
            {
                // R4.30 Request leaderboard data from Relic Web API. Put result JSON data in string for parsing.
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() +
                                          " Web Request sending...");

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
                    _frmMain.PlrCountry[t] = currentPlayer.Country.Code; //backward compatibility
                    _frmMain.PlrCountryName[t] = currentPlayer.Country.Name; //backward compatibility

                    for (int i = 0; i < _frmMain.RelDataLeaderId.GetUpperBound(0); i++) //backward compatibility
                    {
                        for (int j = 0; j < _frmMain.RelDataLeaderId.GetUpperBound(1); j++) //backward compatibility
                        {
                            var leaderBoardPlayerData = response.LeaderBoardStats.FirstOrDefault(x => currentPlayerData.PersonalStatGroupId == x.StatGroupId && x.LeaderBoardId == _frmMain.RelDataLeaderId[i, j]);
                            if (leaderBoardPlayerData != null)
                            {
                                var rank = Convert.ToInt32(leaderBoardPlayerData.Rank) == -1 ? 0 : Convert.ToInt32(leaderBoardPlayerData.Rank); //backward compatibility
                                var percent = ((double)leaderBoardPlayerData.Wins /
                                              (leaderBoardPlayerData.Losses + leaderBoardPlayerData.Wins)).ToString("P");
                                var personalStats = new PersonalStats
                                {
                                    Faction = (Faction) i,
                                    GameMode = (GameMode) j,
                                    Losses = leaderBoardPlayerData.Losses,
                                    Wins = leaderBoardPlayerData.Wins,
                                    Rank = rank,
                                    RankLevel = leaderBoardPlayerData.RankLevel,
                                    TotalPlayers = leaderBoardPlayerData.RankTotal,
                                    WinLossPercentRatio = percent
                                };

                                currentPlayer.PersonalStats.Add(personalStats);
                                if (currentPlayer.CurrentFaction == personalStats.Faction)
                                    currentPlayer.CurrentPersonalStats = personalStats;

                                _frmMain.PlrRankWin[t, i, j] = leaderBoardPlayerData.Wins; //backward compatibility
                                _frmMain.PlrRankLoss[t, i, j] = leaderBoardPlayerData.Losses; //backward compatibility
                                _frmMain.PlrRankALL[t, i, j] = rank; //backward compatibility
                                _frmMain.PlrRankPerc[t, i, j] = percent; //backward compatibility
                                


                            }
                        }
                    }

                    currentPlayer.Teams = response.StatGroups.Where(statGroup =>
                            statGroup.Type != 1 && statGroup.Members.Any(member => member.ProfileId == currentPlayer.RelicId))
                        .Select((statGroup, i) =>
                        {
                            _frmMain.TeamList[t, i].PLR1 = statGroup.Members[0].Alias; //backward compatibility
                            _frmMain.TeamList[t, i].PLR2 = statGroup.Members.ElementAtOrDefault(1)?.Alias ?? ""; //backward compatibility
                            _frmMain.TeamList[t, i].PLR3 = statGroup.Members.ElementAtOrDefault(2)?.Alias ?? ""; //backward compatibility
                            _frmMain.TeamList[t, i].PLR4 = statGroup.Members.ElementAtOrDefault(3)?.Alias ?? ""; //backward compatibility
                            _frmMain.TeamList[t, i].RID1 = statGroup.Members[0].ProfileId; //backward compatibility
                            _frmMain.TeamList[t, i].RID2 = statGroup.Members.ElementAtOrDefault(1)?.ProfileId ?? ""; //backward compatibility
                            _frmMain.TeamList[t, i].RID3 = statGroup.Members.ElementAtOrDefault(2)?.ProfileId ?? ""; //backward compatibility
                            _frmMain.TeamList[t, i].RID4 = statGroup.Members.ElementAtOrDefault(3)?.ProfileId ?? ""; //backward compatibility
                            _frmMain.TeamList[t, i].RankID = Convert.ToInt32(statGroup.Id); //backward compatibility
                            _frmMain.TeamList[t, i].PlrCnt = statGroup.Type; //backward compatibility

                            return new Team
                            {
                                Id = statGroup.Id,
                                Players = statGroup.Members.Select(y => new TeamMember
                                {
                                    Name = y.Alias,
                                    RelicId = y.ProfileId
                                } ).ToList(),
                                TeamStats = response.LeaderBoardStats
                                    .Where(leaderBoard => leaderBoard.StatGroupId == statGroup.Id).Take(2)
                                    .Select(leaderBoard =>
                                    {
                                        var teamStats = new TeamStats
                                        {
                                            Side = leaderBoard.LeaderBoardId is "20" or "22" or "24"
                                                           ? Side.Axis
                                                           : Side.Allies,
                                            Rank = leaderBoard.Rank,
                                            RankLevel = leaderBoard.RankLevel,
                                            Wins = leaderBoard.Wins,
                                            Losses = leaderBoard.Losses,
                                            WinLossPercentRatio = ((double)leaderBoard.Wins /
                                                                              (leaderBoard.Losses + leaderBoard.Wins))
                                                           .ToString("P")
                                        };

                                        if (teamStats.Side == Side.Allies)
                                        {
                                            _frmMain.TeamList[t, i].RankAllies = Convert.ToInt32(leaderBoard.Rank); //backward compatibility
                                            _frmMain.TeamList[t, i].WinAllies = leaderBoard.Wins; //backward compatibility
                                            _frmMain.TeamList[t, i].LossAllies = leaderBoard.Losses; //backward compatibility
                                        }
                                        else
                                        {
                                            _frmMain.TeamList[t, i].RankAxis = Convert.ToInt32(leaderBoard.Rank); //backward compatibility
                                            _frmMain.TeamList[t, i].WinAxis = leaderBoard.Wins; //backward compatibility
                                            _frmMain.TeamList[t, i].LossAxis = leaderBoard.Losses; //backward compatibility
                                        }
                                        return teamStats;
                                    }).ToList()
                            };
                        }).ToList();
                    _frmMain.TeamListCnt[t] = currentPlayer.Teams.Count; //backward compatibility

                    _frmMain.PlrRank[t] = _frmMain.PlrRankALL[t, Conversions.ToInteger(_frmMain.PlrFact[t]), (int)matchFound.GameMode].ToString(); //backward compatibility
                    if (_frmMain.PlrRank[t] == "0") _frmMain.PlrRank[t] = "---";

                    

                    _frmMain.PlrWin[t] = _frmMain.PlrRankWin[t, Conversions.ToInteger(_frmMain.PlrFact[t]), (int)matchFound.GameMode]; //backward compatibility
                    _frmMain.PlrLoss[t] = _frmMain.PlrRankLoss[t, Conversions.ToInteger(_frmMain.PlrFact[t]), (int)matchFound.GameMode]; //backward compatibility
                    var currentMatchPersonalStats = currentPlayer.PersonalStats.First(x => x.Faction == currentPlayer.CurrentFaction && x.GameMode == matchFound.GameMode);
                    _frmMain.PlrELO[t] = currentMatchPersonalStats.Rank <= 0 ? "---" :
                        ((double)currentMatchPersonalStats.Rank / currentMatchPersonalStats.TotalPlayers)
                        .ToString("P"); //backward compatibility
                    _frmMain.PlrLVL[t] = Convert.ToInt32(currentMatchPersonalStats.RankLevel) <= 0 ? "---" : "L-" + currentMatchPersonalStats.RankLevel; //backward compatibility

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

                _frmMain.LstLog.Items.Add(
                    $"{DateAndTime.Now.ToLongTimeString()} - Get RID Parsing data...{content.Length} bytes");

                if (content.Length < 10) //not sure if needed after refactor
                {
                    _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID No data returned.");
                }
                else
                {
                    using StreamReader sr = new StreamReader(content);
                    using JsonReader reader = new JsonTextReader(sr);

                    var deserializeObject = _jsonSerializer.Deserialize<Response>(reader);

                    if (deserializeObject != null && deserializeObject.Result.Message == "SUCCESS")
                    {
                        return deserializeObject;
                    }

                    if (deserializeObject != null)
                    {
                        _frmMain.LstLog.Items.Add(
                            $"{DateAndTime.Now.ToLongTimeString()} - Get RID - Error in Relic API, status code: {deserializeObject.Result.Code}, Message: {deserializeObject.Result.Message} ");

                    }
                }
            }
            else
            {
                _frmMain.LstLog.Items.Add(
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

        private void STAT_GetCheckForTeam()
        {
            // R4.34 Added.
            var tempMax = default(long);
            var mCnt = new int[10];
            var mTeam = new int[10];

            // R4.41 Added TRY CATCH.
            try
            {
                // *********************************************************
                // R4.34 Loop thru TEAM 1 looking for possible teams.
                // *********************************************************
                int cnt;
                for (var t = 1; t <= 8; t += 2)
                    for (int t2 = 1, loopTo = _frmMain.TeamListCnt[t]; t2 <= loopTo; t2++)
                    {
                        cnt = 0;
                        if ("" != _frmMain.TeamList[t, t2].RID1)
                        {
                            if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                            if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                            if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                            if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID1) cnt += 1;
                        }

                        if ("" != _frmMain.TeamList[t, t2].RID2)
                        {
                            if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                            if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                            if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                            if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID2) cnt += 1;
                        }

                        if ("" != _frmMain.TeamList[t, t2].RID3)
                        {
                            if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                            if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                            if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                            if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID3) cnt += 1;
                        }

                        if ("" != _frmMain.TeamList[t, t2].RID4)
                        {
                            if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID4) cnt += 1;

                            if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID4) cnt += 1;

                            if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID4) cnt += 1;

                            if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID4) cnt += 1;
                        }

                        if (1 < cnt)
                            // we found a team.
                            if ((mCnt[t] <= cnt) & (_frmMain.TeamList[t, t2].PlrCnt <= cnt))
                            {
                                mCnt[t] = cnt;
                                mTeam[t] = t2;
                            }
                    }

                // R4.34 Decide if the team is Axis(20,22,24) or Allies(21,23,25) by faction.
                long tempRank;
                long tempWin;
                long tempLoss;
                for (var t = 1; t <= 8; t += 2)
                    if (0 < mTeam[t])
                    {
                        if ((_frmMain.PlrFact[t] == "01") | (_frmMain.PlrFact[t] == "03")) // R4.34 OST or OKW.
                        {
                            // R4.35 Added TEAM Win/Loss.
                            tempRank = _frmMain.TeamList[t, mTeam[t]].RankAxis;
                            tempWin = _frmMain.TeamList[t, mTeam[t]].WinAxis;
                            tempLoss = _frmMain.TeamList[t, mTeam[t]].LossAxis;
                            switch (_frmMain.TeamList[t, mTeam[t]].PlrCnt)
                            {
                                case 2:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[7, 2])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 3:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[7, 3])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 4:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[7, 4])); // R4.41 Bug fix.
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            // R4.35 Added TEAM Win/Loss.
                            tempRank = _frmMain.TeamList[t, mTeam[t]].RankAllies;
                            tempWin = _frmMain.TeamList[t, mTeam[t]].WinAllies;
                            tempLoss = _frmMain.TeamList[t, mTeam[t]].LossAllies;
                            switch (_frmMain.TeamList[t, mTeam[t]].PlrCnt)
                            {
                                case 2:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[6, 2])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 3:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[6, 3])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 4:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[6, 4])); // R4.41 Bug fix.
                                        break;
                                    }
                            }
                        }

                        _frmMain.PlrTeam[t] = mTeam[t];
                        _frmMain.PlrTWin[t] = (int)tempWin; // R4.35 Added TEAM Win/Loss.
                        _frmMain.PlrTLoss[t] = (int)tempLoss;
                        _frmMain.PlrRank[t] = tempRank + ".";
                        if ((tempMax < 1L) | (tempRank < 1L))
                        {
                            if (_frmMain.PlrRank[t] == "-1.") _frmMain.PlrRank[t] = "P."; // R4.46 Added.

                            _frmMain.PlrELO[t] = "---";
                            _frmMain.PlrLVL[t] = "---";
                        }
                        else
                        {
                            _frmMain.PlrELO[t] = (100d * (Conversion.Val(_frmMain.PlrRank[t]) / tempMax)).ToString("##.0") + "%";
                            _frmMain.PlrLVL[t] = "L-" + LOG_CalcLevel((int)Math.Round(Conversion.Val(_frmMain.PlrRank[t])),
                                (int)tempMax);
                        }
                    }

                // *********************************************************
                // R4.34 Loop thru TEAM 2 looking for possible teams.
                // *********************************************************
                for (var t = 2; t <= 8; t += 2)
                    for (int t2 = 1, loopTo1 = _frmMain.TeamListCnt[t]; t2 <= loopTo1; t2++)
                    {
                        cnt = 0;
                        if ("" != _frmMain.TeamList[t, t2].RID1)
                        {
                            if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                            if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                            if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                            if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID1) cnt += 1;
                        }

                        if ("" != _frmMain.TeamList[t, t2].RID2)
                        {
                            if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                            if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                            if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                            if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID2) cnt += 1;
                        }

                        if ("" != _frmMain.TeamList[t, t2].RID3)
                        {
                            if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                            if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                            if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                            if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID3) cnt += 1;
                        }

                        if ("" != _frmMain.TeamList[t, t2].RID4)
                        {
                            if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID4) cnt += 1;

                            if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID4) cnt += 1;

                            if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID4) cnt += 1;

                            if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID4) cnt += 1;
                        }

                        if (1 < cnt)
                            // R4.34 We found a team.
                            if ((mCnt[t] <= cnt) & (_frmMain.TeamList[t, t2].PlrCnt <= cnt))
                            {
                                mCnt[t] = cnt;
                                mTeam[t] = t2;
                            }
                    }

                // R4.34 Decide if the team is Axis(20,22,24) or Allies(21,23,25) by faction.
                for (var t = 2; t <= 8; t += 2)
                {
                    tempMax = 0L;
                    if (0 < mTeam[t])
                    {
                        if ((_frmMain.PlrFact[t] == "01") | (_frmMain.PlrFact[t] == "03")) // R4.34 OST or OKW.
                        {
                            tempRank = _frmMain.TeamList[t, mTeam[t]].RankAxis;
                            tempWin = _frmMain.TeamList[t, mTeam[t]].WinAxis;
                            tempLoss = _frmMain.TeamList[t, mTeam[t]].LossAxis;
                            switch (_frmMain.TeamList[t, mTeam[t]].PlrCnt)
                            {
                                case 2:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[7, 2])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 3:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[7, 3])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 4:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[7, 4])); // R4.41 Bug fix.
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            tempRank = _frmMain.TeamList[t, mTeam[t]].RankAllies;
                            tempWin = _frmMain.TeamList[t, mTeam[t]].WinAllies;
                            tempLoss = _frmMain.TeamList[t, mTeam[t]].LossAllies;
                            switch (_frmMain.TeamList[t, mTeam[t]].PlrCnt)
                            {
                                case 2:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[6, 2])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 3:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[6, 3])); // R4.41 Bug fix.
                                        break;
                                    }

                                case 4:
                                    {
                                        tempMax = (long)Math.Round(Conversion.Val(_frmMain.LVLS[6, 4])); // R4.41 Bug fix.
                                        break;
                                    }
                            }
                        }

                        _frmMain.PlrTeam[t] = mTeam[t];
                        _frmMain.PlrTWin[t] = (int)tempWin; // R4.35 Added TEAM Win/Loss.
                        _frmMain.PlrTLoss[t] = (int)tempLoss;
                        _frmMain.PlrRank[t] = tempRank + ".";
                        if ((tempMax < 1L) | (tempRank < 1L))
                        {
                            if (_frmMain.PlrRank[t] == "-1.") _frmMain.PlrRank[t] = "P."; // R4.46 Added.

                            _frmMain.PlrELO[t] = "---";
                            _frmMain.PlrLVL[t] = "---";
                        }
                        else
                        {
                            _frmMain.PlrELO[t] = (100d * (Conversion.Val(_frmMain.PlrRank[t]) / tempMax)).ToString("##.0") + "%";
                            _frmMain.PlrLVL[t] = "L-" + LOG_CalcLevel((int)Math.Round(Conversion.Val(_frmMain.PlrRank[t])),
                                (int)tempMax);
                        }
                    }
                }
            }
            catch (Exception)
            {
                _frmMain.LbError3.Text = "Team Check Err";
                _frmMain.LbError3.BackColor = Color.FromArgb(255, 255, 0, 0);
                _frmMain.LstLog2.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Team Check - ERR:" +
                                           Information.Err().Description);
            }
        }

        private void STATS_ReadAloud()
        {
            // R4.34 Added.
            var a = "";
            var goodGuys = "Good Guys";
            var badGuys = "Bad Guys";
            // Dim tts As New SpeechSynthesizer
            var tp = new string[10];
            if (_frmMain.FlagSpeechOk == false)
            {
                _frmMain.LbError4.Text = "Speech Error";
                return;
            }

            // R4.34 Remove some chars for clearer speech.
            for (var t = 1; t <= 8; t++)
            {
                a = _frmMain.PlrName[t];
                a = a.Replace(".", "");
                a = a.Replace(",", "");
                a = a.Replace("|", "");
                tp[t] = a;
            }

            if ((_frmMain.PlrFact[1] == "01") | (_frmMain.PlrFact[1] == "03"))
            {
                goodGuys = "Axis";
                badGuys = "Allies";
            }

            if ((_frmMain.PlrFact[1] == "02") | (_frmMain.PlrFact[1] == "04") | (_frmMain.PlrFact[1] == "05"))
            {
                goodGuys = "Allies";
                badGuys = "Axis";
            }

            a = a + "The " + goodGuys + " players are ";
            for (var t = 1; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(_frmMain.PlrName[t])) // R4.34 User may be "..." which will be "".
                {
                    a = a + "Player " + tp[t] + ",";
                    a = _frmMain.PlrRank[t] == "---"
                        ? a + "Faction Rank is None" + ","
                        : a + "Faction Rank is " + _frmMain.PlrRank[t] + ",";
                }

            a = a + "The " + badGuys + " players are ";
            for (var t = 2; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(_frmMain.PlrName[t])) // R4.34 User may be "..." which will be "".
                {
                    a = a + "Player " + tp[t] + ",";
                    a = _frmMain.PlrRank[t] == "---"
                        ? a + "Faction Rank is None" + ","
                        : a + "Faction Rank is " + _frmMain.PlrRank[t] + ",";
                }

            a = a + ",So we have " + goodGuys + " ";
            for (var t = 1; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(_frmMain.PlrName[t]))
                {
                    switch (_frmMain.PlrFact[t] ?? "")
                    {
                        case "01":
                            {
                                a += "O S T,";
                                break;
                            }

                        case "02":
                            {
                                a += "SOVIET,";
                                break;
                            }

                        case "03":
                            {
                                a += "O K W,";
                                break;
                            }

                        case "04":
                            {
                                a += "U S F,";
                                break;
                            }

                        case "05":
                            {
                                a += "BRIT,";
                                break;
                            }
                    }

                    a = _frmMain.PlrRank[t] == "---" ? a + "No rank" + "," : a + "Rank " + _frmMain.PlrRank[t] + ",";
                }

            a = a + ", versus " + badGuys + " ";
            for (var t = 2; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(_frmMain.PlrName[t]))
                    if (!string.IsNullOrEmpty(_frmMain.PlrName[t]))
                    {
                        switch (_frmMain.PlrFact[t] ?? "")
                        {
                            case "01":
                                {
                                    a += "O S T,";
                                    break;
                                }

                            case "02":
                                {
                                    a += "SOVIET,";
                                    break;
                                }

                            case "03":
                                {
                                    a += "O K W,";
                                    break;
                                }

                            case "04":
                                {
                                    a += "U S F,";
                                    break;
                                }

                            case "05":
                                {
                                    a += "BRIT,";
                                    break;
                                }
                        }

                        a = _frmMain.PlrRank[t] == "---" ? a + "No rank" + "," : a + "Rank " + _frmMain.PlrRank[t] + ",";
                    }

            if (!string.IsNullOrEmpty(a)) _frmMain.SpeechSynth1.SpeakAsync(a);
        }

        private void STATS_StoreLast()
        {
            int t;
            var hit = default(int);

            // R4.30 See if we have new valid data.
            for (t = 1; t <= 8; t++)
                if ("" != _frmMain.PlrRID[t])
                {
                    hit = t;
                    break;
                }

            // R3.000 We have new valid incoming data, so clear old data.
            if (0 < hit)
                for (t = 1; t <= 8; t++)
                {
                    _frmMain.PlrRankLast[t] = _frmMain.PlrRank_Buffer[t];
                    _frmMain.PlrNameLast[t] = _frmMain.PlrName_Buffer[t];
                    _frmMain.PlrSteamLast[t] = _frmMain.PlrSteam_Buffer[t];
                    _frmMain.PlrRidLast[t] = _frmMain.PlrRID_Buffer[t];
                    _frmMain.PlrFactLast[t] = _frmMain.PlrFact_Buffer[t];
                    _frmMain.PlrTeamLast[t] = _frmMain.PlrTeam_Buffer[t];
                    _frmMain.PlrTWinLast[t] = _frmMain.PlrTWin_Buffer[t];
                    _frmMain.PlrTLossLast[t] = _frmMain.PlrTLoss_Buffer[t];
                    _frmMain.PlrCountryLast[t] = _frmMain.PlrCountry_Buffer[t];
                    _frmMain.PlrCountryNameLast[t] = _frmMain.PlrCountryName_Buffer[t];
                    for (var t2 = 1; t2 <= 5; t2++)
                        for (var t3 = 1; t3 <= 4; t3++)
                        {
                            _frmMain.PlrRankAllLast[t, t2, t3] = _frmMain.PlrRankALL_Buffer[t, t2, t3];
                            _frmMain.PlrRankWinLast[t, t2, t3] = _frmMain.PlrRankWin_Buffer[t, t2, t3];
                            _frmMain.PlrRankLossLast[t, t2, t3] = _frmMain.PlrRankLoss_Buffer[t, t2, t3];
                            _frmMain.PlrRankPercLast[t, t2, t3] = _frmMain.PlrRankPerc_Buffer[t, t2, t3];
                        }

                    _frmMain.TeamListCntLast[t] = _frmMain.TeamListCnt_Buffer[t];
                    for (int t2 = 1, loopTo = _frmMain.TeamListLast.GetUpperBound(1); t2 <= loopTo; t2++) _frmMain.TeamListLast[t, t2] = _frmMain.TeamList_Buffer[t, t2];

                    _frmMain.PlrEloLast[t] = _frmMain.PlrELO_Buffer[t];
                    _frmMain.PlrLvlLast[t] = _frmMain.PlrLVL_Buffer[t];
                    _frmMain.PlrGlvlLast[t] = _frmMain.PlrGLVL_Buffer[t];
                }
        }

        private int LOG_CalcLevel(int plrRank, int tMax)
        {
            // R4.30 loop thru the possible levels.
            var tl = 1;
            for (var t = 1; t <= 15; t++)
                if (_frmMain.LvLpercs[t] * tMax > plrRank)
                    tl = t + 1;

            // R4.30 Levels above rank 200 are always the same.
            if (200 > plrRank) tl = 16;

            if (81 > plrRank) tl = 17;

            if (37 > plrRank) tl = 18;

            if (14 > plrRank) tl = 19;

            if (3 > plrRank) tl = 20;

            if (plrRank == 0) tl = 0;

            return tl;
        }
    }
}