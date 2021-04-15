using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MakoCelo
{
    public class Settings
    {
        private frmMain _frmMain;

        public Settings(frmMain frmMain1)
        {
            _frmMain = frmMain1;
        }

        public void SETTINGS_Load_Old(string tFILE)
        {
            string tPath;
            var A = "";
            int Ca, Cr, Cg, Cb;
            var Frev = 0;

            // R4.20 Added Load/Save setups options.
            tPath = string.IsNullOrEmpty(tFILE) ? Application.StartupPath + @"\MakoCelo_settings.dat" : tFILE;

            if (!File.Exists(tPath)) return;

            // R4.00 Open the SETTINGS file.
            FileSystem.FileOpen(1, tPath, OpenMode.Input);
            try
            {
                FileSystem.Input(1, ref A);
                switch (A ?? "")
                {
                    case "VERSION MC200":
                    {
                        Frev = 2;
                        FileSystem.Input(1, ref A); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC300":
                    {
                        Frev = 3;
                        FileSystem.Input(1, ref A); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC400":
                    {
                        Frev = 4;
                        FileSystem.Input(1, ref A); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC500":
                    {
                        Frev = 5;
                        FileSystem.Input(1, ref A); // R2.00 Read extra header line.
                        break;
                    }

                    default:
                    {
                        Interaction.MsgBox(
                            "WARNING: The startup data file appears to be corrupt or incorrect." + Constants.vbCr +
                            Constants.vbCr + "Path: " + Application.StartupPath + @"\MakoCelo_settings.dat",
                            Constants.vbCritical, "MakoCELO");
                        break;
                    }
                }

                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A); // R1.00 BACK COLOR
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A); // R1.00 ALPHA
                FileSystem.Input(1, ref A);
                FileSystem.Input(1, ref A); // R1.00 BACK IMAGE 
                FileSystem.Input(1, ref A);
                if (File.Exists(A))
                {
                    _frmMain.NAME_bmp = new Bitmap(A); // R4.00 Switched to memory based image management.
                    frmMain.PATH_BackgroundImage = A; // R3.00 Removed pnlPlr.BackgroundImage = Image.FromFile(A)
                }
                // R3.30 Added notice if image is missing.
                else if (!string.IsNullOrEmpty(A))
                {
                    Interaction.MsgBox("ERROR: The User Settings background image no longer exists." + Constants.vbCr +
                                       Constants.vbCr + "File:" + A);
                }

                FileSystem.Input(1, ref A); // R1.00 GAME PATH
                FileSystem.Input(1, ref A);
                _frmMain.PATH_Game = Strings.Trim(A);
                FileSystem.Input(1, ref A); // R1.00 FONT
                FileSystem.Input(1, ref A);
                frmMain.FONT_Rank_Name = Strings.Trim(A);
                FileSystem.Input(1, ref A);
                frmMain.FONT_Rank_Size = Strings.Trim(A);
                FileSystem.Input(1, ref A);
                frmMain.FONT_Rank_Bold = Strings.Trim(A);
                FileSystem.Input(1, ref A);
                frmMain.FONT_Rank_Italic = Strings.Trim(A);
                if (frmMain.FONT_Rank_Bold == "True")
                {
                }

                if (frmMain.FONT_Rank_Italic == "True")
                {
                }

                frmMain.FONT_Rank = new Font(frmMain.FONT_Rank_Name, Conversions.ToSingle(frmMain.FONT_Rank_Size),
                    FontStyle.Regular);
                if (frmMain.FONT_Rank_Bold == "True")
                    frmMain.FONT_Rank = new Font(frmMain.FONT_Rank_Name, Conversions.ToSingle(frmMain.FONT_Rank_Size),
                        FontStyle.Bold);

                if (frmMain.FONT_Rank_Italic == "True")
                    frmMain.FONT_Rank = new Font(frmMain.FONT_Rank_Name, Conversions.ToSingle(frmMain.FONT_Rank_Size),
                        FontStyle.Italic);

                // R3.10 Version 2.0 and above.
                if (0 < Frev)
                {
                    FileSystem.Input(1, ref A); // R1.00 FORE COLOR - DEPRECATED
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A); // R1.00 BACK COLOR - DEPRECATED
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A); // R1.00 ALPHA - DEPRECATED
                    FileSystem.Input(1, ref A);
                    FileSystem.Input(1, ref A); // R1.00 FONT
                    FileSystem.Input(1, ref A);
                    frmMain.FONT_Name_Name = Strings.Trim(A);
                    FileSystem.Input(1, ref A);
                    frmMain.FONT_Name_Size = Strings.Trim(A);
                    FileSystem.Input(1, ref A);
                    frmMain.FONT_Name_Bold = Strings.Trim(A);
                    FileSystem.Input(1, ref A);
                    frmMain.FONT_Name_Italic = Strings.Trim(A);
                    if (frmMain.FONT_Name_Bold == "True")
                    {
                    }

                    if (frmMain.FONT_Name_Italic == "True")
                    {
                    }

                    frmMain.FONT_Name = new Font(frmMain.FONT_Name_Name, Conversions.ToSingle(frmMain.FONT_Name_Size),
                        FontStyle.Regular);
                    if (frmMain.FONT_Name_Bold == "True")
                        frmMain.FONT_Name = new Font(frmMain.FONT_Name_Name,
                            Conversions.ToSingle(frmMain.FONT_Name_Size), FontStyle.Bold);

                    if (frmMain.FONT_Name_Italic == "True")
                        frmMain.FONT_Name = new Font(frmMain.FONT_Name_Name,
                            Conversions.ToSingle(frmMain.FONT_Name_Size), FontStyle.Italic);

                    FileSystem.Input(1, ref A); // R2.00 SCREEN SIZE
                    FileSystem.Input(1, ref A); // cboPageSize.Text = Trim(A)
                    _frmMain.SETTINGS_GetStatSize(A); // R4.10 Changed size.
                    FileSystem.Input(1, ref A); // R2.00 PAGE LAYOUT Y
                    FileSystem.Input(1, ref A);
                    _frmMain.cboLayoutY.Text = Strings.Trim(A);
                    FileSystem.Input(1, ref A); // R2.00 PAGE LAYOUT X
                    FileSystem.Input(1, ref A);
                    _frmMain.cboLayoutX.Text = Strings.Trim(A);
                    FileSystem.Input(1, ref A); // R2.00 PANEL BACK COLOR
                    FileSystem.Input(1, ref A);
                    Ca = (int) Math.Round(Conversion.Val(A));
                    FileSystem.Input(1, ref A);
                    Cr = (int) Math.Round(Conversion.Val(A));
                    FileSystem.Input(1, ref A);
                    Cg = (int) Math.Round(Conversion.Val(A));
                    FileSystem.Input(1, ref A);
                    Cb = (int) Math.Round(Conversion.Val(A));
                    _frmMain.pbStats.BackColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                    _frmMain.LSName.BackC = _frmMain.pbStats.BackColor; // R4.00 Added.
                    FileSystem.Input(1, ref A); // R2.00 IMAGE SCALING
                    FileSystem.Input(1, ref A);
                    _frmMain.LSName.Scaling = Strings.Trim(A); // R4.00 Added Scaling var.
                    if (!FileSystem.EOF(1))
                    {
                        FileSystem.Input(1, ref A); // R2.00 GUI COLOR SCHEME
                        FileSystem.Input(1, ref A);
                        _frmMain.GUI_ColorMode = (int) Math.Round(Conversion.Val(Strings.Trim(A)));
                    }
                    else
                    {
                        _frmMain.GUI_ColorMode = 0;
                    }

                    // **********************************************************
                    // REV 3 and newer files
                    // **********************************************************
                    if (2 < Frev)
                    {
                        // R3.00 RANK LABEL VARS
                        FileSystem.Input(1, ref A); // R3.00 RANK FORE COLOR 1 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 RANK FORE COLOR 2 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 RANK FORE GRADIENT 
                        FileSystem.Input(1, ref A);
                        _frmMain.LSRank.FDir = Conversions.ToInteger(A);
                        FileSystem.Input(1, ref A); // R3.00 RANK BACK COLOR 1 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 RANK BACK COLOR 2 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 RANK BACK GRADIENT 
                        FileSystem.Input(1, ref A);
                        _frmMain.LSRank.BDir = Conversions.ToInteger(A);

                        // R3.00 NAME LABEL VARS
                        FileSystem.Input(1, ref A); // R3.00 NAME FORE COLOR 1 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 NAME FORE COLOR 2 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 NAME FORE GRADIENT 
                        FileSystem.Input(1, ref A);
                        _frmMain.LSName.FDir = Conversions.ToInteger(A);
                        FileSystem.Input(1, ref A); // R3.00 NAME BACK COLOR 1 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 NAME BACK COLOR 2 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 NAME BACK GRADIENT 
                        FileSystem.Input(1, ref A);
                        _frmMain.LSName.BDir = Conversions.ToInteger(A);
                        FileSystem.Input(1, ref A); // R3.00 RANK SHADOW COLOR 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 RANK SHADOW DIR
                        FileSystem.Input(1, ref A);
                        _frmMain.LSRank.ShadowDir = A;
                        FileSystem.Input(1, ref A); // R3.00 RANK SHADOW DEPTH - Future
                        FileSystem.Input(1, ref A);
                        _frmMain.LSRank.ShadowDepth = A;
                        FileSystem.Input(1, ref A); // R3.00 NAME SHADOW COLOR 
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 NAME SHADOW DIR
                        FileSystem.Input(1, ref A);
                        // CName3D = A
                        // cboName3D.Text = A
                        _frmMain.LSName.ShadowDir = A;
                        FileSystem.Input(1, ref A); // R3.00 NAME SHADOW DEPTH - Future
                        FileSystem.Input(1, ref A);
                        // CName3Depth = A
                        _frmMain.LSName.ShadowDepth = A;
                        FileSystem.Input(1, ref A); // R3.00 BACK GRADIENT COLOR 1 - Future
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.COLOR_Back1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 BACK GRADIENT COLOR 2 - Future
                        FileSystem.Input(1, ref A);
                        Ca = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cr = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cg = (int) Math.Round(Conversion.Val(A));
                        FileSystem.Input(1, ref A);
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.COLOR_Back2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        FileSystem.Input(1, ref A); // R3.00 NAME SHADOW DIR - Future
                        FileSystem.Input(1, ref A);
                        _frmMain.COLOR_Back_Dir = Conversions.ToInteger(A);

                        // **********************************************************
                        // REV 4 and newer files
                        // **********************************************************
                        if (3 < Frev)
                        {
                            FileSystem.Input(1, ref A); // R3.10 FX COLORS
                            for (var t = 1; t <= 10; t++)
                            {
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                _frmMain.CFX3DC[t] = Color.FromArgb(Ca, Cr, Cg, Cb);
                            }

                            FileSystem.Input(1, ref A); // R3.10 FX VARS
                            for (var N = 1; N <= 10; N++)
                            for (var t = 1; t <= 10; t++)
                            {
                                FileSystem.Input(1, ref A);
                                _frmMain.CFX3DVar[N, t] = Strings.Trim(A);
                            }

                            FileSystem.Input(1, ref A); // R3.10 FX ACTIVE
                            for (var N = 1; N <= 10; N++)
                            {
                                FileSystem.Input(1, ref A);
                                if (A == "True")
                                    _frmMain.CFX3DActive[N] = true;
                                else
                                    _frmMain.CFX3DActive[N] = false;
                            }

                            // ***********************************
                            // R3.00 Rev 5 and above.
                            // ***********************************
                            if (4 < Frev)
                            {
                                FileSystem.Input(1, ref A); // NAME OVERLAY IMAGE
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Name_OVLBmp = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.LSName.OVLScaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A); // NOTE 01 VARS
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.BDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.FDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.Height = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.O1 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.O2 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.Scaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.OVLScaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.ShadowDepth = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.ShadowDir = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote01.Width = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note01_Bmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note01_BmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note01_OVLBmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note01_OVLBmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note01_Name = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note01_Bold = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note01_Italic = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note01_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    FileSystem.Input(1, ref A);
                                    _frmMain.NoteAnim01_Text[t] = A;
                                }

                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.Mode = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.Speed = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.TimeHold = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.Align = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.Delim = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.Xoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim01.Yoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A); // NOTE 02 VARS
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.BDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.FDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.Height = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.O1 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.O2 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.Scaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.OVLScaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.ShadowDepth = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.ShadowDir = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote02.Width = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note02_Bmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note02_BmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note02_OVLBmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note02_OVLBmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note02_Name = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note02_Bold = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note02_Italic = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note02_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    FileSystem.Input(1, ref A);
                                    _frmMain.NoteAnim02_Text[t] = A;
                                }

                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.Mode = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.Speed = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.TimeHold = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.Align = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.Delim = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.Xoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim02.Yoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A); // NOTE 03 VARS
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.BDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.FDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.Height = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.O1 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.O2 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.Scaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.OVLScaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.ShadowDepth = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.ShadowDir = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote03.Width = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note03_Bmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note03_BmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note03_OVLBmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note03_OVLBmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note03_Name = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note03_Bold = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note03_Italic = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note03_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    FileSystem.Input(1, ref A);
                                    _frmMain.NoteAnim03_Text[t] = A;
                                }

                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.Mode = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.Speed = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.TimeHold = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.Align = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.Delim = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.Xoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim03.Yoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A); // NOTE 04 VARS
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.BDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.FDir = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.Height = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.O1 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.O2 = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.Scaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.OVLScaling = Conversion.Val(A).ToString();
                                FileSystem.Input(1, ref A);
                                Ca = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cr = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cg = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.ShadowDepth = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.ShadowDir = A;
                                FileSystem.Input(1, ref A);
                                frmMain.LSNote04.Width = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note04_Bmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note04_BmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note04_OVLBmp = A;
                                FileSystem.Input(1, ref A);
                                frmMain.PATH_Note04_OVLBmpPath = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note04_Name = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note04_Bold = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note04_Italic = A;
                                FileSystem.Input(1, ref A);
                                frmMain.FONT_Note04_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    FileSystem.Input(1, ref A);
                                    _frmMain.NoteAnim04_Text[t] = A;
                                }

                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.Mode = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.Speed = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.TimeHold = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.Align = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.Delim = A;
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.Xoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.NoteAnim04.Yoffset = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A); // NOTE SPACING
                                FileSystem.Input(1, ref A);
                                _frmMain.NOTE_Spacing = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A); // SOUND SAMPLES
                                for (var t = 1; t <= 30; t++)
                                {
                                    FileSystem.Input(1, ref A);
                                    _frmMain.SOUND_File[t] = Strings.Trim(A);
                                    FileSystem.Input(1, ref A); // R4.00 Future Pitch.
                                    FileSystem.Input(1, ref A); // R4.00 Future Vol.
                                }

                                FileSystem.Input(1, ref A);
                                _frmMain.scrVolume.Value = (int) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A); // WINDOW STATE
                                FileSystem.Input(1, ref A);
                                _frmMain.Celo_Windowstate = Strings.Trim(A);
                                FileSystem.Input(1, ref A);
                                _frmMain.Celo_Left = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.Celo_Top = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.Celo_Width = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                _frmMain.Celo_Height = (long) Math.Round(Conversion.Val(A));
                                FileSystem.Input(1, ref A);
                                if (A == "1") _frmMain.chkPosition.Checked = true;
                            } // <-- REV 5 END
                        } // <-- REV 4 END
                    } // <-- REV 3 END
                } // <-- REV 2 END
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(
                    "ERROR: " + ex.Message + Constants.vbCr + Constants.vbCr + "Unable to read the saved settings." +
                    Constants.vbCr + "The last known setup could not be loaded." + Constants.vbCr + Constants.vbCr +
                    "If running a new version, this error may fix itself when a new setup is saved.",
                    MsgBoxStyle.Critical, "MakoCelo - Setup Error");
            }

            _frmMain.FX_SetVarControls();
            _frmMain.SETTINGS_SetupAfterLoad();
            FileSystem.FileClose(1);
        }

        public void SETTINGS_Load(string tFILE)
        {
            long Vlong;
            string tPath;
            string A;
            int Ca, Cr, Cg, Cb;
            var Frev = 0;

            // R4.20 Added Load/Save setups options.
            tPath = string.IsNullOrEmpty(tFILE) ? Application.StartupPath + @"\MakoCelo_settings.dat" : tFILE;

            if (!File.Exists(tPath)) return;


            // R2.01 OPEN log file and start parsing.
            var fs = default(FileStream);
            var sr = default(StreamReader);
            try
            {
                fs = new FileStream(tPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sr = new StreamReader(fs, Encoding.UTF8);
                A = sr.ReadLine();
                switch (A ?? "")
                {
                    case "VERSION MC200":
                    {
                        Frev = 2;
                        sr.ReadLine(); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC300":
                    {
                        Frev = 3;
                        sr.ReadLine(); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC400":
                    {
                        Frev = 4;
                        sr.ReadLine(); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC500":
                    {
                        Frev = 5;
                        sr.ReadLine(); // R2.00 Read extra header line.
                        break;
                    }

                    case "VERSION MC600":
                    {
                        Frev = 6;
                        sr.ReadLine(); // R2.00 Read extra header line.
                        break;
                    }

                    default:
                    {
                        Interaction.MsgBox(
                            "WARNING: The startup data file appears to be corrupt or incorrect." + Constants.vbCr +
                            Constants.vbCr + "Path: " + Application.StartupPath + @"\MakoCelo_settings.dat",
                            Constants.vbCritical, "MakoCELO");
                        break;
                    }
                }

                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine(); // R1.00 BACK COLOR
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine(); // R1.00 ALPHA
                sr.ReadLine();
                sr.ReadLine(); // R1.00 BACK IMAGE 
                A = sr.ReadLine();
                if (File.Exists(A))
                {
                    _frmMain.NAME_bmp = new Bitmap(A); // R3.00 Switched to memory based image management.
                    frmMain.PATH_BackgroundImage = A; // R3.00 Removed pnlPlr.BackgroundImage = Image.FromFile(A)
                }
                // R3.30 Added notice if image is missing.
                else if (string.IsNullOrEmpty(A))
                {
                    _frmMain.NAME_bmp = null;
                    frmMain.PATH_BackgroundImage =
                        ""; // R4.46 Fixed bug where images did not get cleared on LOAD SETUP if no image was set.
                }
                else
                {
                    Interaction.MsgBox("ERROR: The User Settings background image no longer exists." + Constants.vbCr +
                                       Constants.vbCr + "File:" + A);
                }

                sr.ReadLine(); // R1.00 GAME PATH
                A = sr.ReadLine();
                _frmMain.PATH_Game = Strings.Trim(A);
                // R3.40 lbPath.Text = PATH_Game

                sr.ReadLine(); // R1.00 FONT
                A = sr.ReadLine();
                frmMain.FONT_Rank_Name = Strings.Trim(A);
                A = sr.ReadLine();
                frmMain.FONT_Rank_Size = Strings.Trim(A);
                A = sr.ReadLine();
                frmMain.FONT_Rank_Bold = Strings.Trim(A);
                A = sr.ReadLine();
                frmMain.FONT_Rank_Italic = Strings.Trim(A);
                if (frmMain.FONT_Rank_Bold == "True")
                {
                }

                if (frmMain.FONT_Rank_Italic == "True")
                {
                }

                frmMain.FONT_Rank = new Font(frmMain.FONT_Rank_Name, Conversions.ToSingle(frmMain.FONT_Rank_Size),
                    FontStyle.Regular);
                if (frmMain.FONT_Rank_Bold == "True")
                    frmMain.FONT_Rank = new Font(frmMain.FONT_Rank_Name, Conversions.ToSingle(frmMain.FONT_Rank_Size),
                        FontStyle.Bold);

                if (frmMain.FONT_Rank_Italic == "True")
                    frmMain.FONT_Rank = new Font(frmMain.FONT_Rank_Name, Conversions.ToSingle(frmMain.FONT_Rank_Size),
                        FontStyle.Italic);

                // R3.10 Version 2.0 and above.
                if (0 < Frev)
                {
                    sr.ReadLine(); // R1.00 FORE COLOR - DEPRECATED
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();

                    sr.ReadLine(); // R1.00 BACK COLOR - DEPRECATED
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();
                    sr.ReadLine();

                    sr.ReadLine(); // R1.00 ALPHA - DEPRECATED
                    sr.ReadLine();

                    sr.ReadLine(); // R1.00 FONT
                    A = sr.ReadLine();
                    frmMain.FONT_Name_Name = Strings.Trim(A);
                    A = sr.ReadLine();
                    frmMain.FONT_Name_Size = Strings.Trim(A);
                    A = sr.ReadLine();
                    frmMain.FONT_Name_Bold = Strings.Trim(A);
                    A = sr.ReadLine();
                    frmMain.FONT_Name_Italic = Strings.Trim(A);
                    if (frmMain.FONT_Name_Bold == "True")
                    {
                    }

                    if (frmMain.FONT_Name_Italic == "True")
                    {
                    }

                    frmMain.FONT_Name = new Font(frmMain.FONT_Name_Name, Conversions.ToSingle(frmMain.FONT_Name_Size),
                        FontStyle.Regular);
                    if (frmMain.FONT_Name_Bold == "True")
                        frmMain.FONT_Name = new Font(frmMain.FONT_Name_Name,
                            Conversions.ToSingle(frmMain.FONT_Name_Size), FontStyle.Bold);

                    if (frmMain.FONT_Name_Italic == "True")
                        frmMain.FONT_Name = new Font(frmMain.FONT_Name_Name,
                            Conversions.ToSingle(frmMain.FONT_Name_Size), FontStyle.Italic);

                    sr.ReadLine(); // R2.00 SCREEN SIZE
                    A = sr.ReadLine(); // cboPageSize.Text = Trim(A)
                    _frmMain.SETTINGS_GetStatSize(A);
                    sr.ReadLine(); // R2.00 PAGE LAYOUT Y
                    A = sr.ReadLine();
                    _frmMain.cboLayoutY.Text = Strings.Trim(A);
                    sr.ReadLine(); // R2.00 PAGE LAYOUT X
                    A = sr.ReadLine();
                    _frmMain.cboLayoutX.Text = Strings.Trim(A);
                    sr.ReadLine(); // R2.00 PANEL BACK COLOR
                    A = sr.ReadLine();
                    Ca = (int) Math.Round(Conversion.Val(A));
                    A = sr.ReadLine();
                    Cr = (int) Math.Round(Conversion.Val(A));
                    A = sr.ReadLine();
                    Cg = (int) Math.Round(Conversion.Val(A));
                    A = sr.ReadLine();
                    Cb = (int) Math.Round(Conversion.Val(A));
                    _frmMain.pbStats.BackColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                    _frmMain.LSName.BackC = _frmMain.pbStats.BackColor; // R4.00 Added.
                    sr.ReadLine(); // R2.00 IMAGE SCALING
                    A = sr.ReadLine(); // cboScaling.Text = Trim(A)
                    _frmMain.LSName.Scaling = Strings.Trim(A); // R4.00 Added Scaling var.
                    if (!sr.EndOfStream)
                    {
                        sr.ReadLine(); // R2.00 GUI COLOR SCHEME
                        A = sr.ReadLine();
                        _frmMain.GUI_ColorMode = (int) Math.Round(Conversion.Val(Strings.Trim(A)));
                        if (_frmMain.GUI_ColorMode != 0) _frmMain.chkGUIMode.Checked = true; // R4.30 Added.
                    }
                    else
                    {
                        _frmMain.GUI_ColorMode = 0;
                    }

                    // R3.00 Rev 3 and above.
                    if (2 < Frev)
                    {
                        // R3.00 RANK LABEL VARS
                        sr.ReadLine(); // R3.00 RANK FORE COLOR 1 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 RANK FORE COLOR 2 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 RANK FORE GRADIENT 
                        A = sr.ReadLine();
                        _frmMain.LSRank.FDir = Conversions.ToInteger(A);
                        sr.ReadLine(); // R3.00 RANK BACK COLOR 1 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 RANK BACK COLOR 2 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 RANK BACK GRADIENT 
                        A = sr.ReadLine();
                        _frmMain.LSRank.BDir = Conversions.ToInteger(A);

                        // R3.00 NAME LABEL VARS
                        sr.ReadLine(); // R3.00 NAME FORE COLOR 1 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 NAME FORE COLOR 2 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 NAME FORE GRADIENT 
                        A = sr.ReadLine();
                        _frmMain.LSName.FDir = Conversions.ToInteger(A);
                        sr.ReadLine(); // R3.00 NAME BACK COLOR 1 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 NAME BACK COLOR 2 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 NAME BACK GRADIENT 
                        A = sr.ReadLine();
                        _frmMain.LSName.BDir = Conversions.ToInteger(A);
                        sr.ReadLine(); // R3.00 RANK SHADOW COLOR 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSRank.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 RANK SHADOW DIR
                        A = sr.ReadLine();
                        _frmMain.LSRank.ShadowDir = A;
                        sr.ReadLine(); // R3.00 RANK SHADOW DEPTH - Future
                        A = sr.ReadLine();
                        _frmMain.LSRank.ShadowDepth = A;
                        sr.ReadLine(); // R3.00 NAME SHADOW COLOR 
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.LSName.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 NAME SHADOW DIR
                        A = sr.ReadLine();
                        _frmMain.LSName.ShadowDir = A;
                        sr.ReadLine(); // R3.00 NAME SHADOW DEPTH - Future
                        A = sr.ReadLine();
                        _frmMain.LSName.ShadowDepth = A;
                        sr.ReadLine(); // R3.00 BACK GRADIENT COLOR 1 - Future
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.COLOR_Back1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 BACK GRADIENT COLOR 2 - Future
                        A = sr.ReadLine();
                        Ca = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cr = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cg = (int) Math.Round(Conversion.Val(A));
                        A = sr.ReadLine();
                        Cb = (int) Math.Round(Conversion.Val(A));
                        _frmMain.COLOR_Back2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                        sr.ReadLine(); // R3.00 NAME SHADOW DIR - Future
                        A = sr.ReadLine();
                        _frmMain.COLOR_Back_Dir = Conversions.ToInteger(A);

                        // R3.00 Rev 4 and above.
                        if (3 < Frev)
                        {
                            sr.ReadLine(); // R3.10 FX COLORS
                            for (var t = 1; t <= 10; t++)
                            {
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                _frmMain.CFX3DC[t] = Color.FromArgb(Ca, Cr, Cg, Cb);
                            }

                            sr.ReadLine(); // R3.10 FX VARS
                            for (var N = 1; N <= 10; N++)
                            for (var t = 1; t <= 10; t++)
                            {
                                A = sr.ReadLine();
                                _frmMain.CFX3DVar[N, t] = Strings.Trim(A);
                            }

                            sr.ReadLine(); // R3.10 FX ACTIVE
                            for (var N = 1; N <= 10; N++)
                            {
                                A = sr.ReadLine();
                                if (A == "True")
                                    _frmMain.CFX3DActive[N] = true;
                                else
                                    _frmMain.CFX3DActive[N] = false;
                            }

                            // **********************************************************
                            // REV 5 and newer files
                            // **********************************************************
                            if (4 < Frev)
                            {
                                sr.ReadLine(); // NAME OVERLAY IMAGE
                                A = sr.ReadLine();
                                frmMain.PATH_Name_OVLBmp = A;
                                A = sr.ReadLine();
                                _frmMain.LSName.OVLScaling = Conversion.Val(A).ToString();
                                sr.ReadLine(); // NOTE 01 VARS
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote01.BDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote01.FDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote01.Height = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote01.O1 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote01.O2 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote01.Scaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                frmMain.LSNote01.OVLScaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote01.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote01.ShadowDepth = A;
                                A = sr.ReadLine();
                                frmMain.LSNote01.ShadowDir = A;
                                A = sr.ReadLine();
                                frmMain.LSNote01.Width = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.PATH_Note01_Bmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note01_BmpPath = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note01_OVLBmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note01_OVLBmpPath = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note01_Name = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note01_Bold = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note01_Italic = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note01_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    A = sr.ReadLine();
                                    _frmMain.NoteAnim01_Text[t] = A;
                                }

                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.Mode = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.Speed = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.TimeHold = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.Align = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.Delim = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.Xoffset = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim01.Yoffset = (int) Math.Round(Conversion.Val(A));
                                sr.ReadLine(); // NOTE 02 VARS
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote02.BDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote02.FDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote02.Height = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote02.O1 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote02.O2 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote02.Scaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                frmMain.LSNote02.OVLScaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote02.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote02.ShadowDepth = A;
                                A = sr.ReadLine();
                                frmMain.LSNote02.ShadowDir = A;
                                A = sr.ReadLine();
                                frmMain.LSNote02.Width = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.PATH_Note02_Bmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note02_BmpPath = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note02_OVLBmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note02_OVLBmpPath = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note02_Name = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note02_Bold = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note02_Italic = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note02_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    A = sr.ReadLine();
                                    _frmMain.NoteAnim02_Text[t] = A;
                                }

                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.Mode = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.Speed = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.TimeHold = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.Align = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.Delim = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.Xoffset = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim02.Yoffset = (int) Math.Round(Conversion.Val(A));
                                sr.ReadLine(); // NOTE 03 VARS
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote03.BDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote03.FDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote03.Height = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote03.O1 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote03.O2 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote03.Scaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                frmMain.LSNote03.OVLScaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote03.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote03.ShadowDepth = A;
                                A = sr.ReadLine();
                                frmMain.LSNote03.ShadowDir = A;
                                A = sr.ReadLine();
                                frmMain.LSNote03.Width = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.PATH_Note03_Bmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note03_BmpPath = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note03_OVLBmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note03_OVLBmpPath = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note03_Name = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note03_Bold = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note03_Italic = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note03_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    A = sr.ReadLine();
                                    _frmMain.NoteAnim03_Text[t] = A;
                                }

                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.Mode = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.Speed = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.TimeHold = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.Align = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.Delim = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.Xoffset = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim03.Yoffset = (int) Math.Round(Conversion.Val(A));
                                sr.ReadLine(); // NOTE 04 VARS
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.B1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.B2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.BackC = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote04.BDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.F1 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.F2 = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote04.FDir = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote04.Height = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote04.O1 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote04.O2 = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.LSNote04.Scaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                frmMain.LSNote04.OVLScaling = Conversion.Val(A).ToString();
                                A = sr.ReadLine();
                                Ca = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cr = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cg = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                Cb = (int) Math.Round(Conversion.Val(A));
                                frmMain.LSNote04.ShadowColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                A = sr.ReadLine();
                                frmMain.LSNote04.ShadowDepth = A;
                                A = sr.ReadLine();
                                frmMain.LSNote04.ShadowDir = A;
                                A = sr.ReadLine();
                                frmMain.LSNote04.Width = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                frmMain.PATH_Note04_Bmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note04_BmpPath = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note04_OVLBmp = A;
                                A = sr.ReadLine();
                                frmMain.PATH_Note04_OVLBmpPath = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note04_Name = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note04_Bold = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note04_Italic = A;
                                A = sr.ReadLine();
                                frmMain.FONT_Note04_Size = A;
                                for (var t = 1; t <= 10; t++)
                                {
                                    A = sr.ReadLine();
                                    _frmMain.NoteAnim04_Text[t] = A;
                                }

                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.Mode = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.Speed = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.TimeHold = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.Align = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.Delim = A;
                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.Xoffset = (int) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.NoteAnim04.Yoffset = (int) Math.Round(Conversion.Val(A));
                                sr.ReadLine(); // NOTE SPACING
                                A = sr.ReadLine();
                                _frmMain.NOTE_Spacing = (int) Math.Round(Conversion.Val(A));
                                sr.ReadLine(); // SOUND SAMPLES
                                for (var t = 1; t <= 30; t++)
                                {
                                    A = sr.ReadLine();
                                    _frmMain.SOUND_File[t] = Strings.Trim(A);
                                    sr.ReadLine(); // R4.00 Future Pitch.
                                    A = sr.ReadLine();
                                    _frmMain.SOUND_Vol[t] = Conversion.Val(A).ToString(); // R4.10 Added.
                                    if (Conversions.ToDouble(_frmMain.SOUND_Vol[t]) < 10d)
                                        _frmMain.SOUND_Vol[t] =
                                            100.ToString(); // R4.10 Old version 5 has no volume so it will be ZERO.  
                                }

                                A = sr.ReadLine();
                                _frmMain.scrVolume.Value = (int) Math.Round(Conversion.Val(A));
                                sr.ReadLine(); // WINDOW STATE
                                A = sr.ReadLine();
                                _frmMain.Celo_Windowstate = Strings.Trim(A);
                                A = sr.ReadLine();
                                _frmMain.Celo_Left = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.Celo_Top = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.Celo_Width = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                _frmMain.Celo_Height = (long) Math.Round(Conversion.Val(A));
                                A = sr.ReadLine();
                                if (A == "1") _frmMain.chkPosition.Checked = true;

                                A = sr.ReadLine();
                                if (A == "1") _frmMain.chkPopUp.Checked = true;
                                //Celo_Popup = true; not used anymore

                                A = sr.ReadLine(); // R4.10 XY OFFSET
                                if (A == "PAGE XY OFFSET")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.tbXoff.Text = Strings.Trim(A);
                                    A = sr.ReadLine();
                                    _frmMain.tbYoff.Text = Strings.Trim(A);
                                }
                                else
                                {
                                    sr.ReadLine();
                                    sr.ReadLine();
                                }

                                A = sr.ReadLine(); // R4.10 XY OFFSET
                                if (A == "PAGE XY SIZE")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.tbXsize.Text = Strings.Trim(A);
                                    A = sr.ReadLine();
                                    _frmMain.tbYSize.Text = Strings.Trim(A);
                                    _frmMain.STATS_ClipXY((float) Conversion.Val(_frmMain.tbXsize.Text),
                                        (float) Conversion.Val(_frmMain.tbYSize.Text));
                                }
                                else
                                {
                                    sr.ReadLine();
                                    sr.ReadLine();
                                }

                                // **********************************************************
                                // REV 6 and newer files
                                // **********************************************************
                                if (5 < Frev)
                                {
                                    A = sr.ReadLine(); // R4.30 HOW OFTEN TO READ THE LOG FILE.
                                    if (A == "SCAN TIME")
                                    {
                                        A = sr.ReadLine();
                                        Vlong = (long) Math.Round(Conversion.Val(A));
                                        if (Vlong < 5L) A = "5";

                                        if (60L < Vlong) A = "60";

                                        switch (A ?? "")
                                        {
                                            case "5":
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 0;
                                                _frmMain.SCAN_Time = 5L;
                                                break;
                                            }

                                            case "10":
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 1;
                                                _frmMain.SCAN_Time = 10L;
                                                break;
                                            }

                                            case "20":
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 2;
                                                _frmMain.SCAN_Time = 20L;
                                                break;
                                            }

                                            case "30":
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 3;
                                                _frmMain.SCAN_Time = 30L;
                                                break;
                                            }

                                            case "45":
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 4;
                                                _frmMain.SCAN_Time = 45L;
                                                break;
                                            }

                                            case "60":
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 5;
                                                _frmMain.SCAN_Time = 60L;
                                                break;
                                            }

                                            default:
                                            {
                                                _frmMain.cboDelay.SelectedIndex = 1;
                                                _frmMain.SCAN_Time = 10L;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sr.ReadLine();
                                    }

                                    A = sr.ReadLine(); // R4.30 PLAY A SOUND WHEN A MATCH IS FOUND.
                                    if (A == "MATCH ALARM ON")
                                    {
                                        A = sr.ReadLine();
                                        _frmMain.chkFoundSound.Checked = A == "1";
                                    }
                                    else
                                    {
                                        sr.ReadLine();
                                    }

                                    A = sr.ReadLine(); // R4.30 DONT DRAW BOXES FOR EMPTY PLAYER SLOTS.
                                    if (A == "HIDE MISSING")
                                    {
                                        A = sr.ReadLine();
                                        _frmMain.chkHideMissing.Checked = A == "1";
                                    }
                                    else
                                    {
                                        sr.ReadLine();
                                    }

                                    A = sr.ReadLine(); // R4.30 MAX PLAYERS FOR EACH GAME MODE.
                                    if (A == "LEVEL STORAGE")
                                        for (var t = 1; t <= 7; t++)
                                        for (var tt = 1; tt <= 4; tt++)
                                        {
                                            A = sr.ReadLine();
                                            _frmMain.LVLS[t, tt] = A;
                                        }

                                    A = sr.ReadLine(); // R4.30 SHOW RANKS, ELO %, and LEVEL. 
                                    if (A == "CYCLE ELO")
                                    {
                                        A = sr.ReadLine();
                                        if (A == "1")
                                        {
                                            _frmMain.chkShowELO.Checked = true;
                                            _frmMain.FLAG_EloUse = true;
                                            _frmMain.timELOCycle.Enabled =
                                                true; // R4.30 Turn on the timer to rotate stats.
                                        }
                                        else
                                        {
                                            _frmMain.chkShowELO.Checked = false;
                                            _frmMain.FLAG_EloUse = false;
                                        }
                                    }
                                    else
                                    {
                                        sr.ReadLine();
                                    }
                                }

                                A = sr.ReadLine(); // R4.34 Use Web Calls and JSON data searches. 
                                if (A == "USE WEB SEARCH")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.chkSpeech.Checked = A == "1";
                                }

                                A = sr.ReadLine(); // R4.34 Read Ranks Aloud. 
                                if (A == "SPEECH RANKS")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.chkSpeech.Checked = A == "1";
                                }

                                A = sr.ReadLine(); // R4.34 Read Ranks Aloud. 
                                if (A == "FIND TEAMS")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.chkGetTeams.Checked = A == "1";
                                }

                                A = sr.ReadLine(); // R4.40 Draw plain player card. 
                                if (A == "PLAYER CARD BACK")
                                {
                                    A = sr.ReadLine();
                                    if (A == "1")
                                    {
                                        _frmMain.LSRank.UseCardBack = true;
                                        _frmMain.LSName.UseCardBack = true;
                                    }
                                    else
                                    {
                                        _frmMain.LSRank.UseCardBack = false;
                                        _frmMain.LSName.UseCardBack = false;
                                    }
                                }

                                // R4.40 Get the RANK border options.
                                A = sr.ReadLine();
                                if (A == "RANK BORDER")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.LSRank.BorderMode = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Ca = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cr = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cg = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cb = (int) Math.Round(Conversion.Val(A));
                                    _frmMain.LSRank.BorderColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                    A = sr.ReadLine();
                                    _frmMain.LSRank.BorderWidth = (int) Math.Round(Conversion.Val(A));

                                    // R4.40 Future Use.
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                }

                                // R4.40 Get the NAME border options.
                                A = sr.ReadLine();
                                if (A == "NAME BORDER")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.LSName.BorderMode = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Ca = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cr = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cg = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cb = (int) Math.Round(Conversion.Val(A));
                                    _frmMain.LSName.BorderColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                    A = sr.ReadLine();
                                    _frmMain.LSName.BorderWidth = (int) Math.Round(Conversion.Val(A));

                                    // R4.40 Future Use.
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                }

                                // R4.40 Get the PANEL border options.
                                A = sr.ReadLine();
                                if (A == "PANEL BORDER")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.LSName.BorderPanelMode = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Ca = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cr = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cg = (int) Math.Round(Conversion.Val(A));
                                    A = sr.ReadLine();
                                    Cb = (int) Math.Round(Conversion.Val(A));
                                    _frmMain.LSName.BorderPanelColor = Color.FromArgb(Ca, Cr, Cg, Cb);
                                    A = sr.ReadLine();
                                    _frmMain.LSName.BorderPanelWidth = (int) Math.Round(Conversion.Val(A));

                                    // R4.40 These must be the same for both RANK and NAME.
                                    _frmMain.LSRank.BorderPanelWidth = _frmMain.LSName.BorderPanelWidth;
                                    _frmMain.LSRank.BorderPanelColor = _frmMain.LSName.BorderPanelColor;
                                    _frmMain.LSRank.BorderPanelMode = _frmMain.LSName.BorderPanelMode;

                                    // R4.40 Future Use.
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                    sr.ReadLine();
                                }

                                A = sr.ReadLine(); // R4.45 Draw flags. 
                                if (A == "SHOW COUNTRY")
                                {
                                    A = sr.ReadLine();
                                    _frmMain.chkCountry.Checked = A == "1";
                                }

                                A = sr.ReadLine(); // R4.45 Draw flags. 
                                if (A == "SHOW OVERLAY")
                                {
                                    A = sr.ReadLine();
                                    _frmMain._chkToggleOverlay.Checked = A == "1";
                                }
                            } // <-- REV 5 END
                        } // <-- REV 4 END
                    } // <-- REV 3 END
                } // <-- REV 2 END
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(
                    "ERROR: " + ex.Message + Constants.vbCr + Constants.vbCr + "Unable to read the saved settings." +
                    Constants.vbCr + "The last known setup could not be loaded." + Constants.vbCr + Constants.vbCr +
                    "If running a new version, this error may fix itself when a new setup is saved.",
                    MsgBoxStyle.Critical, "MakoCelo - Setup Error");
            }
            finally
            {
                // R4.00 Close / Clean up  streams?
                if (Information.IsNothing(sr) == false)
                {
                    sr.Close();
                    sr.Dispose();
                }

                if (Information.IsNothing(fs) == false)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

            _frmMain.FX_SetVarControls();
            _frmMain.SETTINGS_SetupAfterLoad();
        }

        public void SETTINGS_Save(string tFile)
        {
            string A;
            Color C;
            string tPath;
            if (_frmMain.FLAG_Loading) return;

            // R4.00 OPEN log file and start parsing.
            var fs = default(FileStream);
            var sw = default(StreamWriter);
            tPath = Application.StartupPath;
            try
            {
                fs = string.IsNullOrEmpty(tFile)
                    ? new FileStream(tPath + @"\MakoCelo_settings.dat", FileMode.OpenOrCreate)
                    : new FileStream(tFile, FileMode.OpenOrCreate);

                sw = new StreamWriter(fs, Encoding.UTF8);
                sw.WriteLine("VERSION MC600");
                C = _frmMain.LSName.F1; // R3.30 lbRank01.ForeColor
                sw.WriteLine("RANK FORE COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.LSName.B1; // R3.30 lbRank01.BackColor
                sw.WriteLine("RANK BACK COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("RANK ALPHA %");
                sw.WriteLine(_frmMain.LSRank.O1);
                sw.WriteLine("BACK IMAGE");
                sw.WriteLine(frmMain.PATH_BackgroundImage);
                sw.WriteLine("LOG PATH");
                sw.WriteLine(_frmMain.PATH_Game);
                sw.WriteLine("RANK FONT");
                sw.WriteLine(frmMain.FONT_Rank_Name);
                sw.WriteLine(frmMain.FONT_Rank_Size);
                sw.WriteLine(frmMain.FONT_Rank_Bold);
                sw.WriteLine(frmMain.FONT_Rank_Italic);
                C = _frmMain.LSName.F1; // R3.30 lbName01.ForeColor
                sw.WriteLine("NAME FORE COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.LSName.B1; // R3.30 lbName01.BackColor
                sw.WriteLine("NAME BACK COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("NAME ALPHA %");
                sw.WriteLine(_frmMain.LSName.O1);
                sw.WriteLine("NAME FONT");
                sw.WriteLine(frmMain.FONT_Name_Name);
                sw.WriteLine(frmMain.FONT_Name_Size);
                sw.WriteLine(frmMain.FONT_Name_Bold);
                sw.WriteLine(frmMain.FONT_Name_Italic);
                sw.WriteLine("SCREEN SIZE");
                sw.WriteLine(" "); // R4.10 Changed to actual XY size vars at bottom.
                sw.WriteLine("PAGE LAYOUT Y");
                sw.WriteLine(_frmMain.cboLayoutY.Text);
                sw.WriteLine("PAGE LAYOUT X");
                sw.WriteLine(_frmMain.cboLayoutX.Text);
                C = _frmMain.pbStats.BackColor; // R3.30 pnlPlrz.BackColor
                sw.WriteLine("PANEL BACK COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("IMAGE SCALING");
                sw.WriteLine(_frmMain.LSName.Scaling);
                sw.WriteLine("GUI COLOR");
                sw.WriteLine(_frmMain.GUI_ColorMode);
                C = _frmMain.LSRank.F1;
                sw.WriteLine("RANK FORE COLOR 1");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.LSRank.F2;
                sw.WriteLine("RANK FORE COLOR 2");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("RANK FORE GRADIENT DIR");
                sw.WriteLine(_frmMain.LSRank.FDir);
                C = _frmMain.LSRank.B1;
                sw.WriteLine("RANK BACK COLOR 1");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.LSRank.B2;
                sw.WriteLine("RANK BACK COLOR 2");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("RANK BACK GRADIENT DIR");
                sw.WriteLine(_frmMain.LSRank.BDir);
                C = _frmMain.LSName.F1;
                sw.WriteLine("NAME FORE COLOR 1");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.LSName.F2;
                sw.WriteLine("NAME FORE COLOR 2");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("NAME FORE GRADIENT DIR");
                sw.WriteLine(_frmMain.LSName.FDir);
                C = _frmMain.LSName.B1;
                sw.WriteLine("NAME BACK COLOR 1");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.LSName.B2;
                sw.WriteLine("NAME BACK COLOR 2");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("NAME BACK GRADIENT DIR");
                sw.WriteLine(_frmMain.LSName.BDir);
                C = _frmMain.LSRank.ShadowColor;
                sw.WriteLine("RANK SHADOW COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("RANK SHADOW DIR");
                sw.WriteLine(_frmMain.LSRank.ShadowDir);
                sw.WriteLine("RANK SHADOW DEPTH");
                if (string.IsNullOrEmpty(_frmMain.LSRank.ShadowDepth)) _frmMain.LSRank.ShadowDepth = "";

                sw.WriteLine(_frmMain.LSRank.ShadowDepth);
                C = _frmMain.LSName.ShadowColor;
                sw.WriteLine("NAME SHADOW COLOR");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("NAME SHADOW DIR");
                sw.WriteLine(_frmMain.LSName.ShadowDir);
                sw.WriteLine("NAME SHADOW DEPTH - Future");
                sw.WriteLine(_frmMain.LSName.ShadowDepth);
                C = _frmMain.COLOR_Back1;
                sw.WriteLine("BACK GRADIENT COLOR 1 - Future");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                C = _frmMain.COLOR_Back2;
                sw.WriteLine("BACK GRADIENT COLOR 2 - Future");
                sw.WriteLine(C.A);
                sw.WriteLine(C.R);
                sw.WriteLine(C.G);
                sw.WriteLine(C.B);
                sw.WriteLine("BACK GRADIENT DIR - Future");
                sw.WriteLine(_frmMain.COLOR_Back_Dir);
                sw.WriteLine("FX COLORS");
                for (var t = 1; t <= 10; t++)
                {
                    C = _frmMain.CFX3DC[t];
                    sw.WriteLine(C.A);
                    sw.WriteLine(C.R);
                    sw.WriteLine(C.G);
                    sw.WriteLine(C.B);
                }

                sw.WriteLine("FX VARS");
                for (var N = 1; N <= 10; N++)
                    for (var t = 1; t <= 10; t++)
                    {
                        if (string.IsNullOrEmpty(_frmMain.CFX3DVar[N, t])) _frmMain.CFX3DVar[N, t] = "";

                        sw.WriteLine(_frmMain.CFX3DVar[N, t]);
                    }

                sw.WriteLine("FX ACTIVE");
                for (var N = 1; N <= 10; N++) sw.WriteLine(_frmMain.CFX3DActive[N] ? "True" : "False");

                // *********************************
                // R4.00 ADDED!
                // *********************************
                sw.WriteLine("NAME OVERLAY IMAGE");
                sw.WriteLine(frmMain.PATH_Name_OVLBmp);
                sw.WriteLine(_frmMain.LSName.OVLScaling);
                sw.WriteLine("NOTE 01 VARS");
                sw.WriteLine(frmMain.LSNote01.B1.A);
                sw.WriteLine(frmMain.LSNote01.B1.R);
                sw.WriteLine(frmMain.LSNote01.B1.G);
                sw.WriteLine(frmMain.LSNote01.B1.B);
                sw.WriteLine(frmMain.LSNote01.B2.A);
                sw.WriteLine(frmMain.LSNote01.B2.R);
                sw.WriteLine(frmMain.LSNote01.B2.G);
                sw.WriteLine(frmMain.LSNote01.B2.B);
                sw.WriteLine(frmMain.LSNote01.BackC.A);
                sw.WriteLine(frmMain.LSNote01.BackC.R);
                sw.WriteLine(frmMain.LSNote01.BackC.G);
                sw.WriteLine(frmMain.LSNote01.BackC.B);
                sw.WriteLine(frmMain.LSNote01.BDir);
                sw.WriteLine(frmMain.LSNote01.F1.A);
                sw.WriteLine(frmMain.LSNote01.F1.R);
                sw.WriteLine(frmMain.LSNote01.F1.G);
                sw.WriteLine(frmMain.LSNote01.F1.B);
                sw.WriteLine(frmMain.LSNote01.F2.A);
                sw.WriteLine(frmMain.LSNote01.F2.R);
                sw.WriteLine(frmMain.LSNote01.F2.G);
                sw.WriteLine(frmMain.LSNote01.F2.B);
                sw.WriteLine(frmMain.LSNote01.FDir);
                sw.WriteLine(frmMain.LSNote01.Height);
                sw.WriteLine(frmMain.LSNote01.O1);
                sw.WriteLine(frmMain.LSNote01.O2);
                sw.WriteLine(frmMain.LSNote01.Scaling);
                sw.WriteLine(frmMain.LSNote01.OVLScaling);
                sw.WriteLine(frmMain.LSNote01.ShadowColor.A);
                sw.WriteLine(frmMain.LSNote01.ShadowColor.R);
                sw.WriteLine(frmMain.LSNote01.ShadowColor.G);
                sw.WriteLine(frmMain.LSNote01.ShadowColor.B);
                sw.WriteLine(frmMain.LSNote01.ShadowDepth);
                sw.WriteLine(frmMain.LSNote01.ShadowDir);
                sw.WriteLine(frmMain.LSNote01.Width);
                sw.WriteLine(frmMain.PATH_Note01_Bmp);
                sw.WriteLine(frmMain.PATH_Note01_BmpPath);
                sw.WriteLine(frmMain.PATH_Note01_OVLBmp);
                sw.WriteLine(frmMain.PATH_Note01_OVLBmpPath);
                sw.WriteLine(frmMain.FONT_Note01_Name);
                sw.WriteLine(frmMain.FONT_Note01_Bold);
                sw.WriteLine(frmMain.FONT_Note01_Italic);
                sw.WriteLine(frmMain.FONT_Note01_Size);
                for (var t = 1; t <= 10; t++) sw.WriteLine(_frmMain.NoteAnim01_Text[t]);

                sw.WriteLine(_frmMain.NoteAnim01.Mode);
                sw.WriteLine(_frmMain.NoteAnim01.Speed);
                sw.WriteLine(_frmMain.NoteAnim01.TimeHold);
                sw.WriteLine(_frmMain.NoteAnim01.Align);
                sw.WriteLine(_frmMain.NoteAnim01.Delim);
                sw.WriteLine(_frmMain.NoteAnim01.Xoffset);
                sw.WriteLine(_frmMain.NoteAnim01.Yoffset);
                sw.WriteLine("NOTE 02 VARS");
                sw.WriteLine(frmMain.LSNote02.B1.A);
                sw.WriteLine(frmMain.LSNote02.B1.R);
                sw.WriteLine(frmMain.LSNote02.B1.G);
                sw.WriteLine(frmMain.LSNote02.B1.B);
                sw.WriteLine(frmMain.LSNote02.B2.A);
                sw.WriteLine(frmMain.LSNote02.B2.R);
                sw.WriteLine(frmMain.LSNote02.B2.G);
                sw.WriteLine(frmMain.LSNote02.B2.B);
                sw.WriteLine(frmMain.LSNote02.BackC.A);
                sw.WriteLine(frmMain.LSNote02.BackC.R);
                sw.WriteLine(frmMain.LSNote02.BackC.G);
                sw.WriteLine(frmMain.LSNote02.BackC.B);
                sw.WriteLine(frmMain.LSNote02.BDir);
                sw.WriteLine(frmMain.LSNote02.F1.A);
                sw.WriteLine(frmMain.LSNote02.F1.R);
                sw.WriteLine(frmMain.LSNote02.F1.G);
                sw.WriteLine(frmMain.LSNote02.F1.B);
                sw.WriteLine(frmMain.LSNote02.F2.A);
                sw.WriteLine(frmMain.LSNote02.F2.R);
                sw.WriteLine(frmMain.LSNote02.F2.G);
                sw.WriteLine(frmMain.LSNote02.F2.B);
                sw.WriteLine(frmMain.LSNote02.FDir);
                sw.WriteLine(frmMain.LSNote02.Height);
                sw.WriteLine(frmMain.LSNote02.O1);
                sw.WriteLine(frmMain.LSNote02.O2);
                sw.WriteLine(frmMain.LSNote02.Scaling);
                sw.WriteLine(frmMain.LSNote02.OVLScaling);
                sw.WriteLine(frmMain.LSNote02.ShadowColor.A);
                sw.WriteLine(frmMain.LSNote02.ShadowColor.R);
                sw.WriteLine(frmMain.LSNote02.ShadowColor.G);
                sw.WriteLine(frmMain.LSNote02.ShadowColor.B);
                sw.WriteLine(frmMain.LSNote02.ShadowDepth);
                sw.WriteLine(frmMain.LSNote02.ShadowDir);
                sw.WriteLine(frmMain.LSNote02.Width);
                sw.WriteLine(frmMain.PATH_Note02_Bmp);
                sw.WriteLine(frmMain.PATH_Note02_BmpPath);
                sw.WriteLine(frmMain.PATH_Note02_OVLBmp);
                sw.WriteLine(frmMain.PATH_Note02_OVLBmpPath);
                sw.WriteLine(frmMain.FONT_Note02_Name);
                sw.WriteLine(frmMain.FONT_Note02_Bold);
                sw.WriteLine(frmMain.FONT_Note02_Italic);
                sw.WriteLine(frmMain.FONT_Note02_Size);
                for (var t = 1; t <= 10; t++) sw.WriteLine(_frmMain.NoteAnim02_Text[t]);

                sw.WriteLine(_frmMain.NoteAnim02.Mode);
                sw.WriteLine(_frmMain.NoteAnim02.Speed);
                sw.WriteLine(_frmMain.NoteAnim02.TimeHold);
                sw.WriteLine(_frmMain.NoteAnim02.Align);
                sw.WriteLine(_frmMain.NoteAnim02.Delim);
                sw.WriteLine(_frmMain.NoteAnim02.Xoffset);
                sw.WriteLine(_frmMain.NoteAnim02.Yoffset);
                sw.WriteLine("NOTE 03 VARS");
                sw.WriteLine(frmMain.LSNote03.B1.A);
                sw.WriteLine(frmMain.LSNote03.B1.R);
                sw.WriteLine(frmMain.LSNote03.B1.G);
                sw.WriteLine(frmMain.LSNote03.B1.B);
                sw.WriteLine(frmMain.LSNote03.B2.A);
                sw.WriteLine(frmMain.LSNote03.B2.R);
                sw.WriteLine(frmMain.LSNote03.B2.G);
                sw.WriteLine(frmMain.LSNote03.B2.B);
                sw.WriteLine(frmMain.LSNote03.BackC.A);
                sw.WriteLine(frmMain.LSNote03.BackC.R);
                sw.WriteLine(frmMain.LSNote03.BackC.G);
                sw.WriteLine(frmMain.LSNote03.BackC.B);
                sw.WriteLine(frmMain.LSNote03.BDir);
                sw.WriteLine(frmMain.LSNote03.F1.A);
                sw.WriteLine(frmMain.LSNote03.F1.R);
                sw.WriteLine(frmMain.LSNote03.F1.G);
                sw.WriteLine(frmMain.LSNote03.F1.B);
                sw.WriteLine(frmMain.LSNote03.F2.A);
                sw.WriteLine(frmMain.LSNote03.F2.R);
                sw.WriteLine(frmMain.LSNote03.F2.G);
                sw.WriteLine(frmMain.LSNote03.F2.B);
                sw.WriteLine(frmMain.LSNote03.FDir);
                sw.WriteLine(frmMain.LSNote03.Height);
                sw.WriteLine(frmMain.LSNote03.O1);
                sw.WriteLine(frmMain.LSNote03.O2);
                sw.WriteLine(frmMain.LSNote03.Scaling);
                sw.WriteLine(frmMain.LSNote03.OVLScaling);
                sw.WriteLine(frmMain.LSNote03.ShadowColor.A);
                sw.WriteLine(frmMain.LSNote03.ShadowColor.R);
                sw.WriteLine(frmMain.LSNote03.ShadowColor.G);
                sw.WriteLine(frmMain.LSNote03.ShadowColor.B);
                sw.WriteLine(frmMain.LSNote03.ShadowDepth);
                sw.WriteLine(frmMain.LSNote03.ShadowDir);
                sw.WriteLine(frmMain.LSNote03.Width);
                sw.WriteLine(frmMain.PATH_Note03_Bmp);
                sw.WriteLine(frmMain.PATH_Note03_BmpPath);
                sw.WriteLine(frmMain.PATH_Note03_OVLBmp);
                sw.WriteLine(frmMain.PATH_Note03_OVLBmpPath);
                sw.WriteLine(frmMain.FONT_Note03_Name);
                sw.WriteLine(frmMain.FONT_Note03_Bold);
                sw.WriteLine(frmMain.FONT_Note03_Italic);
                sw.WriteLine(frmMain.FONT_Note03_Size);
                for (var t = 1; t <= 10; t++) sw.WriteLine(_frmMain.NoteAnim03_Text[t]);

                sw.WriteLine(_frmMain.NoteAnim03.Mode);
                sw.WriteLine(_frmMain.NoteAnim03.Speed);
                sw.WriteLine(_frmMain.NoteAnim03.TimeHold);
                sw.WriteLine(_frmMain.NoteAnim03.Align);
                sw.WriteLine(_frmMain.NoteAnim03.Delim);
                sw.WriteLine(_frmMain.NoteAnim03.Xoffset);
                sw.WriteLine(_frmMain.NoteAnim03.Yoffset);
                sw.WriteLine("NOTE 04 VARS");
                sw.WriteLine(frmMain.LSNote04.B1.A);
                sw.WriteLine(frmMain.LSNote04.B1.R);
                sw.WriteLine(frmMain.LSNote04.B1.G);
                sw.WriteLine(frmMain.LSNote04.B1.B);
                sw.WriteLine(frmMain.LSNote04.B2.A);
                sw.WriteLine(frmMain.LSNote04.B2.R);
                sw.WriteLine(frmMain.LSNote04.B2.G);
                sw.WriteLine(frmMain.LSNote04.B2.B);
                sw.WriteLine(frmMain.LSNote04.BackC.A);
                sw.WriteLine(frmMain.LSNote04.BackC.R);
                sw.WriteLine(frmMain.LSNote04.BackC.G);
                sw.WriteLine(frmMain.LSNote04.BackC.B);
                sw.WriteLine(frmMain.LSNote04.BDir);
                sw.WriteLine(frmMain.LSNote04.F1.A);
                sw.WriteLine(frmMain.LSNote04.F1.R);
                sw.WriteLine(frmMain.LSNote04.F1.G);
                sw.WriteLine(frmMain.LSNote04.F1.B);
                sw.WriteLine(frmMain.LSNote04.F2.A);
                sw.WriteLine(frmMain.LSNote04.F2.R);
                sw.WriteLine(frmMain.LSNote04.F2.G);
                sw.WriteLine(frmMain.LSNote04.F2.B);
                sw.WriteLine(frmMain.LSNote04.FDir);
                sw.WriteLine(frmMain.LSNote04.Height);
                sw.WriteLine(frmMain.LSNote04.O1);
                sw.WriteLine(frmMain.LSNote04.O2);
                sw.WriteLine(frmMain.LSNote04.Scaling);
                sw.WriteLine(frmMain.LSNote04.OVLScaling);
                sw.WriteLine(frmMain.LSNote04.ShadowColor.A);
                sw.WriteLine(frmMain.LSNote04.ShadowColor.R);
                sw.WriteLine(frmMain.LSNote04.ShadowColor.G);
                sw.WriteLine(frmMain.LSNote04.ShadowColor.B);
                sw.WriteLine(frmMain.LSNote04.ShadowDepth);
                sw.WriteLine(frmMain.LSNote04.ShadowDir);
                sw.WriteLine(frmMain.LSNote04.Width);
                sw.WriteLine(frmMain.PATH_Note04_Bmp);
                sw.WriteLine(frmMain.PATH_Note04_BmpPath);
                sw.WriteLine(frmMain.PATH_Note04_OVLBmp);
                sw.WriteLine(frmMain.PATH_Note04_OVLBmpPath);
                sw.WriteLine(frmMain.FONT_Note04_Name);
                sw.WriteLine(frmMain.FONT_Note04_Bold);
                sw.WriteLine(frmMain.FONT_Note04_Italic);
                sw.WriteLine(frmMain.FONT_Note04_Size);
                for (var t = 1; t <= 10; t++) sw.WriteLine(_frmMain.NoteAnim04_Text[t]);

                sw.WriteLine(_frmMain.NoteAnim04.Mode);
                sw.WriteLine(_frmMain.NoteAnim04.Speed);
                sw.WriteLine(_frmMain.NoteAnim04.TimeHold);
                sw.WriteLine(_frmMain.NoteAnim04.Align);
                sw.WriteLine(_frmMain.NoteAnim04.Delim);
                sw.WriteLine(_frmMain.NoteAnim04.Xoffset);
                sw.WriteLine(_frmMain.NoteAnim04.Yoffset);
                sw.WriteLine("NOTE SPACING");
                sw.WriteLine(_frmMain.NOTE_Spacing);
                sw.WriteLine("SOUND SAMPLES");
                for (var t = 1; t <= 30; t++)
                {
                    sw.WriteLine(_frmMain.SOUND_File[t]); // R4.00 WAV file path.
                    sw.WriteLine(" "); // R4.00 Future Pitch/Speed.
                    sw.WriteLine(_frmMain.SOUND_Vol[t]); // R4.10 Volume.
                }

                sw.WriteLine(_frmMain.scrVolume.Value);
                sw.WriteLine("WINDOW STATE");
                A = "Normal";
                if ((int)_frmMain.WindowState == 1) A = "Minimized";

                if ((int)_frmMain.WindowState == 2) A = "Maximized";

                sw.WriteLine(A);
                sw.WriteLine(_frmMain.Location.X);
                sw.WriteLine(_frmMain.Location.Y);
                sw.WriteLine(_frmMain.Size.Width);
                sw.WriteLine(_frmMain.Size.Height);
                sw.WriteLine(_frmMain.chkPosition.Checked ? "1" : "0");

                sw.WriteLine(_frmMain.chkPopUp.Checked ? "1" : "0");

                sw.WriteLine("PAGE XY OFFSET");
                sw.WriteLine(_frmMain.tbXoff.Text);
                sw.WriteLine(_frmMain.tbYoff.Text);
                sw.WriteLine("PAGE XY SIZE");
                sw.WriteLine(_frmMain.STATS_SizeX);
                sw.WriteLine(_frmMain.STATS_SizeY);

                // R4.30 Added.
                sw.WriteLine("SCAN TIME");
                sw.WriteLine((int)Conversion.Val(_frmMain.cboDelay.Items[_frmMain.cboDelay.SelectedIndex]));

                // R4.30 Added.
                sw.WriteLine("MATCH ALARM ON");
                sw.WriteLine(_frmMain.chkFoundSound.Checked ? "1" : "0");

                // R4.30 Added.
                sw.WriteLine("HIDE MISSING");
                sw.WriteLine(_frmMain.chkHideMissing.Checked ? "1" : "0");

                sw.WriteLine("LEVEL STORAGE");
                for (var t = 1; t <= 7; t++)
                    for (var tt = 1; tt <= 4; tt++)
                        sw.WriteLine(_frmMain.LVLS[t, tt]);

                // R4.30 Added.
                sw.WriteLine("CYCLE ELO");
                sw.WriteLine(_frmMain.chkShowELO.Checked ? "1" : "0");

                // R4.34 Added until Relic fixes the Warngins.Log file.
                sw.WriteLine("USE WEB SEARCH");
                sw.WriteLine("1");

                // R4.34 Added.
                sw.WriteLine("SPEECH RANKS");
                sw.WriteLine(_frmMain.chkSpeech.Checked ? "1" : "0");

                // R4.34 Added.
                sw.WriteLine("FIND TEAMS");
                sw.WriteLine(_frmMain.chkGetTeams.Checked ? "1" : "0");

                // R4.40 Added. LSname and LSrank must be the same.
                sw.WriteLine("PLAYER CARD BACK");
                sw.WriteLine(_frmMain.LSName.UseCardBack ? "1" : "0");

                // R4.40 Added. LSname and LSrank must be the same.
                sw.WriteLine("RANK BORDER");
                sw.WriteLine(_frmMain.LSRank.BorderMode);
                sw.WriteLine(_frmMain.LSRank.BorderColor.A);
                sw.WriteLine(_frmMain.LSRank.BorderColor.R);
                sw.WriteLine(_frmMain.LSRank.BorderColor.G);
                sw.WriteLine(_frmMain.LSRank.BorderColor.B);
                sw.WriteLine(_frmMain.LSRank.BorderWidth);
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");

                // R4.40 Added. LSname and LSrank must be the same.
                sw.WriteLine("NAME BORDER");
                sw.WriteLine(_frmMain.LSName.BorderMode);
                sw.WriteLine(_frmMain.LSName.BorderColor.A);
                sw.WriteLine(_frmMain.LSName.BorderColor.R);
                sw.WriteLine(_frmMain.LSName.BorderColor.G);
                sw.WriteLine(_frmMain.LSName.BorderColor.B);
                sw.WriteLine(_frmMain.LSName.BorderWidth);
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");

                // R4.40 Added. LSname and LSrank must be the same.
                sw.WriteLine("PANEL BORDER");
                sw.WriteLine(_frmMain.LSRank.BorderPanelMode);
                sw.WriteLine(_frmMain.LSRank.BorderPanelColor.A);
                sw.WriteLine(_frmMain.LSRank.BorderPanelColor.R);
                sw.WriteLine(_frmMain.LSRank.BorderPanelColor.G);
                sw.WriteLine(_frmMain.LSRank.BorderPanelColor.B);
                sw.WriteLine(_frmMain.LSRank.BorderPanelWidth);
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");
                sw.WriteLine("0");

                // R4.45 Added.
                sw.WriteLine("SHOW COUNTRY");
                sw.WriteLine(_frmMain.chkCountry.Checked ? "1" : "0");

                // R5.00 Added.
                sw.WriteLine("SHOW OVERLAY");
                sw.WriteLine(_frmMain._chkToggleOverlay.Checked ? "1" : "0");

                // R3.10 Write some extra lines to stop file open fails on future revs.
                for (var t = 1; t <= 100; t++) sw.WriteLine(" ");
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(
                    "ERROR: " + ex.Message + Constants.vbCr + Constants.vbCr + "Unable to save the last known setup.",
                    MsgBoxStyle.Critical, "MakoCelo - Setup Error");
            }

            // R4.00 Close / Clean up  streams?
            if (Information.IsNothing(sw) == false)
            {
                sw.Close();
                sw.Dispose();
            }

            if (Information.IsNothing(fs) == false)
            {
                fs.Close();
                fs.Dispose();
            }
        }
    }
}