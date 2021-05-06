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

        public Match StartScanningLogFile(Match previousMatch)
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
            var tPlrGlvl = new int[10];


            // This will be replaced with object returned to form so there will be no need to buffer previous data at this point (it will be done after search)
            #region ToRemoveAfterMigration
            // R3.10 Clear the current match and find new data below. 
            for (var t = 1; t <= 8; t++)
            {
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
                
                tPlrGlvl[t] = _frmMain.PlrGLVL[t];
                _frmMain.PlrGLVL[t] = 0;

                // R4.30 Store PUBLIC copy for LAST MATCH ability.
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
                
                _frmMain.PlrGLVL_Buffer[t] = tPlrGlvl[t];
            }

            #endregion
            var matchFound = _logFileParser.ParsePlayersFromGameLog(tempTl);
            #region ToRemoveAfterMigration
            if (!matchFound.IsMatchFound()) //backward compatibility - won't be needed if we use previousMatch
                for (var t = 1; t <= 8; t++)
                {
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
                    
                }

            #endregion ToRemoveAfterMigration

            if (matchFound.IsMatchFound() && (previousMatch == null || matchFound.Id != previousMatch.Id))
            {
                previousMatch = matchFound;

                // R4.30 Reset the ELO cycle mode.
                _frmMain.RankDisplayMode = 0;

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
                                var t = Array.FindIndex(_frmMain.PlrRID, x => x == matchFoundAlliesPlayer.RelicId);
                                //_frmMain.PlrTeam[t] = mTeam[t];
                                var teamStats = team.TeamStats.FirstOrDefault(x => x.Side == Side.Allies);
                                _frmMain.PlrTWin[t] = teamStats?.Wins ?? 0; //backward compatibility
                                _frmMain.PlrTLoss[t] = teamStats?.Losses ?? 0; //backward compatibility
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
                                var t = Array.FindIndex(_frmMain.PlrRID, x => x == matchFoundAlliesPlayer.RelicId);
                                //_frmMain.PlrTeam[t] = mTeam[t];
                                var teamStats = team.TeamStats.FirstOrDefault(x => x.Side == Side.Axis);
                                _frmMain.PlrTWin[t] = teamStats?.Wins ?? 0; //backward compatibility
                                _frmMain.PlrTLoss[t] = teamStats?.Losses ?? 0; //backward compatibility
                            }

                        }

                    }

                    _frmMain.lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - TEAM Check - Completed.");
                }

                // R4.34 Text-to-Speech the ranks.
                if (_frmMain.chkSpeech.Checked) throw new NotImplementedException(); //STATS_ReadAloud();

                // R4.50 Force the STATS image redraw.
                _frmMain.MainBuffer_Valid = false;

                // R4.34 Draw the updated ranks.
                _frmMain.Gfx.GFX_DrawStats();
            }

            // R5.00 Overlay
            if (_frmMain.AutoScanEnabled && matchFound.IsMatchFound() && _frmMain._chkToggleOverlay.Checked) _frmMain._overlay.Run(matchFound);
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
                                    TotalPlayers = leaderBoardPlayerData.RankTotal
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
                                            TotalTeams = leaderBoard.RankTotal
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
                    

                    

                    _frmMain.PlrWin[t] = _frmMain.PlrRankWin[t, Conversions.ToInteger(_frmMain.PlrFact[t]), (int)matchFound.GameMode]; //backward compatibility
                    _frmMain.PlrLoss[t] = _frmMain.PlrRankLoss[t, Conversions.ToInteger(_frmMain.PlrFact[t]), (int)matchFound.GameMode]; //backward compatibility

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
        
        ////private void STATS_ReadAloud()
        //{
        //    // R4.34 Added.
        //    var a = "";
        //    var goodGuys = "Good Guys";
        //    var badGuys = "Bad Guys";
        //    // Dim tts As New SpeechSynthesizer
        //    var tp = new string[10];
        //    if (_frmMain.FlagSpeechOk == false)
        //    {
        //        _frmMain.LbError4.Text = "Speech Error";
        //        return;
        //    }

        //    // R4.34 Remove some chars for clearer speech.
        //    for (var t = 1; t <= 8; t++)
        //    {
        //        a = _frmMain.PlrName[t];
        //        a = a.Replace(".", "");
        //        a = a.Replace(",", "");
        //        a = a.Replace("|", "");
        //        tp[t] = a;
        //    }

        //    if ((_frmMain.PlrFact[1] == "01") | (_frmMain.PlrFact[1] == "03"))
        //    {
        //        goodGuys = "Axis";
        //        badGuys = "Allies";
        //    }

        //    if ((_frmMain.PlrFact[1] == "02") | (_frmMain.PlrFact[1] == "04") | (_frmMain.PlrFact[1] == "05"))
        //    {
        //        goodGuys = "Allies";
        //        badGuys = "Axis";
        //    }

        //    a = a + "The " + goodGuys + " players are ";
        //    for (var t = 1; t <= 8; t += 2)
        //        if (!string.IsNullOrEmpty(_frmMain.PlrName[t])) // R4.34 User may be "..." which will be "".
        //        {
        //            a = a + "Player " + tp[t] + ",";
        //            a = _frmMain.PlrRank[t] == "---"
        //                ? a + "Faction Rank is None" + ","
        //                : a + "Faction Rank is " + _frmMain.PlrRank[t] + ",";
        //        }

        //    a = a + "The " + badGuys + " players are ";
        //    for (var t = 2; t <= 8; t += 2)
        //        if (!string.IsNullOrEmpty(_frmMain.PlrName[t])) // R4.34 User may be "..." which will be "".
        //        {
        //            a = a + "Player " + tp[t] + ",";
        //            a = _frmMain.PlrRank[t] == "---"
        //                ? a + "Faction Rank is None" + ","
        //                : a + "Faction Rank is " + _frmMain.PlrRank[t] + ",";
        //        }

        //    a = a + ",So we have " + goodGuys + " ";
        //    for (var t = 1; t <= 8; t += 2)
        //        if (!string.IsNullOrEmpty(_frmMain.PlrName[t]))
        //        {
        //            switch (_frmMain.PlrFact[t] ?? "")
        //            {
        //                case "01":
        //                    {
        //                        a += "O S T,";
        //                        break;
        //                    }

        //                case "02":
        //                    {
        //                        a += "SOVIET,";
        //                        break;
        //                    }

        //                case "03":
        //                    {
        //                        a += "O K W,";
        //                        break;
        //                    }

        //                case "04":
        //                    {
        //                        a += "U S F,";
        //                        break;
        //                    }

        //                case "05":
        //                    {
        //                        a += "BRIT,";
        //                        break;
        //                    }
        //            }

        //            a = _frmMain.PlrRank[t] == "---" ? a + "No rank" + "," : a + "Rank " + _frmMain.PlrRank[t] + ",";
        //        }

        //    a = a + ", versus " + badGuys + " ";
        //    for (var t = 2; t <= 8; t += 2)
        //        if (!string.IsNullOrEmpty(_frmMain.PlrName[t]))
        //            if (!string.IsNullOrEmpty(_frmMain.PlrName[t]))
        //            {
        //                switch (_frmMain.PlrFact[t] ?? "")
        //                {
        //                    case "01":
        //                        {
        //                            a += "O S T,";
        //                            break;
        //                        }

        //                    case "02":
        //                        {
        //                            a += "SOVIET,";
        //                            break;
        //                        }

        //                    case "03":
        //                        {
        //                            a += "O K W,";
        //                            break;
        //                        }

        //                    case "04":
        //                        {
        //                            a += "U S F,";
        //                            break;
        //                        }

        //                    case "05":
        //                        {
        //                            a += "BRIT,";
        //                            break;
        //                        }
        //                }

        //                a = _frmMain.PlrRank[t] == "---" ? a + "No rank" + "," : a + "Rank " + _frmMain.PlrRank[t] + ",";
        //            }

        //    if (!string.IsNullOrEmpty(a)) _frmMain.SpeechSynth1.SpeakAsync(a);
        //}
        
    }
}