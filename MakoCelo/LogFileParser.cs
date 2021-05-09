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
        private long _lastMatchGameLogPosition;
        private string _startLineWithGameStartTime;

        public LogFileParser(frmMain frmMain)
        {
        }

        public Match ParsePlayersFromGameLog(string filePath)
        {
            Match match = new Match();
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                            }
                            else
                            {
                                newPlayer.Name = LOG_FindPlayer(a, 36);
                                newPlayer.RelicId = ""; 
                            }
                            
                            var tLen = Strings.Len(a);
                            if (20 < tLen) // R3.40 This should never happen, but just in case.
                            {
                                if (Strings.Mid(a, tLen - 5, 6) == "german")
                                {
                                    newPlayer.CurrentFaction = Faction.Ost;
                                }

                                if (Strings.Mid(a, tLen - 5, 6) == "soviet")
                                {
                                    newPlayer.CurrentFaction = Faction.Sov;
                                }

                                if (Strings.Mid(a, tLen - 10, 11) == "west_german")
                                {
                                    newPlayer.CurrentFaction = Faction.Okw;
                                }

                                if (Strings.Mid(a, tLen - 2, 3) == "aef")
                                {
                                    newPlayer.CurrentFaction = Faction.Usf;
                                }

                                if (Strings.Mid(a, tLen - 6, 7) == "british")
                                {
                                    newPlayer.CurrentFaction = Faction.Ukf;
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