using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MakoCelo.Model;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MakoCelo.Scanner
{
    public class LogFileParser : ILogFileParser
    {
        private long _lastMatchGameLogPosition;
        private string _startLineWithGameStartTime;


        public Model.Match ParseGameLog(string filePath)
        {
            Model.Match match = null;
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
                        match = new Model.Match();


                        var plrCnt = 0;
                        var findPlayers = true;
                        while (!sr.EndOfStream & findPlayers)
                        {
                            plrCnt += 1;
                            var newPlayer = new Player();
                            long test1 = Strings.InStr(a, "Human Player");
                            if (Conversions.ToBoolean(test1))
                            {
                                newPlayer.Name = Utilities.FindPlayerNameInLine(a, 39);
                                newPlayer.RelicId = Utilities.FindPlayerRelicIdInLine(a);
                            }
                            else
                            {
                                newPlayer.Name = Utilities.FindPlayerNameInLine(a, 36);
                                newPlayer.IsAIPlayer = true;
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


    }
}