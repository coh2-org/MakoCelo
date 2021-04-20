using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MakoCelo.My.Resources;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MakoCelo
{
    public class Gfx
    {
        private frmMain _frmMain;

        public Gfx(frmMain frmMain1)
        {
            _frmMain = frmMain1;
        }

        public void GFX_DrawPlayerCard(int N)
        {
            // R4.10 OFFSET values added.
            PictureBox tPic;
            int tLabHgt;
            Brush BruRank;
            Brush BruRank2;
            Brush BruName;
            Font fonRank;
            int tX, tY;
            int Xoff = default, Yoff = default;
            var YSec = new int[6];
            var YFact = new int[6];
            var YStart = new int[6];
            var XSec = new int[11];
            var Xmid = (int) Math.Round(_frmMain.pbStats.Width * 0.5d);

            // R3.00 Precalc some vars for readability in code.
            tLabHgt = (int) ((long) Math.Round(_frmMain.LAB_Rank[1].Height) / 2L);

            // R4.30 Calc Y for each section.
            YFact[1] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.5d - tLabHgt);
            YFact[2] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.5d + _frmMain.pbStats.Height / 3d - tLabHgt);
            YFact[3] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.5d + _frmMain.pbStats.Height / 3d + _frmMain.pbStats.Height / 3d - tLabHgt);
            YFact[4] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.5d - tLabHgt);
            YFact[5] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.5d + _frmMain.pbStats.Height / 3d - tLabHgt);
            YSec[1] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0d);
            YSec[2] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.2d);
            YSec[3] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.4d);
            YSec[4] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.6d);
            YSec[5] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 0.8d);
            YStart[1] = 0;
            YStart[2] = (int) Math.Round(_frmMain.pbStats.Height / 3d);
            YStart[3] = (int) Math.Round(_frmMain.pbStats.Height / 3d * 2d);
            YStart[4] = 0;
            YStart[5] = (int) Math.Round(_frmMain.pbStats.Height / 3d);
            XSec[1] = (int) Math.Round(_frmMain.LAB_Fact[1].Width + 10f);
            XSec[2] = (int) Math.Round(_frmMain.LAB_Fact[1].Width + 10f + _frmMain.pbStats.Width * 0.4d * 0.2d);
            XSec[3] = (int) Math.Round(_frmMain.LAB_Fact[1].Width + 10f + _frmMain.pbStats.Width * 0.4d * 0.4d);
            XSec[4] = (int) Math.Round(_frmMain.LAB_Fact[1].Width + 10f + _frmMain.pbStats.Width * 0.4d * 0.6d);
            XSec[5] = (int) Math.Round(_frmMain.LAB_Fact[1].Width + 10f + _frmMain.pbStats.Width * 0.4d * 0.8d);
            XSec[6] = (int) Math.Round(_frmMain.pbStats.Width * 0.5d + XSec[1]);
            XSec[7] = (int) Math.Round(_frmMain.pbStats.Width * 0.5d + XSec[2]);
            XSec[8] = (int) Math.Round(_frmMain.pbStats.Width * 0.5d + XSec[3]);
            XSec[9] = (int) Math.Round(_frmMain.pbStats.Width * 0.5d + XSec[4]);
            XSec[10] = (int) Math.Round(_frmMain.pbStats.Width * 0.5d + XSec[5]);
            fonRank = new Font("arial", (float) (_frmMain.pbStats.Width * 0.008d), FontStyle.Regular);

            // R4.10 Get STATS offsets.
            _frmMain.OFFSET_Validate(ref Xoff, ref Yoff);

            // R4.32 Adjust our working BMP if needed.
            _frmMain.STATS_AdjustSize();
            if ((_frmMain.Main_BM.Width != _frmMain.pbStats.Width) | (_frmMain.Main_BM.Height != _frmMain.pbStats.Height))
            {
                _frmMain.Main_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                _frmMain.Main_Gfx = Graphics.FromImage(_frmMain.Main_BM);
            }

            _frmMain.Main_Gfx.Clear(_frmMain.LSName.BackC);

            // *****************************************************************
            // R3.00 Draw the stats page background.
            // *****************************************************************
            if (_frmMain.NAME_bmp is null | (_frmMain.LSName.UseCardBack == false))
                // R3.00 No image available so draw a solid color background.
                _frmMain.Main_Gfx.FillRectangle(new SolidBrush(_frmMain.LSName.BackC), 0, 0, _frmMain.pbStats.Width, _frmMain.pbStats.Height);
            else
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(_frmMain.LSName.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, 0, 0, _frmMain.NAME_bmp.Width, _frmMain.NAME_bmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = _frmMain.Main_BM.Height;
                        for (tY = 0; _frmMain.NAME_bmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += _frmMain.NAME_bmp.Height)
                        {
                            var loopTo1 = _frmMain.Main_BM.Width;
                            for (tX = 0; _frmMain.NAME_bmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1; tX += _frmMain.NAME_bmp.Width) _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, tX, tY, _frmMain.NAME_bmp.Width, _frmMain.NAME_bmp.Height);
                        }

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, 0, 0, _frmMain.Main_BM.Width, _frmMain.Main_BM.Height);
                        break;
                    }
                }


            // *****************************************************************
            // Draw the Faction images to the stats page.
            // ***************************************************************** 
            BruRank = new SolidBrush(_frmMain.LSRank.F1);
            BruRank2 = new SolidBrush(_frmMain.LSRank.F2);
            BruName = new SolidBrush(_frmMain.LSName.F1);
            for (var t = 1; t <= 3; t++)
            {
                tPic = (PictureBox) _frmMain.Controls["pbFact0" + t];
                _frmMain.Main_Gfx.DrawImage(tPic.Image, 0f, YFact[t], _frmMain.LAB_Fact[t].Width, _frmMain.LAB_Fact[t].Height);
                if (t == 1)
                {
                    _frmMain.Main_Gfx.DrawString("Mode", fonRank, BruRank, XSec[1], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("Rank", fonRank, BruRank, XSec[2], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("Win", fonRank, BruRank, XSec[3], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("Loss", fonRank, BruRank, XSec[4], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("%", fonRank, BruRank, XSec[5], YStart[t] + YSec[1]);
                }

                _frmMain.Main_Gfx.DrawString("1v1", fonRank, BruRank2, XSec[1], YStart[t] + YSec[2]);
                if (_frmMain.PlrRankALL[N, t, 1] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 1].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[2]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 1].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[2]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 1].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[2]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 1], fonRank, BruName, XSec[5], YStart[t] + YSec[2]);
                _frmMain.Main_Gfx.DrawString("2v2", fonRank, BruRank2, XSec[1], YStart[t] + YSec[3]);
                if (_frmMain.PlrRankALL[N, t, 2] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 2].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[3]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 2].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[3]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 2].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[3]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 2], fonRank, BruName, XSec[5], YStart[t] + YSec[3]);
                _frmMain.Main_Gfx.DrawString("3v3", fonRank, BruRank2, XSec[1], YStart[t] + YSec[4]);
                if (_frmMain.PlrRankALL[N, t, 3] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 3].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[4]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 3].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[4]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 3].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[4]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 3], fonRank, BruName, XSec[5], YStart[t] + YSec[4]);
                _frmMain.Main_Gfx.DrawString("4v4", fonRank, BruRank2, XSec[1], YStart[t] + YSec[5]);
                if (_frmMain.PlrRankALL[N, t, 4] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 4].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[5]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 4].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[5]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 4].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[5]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 4], fonRank, BruName, XSec[5], YStart[t] + YSec[5]);
            }

            for (var t = 4; t <= 5; t++)
            {
                tPic = (PictureBox) _frmMain.Controls["pbFact0" + t];
                _frmMain.Main_Gfx.DrawImage(tPic.Image, Xmid, YFact[t], _frmMain.LAB_Fact[t].Width, _frmMain.LAB_Fact[t].Height);
                if (t == 4)
                {
                    _frmMain.Main_Gfx.DrawString("Mode", fonRank, BruRank, XSec[6], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("Rank", fonRank, BruRank, XSec[7], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("Win", fonRank, BruRank, XSec[8], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("Loss", fonRank, BruRank, XSec[9], YStart[t] + YSec[1]);
                    _frmMain.Main_Gfx.DrawString("%", fonRank, BruRank, XSec[10], YStart[t] + YSec[1]);
                }

                _frmMain.Main_Gfx.DrawString("1v1", fonRank, BruRank2, XSec[6], YStart[t] + YSec[2]);
                if (_frmMain.PlrRankALL[N, t, 1] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 1].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[2]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 1].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[2]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 1].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[2]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 1], fonRank, BruName, XSec[10], YStart[t] + YSec[2]);
                _frmMain.Main_Gfx.DrawString("2v2", fonRank, BruRank2, XSec[6], YStart[t] + YSec[3]);
                if (_frmMain.PlrRankALL[N, t, 2] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 2].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[3]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 2].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[3]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 2].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[3]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 2], fonRank, BruName, XSec[10], YStart[t] + YSec[3]);
                _frmMain.Main_Gfx.DrawString("3v3", fonRank, BruRank2, XSec[6], YStart[t] + YSec[4]);
                if (_frmMain.PlrRankALL[N, t, 3] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 3].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[4]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 3].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[4]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 3].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[4]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 3], fonRank, BruName, XSec[10], YStart[t] + YSec[4]);
                _frmMain.Main_Gfx.DrawString("4v4", fonRank, BruRank2, XSec[6], YStart[t] + YSec[5]);
                if (_frmMain.PlrRankALL[N, t, 4] != 0) _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankALL[N, t, 4].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[5]);

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankWin[N, t, 4].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[5]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankLoss[N, t, 4].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[5]);
                _frmMain.Main_Gfx.DrawString(_frmMain.PlrRankPerc[N, t, 4], fonRank, BruName, XSec[10], YStart[t] + YSec[5]);
            }

            _frmMain.Main_Gfx.DrawString("Player: " + _frmMain.PlrName[N], fonRank, BruRank, XSec[6], YStart[3] + YSec[2]);
            // R4.46 Removed.   Main_Gfx.DrawString("RelicID: " & PlrRID(N), fonRank, BruRank, XSec(6), YStart(3) + YSec(3))
            _frmMain.Main_Gfx.DrawString("Left Click to exit, Right Click for teams", fonRank, BruName, XSec[6],
                YStart[3] + YSec[4]);
            _frmMain.Main_Gfx.DrawString("Team Win: " + _frmMain.PlrTWin[N], fonRank, BruRank, XSec[9], YStart[3] + YSec[2]);
            _frmMain.Main_Gfx.DrawString("Team Loss: " + _frmMain.PlrTLoss[N], fonRank, BruRank, XSec[9], YStart[3] + YSec[3]);
            _frmMain.Main_Gfx.DrawString("Team: " + _frmMain.PlrTeam[N], fonRank, BruRank, XSec[9], YStart[3] + YSec[4]);

            // R4.45 Added country flags.
            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[N]))
            {
                var bmCountry = Resources.ResourceManager.GetObject("flag_" + _frmMain.PlrCountry[N]) as Bitmap;
                if (!Information.IsNothing(bmCountry)) _frmMain.Main_Gfx.DrawImage(bmCountry, new Point(XSec[6] - 20, YStart[3] + YSec[2]));

                _frmMain.Main_Gfx.DrawString(_frmMain.PlrCountry[N], fonRank, BruRank, XSec[6] - 20, YStart[3] + YSec[3]);
                _frmMain.Main_Gfx.DrawString("Country: " + _frmMain.PlrCountryName[N], fonRank, BruRank, XSec[6],
                    YStart[3] + YSec[3]); // R4.46 Added.
            }

            _frmMain.pbStats.Image = _frmMain.Main_BM;
        }

        public void GFX_DrawTeams(int n)
        {
            // R4.10 OFFSET values added.
            string A;
            Brush BruRank;
            Brush BruRank2;
            Brush BruName;
            Font fonRank;
            int tX, tY;
            int Yoff;
            int YAct;

            // R3.00 Precalc some vars for readability in code.
            fonRank = new Font("arial", (float) (_frmMain.pbStats.Width * 0.008d), FontStyle.Regular);

            // R4.32 Adjust our working BMP if needed.
            _frmMain.STATS_AdjustSize();
            if ((_frmMain.Main_BM.Width != _frmMain.pbStats.Width) | (_frmMain.Main_BM.Height != _frmMain.pbStats.Height))
            {
                _frmMain.Main_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                _frmMain.Main_Gfx = Graphics.FromImage(_frmMain.Main_BM);
            }

            _frmMain.Main_Gfx.Clear(_frmMain.LSName.BackC);

            // *****************************************************************
            // R3.00 Draw the stats page background.
            // *****************************************************************
            if (_frmMain.NAME_bmp is null | (_frmMain.LSName.UseCardBack == false))
                // R3.00 No image available so draw a solid color background.
                _frmMain.Main_Gfx.FillRectangle(new SolidBrush(_frmMain.LSName.BackC), 0, 0, _frmMain.pbStats.Width, _frmMain.pbStats.Height);
            else
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(_frmMain.LSName.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, 0, 0, _frmMain.NAME_bmp.Width, _frmMain.NAME_bmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = _frmMain.Main_BM.Height;
                        for (tY = 0; _frmMain.NAME_bmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += _frmMain.NAME_bmp.Height)
                        {
                            var loopTo1 = _frmMain.Main_BM.Width;
                            for (tX = 0; _frmMain.NAME_bmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1; tX += _frmMain.NAME_bmp.Width) _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, tX, tY, _frmMain.NAME_bmp.Width, _frmMain.NAME_bmp.Height);
                        }

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, 0, 0, _frmMain.Main_BM.Width, _frmMain.Main_BM.Height);
                        break;
                    }
                }

            // *****************************************************************
            // Draw the Faction images to the stats page.
            // ***************************************************************** 
            BruRank = new SolidBrush(_frmMain.LSRank.F1);
            BruRank2 = new SolidBrush(_frmMain.LSRank.F2);
            BruName = new SolidBrush(_frmMain.LSName.F1);
            Yoff = _frmMain.scrStats.Value * -1; // * -75

            // R4.40 Loop thru TEAM lists and draw them until we are off screen.
            for (int t = 1, loopTo2 = _frmMain.TeamListCnt[n]; t <= loopTo2; t++)
            {
                YAct = Yoff + (t - 1) * 90;
                if (_frmMain.pbStats.Height < YAct) break;

                if (-91 < YAct)
                {
                    _frmMain.Main_Gfx.DrawString(t.ToString(), fonRank, BruName, 10f, YAct);
                    _frmMain.Main_Gfx.DrawString("Team of ", fonRank, BruRank, 45f, YAct);
                    _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].PlrCnt.ToString(), fonRank, BruName, 90f, YAct);
                    _frmMain.Main_Gfx.DrawString("Allies:", fonRank, BruRank, 120f, YAct);
                    _frmMain.Main_Gfx.DrawString("Axis:", fonRank, BruRank, 260f, YAct);
                    A = " (" + _frmMain.TeamList[n, t].WinAllies + "," + _frmMain.TeamList[n, t].LossAllies + ")";
                    switch (_frmMain.TeamList[n, t].RankAllies)
                    {
                        case -1:
                        {
                            _frmMain.Main_Gfx.DrawString("P" + A, fonRank, BruName, 160f, YAct);
                            break;
                        }

                        case 0:
                        {
                            _frmMain.Main_Gfx.DrawString("---" + A, fonRank, BruName, 160f, YAct);
                            break;
                        }

                        default:
                        {
                            _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].RankAllies + A, fonRank, BruName, 160f, YAct);
                            break;
                        }
                    }

                    A = " (" + _frmMain.TeamList[n, t].WinAxis + "," + _frmMain.TeamList[n, t].LossAxis + ")";
                    switch (_frmMain.TeamList[n, t].RankAxis)
                    {
                        case -1:
                        {
                            _frmMain.Main_Gfx.DrawString("P" + A, fonRank, BruName, 290f, YAct);
                            break;
                        }

                        case 0:
                        {
                            _frmMain.Main_Gfx.DrawString("---" + A, fonRank, BruName, 290f, YAct);
                            break;
                        }

                        default:
                        {
                            _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].RankAxis + A, fonRank, BruName, 290f, YAct);
                            break;
                        }
                    }

                    _frmMain.Main_Gfx.DrawString("1:", fonRank, BruRank2, 10f, YAct + 15);
                    _frmMain.Main_Gfx.DrawString("2:", fonRank, BruRank2, 10f, YAct + 30);
                    _frmMain.Main_Gfx.DrawString("3:", fonRank, BruRank2, 10f, YAct + 45);
                    _frmMain.Main_Gfx.DrawString("4:", fonRank, BruRank2, 10f, YAct + 60);
                    _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].PLR1, fonRank, BruName, 25f, YAct + 15);
                    _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].PLR2, fonRank, BruName, 25f, YAct + 30);
                    _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].PLR3, fonRank, BruName, 25f, YAct + 45);
                    _frmMain.Main_Gfx.DrawString(_frmMain.TeamList[n, t].PLR4, fonRank, BruName, 25f, YAct + 60);
                }
            }

            _frmMain.Main_Gfx.DrawString("PREMADE TEAM RANKS", fonRank, BruRank2, 500f, 0f);
            _frmMain.Main_Gfx.DrawString("Player: " + _frmMain.PlrName[n], fonRank, BruRank, 500f, 15f);
            _frmMain.Main_Gfx.DrawString("RelicID: " + _frmMain.PlrRID[n], fonRank, BruRank, 500f, 30f);
            _frmMain.Main_Gfx.DrawString("Click the screen again to exit", fonRank, BruName, 500f, 45f);
            _frmMain.pbStats.Image = _frmMain.Main_BM;
        }

        private void GFX_BuildStatsBackground()
        {
            // R4.10 OFFSET values added.
            LinearGradientBrush linGrBrush;
            int tX, tY;
            int Xoff = default, Yoff = default;
            Pen tPen;

            // R3.00 Precalc some vars for readability in code.

            // R4.10 Get STATS offsets.
            _frmMain.OFFSET_Validate(ref Xoff, ref Yoff);

            // *****************************************************************
            // R3.00 Draw the stats page background.
            // *****************************************************************
            if (_frmMain.NAME_bmp is null)
                // R3.00 No image available so draw a solid color background.
                _frmMain.Main_Gfx.FillRectangle(new SolidBrush(_frmMain.LSName.BackC), 0, 0, _frmMain.pbStats.Width, _frmMain.pbStats.Height);
            else
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(_frmMain.LSName.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, 0, 0, _frmMain.NAME_bmp.Width, _frmMain.NAME_bmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = _frmMain.Main_BM.Height;
                        for (tY = 0; _frmMain.NAME_bmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += _frmMain.NAME_bmp.Height)
                        {
                            var loopTo1 = _frmMain.Main_BM.Width;
                            for (tX = 0; _frmMain.NAME_bmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1; tX += _frmMain.NAME_bmp.Width) _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, tX, tY, _frmMain.NAME_bmp.Width, _frmMain.NAME_bmp.Height);
                        }

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NAME_bmp, 0, 0, _frmMain.Main_BM.Width, _frmMain.Main_BM.Height);
                        break;
                    }
                }

            switch (_frmMain.LSRank.BorderPanelMode)
            {
                case 0: // R4.40 No border.
                {
                    break;
                }

                case 1: // R4.40 Normal.
                {
                    // Main_Gfx.draw
                    tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderPanelColor))
                    {
                        Width = _frmMain.LSRank.BorderPanelWidth
                    };
                    _frmMain.Main_Gfx.DrawRectangle(tPen, 2, 2, _frmMain.Main_BM.Width - 5, _frmMain.Main_BM.Height - 5);
                    break;
                }

                case 2: // R4.40 Glow
                {
                    _frmMain.Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(255, _frmMain.LSRank.BorderPanelColor.R, _frmMain.LSRank.BorderPanelColor.G, _frmMain.LSRank.BorderPanelColor.B))), 2, 2, _frmMain.Main_BM.Width - 5, _frmMain.Main_BM.Height - 5);
                    _frmMain.Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(192, _frmMain.LSRank.BorderPanelColor.R, _frmMain.LSRank.BorderPanelColor.G, _frmMain.LSRank.BorderPanelColor.B))), 3, 3, _frmMain.Main_BM.Width - 7, _frmMain.Main_BM.Height - 7);
                    _frmMain.Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(128, _frmMain.LSRank.BorderPanelColor.R, _frmMain.LSRank.BorderPanelColor.G, _frmMain.LSRank.BorderPanelColor.B))), 4, 4, _frmMain.Main_BM.Width - 9, _frmMain.Main_BM.Height - 9);
                    _frmMain.Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(64, _frmMain.LSRank.BorderPanelColor.R, _frmMain.LSRank.BorderPanelColor.G, _frmMain.LSRank.BorderPanelColor.B))), 5, 5, _frmMain.Main_BM.Width - 11, _frmMain.Main_BM.Height - 11);
                    _frmMain.Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(32, _frmMain.LSRank.BorderPanelColor.R, _frmMain.LSRank.BorderPanelColor.G, _frmMain.LSRank.BorderPanelColor.B))), 6, 6, _frmMain.Main_BM.Width - 13, _frmMain.Main_BM.Height - 13);
                    break;
                }

                case 3: // R4.40 3D
                {
                    _frmMain.Main_Gfx.DrawLine(new Pen(new SolidBrush(_frmMain.LSRank.BorderPanelColor)), 2, 2, _frmMain.Main_BM.Width - 3, 2);
                    _frmMain.Main_Gfx.DrawLine(new Pen(new SolidBrush(_frmMain.LSRank.BorderPanelColor)), 2, 2, 2, _frmMain.Main_BM.Height - 3);
                    _frmMain.Main_Gfx.DrawLine(new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0))), 2, _frmMain.Main_BM.Height - 3, _frmMain.Main_BM.Width - 3, _frmMain.Main_BM.Height - 3);
                    _frmMain.Main_Gfx.DrawLine(new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0))), _frmMain.Main_BM.Width - 3, 2, _frmMain.Main_BM.Width - 3, _frmMain.Main_BM.Height - 2);
                    break;
                }

                case 4: // R4.40 Rounded Corners 4 pixels.
                {
                    tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderPanelColor))
                    {
                        Width = _frmMain.LSRank.BorderPanelWidth
                    };
                    DrawRoundedRectangle(_frmMain.Main_Gfx, new Rectangle(2, 2, _frmMain.Main_BM.Width - 5, _frmMain.Main_BM.Height - 5), tPen, 4);
                    break;
                }

                case 5: // R4.40 Rounded Corners 8 pixels.
                {
                    tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderPanelColor))
                    {
                        Width = _frmMain.LSRank.BorderPanelWidth
                    };
                    DrawRoundedRectangle(_frmMain.Main_Gfx, new Rectangle(2, 2, _frmMain.Main_BM.Width - 5, _frmMain.Main_BM.Height - 5), tPen, 8);
                    break;
                }

                case 6: // R4.40 Rounded Corners LARGE.
                {
                    tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderPanelColor))
                    {
                        Width = _frmMain.LSRank.BorderPanelWidth
                    };
                    DrawRoundedRectangle_Max(_frmMain.Main_Gfx, new Rectangle(2, 2, _frmMain.Main_BM.Width - 5, _frmMain.Main_BM.Height - 5),
                        tPen);
                    break;
                }
            }

            // *****************************************************************
            // R3.10 Call any background based FX MODE options like SHADOW, etc
            // *****************************************************************
            if (_frmMain.CFX3DActive[(int) clsGlobal.FXMode.LabelBlur]) GFX_FX_LabBlur(_frmMain.Main_Gfx, _frmMain.Main_BM, Xoff, Yoff);

            // *****************************************************************
            // R3.10 Draw the Rank background rectangles
            // *****************************************************************
            for (var T = 1; T <= 8; T++)
                if (!_frmMain.FLAG_HideMissing | !string.IsNullOrEmpty(_frmMain.PlrName[T]) | !string.IsNullOrEmpty(_frmMain.PlrSteam[T]))
                {
                    // R3.00 Draw the RANK background rectangle.
                    linGrBrush = _frmMain.LSRank.BDir == 0
                        ? new LinearGradientBrush(new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y - 1f)),
                            new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y + _frmMain.LAB_Rank[T].Height + 1f)), _frmMain.LSRank.B1, _frmMain.LSRank.B2)
                        : new LinearGradientBrush(new Point((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X - 1f), Yoff + 0),
                            new Point((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width + 1f), Yoff + 0), _frmMain.LSRank.B1, _frmMain.LSRank.B2);

                    // R4.40 Added BORDERS.
                    switch (_frmMain.LSRank.BorderMode)
                    {
                        case 0: // R4.40 No border.
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, _frmMain.LAB_Rank[T].Width, _frmMain.LAB_Rank[T].Height);
                            break;
                        }

                        case 1: // R4.40 Normal.
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, _frmMain.LAB_Rank[T].Width, _frmMain.LAB_Rank[T].Height);
                            tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderColor))
                            {
                                Width = _frmMain.LSRank.BorderWidth
                            };
                            _frmMain.Main_Gfx.DrawRectangle(tPen, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, _frmMain.LAB_Rank[T].Width, _frmMain.LAB_Rank[T].Height);
                            break;
                        }

                        case 2: // R4.40 Glow
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, _frmMain.LAB_Rank[T].Width, _frmMain.LAB_Rank[T].Height);
                            _frmMain.Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(255, _frmMain.LSRank.BorderColor.R, _frmMain.LSRank.BorderColor.G, _frmMain.LSRank.BorderColor.B))), Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, _frmMain.LAB_Rank[T].Width - 1f, _frmMain.LAB_Rank[T].Height - 1f);
                            _frmMain.Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(192, _frmMain.LSRank.BorderColor.R, _frmMain.LSRank.BorderColor.G, _frmMain.LSRank.BorderColor.B))), Xoff + _frmMain.LAB_Rank[T].X + 1f, Yoff + _frmMain.LAB_Rank[T].Y + 1f, _frmMain.LAB_Rank[T].Width - 3f, _frmMain.LAB_Rank[T].Height - 3f);
                            _frmMain.Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(64, _frmMain.LSRank.BorderColor.R, _frmMain.LSRank.BorderColor.G, _frmMain.LSRank.BorderColor.B))), Xoff + _frmMain.LAB_Rank[T].X + 2f, Yoff + _frmMain.LAB_Rank[T].Y + 2f, _frmMain.LAB_Rank[T].Width - 5f, _frmMain.LAB_Rank[T].Height - 5f);
                            break;
                        }

                        case 3: // R4.40 3D
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, _frmMain.LAB_Rank[T].Width, _frmMain.LAB_Rank[T].Height);
                            tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderColor))
                            {
                                Width = _frmMain.LSRank.BorderWidth
                            };
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y,
                                Xoff + _frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width - 1f, Yoff + _frmMain.LAB_Rank[T].Y);
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y, Xoff + _frmMain.LAB_Rank[T].X,
                                Yoff + _frmMain.LAB_Rank[T].Y + _frmMain.LAB_Rank[T].Height - 1f);
                            tPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                            {
                                Width = _frmMain.LSRank.BorderWidth
                            };
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Rank[T].X, Yoff + _frmMain.LAB_Rank[T].Y + _frmMain.LAB_Rank[T].Height,
                                Xoff + _frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width, Yoff + _frmMain.LAB_Rank[T].Y + _frmMain.LAB_Rank[T].Height);
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width, Yoff + _frmMain.LAB_Rank[T].Y,
                                Xoff + _frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width, Yoff + _frmMain.LAB_Rank[T].Y + _frmMain.LAB_Rank[T].Height);
                            break;
                        }

                        case 4: // R4.40 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)), linGrBrush, 4);
                            tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderColor))
                            {
                                Width = _frmMain.LSRank.BorderWidth
                            };
                            DrawRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)), tPen, 4);
                            break;
                        }

                        case 5: // R4.40 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)), linGrBrush, 8);
                            tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderColor))
                            {
                                Width = _frmMain.LSRank.BorderWidth
                            };
                            DrawRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)), tPen, 8);
                            break;
                        }

                        case 6: // R4.40 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)), linGrBrush);
                            tPen = new Pen(new SolidBrush(_frmMain.LSRank.BorderColor))
                            {
                                Width = _frmMain.LSRank.BorderWidth
                            };
                            DrawRoundedRectangle_Max(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)), tPen);
                            break;
                        }
                    }
                }

            // *****************************************************************
            // R3.10 Draw the Name background rectangles
            // *****************************************************************  
            for (var T = 1; T <= 8; T++)
                if (!_frmMain.FLAG_HideMissing | !string.IsNullOrEmpty(_frmMain.PlrName[T]))
                {
                    // R3.00 Draw the NAME background rectangle.
                    if (_frmMain.LSName.BDir == 0)
                        linGrBrush = new LinearGradientBrush(
                            new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y - 1f)),
                            new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + _frmMain.LAB_Name[T].Height + 1f)), _frmMain.LSName.B1, _frmMain.LSName.B2);
                    // R3.40 Reverse gradient direction for RIGHT justified text.
                    else if (_frmMain.LAB_Name_Align[T].Alignment == StringAlignment.Far)
                        linGrBrush = new LinearGradientBrush(
                            new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width + 1f), Yoff + 0),
                            new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X - 1f), Yoff + 0), _frmMain.LSName.B1, _frmMain.LSName.B2);
                    else
                        linGrBrush = new LinearGradientBrush(
                            new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X - 1f), Yoff + 0),
                            new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width + 1f), Yoff + 0), _frmMain.LSName.B1, _frmMain.LSName.B2);

                    switch (_frmMain.LSName.BorderMode)
                    {
                        case 0:
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, _frmMain.LAB_Name[T].Width, _frmMain.LAB_Name[T].Height);
                            break;
                        }

                        case 1: // R4.40 Normal.
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, _frmMain.LAB_Name[T].Width, _frmMain.LAB_Name[T].Height);
                            tPen = new Pen(new SolidBrush(_frmMain.LSName.BorderColor))
                            {
                                Width = _frmMain.LSName.BorderWidth
                            };
                            _frmMain.Main_Gfx.DrawRectangle(tPen, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, _frmMain.LAB_Name[T].Width, _frmMain.LAB_Name[T].Height);
                            break;
                        }

                        case 2: // R4.40 Glow
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, _frmMain.LAB_Name[T].Width, _frmMain.LAB_Name[T].Height);
                            _frmMain.Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(255, _frmMain.LSName.BorderColor.R, _frmMain.LSName.BorderColor.G, _frmMain.LSName.BorderColor.B))), Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, _frmMain.LAB_Name[T].Width - 1f, _frmMain.LAB_Name[T].Height - 1f);
                            _frmMain.Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(192, _frmMain.LSName.BorderColor.R, _frmMain.LSName.BorderColor.G, _frmMain.LSName.BorderColor.B))), Xoff + _frmMain.LAB_Name[T].X + 1f, Yoff + _frmMain.LAB_Name[T].Y + 1f, _frmMain.LAB_Name[T].Width - 3f, _frmMain.LAB_Name[T].Height - 3f);
                            _frmMain.Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(64, _frmMain.LSName.BorderColor.R, _frmMain.LSName.BorderColor.G, _frmMain.LSName.BorderColor.B))), Xoff + _frmMain.LAB_Name[T].X + 2f, Yoff + _frmMain.LAB_Name[T].Y + 2f, _frmMain.LAB_Name[T].Width - 5f, _frmMain.LAB_Name[T].Height - 5f);
                            break;
                        }

                        case 3: // R4.40 3D
                        {
                            _frmMain.Main_Gfx.FillRectangle(linGrBrush, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, _frmMain.LAB_Name[T].Width, _frmMain.LAB_Name[T].Height);
                            tPen = new Pen(new SolidBrush(_frmMain.LSName.BorderColor))
                            {
                                Width = _frmMain.LSName.BorderWidth
                            };
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y,
                                Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width - 1f, Yoff + _frmMain.LAB_Name[T].Y);
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y, Xoff + _frmMain.LAB_Name[T].X,
                                Yoff + _frmMain.LAB_Name[T].Y + _frmMain.LAB_Name[T].Height - 1f);
                            tPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                            {
                                Width = _frmMain.LSName.BorderWidth
                            };
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Name[T].X, Yoff + _frmMain.LAB_Name[T].Y + _frmMain.LAB_Name[T].Height,
                                Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width - 1f,
                                Yoff + _frmMain.LAB_Name[T].Y + _frmMain.LAB_Name[T].Height);
                            _frmMain.Main_Gfx.DrawLine(tPen, Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width, Yoff + _frmMain.LAB_Name[T].Y,
                                Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width,
                                Yoff + _frmMain.LAB_Name[T].Y + _frmMain.LAB_Name[T].Height - 1f);
                            break;
                        }

                        case 4: // R4.40 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[T].Height)), linGrBrush, 4);
                            tPen = new Pen(new SolidBrush(_frmMain.LSName.BorderColor))
                            {
                                Width = _frmMain.LSName.BorderWidth
                            };
                            DrawRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[T].Height)), tPen, 4);
                            break;
                        }

                        case 5: // R4.40 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[T].Height)), linGrBrush, 8);
                            tPen = new Pen(new SolidBrush(_frmMain.LSName.BorderColor))
                            {
                                Width = _frmMain.LSName.BorderWidth
                            };
                            DrawRoundedRectangle(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[T].Height)), tPen, 8);
                            break;
                        }

                        case 6: // R4.40 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[T].Height)), linGrBrush);
                            tPen = new Pen(new SolidBrush(_frmMain.LSName.BorderColor))
                            {
                                Width = _frmMain.LSName.BorderWidth
                            };
                            DrawRoundedRectangle_Max(_frmMain.Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[T].Height)), tPen);
                            break;
                        }
                    }
                }

            // R4.50 Save the background image we just created for fast draws later.
            _frmMain.MainBuffer_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
            _frmMain.MainBuffer_Gfx = Graphics.FromImage(_frmMain.MainBuffer_BM);
            _frmMain.MainBuffer_Gfx.DrawImage(_frmMain.Main_BM, 0, 0);
            _frmMain.MainBuffer_Valid = true;
        }

        private void GFX_BuildStatsForeground()
        {
            // R4.10 OFFSET values added.
            int tLabHgt;
            LinearGradientBrush linGrBrush;
            int tX, tY;
            int Xoff = default, Yoff = default;
            var tString = "";
            int Cx, Cy;
            int TextHgt12;
            PictureBox tPic;

            // R3.00 Precalc some vars for readability in code.
            tLabHgt = (int) ((long) Math.Round(_frmMain.LAB_Rank[1].Height) / 2L);

            // R4.10 Get STATS offsets.
            _frmMain.OFFSET_Validate(ref Xoff, ref Yoff);

            // *****************************************************************
            // R3.10 Call any MID draw based FX MODE options like SHADOW, etc
            // *****************************************************************
            if (_frmMain.CFX3DActive[(int) clsGlobal.FXMode.Shadow]) GFX_FX_Shadow(_frmMain.Main_Gfx, Xoff, Yoff);

            // *****************************************************************
            // R3.00 Define paint/fill brushes for the RANK stats.
            // *****************************************************************
            var tBrushShadow = new SolidBrush(_frmMain.LSRank.ShadowColor);
            TextHgt12 = (int) Math.Round(_frmMain.Main_Gfx.MeasureString("X", frmMain.FONT_Rank).Height /
                                         2f); // R3.30 Calc height of gradient color.  'R3.30Changed from Xq.
            for (var T = 1; T <= 8; T++)
            {
                if (_frmMain.FLAG_EloUse == false)
                {
                    tString = _frmMain.PlrRank[T];
                }
                else
                {
                    if (_frmMain.RankDisplayMode == RankDisplayMode.Rank) tString = _frmMain.PlrRank[T];

                    if (_frmMain.RankDisplayMode == RankDisplayMode.Elo) tString = _frmMain.PlrELO[T];

                    if (_frmMain.RankDisplayMode == RankDisplayMode.Lvl) tString = _frmMain.PlrLVL[T];
                }

                // R3.00 Create a clipping area so names do not draw past the rectangle.
                _frmMain.Main_Gfx.Clip = new Region(new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X + 1f),
                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width - 2f),
                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)));

                // R4.00 Draw the RANK Shadow text.
                Cx = (int) Math.Round(_frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width / 2f - _frmMain.Main_Gfx.MeasureString(tString, frmMain.FONT_Rank).Width / 2f);
                Cy = (int) Math.Round(_frmMain.LAB_Rank[T].Y + _frmMain.LAB_Rank[T].Height / 2f - _frmMain.Main_Gfx.MeasureString(tString, frmMain.FONT_Rank).Height / 2f);
                if ((_frmMain.LSRank.ShadowX != 0) | (_frmMain.LSRank.ShadowY != 0))
                    _frmMain.Main_Gfx.DrawString(tString, frmMain.FONT_Rank, tBrushShadow,
                        (float) (Xoff + Cx + _frmMain.LSRank.ShadowX * Conversion.Val(_frmMain.LSRank.ShadowDepth)),
                        (float) (Yoff + Cy + _frmMain.LSRank.ShadowY * Conversion.Val(_frmMain.LSRank.ShadowDepth)));

                // R3.00 Draw the RANK text.
                linGrBrush = _frmMain.LSRank.FDir == 0
                    ? new LinearGradientBrush(
                        new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Ycenter - TextHgt12)),
                        new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Ycenter + TextHgt12)),
                        Color.FromArgb(255, _frmMain.LSRank.F1.R, _frmMain.LSRank.F1.G, _frmMain.LSRank.F1.B),
                        Color.FromArgb(255, _frmMain.LSRank.F2.R, _frmMain.LSRank.F2.G, _frmMain.LSRank.F2.B))
                    : new LinearGradientBrush(new Point((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X), Yoff + 0),
                        new Point((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X + _frmMain.LAB_Rank[T].Width), Yoff + 0),
                        Color.FromArgb(255, _frmMain.LSRank.F1.R, _frmMain.LSRank.F1.G, _frmMain.LSRank.F1.B),
                        Color.FromArgb(255, _frmMain.LSRank.F2.R, _frmMain.LSRank.F2.G, _frmMain.LSRank.F2.B));

                _frmMain.Main_Gfx.DrawString(tString, frmMain.FONT_Rank, linGrBrush, Xoff + Cx, Yoff + Cy);

                // R3.00 Clear the clipping area.
                _frmMain.Main_Gfx.Clip = new Region(new Rectangle(0, 0, _frmMain.Main_BM.Width, _frmMain.Main_BM.Height));
            }

            // *****************************************************************
            // Draw the Faction images to the stats page.
            // ***************************************************************** 
            for (var t = 1; t <= 8; t++)
                if (!string.IsNullOrEmpty(_frmMain.PlrFact[t]))
                {
                    tPic = (PictureBox) _frmMain.Controls["pbFact" + _frmMain.PlrFact[t]];
                    _frmMain.Main_Gfx.DrawImage(tPic.Image, Xoff + _frmMain.LAB_Fact[t].X, Yoff + _frmMain.LAB_Fact[t].Y, _frmMain.LAB_Fact[t].Width, _frmMain.LAB_Fact[t].Height);
                }

            // *****************************************************************
            // R3.00 Define paint/fill brushes for the NAME stats.
            // *****************************************************************  
            tBrushShadow = new SolidBrush(_frmMain.LSName.ShadowColor);
            TextHgt12 = (int) Math.Round(_frmMain.Main_Gfx.MeasureString("X", frmMain.FONT_Name).Height / 2f); // R3.30 Changed from Xq.
            for (var T = 1; T <= 8; T++)
            {
                // R3.00 Create a clipping area so names do not draw past the rectangle.
                _frmMain.Main_Gfx.Clip = new Region(new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + 1f),
                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width - 2f),
                    (int) Math.Round(_frmMain.LAB_Name[T].Height)));

                // R3.40 Adjust the text X position if RIGHT justified.
                if (_frmMain.LAB_Name_Align[T].Alignment == StringAlignment.Far) _frmMain.LAB_Name[T].Xtext = _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width;

                // R4.00 Draw the NAME shadow text.
                Cx = (int) Math.Round(Xoff + _frmMain.LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - TextHgt12);
                if ((_frmMain.LSName.ShadowX != 0) | (_frmMain.LSName.ShadowY != 0))
                {
                    if (_frmMain.chkCountry.Checked)
                    {
                        // R4.50 Normal X layout style.
                        if (Conversion.Val(_frmMain.cboLayoutX.Text) < 10d)
                        {
                            _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow,
                                (float) (Cx + 18 + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth)),
                                (float) (Cy + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), _frmMain.LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                                _frmMain.Main_Gfx.FillRectangle(tBrushShadow,
                                    (float) (Cx + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth) + 4d),
                                    (float) (Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), 16f, 11f);
                        }
                        // R4.50 Centered X layout style.
                        else if (Conversions.ToBoolean(T % 2))
                        {
                            // R4.46 ODD players.
                            _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow,
                                (float) (Cx - 19 + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth)),
                                (float) (Cy + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), _frmMain.LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                                _frmMain.Main_Gfx.FillRectangle(tBrushShadow,
                                    (float) (Cx + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth) - 20d),
                                    (float) (Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), 16f, 11f);
                        }
                        else
                        {
                            // R4.50 Even players.
                            _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow,
                                (float) (Cx + 19 + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth)),
                                (float) (Cy + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), _frmMain.LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                                _frmMain.Main_Gfx.FillRectangle(tBrushShadow,
                                    (float) (Cx + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth) + 4d),
                                    (float) (Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), 16f, 11f);
                        }
                    }
                    else
                    {
                        _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow,
                            (float) (Cx + _frmMain.LSName.ShadowX * Conversion.Val(_frmMain.LSName.ShadowDepth)),
                            (float) (Cy + _frmMain.LSName.ShadowY * Conversion.Val(_frmMain.LSName.ShadowDepth)), _frmMain.LAB_Name_Align[T]);
                    }
                }

                // R3.00 Setup the LINEAR brush for NAME text.
                if (_frmMain.LSName.FDir == 0)
                    linGrBrush = new LinearGradientBrush(
                        new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Ycenter - TextHgt12)),
                        new Point(Xoff + 0, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Ycenter + TextHgt12)),
                        Color.FromArgb(255, _frmMain.LSName.F1.R, _frmMain.LSName.F1.G, _frmMain.LSName.F1.B),
                        Color.FromArgb(255, _frmMain.LSName.F2.R, _frmMain.LSName.F2.G, _frmMain.LSName.F2.B));
                // R3.40 Reverse gradient if drawing RIGHT justified text.
                else if (_frmMain.LAB_Name_Align[T].Alignment == StringAlignment.Far)
                    linGrBrush = new LinearGradientBrush(
                        new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width), Yoff + 0),
                        new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X), Yoff + 0),
                        Color.FromArgb(255, _frmMain.LSName.F1.R, _frmMain.LSName.F1.G, _frmMain.LSName.F1.B),
                        Color.FromArgb(255, _frmMain.LSName.F2.R, _frmMain.LSName.F2.G, _frmMain.LSName.F2.B));
                else
                    linGrBrush = new LinearGradientBrush(new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X), Yoff + 0),
                        new Point((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width), Yoff + 0),
                        Color.FromArgb(255, _frmMain.LSName.F1.R, _frmMain.LSName.F1.G, _frmMain.LSName.F1.B),
                        Color.FromArgb(255, _frmMain.LSName.F2.R, _frmMain.LSName.F2.G, _frmMain.LSName.F2.B));

                // R3.00 Draw the NAME Text.
                Cx = (int) Math.Round(Xoff + _frmMain.LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - TextHgt12);
                if (_frmMain.chkCountry.Checked)
                {
                    // R4.50 Normal X layout style.
                    if (Conversion.Val(_frmMain.cboLayoutX.Text) < 10d)
                    {
                        _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, linGrBrush, Cx + 18, Cy, _frmMain.LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                        {
                            var BM_Country = Resources.ResourceManager.GetObject("flag_" + _frmMain.PlrCountry[T]) as Bitmap;
                            if (!Information.IsNothing(BM_Country))
                                _frmMain.Main_Gfx.DrawImage(BM_Country,
                                    new Point(Cx + 4, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f)));
                        }
                    }
                    // R4.46 Centered X layout style.
                    else if (Conversions.ToBoolean(T % 2))
                    {
                        // R4.50 ODD players.
                        _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, linGrBrush, Cx - 19, Cy, _frmMain.LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                        {
                            var BM_Country = Resources.ResourceManager.GetObject("flag_" + _frmMain.PlrCountry[T]) as Bitmap;
                            if (!Information.IsNothing(BM_Country))
                                _frmMain.Main_Gfx.DrawImage(BM_Country,
                                    new Point(Cx - 20, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f)));
                        }
                    }
                    else
                    {
                        // R4.50 Even players.
                        _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, linGrBrush, Cx + 19, Cy, _frmMain.LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                        {
                            var BM_Country = Resources.ResourceManager.GetObject("flag_" + _frmMain.PlrCountry[T]) as Bitmap;
                            if (!Information.IsNothing(BM_Country))
                                _frmMain.Main_Gfx.DrawImage(BM_Country,
                                    new Point(Cx + 4, (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f)));
                        }
                    }
                }
                else
                {
                    _frmMain.Main_Gfx.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, linGrBrush, Cx, Cy, _frmMain.LAB_Name_Align[T]);
                }

                // R3.00 Clear the clipping area.
                _frmMain.Main_Gfx.Clip = new Region(new Rectangle(0, 0, _frmMain.Main_BM.Width, _frmMain.Main_BM.Height));
            }

            // *****************************************************************
            // R3.10 Call any foreground based FX MODE options like SHADOW, etc
            // *****************************************************************
            if (_frmMain.CFX3DActive[(int) clsGlobal.FXMode.Emboss]) GFX_FX_Emboss(_frmMain.Main_BM, Xoff, Yoff);


            // *****************************************************************
            // R4.00 Draw the OVERLAY image.
            // *****************************************************************
            if (_frmMain.NameOvlBmp is object)
                // R4.00 Scale and Draw the background image.
                switch (Conversion.Val(_frmMain.LSName.OVLScaling))
                {
                    case 0d: // R4.00 Normal Scaling
                    {
                        _frmMain.Main_Gfx.DrawImage(_frmMain.NameOvlBmp, 0, 0, _frmMain.NameOvlBmp.Width, _frmMain.NameOvlBmp.Height);
                        break;
                    }

                    case 1d: // R4.00 Tiled Scaling
                    {
                        var loopTo = _frmMain.Main_BM.Height;
                        for (tY = 0; _frmMain.NameOvlBmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += _frmMain.NameOvlBmp.Height)
                        {
                            var loopTo1 = _frmMain.Main_BM.Width;
                            for (tX = 0;
                                _frmMain.NameOvlBmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1;
                                tX += _frmMain.NameOvlBmp.Width)
                                _frmMain.Main_Gfx.DrawImage(_frmMain.NameOvlBmp, tX, tY, _frmMain.NameOvlBmp.Width, _frmMain.NameOvlBmp.Height);
                        }

                        break;
                    }

                    case 2d: // R4.00 Stretch/Fit Scaling
                    {
                        // R4.00 MS sux.
                        if ((_frmMain.NameOvlBmp.Width < _frmMain.Main_BM.Width) | (_frmMain.NameOvlBmp.Height < _frmMain.Main_BM.Height))
                        {
                            var OvlXoff = (float) (_frmMain.Main_BM.Width / (double) _frmMain.NameOvlBmp.Width);
                            var OvlYoff = (float) (_frmMain.Main_BM.Height / (double) _frmMain.NameOvlBmp.Height);
                            _frmMain.Main_Gfx.DrawImage(_frmMain.NameOvlBmp, 0f, 0f, _frmMain.Main_BM.Width + OvlXoff, _frmMain.Main_BM.Height + OvlYoff);
                        }
                        else
                        {
                            _frmMain.Main_Gfx.DrawImage(_frmMain.NameOvlBmp, 0, 0, _frmMain.Main_BM.Width, _frmMain.Main_BM.Height);
                        }

                        break;
                    }
                }
        }

        public void GFX_DrawStats()
        {
            // R4.32 Adjust our working BMP if needed.
            _frmMain.STATS_AdjustSize();
            if ((_frmMain.Main_BM.Width != _frmMain.pbStats.Width) | (_frmMain.Main_BM.Height != _frmMain.pbStats.Height))
            {
                _frmMain.MainBuffer_Valid = false;
                _frmMain.Main_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                _frmMain.Main_Gfx = Graphics.FromImage(_frmMain.Main_BM);
            }

            _frmMain.Main_Gfx.Clear(_frmMain.LSName.BackC);

            // R4.50 Buffers are not valid, redraw ALL buffer images.
            if (_frmMain.MainBuffer_Valid == false)
            {
                _frmMain.MainBlur_Valid = false;
                _frmMain.MainBuffer1_Valid = false;
                _frmMain.MainBuffer2_Valid = false;
                _frmMain.MainBuffer3_Valid = false;
            }

            // R4.50 If we have stored versions skip the drawing code and show buffered versions.
            // R4.50 Are we using ELO cycle. If yes there are three modes Rank, Level, %.
            if (_frmMain.FLAG_EloUse == false)
            {
                if (_frmMain.MainBuffer1_Valid)
                {
                    _frmMain.Main_Gfx.DrawImage(_frmMain.MainBuffer1_BM, 0, 0);
                    _frmMain.pbStats.Image = _frmMain.Main_BM;
                    return;
                }
            }
            else
            {
                if (_frmMain.MainBuffer1_Valid & (_frmMain.RankDisplayMode == RankDisplayMode.Rank))
                {
                    _frmMain.Main_Gfx.DrawImage(_frmMain.MainBuffer1_BM, 0, 0);
                    _frmMain.pbStats.Image = _frmMain.Main_BM;
                    return;
                }

                if (_frmMain.MainBuffer2_Valid & (_frmMain.RankDisplayMode == RankDisplayMode.Elo))
                {
                    _frmMain.Main_Gfx.DrawImage(_frmMain.MainBuffer2_BM, 0, 0);
                    _frmMain.pbStats.Image = _frmMain.Main_BM;
                    return;
                }

                if (_frmMain.MainBuffer3_Valid & (_frmMain.RankDisplayMode == RankDisplayMode.Lvl))
                {
                    _frmMain.Main_Gfx.DrawImage(_frmMain.MainBuffer3_BM, 0, 0);
                    _frmMain.pbStats.Image = _frmMain.Main_BM;
                    return;
                }
            }

            // R4.50 If the current background image is valid use it and skip background drawing else build background image.
            if (_frmMain.MainBuffer_Valid)
                _frmMain.Main_Gfx.DrawImage(_frmMain.MainBuffer_BM, 0, 0);
            else
                GFX_BuildStatsBackground();

            GFX_BuildStatsForeground();


            // R4.50 Store the rendered STATS images so we can skip draws on the next Draw/ELO cycle.
            // R4.50 Are we using ELO cycle. If yes there are three modes Rank, Level, %.
            if (_frmMain.FLAG_EloUse == false)
            {
                if (_frmMain.MainBuffer1_Valid == false)
                {
                    _frmMain.MainBuffer1_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                    _frmMain.MainBuffer1_Gfx = Graphics.FromImage(_frmMain.MainBuffer1_BM);
                    _frmMain.MainBuffer1_Gfx.DrawImage(_frmMain.Main_BM, 0, 0);
                    _frmMain.MainBuffer1_Valid = true;
                }
            }
            else
            {
                if ((_frmMain.RankDisplayMode == RankDisplayMode.Rank) & (_frmMain.MainBuffer1_Valid == false))
                {
                    _frmMain.MainBuffer1_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                    _frmMain.MainBuffer1_Gfx = Graphics.FromImage(_frmMain.MainBuffer1_BM);
                    _frmMain.MainBuffer1_Gfx.DrawImage(_frmMain.Main_BM, 0, 0);
                    _frmMain.MainBuffer1_Valid = true;
                }

                if ((_frmMain.RankDisplayMode == RankDisplayMode.Elo) & (_frmMain.MainBuffer2_Valid == false))
                {
                    _frmMain.MainBuffer2_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                    _frmMain.MainBuffer2_Gfx = Graphics.FromImage(_frmMain.MainBuffer2_BM);
                    _frmMain.MainBuffer2_Gfx.DrawImage(_frmMain.Main_BM, 0, 0);
                    _frmMain.MainBuffer2_Valid = true;
                }

                if ((_frmMain.RankDisplayMode == RankDisplayMode.Lvl) & (_frmMain.MainBuffer3_Valid == false))
                {
                    _frmMain.MainBuffer3_BM = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                    _frmMain.MainBuffer3_Gfx = Graphics.FromImage(_frmMain.MainBuffer3_BM);
                    _frmMain.MainBuffer3_Gfx.DrawImage(_frmMain.Main_BM, 0, 0);
                    _frmMain.MainBuffer3_Valid = true;
                }
            }

            // R4.30 Put the rendered bitmap into the STATS image.
            _frmMain.pbStats.Image = _frmMain.Main_BM;
        }

        private void GFX_FX_LabBlur(Graphics Gfx, Bitmap BM, int Xoff, int Yoff)
        {
            // R4.10 OFFSET values added.
            // R3.20  Lock the bitmap's bits.
            var rect = new Rectangle(0, 0, BM.Width, BM.Height);
            var bmpData = BM.LockBits(rect, ImageLockMode.ReadWrite, BM.PixelFormat);
            var ptr = bmpData.Scan0; // Get the address of the first line.
            IntPtr ptrMask;
            var Stride = bmpData.Stride;
            var bytes = BM.Width * BM.Height * 4; // Declare an array to hold the bytes of the bitmap.
            var rgbValues = new byte[bytes]; // This code is specific to a bitmap with 32 bits per pixels.
            var rgbMask = new byte[bytes]; // R4.50 Added.
            int t;
            float BlurBias;
            float Blur;

            // R4.50 See if our RANK Bitmap MASK is valid. If not draw the mask.
            if (((3 < _frmMain.LSRank.BorderMode) | (3 < _frmMain.LSName.BorderMode)) & !_frmMain.MainBlur_Valid)
            {
                _frmMain.MainBlur_Valid = true;
                _frmMain.MainBlur_BM = new Bitmap(BM.Width, BM.Height);
                _frmMain.MainBlur_Gfx = Graphics.FromImage(_frmMain.MainBlur_BM);
                _frmMain.MainBlur_Gfx.Clear(Color.Black);
                var tBrush = new SolidBrush(Color.White);
                for (t = 1; t <= 8; t++)
                {
                    switch (_frmMain.LSRank.BorderMode)
                    {
                        case 4: // R4.50 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(_frmMain.MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[t].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[t].Y), (int) Math.Round(_frmMain.LAB_Rank[t].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[t].Height)), tBrush, 4);
                            break;
                        }

                        case 5: // R4.50 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(_frmMain.MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[t].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[t].Y), (int) Math.Round(_frmMain.LAB_Rank[t].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[t].Height)), tBrush, 8);
                            break;
                        }

                        case 6: // R4.50 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(_frmMain.MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[t].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[t].Y), (int) Math.Round(_frmMain.LAB_Rank[t].Width),
                                    (int) Math.Round(_frmMain.LAB_Rank[t].Height)), tBrush);
                            break;
                        }

                        default:
                        {
                            _frmMain.MainBlur_Gfx.FillRectangle(tBrush, Xoff + _frmMain.LAB_Rank[t].X, Yoff + _frmMain.LAB_Rank[t].Y, _frmMain.LAB_Rank[t].Width, _frmMain.LAB_Rank[t].Height);
                            break;
                        }
                    }

                    switch (_frmMain.LSName.BorderMode)
                    {
                        case 4: // R4.50 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(_frmMain.MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[t].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[t].Y), (int) Math.Round(_frmMain.LAB_Name[t].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[t].Height)), tBrush, 4);
                            break;
                        }

                        case 5: // R4.50 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(_frmMain.MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[t].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[t].Y), (int) Math.Round(_frmMain.LAB_Name[t].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[t].Height)), tBrush, 8);
                            break;
                        }

                        case 6: // R4.50 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(_frmMain.MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[t].X),
                                    (int) Math.Round(Yoff + _frmMain.LAB_Name[t].Y), (int) Math.Round(_frmMain.LAB_Name[t].Width),
                                    (int) Math.Round(_frmMain.LAB_Name[t].Height)), tBrush);
                            break;
                        }

                        default:
                        {
                            _frmMain.MainBlur_Gfx.FillRectangle(tBrush, Xoff + _frmMain.LAB_Name[t].X, Yoff + _frmMain.LAB_Name[t].Y, _frmMain.LAB_Name[t].Width, _frmMain.LAB_Name[t].Height);
                            break;
                        }
                    }
                }
            }

            // R4.50 If doing borders we need to get to the locked data.
            if ((3 < _frmMain.LSRank.BorderMode) | (3 < _frmMain.LSName.BorderMode))
            {
                _frmMain.MainBlur_Data = _frmMain.MainBlur_BM.LockBits(new Rectangle(0, 0, BM.Width, BM.Height), ImageLockMode.ReadWrite,
                    BM.PixelFormat);
                ptrMask = _frmMain.MainBlur_Data.Scan0; // R4.50  Get the address of the first line.
                Marshal.Copy(ptrMask, rgbMask, 0, bytes);
            }


            // R3.40 Setup the BLUR BIAS value.
            BlurBias = 1 < Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeBias])
                ? (float) (1d +
                           Conversion.Val(Strings.Mid(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeBias], 1,
                               Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.LabelBlur,
                                   (int) clsGlobal.FXVarDefs.ShadeBias]) - 1)) * 0.01d)
                : 1f;

            if (BlurBias < 1f) BlurBias = 1f;

            if (1.1d < BlurBias) BlurBias = 1.1f;

            BlurBias = 1f + (BlurBias - 1f) * 4f; // R3.40 Reworked BLUR routines so Bias needs to be bigger.

            // R3.40 Setup the BLUR amount.
            Blur = 1 < Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeAmount])
                ? (float) (Conversion.Val(Strings.Mid(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeAmount], 1,
                    Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeAmount]) -
                    1)) * 0.01d)
                : 0.4f;

            if (Blur == 0f) Blur = 0.8f;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);


            // R4.50 Blur the rectangles. This needs updated for new curved borders.
            for (t = 1; t <= 8; t++)
                if (!_frmMain.FLAG_HideMissing | !string.IsNullOrEmpty(_frmMain.PlrName[t]) | !string.IsNullOrEmpty(_frmMain.PlrSteam[t]))
                {
                    if (_frmMain.LSRank.BorderMode < 4)
                        FX_BlurRect(ref rgbValues, Stride, Blur, BlurBias, (int) Math.Round(Xoff + _frmMain.LAB_Rank[t].X),
                            (int) Math.Round(Yoff + _frmMain.LAB_Rank[t].Y), (int) Math.Round(_frmMain.LAB_Rank[t].Width),
                            (int) Math.Round(_frmMain.LAB_Rank[t].Height));
                    else
                        FX_BlurRect_Bordered(ref rgbValues, ref rgbMask, Stride, Blur, BlurBias,
                            (int) Math.Round(Xoff + _frmMain.LAB_Rank[t].X), (int) Math.Round(Yoff + _frmMain.LAB_Rank[t].Y),
                            (int) Math.Round(_frmMain.LAB_Rank[t].Width), (int) Math.Round(_frmMain.LAB_Rank[t].Height));

                    if (_frmMain.LSName.BorderMode < 4)
                        FX_BlurRect(ref rgbValues, Stride, Blur, BlurBias, (int) Math.Round(Xoff + _frmMain.LAB_Name[t].X),
                            (int) Math.Round(Yoff + _frmMain.LAB_Name[t].Y), (int) Math.Round(_frmMain.LAB_Name[t].Width),
                            (int) Math.Round(_frmMain.LAB_Name[t].Height));
                    else
                        FX_BlurRect_Bordered(ref rgbValues, ref rgbMask, Stride, Blur, BlurBias,
                            (int) Math.Round(Xoff + _frmMain.LAB_Name[t].X), (int) Math.Round(Yoff + _frmMain.LAB_Name[t].Y),
                            (int) Math.Round(_frmMain.LAB_Name[t].Width), (int) Math.Round(_frmMain.LAB_Name[t].Height));
                }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);
            if ((3 < _frmMain.LSRank.BorderMode) | (3 < _frmMain.LSName.BorderMode)) _frmMain.MainBlur_BM.UnlockBits(_frmMain.MainBlur_Data);

            BM.UnlockBits(bmpData);

            // Draw the modified image.
            Gfx.DrawImage(BM, 0, 0);
        }

        private void GFX_FX_Shadow(Graphics Gfx, int Xoff, int Yoff)
        {
            // R4.10 OFFSET values added.
            PictureBox tPic;
            int tLabHgt;
            int Cx, Cy, Cx2, Cy2;
            var linGrBrush = new LinearGradientBrush(new Point(0, 0), new Point(20, 0), Color.FromArgb(255, 255, 0, 0),
                Color.FromArgb(255, 0, 0, 255));
            int TextHgt12;
            int Idx;
            var tString = ""; // R4.30 Added.

            // R3.10 Precalc some vars for readability in code.
            tLabHgt = (int) ((long) Math.Round(_frmMain.LAB_Rank[1].Height) / 2L);

            // R4.32 Adjust our working BMP if needed.
            if ((_frmMain.Main_BM2.Width != _frmMain.pbStats.Width) | (_frmMain.Main_BM2.Height != _frmMain.pbStats.Height))
            {
                _frmMain.Main_BM2 = new Bitmap(_frmMain.pbStats.Width, _frmMain.pbStats.Height);
                _frmMain.Main_Gfx2 = Graphics.FromImage(_frmMain.Main_BM2);
            }

            _frmMain.Main_Gfx2.Clear(Color.FromArgb(0, 0, 0, 0));

            // *****************************************************************
            // R3.10 Draw the Faction images to the stats page.
            // ***************************************************************** 
            // R3.20 RemovedDim POP(0 To 8) As Integer
            // R3.20 Removed If 0 < GUI_Mouse_PlrIndex Then POP(GUI_Mouse_PlrIndex) = LAB_Fact(1).Height / 6

            for (var t = 1; t <= 8; t++)
                if (!string.IsNullOrEmpty(_frmMain.PlrFact[t]))
                {
                    tPic = (PictureBox) _frmMain.Controls["pbFact" + _frmMain.PlrFact[t]];
                    // R3.20 Removed Gfx2.DrawImage(tPic.Image, LAB_Fact(t).X - POP(t), LAB_Fact(t).Y - POP(t), LAB_Fact(t).Width + POP(t) * 2, LAB_Fact(t).Height + POP(t) * 2)
                    _frmMain.Main_Gfx2.DrawImage(tPic.Image, Xoff + _frmMain.LAB_Fact[t].X, Yoff + _frmMain.LAB_Fact[t].Y, _frmMain.LAB_Fact[t].Width, _frmMain.LAB_Fact[t].Height);
                }

            // *****************************************************************
            // R3.00 Paint a blurred shadow.
            // *****************************************************************
            var tBrushShadow = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            for (var T = 1; T <= 8; T++)
            {
                if (_frmMain.FLAG_EloUse == false)
                {
                    tString = _frmMain.PlrRank[T];
                }
                else
                {
                    if (_frmMain.RankDisplayMode == RankDisplayMode.Rank) tString = _frmMain.PlrRank[T];

                    if (_frmMain.RankDisplayMode == RankDisplayMode.Elo) tString = _frmMain.PlrELO[T];

                    if (_frmMain.RankDisplayMode == RankDisplayMode.Lvl) tString = _frmMain.PlrLVL[T];
                }

                // R3.00 Create a clipping area so names do not draw past the rectangle.
                _frmMain.Main_Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X + 1f),
                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width - 2f),
                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)));

                // R3.00 Draw the RANK Shadow text.
                Cx = (int) Math.Round(_frmMain.LAB_Rank[T].X + 2f + _frmMain.LAB_Rank[T].Width / 2f - _frmMain.Main_Gfx2.MeasureString(tString, frmMain.FONT_Rank).Width / 2f);
                Cy = (int) Math.Round(_frmMain.LAB_Rank[T].Y + 2f + _frmMain.LAB_Rank[T].Height / 2f - _frmMain.Main_Gfx2.MeasureString(tString, frmMain.FONT_Rank).Height / 2f);
                switch (_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] ?? "")
                {
                    case "45°":
                    {
                        Cx2 = 0;
                        Cy2 = -4;
                        break;
                    }

                    case "90°":
                    {
                        Cx2 = -2;
                        Cy2 = -4;
                        break;
                    }

                    case "135°":
                    {
                        Cx2 = -4;
                        Cy2 = -4;
                        break;
                    }

                    case "180°":
                    {
                        Cx2 = -4;
                        Cy2 = -2;
                        break;
                    }

                    case "225°":
                    {
                        Cx2 = -4;
                        Cy2 = 0;
                        break;
                    }

                    case "270°":
                    {
                        Cx2 = -2;
                        Cy2 = 0;
                        break;
                    }

                    case "315°":
                    {
                        Cx2 = 0;
                        Cy2 = 0;
                        break;
                    }

                    case "360°":
                    {
                        Cx2 = 0;
                        Cy2 = -2;
                        break;
                    }

                    default:
                    {
                        Cx2 = 0;
                        Cy2 = 0;
                        break;
                    }
                }

                if (_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] != "--") _frmMain.Main_Gfx2.DrawString(tString, frmMain.FONT_Rank, tBrushShadow, Xoff + Cx + Cx2, Yoff + Cy + Cy2);

                // R3.00 Clear the clipping area.
                _frmMain.Main_Gfx2.Clip = new Region(new Rectangle(0, 0, _frmMain.Main_BM2.Width, _frmMain.Main_BM2.Height));
            }

            TextHgt12 = (int) Math.Round(_frmMain.Main_Gfx2.MeasureString("X", frmMain.FONT_Name).Height / 2f);
            for (var T = 1; T <= 8; T++)
            {
                // R3.00 Create a clipping area so names do not draw past the rectangle.
                _frmMain.Main_Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + 1f),
                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width - 2f),
                    (int) Math.Round(_frmMain.LAB_Name[T].Height)));

                // R3.40 Adjust the text X position if RIGHT justified.
                if (_frmMain.LAB_Name_Align[T].Alignment == StringAlignment.Far) _frmMain.LAB_Name[T].Xtext = _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width;

                Cx = (int) Math.Round(Xoff + _frmMain.LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - TextHgt12);
                switch (_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] ?? "")
                {
                    case "45°":
                    {
                        Cx2 = 2;
                        Cy2 = -2;
                        break;
                    }

                    case "90°":
                    {
                        Cx2 = 0;
                        Cy2 = -2;
                        break;
                    }

                    case "135°":
                    {
                        Cx2 = -2;
                        Cy2 = -2;
                        break;
                    }

                    case "180°":
                    {
                        Cx2 = -2;
                        Cy2 = 0;
                        break;
                    }

                    case "225°":
                    {
                        Cx2 = -2;
                        Cy2 = 2;
                        break;
                    }

                    case "270°":
                    {
                        Cx2 = 0;
                        Cy2 = 2;
                        break;
                    }

                    case "315°":
                    {
                        Cx2 = 2;
                        Cy2 = 2;
                        break;
                    }

                    case "360°":
                    {
                        Cx2 = 2;
                        Cy2 = 0;
                        break;
                    }

                    default:
                    {
                        Cx2 = 0;
                        Cy2 = 0;
                        break;
                    }
                }

                if (_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] != "--")
                {
                    if (_frmMain.chkCountry.Checked)
                    {
                        // R4.50 Normal X layout style.
                        if (Conversion.Val(_frmMain.cboLayoutX.Text) < 10d)
                        {
                            _frmMain.Main_Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx + 18 + Cx2, Cy + Cy2, _frmMain.LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                                _frmMain.Main_Gfx2.FillRectangle(tBrushShadow, Cx + 4, Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f, 16f,
                                    11f);
                        }
                        // R4.50 Centered X layout style.
                        else if (Conversions.ToBoolean(T % 2))
                        {
                            // R4.46 ODD players.
                            _frmMain.Main_Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx - 19 + Cx2, Cy + Cy2, _frmMain.LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                                _frmMain.Main_Gfx2.FillRectangle(tBrushShadow, Cx - 20 + Cx2,
                                    Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f + Cy2, 16f, 11f);
                        }
                        else
                        {
                            // R4.50 Even players.
                            _frmMain.Main_Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx + 19 + Cx2, Cy + Cy2, _frmMain.LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                                _frmMain.Main_Gfx2.FillRectangle(tBrushShadow, Cx + 4 + Cx2,
                                    Yoff + _frmMain.LAB_Name[T].Y + tLabHgt - 6f + Cy2, 16f, 11f);
                        }
                    }
                    else
                    {
                        _frmMain.Main_Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx + Cx2, Cy + Cy2, _frmMain.LAB_Name_Align[T]);
                    }
                }

                // R3.00 Clear the clipping area.
                _frmMain.Main_Gfx2.Clip = new Region(new Rectangle(0, 0, _frmMain.Main_BM2.Width, _frmMain.Main_BM2.Height));
            }

            // Lock the bitmap's bits.
            var rect = new Rectangle(0, 0, _frmMain.Main_BM2.Width, _frmMain.Main_BM2.Height);
            var bmpData = _frmMain.Main_BM2.LockBits(rect, ImageLockMode.ReadWrite, _frmMain.Main_BM2.PixelFormat);
            var ptr = bmpData.Scan0; // R3.40 Get the address of the first byte.
            var Stride = bmpData.Stride;
            var bytes = _frmMain.Main_BM2.Width * _frmMain.Main_BM2.Height * 4; // R3.40 Declare an array to hold the bytes of the bitmap.
            var rgbValues = new byte[bytes]; // R3.40 This code is specific to a bitmap with 32 bits per pixels.
            int Ca = default, Ca2;
            float Blr1, Blr2;
            int TempI;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            // R3.10 Calculate the Blur Bias value.
            float BlrFac;
            BlrFac = 1 < Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeBias])
                ? (float) (1d +
                           Conversion.Val(Strings.Mid(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeBias], 1,
                               Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow,
                                   (int) clsGlobal.FXVarDefs.ShadeBias]) - 1)) * 0.01d)
                : 1f;

            if (BlrFac < 1f) BlrFac = 1f;

            if (1.1d < BlrFac) BlrFac = 1.1f;

            BlrFac = 1f + (BlrFac - 1f) * 16f;

            // R3.10 Calculate the Blur Amount.
            Blr1 = 1 < Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAmount])
                ? (float) (Conversion.Val(Strings.Mid(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAmount], 1,
                               Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Shadow,
                                   (int) clsGlobal.FXVarDefs.ShadeAmount]) - 1)) *
                           0.01d)
                : 0.5f;

            if (Blr1 == 0f) Blr1 = 0.8f;

            Blr2 = 1f - Blr1;

            // R3.00 Fill in the SHADOW color, and BLUR the alpha channel to make it bigger or smaller.
            for (int y = 0, loopTo = _frmMain.Main_BM2.Height - 1; y <= loopTo; y++)
            {
                Idx = y * Stride;
                for (int x = 0, loopTo1 = _frmMain.Main_BM2.Width - 1; x <= loopTo1; x++)
                {
                    rgbValues[Idx] = _frmMain.CFX3DC[1].B; // R3.30 Fill in SOLID color RGB.
                    rgbValues[Idx + 1] = _frmMain.CFX3DC[1].G;
                    rgbValues[Idx + 2] = _frmMain.CFX3DC[1].R;
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3]; // R3.30 We are only BLURRING alpha channel (Idx+3).
                    Idx += 4;
                }
            }

            TempI = (_frmMain.Main_BM2.Width - 2) * 4; // R3.40 Do some precalc here.   
            for (int y = 0, loopTo2 = _frmMain.Main_BM2.Height - 2; y <= loopTo2; y++)
            {
                Idx = y * Stride + TempI;
                for (var x = _frmMain.Main_BM2.Width - 2; x >= 2; x -= 1)
                {
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3];
                    Idx -= 4;
                }
            }

            // TempI = (2 * Stride)                       'R3.40 Do some precalc here.   
            for (int x = 2, loopTo3 = _frmMain.Main_BM2.Width - 2; x <= loopTo3; x++)
            {
                Idx = x * 4;
                for (int y = 0, loopTo4 = _frmMain.Main_BM2.Height - 2; y <= loopTo4; y++)
                {
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3];
                    Idx += Stride;
                }
            }

            TempI = (_frmMain.Main_BM2.Height - 2) * Stride; // R3.40 Do some precalc here.   
            for (int x = 2, loopTo5 = _frmMain.Main_BM2.Width - 2; x <= loopTo5; x++)
            {
                Idx = TempI + x * 4;
                for (var y = _frmMain.Main_BM2.Height - 2; y >= 2; y -= 1)
                {
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3];
                    Idx -= Stride;
                }
            }


            // R3.10 Multiply the Original data times the Emboss bitmap.
            Idx = 3; // R3.40 Start on ALPHA to reduce # of adds.
            for (int Y = 0, loopTo6 = bytes - 1; Y <= loopTo6; Y += 4)
            {
                // R3.40 Apply the BIAS factor to brighten the final image.
                // R3.40 Colors get dim when blurred.
                Ca2 = (int) Math.Round(rgbValues[Idx] * BlrFac);
                if (255 < Ca2) Ca2 = 255;

                rgbValues[Idx] = (byte) Ca2;
                Idx += 4;
            }


            // R3.00 Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // R3.00 Unlock the bits so they can be used.
            _frmMain.Main_BM2.UnlockBits(bmpData);

            // R3.00 Draw the modified image.
            Gfx.DrawImage(_frmMain.Main_BM2, 0, 0);

            // R4.32 Attempt some clean up.
            linGrBrush.Dispose();
        }

        private void GFX_FX_Emboss(Bitmap BM, int Xoff, int Yoff)
        {
            // R4.10 OFFSET values added.
            PictureBox tPic;
            int LabYCenter;
            int Cx, Cy, Cx2, Cy2;
            int TextHgt12;
            int Idx;
            var tString = "";
            var BM2 = new Bitmap(BM.Width, BM.Height);
            var Gfx2 = Graphics.FromImage(BM2);

            // R3.10 Precalc some vars for readability in code.
            LabYCenter = (int) ((long) Math.Round(_frmMain.LAB_Rank[1].Height) / 2L);

            // *****************************************************************
            // R3.10 Draw the Faction images to the stats page.
            // ***************************************************************** 
            // Dim POP(0 To 8) As Integer
            // If 0 < GUI_Mouse_PlrIndex Then POP(GUI_Mouse_PlrIndex) = LAB_Fact(1).Height / 6

            for (var t = 1; t <= 8; t++)
                if (!string.IsNullOrEmpty(_frmMain.PlrFact[t]))
                {
                    tPic = (PictureBox) _frmMain.Controls["pbFact" + _frmMain.PlrFact[t]];
                    Gfx2.DrawImage(tPic.Image, Xoff + _frmMain.LAB_Fact[t].X, Yoff + _frmMain.LAB_Fact[t].Y, _frmMain.LAB_Fact[t].Width, _frmMain.LAB_Fact[t].Height);
                }

            // *****************************************************************
            // R3.00 Print the RANK and NAME text on the blank bitmap.
            // *****************************************************************
            var tBrushShadow = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            for (var T = 1; T <= 8; T++)
            {
                if (_frmMain.FLAG_EloUse == false)
                {
                    tString = _frmMain.PlrRank[T];
                }
                else
                {
                    if (_frmMain.RankDisplayMode == RankDisplayMode.Rank) tString = _frmMain.PlrRank[T];

                    if (_frmMain.RankDisplayMode == RankDisplayMode.Elo) tString = _frmMain.PlrELO[T];

                    if (_frmMain.RankDisplayMode == RankDisplayMode.Lvl) tString = _frmMain.PlrLVL[T];
                }

                // R3.00 Create a clipping area so names do not draw past the rectangle/label.
                Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X + 1f),
                    (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y), (int) Math.Round(_frmMain.LAB_Rank[T].Width - 2f),
                    (int) Math.Round(_frmMain.LAB_Rank[T].Height)));

                // R3.00 Draw the RANK Shadow text.
                Cx = (int) Math.Round(Xoff + _frmMain.LAB_Rank[T].X + 2f + _frmMain.LAB_Rank[T].Width / 2f -
                                      Gfx2.MeasureString(tString, frmMain.FONT_Rank).Width / 2f);
                Cy = (int) Math.Round(Yoff + _frmMain.LAB_Rank[T].Y + 2f + _frmMain.LAB_Rank[T].Height / 2f -
                                      Gfx2.MeasureString(tString, frmMain.FONT_Rank).Height / 2f);
                Cx2 = 0;
                Cy2 = -2;
                Gfx2.DrawString(tString, frmMain.FONT_Rank, tBrushShadow, Cx + Cx2, Cy + Cy2);

                // R3.00 Clear the clipping area.
                Gfx2.Clip = new Region(new Rectangle(0, 0, BM2.Width, BM2.Height));
            }

            TextHgt12 = (int) Math.Round(Gfx2.MeasureString("X", frmMain.FONT_Name).Height / 2f); // R3.30 Was Xq.
            for (var T = 1; T <= 8; T++)
            {
                // R3.00 Create a clipping area so names do not draw past the rectangle.
                Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + _frmMain.LAB_Name[T].X + 1f),
                    (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y), (int) Math.Round(_frmMain.LAB_Name[T].Width - 2f),
                    (int) Math.Round(_frmMain.LAB_Name[T].Height)));

                // R3.40 Adjust the text X position if RIGHT justified.
                if (_frmMain.LAB_Name_Align[T].Alignment == StringAlignment.Far) _frmMain.LAB_Name[T].Xtext = _frmMain.LAB_Name[T].X + _frmMain.LAB_Name[T].Width;

                Cx = (int) Math.Round(Xoff + _frmMain.LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + _frmMain.LAB_Name[T].Y + LabYCenter - TextHgt12);

                // R4.46 Added Country flags.
                if (_frmMain.chkCountry.Checked)
                {
                    // R4.50 Normal X layout style.
                    if (Conversion.Val(_frmMain.cboLayoutX.Text) < 10d)
                    {
                        Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx + 18, Cy, _frmMain.LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                            Gfx2.FillRectangle(tBrushShadow, Cx + 4, Yoff + _frmMain.LAB_Name[T].Y + LabYCenter - 6f, 16f, 11f);
                    }
                    // R4.46 Centered X layout style.
                    else if (Conversions.ToBoolean(T % 2))
                    {
                        // R4.50 ODD players.
                        Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx - 19, Cy, _frmMain.LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                            Gfx2.FillRectangle(tBrushShadow, Cx - 20, Yoff + _frmMain.LAB_Name[T].Y + LabYCenter - 6f, 16f, 11f);
                    }
                    else
                    {
                        // R4.50 Even players.
                        Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx + 19, Cy, _frmMain.LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(_frmMain.PlrCountry[T]))
                            Gfx2.FillRectangle(tBrushShadow, Cx + 4, Yoff + _frmMain.LAB_Name[T].Y + LabYCenter - 6f, 16f, 11f);
                    }
                }
                else
                {
                    Gfx2.DrawString(_frmMain.PlrName[T], frmMain.FONT_Name, tBrushShadow, Cx, Cy, _frmMain.LAB_Name_Align[T]);
                }

                // R3.00 Clear the clipping area.
                Gfx2.Clip = new Region(new Rectangle(0, 0, BM2.Width, BM2.Height));
            }


            // ********************************************************************************************
            // R3.10 Setup high speed memory operations.
            // ********************************************************************************************
            var rect = new Rectangle(0, 0, BM2.Width, BM2.Height);
            var bmpData = BM.LockBits(rect, ImageLockMode.ReadWrite, BM.PixelFormat);
            var bmpData2 = BM2.LockBits(rect, ImageLockMode.ReadWrite, BM2.PixelFormat);
            var ptr = bmpData.Scan0; // R3.10 Get the address of the Original data.
            var ptr2 = bmpData2.Scan0; // R3.10 Get the address of the Embossed data.
            var Stride = bmpData2.Stride;
            var bytes = BM2.Width * BM2.Height * 4; // R3.30 Declare an array to hold the bytes of the bitmap.
            var rgbValues = new byte[bytes]; // R3.30 This code is specific to a bitmap with 32 bits per pixels.
            var rgbOrg = new byte[bytes];

            // R3.10 Copy the BGRA values into arrays we can modify.
            Marshal.Copy(ptr2, rgbValues, 0, bytes);
            Marshal.Copy(ptr, rgbOrg, 0, bytes);

            // R3.10 Blur to the right.
            int Cb = default, Ca2;
            float Blr1, Blr2;

            // R3.10 Create GreyScale Embossed bitmap (315 degree) on BLUE channel only.
            // R3.30 Need options for B2 to get other Emboss angles. B2 here is X+1, Y+1.
            // R3.40 Since we read a line and pixel ahead, do not scan the entire bitmap.
            for (int y = 0, loopTo = BM2.Height - 2; y <= loopTo; y++)
            {
                // R3.40 ScanY = (y * Stride)
                Idx = y * Stride;
                for (int x = 0, loopTo1 = BM2.Width - 2; x <= loopTo1; x++)
                {
                    // R3.40 Calc EMBOSS pixels on ALPHA, modify BLUE only. 
                    switch (rgbValues[Idx + 3] - rgbValues[Idx + 3 + Stride + 4])
                    {
                        case 0:
                        {
                            rgbValues[Idx] = 128; // R3.10 CFX3DC(1).B   'User Defined color.
                            break;
                        }
                        // rgbValues(Idx + 1) = 128     'R3.10 CFX3DC(1).G
                        // rgbValues(Idx + 2) = 128     'R3.10 CFX3DC(1).R
                        case < 0:
                        {
                            rgbValues[Idx] = 255;
                            break;
                        }
                        // rgbValues(Idx + 1) = 255
                        // rgbValues(Idx + 2) = 255
                        case > 0:
                        {
                            rgbValues[Idx] = 0;
                            break;
                        }
                        // rgbValues(Idx + 1) = 0
                        // rgbValues(Idx + 2) = 0
                    }

                    Idx += 4;
                }
            }

            // ********************************************************************
            // R3.10 Blur the Embossed bitmap. Low blur works best for emboss.
            // ********************************************************************
            float BlrBias;
            BlrBias = 1 < Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeBias])
                ? (float) (1d +
                           Conversion.Val(Strings.Mid(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeBias], 1,
                               Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Emboss,
                                   (int) clsGlobal.FXVarDefs.ShadeBias]) - 1)) * 0.01d)
                : 1f;

            if (BlrBias < 1f) BlrBias = 1f;

            if (1.5d < BlrBias) BlrBias = 1.5f;

            BlrBias = 1f + (BlrBias - 1f) * 16f;
            Blr1 = 1 < Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeAmount])
                ? (float) (Conversion.Val(Strings.Mid(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeAmount], 1,
                               Strings.Len(_frmMain.CFX3DVar[(int) clsGlobal.FXMode.Emboss,
                                   (int) clsGlobal.FXVarDefs.ShadeAmount]) - 1)) *
                           0.01d)
                : 0.4f;

            if (Blr1 == 0f) Blr1 = 0.8f;

            Blr2 = 1f - Blr1;

            // ************************************************************************
            // R3.10 Smooth the EMBOSSED image.
            // R3.40 Since we are multiplying by BlrBias we need to clip values to 255.
            // R3.40 Scan the bitmap and blur four times: Right, Left, Down, then Up.
            // ************************************************************************
            int TempI;
            for (int y = 2, loopTo2 = BM2.Height - 2; y <= loopTo2; y++)
            {
                Idx = y * Stride + 8; // R3.40 (X * 4)
                for (int x = 2, loopTo3 = BM2.Width - 2; x <= loopTo3; x++)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    Idx += 4;
                }
            }

            TempI = (BM2.Width - 2) * 4;
            for (int y = 2, loopTo4 = BM2.Height - 2; y <= loopTo4; y++)
            {
                Idx = y * Stride + TempI;
                for (var x = BM2.Width - 2; x >= 2; x -= 1)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    Idx -= 4;
                }
            }

            for (int x = 2, loopTo5 = BM2.Width - 2; x <= loopTo5; x++)
            {
                Idx = 2 * Stride + x * 4;
                for (int y = 2, loopTo6 = BM2.Height - 2; y <= loopTo6; y++)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    Idx += Stride;
                }
            }

            TempI = (BM2.Height - 2) * Stride;
            for (int x = 2, loopTo7 = BM2.Width - 2; x <= loopTo7; x++)
            {
                Idx = TempI + x * 4;
                for (var y = BM2.Height - 2; y >= 2; y -= 1)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    Idx -= Stride;
                }
            }

            // R4.50 Multiply the Original data times the Emboss bitmap.
            float tFac;
            Idx = 0;
            for (int Y = 0, loopTo8 = bytes - 1; Y <= loopTo8; Y += 4)
            {
                // R3.40 Apply the BIAS factor to brighten the final image.
                // R3.40 Colors get dim when blurred.
                Ca2 = (int) Math.Round(rgbValues[Idx] * BlrBias);
                if (255 < Ca2) Ca2 = 255;

                rgbValues[Idx] = (byte) Ca2;

                // R4.50 Added tFac for possible optimization.
                tFac = (float) (Ca2 / 255.0d);
                rgbOrg[Idx] = (byte) Math.Round(rgbOrg[Idx] * tFac);
                rgbOrg[Idx + 1] = (byte) Math.Round(rgbOrg[Idx + 1] * tFac); // R3.10 Only use BLUE from Emboss map.
                rgbOrg[Idx + 2] = (byte) Math.Round(rgbOrg[Idx + 2] * tFac); // R3.10 Only use BLUE from Emboss map.
                Idx += 4;
            }

            // R3.10 Copy the original (now modified) RGB values back to the bitmap memory. It gets drawn in caller.
            Marshal.Copy(rgbOrg, 0, ptr, bytes);

            // R3.10 Unlock the bitmap memory so it can be used.
            BM2.UnlockBits(bmpData2);
            BM.UnlockBits(bmpData);
            Application.DoEvents();

            // R3.10 Draw the modified value Bitmap. (Not used here, we are modifying the original bitmap.)
            // Gfx.DrawImage(BM, 0, 0)

            // R4.32 Added.
            BM2.Dispose();
            Gfx2.Dispose();
        }

        private void FX_BlurRect(ref byte[] rgbValues, int Stride, float BlurAmount, float BlurBias, int Left, int Top,
            int Width, int Height)
        {
            // *******************************************************************************
            // R3.20 ADDED this sub to blur ractangle areas in rgbValues (Bitmap data).
            // R3.20 Uses a criss/cross weighted average.
            // *******************************************************************************
            float Blr1, Blr2;
            int TempI;
            int Idx;
            int Cr = default, Cg = default, Cb = default; // R4.500 Removed ALPHA from this sub.   
            int tC;
            Blr1 = BlurAmount; // R3.20 How blurry are we 0 - 1.00.
            Blr2 = 1f - Blr1; // R3.20 Inverse amount of blur.

            // R4.10 ADDED checks for new OFFSET code.
            if (_frmMain.pbStats.Width < Left) return;

            if (_frmMain.pbStats.Height < Top) return;

            if (Left < 0) Left = 0;

            if (Top < 0) Top = 0;

            if (_frmMain.pbStats.Width < Left + Width) Width = _frmMain.pbStats.Width - Left;

            if (_frmMain.pbStats.Height < Top + Height) Height = _frmMain.pbStats.Height - Top;

            // R3.30 BLUR the selected rectangle area.
            // R3.40 Added -1 to Height and Width calcs.
            TempI = Left * 4;
            for (int y = Top, loopTo = Top + Height - 1; y <= loopTo; y++)
            {
                Idx = y * Stride + TempI;
                for (int x = Left, loopTo1 = Left + Width - 1; x <= loopTo1; x++)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    rgbValues[Idx + 1] = (byte) Math.Round(Cg * Blr1 + rgbValues[Idx + 1] * Blr2);
                    Cg = rgbValues[Idx + 1];
                    rgbValues[Idx + 2] = (byte) Math.Round(Cr * Blr1 + rgbValues[Idx + 2] * Blr2);
                    Cr = rgbValues[Idx + 2];
                    Idx += 4;
                }
            }

            TempI = (Left + Width - 1) * 4;
            for (int y = Top, loopTo2 = Top + Height - 1; y <= loopTo2; y++)
            {
                Idx = y * Stride + TempI;
                for (int x = Left + Width - 1, loopTo3 = Left; x >= loopTo3; x -= 1)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    rgbValues[Idx + 1] = (byte) Math.Round(Cg * Blr1 + rgbValues[Idx + 1] * Blr2);
                    Cg = rgbValues[Idx + 1];
                    rgbValues[Idx + 2] = (byte) Math.Round(Cr * Blr1 + rgbValues[Idx + 2] * Blr2);
                    Cr = rgbValues[Idx + 2];
                    Idx -= 4;
                }
            }

            TempI = Stride * Top;
            for (int x = Left, loopTo4 = Left + Width - 1; x <= loopTo4; x++)
            {
                Idx = TempI + x * 4;
                for (int y = Top, loopTo5 = Top + Height - 1; y <= loopTo5; y++)
                {
                    rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                    Cb = rgbValues[Idx];
                    rgbValues[Idx + 1] = (byte) Math.Round(Cg * Blr1 + rgbValues[Idx + 1] * Blr2);
                    Cg = rgbValues[Idx + 1];
                    rgbValues[Idx + 2] = (byte) Math.Round(Cr * Blr1 + rgbValues[Idx + 2] * Blr2);
                    Cr = rgbValues[Idx + 2];
                    Idx += Stride;
                }
            }

            // R3.40 On the last pass, apply the BIAS value to brighten the blur.
            TempI = Stride * (Top + Height - 1);
            for (int x = Left, loopTo6 = Left + Width - 1; x <= loopTo6; x++)
            {
                Idx = TempI + x * 4;
                for (int y = Top + Height - 1, loopTo7 = Top; y >= loopTo7; y -= 1)
                {
                    tC = (int) Math.Round(Conversion.Int(Cb * Blr1 + rgbValues[Idx] * Blr2) * BlurBias);
                    if (255 < tC) tC = 255;

                    rgbValues[Idx] = (byte) tC;
                    Cb = rgbValues[Idx];
                    tC = (int) Math.Round(Conversion.Int(Cg * Blr1 + rgbValues[Idx + 1] * Blr2) * BlurBias);
                    if (255 < tC) tC = 255;

                    rgbValues[Idx + 1] = (byte) tC;
                    Cg = rgbValues[Idx + 1];
                    tC = (int) Math.Round(Conversion.Int(Cr * Blr1 + rgbValues[Idx + 2] * Blr2) * BlurBias);
                    if (255 < tC) tC = 255;

                    rgbValues[Idx + 2] = (byte) tC;
                    Cr = rgbValues[Idx + 2];
                    Idx -= Stride;
                }
            }
        }

        private void FX_BlurRect_Bordered(ref byte[] rgbValues, ref byte[] rgbMask, int Stride, float BlurAmount,
            float BlurBias, int Left, int Top, int Width, int Height)
        {
            // *******************************************************************************
            // R4.50 ADDED this sub to blur using a mask (B/W) bitmap of the same size.
            // *******************************************************************************
            float Blr1, Blr2;
            int TempI;
            int Idx;
            int Cr = default, Cg = default, Cb = default;
            int tC;
            Blr1 = BlurAmount; // R4.50 How blurry are we 0 - 1.00.
            Blr2 = 1f - Blr1; // R4.50 Inverse amount of blur.

            // R4.50 ADDED checks for new OFFSET code.
            if (_frmMain.pbStats.Width < Left) return;

            if (_frmMain.pbStats.Height < Top) return;

            if (Left < 0) Left = 0;

            if (Top < 0) Top = 0;

            if (_frmMain.pbStats.Width < Left + Width) Width = _frmMain.pbStats.Width - Left;

            if (_frmMain.pbStats.Height < Top + Height) Height = _frmMain.pbStats.Height - Top;


            // R4.50 BLUR the selected rectangle area.
            // R4.50 Added -1 to Height and Width calcs.
            TempI = Left * 4;
            for (int y = Top, loopTo = Top + Height - 1; y <= loopTo; y++)
            {
                Idx = y * Stride + TempI;
                for (int x = Left, loopTo1 = Left + Width - 1; x <= loopTo1; x++)
                {
                    if (Conversions.ToBoolean(rgbMask[Idx]))
                    {
                        rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                        Cb = rgbValues[Idx];
                        rgbValues[Idx + 1] = (byte) Math.Round(Cg * Blr1 + rgbValues[Idx + 1] * Blr2);
                        Cg = rgbValues[Idx + 1];
                        rgbValues[Idx + 2] = (byte) Math.Round(Cr * Blr1 + rgbValues[Idx + 2] * Blr2);
                        Cr = rgbValues[Idx + 2];
                    }

                    Idx += 4;
                }
            }

            TempI = (Left + Width - 1) * 4;
            for (int y = Top, loopTo2 = Top + Height - 1; y <= loopTo2; y++)
            {
                Idx = y * Stride + TempI;
                for (int x = Left + Width - 1, loopTo3 = Left; x >= loopTo3; x -= 1)
                {
                    if (Conversions.ToBoolean(rgbMask[Idx]))
                    {
                        rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                        Cb = rgbValues[Idx];
                        rgbValues[Idx + 1] = (byte) Math.Round(Cg * Blr1 + rgbValues[Idx + 1] * Blr2);
                        Cg = rgbValues[Idx + 1];
                        rgbValues[Idx + 2] = (byte) Math.Round(Cr * Blr1 + rgbValues[Idx + 2] * Blr2);
                        Cr = rgbValues[Idx + 2];
                    }

                    Idx -= 4;
                }
            }

            TempI = Stride * Top;
            for (int x = Left, loopTo4 = Left + Width - 1; x <= loopTo4; x++)
            {
                Idx = TempI + x * 4;
                for (int y = Top, loopTo5 = Top + Height - 1; y <= loopTo5; y++)
                {
                    if (Conversions.ToBoolean(rgbMask[Idx]))
                    {
                        rgbValues[Idx] = (byte) Math.Round(Cb * Blr1 + rgbValues[Idx] * Blr2);
                        Cb = rgbValues[Idx];
                        rgbValues[Idx + 1] = (byte) Math.Round(Cg * Blr1 + rgbValues[Idx + 1] * Blr2);
                        Cg = rgbValues[Idx + 1];
                        rgbValues[Idx + 2] = (byte) Math.Round(Cr * Blr1 + rgbValues[Idx + 2] * Blr2);
                        Cr = rgbValues[Idx + 2];
                    }

                    Idx += Stride;
                }
            }

            // R4.50 On the last pass, apply the BIAS value to brighten the blur.
            TempI = Stride * (Top + Height - 1);
            for (int x = Left, loopTo6 = Left + Width - 1; x <= loopTo6; x++)
            {
                Idx = TempI + x * 4;
                for (int y = Top + Height - 1, loopTo7 = Top; y >= loopTo7; y -= 1)
                {
                    if (Conversions.ToBoolean(rgbMask[Idx]))
                    {
                        tC = (int) Math.Round(Conversion.Int(Cb * Blr1 + rgbValues[Idx] * Blr2) * BlurBias);
                        if (255 < tC) tC = 255;

                        rgbValues[Idx] = (byte) tC;
                        Cb = rgbValues[Idx];
                        tC = (int) Math.Round(Conversion.Int(Cg * Blr1 + rgbValues[Idx + 1] * Blr2) * BlurBias);
                        if (255 < tC) tC = 255;

                        rgbValues[Idx + 1] = (byte) tC;
                        Cg = rgbValues[Idx + 1];
                        tC = (int) Math.Round(Conversion.Int(Cr * Blr1 + rgbValues[Idx + 2] * Blr2) * BlurBias);
                        if (255 < tC) tC = 255;

                        rgbValues[Idx + 2] = (byte) tC;
                        Cr = rgbValues[Idx + 2];
                    }

                    Idx -= Stride;
                }
            }
        }

        private void DrawRoundedRectangle(Graphics Graphics, Rectangle Rectangle, Pen Pen, int radius)
        {
            using var path = RoundedRectangle(Rectangle, radius);
            Graphics.DrawPath(Pen, path);
        }

        private void DrawRoundedRectangle_Max(Graphics Graphics, Rectangle Rectangle, Pen Pen)
        {
            using var path = RoundedRectangle_Max(Rectangle);
            Graphics.DrawPath(Pen, path);
        }

        private void FillRoundedRectangle_Max(Graphics Graphics, Rectangle Rectangle, Brush Brush)
        {
            using var path = RoundedRectangle_Max(Rectangle);
            Graphics.FillPath(Brush, path);
        }

        private void FillRoundedRectangle(Graphics Graphics, Rectangle Rectangle, Brush Brush, int radius)
        {
            using var path = RoundedRectangle(Rectangle, radius);
            Graphics.FillPath(Brush, path);
        }

        public GraphicsPath RoundedRectangle(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            var d = radius * 2;
            var Mid = (int) Math.Round(r.Top - (r.Top - r.Bottom) / 2d);
            var y1 = r.Top + d;
            if (Mid < y1) y1 = Mid;

            var y2 = r.Bottom - d;
            if (y2 < Mid) y2 = Mid;

            path.AddLine(r.Left + d, r.Top, r.Right - d, r.Top);
            path.AddArc(Rectangle.FromLTRB(r.Right - d, r.Top, r.Right, r.Top + d), -90, 90f);
            path.AddLine(r.Right, y1, r.Right, y2);
            path.AddArc(Rectangle.FromLTRB(r.Right - d, r.Bottom - d, r.Right, r.Bottom), 0f, 90f);
            path.AddLine(r.Right - d, r.Bottom, r.Left + d, r.Bottom);
            path.AddArc(Rectangle.FromLTRB(r.Left, r.Bottom - d, r.Left + d, r.Bottom), 90f, 90f);
            path.AddLine(r.Left, y2, r.Left, y1);
            path.AddArc(Rectangle.FromLTRB(r.Left, r.Top, r.Left + d, r.Top + d), 180f, 90f);
            path.CloseFigure();
            return path;
        }

        public GraphicsPath RoundedRectangle_Max(Rectangle r)
        {
            var path = new GraphicsPath();
            var d = r.Bottom - r.Top;
            path.AddLine(r.Left + d, r.Top, r.Right - d, r.Top);
            path.AddArc(Rectangle.FromLTRB(r.Right - d, r.Top, r.Right, r.Top + d), -90, 180f);
            path.AddLine(r.Right - d, r.Bottom, r.Left + d, r.Bottom);
            path.AddArc(Rectangle.FromLTRB(r.Left, r.Bottom - d, r.Left + d, r.Bottom), 90f, 180f);
            path.CloseFigure();
            return path;
        }

        public void NOTE_Animate(ref PictureBox pbNote, ref string[] Note, ref clsGlobal.t_LabelSetup LSNote,
            ref Font tFont, ref clsGlobal.t_NoteAnimation NoteAnim, ref Bitmap BM, ref Graphics Gfx, ref Bitmap BackBmp,
            ref Bitmap OVLBmp)
        {
            LinearGradientBrush linGrBrush;
            var SF = new StringFormat();
            int TextHgt12;
            int tY;
            float sX, sY, sY2;

            // *****************************************************************
            // R4.00 Draw the background image.
            // *****************************************************************
            if (BackBmp is null)
            {
            }
            // R3.00 No image available so draw a solid color background.
            // R4.00 without this we get a motion blur effect due to gradient alpha not clearing the image.
            // Gfx.FillRectangle(New SolidBrush(LSNote.BackC), 0, 0, pbNote.Width, pbNote.Height)
            else
            {
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(LSNote.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        Gfx.DrawImage(BackBmp, 0, 0, BackBmp.Width, BackBmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = BM.Height;
                        for (tY = 0; BackBmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += BackBmp.Height)
                        for (int tX = 0, loopTo1 = BM.Width;
                            BackBmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1;
                            tX += BackBmp.Width)
                            Gfx.DrawImage(BackBmp, tX, tY, BackBmp.Width, BackBmp.Height);

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        Gfx.DrawImage(BackBmp, 0, 0, BM.Width, BM.Height);
                        break;
                    }
                }
            }

            // *****************************************************************
            // R4.00 Draw the background GRADIENT
            // *****************************************************************
            var tBrush = NoteAnim.Mode == 4
                ? new SolidBrush(Color.FromArgb((int) Math.Round(NoteAnim.Ystart), LSNote.ShadowColor))
                : new SolidBrush(LSNote.ShadowColor);

            TextHgt12 = (int) Math.Round(Gfx.MeasureString("H", tFont).Height / 2f);

            // R3.00 Draw the background rectangle.
            linGrBrush = LSNote.BDir == 0
                ? new LinearGradientBrush(new Point(0, 0), new Point(0, pbNote.Height), LSNote.B1, LSNote.B2)
                : new LinearGradientBrush(new Point(0, 0), new Point(pbNote.Width, 0), LSNote.B1, LSNote.B2);

            Gfx.FillRectangle(linGrBrush, 0, 0, pbNote.Width, pbNote.Height);

            // R4.10 Move/Animate NOTE OBJECTS.
            if (NoteAnim.Holding == false)
                switch (NoteAnim.Mode)
                {
                    case 1:
                    {
                        NoteAnim.X += NoteAnim.Xdir;
                        break;
                    }

                    case 0:
                    case 2:
                    case 5:
                    {
                        NoteAnim.Y += NoteAnim.Ydir;
                        if (NoteAnim.Y <= NoteAnim.Yend) NoteAnim.Y = NoteAnim.Yend;

                        break;
                    }

                    case 3:
                    {
                        NoteAnim.Y += NoteAnim.Ydir;
                        if (NoteAnim.Y >= NoteAnim.Yend) NoteAnim.Y = NoteAnim.Yend;

                        break;
                    }

                    case 4: // R4.00 Use Ystart as ALPHA counter. Limit to 255 or overflow.
                    {
                        NoteAnim.Ystart += NoteAnim.Ydir;
                        if (255f < NoteAnim.Ystart) NoteAnim.Ystart = 255f;

                        if (NoteAnim.Y >= NoteAnim.Yend) NoteAnim.Y = NoteAnim.Yend;

                        break;
                    }
                }

            // R4.00 Setup text align, RLCrawl is always Near aligned.
            switch (Conversion.Val(NoteAnim.Align))
            {
                case 0d:
                {
                    SF.Alignment = StringAlignment.Near;
                    break;
                }

                case 1d:
                {
                    SF.Alignment = StringAlignment.Center;
                    break;
                }

                case 2d:
                {
                    SF.Alignment = StringAlignment.Far;
                    break;
                }
            }

            if (NoteAnim.Mode == 1) SF.Alignment = StringAlignment.Near;

            // R4.00 Draw the TEXT SHADOW if one has been selected.
            if ((0 != LSNote.ShadowX) | (0 != LSNote.ShadowY))
            {
                Gfx.DrawString(Note[NoteAnim.TextCurrent], tFont, tBrush,
                    (float) (NoteAnim.X + LSNote.ShadowX * Conversion.Val(LSNote.ShadowDepth) + NoteAnim.Xoffset),
                    (float) (NoteAnim.Y + LSNote.ShadowY * Conversion.Val(LSNote.ShadowDepth) + NoteAnim.Yoffset), SF);

                // R4.10 Print all messages for ROLLING style.
                if (NoteAnim.Mode == 5)
                {
                    sX = (float) (NoteAnim.X + LSNote.ShadowX * Conversion.Val(LSNote.ShadowDepth) + NoteAnim.Xoffset);
                    sY = (float) (NoteAnim.Y + LSNote.ShadowY * Conversion.Val(LSNote.ShadowDepth) + NoteAnim.Yoffset);
                    for (int t = 1, loopTo2 = NoteAnim.TextCount - NoteAnim.TextCurrent; t <= loopTo2; t++)
                    {
                        sY2 = sY + (TextHgt12 + TextHgt12) * t;
                        if (sY2 < pbNote.Height)
                            Gfx.DrawString(Note[NoteAnim.TextCurrent + t], tFont, tBrush, sX, sY2, SF);
                    }
                }
            }


            // R4.00 Adjust gradients for moving text.
            sX = NoteAnim.X + NoteAnim.Xoffset;
            sY = NoteAnim.Y + NoteAnim.Yoffset;
            if (NoteAnim.Mode == 4)
                // R4.00 Build ALPHA animation gradient brush for text.
                linGrBrush = LSNote.FDir == 0
                    ? new LinearGradientBrush(new Point(0, (int) Math.Round(sY)),
                        new Point(0, (int) Math.Round(sY + TextHgt12 + TextHgt12)),
                        Color.FromArgb((int) Math.Round(NoteAnim.Ystart), LSNote.F1.R, LSNote.F1.G, LSNote.F1.B),
                        Color.FromArgb((int) Math.Round(NoteAnim.Ystart), LSNote.F2.R, LSNote.F2.G, LSNote.F2.B))
                    : new LinearGradientBrush(new Point(0, 0), new Point(pbNote.Width, 0),
                        Color.FromArgb((int) Math.Round(NoteAnim.Ystart), LSNote.F1.R, LSNote.F1.G, LSNote.F1.B),
                        Color.FromArgb((int) Math.Round(NoteAnim.Ystart), LSNote.F2.R, LSNote.F2.G, LSNote.F2.B));
            // R4.00 Build gradient brush for text.
            else if (LSNote.FDir == 0)
                linGrBrush = new LinearGradientBrush(new Point(0, (int) Math.Round(sY)),
                    new Point(0, (int) Math.Round(sY + TextHgt12 + TextHgt12)),
                    Color.FromArgb(255, LSNote.F1.R, LSNote.F1.G, LSNote.F1.B),
                    Color.FromArgb(255, LSNote.F2.R, LSNote.F2.G, LSNote.F2.B));
            else
                linGrBrush = new LinearGradientBrush(new Point(0, 0), new Point(pbNote.Width, 0),
                    Color.FromArgb(255, LSNote.F1.R, LSNote.F1.G, LSNote.F1.B),
                    Color.FromArgb(255, LSNote.F2.R, LSNote.F2.G, LSNote.F2.B));

            Gfx.DrawString(Note[NoteAnim.TextCurrent], tFont, linGrBrush, sX, sY, SF);

            // R4.10 Print all messages for ROLLING style.
            if (NoteAnim.Mode == 5)
                for (int t = 1, loopTo3 = NoteAnim.TextCount - NoteAnim.TextCurrent; t <= loopTo3; t++)
                {
                    tY = (TextHgt12 + TextHgt12) * t;
                    tY = (int) Math.Round(NoteAnim.Y + NoteAnim.Yoffset + tY);
                    if (tY < pbNote.Height)
                    {
                        linGrBrush = LSNote.FDir == 0
                            ? new LinearGradientBrush(new Point(0, tY), new Point(0, TextHgt12 + TextHgt12 + tY),
                                Color.FromArgb(255, LSNote.F1.R, LSNote.F1.G, LSNote.F1.B),
                                Color.FromArgb(255, LSNote.F2.R, LSNote.F2.G, LSNote.F2.B))
                            : new LinearGradientBrush(new Point(0, 0 + tY), new Point(pbNote.Width, 0 + tY),
                                Color.FromArgb(255, LSNote.F1.R, LSNote.F1.G, LSNote.F1.B),
                                Color.FromArgb(255, LSNote.F2.R, LSNote.F2.G, LSNote.F2.B));

                        Gfx.DrawString(Note[NoteAnim.TextCurrent + t], tFont, linGrBrush, NoteAnim.X + NoteAnim.Xoffset,
                            tY, SF);
                    }
                }

            // *****************************************************************
            // R3.50 Draw the NOTE OVERLAY image.
            // *****************************************************************
            if (OVLBmp is object)
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(LSNote.OVLScaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        Gfx.DrawImage(OVLBmp, 0, 0, OVLBmp.Width, OVLBmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo4 = BM.Height;
                        for (tY = 0; OVLBmp.Height >= 0 ? tY <= loopTo4 : tY >= loopTo4; tY += OVLBmp.Height)
                        for (int tX = 0, loopTo5 = BM.Width;
                            OVLBmp.Width >= 0 ? tX <= loopTo5 : tX >= loopTo5;
                            tX += OVLBmp.Width)
                            Gfx.DrawImage(OVLBmp, tX, tY, OVLBmp.Width, OVLBmp.Height);

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        // R4.00 MS sux.
                        if ((OVLBmp.Width < BM.Width) | (OVLBmp.Height < BM.Height))
                        {
                            var Xoff = (float) (BM.Width / (double) OVLBmp.Width);
                            var Yoff = (float) (BM.Height / (double) OVLBmp.Height);
                            Gfx.DrawImage(OVLBmp, 0f, 0f, BM.Width + Xoff, BM.Height + Yoff);
                        }
                        else
                        {
                            Gfx.DrawImage(OVLBmp, 0, 0, BM.Width, BM.Height);
                        }

                        break;
                    }
                }

            if (NoteAnim.Holding == false)
                switch (NoteAnim.Mode)
                {
                    case 1:
                    {
                        if (NoteAnim.X == NoteAnim.Xend)
                        {
                            NoteAnim.Holding = true;
                            NoteAnim.TimeAcc = 0L;
                        }

                        break;
                    }

                    case 0:
                    case 2:
                    case 5:
                    {
                        if (NoteAnim.Y <= NoteAnim.Yend)
                        {
                            NoteAnim.Y = NoteAnim.Yend;
                            NoteAnim.Holding = true;
                            NoteAnim.TimeAcc = 0L;
                        }

                        break;
                    }

                    case 3:
                    {
                        if (NoteAnim.Y >= NoteAnim.Yend)
                        {
                            NoteAnim.Y = NoteAnim.Yend;
                            NoteAnim.Holding = true;
                            NoteAnim.TimeAcc = 0L;
                        }

                        break;
                    }

                    case 4:
                    {
                        if (NoteAnim.Ystart >= NoteAnim.Yend)
                        {
                            NoteAnim.Ystart = NoteAnim.Yend;
                            NoteAnim.Holding = true;
                            NoteAnim.TimeAcc = 0L;
                        }

                        break;
                    }
                }

            NoteAnim.TimeAcc += 33L;
            if (NoteAnim.TimeHold * 1000L < NoteAnim.TimeAcc)
                NoteAnim.Holding = false; // R4.00 TimeHold is in secs, need mS.

            // R4.00 Are we done animating a section/Line.
            if (NoteAnim.Holding == false)
                switch (NoteAnim.Mode)
                {
                    case 1:
                    {
                        if (NoteAnim.X < NoteAnim.Xend) NoteAnim.X = NoteAnim.Xstart;

                        break;
                    }

                    case 0:
                    case 2:
                    case 5:
                    {
                        if (NoteAnim.Y <= NoteAnim.Yend)
                        {
                            NoteAnim.TimeAcc = 0L;
                            if (NoteAnim.Mode == 5)
                                NoteAnim.Y += TextHgt12 + TextHgt12;
                            else
                                NoteAnim.Y = NoteAnim.Ystart;

                            NoteAnim.TextCurrent += 1;
                            if (NoteAnim.TextCount < NoteAnim.TextCurrent)
                            {
                                NoteAnim.TextCurrent = 1;
                                if (NoteAnim.Mode == 5) NoteAnim.Y = NoteAnim.Ystart;
                            }
                        }

                        break;
                    }

                    case 3:
                    {
                        if (NoteAnim.Y >= NoteAnim.Yend)
                        {
                            NoteAnim.TimeAcc = 0L;
                            NoteAnim.Y = NoteAnim.Ystart;
                            NoteAnim.TextCurrent += 1;
                            if (NoteAnim.TextCount < NoteAnim.TextCurrent) NoteAnim.TextCurrent = 1;
                        }

                        break;
                    }

                    case 4:
                    {
                        if (NoteAnim.Ystart >= NoteAnim.Yend)
                        {
                            NoteAnim.TimeAcc = 0L;
                            NoteAnim.Ystart = 0f;
                            NoteAnim.TextCurrent += 1;
                            if (NoteAnim.TextCount < NoteAnim.TextCurrent) NoteAnim.TextCurrent = 1;
                        }

                        break;
                    }
                }

            // 3.50 Update the Screen.
            pbNote.Image = BM;
        }

        public void NOTE_Animation_Setup(ref clsGlobal.t_NoteAnimation NoteAnim, ref PictureBox pbNote, ref Font tFont,
            ref string[] Note_Text, ref string[] NoteAnim_Text)
        {
            // R4.00 this sub sets up the animation variables for a NOTE.
            var BM = new Bitmap(pbNote.Width, pbNote.Height);
            var Gfx = Graphics.FromImage(BM);
            string tDelim;
            var Cnt = default(int);
            var HasText = false;
            NoteAnim.TimeAcc = 0L;
            NoteAnim.Holding = false;
            switch (NoteAnim.Mode)
            {
                case 0: // R4.00 NO ANIMATION
                {
                    // R4.00 Select X position by TEXT alignment setting.
                    switch (Conversion.Val(NoteAnim.Align))
                    {
                        case 0d:
                        {
                            NoteAnim.X = 0f;
                            NoteAnim.Xstart = 0f;
                            break;
                        }

                        case 1d:
                        {
                            NoteAnim.X = (float) (pbNote.Width / 2d);
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }

                        case 2d:
                        {
                            NoteAnim.X = pbNote.Width;
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }
                    }

                    NoteAnim.Yend = (float) (pbNote.Height / 2d - Gfx.MeasureString("H", tFont).Height / 2f);
                    NoteAnim.Ystart = NoteAnim.Yend;
                    NoteAnim.Y = NoteAnim.Yend;
                    NoteAnim.Ydir = -2;
                    NoteAnim.TextCount = 1;
                    NoteAnim.TextCurrent = 1;
                    Note_Text[1] = NoteAnim_Text[1]; // R4.00 Only show the first line.
                    if (!string.IsNullOrEmpty(Note_Text[1])) HasText = true;

                    break;
                }

                case 1: // R4.00 L->R CRAWLER
                {
                    // R4.00 Do we need the defined delimiter string?
                    tDelim = "";
                    for (var t = 1; t <= 10; t++)
                        if (!string.IsNullOrEmpty(Strings.Trim(NoteAnim_Text[t])))
                            Cnt += 1;

                    if (1 < Cnt) tDelim = NoteAnim.Delim;

                    // R4.00 Build a giant string with all notes in it. Place Delimiter string in between each line.
                    Note_Text[1] = "";
                    for (var t = 1; t <= 10; t++)
                        if (!string.IsNullOrEmpty(Strings.Trim(NoteAnim_Text[t])))
                        {
                            Note_Text[1] = Note_Text[1] + NoteAnim_Text[t] + tDelim;
                            HasText = true;
                        }

                    NoteAnim.Xstart = pbNote.Width;
                    NoteAnim.X = NoteAnim.Xstart;
                    NoteAnim.Xend = 0f - Gfx.MeasureString(Note_Text[1], tFont).Width;
                    NoteAnim.Xdir = NoteAnim.Speed * -1;
                    NoteAnim.Ystart = 0f;
                    NoteAnim.Y = (float) (pbNote.Height / 2d - Gfx.MeasureString("H", tFont).Height / 2f);
                    NoteAnim.TextCount = 1;
                    NoteAnim.TextCurrent = 1;
                    break;
                }

                case 2: // R4.00 UP CRAWLER
                {
                    // R4.00 Select X position by TEXT alignment setting.
                    switch (Conversion.Val(NoteAnim.Align))
                    {
                        case 0d:
                        {
                            NoteAnim.X = 0f;
                            NoteAnim.Xstart = 0f;
                            break;
                        }

                        case 1d:
                        {
                            NoteAnim.X = (float) (pbNote.Width / 2d);
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }

                        case 2d:
                        {
                            NoteAnim.X = pbNote.Width;
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }
                    }

                    NoteAnim.Ystart = pbNote.Height;
                    NoteAnim.Y = NoteAnim.Ystart;
                    NoteAnim.Yend = (float) (pbNote.Height / 2d - Gfx.MeasureString("H", tFont).Height / 2f);
                    NoteAnim.Ydir = NoteAnim.Speed * -1;
                    NoteAnim.TextCurrent = 1;
                    NoteAnim.TextCount = 0;
                    for (var t = 1; t <= 10; t++)
                        if (!string.IsNullOrEmpty(Strings.Trim(NoteAnim_Text[t])))
                        {
                            Note_Text[t] = NoteAnim_Text[t];
                            NoteAnim.TextCount += 1;
                            HasText = true;
                        }

                    break;
                }

                case 3: // R4.00 DOWN CRAWLER
                {
                    // R4.00 Select X position by TEXT alignment setting.
                    switch (Conversion.Val(NoteAnim.Align))
                    {
                        case 0d:
                        {
                            NoteAnim.X = 0f;
                            NoteAnim.Xstart = 0f;
                            break;
                        }

                        case 1d:
                        {
                            NoteAnim.X = (float) (pbNote.Width / 2d);
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }

                        case 2d:
                        {
                            NoteAnim.X = pbNote.Width;
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }
                    }

                    NoteAnim.Ystart = (float) (0d - Gfx.MeasureString("H", tFont).Height * 1.1d);
                    NoteAnim.Y = NoteAnim.Ystart;
                    NoteAnim.Yend = (float) (pbNote.Height / 2d - Gfx.MeasureString("H", tFont).Height / 2f);
                    NoteAnim.Ydir = NoteAnim.Speed * 1;
                    NoteAnim.TextCurrent = 1;
                    NoteAnim.TextCount = 0;
                    for (var t = 1; t <= 10; t++)
                        if (!string.IsNullOrEmpty(Strings.Trim(NoteAnim_Text[t])))
                        {
                            Note_Text[t] = NoteAnim_Text[t];
                            NoteAnim.TextCount += 1;
                            HasText = true;
                        }

                    break;
                }

                case 4: // R4.00 FADE IN
                {
                    // R4.00 Select X position by TEXT alignment setting.
                    switch (Conversion.Val(NoteAnim.Align))
                    {
                        case 0d:
                        {
                            NoteAnim.X = 0f;
                            NoteAnim.Xstart = 0f;
                            break;
                        }

                        case 1d:
                        {
                            NoteAnim.X = (float) (pbNote.Width / 2d);
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }

                        case 2d:
                        {
                            NoteAnim.X = pbNote.Width;
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }
                    }

                    NoteAnim.Ystart = 0f;
                    NoteAnim.Y = (float) (pbNote.Height / 2d - Gfx.MeasureString("H", tFont).Height / 2f);
                    NoteAnim.Yend = 255f;
                    NoteAnim.Ydir = NoteAnim.Speed * 1;
                    NoteAnim.TextCurrent = 1;
                    NoteAnim.TextCount = 0;
                    for (var t = 1; t <= 10; t++)
                        if (!string.IsNullOrEmpty(Strings.Trim(NoteAnim_Text[t])))
                        {
                            Note_Text[t] = NoteAnim_Text[t];
                            NoteAnim.TextCount += 1;
                            HasText = true;
                        }

                    break;
                }

                case 5: // R4.10 UP CRAWLER
                {
                    // R4.10 Select X position by TEXT alignment setting.
                    switch (Conversion.Val(NoteAnim.Align))
                    {
                        case 0d:
                        {
                            NoteAnim.X = 0f;
                            NoteAnim.Xstart = 0f;
                            break;
                        }

                        case 1d:
                        {
                            NoteAnim.X = (float) (pbNote.Width / 2d);
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }

                        case 2d:
                        {
                            NoteAnim.X = pbNote.Width;
                            NoteAnim.Xstart = NoteAnim.X;
                            break;
                        }
                    }

                    NoteAnim.Ystart = pbNote.Height;
                    NoteAnim.Y = NoteAnim.Ystart;
                    NoteAnim.Yend = 0f - Gfx.MeasureString("H", tFont).Height;
                    NoteAnim.Ydir = NoteAnim.Speed * -1;
                    NoteAnim.TimeHold = 0L; // NoteAnim.TimeHold
                    NoteAnim.TextCurrent = 1;
                    NoteAnim.TextCount = 0;
                    for (var t = 1; t <= 10; t++)
                        if (!string.IsNullOrEmpty(Strings.Trim(NoteAnim_Text[t])))
                        {
                            Note_Text[t] = NoteAnim_Text[t];
                            NoteAnim.TextCount += 1;
                            HasText = true;
                        }

                    break;
                }
            }

            NoteAnim.Active = HasText;
            Gfx.Dispose();
            BM.Dispose();
        }
    }
}