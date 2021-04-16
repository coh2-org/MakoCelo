using System;
using System.Drawing;
using System.IO;
using System.Net;
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
            int plrCnt;
            var matchMode = default(int); // R4.30 1-1v1.2-2v2,etc
            var tPlrRank = new string[10]; // R3.10 Added Storage to place old vals on screen if no new onesa are found.
            var tPlrName = new string[10];
            var tPlrSteam = new string[10];
            var tPlrRid = new int[10];
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
            var matchRid = new long[10];
            var gameRid = new long[10];

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
                    for (var t2 = 1; t2 <= 5; t2++)
                    for (var t3 = 1; t3 <= 4; t3++)
                    {
                        _frmMain.PlrRankALL[t, t2, t3] = 0;
                        _frmMain.PlrRankWin[t, t2, t3] = 0;
                        _frmMain.PlrRankLoss[t, t2, t3] = 0;
                        _frmMain.PlrRankPerc[t, t2, t3] = "";
                    }

                    _frmMain.TeamListCnt[t] = 0;
                    for (int t2 = 1, loopTo = _frmMain.TeamList.GetUpperBound(1); t2 <= loopTo; t2++) _frmMain.TeamList[t, t2] = tempTl;

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
                tPlrRid[t] = _frmMain.PlrRID[t];
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

            _frmMain.lbStatus.Text = "Open file...";
            Application.DoEvents();
            _frmMain.Cursor = Cursors.WaitCursor;

            // R2.01 OPEN log file and start parsing.


            using (var fs = new FileStream(_frmMain.PATH_Game, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs, Encoding.UTF8))
            {
                plrCnt = 0;

                // R2.01 Loop thru the file looking for the match stats.
                while (!sr.EndOfStream)
                {
                    var a = sr.ReadLine();

                    // **********************************************************************************
                    // R3.20 If Match Started string is found, find all of the other match stats lines.
                    // **********************************************************************************
                    if (Conversions.ToBoolean(Strings.InStr(a, "Match Started")))
                    {
                        var findMatch = true;

                        // R3.20 We have found a new section so clear the previous data.
                        for (var t = 1; t <= 8; t++)
                        {
                            _frmMain.PlrRank[t] = "---";
                            _frmMain.PlrSteam[t] = "";
                            matchRid[t] = 0L;
                        }

                        plrCnt = 0;
                        while (!sr.EndOfStream & findMatch)
                        {
                            plrCnt += 1;
                            var b = Strings.Trim(Strings.Mid(a, 98, 20));
                            if ((b == "-1") | string.IsNullOrEmpty(b)) // R2.01 Added +1 to rank code. 
                            {
                                b = ""; // R1.00 Show unranked as --       
                            }
                            else
                            {
                                var tRank = (long) Math.Round(Conversion.Val(b));
                                b = (tRank + 1L).ToString();
                            }

                            _frmMain.PlrRank[plrCnt] = b;

                            // R3.20 Get SteamID. If valid, get RelicID also.
                            _frmMain.PlrSteam[plrCnt] = Strings.Mid(a, 57, 17);
                            if (Strings.Mid(_frmMain.PlrSteam[plrCnt], 1, 4) != "7656")
                            {
                                _frmMain.PlrSteam[plrCnt] = "";
                                matchRid[plrCnt] = 0L;
                            }
                            else
                            {
                                matchRid[plrCnt] = LOG_HexToLong(Strings.Mid(a, 41,
                                    8)); // R3.20 <-- Convert.ToInt64(Mid(A, 41, 8), 16)
                                if (matchRid[plrCnt] == -1) matchRid[plrCnt] = 0L;
                            }

                            // R3.20 Read the next line of the file and exit if all of the match lines have been found.
                            a = sr.ReadLine();
                            if (Strings.InStr(a, "Match Started") == 0) findMatch = false;
                        }
                    }


                    // **********************************************************************************
                    // 3.20 If we find a GAME Human Player string, find all of the other player stats.
                    // **********************************************************************************
                    if (Conversions.ToBoolean(Strings.InStr(a, "GAME ")))
                        if (Conversions.ToBoolean(Strings.InStr(a, "Human Player") | Strings.InStr(a, "AI Player")))
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

                                gameRid[t] = 0L;
                            }

                            plrCnt = 0;
                            var findPlayers = true;
                            while (!sr.EndOfStream & findPlayers)
                            {
                                plrCnt += 1;
                                long test1 = Strings.InStr(a, "Human Player");
                                if (Conversions.ToBoolean(test1))
                                {
                                    _frmMain.PlrName[plrCnt] = LOG_FindPlayer(a, 39); // R2.01 Names are not Delimited, need to search for end of name from the end of line.
                                    gameRid[plrCnt] = LOG_Find_RelicID(a); // R3.20 Get the RelicID for this player.  
                                    _frmMain.PlrRID[plrCnt] = (int) gameRid[plrCnt]; // R4.30 Added.
                                }
                                else
                                {
                                    _frmMain.PlrName[plrCnt] = LOG_FindPlayer(a, 36); // R2.01 AI player. 
                                    // PlrRank(PlrCnt) = ""                     'R4.30 Added for match tracking.
                                    gameRid[plrCnt] = 0L; // R3.20 AI has no RelicID.
                                    _frmMain.PlrRID[plrCnt] = 0;
                                } // R4.30 Added.

                                // R3.40 The last part of the string will have faction.
                                var tLen = Strings.Len(a);
                                if (20 < tLen) // R3.40 This should never happen, but just in case.
                                {
                                    if (Strings.Mid(a, tLen - 5, 6) == "german") _frmMain.PlrFact[plrCnt] = "01";

                                    if (Strings.Mid(a, tLen - 5, 6) == "soviet") _frmMain.PlrFact[plrCnt] = "02";

                                    if (Strings.Mid(a, tLen - 10, 11) == "west_german") _frmMain.PlrFact[plrCnt] = "03";

                                    if (Strings.Mid(a, tLen - 2, 3) == "aef") _frmMain.PlrFact[plrCnt] = "04";

                                    if (Strings.Mid(a, tLen - 6, 7) == "british") _frmMain.PlrFact[plrCnt] = "05";
                                }

                                // R3.20 Read the next line of the file.
                                a = sr.ReadLine();
                                test1 = Strings.InStr(a, "Human Player");
                                long test2 = Strings.InStr(a, "AI Player");
                                if ((test1 == 0L) & (test2 == 0L)) findPlayers = false;
                            }
                        }
                }
            }

            // R4.30 Play the MATCH FOUND ALERT if this a NEW match and are we in AUTO mode.
            var fNewData = false;
            for (var t = 1; t <= 8; t++)
            {
                if ((_frmMain.PlrName[t] ?? "") != (tPlrName[t] ?? ""))
                {
                    fNewData = true;
                    break;
                }

                if (_frmMain.PlrRID[t] != tPlrRid[t])
                {
                    fNewData = true;
                    break;
                }

                if ((_frmMain.PlrFact[t] ?? "") != (tPlrFact[t] ?? ""))
                {
                    fNewData = true;
                    break;
                }
                // If PlrSteam(t) <> tPlrSteam(t) Then F_NewData = True : Exit For  'R4.30 Not in file anymore.
                // If PlrRank(t) <> tPlrRank(t) Then F_NewData = True : Exit For    'R4.30 Not in file anymore.
            }

            // R4.30 Clear the Last Valid Match stats if necessary.
            if (fNewData)
            {
                // R4.30 Reset the ELO cycle mode.
                _frmMain.FLAG_EloMode = 0;
                STATS_StoreLast();
                if (_frmMain.SCAN_Enabled && _frmMain.chkFoundSound.Checked && !string.IsNullOrEmpty(_frmMain.SOUND_File[15]) && plrCnt > 0)
                {
                    _frmMain.AUDIO_SetVolume(100, Conversions.ToInteger(_frmMain.SOUND_Vol[15]));
                    _frmMain.SOUND_Play(_frmMain.SOUND_File[15]);
                }

                // R4.50 Show time on status bar.
                _frmMain.SS1_Time.Text = "Match found: " + DateTime.Now.ToString("HH:mm"); // TimeString.ToString("HH:mm")
            }

            // R3.10 If no new data was found, show the old data.
            if (fNewData == false)
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

            _frmMain.lbStatus.Text = "Render...";
            Application.DoEvents();
            if (0 < plrCnt) matchMode = 1;

            if (2 < plrCnt) matchMode = 2;

            if (4 < plrCnt) matchMode = 3;

            if (6 < plrCnt) matchMode = 4;

            // R4.30 Adjust NAME and RANKS before drawing.
            if ((0 < plrCnt) & (fNewData | !_frmMain.SCAN_Enabled))
            {
                _frmMain.lstLog.Items.Clear(); // R4.42 Clear error log.

                // R4.30 Get player ranks from the RELIC API.
                for (var t = 1; t <= 8; t++)
                    if (!string.IsNullOrEmpty(_frmMain.PlrName[t]) & !string.IsNullOrEmpty(_frmMain.PlrFact[t]) &
                        (0d < Conversion.Val(gameRid[t])))
                    {
                        _frmMain.lbStatus.Text = "Web Player: " + t;
                        Application.DoEvents();
                        STAT_GetFromRID(gameRid[t].ToString(), t);
                        _frmMain.lbStatus.Text = "";
                        Application.DoEvents();
                        _frmMain.PlrRank[t] = _frmMain.PlrRankALL[t, Conversions.ToInteger(_frmMain.PlrFact[t]), matchMode].ToString();
                        if (_frmMain.PlrRank[t] == "0") _frmMain.PlrRank[t] = "---";

                        _frmMain.PlrWin[t] = _frmMain.PlrRankWin[t, Conversions.ToInteger(_frmMain.PlrFact[t]), matchMode];
                        _frmMain.PlrLoss[t] = _frmMain.PlrRankLoss[t, Conversions.ToInteger(_frmMain.PlrFact[t]), matchMode];
                    }

                _frmMain.lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - Complete.");
                for (var t = 1; t <= 8; t++)
                {
                    // R4.30 See if any players are a premade team.
                    var tCnt = 0;
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
                        (int) Math.Round(Conversion.Val(_frmMain.LVLS[(int) Math.Round(Conversion.Val(_frmMain.PlrFact[t])), matchMode]));
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
                    else if (0d < Conversion.Val(_frmMain.LVLS[Conversions.ToInteger(_frmMain.PlrFact[t]), matchMode]))
                    {
                        if (_frmMain.PlrRank[t] == "---")
                        {
                            _frmMain.PlrELO[t] = "---";
                            _frmMain.PlrLVL[t] = "---";
                        }
                        else
                        {
                            _frmMain.PlrELO[t] = (100d * (Conversion.Val(_frmMain.PlrRank[t]) / _frmMain.PlrGLVL[t])).ToString("##.0") + "%";
                            _frmMain.PlrLVL[t] = "L-" + LOG_CalcLevel((int) Math.Round(Conversion.Val(_frmMain.PlrRank[t])), _frmMain.PlrGLVL[t]);
                        }
                    }
                    else
                    {
                        _frmMain.PlrELO[t] = "";
                        _frmMain.PlrLVL[t] = "";
                    }
                }
            }

            // R4.34 Don't write over the ELO cycle unless we have new data.
            if (fNewData | !_frmMain.SCAN_Enabled)
            {
                // R4.34 See if we should search the net for team ranks.
                if (_frmMain.chkGetTeams.Checked)
                {
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
            if (_frmMain.SCAN_Enabled && fNewData && _frmMain._chkToggleOverlay.Checked) _frmMain._overlay.Run(_frmMain.PlrName, _frmMain.PlrRank);
            // R4.30 Clean up our GUI user indicators.
            _frmMain.Cursor = Cursors.Default;
            _frmMain.lbStatus.Text = "Ready";
            Application.DoEvents();
        }

        private void STAT_GetFromRID(string rid, int plrSlot)
        {
            var rawResp = "";
            try
            {
                // R4.30 Request leaderboard data from Relic Web API. Put result JSON data in string for parsing.
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + plrSlot +
                                          " Web Request sending...");
                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12 'R4.43 Added for Connection issues.

                var a =
                    "https://coh2-api.reliclink.com/community/leaderboard/GetPersonalStat?title=coh2&profile_ids=[" +
                    rid + "]";
                _frmMain.WBrequest1 = (HttpWebRequest) WebRequest.Create(a);
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + plrSlot +
                                          " Getting response...");
                _frmMain.WBresponse1 = (HttpWebResponse)_frmMain.WBrequest1.GetResponse();
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + plrSlot +
                                          " Getting stream...");
                _frmMain.WBreader1 = new StreamReader(_frmMain.WBresponse1.GetResponseStream());
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + plrSlot +
                                          " Reading stream...");
                rawResp = _frmMain.WBreader1.ReadToEnd();
                _frmMain.WBresponse1.Close();
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + plrSlot +
                                          " Parsing data..." + Strings.Len(rawResp) + " bytes");

                // R4.41 Added to catch bad data.
                if (Strings.Len(rawResp) < 10)
                {
                    _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID - PLR:" + plrSlot +
                                              " No data returned.");
                    _frmMain.LbError1.Text = "RID error:" + plrSlot;
                    _frmMain.LbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // R4.41 Added to catch bad data.
                a = "message" + '"' + ":" + '"' + "SUCCESS";
                var p1 = Strings.InStr(rawResp, a);
                if (p1 < 1)
                {
                    _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID - PLR:" + plrSlot +
                                              " Server returned error.");
                    _frmMain.LbError1.Text = "RID error:" + plrSlot;
                    _frmMain.LbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // R4.41 Start to PARSE the JSON data in a crude and broken manner.
                p1 = Strings.InStr(rawResp, "statGroups");

                // R4.45 Get the players COUNTRY.
                int p2;
                if (0 < p1)
                {
                    a = "profile_id" + '"' + ":" + rid;
                    p2 = Strings.InStr(p1, rawResp, a);
                    if (0 < p2)
                    {
                        p2 = Strings.InStr(p2, rawResp, "country");
                        if (0 < p2)
                        {
                            _frmMain.PlrCountry[plrSlot] = rawResp.Substring(p2 + 9, 2);
                            _frmMain.PlrCountryName[plrSlot] = Country_GetName(_frmMain.PlrCountry[plrSlot]);
                        }
                        else
                        {
                            _frmMain.PlrCountry[plrSlot] = "";
                            _frmMain.PlrCountryName[plrSlot] = "";
                        }
                    }
                }

                // R4.45 Get the Leadboard ranks (player card).
                p2 = Strings.InStr(p1, rawResp, "leaderboard_id");
                while (0 < p2)
                {
                    var s = rawResp.Substring(p2 + 15, 6);
                    var gMode = (int) Math.Round(Conversion.Val(s));
                    for (var t1 = 1; t1 <= 5; t1++)
                    for (var t2 = 1; t2 <= 4; t2++)
                        if (gMode == Conversions.ToDouble(_frmMain.RelDataLeaderId[t1, t2]))
                        {
                            var p3 = Strings.InStr(p2, rawResp, "wins");
                            _frmMain.PlrRankWin[plrSlot, t1, t2] =
                                (int) Math.Round(Conversion.Val(rawResp.Substring(p3 + 5, 10)));
                            p3 = Strings.InStr(p2, rawResp, "losses");
                            _frmMain.PlrRankLoss[plrSlot, t1, t2] =
                                (int) Math.Round(Conversion.Val(rawResp.Substring(p3 + 7, 10)));
                            p3 = Strings.InStr(p2, rawResp, "rank");
                            _frmMain.PlrRankALL[plrSlot, t1, t2] =
                                (int) Math.Round(Conversion.Val(rawResp.Substring(p3 + 5, 10)));
                            if (_frmMain.PlrRankALL[plrSlot, t1, t2] == -1) _frmMain.PlrRankALL[plrSlot, t1, t2] = 0;

                            if (0 < _frmMain.PlrRankWin[plrSlot, t1, t2])
                                _frmMain.PlrRankPerc[plrSlot, t1, t2] =
                                    (100 * _frmMain.PlrRankWin[plrSlot, t1, t2] /
                                     (double) (_frmMain.PlrRankWin[plrSlot, t1, t2] + _frmMain.PlrRankLoss[plrSlot, t1, t2]))
                                    .ToString("#0");
                            else
                                _frmMain.PlrRankPerc[plrSlot, t1, t2] = "";
                        }

                    p2 = Strings.InStr(p2 + 15, rawResp, "leaderboard_id");
                }
            }
            catch (Exception)
            {
                // R4.41 Added logging and color change.
                _frmMain.LbError1.Text = "RID error:" + plrSlot;
                _frmMain.LbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                _frmMain.LstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID - PLR:" + plrSlot + " " +
                                          Information.Err().Description);
            }

            if (!string.IsNullOrEmpty(rawResp)) STAT_GetTeamsFromRID(rawResp, plrSlot);
        }

        private void STAT_GetTeamsFromRID(string rawResp, int plRslot) // R4.45 Was  RID As Integer, PLRSlot As Integer)
        {
            var cnt = default(int);
            try
            {
                // R4.41 Added to catch bad data.
                if (Strings.Len(rawResp) < 10)
                {
                    _frmMain.LstLog1.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR TEAM - PLR:" + plRslot +
                                               " No data returned.");
                    _frmMain.LbError2.Text = "Team error:" + plRslot;
                    _frmMain.LbError2.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // R4.41 Added to catch bad data.
                var a = "message" + '"' + ":" + '"' + "SUCCESS";
                var p1 = Strings.InStr(rawResp, a);
                if (p1 < 1)
                {
                    _frmMain.LstLog1.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR TEAM - PLR:" + plRslot +
                                               " Server returned error.");
                    _frmMain.LbError2.Text = "Team error:" + plRslot;
                    _frmMain.LbError2.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // *************************************************
                // R4.33 Find all PREMADE TEAMS. Can be team of 1.
                // R4.41 Start to PARSE the JSON data in a crude and broken manner.
                // *************************************************
                p1 = Strings.InStr(rawResp, "statGroups");
                var pEnd = Strings.InStr(p1, rawResp, "leaderboardStats");
                p1 = Strings.InStr(p1 + 13, rawResp, "{" + '"' + "id");
                string s;
                while ((0 < p1) & (p1 < pEnd) & (cnt < 500))
                {
                    cnt += 1;
                    _frmMain.LbStatus.Text = "Team: " + cnt;
                    _frmMain.LbStatus.Refresh();
                    s = rawResp.Substring(p1 + 5, 9);
                    var rankId = (int) Math.Round(Conversion.Val(s));
                    p1 = Strings.InStr(p1 + 5, rawResp, "type");
                    s = rawResp.Substring(p1 + 5, 9);
                    var plrCnt = (int) Math.Round(Conversion.Val(s));

                    // R4.34 Get the relicID and Name of each player in this team. Team can be 1-4 players.
                    p1 = Strings.InStr(p1 + 5, rawResp, "profile_id");
                    s = rawResp.Substring(p1 + 11, 32);
                    var p2 = Strings.InStr(p1 + 10, rawResp, Conversions.ToString('"'));
                    var rid1 = (int) Math.Round(Conversion.Val(s.Substring(0, p2 - (p1 + 0))));
                    p1 = Strings.InStr(p1 + 5, rawResp, "alias");
                    s = rawResp.Substring(p1 + 7, 64); // R4.44 Was 32 chars long.
                    p2 = Strings.InStr(p1 + 8, rawResp, Conversions.ToString('"'));
                    var plr1 = s.Substring(0, p2 - (p1 + 8));
                    string plr2;
                    int rid2;
                    if (1 < plrCnt)
                    {
                        p1 = Strings.InStr(p1 + 5, rawResp, "profile_id");
                        s = rawResp.Substring(p1 + 11, 32);
                        p2 = Strings.InStr(p1 + 10, rawResp, Conversions.ToString('"'));
                        rid2 = (int) Math.Round(Conversion.Val(s.Substring(0, p2 - (p1 + 0))));
                        p1 = Strings.InStr(p1 + 5, rawResp, "alias");
                        s = rawResp.Substring(p1 + 7, 64); // R4.44 Was 32 chars long.
                        p2 = Strings.InStr(p1 + 8, rawResp, Conversions.ToString('"'));
                        plr2 = s.Substring(0, p2 - (p1 + 8));
                    }
                    else
                    {
                        rid2 = 0;
                        plr2 = "";
                    }

                    string plr3;
                    int rid3;
                    if (2 < plrCnt)
                    {
                        p1 = Strings.InStr(p1 + 5, rawResp, "profile_id");
                        s = rawResp.Substring(p1 + 11, 32);
                        p2 = Strings.InStr(p1 + 10, rawResp, Conversions.ToString('"'));
                        rid3 = (int) Math.Round(Conversion.Val(s.Substring(0, p2 - (p1 + 0))));
                        p1 = Strings.InStr(p1 + 5, rawResp, "alias");
                        s = rawResp.Substring(p1 + 7, 64); // R4.44 Was 32 chars long.
                        p2 = Strings.InStr(p1 + 8, rawResp, Conversions.ToString('"'));
                        plr3 = s.Substring(0, p2 - (p1 + 8));
                    }
                    else
                    {
                        rid3 = 0;
                        plr3 = "";
                    }

                    string plr4;
                    int rid4;
                    if (3 < plrCnt)
                    {
                        p1 = Strings.InStr(p1 + 5, rawResp, "profile_id");
                        s = rawResp.Substring(p1 + 11, 32);
                        p2 = Strings.InStr(p1 + 10, rawResp, Conversions.ToString('"'));
                        rid4 = (int) Math.Round(Conversion.Val(s.Substring(0, p2 - (p1 + 0))));
                        p1 = Strings.InStr(p1 + 5, rawResp, "alias");
                        s = rawResp.Substring(p1 + 7, 64); // R4.44 Was 32 chars long.
                        p2 = Strings.InStr(p1 + 8, rawResp, Conversions.ToString('"'));
                        plr4 = s.Substring(0, p2 - (p1 + 8));
                    }
                    else
                    {
                        rid4 = 0;
                        plr4 = "";
                    }

                    _frmMain.TeamList[plRslot, cnt].PLR1 = plr1;
                    _frmMain.TeamList[plRslot, cnt].PLR2 = plr2;
                    _frmMain.TeamList[plRslot, cnt].PLR3 = plr3;
                    _frmMain.TeamList[plRslot, cnt].PLR4 = plr4;
                    _frmMain.TeamList[plRslot, cnt].RID1 = rid1;
                    _frmMain.TeamList[plRslot, cnt].RID2 = rid2;
                    _frmMain.TeamList[plRslot, cnt].RID3 = rid3;
                    _frmMain.TeamList[plRslot, cnt].RID4 = rid4;
                    _frmMain.TeamList[plRslot, cnt].RankID = rankId;
                    _frmMain.TeamList[plRslot, cnt].PlrCnt = plrCnt;
                    p1 = Strings.InStr(p1 + 5, rawResp, "{" + '"' + "id");
                }

                _frmMain.TeamListCnt[plRslot] = cnt;

                // ******************************************
                // R4.33 Find all ranks for premade teams.
                // ******************************************
                p1 = pEnd;
                pEnd = Strings.Len(rawResp);
                cnt = 0;
                p1 = Strings.InStr(p1 + 13, rawResp, "statgroup_id");
                while ((0 < p1) & (p1 < pEnd) & (cnt < 1500))
                {
                    cnt += 1;
                    s = rawResp.Substring(p1 + 13, 12);
                    var rankId2 = (int) Math.Round(Conversion.Val(s));
                    p1 = Strings.InStr(p1 + 13, rawResp, "leaderboard_id");
                    s = rawResp.Substring(p1 + 15, 12);
                    var lid = (int) Math.Round(Conversion.Val(s));
                    p1 = Strings.InStr(p1 + 13, rawResp, "wins");
                    s = rawResp.Substring(p1 + 5, 12);
                    var win = (int) Math.Round(Conversion.Val(s));
                    p1 = Strings.InStr(p1 + 5, rawResp, "losses");
                    s = rawResp.Substring(p1 + 7, 12);
                    var loss = (int) Math.Round(Conversion.Val(s));
                    p1 = Strings.InStr(p1 + 7, rawResp, "rank");
                    s = rawResp.Substring(p1 + 5, 12);
                    var rank = (int) Math.Round(Conversion.Val(s));

                    // R4.33 Try to find a rank for this team. 
                    for (int t = 1, loopTo = _frmMain.TeamListCnt[plRslot]; t <= loopTo; t++)
                        if (_frmMain.TeamList[plRslot, t].RankID == rankId2)
                        {
                            if ((lid == 20) | (lid == 22) | (lid == 24))
                            {
                                _frmMain.TeamList[plRslot, t].RankAxis = rank;
                                _frmMain.TeamList[plRslot, t].WinAxis = win;
                                _frmMain.TeamList[plRslot, t].LossAxis = loss;
                                break;
                            }

                            if ((lid == 21) | (lid == 23) | (lid == 25))
                            {
                                _frmMain.TeamList[plRslot, t].RankAllies = rank;
                                _frmMain.TeamList[plRslot, t].WinAllies = win;
                                _frmMain.TeamList[plRslot, t].LossAllies = loss;
                                break;
                            }
                        }

                    p1 = Strings.InStr(p1 + 5, rawResp, "statgroup_id");
                }
            }
            catch (Exception)
            {
                // R4.41 Added logging and color change.
                _frmMain.LbError2.Text = "Team Error";
                _frmMain.LbError2.Refresh();
                _frmMain.LbError2.BackColor = Color.FromArgb(255, 255, 0, 0);
                _frmMain.LstLog1.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR TEAM - PLR:" + plRslot + " " +
                                           Information.Err().Description);
            }

            _frmMain.LbStatus.Text = "";
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
                    if (0 < _frmMain.TeamList[t, t2].RID1)
                    {
                        if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                        if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                        if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                        if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID1) cnt += 1;
                    }

                    if (0 < _frmMain.TeamList[t, t2].RID2)
                    {
                        if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                        if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                        if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                        if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID2) cnt += 1;
                    }

                    if (0 < _frmMain.TeamList[t, t2].RID3)
                    {
                        if (_frmMain.PlrRID[1] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                        if (_frmMain.PlrRID[3] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                        if (_frmMain.PlrRID[5] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                        if (_frmMain.PlrRID[7] == _frmMain.TeamList[t, t2].RID3) cnt += 1;
                    }

                    if (0 < _frmMain.TeamList[t, t2].RID4)
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
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[7, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[7, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[7, 4])); // R4.41 Bug fix.
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
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[6, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[6, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[6, 4])); // R4.41 Bug fix.
                                    break;
                                }
                            }
                        }

                        _frmMain.PlrTeam[t] = mTeam[t];
                        _frmMain.PlrTWin[t] = (int) tempWin; // R4.35 Added TEAM Win/Loss.
                        _frmMain.PlrTLoss[t] = (int) tempLoss;
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
                            _frmMain.PlrLVL[t] = "L-" + LOG_CalcLevel((int) Math.Round(Conversion.Val(_frmMain.PlrRank[t])),
                                (int) tempMax);
                        }
                    }

                // *********************************************************
                // R4.34 Loop thru TEAM 2 looking for possible teams.
                // *********************************************************
                for (var t = 2; t <= 8; t += 2)
                for (int t2 = 1, loopTo1 = _frmMain.TeamListCnt[t]; t2 <= loopTo1; t2++)
                {
                    cnt = 0;
                    if (0 < _frmMain.TeamList[t, t2].RID1)
                    {
                        if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                        if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                        if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID1) cnt += 1;

                        if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID1) cnt += 1;
                    }

                    if (0 < _frmMain.TeamList[t, t2].RID2)
                    {
                        if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                        if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                        if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID2) cnt += 1;

                        if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID2) cnt += 1;
                    }

                    if (0 < _frmMain.TeamList[t, t2].RID3)
                    {
                        if (_frmMain.PlrRID[2] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                        if (_frmMain.PlrRID[4] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                        if (_frmMain.PlrRID[6] == _frmMain.TeamList[t, t2].RID3) cnt += 1;

                        if (_frmMain.PlrRID[8] == _frmMain.TeamList[t, t2].RID3) cnt += 1;
                    }

                    if (0 < _frmMain.TeamList[t, t2].RID4)
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
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[7, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[7, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[7, 4])); // R4.41 Bug fix.
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
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[6, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[6, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    tempMax = (long) Math.Round(Conversion.Val(_frmMain.LVLS[6, 4])); // R4.41 Bug fix.
                                    break;
                                }
                            }
                        }

                        _frmMain.PlrTeam[t] = mTeam[t];
                        _frmMain.PlrTWin[t] = (int) tempWin; // R4.35 Added TEAM Win/Loss.
                        _frmMain.PlrTLoss[t] = (int) tempLoss;
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
                            _frmMain.PlrLVL[t] = "L-" + LOG_CalcLevel((int) Math.Round(Conversion.Val(_frmMain.PlrRank[t])),
                                (int) tempMax);
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

        private long LOG_HexToLong(string a)
        {
            long l;

            // R3.20 Convert a Hex String to a Long. If ERROR, set to ZERO.
            try
            {
                l = Convert.ToInt64(a, 16);
            }
            catch (Exception)
            {
                l = 0L;
            }

            return l;
        }

        private void STATS_StoreLast()
        {
            int t;
            var hit = default(int);

            // R4.30 See if we have new valid data.
            for (t = 1; t <= 8; t++)
                if (0 < _frmMain.PlrRID[t])
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

        private long LOG_Find_RelicID(string a)
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

            return rid;
        }

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