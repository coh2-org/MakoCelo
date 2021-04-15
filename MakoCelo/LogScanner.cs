using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MakoCelo
{
    public class LogScanner
    {
        private readonly frmMain _frmMain;

        public LogScanner(frmMain frmMain1)
        {
            _frmMain = frmMain1;
        }

        public void LOG_Scan()
        {
            // R4.00 Read the RELIC log file and get the match stats.
            // R4.00 Stats come in two sections. Each has a Relic ID #.
            // R4.00 Match the two sections using the Relic ID #.
            // R4.50 Relic broke the file so now there is only one section.
            string A, B;
            bool FindMatch;
            bool FindPlayers;
            int PlrCnt;
            var MatchMode = default(int); // R4.30 1-1v1.2-2v2,etc
            int tLen;
            long test1;
            long test2;
            long tRank;
            var tPlrRank = new string[10]; // R3.10 Added Storage to place old vals on screen if no new onesa are found.
            var tPlrName = new string[10];
            var tPlrSteam = new string[10];
            var tPlrRID = new int[10];
            var tPlrFact = new string[10];
            var tPlrTWin = new int[10];
            var tPlrTLoss = new int[10];
            var tPlrTeam = new int[10];
            var tPlrCountry = new string[10];
            var tPlrCountryName = new string[10];
            var tempTL = new clsGlobal.t_TeamList();
            var tTeamList = new clsGlobal.t_TeamList[10, 1001];
            var tTeamListCnt = new int[10];
            var tPlrRankALL = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrRankWin = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrRankLoss = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrRankPerc = new string[9, 8, 5]; // R4.30 Rank from RID for all game modes.
            var tPlrELO = new string[10];
            var tPlrLVL = new string[10];
            var tPlrGLVL = new int[10];
            var MatchRID = new long[10];
            var GameRID = new long[10];
            int tCnt;

            // R3.40 Reset the on screen ERROR labels.
            _frmMain.lbError1.Text = "";
            _frmMain.lbError1.BackColor = Color.FromArgb(255, 192, 192, 192);
            _frmMain.lbError2.Text = "";
            _frmMain.lbError2.BackColor = Color.FromArgb(255, 192, 192, 192);

            // R1.00 If we dont have a valid log file path, exit with help notice.
            if (!File.Exists(_frmMain.PATH_Game))
            {
                if (string.IsNullOrEmpty(_frmMain.PATH_Game))
                    Interaction.MsgBox(
                        "Please locate the warnings.log file in your COH2 My Games directory." + Constants.vbCr +
                        Constants.vbCr + "Click on FIND LOG FILE to search and select.", MsgBoxStyle.Information);
                else
                    Interaction.MsgBox(
                        "ERROR: The LOG file location does not appear to be valid." + Constants.vbCr + Constants.vbCr +
                        "Unable to open the LOG file to get stats." + Constants.vbCr + "Verify this file/path exists." +
                        Constants.vbCr + Constants.vbCr + _frmMain.PATH_Game, MsgBoxStyle.Critical);

                return;
            }

            // R4.30 Delete the initial pretend setup player information.
            if (!_frmMain.FLAG_InitialScanning)
            {
                _frmMain.FLAG_InitialScanning = true;
                for (var t = 1; t <= 8; t++)
                {
                    _frmMain.PlrRank[t] = "---";
                    _frmMain.PlrName[t] = "";
                    _frmMain.PlrSteam[t] = "";
                    _frmMain.PlrFact[t] = "";
                    _frmMain.PlrTWin[t] = 0;
                    _frmMain.PlrTLoss[t] = 0;
                    _frmMain.PlrTeam[t] = 0;
                    _frmMain.PlrCountry[t] = "";
                    _frmMain.PlrCountryName[t] = "";
                    for (var T2 = 1; T2 <= 5; T2++)
                    for (var T3 = 1; T3 <= 4; T3++)
                    {
                        _frmMain.PlrRankALL[t, T2, T3] = 0;
                        _frmMain.PlrRankWin[t, T2, T3] = 0;
                        _frmMain.PlrRankLoss[t, T2, T3] = 0;
                        _frmMain.PlrRankPerc[t, T2, T3] = "";
                    }

                    _frmMain.TeamListCnt[t] = 0;
                    for (int T2 = 1, loopTo = _frmMain.TeamList.GetUpperBound(1); T2 <= loopTo; T2++) _frmMain.TeamList[t, T2] = tempTL;

                    _frmMain.PlrELO[t] = "";
                    _frmMain.PlrLVL[t] = "";
                    _frmMain.PlrGLVL[t] = 0;
                }
            }

            // R3.00 Clear the Last Valid Match stats if necessary.
            // Call STATS_StoreLast()

            // R3.10 Clear the current match and find new data below. 
            for (var t = 1; t <= 8; t++)
            {
                tPlrRank[t] = _frmMain.PlrRank[t];
                _frmMain.PlrRank[t] = "---";
                tPlrName[t] = _frmMain.PlrName[t];
                _frmMain.PlrName[t] = "";
                tPlrSteam[t] = _frmMain.PlrSteam[t];
                _frmMain.PlrSteam[t] = "";
                tPlrRID[t] = _frmMain.PlrRID[t];
                _frmMain.PlrRID[t] = 0;
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
                for (var T2 = 1; T2 <= 5; T2++)
                for (var T3 = 1; T3 <= 4; T3++)
                {
                    tPlrRankALL[t, T2, T3] = _frmMain.PlrRankALL[t, T2, T3];
                    _frmMain.PlrRankALL[t, T2, T3] = 0;
                    tPlrRankWin[t, T2, T3] = _frmMain.PlrRankWin[t, T2, T3];
                    _frmMain.PlrRankWin[t, T2, T3] = 0;
                    tPlrRankLoss[t, T2, T3] = _frmMain.PlrRankLoss[t, T2, T3];
                    _frmMain.PlrRankLoss[t, T2, T3] = 0;
                    tPlrRankPerc[t, T2, T3] = _frmMain.PlrRankPerc[t, T2, T3];
                    _frmMain.PlrRankPerc[t, T2, T3] = "";
                }

                tTeamListCnt[t] = _frmMain.TeamListCnt[t];
                _frmMain.TeamListCnt[t] = 0;
                for (int T2 = 1, loopTo1 = _frmMain.TeamList.GetUpperBound(1); T2 <= loopTo1; T2++)
                {
                    tTeamList[t, T2] = _frmMain.TeamList[t, T2];
                    _frmMain.TeamList[t, T2] = tempTL;
                }

                tPlrELO[t] = _frmMain.PlrELO[t];
                _frmMain.PlrELO[t] = "";
                tPlrLVL[t] = _frmMain.PlrLVL[t];
                _frmMain.PlrLVL[t] = "";
                tPlrGLVL[t] = _frmMain.PlrGLVL[t];
                _frmMain.PlrGLVL[t] = 0;

                // R4.30 Store PUBLIC copy for LAST MATCH ability.
                _frmMain.PlrRank_Buffer[t] = tPlrRank[t];
                _frmMain.PlrName_Buffer[t] = tPlrName[t];
                _frmMain.PlrSteam_Buffer[t] = tPlrSteam[t];
                _frmMain.PlrRID_Buffer[t] = tPlrRID[t];
                _frmMain.PlrFact_Buffer[t] = tPlrFact[t];
                _frmMain.PlrTeam_Buffer[t] = tPlrTeam[t];
                _frmMain.PlrTWin_Buffer[t] = tPlrTWin[t];
                _frmMain.PlrTLoss_Buffer[t] = tPlrTLoss[t];
                _frmMain.PlrCountry_Buffer[t] = tPlrCountry[t]; // R4.45 Added.
                _frmMain.PlrCountryName_Buffer[t] = tPlrCountryName[t]; // R4.45 Added.
                for (var T2 = 1; T2 <= 5; T2++)
                for (var T3 = 1; T3 <= 4; T3++)
                {
                    _frmMain.PlrRankALL_Buffer[t, T2, T3] = tPlrRankALL[t, T2, T3];
                    _frmMain.PlrRankWin_Buffer[t, T2, T3] = tPlrRankWin[t, T2, T3];
                    _frmMain.PlrRankLoss_Buffer[t, T2, T3] = tPlrRankLoss[t, T2, T3];
                    _frmMain.PlrRankPerc_Buffer[t, T2, T3] = tPlrRankPerc[t, T2, T3];
                }

                _frmMain.TeamListCnt_Buffer[t] = tTeamListCnt[t];
                for (int T2 = 1, loopTo2 = _frmMain.TeamList_Buffer.GetUpperBound(1); T2 <= loopTo2; T2++) _frmMain.TeamList_Buffer[t, T2] = tTeamList[t, T2];

                _frmMain.PlrELO_Buffer[t] = tPlrELO[t];
                _frmMain.PlrLVL_Buffer[t] = tPlrLVL[t];
                _frmMain.PlrGLVL_Buffer[t] = tPlrGLVL[t];
            }

            _frmMain.lbStatus.Text = "Open file...";
            Application.DoEvents();
            _frmMain.Cursor = Cursors.WaitCursor;

            // R2.01 OPEN log file and start parsing.
            var fs = new FileStream(_frmMain.PATH_Game, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(fs, Encoding.UTF8);
            using (sr)
            {
                PlrCnt = 0;

                // R2.01 Loop thru the file looking for the match stats.
                while (!sr.EndOfStream)
                {
                    A = sr.ReadLine();

                    // **********************************************************************************
                    // R3.20 If Match Started string is found, find all of the other match stats lines.
                    // **********************************************************************************
                    if (Conversions.ToBoolean(Strings.InStr(A, "Match Started")))
                    {
                        FindMatch = true;

                        // R3.20 We have found a new section so clear the previous data.
                        for (var t = 1; t <= 8; t++)
                        {
                            _frmMain.PlrRank[t] = "---";
                            _frmMain.PlrSteam[t] = "";
                            MatchRID[t] = 0L;
                        }

                        PlrCnt = 0;
                        while (!sr.EndOfStream & FindMatch)
                        {
                            PlrCnt += 1;
                            B = Strings.Trim(Strings.Mid(A, 98, 20));
                            if ((B == "-1") | string.IsNullOrEmpty(B)) // R2.01 Added +1 to rank code. 
                            {
                                B = ""; // R1.00 Show unranked as --       
                            }
                            else
                            {
                                tRank = (long) Math.Round(Conversion.Val(B));
                                B = (tRank + 1L).ToString();
                            }

                            _frmMain.PlrRank[PlrCnt] = B;

                            // R3.20 Get SteamID. If valid, get RelicID also.
                            _frmMain.PlrSteam[PlrCnt] = Strings.Mid(A, 57, 17);
                            if (Strings.Mid(_frmMain.PlrSteam[PlrCnt], 1, 4) != "7656")
                            {
                                _frmMain.PlrSteam[PlrCnt] = "";
                                MatchRID[PlrCnt] = 0L;
                            }
                            else
                            {
                                MatchRID[PlrCnt] = _frmMain.LOG_HexToLong(Strings.Mid(A, 41,
                                    8)); // R3.20 <-- Convert.ToInt64(Mid(A, 41, 8), 16)
                                if (MatchRID[PlrCnt] == -1) MatchRID[PlrCnt] = 0L;
                            }

                            // R3.20 Read the next line of the file and exit if all of the match lines have been found.
                            A = sr.ReadLine();
                            if (Strings.InStr(A, "Match Started") == 0) FindMatch = false;
                        }
                    }


                    // **********************************************************************************
                    // 3.20 If we find a GAME Human Player string, find all of the other player stats.
                    // **********************************************************************************
                    if (Conversions.ToBoolean(Strings.InStr(A, "GAME ")))
                        if (Conversions.ToBoolean(Strings.InStr(A, "Human Player") | Strings.InStr(A, "AI Player")))
                        {
                            // R3.20 We have found a new section so clear the previous data.
                            for (var t = 1; t <= 8; t++)
                            {
                                _frmMain.PlrName[t] = "";
                                _frmMain.PlrFact[t] = "";
                                _frmMain.PlrRID[t] = 0;
                                _frmMain.PlrTeam[t] = 0;
                                _frmMain.PlrTWin[t] = 0;
                                _frmMain.PlrTLoss[t] = 0;
                                _frmMain.PlrCountry[t] = ""; // R4.45 Added.
                                _frmMain.PlrCountryName[t] = ""; // R4.46 Added.
                                for (var T2 = 1; T2 <= 5; T2++)
                                for (var T3 = 1; T3 <= 4; T3++)
                                {
                                    _frmMain.PlrRankALL[t, T2, T3] = 0;
                                    _frmMain.PlrRankWin[t, T2, T3] = 0;
                                    _frmMain.PlrRankLoss[t, T2, T3] = 0;
                                    _frmMain.PlrRankPerc[t, T2, T3] = "";
                                }

                                _frmMain.TeamListCnt[t] = 0;
                                for (int T2 = 1, loopTo3 = _frmMain.TeamList.GetUpperBound(1); T2 <= loopTo3; T2++) _frmMain.TeamList[t, T2] = tempTL;

                                GameRID[t] = 0L;
                            }

                            PlrCnt = 0;
                            FindPlayers = true;
                            while (!sr.EndOfStream & FindPlayers)
                            {
                                PlrCnt += 1;
                                test1 = Strings.InStr(A, "Human Player");
                                if (Conversions.ToBoolean(test1))
                                {
                                    _frmMain.PlrName[PlrCnt] = _frmMain.LOG_FindPlayer(A,
                                        39); // R2.01 Names are not Delimited, need to search for end of name from the end of line.
                                    GameRID[PlrCnt] = _frmMain.LOG_Find_RelicID(A); // R3.20 Get the RelicID for this player.  
                                    _frmMain.PlrRID[PlrCnt] = (int) GameRID[PlrCnt]; // R4.30 Added.
                                }
                                else
                                {
                                    _frmMain.PlrName[PlrCnt] = _frmMain.LOG_FindPlayer(A, 36); // R2.01 AI player. 
                                    // PlrRank(PlrCnt) = ""                     'R4.30 Added for match tracking.
                                    GameRID[PlrCnt] = 0L; // R3.20 AI has no RelicID.
                                    _frmMain.PlrRID[PlrCnt] = 0;
                                } // R4.30 Added.

                                // R3.40 The last part of the string will have faction.
                                tLen = Strings.Len(A); // R3.40 Get the lenght of the string.
                                if (20 < tLen) // R3.40 This should never happen, but just in case.
                                {
                                    if (Strings.Mid(A, tLen - 5, 6) == "german") _frmMain.PlrFact[PlrCnt] = "01";

                                    if (Strings.Mid(A, tLen - 5, 6) == "soviet") _frmMain.PlrFact[PlrCnt] = "02";

                                    if (Strings.Mid(A, tLen - 10, 11) == "west_german") _frmMain.PlrFact[PlrCnt] = "03";

                                    if (Strings.Mid(A, tLen - 2, 3) == "aef") _frmMain.PlrFact[PlrCnt] = "04";

                                    if (Strings.Mid(A, tLen - 6, 7) == "british") _frmMain.PlrFact[PlrCnt] = "05";
                                }

                                // R3.20 Read the next line of the file.
                                A = sr.ReadLine();
                                test1 = Strings.InStr(A, "Human Player");
                                test2 = Strings.InStr(A, "AI Player");
                                if ((test1 == 0L) & (test2 == 0L)) FindPlayers = false;
                            }
                        }
                }
            }


            // R2.01 Close / Clean up  streams?
            sr.Close(); // R4.00 Should not need, is inside USING.
            sr.Dispose();
            fs.Close();
            fs.Dispose();


            // R4.30 Play the MATCH FOUND ALERT if this a NEW match and are we in AUTO mode.
            var F_NewData = false;
            for (var t = 1; t <= 8; t++)
            {
                if ((_frmMain.PlrName[t] ?? "") != (tPlrName[t] ?? ""))
                {
                    F_NewData = true;
                    break;
                }

                if (_frmMain.PlrRID[t] != tPlrRID[t])
                {
                    F_NewData = true;
                    break;
                }

                if ((_frmMain.PlrFact[t] ?? "") != (tPlrFact[t] ?? ""))
                {
                    F_NewData = true;
                    break;
                }
                // If PlrSteam(t) <> tPlrSteam(t) Then F_NewData = True : Exit For  'R4.30 Not in file anymore.
                // If PlrRank(t) <> tPlrRank(t) Then F_NewData = True : Exit For    'R4.30 Not in file anymore.
            }

            // R4.30 Clear the Last Valid Match stats if necessary.
            if (F_NewData)
            {
                // R4.30 Reset the ELO cycle mode.
                _frmMain.FLAG_EloMode = 0;
                _frmMain.STATS_StoreLast();
                if (_frmMain.SCAN_Enabled && _frmMain.chkFoundSound.Checked && !string.IsNullOrEmpty(_frmMain.SOUND_File[15]) && PlrCnt > 0)
                {
                    _frmMain.AUDIO_SetVolume(100, Conversions.ToInteger(_frmMain.SOUND_Vol[15]));
                    _frmMain.SOUND_Play(_frmMain.SOUND_File[15]);
                }

                // R4.50 Show time on status bar.
                _frmMain.SS1_Time.Text = "Match found: " + DateTime.Now.ToString("HH:mm"); // TimeString.ToString("HH:mm")
            }

            // R3.10 If no new data was found, show the old data.
            if (F_NewData == false)
                for (var t = 1; t <= 8; t++)
                {
                    _frmMain.PlrRank[t] = tPlrRank[t];
                    _frmMain.PlrName[t] = tPlrName[t];
                    _frmMain.PlrSteam[t] = tPlrSteam[t];
                    _frmMain.PlrRID[t] = tPlrRID[t];
                    _frmMain.PlrFact[t] = tPlrFact[t];
                    _frmMain.PlrTeam[t] = tPlrTeam[t];
                    _frmMain.PlrTWin[t] = tPlrTWin[t];
                    _frmMain.PlrTLoss[t] = tPlrTLoss[t];
                    _frmMain.PlrCountry[t] = tPlrCountry[t]; // R4.45 Added.
                    _frmMain.PlrCountryName[t] = tPlrCountryName[t]; // R4.46 Added.
                    for (var T2 = 1; T2 <= 5; T2++)
                    for (var T3 = 1; T3 <= 4; T3++)
                    {
                        _frmMain.PlrRankALL[t, T2, T3] = tPlrRankALL[t, T2, T3];
                        _frmMain.PlrRankWin[t, T2, T3] = tPlrRankWin[t, T2, T3];
                        _frmMain.PlrRankLoss[t, T2, T3] = tPlrRankLoss[t, T2, T3];
                        _frmMain.PlrRankPerc[t, T2, T3] = tPlrRankPerc[t, T2, T3];
                    }

                    _frmMain.TeamListCnt[t] = tTeamListCnt[t];
                    for (int T2 = 1, loopTo4 = _frmMain.TeamList.GetUpperBound(1); T2 <= loopTo4; T2++) _frmMain.TeamList[t, T2] = tTeamList[t, T2];

                    _frmMain.PlrELO[t] = tPlrELO[t];
                    _frmMain.PlrLVL[t] = tPlrLVL[t];
                }

            _frmMain.lbStatus.Text = "Render...";
            Application.DoEvents();
            if (0 < PlrCnt) MatchMode = 1;

            if (2 < PlrCnt) MatchMode = 2;

            if (4 < PlrCnt) MatchMode = 3;

            if (6 < PlrCnt) MatchMode = 4;

            // R4.30 Adjust NAME and RANKS before drawing.
            if ((0 < PlrCnt) & (F_NewData | !_frmMain.SCAN_Enabled))
            {
                _frmMain.lstLog.Items.Clear(); // R4.42 Clear error log.

                // R4.30 Get player ranks from the RELIC API.
                for (var t = 1; t <= 8; t++)
                    if (!string.IsNullOrEmpty(_frmMain.PlrName[t]) & !string.IsNullOrEmpty(_frmMain.PlrFact[t]) &
                        (0d < Conversion.Val(GameRID[t])))
                    {
                        _frmMain.lbStatus.Text = "Web Player: " + t;
                        Application.DoEvents();
                        _frmMain.STAT_GetFromRID(GameRID[t].ToString(), t);
                        _frmMain.lbStatus.Text = "";
                        Application.DoEvents();
                        _frmMain.PlrRank[t] = _frmMain.PlrRankALL[t, Conversions.ToInteger(_frmMain.PlrFact[t]), MatchMode].ToString();
                        if (_frmMain.PlrRank[t] == "0") _frmMain.PlrRank[t] = "---";

                        _frmMain.PlrWin[t] = _frmMain.PlrRankWin[t, Conversions.ToInteger(_frmMain.PlrFact[t]), MatchMode];
                        _frmMain.PlrLoss[t] = _frmMain.PlrRankLoss[t, Conversions.ToInteger(_frmMain.PlrFact[t]), MatchMode];
                    }

                _frmMain.lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - Complete.");
                for (var t = 1; t <= 8; t++)
                {
                    // R4.30 See if any players are a premade team.
                    tCnt = 0;
                    if (t % 2 == 1)
                    {
                        for (var t2 = 1; t2 <= 7; t2 += 2)
                            if ((_frmMain.PlrRank[t] ?? "") == (_frmMain.PlrRank[t2] ?? ""))
                                tCnt += 1;
                    }
                    else
                    {
                        for (var t2 = 2; t2 <= 8; t2 += 2)
                            if ((_frmMain.PlrRank[t] ?? "") == (_frmMain.PlrRank[t2] ?? ""))
                                tCnt += 1;
                    }

                    // R4.30 Update the MAX player counts if on a premade team.
                    _frmMain.PlrGLVL[t] =
                        (int) Math.Round(Conversion.Val(_frmMain.LVLS[(int) Math.Round(Conversion.Val(_frmMain.PlrFact[t])), MatchMode]));
                    if (tCnt == 2) // R4.30 AT 2v2
                    {
                        if ((_frmMain.PlrFact[t] == "01") | (_frmMain.PlrFact[t] == "03"))
                            _frmMain.PlrGLVL[t] = (int) Math.Round(Conversion.Val(_frmMain.LVLS[7, 2]));
                        else
                            _frmMain.PlrGLVL[t] = (int) Math.Round(Conversion.Val(_frmMain.LVLS[6, 2]));
                    }

                    if (tCnt == 3) // R4.30 AT 3v3
                    {
                        if ((_frmMain.PlrFact[t] == "01") | (_frmMain.PlrFact[t] == "03"))
                            _frmMain.PlrGLVL[t] = (int) Math.Round(Conversion.Val(_frmMain.LVLS[7, 3]));
                        else
                            _frmMain.PlrGLVL[t] = (int) Math.Round(Conversion.Val(_frmMain.LVLS[6, 3]));
                    }

                    if (tCnt == 4) // R4.30 AT 4v4
                    {
                        if ((_frmMain.PlrFact[t] == "01") | (_frmMain.PlrFact[t] == "03"))
                            _frmMain.PlrGLVL[t] = (int) Math.Round(Conversion.Val(_frmMain.LVLS[7, 4]));
                        else
                            _frmMain.PlrGLVL[t] = (int) Math.Round(Conversion.Val(_frmMain.LVLS[6, 4]));
                    }

                    // R4.30 Calc ELO % and LEVEL values.
                    if (string.IsNullOrEmpty(_frmMain.PlrRank[t])) _frmMain.PlrRank[t] = "---";

                    if (string.IsNullOrEmpty(_frmMain.PlrName[t]))
                    {
                        _frmMain.PlrRank[t] = "";
                        _frmMain.PlrELO[t] = "";
                        _frmMain.PlrLVL[t] = "";
                    }
                    // R4.30 We have a valid player so calc ELO % and approximate LEVEL value.
                    else if (0d < Conversion.Val(_frmMain.LVLS[Conversions.ToInteger(_frmMain.PlrFact[t]), MatchMode]))
                    {
                        if (_frmMain.PlrRank[t] == "---")
                        {
                            _frmMain.PlrELO[t] = "---";
                            _frmMain.PlrLVL[t] = "---";
                        }
                        else
                        {
                            _frmMain.PlrELO[t] = (100d * (Conversion.Val(_frmMain.PlrRank[t]) / _frmMain.PlrGLVL[t])).ToString("##.0") + "%";
                            _frmMain.PlrLVL[t] = "L-" + _frmMain.LOG_CalcLevel((int) Math.Round(Conversion.Val(_frmMain.PlrRank[t])), _frmMain.PlrGLVL[t]);
                        }
                    }
                    else
                    {
                        _frmMain.PlrELO[t] = "";
                        _frmMain.PlrLVL[t] = "";
                    }
                }
            }

            // R4.34 Dont write over the ELO cycle unless we have new data.
            if (F_NewData | !_frmMain.SCAN_Enabled)
            {
                // R4.34 See if we should search the net for team ranks.
                if (_frmMain.chkGetTeams.Checked)
                {
                    // R4.34 Set ranks if players are a team.
                    _frmMain.STAT_GetCheckForTeam();
                    _frmMain.lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - TEAM Check - Completed.");
                }

                // R4.34 Text-to-Speech the ranks.
                if (_frmMain.chkSpeech.Checked) _frmMain.STATS_ReadAloud();

                // R4.50 Force the STATS image redraw.
                _frmMain.MainBuffer_Valid = false;

                // R4.34 Draw the updated ranks.
                _frmMain.GFX_DrawStats();
            }

            // R5.00 Overlay
            if (_frmMain.SCAN_Enabled && F_NewData && _frmMain._chkToggleOverlay.Checked) _frmMain._overlay.Run(_frmMain.PlrName, _frmMain.PlrRank);
            // R4.30 Clean up our GUI user indicators.
            _frmMain.Cursor = Cursors.Default;
            _frmMain.lbStatus.Text = "Ready";
            Application.DoEvents();
        }
    }
}