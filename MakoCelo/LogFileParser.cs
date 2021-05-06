using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MakoCelo.Model;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Match = MakoCelo.Model.Match;

namespace MakoCelo
{
    public class LogFileParser
    {
        private readonly frmMain _frmMain;
        private long _lastMatchGameLogPosition;
        private string _startLineWithGameStartTime;

        public LogFileParser(frmMain frmMain)
        {
            _frmMain = frmMain;
        }

        public Match ParsePlayersFromGameLog(clsGlobal.t_TeamList tempTl)
        {
            Match match = new Match();
            using var fs = new FileStream(_frmMain.PATH_Game, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.UTF8);
            SetStreamPosition(fs, sr);
            while (!sr.EndOfStream)
            {
                var a = sr.ReadLine();
                // **********************************************************************************
                // 3.20 If we find a GAME Human Player string, find all of the other player stats.
                // **********************************************************************************
                if (Conversions.ToBoolean(Strings.InStr(a, "GAME ")))
                    if (Conversions.ToBoolean(Strings.InStr(a, "Human Player") | Strings.InStr(a, "AI Player")))
                    {
                        match = new Match();

                        #region ToRemoveAfterMigration
                        
                        // R3.20 We have found a new section so clear the previous data.
                        for (var t = 1; t <= 8; t++)
                        {
                            _frmMain.PlrFact[t] = "";
                            _frmMain.PlrRID[t] = "";
                            _frmMain.PlrTeam[t] = 0;
                            _frmMain.PlrTWin[t] = 0;
                            _frmMain.PlrTLoss[t] = 0;
                            _frmMain.PlrCountry[t] = ""; // R4.45 Added.
                            _frmMain.PlrCountryName[t] = ""; // R4.46 Added.
                            for (var t2 = 1; t2 <= 5; t2++)
                            for (var t3 = 1; t3 <= 4; t3++)
                            {
                                _frmMain.PlrRankALL[t, t2, t3] = 0;
                                _frmMain.PlrRankWin[t, t2, t3] = 0;
                                _frmMain.PlrRankLoss[t, t2, t3] = 0;
                                _frmMain.PlrRankPerc[t, t2, t3] = "";
                            }

                            _frmMain.TeamListCnt[t] = 0;
                            for (int t2 = 1, loopTo3 = _frmMain.TeamList.GetUpperBound(1); t2 <= loopTo3; t2++) _frmMain.TeamList[t, t2] = tempTl;
                            
                        }

                        #endregion

                        var plrCnt = 0;
                        var findPlayers = true;
                        while (!sr.EndOfStream & findPlayers)
                        {
                            plrCnt += 1;
                            var newPlayer = new Player();
                            long test1 = Strings.InStr(a, "Human Player");
                            if (Conversions.ToBoolean(test1))
                            {
                                newPlayer.Name = LOG_FindPlayer(a, 39);
                                newPlayer.RelicId = LOG_Find_RelicID(a);
                                _frmMain.PlrRID[plrCnt] = newPlayer.RelicId; //backward compatibility
                            }
                            else
                            {
                                newPlayer.Name = LOG_FindPlayer(a, 36);
                                newPlayer.RelicId = ""; 
                                _frmMain.PlrRID[plrCnt] = ""; //backward compatibility
                            }
                            
                            var tLen = Strings.Len(a);
                            if (20 < tLen) // R3.40 This should never happen, but just in case.
                            {
                                if (Strings.Mid(a, tLen - 5, 6) == "german")
                                {
                                    newPlayer.CurrentFaction = Faction.Ost;
                                    _frmMain.PlrFact[plrCnt] = "01"; //backward compatibility
                                }

                                if (Strings.Mid(a, tLen - 5, 6) == "soviet")
                                {
                                    newPlayer.CurrentFaction = Faction.Sov;
                                    _frmMain.PlrFact[plrCnt] = "02"; //backward compatibility
                                }

                                if (Strings.Mid(a, tLen - 10, 11) == "west_german")
                                {
                                    newPlayer.CurrentFaction = Faction.Okw;
                                    _frmMain.PlrFact[plrCnt] = "03"; //backward compatibility
                                }

                                if (Strings.Mid(a, tLen - 2, 3) == "aef")
                                {
                                    newPlayer.CurrentFaction = Faction.Usf;
                                    _frmMain.PlrFact[plrCnt] = "04"; //backward compatibility
                                }

                                if (Strings.Mid(a, tLen - 6, 7) == "british")
                                {
                                    newPlayer.CurrentFaction = Faction.Ukf;
                                    _frmMain.PlrFact[plrCnt] = "05"; //backward compatibility
                                }
                            }
                            match.Players.Add(newPlayer);
                            a = sr.ReadLine();
                            if (a != null && a.Contains("Party::SetStatus - S_PLAYING")) // we already know that the game has started but we need gameID to successfully detect duplicate reads
                            {
                                a = sr.ReadLine();
                            }
                            if (a != null && a.Contains("GameObj::StartGameObj"))
                            {
                                match.Id = Regex.Match(a, "\\[(.*?)\\]").Value;
                                _lastMatchGameLogPosition = fs.Position;
                                findPlayers = false;
                            }
                        }
                    }
            }
            return match;
        }

        private void SetStreamPosition(FileStream fs, StreamReader sr)
        {
            var startLine = sr.ReadLine();
            if (_startLineWithGameStartTime != startLine) //Game Restart
            {
                _startLineWithGameStartTime = startLine;
                _lastMatchGameLogPosition = 0;
            }
            else //Set position to last buffer read
            {
                fs.Position = _lastMatchGameLogPosition;
                sr.DiscardBufferedData();
            }
        }

        private string LOG_Find_RelicID(string a)
        {
            var rid = 0L;
            int T;
            var cnt = default(int);
            var charStart = default(int);
            var charEnd = default(int);

            // R3.20 Get the RelicID number from the Player stats line.
            for (T = Strings.Len(a); T >= 1; T -= 1)
            {
                var c = Strings.Mid(a, T, 1);
                if (c == " ")
                {
                    cnt += 1;
                    if (cnt == 2) charEnd = T;

                    if (cnt == 3)
                    {
                        charStart = T;
                        break;
                    }
                }
            }

            if (Conversions.ToBoolean(charEnd))
                rid = (long) Math.Round(Conversion.Val(Strings.Mid(a, charStart, charEnd - charStart)));

            return rid.ToString();
        }

        /// <summary>
        /// Names are not Delimited, need to search for end of name from the end of line.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="charStart"></param>
        /// <returns></returns>
        private string LOG_FindPlayer(string a, int charStart)
        {
            string c;
            int T;
            var cnt = default(int);
            var charEnd = default(int);
            for (T = Strings.Len(a); T >= 1; T -= 1)
            {
                c = Strings.Mid(a, T, 1);
                if (c == " ") cnt += 1;

                if (cnt == 3)
                {
                    charEnd = T;
                    break;
                }
            }

            c = "None";
            if (Conversions.ToBoolean(charEnd)) c = Strings.Mid(a, charStart, charEnd - charStart);

            return c;
        }
    }
}