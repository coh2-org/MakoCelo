﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Forms;
using GameOverlay;
using MakoCelo.My.Resources;
using MakoCelo.Overlay;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using FileSystem = Microsoft.VisualBasic.FileSystem;

namespace MakoCelo
{
    public partial class frmMain
    {
        public static string PATH_BackgroundImage = "";

        // R4.00 Need these global so we can set them in frmLabelSetup objects. Could be properties.
        public static string PATH_DlgBmp = ""; // R4.00 Added NOTE objects.  
        public static string PATH_DlgBmpPath = ""; // R4.00 Path only.  
        public static string PATH_Note01_Bmp = "";
        public static string PATH_Note02_Bmp = "";
        public static string PATH_Note03_Bmp = "";
        public static string PATH_Note04_Bmp = "";
        public static string PATH_Note01_BmpPath = ""; // R4.00 Filename removed. Path only for dialog boxes.  
        public static string PATH_Note02_BmpPath = "";
        public static string PATH_Note03_BmpPath = "";
        public static string PATH_Note04_BmpPath = "";
        public static string PATH_DlgOVLBmp = ""; // R4.00 Added NOTE objects.  
        public static string PATH_DlgOVLBmpPath = ""; // R4.00 Path only.  
        public static string PATH_Name_OVLBmp = "";
        public static string PATH_Note01_OVLBmp = "";
        public static string PATH_Note02_OVLBmp = "";
        public static string PATH_Note03_OVLBmp = "";
        public static string PATH_Note04_OVLBmp = "";
        public static string PATH_Name_OVLBmpPath = "";
        public static string PATH_Note01_OVLBmpPath = ""; // R4.00 Filename removed. Path only for dialog boxes.  
        public static string PATH_Note02_OVLBmpPath = "";
        public static string PATH_Note03_OVLBmpPath = "";
        public static string PATH_Note04_OVLBmpPath = "";

        // R4.00 Need these global so we can set them in frmLabelSetup objects. Could be properties.
        public static Font FONT_Setup; // R4.00 Added a var for temporary dialog setups.
        public static string FONT_Setup_Name;
        public static string FONT_Setup_Size;
        public static string FONT_Setup_Bold;
        public static string FONT_Setup_Italic;
        public static Font FONT_Note01; // R4.00 Note Font setup.
        public static string FONT_Note01_Name;
        public static string FONT_Note01_Size;
        public static string FONT_Note01_Bold;
        public static string FONT_Note01_Italic;
        public static Font FONT_Note02; // R4.00 Note Font setup.
        public static string FONT_Note02_Name;
        public static string FONT_Note02_Size;
        public static string FONT_Note02_Bold;
        public static string FONT_Note02_Italic;
        public static Font FONT_Note03; // R4.00 Note Font setup.
        public static string FONT_Note03_Name;
        public static string FONT_Note03_Size;
        public static string FONT_Note03_Bold;
        public static string FONT_Note03_Italic;
        public static Font FONT_Note04; // R4.00 Note Font setup.
        public static string FONT_Note04_Name;
        public static string FONT_Note04_Size;
        public static string FONT_Note04_Bold;
        public static string FONT_Note04_Italic;
        public static Font FONT_Rank; // R3.00 Added.
        public static string FONT_Rank_Name;
        public static string FONT_Rank_Size;
        public static string FONT_Rank_Bold;
        public static string FONT_Rank_Italic;
        public static Font FONT_Name; // R3.00 Added.
        public static string FONT_Name_Name;
        public static string FONT_Name_Size;
        public static string FONT_Name_Bold;
        public static string FONT_Name_Italic;
        public static clsGlobal.t_LabelSetup LSNote01;
        public static clsGlobal.t_LabelSetup LSNote02;
        public static clsGlobal.t_LabelSetup LSNote03;
        public static clsGlobal.t_LabelSetup LSNote04;

        // R4.00 Need these global so we can set them in frmLabelSetup objects. Could be properties.
        public static Bitmap Note_BackBmp; // R4.00 Setup var for dialogs.
        public static Bitmap Note01_BackBmp; // R4.00 Note background images.
        public static Bitmap Note02_BackBmp;
        public static Bitmap Note03_BackBmp;
        public static Bitmap Note04_BackBmp;
        public static Bitmap Note_OVLBmp; // R4.00 Setup var for dialogs.
        public static Bitmap Note01_OVLBmp; // R4.00 Note OVERLAY image.
        public static Bitmap Note02_OVLBmp;
        public static Bitmap Note03_OVLBmp;
        public static Bitmap Note04_OVLBmp;
        public readonly Coh2Overlay _overlay = new();
        private readonly SoundPlayer _soundPlayer = new();
        public readonly bool[] CFX3DActive = new bool[11];
        public readonly Color[] CFX3DC = new Color[11];
        public readonly string[,] CFX3DVar = new string[11, 11]; // R3.10 1=Mode
        private readonly string[] Country_Abbr = new string[301];

        // R4.46 Added Country Strings.
        private readonly string[] Country_Name = new string[301];
        private readonly clsGlobal.t_Box[] LAB_Fact = new clsGlobal.t_Box[9];
        private readonly clsGlobal.t_Box[] LAB_Name = new clsGlobal.t_Box[9];
        private readonly StringFormat[] LAB_Name_Align = new StringFormat[9];
        private readonly clsGlobal.t_Box[] LAB_Rank = new clsGlobal.t_Box[9]; // R2.00 Defs for current label layout. 
        private readonly float[] LVLpercs = new float[21];
        public readonly string[,] LVLS = new string[8, 5];
        public readonly string[] PlrCountry = new string[9]; // R4.45 Added contry.
        public readonly string[] PlrCountry_Buffer = new string[9];
        private readonly string[] PlrCountry_Last = new string[9];
        public readonly string[] PlrCountryName = new string[9]; // R4.46 Added contry.
        public readonly string[] PlrCountryName_Buffer = new string[9]; // R4.46 Added contry.
        private readonly string[] PlrCountryName_Last = new string[9]; // R4.46 Added contry.
        public readonly string[] PlrELO = new string[9];
        public readonly string[] PlrELO_Buffer = new string[9];
        private readonly string[] PlrELO_Last = new string[9];
        public readonly string[] PlrFact = new string[9];
        public readonly string[] PlrFact_Buffer = new string[9];
        private readonly string[] PlrFact_Last = new string[9];
        public readonly int[] PlrGLVL = new int[9];
        public readonly int[] PlrGLVL_Buffer = new int[9];
        private readonly int[] PlrGLVL_Last = new int[9];
        public readonly int[] PlrLoss = new int[9];
        public readonly string[] PlrLVL = new string[9];
        public readonly string[] PlrLVL_Buffer = new string[9];
        private readonly string[] PlrLVL_Last = new string[9];
        public readonly string[] PlrName = new string[9];
        public readonly string[] PlrName_Buffer = new string[9];
        private readonly string[] PlrName_Last = new string[9];
        public readonly string[] PlrRank = new string[9];
        public readonly string[] PlrRank_Buffer = new string[9];
        private readonly string[] PlrRank_Last = new string[9];
        public readonly int[,,] PlrRankALL = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly int[,,] PlrRankALL_Buffer = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        private readonly int[,,] PlrRankALL_Last = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly int[,,] PlrRankLoss = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly int[,,] PlrRankLoss_Buffer = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        private readonly int[,,] PlrRankLoss_Last = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly string[,,] PlrRankPerc = new string[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly string[,,] PlrRankPerc_Buffer = new string[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        private readonly string[,,] PlrRankPerc_Last = new string[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly int[,,] PlrRankWin = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly int[,,] PlrRankWin_Buffer = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        private readonly int[,,] PlrRankWin_Last = new int[9, 8, 5]; // R4.30 Rank from RID for all game modes.
        public readonly int[] PlrRID = new int[9];
        public readonly int[] PlrRID_Buffer = new int[9];
        private readonly int[] PlrRID_Last = new int[9];
        public readonly string[] PlrSteam = new string[9];
        public readonly string[] PlrSteam_Buffer = new string[9];
        private readonly string[] PlrSteam_Last = new string[9];
        public readonly int[] PlrTeam = new int[9];
        public readonly int[] PlrTeam_Buffer = new int[9];
        private readonly int[] PlrTeam_Last = new int[9];
        public readonly int[] PlrTLoss = new int[9];
        public readonly int[] PlrTLoss_Buffer = new int[9];
        private readonly int[] PlrTLoss_Last = new int[9];
        public readonly int[] PlrTWin = new int[9];
        public readonly int[] PlrTWin_Buffer = new int[9];
        private readonly int[] PlrTWin_Last = new int[9];
        public readonly int[] PlrWin = new int[9];

        // R4.30 Get player info from RELICID
        // R4.30 ID for each game mode.
        private readonly string[,] RelDataLeaderID = new string[8, 5];
        public readonly string[] SOUND_File = new string[31]; // R4.00 Added.

        public readonly string[] SOUND_Vol = new string[31]; // R4.10 Added.
        // R4.00 NEEDS A fast check to see if LOG file has been updated since last check, since it hiccups the animation code.
        // R4.00 NEEDS Check could be file size and date?
        // R4.00 NEEDS Could put STATS on clipboard for pasting into Coh2 Console for people who cant Alt-Tab without crashing.


        private readonly SpeechSynthesizer SpeechSynth = new(); // R4.34 Added for TEXT-TO-SPEECH option.

        public readonly clsGlobal.t_TeamList[,]
            TeamList = new clsGlobal.t_TeamList[10, 1001]; // R4.34 Added for JSON team parsing.

        public readonly clsGlobal.t_TeamList[,]
            TeamList_Buffer = new clsGlobal.t_TeamList[10, 1001]; // R4.34 Added for JSON team parsing.

        private readonly clsGlobal.t_TeamList[,]
            TeamList_Last = new clsGlobal.t_TeamList[10, 1001]; // R4.34 Added for JSON team parsing.

        public readonly int[] TeamListCnt = new int[10];
        public readonly int[] TeamListCnt_Buffer = new int[10];
        private readonly int[] TeamListCnt_Last = new int[10];
        private bool ANIMATION_Smooth;
        public long Celo_Height;
        public long Celo_Left;
        private int Celo_PopupHit; // R4.00 Toggle POPUP menu on/off.  
        private object CELO_PopUpObject;
        public long Celo_Top;
        public long Celo_Width;

        // R3.40 Public Variables
        public string Celo_Windowstate; // R4.00 Aded CELO for window position storage.
        public int COLOR_Back_Dir; // R3.00 Future gradient on background. 
        public Color COLOR_Back1; // R3.00 Future gradient on background. 
        public Color COLOR_Back2; // R3.00 Future gradient on background. 
        private int CountryCount;
        private bool FLAG_CheckingLog;
        private bool FLAG_Drawing; // R3.40 Dont let combo boxes call when we are drawing.
        public int FLAG_EloMode; // R4.30 Which value are we showing right now. Cycles with Scans.
        public bool FLAG_EloUse; // R4.30 Try to draw the ELO values on screen?
        private bool FLAG_EloValid; // R4.30 Are the current ELO values valid?
        private bool FLAG_HideMissing; // R4.30 Added to hide blanks on Overlays/Green Screens.
        public bool FLAG_InitialScanning; // R4.30 Added a flag to clear player data. 
        public bool FLAG_Loading; // R2.00 Flag that we are loading, so do not update.
        private int FLAG_ShowPlayerCard; // R4.30 Toggle STATS and PLAYERCARD display.
        private int FLAG_ShowPlayerCardNum; // R4.30 Toggle STATS and PLAYERCARD display.
        private bool FLAG_SpeechOK; // R4.34 Is this PC speech capable?
        private bool GUI_Active; // R3.10 If TRUE, gui will redraw when mouse over events happen. May be too slow.
        public int GUI_ColorMode; // R2.01 Color scheme number. 0-Lite,1-Dark
        private float LAB_Height; // R2.00 Height of labels.
        public clsGlobal.t_LabelSetup LSName;

        // R4.00 Need these global so we can set them in frmLabelSetup objects. Could be properties.
        public clsGlobal.t_LabelSetup LSRank;
        private Bitmap Main_BM; // R4.32 Used in GFX_Draw subs.
        private Bitmap Main_BM2; // R4.32 Used in GFX Effects subs.  
        private Graphics Main_Gfx;
        private Graphics Main_Gfx2;
        private Bitmap MainBlur_BM; // R4.50 Used for BLUR masking.  
        private BitmapData MainBlur_Data;
        private Graphics MainBlur_Gfx;
        private bool MainBlur_Valid;
        private Bitmap MainBuffer_BM; // R4.50 Store the premade STATS background.  
        private Graphics MainBuffer_Gfx;
        public bool MainBuffer_Valid;
        private Bitmap MainBuffer1_BM; // R4.50 Store the premade STATS GFX with RANK.  
        private Graphics MainBuffer1_Gfx;
        private bool MainBuffer1_Valid;
        private Bitmap MainBuffer2_BM; // R4.50 Store the premade STATS GFX with LEVEL.  
        private Graphics MainBuffer2_Gfx;
        private bool MainBuffer2_Valid;
        private Bitmap MainBuffer3_BM; // R4.50 Store the premade STATS GFX with %.  
        private Graphics MainBuffer3_Gfx;
        private bool MainBuffer3_Valid;
        public Bitmap NAME_bmp;
        private Bitmap NAME_OVLBmp;
        public int NOTE_Spacing;
        private Bitmap Note01_Bmp;
        private Graphics Note01_Gfx;

        // Public NoteAnim01 As t_NoteAnimation            'R4.00 Added.
        private string[]
            Note01_Text = new string[21]; // R4.00 These are used during the animation and are modified in code.

        private Bitmap Note02_Bmp;
        private Graphics Note02_Gfx;

        private string[]
            Note02_Text = new string[21]; // R4.00 These are used during the animation and are modified in code.

        private Bitmap Note03_Bmp;
        private Graphics Note03_Gfx;

        private string[]
            Note03_Text = new string[21]; // R4.00 These are used during the animation and are modified in code.

        private Bitmap Note04_Bmp;
        private Graphics Note04_Gfx;

        private string[]
            Note04_Text = new string[21]; // R4.00 These are used during the animation and are modified in code.

        public clsGlobal.t_NoteAnimation NoteAnim01 = new();
        public string[] NoteAnim01_Text = new string[11]; // R4.00 Added.
        public clsGlobal.t_NoteAnimation NoteAnim02 = new();
        public string[] NoteAnim02_Text = new string[11]; // R4.00 Added.
        public clsGlobal.t_NoteAnimation NoteAnim03 = new();
        public string[] NoteAnim03_Text = new string[11]; // R4.00 Added.
        public clsGlobal.t_NoteAnimation NoteAnim04 = new();
        public string[] NoteAnim04_Text = new string[11]; // R4.00 Added.
        public string PATH_Game = "";
        private string PATH_GamePath = ""; // R2.00 Raw path for dialogs.
        private string PATH_SaveStatsImage = "";
        private string PATH_SetupPath = ""; // R4.20 Raw path for dialogs.
        private string PATH_SoundFiles = ""; // R4.00 Added sound playing.
        public bool SCAN_Enabled;
        private long SCAN_SecCnt; // R4.30 Counter for how many secs before Auto Scan.
        public long SCAN_Time; // R4.30 Added variable scan delays.
        public int STATS_SizeX = 900;
        public int STATS_SizeY = 180;

        private StreamReader WBreader;

        // R4.41 Made these PUBLIC.
        private HttpWebRequest WBrequest;
        private HttpWebResponse WBresponse;
        private readonly Settings _settings;
        private readonly LogScanner _logScanner;

        public frmMain()
        {
            InitializeComponent();
            _cmCheckLog.Name = "cmCheckLog";
            _cmFindLog.Name = "cmFindLog";
            _cmScanLog.Name = "cmScanLog";
            _cmCopy.Name = "cmCopy";
            _cmAbout.Name = "cmAbout";
            _cboLayoutY.Name = "cboLayoutY";
            _cmELO.Name = "cmELO";
            _cmSetupSave.Name = "cmSetupSave";
            _cmSetupLoad.Name = "cmSetupLoad";
            _cmNote_PlayAll.Name = "cmNote_PlayAll";
            _cmNote04_Play.Name = "cmNote04_Play";
            _cmNote03_Play.Name = "cmNote03_Play";
            _cmNote02_Play.Name = "cmNote02_Play";
            _cmNote01_Play.Name = "cmNote01_Play";
            _cmNote4.Name = "cmNote4";
            _cmNote04Setup.Name = "cmNote04Setup";
            _cmNote3.Name = "cmNote3";
            _cmNote2.Name = "cmNote2";
            _cmNote1.Name = "cmNote1";
            _cmNote03Setup.Name = "cmNote03Setup";
            _cmNote02Setup.Name = "cmNote02Setup";
            _cmNote01Setup.Name = "cmNote01Setup";
            _cmNameSetup.Name = "cmNameSetup";
            _cmRankSetup.Name = "cmRankSetup";
            _cmSave.Name = "cmSave";
            _cmStatsModeHelp.Name = "cmStatsModeHelp";
            _cmDefaults.Name = "cmDefaults";
            _tbYSize.Name = "tbYSize";
            _tbXsize.Name = "tbXsize";
            _tbYoff.Name = "tbYoff";
            _tbXoff.Name = "tbXoff";
            _cboNoteSpace.Name = "cboNoteSpace";
            _chkGUIMode.Name = "chkGUIMode";
            _cmSizeRefresh.Name = "cmSizeRefresh";
            _cboLayoutX.Name = "cboLayoutX";
            _pbStats.Name = "pbStats";
            _cmTestData.Name = "cmTestData";
            _cmLastMatch.Name = "cmLastMatch";
            _chkFX.Name = "chkFX";
            _cmFXModeHelp.Name = "cmFXModeHelp";
            _cboFXVar1.Name = "cboFXVar1";
            _cboFxVar3.Name = "cboFxVar3";
            _cboFxVar4.Name = "cboFxVar4";
            _cmFX3DC.Name = "cmFX3DC";
            _cboFXVar2.Name = "cboFXVar2";
            _chkTips.Name = "chkTips";
            _cmAudioStop.Name = "cmAudioStop";
            _scrVolume.Name = "scrVolume";
            _cmSound15.Name = "cmSound15";
            _cmSound14.Name = "cmSound14";
            _cmSound13.Name = "cmSound13";
            _cmSound12.Name = "cmSound12";
            _cmSound11.Name = "cmSound11";
            _cmSound10.Name = "cmSound10";
            _cmSound09.Name = "cmSound09";
            _cmSound08.Name = "cmSound08";
            _cmSound07.Name = "cmSound07";
            _cmSound06.Name = "cmSound06";
            _cmSound05.Name = "cmSound05";
            _cmSound04.Name = "cmSound04";
            _cmSound03.Name = "cmSound03";
            _cmSound02.Name = "cmSound02";
            _cmSound01.Name = "cmSound01";
            _tsmPlayer_Relic.Name = "tsmPlayer_Relic";
            _tsmPlayer_OrgAT.Name = "tsmPlayer_OrgAT";
            _tsmPlayer_OrgFaction.Name = "tsmPlayer_OrgFaction";
            _tsmPlayer_OrgPlayercard.Name = "tsmPlayer_OrgPlayercard";
            _tsmPlayer_Google.Name = "tsmPlayer_Google";
            _tsmPlayer_Steam.Name = "tsmPlayer_Steam";
            _chkPopUp.Name = "chkPopUp";
            _tsmSetVolTo100.Name = "tsmSetVolTo100";
            _tsmSetVolTo90.Name = "tsmSetVolTo90";
            _tsmSetVolTo80.Name = "tsmSetVolTo80";
            _tsmSetVolTo70.Name = "tsmSetVolTo70";
            _tsmSetVolTo60.Name = "tsmSetVolTo60";
            _tsmSetVolTo50.Name = "tsmSetVolTo50";
            _tsmSetVolTo40.Name = "tsmSetVolTo40";
            _tsmSetVolTo30.Name = "tsmSetVolTo30";
            _tsmSetVolTo20.Name = "tsmSetVolTo20";
            _tsmSetVolTo10.Name = "tsmSetVolTo10";
            _tsmSelectFile.Name = "tsmSelectFile";
            _chkSmoothAni.Name = "chkSmoothAni";
            _chkFoundSound.Name = "chkFoundSound";
            _cboDelay.Name = "cboDelay";
            _chkHideMissing.Name = "chkHideMissing";
            _chkShowELO.Name = "chkShowELO";
            _scrStats.Name = "scrStats";
            _chkSpeech.Name = "chkSpeech";
            _chkGetTeams.Name = "chkGetTeams";
            _cmErrLog.Name = "cmErrLog";
            _chkCountry.Name = "chkCountry";
            _settings = new Settings(this);
            _logScanner = new LogScanner(this);
        }

        public Settings Settings
        {
            get { return _settings; }
        }

        public LogScanner LogScanner
        {
            get { return _logScanner; }
        }

        public Bitmap NameOvlBmp
        {
            set { NAME_OVLBmp = value; }
            get { return NAME_OVLBmp; }
        }

        public PictureBox PbNote1
        {
            set { pbNote1 = value; }
            get { return pbNote1; }
        }

        public PictureBox PbNote2
        {
            set { pbNote2 = value; }
            get { return pbNote2; }
        }

        public PictureBox PbNote3
        {
            set { pbNote3 = value; }
            get { return pbNote3; }
        }

        public PictureBox PbNote4
        {
            set { pbNote4 = value; }
            get { return pbNote4; }
        }

        public string PathSoundFiles
        {
            set { PATH_SoundFiles = value; }
            get { return PATH_SoundFiles; }
        }

        private void cmCheckLog_Click(object sender, EventArgs e)
        {
            // R4.41 Added.
            lbStatus.BackColor = Color.FromArgb(255, 192, 255, 192);
            lbStatus.Refresh();
            CONTROLS_Enabled(false);
            cmScanLog.Enabled = false;

            // R4.50 Force the STATS image redraw.
            MainBuffer_Valid = false;

            // R4.30 Removed extra GFX draw here.
            LogScanner.LOG_Scan();

            // R4.41 Added.
            lbStatus.BackColor = Color.FromArgb(255, 192, 192, 192);
            lbStatus.Refresh();
            CONTROLS_Enabled(true);
            cmScanLog.Enabled = true;
        }

        private void LOG_InitCalcArrays()
        {
            LVLpercs[20] = 0.00013f;
            LVLpercs[19] = 0.00038f;
            LVLpercs[18] = 0.00176f;
            LVLpercs[17] = 0.00466f;
            LVLpercs[16] = 0.01019f;
            LVLpercs[15] = 0.0253f;
            LVLpercs[14] = 0.05021f;
            LVLpercs[13] = 0.10018f;
            LVLpercs[12] = 0.15014f;
            LVLpercs[11] = 0.20023f;
            LVLpercs[10] = 0.25019f;
            LVLpercs[9] = 0.31022f;
            LVLpercs[8] = 0.38019f;
            LVLpercs[7] = 0.45016f;
            LVLpercs[6] = 0.55021f;
            LVLpercs[5] = 0.65014f;
            LVLpercs[4] = 0.75019f;
            LVLpercs[3] = 0.80015f;
            LVLpercs[2] = 0.86018f;
            LVLpercs[1] = 0.94022f;
        }

        public void STATS_StoreLast()
        {
            int t;
            var Hit = default(int);

            // R4.30 See if we have new valid data.
            for (t = 1; t <= 8; t++)
                if (0 < PlrRID[t])
                {
                    Hit = t;
                    break;
                }

            // R3.000 We have new valid incoming data, so clear old data.
            if (0 < Hit)
                for (t = 1; t <= 8; t++)
                {
                    PlrRank_Last[t] = PlrRank_Buffer[t];
                    PlrName_Last[t] = PlrName_Buffer[t];
                    PlrSteam_Last[t] = PlrSteam_Buffer[t];
                    PlrRID_Last[t] = PlrRID_Buffer[t];
                    PlrFact_Last[t] = PlrFact_Buffer[t];
                    PlrTeam_Last[t] = PlrTeam_Buffer[t];
                    PlrTWin_Last[t] = PlrTWin_Buffer[t];
                    PlrTLoss_Last[t] = PlrTLoss_Buffer[t];
                    PlrCountry_Last[t] = PlrCountry_Buffer[t];
                    PlrCountryName_Last[t] = PlrCountryName_Buffer[t];
                    for (var T2 = 1; T2 <= 5; T2++)
                    for (var T3 = 1; T3 <= 4; T3++)
                    {
                        PlrRankALL_Last[t, T2, T3] = PlrRankALL_Buffer[t, T2, T3];
                        PlrRankWin_Last[t, T2, T3] = PlrRankWin_Buffer[t, T2, T3];
                        PlrRankLoss_Last[t, T2, T3] = PlrRankLoss_Buffer[t, T2, T3];
                        PlrRankPerc_Last[t, T2, T3] = PlrRankPerc_Buffer[t, T2, T3];
                    }

                    TeamListCnt_Last[t] = TeamListCnt_Buffer[t];
                    for (int T2 = 1, loopTo = TeamList_Last.GetUpperBound(1); T2 <= loopTo; T2++)
                        TeamList_Last[t, T2] = TeamList_Buffer[t, T2];

                    PlrELO_Last[t] = PlrELO_Buffer[t];
                    PlrLVL_Last[t] = PlrLVL_Buffer[t];
                    PlrGLVL_Last[t] = PlrGLVL_Buffer[t];
                }
        }

        public void STAT_GetCheckForTeam()
        {
            // R4.34 Added.
            long TempRank;
            long TempWin;
            long TempLoss;
            var TempMax = default(long);
            int Cnt;
            var MCnt = new int[10];
            var MTeam = new int[10];

            // R4.41 Added TRY CATCH.
            try
            {
                // *********************************************************
                // R4.34 Loop thru TEAM 1 looking for possible teams.
                // *********************************************************
                for (var t = 1; t <= 8; t += 2)
                for (int t2 = 1, loopTo = TeamListCnt[t]; t2 <= loopTo; t2++)
                {
                    Cnt = 0;
                    if (0 < TeamList[t, t2].RID1)
                    {
                        if (PlrRID[1] == TeamList[t, t2].RID1) Cnt += 1;

                        if (PlrRID[3] == TeamList[t, t2].RID1) Cnt += 1;

                        if (PlrRID[5] == TeamList[t, t2].RID1) Cnt += 1;

                        if (PlrRID[7] == TeamList[t, t2].RID1) Cnt += 1;
                    }

                    if (0 < TeamList[t, t2].RID2)
                    {
                        if (PlrRID[1] == TeamList[t, t2].RID2) Cnt += 1;

                        if (PlrRID[3] == TeamList[t, t2].RID2) Cnt += 1;

                        if (PlrRID[5] == TeamList[t, t2].RID2) Cnt += 1;

                        if (PlrRID[7] == TeamList[t, t2].RID2) Cnt += 1;
                    }

                    if (0 < TeamList[t, t2].RID3)
                    {
                        if (PlrRID[1] == TeamList[t, t2].RID3) Cnt += 1;

                        if (PlrRID[3] == TeamList[t, t2].RID3) Cnt += 1;

                        if (PlrRID[5] == TeamList[t, t2].RID3) Cnt += 1;

                        if (PlrRID[7] == TeamList[t, t2].RID3) Cnt += 1;
                    }

                    if (0 < TeamList[t, t2].RID4)
                    {
                        if (PlrRID[1] == TeamList[t, t2].RID4) Cnt += 1;

                        if (PlrRID[3] == TeamList[t, t2].RID4) Cnt += 1;

                        if (PlrRID[5] == TeamList[t, t2].RID4) Cnt += 1;

                        if (PlrRID[7] == TeamList[t, t2].RID4) Cnt += 1;
                    }

                    if (1 < Cnt)
                        // we found a team.
                        if ((MCnt[t] <= Cnt) & (TeamList[t, t2].PlrCnt <= Cnt))
                        {
                            MCnt[t] = Cnt;
                            MTeam[t] = t2;
                        }
                }

                // R4.34 Decide if the team is Axis(20,22,24) or Allies(21,23,25) by faction.
                for (var t = 1; t <= 8; t += 2)
                    if (0 < MTeam[t])
                    {
                        if ((PlrFact[t] == "01") | (PlrFact[t] == "03")) // R4.34 OST or OKW.
                        {
                            // R4.35 Added TEAM Win/Loss.
                            TempRank = TeamList[t, MTeam[t]].RankAxis;
                            TempWin = TeamList[t, MTeam[t]].WinAxis;
                            TempLoss = TeamList[t, MTeam[t]].LossAxis;
                            switch (TeamList[t, MTeam[t]].PlrCnt)
                            {
                                case 2:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[7, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[7, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[7, 4])); // R4.41 Bug fix.
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // R4.35 Added TEAM Win/Loss.
                            TempRank = TeamList[t, MTeam[t]].RankAllies;
                            TempWin = TeamList[t, MTeam[t]].WinAllies;
                            TempLoss = TeamList[t, MTeam[t]].LossAllies;
                            switch (TeamList[t, MTeam[t]].PlrCnt)
                            {
                                case 2:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[6, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[6, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[6, 4])); // R4.41 Bug fix.
                                    break;
                                }
                            }
                        }

                        PlrTeam[t] = MTeam[t];
                        PlrTWin[t] = (int) TempWin; // R4.35 Added TEAM Win/Loss.
                        PlrTLoss[t] = (int) TempLoss;
                        PlrRank[t] = TempRank + ".";
                        if ((TempMax < 1L) | (TempRank < 1L))
                        {
                            if (PlrRank[t] == "-1.") PlrRank[t] = "P."; // R4.46 Added.

                            PlrELO[t] = "---";
                            PlrLVL[t] = "---";
                        }
                        else
                        {
                            PlrELO[t] = (100d * (Conversion.Val(PlrRank[t]) / TempMax)).ToString("##.0") + "%";
                            PlrLVL[t] = "L-" + LOG_CalcLevel((int) Math.Round(Conversion.Val(PlrRank[t])),
                                (int) TempMax);
                        }
                    }

                // *********************************************************
                // R4.34 Loop thru TEAM 2 looking for possible teams.
                // *********************************************************
                for (var t = 2; t <= 8; t += 2)
                for (int t2 = 1, loopTo1 = TeamListCnt[t]; t2 <= loopTo1; t2++)
                {
                    Cnt = 0;
                    if (0 < TeamList[t, t2].RID1)
                    {
                        if (PlrRID[2] == TeamList[t, t2].RID1) Cnt += 1;

                        if (PlrRID[4] == TeamList[t, t2].RID1) Cnt += 1;

                        if (PlrRID[6] == TeamList[t, t2].RID1) Cnt += 1;

                        if (PlrRID[8] == TeamList[t, t2].RID1) Cnt += 1;
                    }

                    if (0 < TeamList[t, t2].RID2)
                    {
                        if (PlrRID[2] == TeamList[t, t2].RID2) Cnt += 1;

                        if (PlrRID[4] == TeamList[t, t2].RID2) Cnt += 1;

                        if (PlrRID[6] == TeamList[t, t2].RID2) Cnt += 1;

                        if (PlrRID[8] == TeamList[t, t2].RID2) Cnt += 1;
                    }

                    if (0 < TeamList[t, t2].RID3)
                    {
                        if (PlrRID[2] == TeamList[t, t2].RID3) Cnt += 1;

                        if (PlrRID[4] == TeamList[t, t2].RID3) Cnt += 1;

                        if (PlrRID[6] == TeamList[t, t2].RID3) Cnt += 1;

                        if (PlrRID[8] == TeamList[t, t2].RID3) Cnt += 1;
                    }

                    if (0 < TeamList[t, t2].RID4)
                    {
                        if (PlrRID[2] == TeamList[t, t2].RID4) Cnt += 1;

                        if (PlrRID[4] == TeamList[t, t2].RID4) Cnt += 1;

                        if (PlrRID[6] == TeamList[t, t2].RID4) Cnt += 1;

                        if (PlrRID[8] == TeamList[t, t2].RID4) Cnt += 1;
                    }

                    if (1 < Cnt)
                        // R4.34 We found a team.
                        if ((MCnt[t] <= Cnt) & (TeamList[t, t2].PlrCnt <= Cnt))
                        {
                            MCnt[t] = Cnt;
                            MTeam[t] = t2;
                        }
                }

                // R4.34 Decide if the team is Axis(20,22,24) or Allies(21,23,25) by faction.
                for (var t = 2; t <= 8; t += 2)
                {
                    TempMax = 0L;
                    if (0 < MTeam[t])
                    {
                        if ((PlrFact[t] == "01") | (PlrFact[t] == "03")) // R4.34 OST or OKW.
                        {
                            TempRank = TeamList[t, MTeam[t]].RankAxis;
                            TempWin = TeamList[t, MTeam[t]].WinAxis;
                            TempLoss = TeamList[t, MTeam[t]].LossAxis;
                            switch (TeamList[t, MTeam[t]].PlrCnt)
                            {
                                case 2:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[7, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[7, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[7, 4])); // R4.41 Bug fix.
                                    break;
                                }
                            }
                        }
                        else
                        {
                            TempRank = TeamList[t, MTeam[t]].RankAllies;
                            TempWin = TeamList[t, MTeam[t]].WinAllies;
                            TempLoss = TeamList[t, MTeam[t]].LossAllies;
                            switch (TeamList[t, MTeam[t]].PlrCnt)
                            {
                                case 2:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[6, 2])); // R4.41 Bug fix.
                                    break;
                                }

                                case 3:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[6, 3])); // R4.41 Bug fix.
                                    break;
                                }

                                case 4:
                                {
                                    TempMax = (long) Math.Round(Conversion.Val(LVLS[6, 4])); // R4.41 Bug fix.
                                    break;
                                }
                            }
                        }

                        PlrTeam[t] = MTeam[t];
                        PlrTWin[t] = (int) TempWin; // R4.35 Added TEAM Win/Loss.
                        PlrTLoss[t] = (int) TempLoss;
                        PlrRank[t] = TempRank + ".";
                        if ((TempMax < 1L) | (TempRank < 1L))
                        {
                            if (PlrRank[t] == "-1.") PlrRank[t] = "P."; // R4.46 Added.

                            PlrELO[t] = "---";
                            PlrLVL[t] = "---";
                        }
                        else
                        {
                            PlrELO[t] = (100d * (Conversion.Val(PlrRank[t]) / TempMax)).ToString("##.0") + "%";
                            PlrLVL[t] = "L-" + LOG_CalcLevel((int) Math.Round(Conversion.Val(PlrRank[t])),
                                (int) TempMax);
                        }
                    }
                }
            }
            catch (Exception)
            {
                lbError2.Text = "Team Check Err";
                lbError2.BackColor = Color.FromArgb(255, 255, 0, 0);
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Team Check - ERR:" +
                                 Information.Err().Description);
            }
        }

        public void STATS_ReadAloud()
        {
            // R4.34 Added.
            var A = "";
            var GoodGuys = "Good Guys";
            var BadGuys = "Bad Guys";
            // Dim tts As New SpeechSynthesizer
            var TP = new string[10];
            if (FLAG_SpeechOK == false)
            {
                lbError2.Text = "Speech Error";
                return;
            }

            // R4.34 Remove some chars for clearer speech.
            for (var t = 1; t <= 8; t++)
            {
                A = PlrName[t];
                A = A.Replace(".", "");
                A = A.Replace(",", "");
                A = A.Replace("|", "");
                TP[t] = A;
            }

            if ((PlrFact[1] == "01") | (PlrFact[1] == "03"))
            {
                GoodGuys = "Axis";
                BadGuys = "Allies";
            }

            if ((PlrFact[1] == "02") | (PlrFact[1] == "04") | (PlrFact[1] == "05"))
            {
                GoodGuys = "Allies";
                BadGuys = "Axis";
            }

            A = A + "The " + GoodGuys + " players are ";
            for (var t = 1; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(PlrName[t])) // R4.34 User may be "..." which will be "".
                {
                    A = A + "Player " + TP[t] + ",";
                    A = PlrRank[t] == "---"
                        ? A + "Faction Rank is None" + ","
                        : A + "Faction Rank is " + PlrRank[t] + ",";
                }

            A = A + "The " + BadGuys + " players are ";
            for (var t = 2; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(PlrName[t])) // R4.34 User may be "..." which will be "".
                {
                    A = A + "Player " + TP[t] + ",";
                    A = PlrRank[t] == "---"
                        ? A + "Faction Rank is None" + ","
                        : A + "Faction Rank is " + PlrRank[t] + ",";
                }

            A = A + ",So we have " + GoodGuys + " ";
            for (var t = 1; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(PlrName[t]))
                {
                    switch (PlrFact[t] ?? "")
                    {
                        case "01":
                        {
                            A += "O S T,";
                            break;
                        }

                        case "02":
                        {
                            A += "SOVIET,";
                            break;
                        }

                        case "03":
                        {
                            A += "O K W,";
                            break;
                        }

                        case "04":
                        {
                            A += "U S F,";
                            break;
                        }

                        case "05":
                        {
                            A += "BRIT,";
                            break;
                        }
                    }

                    A = PlrRank[t] == "---" ? A + "No rank" + "," : A + "Rank " + PlrRank[t] + ",";
                }

            A = A + ", versus " + BadGuys + " ";
            for (var t = 2; t <= 8; t += 2)
                if (!string.IsNullOrEmpty(PlrName[t]))
                    if (!string.IsNullOrEmpty(PlrName[t]))
                    {
                        switch (PlrFact[t] ?? "")
                        {
                            case "01":
                            {
                                A += "O S T,";
                                break;
                            }

                            case "02":
                            {
                                A += "SOVIET,";
                                break;
                            }

                            case "03":
                            {
                                A += "O K W,";
                                break;
                            }

                            case "04":
                            {
                                A += "U S F,";
                                break;
                            }

                            case "05":
                            {
                                A += "BRIT,";
                                break;
                            }
                        }

                        A = PlrRank[t] == "---" ? A + "No rank" + "," : A + "Rank " + PlrRank[t] + ",";
                    }

            if (!string.IsNullOrEmpty(A)) SpeechSynth.SpeakAsync(A);
        }

        public long LOG_HexToLong(string A)
        {
            long L;

            // R3.20 Convert a Hex String to a Long. If ERROR, set to ZERO.
            try
            {
                L = Convert.ToInt64(A, 16);
            }
            catch (Exception)
            {
                L = 0L;
            }

            return L;
        }

        public int LOG_CalcLevel(int PlrRank, int tMax)
        {
            int TL;

            // R4.30 loop thru the possible levels.
            TL = 1;
            for (var t = 1; t <= 15; t++)
                if (LVLpercs[t] * tMax > PlrRank)
                    TL = t + 1;

            // R4.30 Levels above rank 200 are always the same.
            if (200 > PlrRank) TL = 16;

            if (81 > PlrRank) TL = 17;

            if (37 > PlrRank) TL = 18;

            if (14 > PlrRank) TL = 19;

            if (3 > PlrRank) TL = 20;

            if (PlrRank == 0) TL = 0;

            return TL;
        }

        public long LOG_Find_RelicID(string A)
        {
            var RID = 0L;
            string C;
            int T;
            var Cnt = default(int);
            var CharStart = default(int);
            var CharEnd = default(int);

            // R3.20 Get the RelicID number from the Player stats line.
            for (T = Strings.Len(A); T >= 1; T -= 1)
            {
                C = Strings.Mid(A, T, 1);
                if (C == " ")
                {
                    Cnt += 1;
                    if (Cnt == 2) CharEnd = T;

                    if (Cnt == 3)
                    {
                        CharStart = T;
                        break;
                    }
                }
            }

            if (Conversions.ToBoolean(CharEnd))
                RID = (long) Math.Round(Conversion.Val(Strings.Mid(A, CharStart, CharEnd - CharStart)));

            return RID;
        }

        public string LOG_FindPlayer(string A, int CharStart)
        {
            string C;
            int T;
            var Cnt = default(int);
            var CharEnd = default(int);
            for (T = Strings.Len(A); T >= 1; T -= 1)
            {
                C = Strings.Mid(A, T, 1);
                if (C == " ") Cnt += 1;

                if (Cnt == 3)
                {
                    CharEnd = T;
                    break;
                }
            }

            C = "None";
            if (Conversions.ToBoolean(CharEnd)) C = Strings.Mid(A, CharStart, CharEnd - CharStart);

            return C;
        }

        public void LS_SetShadowXY(ref clsGlobal.t_LabelSetup LS)
        {
            // *****************************************************
            // R4.00 Get a SOUND file path from defined sounds.
            switch (LS.ShadowDir ?? "")
            {
                case "45°":
                {
                    LS.ShadowX = 1;
                    LS.ShadowY = -1;
                    break;
                }

                case "90°":
                {
                    LS.ShadowX = 0;
                    LS.ShadowY = -1;
                    break;
                }

                case "135°":
                {
                    LS.ShadowX = -1;
                    LS.ShadowY = -1;
                    break;
                }

                case "180°":
                {
                    LS.ShadowX = -1;
                    LS.ShadowY = 0;
                    break;
                }

                case "225°":
                {
                    LS.ShadowX = -1;
                    LS.ShadowY = 1;
                    break;
                }

                case "270°":
                {
                    LS.ShadowX = 0;
                    LS.ShadowY = 1;
                    break;
                }

                case "315°":
                {
                    LS.ShadowX = 1;
                    LS.ShadowY = 1;
                    break;
                }

                case "360°":
                {
                    LS.ShadowX = 1;
                    LS.ShadowY = 0;
                    break;
                }

                default:
                {
                    LS.ShadowX = 0;
                    LS.ShadowY = 0;
                    break;
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            FLAG_Loading = true; // R2.00 Tell Controls not to update. Most save settings as they update,
            GUI_Active = false; // R3.10 Default to NO active GUI elements.
            PATH_Game = ""; // R2.00 Initialize the var or we get error 448 in file.
            PATH_BackgroundImage = "";

            // R4.43 Added for Connection issues.
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // R4.46 Added Country Strings. Read from included Resource FIle.
            RES_GetCountryData();

            // R4.30 Create a BMP to work from. 
            Main_BM = new Bitmap(pbStats.Width, pbStats.Height);
            Main_Gfx = Graphics.FromImage(Main_BM);
            Main_Gfx.Clear(Color.Black);
            Main_BM2 = new Bitmap(pbStats.Width, pbStats.Height);
            Main_Gfx2 = Graphics.FromImage(Main_BM2);
            Main_Gfx2.Clear(Color.Black);
            Main_Gfx.SmoothingMode = SmoothingMode.AntiAlias;

            // R3.00 Setup default Ranks and Names
            for (var t = 1; t <= 8; t++)
            {
                PlrName[t] = "Player " + t;
                PlrRank[t] = "123";
                PlrFact[t] = "01";
                LAB_Name_Align[t] = new StringFormat();
            }

            // R4.00 Init some string vars.
            for (var t = 1; t <= 10; t++)
            {
                NoteAnim01_Text[t] = "";
                NoteAnim02_Text[t] = "";
                NoteAnim03_Text[t] = "";
                NoteAnim04_Text[t] = "";
                Note01_Text[t] = "";
                Note02_Text[t] = "";
                Note03_Text[t] = "";
                Note04_Text[t] = "";
            }

            // R4.30 Fill in the RELIC LEADERBOARD IDs for each game mode.
            INIT_LeaderBoardIDs();

            // R3.30 Fill in the Combo Box list items.
            INIT_FillComboBoxes();

            // R3.40 Setup ToolTips
            ToolTip1.Active = false;
            ToolTip_Setup();

            // R4.30 Create array with LEVEL step percentages.
            LOG_InitCalcArrays();

            // R4.00 Setup default FONTs for Rank and Name labels.
            FONT_Rank_Name = "Arial";
            FONT_Rank_Size = "14";
            FONT_Rank_Bold = "True";
            FONT_Rank_Italic = "False";
            FONT_Rank = new Font(FONT_Rank_Name, Conversions.ToSingle(FONT_Rank_Size), FontStyle.Bold);
            FONT_Name_Name = "Arial";
            FONT_Name_Size = "16";
            FONT_Name_Bold = "True";
            FONT_Name_Italic = "False";
            FONT_Name = new Font(FONT_Name_Name, Conversions.ToSingle(FONT_Name_Size), FontStyle.Bold);
            FONT_Note01 = FONT_Rank;
            FONT_Note01_Name = "Arial";
            FONT_Note01_Size = "14";
            FONT_Note01_Bold = "True";
            FONT_Note01_Italic = "False";
            FONT_Note02 = FONT_Rank;
            FONT_Note02_Name = "Arial";
            FONT_Note02_Size = "14";
            FONT_Note02_Bold = "True";
            FONT_Note02_Italic = "False";
            FONT_Note03 = FONT_Rank;
            FONT_Note03_Name = "Arial";
            FONT_Note03_Size = "14";
            FONT_Note03_Bold = "True";
            FONT_Note03_Italic = "False";
            FONT_Note04 = FONT_Rank;
            FONT_Note04_Name = "Arial";
            FONT_Note04_Size = "14";
            FONT_Note04_Bold = "True";
            FONT_Note04_Italic = "False";

            // R4.00 Message seperation string (Delimiter).
            NoteAnim01.Delim = "   ●   ";
            NoteAnim02.Delim = "   ●   ";
            NoteAnim03.Delim = "   ●   ";
            NoteAnim04.Delim = "   ●   ";

            // R3.30 Default label gradient color setups.
            LSRank.F1 = Color.FromArgb(255, 255, 255, 64);
            LSRank.F2 = Color.FromArgb(255, 192, 192, 48);
            LSRank.FDir = 0;
            LSRank.B1 = Color.FromArgb(255, 128, 0, 0);
            LSRank.B2 = Color.FromArgb(255, 0, 0, 0);
            LSRank.BDir = 0;
            LSRank.ShadowColor = Color.FromArgb(255, 0, 0, 0);
            LSRank.O1 = 100;
            LSRank.O2 = 10;
            LSName.F1 = Color.FromArgb(255, 255, 255, 255);
            LSName.F2 = Color.FromArgb(255, 192, 192, 192);
            LSName.FDir = 0;
            LSName.B1 = Color.FromArgb(255, 64, 64, 64);
            LSName.B2 = Color.FromArgb(25, 0, 0, 0);
            LSName.BDir = 1;

            // R4.41 Added some color defaults. ARGB(0,0,0,0) does not let you set ARGB(255,0,0,0) in dialog.
            LSRank.ShadowColor = Color.FromArgb(255, 0, 0, 0);
            LSRank.BorderColor = Color.FromArgb(255, 0, 0, 0);
            LSRank.BorderPanelColor = Color.FromArgb(255, 64, 0, 0);
            LSName.ShadowColor = Color.FromArgb(255, 0, 0, 0);
            LSName.BorderColor = Color.FromArgb(255, 0, 0, 0);
            LSName.BorderPanelColor = Color.FromArgb(255, 64, 0, 0);
            LSName.BackC = Color.FromArgb(255, 0, 0, 0);

            // R4.00 Set some default setups for Notes.
            LSNote01 = LSRank;
            LSNote02 = LSRank;
            LSNote03 = LSRank;
            LSNote04 = LSRank;
            LSNote01.Width = pbNote1.Width;
            LSNote01.Height = pbNote1.Height;
            LSNote02.Width = pbNote2.Width;
            LSNote02.Height = pbNote2.Height;
            LSNote03.Width = pbNote3.Width;
            LSNote03.Height = pbNote3.Height;
            LSNote04.Width = pbNote4.Width;
            LSNote04.Height = pbNote4.Height;

            // R4.50 Init the SOUND PAD variables.
            for (var t = 0; t <= 19; t++)
            {
                SOUND_File[t] = "";
                SOUND_Vol[t] = 100.ToString();
            }

            // ***********************************************************************************************************
            // R3.00 Load the base USER settings. Do not load older files that Windows did not cleanup when uninstalled.
            // ***********************************************************************************************************
            var IsOldStyle = false;
            if (Settings.SETTINGS_Load_CheckVersion("", ref IsOldStyle))
            {
                if (IsOldStyle)
                    Settings.SETTINGS_Load_Old("");
                else
                    Settings.SETTINGS_Load("");
            }

            SETUP_Apply();

            // R4.30 Check the MAX player data to see if is usable.
            ELO_VerifyData();
            SPEECH_Test();

            // R4.50 Force a clean redraw once fully loaded.
            MainBuffer_Valid = false;
        }

        private void SPEECH_Test()
        {
            // Dim tts As New SpeechSynthesizer

            FLAG_SpeechOK = true;
            try
            {
                SpeechSynth.SpeakAsync("");
            }
            catch
            {
                FLAG_SpeechOK = false;
                lbError2.Text = "Speech Error";
            }
        }

        private void INIT_LeaderBoardIDs()
        {
            // OST
            RelDataLeaderID[1, 1] = "4";
            RelDataLeaderID[1, 2] = "8";
            RelDataLeaderID[1, 3] = "12";
            RelDataLeaderID[1, 4] = "16";
            // SOV
            RelDataLeaderID[2, 1] = "5";
            RelDataLeaderID[2, 2] = "9";
            RelDataLeaderID[2, 3] = "13";
            RelDataLeaderID[2, 4] = "17";
            // OKW
            RelDataLeaderID[3, 1] = "6";
            RelDataLeaderID[3, 2] = "10";
            RelDataLeaderID[3, 3] = "14";
            RelDataLeaderID[3, 4] = "18";
            // USF
            RelDataLeaderID[4, 1] = "7";
            RelDataLeaderID[4, 2] = "11";
            RelDataLeaderID[4, 3] = "15";
            RelDataLeaderID[4, 4] = "19";
            // BRIT
            RelDataLeaderID[5, 1] = "51";
            RelDataLeaderID[5, 2] = "52";
            RelDataLeaderID[5, 3] = "53";
            RelDataLeaderID[5, 4] = "54";
        }

        private void SETUP_Apply()
        {
            Note01_Bmp = new Bitmap(pbNote1.Width, pbNote1.Height);
            Note02_Bmp = new Bitmap(pbNote2.Width, pbNote2.Height);
            Note03_Bmp = new Bitmap(pbNote3.Width, pbNote3.Height);
            Note04_Bmp = new Bitmap(pbNote4.Width, pbNote4.Height);
            Note01_Gfx = Graphics.FromImage(Note01_Bmp);
            Note02_Gfx = Graphics.FromImage(Note02_Bmp);
            Note03_Gfx = Graphics.FromImage(Note03_Bmp);
            Note04_Gfx = Graphics.FromImage(Note04_Bmp);

            // R4.00 Setup default sizes.
            LSNote01.Width = pbNote1.Width;
            LSNote01.Height = pbNote1.Height;
            LSNote02.Width = pbNote2.Width;
            LSNote02.Height = pbNote2.Height;
            LSNote03.Width = pbNote3.Width;
            LSNote03.Height = pbNote3.Height;
            LSNote04.Width = pbNote4.Width;
            LSNote04.Height = pbNote4.Height;

            // R3.00 Set GUI color scheme.
            switch (GUI_ColorMode)
            {
                case 0:
                {
                    GUI_SetLite();
                    break;
                }

                case 1:
                {
                    GUI_SetDark();
                    break;
                }
            }

            // R2.00 Try to set some defaults in case we cant read settings.
            LSRank.O1 = (int) Math.Round(LSRank.B1.A / 2.55d);
            LSRank.O2 = (int) Math.Round(LSRank.B2.A / 2.55d);
            LSName.O1 = (int) Math.Round(LSName.B1.A / 2.55d);
            LSName.O2 = (int) Math.Round(LSName.B2.A / 2.55d);
            if (string.IsNullOrEmpty(cboLayoutY.Text)) cboLayoutY.Text = "3 - 90% tight";

            if (string.IsNullOrEmpty(cboLayoutX.Text)) cboLayoutX.Text = "3 - 90% tight";

            if (string.IsNullOrEmpty(LSName.Scaling)) LSName.Scaling = "2 - Fit";

            if (string.IsNullOrEmpty(LSRank.ShadowDir)) LSRank.ShadowDir = "270°";

            if (string.IsNullOrEmpty(cboFXVar2.Text)) cboFXVar2.Text = "270°";

            if (string.IsNullOrEmpty(cboFxVar3.Text)) cboFxVar3.Text = "80%";

            if (string.IsNullOrEmpty(cboFxVar4.Text)) cboFxVar4.Text = "2.0%";

            if (string.IsNullOrEmpty(cboFXVar1.Text)) cboFXVar1.Text = "None";

            FLAG_Loading = false;
            STATS_DefineY();

            // R4.00 Restore last window position if not min or max. 
            // R4.00 Need a check here for valid postition so we dont set it off screen.
            if ((Celo_Windowstate == "Normal") & chkPosition.Checked)
            {
                Location = new Point((int) Celo_Left, (int) Celo_Top);
                Width = (int) Celo_Width;
                Height = (int) Celo_Height;
            }
        }

        private void INIT_FillComboBoxes()
        {
            cboDelay.Items.Add("5");
            cboDelay.Items.Add("10");
            cboDelay.Items.Add("20");
            cboDelay.Items.Add("30");
            cboDelay.Items.Add("45");
            cboDelay.Items.Add("60");

            // R4.30 Set default search time to 10 seconds.
            cboDelay.SelectedIndex = 1;

            // R3.20 Setup some default FX values.
            CFX3DVar[1, 2] = "270°";
            CFX3DVar[1, 3] = "70%";
            CFX3DVar[1, 4] = "5.0%";
            CFX3DVar[2, 2] = "270°";
            CFX3DVar[2, 3] = "50%";
            CFX3DVar[2, 4] = "5.0%";
            CFX3DVar[3, 2] = "270°";
            CFX3DVar[3, 3] = "50%";
            CFX3DVar[3, 4] = "2.0%";
            cboLayoutY.Items.Add("0 - 60% tight");
            cboLayoutY.Items.Add("1 - 70% tight");
            cboLayoutY.Items.Add("2 - 80% tight");
            cboLayoutY.Items.Add("3 - 90% tight");
            cboLayoutY.Items.Add("4 - 100% tight");
            cboLayoutY.Items.Add("5 - 60% spread");
            cboLayoutY.Items.Add("6 - 70% spread");
            cboLayoutY.Items.Add("7 - 80% spread");
            cboLayoutY.Items.Add("8 - 90% spread");
            cboLayoutY.Items.Add("9 - 100% spread");
            cboLayoutX.Items.Add("0 - 60% tight");
            cboLayoutX.Items.Add("1 - 70% tight");
            cboLayoutX.Items.Add("2 - 80% tight");
            cboLayoutX.Items.Add("3 - 90% tight");
            cboLayoutX.Items.Add("4 - 100% tight");
            cboLayoutX.Items.Add("5 - 60% spread");
            cboLayoutX.Items.Add("6 - 70% spread");
            cboLayoutX.Items.Add("7 - 80% spread");
            cboLayoutX.Items.Add("8 - 90% spread");
            cboLayoutX.Items.Add("9 - 100% spread");
            cboLayoutX.Items.Add("10 - 60% mid tight");
            cboLayoutX.Items.Add("11 - 70% mid tight");
            cboLayoutX.Items.Add("12 - 80% mid tight");
            cboLayoutX.Items.Add("13 - 90% mid tight");
            cboLayoutX.Items.Add("14 - 100% mid tight");
            cboLayoutX.Items.Add("15 - 60% mid spread");
            cboLayoutX.Items.Add("16 - 70% mid spread");
            cboLayoutX.Items.Add("17 - 80% mid spread");
            cboLayoutX.Items.Add("18 - 90% mid spread");
            cboLayoutX.Items.Add("19 - 100% mid spread");
            cboLayoutX.Items.Add("20 - clock tight");
            cboLayoutX.Items.Add("21 - clock spread");
            cboNoteSpace.Items.Add("0 px");
            cboNoteSpace.Items.Add("1 px");
            cboNoteSpace.Items.Add("2 px");
            cboNoteSpace.Items.Add("3 px");
            cboNoteSpace.Items.Add("4 px");
            cboNoteSpace.Items.Add("5 px");
            cboFXVar1.Items.Add("None");
            cboFXVar1.Items.Add("Shadow");
            cboFXVar1.Items.Add("Emboss");
            cboFXVar1.Items.Add("Lab Blur");
            cboFXVar2.Items.Add("--");
            cboFXVar2.Items.Add("45°");
            cboFXVar2.Items.Add("90°");
            cboFXVar2.Items.Add("135°");
            cboFXVar2.Items.Add("180°");
            cboFXVar2.Items.Add("225°");
            cboFXVar2.Items.Add("270°");
            cboFXVar2.Items.Add("315°");
            cboFXVar2.Items.Add("360°");
            cboFxVar4.Items.Add("0.0%");
            cboFxVar4.Items.Add("1.0%");
            cboFxVar4.Items.Add("2.0%");
            cboFxVar4.Items.Add("3.0%");
            cboFxVar4.Items.Add("4.0%");
            cboFxVar4.Items.Add("5.0%");
            cboFxVar4.Items.Add("6.0%");
            cboFxVar4.Items.Add("7.0%");
            cboFxVar4.Items.Add("8.0%");
            cboFxVar4.Items.Add("9.0%");
            cboFxVar4.Items.Add("10.0%");
            cboFxVar3.Items.Add("40%");
            cboFxVar3.Items.Add("45%");
            cboFxVar3.Items.Add("50%");
            cboFxVar3.Items.Add("55%");
            cboFxVar3.Items.Add("60%");
            cboFxVar3.Items.Add("65%");
            cboFxVar3.Items.Add("70%");
            cboFxVar3.Items.Add("75%");
            cboFxVar3.Items.Add("80%");
            cboFxVar3.Items.Add("85%");
            cboFxVar3.Items.Add("90%");
            cboFxVar3.Items.Add("95%");
        }

        private void cmFindLog_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            string A;
            int N;

            // R2.00 Find the LOG file. Try top help get to the right location if possible.
            fd.Title = @"FIND: Warnings.Log in My Games\Company of Heroes 2";
            if (!string.IsNullOrEmpty(PATH_GamePath))
            {
                fd.InitialDirectory = PATH_GamePath;
            }
            else
            {
                A = SpecialDirectories.MyDocuments + @"\My Games\Company of Heroes 2\";
                fd.InitialDirectory = Directory.Exists(A) ? A : SpecialDirectories.MyDocuments;
            }

            fd.Filter = "Log Files (*.log)|*.log";
            fd.FilterIndex = 1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                PATH_Game = fd.FileName;
                Settings.SETTINGS_Save("");

                // R3.40 Strip off filename so we can use it for init dir later.
                N = Conversions.ToInteger(Utilities.STRING_FindLastSlash(PATH_Game));
                PATH_GamePath = 3 < N ? Strings.Mid(PATH_Game, 1, N) : "";
            }
        }

        private void cmScanLog_Click(object sender, EventArgs e)
        {
            // R2.10 If we dont have a valid log file path, exit with help notice.
            if (!File.Exists(PATH_Game))
            {
                Interaction.MsgBox(
                    "Please locate the warnings.log file in your COH2 My Games directory." + Constants.vbCr +
                    Constants.vbCr + "Click on FIND LOG FILE to search and select.", MsgBoxStyle.Information);
                return;
            }

            SCAN_Enabled = !SCAN_Enabled;
            if (SCAN_Enabled)
            {
                SCAN_SecCnt = 0L; // R4.30 Reset our wait counter.
                cmScanLog.Text = "Scanning...";
                lbStatus.BackColor = Color.FromArgb(255, 192, 255, 192);
                CONTROLS_Enabled(false);
                Timer1.Enabled = true;
            }
            else
            {
                Timer1.Enabled = false;
                lbStatus.BackColor = Color.FromArgb(255, 192, 192, 192);
                lbStatus.Text = "Ready";
                cmScanLog.Text = "Auto Scan Log";
                CONTROLS_Enabled(true); // R3.20 Turn ON all controls.
                GFX_UpdateScreenControls();
            } // R3.20 Turn off the controls for FX that should not be on now.
        }

        private void CONTROLS_Enabled(bool tState)
        {
            // R1.00 Turn OFF all Controls while scanning on timer.
            // R4.30 Added new controls to this sub.

            cmCheckLog.Enabled = tState;
            cmFindLog.Enabled = tState;
            cmTestData.Enabled = tState;
            cmRankSetup.Enabled = tState;
            cmELO.Enabled = tState; // R4.30 Added.
            cmNameSetup.Enabled = tState;
            cmSetupLoad.Enabled = tState;
            cmSetupSave.Enabled = tState;
            cmAbout.Enabled = tState;
            tbXsize.Enabled = tState;
            tbYSize.Enabled = tState;
            tbXoff.Enabled = tState;
            tbYoff.Enabled = tState;
            cboLayoutY.Enabled = tState;
            cboLayoutX.Enabled = tState;
            cboNoteSpace.Enabled = tState;
            cmDefaults.Enabled = tState;
            cmStatsModeHelp.Enabled = tState;
            cmSizeRefresh.Enabled = tState;
            cboFXVar1.Enabled = tState;
            chkFX.Enabled = tState;
            cmFX3DC.Enabled = tState;
            cboFXVar2.Enabled = tState;
            cboFxVar3.Enabled = tState;
            cboFxVar4.Enabled = tState;
            cmFXModeHelp.Enabled = tState;
        }

        // Static SecCnt As Long
        private void Timer1_Tick(object sender, EventArgs e) // R4.10 Added.
        {
            // R4.10 MSGBOX in LOG_Scan holds code there, but animation timer keeps going which triggers this timer and you get a loop.
            if (FLAG_CheckingLog) return;

            SCAN_SecCnt += 1L;
            if (SCAN_Time < SCAN_SecCnt)
            {
                SCAN_SecCnt = 0L;
                FLAG_CheckingLog = true;
                LogScanner.LOG_Scan();
                FLAG_CheckingLog = false;
            }
            else
            {
                lbStatus.Text = "Scan in " + (SCAN_Time + 1L - SCAN_SecCnt) + "s";
            }
        }

        private void cmCopy_Click(object sender, EventArgs e)
        {
            // R3.00 Copy Picturebox image data to the clipboard.
            var tmpImg = new Bitmap(pbStats.Image, pbStats.Width, pbStats.Height);
            Clipboard.Clear();
            Clipboard.SetImage(tmpImg);
            tmpImg.Dispose();
        }

        private void cmAbout_Click(object sender, EventArgs e)
        {
            var frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }

        private void STATS_DefineY()
        {
            int i;
            var YStart = default(float);
            var YStep = default(float);
            var Y = default(float);
            switch (Conversion.Val(cboLayoutY.Text))
            {
                case 0d:
                {
                    YStart = 0.2f;
                    YStep = (float) (0.6d / 4d);
                    break;
                }

                case 1d:
                {
                    YStart = 0.15f;
                    YStep = (float) (0.7d / 4d);
                    break;
                }

                case 2d:
                {
                    YStart = 0.1f;
                    YStep = (float) (0.8d / 4d);
                    break;
                }

                case 3d:
                {
                    YStart = 0.05f;
                    YStep = (float) (0.9d / 4d);
                    break;
                }

                case 4d:
                {
                    YStart = 0.025f;
                    YStep = (float) (0.94d / 4d); // R4.44 Was 0, 1
                    break;
                }

                case 5d:
                {
                    YStart = 0.2f;
                    YStep = (float) (0.6d / 4d);
                    break;
                }

                case 6d:
                {
                    YStart = 0.15f;
                    YStep = (float) (0.7d / 4d);
                    break;
                }

                case 7d:
                {
                    YStart = 0.1f;
                    YStep = (float) (0.8d / 4d);
                    break;
                }

                case 8d:
                {
                    YStart = 0.05f;
                    YStep = (float) (0.9d / 4d);
                    break;
                }

                case 9d:
                {
                    YStart = 0.025f;
                    YStep = (float) (0.94d / 4d); // R4.44 Was 0, 1
                    break;
                }
            }

            // R2.00 Force YStart and Ystep to an integer pixel value or we get spaces between labels.
            i = (int) Math.Round(YStep * pbStats.Height); // R3.30 Deprecated pnlPlr.Height
            YStep = (float) (i / (double) pbStats.Height);
            i = (int) Math.Round(YStart * pbStats.Height);
            YStart = (float) (i / (double) pbStats.Height);
            switch (Conversion.Val(cboLayoutY.Text))
            {
                case 0d:
                case 1d:
                case 2d:
                case 3d:
                case 4d:
                {
                    Y = YStart;
                    LAB_Height = YStep;
                    break;
                }

                case 5d:
                case 6d:
                case 7d:
                case 8d:
                case 9d:
                {
                    Y = (float) (YStart + YStep * 0.075d);
                    LAB_Height = (float) (YStep * 0.8d);
                    break;
                }
            }

            for (i = 1; i <= 7; i += 2)
            {
                LAB_Rank[i].Y = Y * pbStats.Height;
                LAB_Rank[i].Height = LAB_Height * pbStats.Height;
                LAB_Rank[i].Ycenter = LAB_Rank[i].Y + LAB_Rank[i].Height / 2f;
                LAB_Name[i].Y = Y * pbStats.Height;
                LAB_Name[i].Height = LAB_Height * pbStats.Height;
                LAB_Name[i].Ycenter = LAB_Name[i].Y + LAB_Name[i].Height / 2f;
                LAB_Fact[i].Y = Y * pbStats.Height;
                LAB_Fact[i].Height = LAB_Height * pbStats.Height;
                LAB_Fact[i].Ycenter = LAB_Fact[i].Y + LAB_Fact[i].Height / 2f;
                LAB_Rank[i + 1].Y = Y * pbStats.Height;
                LAB_Rank[i + 1].Height = LAB_Height * pbStats.Height;
                LAB_Rank[i + 1].Ycenter = LAB_Rank[i + 1].Y + LAB_Rank[i + 1].Height / 2f;
                LAB_Name[i + 1].Y = Y * pbStats.Height;
                LAB_Name[i + 1].Height = LAB_Height * pbStats.Height;
                LAB_Name[i + 1].Ycenter = LAB_Name[i + 1].Y + LAB_Name[i + 1].Height / 2f;
                LAB_Fact[i + 1].Y = Y * pbStats.Height;
                LAB_Fact[i + 1].Height = LAB_Height * pbStats.Height;
                LAB_Fact[i + 1].Ycenter = LAB_Fact[i + 1].Y + LAB_Fact[i + 1].Height / 2f;
                Y += YStep;
            }
        }

        private void STATS_DefineX()
        {
            // **************************************************************************************
            // R2.00 Should always calc SIZE, LayoutY, and then LayoutX so faction pics are square.
            // **************************************************************************************
            int i;
            int XC1, XCoff; // R2.00 Center of each teams labels.
            int XF, XR, XP; // R2.00 Faction, Rank, and Player name positions.  
            var XWid = default(int); // R2.00 Width of label group (left or Right). 
            int Hgt; // R2.00 Height of Faction image so we can make it square. 
            var Xoff = default(int); // R2.00 Space between labels.
            var XRankWidth = default(float); // R2.00 Width in percent of space between XR and end of name label.
            Hgt = (int) Math.Round(LAB_Fact[1].Height); // R2.00 Get width of Faction image to make it square.
            XC1 = (int) Math.Round(pbStats.Width / 4d); // R2.00 Center of left Label group.
            XCoff = (int) Math.Round(pbStats.Width / 2d); // R2.00 Offset to Center of right side group. 

            // R2.00 Get the width of the label groups and space between labels.
            switch (Conversion.Val(cboLayoutX.Text))
            {
                case 0d:
                {
                    XWid = (int) Math.Round(0.6d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.35f;
                    break;
                }

                case 1d:
                {
                    XWid = (int) Math.Round(0.7d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.3f;
                    break;
                }

                case 2d:
                {
                    XWid = (int) Math.Round(0.8d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.25f;
                    break;
                }

                case 3d:
                {
                    XWid = (int) Math.Round(0.9d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.2f;
                    break;
                }

                case 4d:
                {
                    XWid = (int) Math.Round(0.96d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.15f;
                    break;
                }

                case 5d:
                {
                    XWid = (int) Math.Round(0.6d * (pbStats.Width / 2d));
                    Xoff = 4;
                    XRankWidth = 0.35f;
                    break;
                }

                case 6d:
                {
                    XWid = (int) Math.Round(0.7d * (pbStats.Width / 2d));
                    Xoff = 6;
                    XRankWidth = 0.3f;
                    break;
                }

                case 7d:
                {
                    XWid = (int) Math.Round(0.8d * (pbStats.Width / 2d));
                    Xoff = 8;
                    XRankWidth = 0.25f;
                    break;
                }

                case 8d:
                {
                    XWid = (int) Math.Round(0.9d * (pbStats.Width / 2d));
                    Xoff = 10;
                    XRankWidth = 0.2f;
                    break;
                }

                case 9d:
                {
                    XWid = (int) Math.Round(0.96d * (pbStats.Width / 2d));
                    Xoff = 12;
                    XRankWidth = 0.15f;
                    break;
                }

                case 10d:
                {
                    XWid = (int) Math.Round(0.6d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.35f;
                    break;
                }

                case 11d:
                {
                    XWid = (int) Math.Round(0.7d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.3f;
                    break;
                }

                case 12d:
                {
                    XWid = (int) Math.Round(0.8d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.25f;
                    break;
                }

                case 13d:
                {
                    XWid = (int) Math.Round(0.9d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.2f;
                    break;
                }

                case 14d:
                {
                    XWid = (int) Math.Round(0.96d * (pbStats.Width / 2d));
                    Xoff = 0;
                    XRankWidth = 0.15f;
                    break;
                }

                case 15d:
                {
                    XWid = (int) Math.Round(0.6d * (pbStats.Width / 2d));
                    Xoff = 4;
                    XRankWidth = 0.35f;
                    break;
                }

                case 16d:
                {
                    XWid = (int) Math.Round(0.7d * (pbStats.Width / 2d));
                    Xoff = 6;
                    XRankWidth = 0.3f;
                    break;
                }

                case 17d:
                {
                    XWid = (int) Math.Round(0.8d * (pbStats.Width / 2d));
                    Xoff = 8;
                    XRankWidth = 0.25f;
                    break;
                }

                case 18d:
                {
                    XWid = (int) Math.Round(0.9d * (pbStats.Width / 2d));
                    Xoff = 10;
                    XRankWidth = 0.2f;
                    break;
                }

                case 19d:
                {
                    XWid = (int) Math.Round(0.96d * (pbStats.Width / 2d));
                    Xoff = 12;
                    XRankWidth = 0.15f;
                    break;
                }

                case 20d:
                {
                    XWid = (int) Math.Round(0.99d * ((pbStats.Width - 50) / 2d));
                    Xoff = 0;
                    XRankWidth = 0.2f;
                    break;
                }

                case 21d:
                {
                    XWid = (int) Math.Round(0.99d * ((pbStats.Width - 50) / 2d));
                    Xoff = 10;
                    XRankWidth = 0.2f;
                    break;
                }
            }

            // R2.00 Calc Xfaction, Xrank, and Xplayername.
            XF = (int) Math.Round(XC1 - XWid * 0.5d);
            XR = Xoff + XF + Hgt;
            XP = (int) Math.Round(Xoff +
                                  (XR + (XWid - XR) * XRankWidth)); // R2.00 Make RANK label some % of remaining width.
            switch (Conversion.Val(cboLayoutX.Text))
            {
                case < 10d:
                {
                    for (i = 1; i <= 7; i += 2)
                    {
                        // R2.00 Adjust Left Group X values.
                        LAB_Fact[i].X = XF;
                        LAB_Fact[i].Xtext = XF;
                        LAB_Rank[i].X = XR;
                        LAB_Rank[i].Xtext = XR;
                        LAB_Name[i].X = XP;
                        LAB_Name[i].Xtext = XP;
                        LAB_Name_Align[i].Alignment = StringAlignment.Near;
                        LAB_Fact[i].Width = Hgt;
                        LAB_Rank[i].Width = XP - XR - Xoff;
                        LAB_Name[i].Width = (float) (XC1 + XWid * 0.5d - XP - Xoff);

                        // R2.00 Adjust Right Group X values. All values are pushed right by XCoff for right side group.
                        LAB_Fact[i + 1].X = XF + XCoff;
                        LAB_Fact[i + 1].Xtext = XF + XCoff;
                        LAB_Rank[i + 1].X = XR + XCoff;
                        LAB_Rank[i + 1].Xtext = XR + XCoff;
                        LAB_Name[i + 1].X = XP + XCoff;
                        LAB_Name[i + 1].Xtext = XP + XCoff;
                        LAB_Name_Align[i + 1].Alignment = StringAlignment.Near;
                        LAB_Fact[i + 1].Width = Hgt;
                        LAB_Rank[i + 1].Width = XP - XR - Xoff;
                        LAB_Name[i + 1].Width = (float) (XC1 + XWid * 0.5d - XP - Xoff);
                    }

                    break;
                }

                case <= 10d and <= 19d: // R4.46 Fixed Bug. Was 11 to 19.
                {
                    for (i = 1; i <= 7; i += 2)
                    {
                        // R2.00 Adjust Left Group X values.
                        LAB_Fact[i].Width = Hgt;
                        LAB_Rank[i].Width = XP - XR - Xoff;
                        LAB_Name[i].Width = (float) (XC1 + XWid * 0.5d - XP - Xoff);
                        LAB_Fact[i].X = XCoff - XF - LAB_Fact[i].Width - 7f;
                        LAB_Fact[i].Xtext = XCoff - XF - LAB_Fact[i].Width - 7f;
                        LAB_Rank[i].X = XCoff - XR - LAB_Rank[i].Width - 7f;
                        LAB_Rank[i].Xtext = XCoff - XR - LAB_Rank[i].Width - 7f;
                        LAB_Name[i].X = XCoff - XP - LAB_Name[i].Width - 7f;
                        LAB_Name[i].Xtext = XCoff - XP - LAB_Name[i].Width - 7f;
                        LAB_Name_Align[i].Alignment = StringAlignment.Far;

                        // R2.00 Adjust Right Group X values. All values are pushed right by XCoff for right side group.
                        LAB_Fact[i + 1].X = XF + XCoff;
                        LAB_Fact[i + 1].Xtext = XF + XCoff;
                        LAB_Rank[i + 1].X = XR + XCoff;
                        LAB_Rank[i + 1].Xtext = XR + XCoff;
                        LAB_Name[i + 1].X = XP + XCoff;
                        LAB_Name[i + 1].Xtext = XP + XCoff;
                        LAB_Fact[i + 1].Width = Hgt;
                        LAB_Rank[i + 1].Width = XP - XR - Xoff;
                        LAB_Name[i + 1].Width = (float) (XC1 + XWid * 0.5d - XP - Xoff);
                        LAB_Name_Align[i + 1].Alignment = StringAlignment.Near;
                    }

                    break;
                }

                case 20d:
                {
                    // R3.40 Has 80 pixel dead space in mid to go around COH2 clock at top of GUI.
                    for (i = 1; i <= 7; i += 2)
                    {
                        // R2.00 Adjust Right Group X values. All values are pushed right by XCoff for right side group.
                        LAB_Fact[i + 1].X = 50 + XCoff;
                        LAB_Fact[i + 1].Xtext = LAB_Fact[i + 1].X;
                        LAB_Rank[i + 1].X = 50 + XCoff + Hgt;
                        LAB_Rank[i + 1].Xtext = LAB_Rank[i + 1].X;
                        LAB_Name[i + 1].X = 50 + XCoff + Hgt + (XP - XR - Xoff);
                        LAB_Name[i + 1].Xtext = LAB_Name[i + 1].X;
                        LAB_Fact[i + 1].Width = Hgt;
                        LAB_Rank[i + 1].Width = XP - XR - Xoff;
                        LAB_Name[i + 1].Width = pbStats.Width - LAB_Name[i + 1].X - 10f;
                        LAB_Name_Align[i + 1].Alignment = StringAlignment.Near;

                        // R2.00 Adjust Left Group X values.
                        LAB_Fact[i].Width = Hgt;
                        LAB_Rank[i].Width = LAB_Rank[i + 1].Width;
                        LAB_Name[i].Width = LAB_Name[i + 1].Width;
                        LAB_Name[i].X = 2f;
                        LAB_Name[i].Xtext = LAB_Name[i].X;
                        LAB_Name_Align[i].Alignment = StringAlignment.Far;
                        LAB_Rank[i].X =
                            LAB_Name[i].X +
                            LAB_Name[i]
                                .Width; // R4.46 changed.  XCoff - Math.Abs(LAB_Rank(i + 1).X + LAB_Rank(i).Width - XCoff)
                        LAB_Rank[i].Xtext = LAB_Rank[i].X;
                        LAB_Fact[i].X =
                            LAB_Rank[i].X +
                            LAB_Rank[i].Width; // R4.46 changed.  XCoff - Math.Abs(LAB_Fact(i + 1).X + Hgt - XCoff)
                        LAB_Fact[i].Xtext = LAB_Fact[i].X;
                    }

                    break;
                }

                case 21d:
                {
                    // R3.40 Has 80 pixel dead space in mid to go around COH2 clock at top of GUI.
                    for (i = 1; i <= 7; i += 2)
                    {
                        // R2.00 Adjust Right Group X values. All values are pushed right by XCoff for right side group.
                        LAB_Fact[i + 1].X = 50 + XCoff;
                        LAB_Fact[i + 1].Xtext = LAB_Fact[i + 1].X;
                        LAB_Rank[i + 1].X = 50 + XCoff + Hgt + 5;
                        LAB_Rank[i + 1].Xtext = LAB_Rank[i + 1].X;
                        LAB_Name[i + 1].X = 50 + XCoff + Hgt + 5 + (XP - XR - Xoff);
                        LAB_Name[i + 1].Xtext = LAB_Name[i + 1].X;
                        LAB_Fact[i + 1].Width = Hgt;
                        LAB_Rank[i + 1].Width = XP - XR - Xoff - 5;
                        LAB_Name[i + 1].Width = pbStats.Width - LAB_Name[i + 1].X - 10f;
                        LAB_Name_Align[i + 1].Alignment = StringAlignment.Near;

                        // R2.00 Adjust Left Group X values.
                        LAB_Fact[i].Width = Hgt;
                        LAB_Rank[i].Width = LAB_Rank[i + 1].Width;
                        LAB_Name[i].Width = LAB_Name[i + 1].Width;
                        LAB_Name[i].X = 3f;
                        LAB_Name[i].Xtext = LAB_Name[i].X;
                        LAB_Name_Align[i].Alignment = StringAlignment.Far;
                        LAB_Rank[i].X =
                            LAB_Name[i].X + LAB_Name[i].Width +
                            5f; // R4.46 changed.  XCoff - Math.Abs(LAB_Rank(i + 1).X + LAB_Rank(i).Width - XCoff)
                        LAB_Rank[i].Xtext = LAB_Rank[i].X;
                        LAB_Fact[i].X =
                            LAB_Rank[i].X + LAB_Rank[i].Width +
                            5f; // R4.46 changed.  XCoff - Math.Abs(LAB_Fact(i + 1).X + Hgt - XCoff)
                        LAB_Fact[i].Xtext = LAB_Fact[i].X;
                    }

                    break;
                }
            }
        }

        public void STATS_ClipXY(float tX, float tY)
        {
            // R4.10 Verify and store valid sizes for STATS image.

            if (tX < 10f) tX = 980f; // R4.30 Changed.

            if (10000f < tX) tX = 10000f;

            STATS_SizeX = (int) Math.Round(tX);
            if (tY < 10f) tY = 180f; // R4.30 Changed.

            if (10000f < tY) tY = 10000f;

            STATS_SizeY = (int) Math.Round(tY);
        }

        private void STATS_AdjustSize()
        {
            // R4.50 Setup STATS page size. Added IF checks.
            if (pbStats.Width != STATS_SizeX) pbStats.Width = STATS_SizeX;

            if (pbStats.Height != STATS_SizeY) pbStats.Height = STATS_SizeY;

            if (scrStats.Left != pbStats.Left + pbStats.Width) scrStats.Left = pbStats.Left + pbStats.Width;

            if (scrStats.Height != pbStats.Height) scrStats.Height = pbStats.Height;
        }

        private void cboLayoutY_SelectedIndexChanged(object sender, EventArgs e)
        {
            // R3.40 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            STATS_DefineY();
            STATS_DefineX(); // R2.00 X gets adjusted to Y size for faction images.

            // R4.50 Force STATS redraw.
            MainBuffer_Valid = false;
            Settings.SETTINGS_Save("");
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void cboLayoutX_SelectedIndexChanged(object sender, EventArgs e)
        {
            // R3.40 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            STATS_DefineY();
            STATS_DefineX(); // R2.00 X gets adjusted to Y size for faction images.

            // R4.50 Force STATS redraw.
            MainBuffer_Valid = false;
            Settings.SETTINGS_Save("");
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void cmSizeRefresh_Click(object sender, EventArgs e)
        {
            // *****************************************************************
            // R4.30 If doing ELO cycles, calc the current Cycle to show.
            // *****************************************************************
            FLAG_EloMode += 1;
            if (2 < FLAG_EloMode) FLAG_EloMode = 0;

            // R4.20 Moved sub for outside calls.
            STATS_Refresh();
        }

        private void STATS_Refresh()
        {
            // R4.20 Added this sub.

            STATS_ClipXY((float) Conversion.Val(tbXsize.Text), (float) Conversion.Val(tbYSize.Text));
            STATS_AdjustSize();
            STATS_DefineY();
            STATS_DefineX(); // R2.00 X gets adjusted to Y size for faction images.
            Settings.SETTINGS_Save("");
            SCREEN_Organize();
            GFX_DrawStats();
        }


        private void frmMain_Shown(object sender, EventArgs e)
        {
            string A;
            FLAG_Loading = true; // R3.00 Tell Controls not to update.
            STATS_AdjustSize();
            STATS_DefineY();
            STATS_DefineX(); // R2.00 X gets adjusted to Y size for faction images.
            SCREEN_Organize();
            GFX_DrawStats();

            // R4.50 Added.
            if (string.IsNullOrEmpty(PATH_Game))
            {
                A =
                    "Before MakoCELO can operate it needs to locate the warnings.log file in your COH2 My Games directory." +
                    Constants.vbCr + Constants.vbCr;
                A = A + "Click on FIND LOG FILE to search and select the directory." + Constants.vbCr + Constants.vbCr;
                A = A + "Once the directory is located, match rankings can be found by either selecting ";
                A = A + "CHECK LOG FILE or AUTO SCAN LOG." + Constants.vbCr + Constants.vbCr;
                A = A + "Click the ABOUT button to get generic help information." + Constants.vbCr + Constants.vbCr;
                Interaction.MsgBox(A, MsgBoxStyle.Information);

                // R4.50 Turn on help TIPS for new users.
                chkTips.Checked = true;
            }

            FLAG_Loading = false; // R3.00 Tell Controls its OK to update.
        }

        private void GUI_SetDark()
        {
            Color CHover;
            Color CFore;
            Color CBack;
            Color CGroupFore;
            Color CLabFore;
            CGroupFore = Color.FromArgb(255, 255, 255, 255);
            CLabFore = Color.FromArgb(255, 192, 192, 192);
            CFore = Color.FromArgb(255, 232, 232, 232);
            CBack = Color.FromArgb(255, 48, 48, 48);
            CHover = Color.FromArgb(255, 48, 64, 48);
            BackColor = Color.FromArgb(255, 32, 32, 32);
            ForeColor = Color.FromArgb(255, 232, 232, 232);
            GUI_SetColor(CFore, CBack, CHover, CGroupFore, CLabFore);
        }

        private void GUI_SetLite()
        {
            Color CHover;
            Color CFore;
            Color CBack;
            Color CGroupFore;
            Color CLabFore;
            CGroupFore = Color.FromArgb(255, 0, 0, 0);
            CLabFore = Color.FromArgb(255, 32, 32, 32);
            CFore = Color.FromArgb(255, 0, 0, 0);
            CBack = Color.FromArgb(255, 232, 232, 232);
            CHover = Color.FromArgb(255, 232, 255, 232);
            BackColor = Color.FromArgb(255, 232, 232, 232);
            ForeColor = Color.FromArgb(255, 0, 0, 0);
            GUI_SetColor(CFore, CBack, CHover, CGroupFore, CLabFore);
        }

        private void GUI_SetColor(Color CFore, Color CBack, Color CHover, Color CGroupFore, Color CLabFore)
        {
            Button Butt;
            Label Labe;

            // R4.50 Added. 
            SS1.BackColor = CBack;
            SS1.ForeColor = CFore;

            // R4.00 Modified this whole sub.
            gbRank.ForeColor = CGroupFore;
            gbLayout.ForeColor = CGroupFore;
            gbFX.ForeColor = CGroupFore;
            gbSound.ForeColor = CGroupFore;
            lbTime.ForeColor = CLabFore;
            Label3.ForeColor = CLabFore;
            Label4.ForeColor = CLabFore;
            Label5.ForeColor = CLabFore;
            Label6.ForeColor = CLabFore;
            Label7.ForeColor = CLabFore;
            foreach (Control cntrl in gbRank.Controls)
                if (cntrl is Label)
                {
                    Labe = (Label) cntrl;
                    Labe.ForeColor = CLabFore;
                }

            foreach (Control cntrl in gbFX.Controls)
                if (cntrl is Label)
                {
                    Labe = (Label) cntrl;
                    Labe.ForeColor = CLabFore;
                }

            foreach (Control cntrl in Controls)
                if (cntrl is Button)
                {
                    Butt = (Button) cntrl;
                    Butt.BackColor = CBack;
                    Butt.ForeColor = CFore;
                    Butt.FlatAppearance.MouseOverBackColor = CHover;
                }

            foreach (Control cntrl in gbRank.Controls)
                if (cntrl is Button)
                {
                    Butt = (Button) cntrl;
                    Butt.BackColor = CBack;
                    Butt.ForeColor = CFore;
                    Butt.FlatAppearance.MouseOverBackColor = CHover;
                }

            foreach (Control cntrl in gbLayout.Controls)
                if (cntrl is Button)
                {
                    Butt = (Button) cntrl;
                    Butt.BackColor = CBack;
                    Butt.ForeColor = CFore;
                    Butt.FlatAppearance.MouseOverBackColor = CHover;
                }

            foreach (Control cntrl in gbFX.Controls)
                if (cntrl is Button)
                {
                    Butt = (Button) cntrl;
                    Butt.BackColor = CBack;
                    Butt.ForeColor = CFore;
                    Butt.FlatAppearance.MouseOverBackColor = CHover;
                }

            foreach (Control cntrl in gbSound.Controls)
                if (cntrl is Button)
                {
                    Butt = (Button) cntrl;
                    Butt.BackColor = CBack;
                    Butt.ForeColor = CFore;
                    Butt.FlatAppearance.MouseOverBackColor = CHover;
                }
        }

        public void FX_SetVarControls()
        {
            int N;
            N = cboFXVar1.SelectedIndex;
            if (0 < N)
            {
                cboFXVar1.Text = CFX3DVar[N, 1];
                cboFXVar2.Text = CFX3DVar[N, 2];
                cboFxVar3.Text = CFX3DVar[N, 3];
                cboFxVar4.Text = CFX3DVar[N, 4];
            }
        }

        private void OFFSET_Validate(ref int Xoff, ref int Yoff)
        {
            float TX, TY;
            TX = (float) Conversion.Val(tbXoff.Text);
            if (TX < -10000) TX = -10000;

            if (10000f < TX) TX = 10000f;

            Xoff = (int) Math.Round(TX);
            TY = (float) Conversion.Val(tbYoff.Text);
            if (TY < -10000) TY = -10000;

            if (10000f < TY) TY = 10000f;

            Yoff = (int) Math.Round(TY);
        }

        private void GFX_DrawPlayerCard(int N)
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
            var Xmid = (int) Math.Round(pbStats.Width * 0.5d);

            // R3.00 Precalc some vars for readability in code.
            tLabHgt = (int) ((long) Math.Round(LAB_Rank[1].Height) / 2L);

            // R4.30 Calc Y for each section.
            YFact[1] = (int) Math.Round(pbStats.Height / 3d * 0.5d - tLabHgt);
            YFact[2] = (int) Math.Round(pbStats.Height / 3d * 0.5d + pbStats.Height / 3d - tLabHgt);
            YFact[3] = (int) Math.Round(
                pbStats.Height / 3d * 0.5d + pbStats.Height / 3d + pbStats.Height / 3d - tLabHgt);
            YFact[4] = (int) Math.Round(pbStats.Height / 3d * 0.5d - tLabHgt);
            YFact[5] = (int) Math.Round(pbStats.Height / 3d * 0.5d + pbStats.Height / 3d - tLabHgt);
            YSec[1] = (int) Math.Round(pbStats.Height / 3d * 0d);
            YSec[2] = (int) Math.Round(pbStats.Height / 3d * 0.2d);
            YSec[3] = (int) Math.Round(pbStats.Height / 3d * 0.4d);
            YSec[4] = (int) Math.Round(pbStats.Height / 3d * 0.6d);
            YSec[5] = (int) Math.Round(pbStats.Height / 3d * 0.8d);
            YStart[1] = 0;
            YStart[2] = (int) Math.Round(pbStats.Height / 3d);
            YStart[3] = (int) Math.Round(pbStats.Height / 3d * 2d);
            YStart[4] = 0;
            YStart[5] = (int) Math.Round(pbStats.Height / 3d);
            XSec[1] = (int) Math.Round(LAB_Fact[1].Width + 10f);
            XSec[2] = (int) Math.Round(LAB_Fact[1].Width + 10f + pbStats.Width * 0.4d * 0.2d);
            XSec[3] = (int) Math.Round(LAB_Fact[1].Width + 10f + pbStats.Width * 0.4d * 0.4d);
            XSec[4] = (int) Math.Round(LAB_Fact[1].Width + 10f + pbStats.Width * 0.4d * 0.6d);
            XSec[5] = (int) Math.Round(LAB_Fact[1].Width + 10f + pbStats.Width * 0.4d * 0.8d);
            XSec[6] = (int) Math.Round(pbStats.Width * 0.5d + XSec[1]);
            XSec[7] = (int) Math.Round(pbStats.Width * 0.5d + XSec[2]);
            XSec[8] = (int) Math.Round(pbStats.Width * 0.5d + XSec[3]);
            XSec[9] = (int) Math.Round(pbStats.Width * 0.5d + XSec[4]);
            XSec[10] = (int) Math.Round(pbStats.Width * 0.5d + XSec[5]);
            fonRank = new Font("arial", (float) (pbStats.Width * 0.008d), FontStyle.Regular);

            // R4.10 Get STATS offsets.
            OFFSET_Validate(ref Xoff, ref Yoff);

            // R4.32 Adjust our working BMP if needed.
            STATS_AdjustSize();
            if ((Main_BM.Width != pbStats.Width) | (Main_BM.Height != pbStats.Height))
            {
                Main_BM = new Bitmap(pbStats.Width, pbStats.Height);
                Main_Gfx = Graphics.FromImage(Main_BM);
            }

            Main_Gfx.Clear(LSName.BackC);

            // *****************************************************************
            // R3.00 Draw the stats page background.
            // *****************************************************************
            if (NAME_bmp is null | (LSName.UseCardBack == false))
                // R3.00 No image available so draw a solid color background.
                Main_Gfx.FillRectangle(new SolidBrush(LSName.BackC), 0, 0, pbStats.Width, pbStats.Height);
            else
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(LSName.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        Main_Gfx.DrawImage(NAME_bmp, 0, 0, NAME_bmp.Width, NAME_bmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = Main_BM.Height;
                        for (tY = 0; NAME_bmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += NAME_bmp.Height)
                        {
                            var loopTo1 = Main_BM.Width;
                            for (tX = 0; NAME_bmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1; tX += NAME_bmp.Width)
                                Main_Gfx.DrawImage(NAME_bmp, tX, tY, NAME_bmp.Width, NAME_bmp.Height);
                        }

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        Main_Gfx.DrawImage(NAME_bmp, 0, 0, Main_BM.Width, Main_BM.Height);
                        break;
                    }
                }


            // *****************************************************************
            // Draw the Faction images to the stats page.
            // ***************************************************************** 
            BruRank = new SolidBrush(LSRank.F1);
            BruRank2 = new SolidBrush(LSRank.F2);
            BruName = new SolidBrush(LSName.F1);
            for (var t = 1; t <= 3; t++)
            {
                tPic = (PictureBox) Controls["pbFact0" + t];
                Main_Gfx.DrawImage(tPic.Image, 0f, YFact[t], LAB_Fact[t].Width, LAB_Fact[t].Height);
                if (t == 1)
                {
                    Main_Gfx.DrawString("Mode", fonRank, BruRank, XSec[1], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("Rank", fonRank, BruRank, XSec[2], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("Win", fonRank, BruRank, XSec[3], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("Loss", fonRank, BruRank, XSec[4], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("%", fonRank, BruRank, XSec[5], YStart[t] + YSec[1]);
                }

                Main_Gfx.DrawString("1v1", fonRank, BruRank2, XSec[1], YStart[t] + YSec[2]);
                if (PlrRankALL[N, t, 1] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 1].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[2]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 1].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[2]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 1].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[2]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 1], fonRank, BruName, XSec[5], YStart[t] + YSec[2]);
                Main_Gfx.DrawString("2v2", fonRank, BruRank2, XSec[1], YStart[t] + YSec[3]);
                if (PlrRankALL[N, t, 2] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 2].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[3]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 2].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[3]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 2].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[3]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 2], fonRank, BruName, XSec[5], YStart[t] + YSec[3]);
                Main_Gfx.DrawString("3v3", fonRank, BruRank2, XSec[1], YStart[t] + YSec[4]);
                if (PlrRankALL[N, t, 3] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 3].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[4]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 3].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[4]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 3].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[4]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 3], fonRank, BruName, XSec[5], YStart[t] + YSec[4]);
                Main_Gfx.DrawString("4v4", fonRank, BruRank2, XSec[1], YStart[t] + YSec[5]);
                if (PlrRankALL[N, t, 4] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 4].ToString(), fonRank, BruName, XSec[2], YStart[t] + YSec[5]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 4].ToString(), fonRank, BruName, XSec[3], YStart[t] + YSec[5]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 4].ToString(), fonRank, BruName, XSec[4], YStart[t] + YSec[5]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 4], fonRank, BruName, XSec[5], YStart[t] + YSec[5]);
            }

            for (var t = 4; t <= 5; t++)
            {
                tPic = (PictureBox) Controls["pbFact0" + t];
                Main_Gfx.DrawImage(tPic.Image, Xmid, YFact[t], LAB_Fact[t].Width, LAB_Fact[t].Height);
                if (t == 4)
                {
                    Main_Gfx.DrawString("Mode", fonRank, BruRank, XSec[6], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("Rank", fonRank, BruRank, XSec[7], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("Win", fonRank, BruRank, XSec[8], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("Loss", fonRank, BruRank, XSec[9], YStart[t] + YSec[1]);
                    Main_Gfx.DrawString("%", fonRank, BruRank, XSec[10], YStart[t] + YSec[1]);
                }

                Main_Gfx.DrawString("1v1", fonRank, BruRank2, XSec[6], YStart[t] + YSec[2]);
                if (PlrRankALL[N, t, 1] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 1].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[2]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 1].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[2]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 1].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[2]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 1], fonRank, BruName, XSec[10], YStart[t] + YSec[2]);
                Main_Gfx.DrawString("2v2", fonRank, BruRank2, XSec[6], YStart[t] + YSec[3]);
                if (PlrRankALL[N, t, 2] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 2].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[3]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 2].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[3]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 2].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[3]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 2], fonRank, BruName, XSec[10], YStart[t] + YSec[3]);
                Main_Gfx.DrawString("3v3", fonRank, BruRank2, XSec[6], YStart[t] + YSec[4]);
                if (PlrRankALL[N, t, 3] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 3].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[4]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 3].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[4]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 3].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[4]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 3], fonRank, BruName, XSec[10], YStart[t] + YSec[4]);
                Main_Gfx.DrawString("4v4", fonRank, BruRank2, XSec[6], YStart[t] + YSec[5]);
                if (PlrRankALL[N, t, 4] != 0)
                    Main_Gfx.DrawString(PlrRankALL[N, t, 4].ToString(), fonRank, BruName, XSec[7], YStart[t] + YSec[5]);

                Main_Gfx.DrawString(PlrRankWin[N, t, 4].ToString(), fonRank, BruName, XSec[8], YStart[t] + YSec[5]);
                Main_Gfx.DrawString(PlrRankLoss[N, t, 4].ToString(), fonRank, BruName, XSec[9], YStart[t] + YSec[5]);
                Main_Gfx.DrawString(PlrRankPerc[N, t, 4], fonRank, BruName, XSec[10], YStart[t] + YSec[5]);
            }

            Main_Gfx.DrawString("Player: " + PlrName[N], fonRank, BruRank, XSec[6], YStart[3] + YSec[2]);
            // R4.46 Removed.   Main_Gfx.DrawString("RelicID: " & PlrRID(N), fonRank, BruRank, XSec(6), YStart(3) + YSec(3))
            Main_Gfx.DrawString("Left Click to exit, Right Click for teams", fonRank, BruName, XSec[6],
                YStart[3] + YSec[4]);
            Main_Gfx.DrawString("Team Win: " + PlrTWin[N], fonRank, BruRank, XSec[9], YStart[3] + YSec[2]);
            Main_Gfx.DrawString("Team Loss: " + PlrTLoss[N], fonRank, BruRank, XSec[9], YStart[3] + YSec[3]);
            Main_Gfx.DrawString("Team: " + PlrTeam[N], fonRank, BruRank, XSec[9], YStart[3] + YSec[4]);

            // R4.45 Added country flags.
            if (!string.IsNullOrEmpty(PlrCountry[N]))
            {
                var BM_Country = Resources.ResourceManager.GetObject("flag_" + PlrCountry[N]) as Bitmap;
                if (!Information.IsNothing(BM_Country))
                    Main_Gfx.DrawImage(BM_Country, new Point(XSec[6] - 20, YStart[3] + YSec[2]));

                Main_Gfx.DrawString(PlrCountry[N], fonRank, BruRank, XSec[6] - 20, YStart[3] + YSec[3]);
                Main_Gfx.DrawString("Country: " + PlrCountryName[N], fonRank, BruRank, XSec[6],
                    YStart[3] + YSec[3]); // R4.46 Added.
            }

            pbStats.Image = Main_BM;
        }

        private void GFX_DrawTeams(int N)
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
            fonRank = new Font("arial", (float) (pbStats.Width * 0.008d), FontStyle.Regular);

            // R4.32 Adjust our working BMP if needed.
            STATS_AdjustSize();
            if ((Main_BM.Width != pbStats.Width) | (Main_BM.Height != pbStats.Height))
            {
                Main_BM = new Bitmap(pbStats.Width, pbStats.Height);
                Main_Gfx = Graphics.FromImage(Main_BM);
            }

            Main_Gfx.Clear(LSName.BackC);

            // *****************************************************************
            // R3.00 Draw the stats page background.
            // *****************************************************************
            if (NAME_bmp is null | (LSName.UseCardBack == false))
                // R3.00 No image available so draw a solid color background.
                Main_Gfx.FillRectangle(new SolidBrush(LSName.BackC), 0, 0, pbStats.Width, pbStats.Height);
            else
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(LSName.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        Main_Gfx.DrawImage(NAME_bmp, 0, 0, NAME_bmp.Width, NAME_bmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = Main_BM.Height;
                        for (tY = 0; NAME_bmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += NAME_bmp.Height)
                        {
                            var loopTo1 = Main_BM.Width;
                            for (tX = 0; NAME_bmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1; tX += NAME_bmp.Width)
                                Main_Gfx.DrawImage(NAME_bmp, tX, tY, NAME_bmp.Width, NAME_bmp.Height);
                        }

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        Main_Gfx.DrawImage(NAME_bmp, 0, 0, Main_BM.Width, Main_BM.Height);
                        break;
                    }
                }

            // *****************************************************************
            // Draw the Faction images to the stats page.
            // ***************************************************************** 
            BruRank = new SolidBrush(LSRank.F1);
            BruRank2 = new SolidBrush(LSRank.F2);
            BruName = new SolidBrush(LSName.F1);
            Yoff = scrStats.Value * -1; // * -75

            // R4.40 Loop thru TEAM lists and draw them until we are off screen.
            for (int t = 1, loopTo2 = TeamListCnt[N]; t <= loopTo2; t++)
            {
                YAct = Yoff + (t - 1) * 90;
                if (pbStats.Height < YAct) break;

                if (-91 < YAct)
                {
                    Main_Gfx.DrawString(t.ToString(), fonRank, BruName, 10f, YAct);
                    Main_Gfx.DrawString("Team of ", fonRank, BruRank, 45f, YAct);
                    Main_Gfx.DrawString(TeamList[N, t].PlrCnt.ToString(), fonRank, BruName, 90f, YAct);
                    Main_Gfx.DrawString("Allies:", fonRank, BruRank, 120f, YAct);
                    Main_Gfx.DrawString("Axis:", fonRank, BruRank, 260f, YAct);
                    A = " (" + TeamList[N, t].WinAllies + "," + TeamList[N, t].LossAllies + ")";
                    switch (TeamList[N, t].RankAllies)
                    {
                        case -1:
                        {
                            Main_Gfx.DrawString("P" + A, fonRank, BruName, 160f, YAct);
                            break;
                        }

                        case 0:
                        {
                            Main_Gfx.DrawString("---" + A, fonRank, BruName, 160f, YAct);
                            break;
                        }

                        default:
                        {
                            Main_Gfx.DrawString(TeamList[N, t].RankAllies + A, fonRank, BruName, 160f, YAct);
                            break;
                        }
                    }

                    A = " (" + TeamList[N, t].WinAxis + "," + TeamList[N, t].LossAxis + ")";
                    switch (TeamList[N, t].RankAxis)
                    {
                        case -1:
                        {
                            Main_Gfx.DrawString("P" + A, fonRank, BruName, 290f, YAct);
                            break;
                        }

                        case 0:
                        {
                            Main_Gfx.DrawString("---" + A, fonRank, BruName, 290f, YAct);
                            break;
                        }

                        default:
                        {
                            Main_Gfx.DrawString(TeamList[N, t].RankAxis + A, fonRank, BruName, 290f, YAct);
                            break;
                        }
                    }

                    Main_Gfx.DrawString("1:", fonRank, BruRank2, 10f, YAct + 15);
                    Main_Gfx.DrawString("2:", fonRank, BruRank2, 10f, YAct + 30);
                    Main_Gfx.DrawString("3:", fonRank, BruRank2, 10f, YAct + 45);
                    Main_Gfx.DrawString("4:", fonRank, BruRank2, 10f, YAct + 60);
                    Main_Gfx.DrawString(TeamList[N, t].PLR1, fonRank, BruName, 25f, YAct + 15);
                    Main_Gfx.DrawString(TeamList[N, t].PLR2, fonRank, BruName, 25f, YAct + 30);
                    Main_Gfx.DrawString(TeamList[N, t].PLR3, fonRank, BruName, 25f, YAct + 45);
                    Main_Gfx.DrawString(TeamList[N, t].PLR4, fonRank, BruName, 25f, YAct + 60);
                }
            }

            Main_Gfx.DrawString("PREMADE TEAM RANKS", fonRank, BruRank2, 500f, 0f);
            Main_Gfx.DrawString("Player: " + PlrName[N], fonRank, BruRank, 500f, 15f);
            Main_Gfx.DrawString("RelicID: " + PlrRID[N], fonRank, BruRank, 500f, 30f);
            Main_Gfx.DrawString("Click the screen again to exit", fonRank, BruName, 500f, 45f);
            pbStats.Image = Main_BM;
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
            OFFSET_Validate(ref Xoff, ref Yoff);

            // *****************************************************************
            // R3.00 Draw the stats page background.
            // *****************************************************************
            if (NAME_bmp is null)
                // R3.00 No image available so draw a solid color background.
                Main_Gfx.FillRectangle(new SolidBrush(LSName.BackC), 0, 0, pbStats.Width, pbStats.Height);
            else
                // R3.00 Scale and Draw the background image.
                switch (Conversion.Val(LSName.Scaling))
                {
                    case 0d: // R3.00 Normal Scaling
                    {
                        Main_Gfx.DrawImage(NAME_bmp, 0, 0, NAME_bmp.Width, NAME_bmp.Height);
                        break;
                    }

                    case 1d: // R3.00 Tiled Scaling
                    {
                        var loopTo = Main_BM.Height;
                        for (tY = 0; NAME_bmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += NAME_bmp.Height)
                        {
                            var loopTo1 = Main_BM.Width;
                            for (tX = 0; NAME_bmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1; tX += NAME_bmp.Width)
                                Main_Gfx.DrawImage(NAME_bmp, tX, tY, NAME_bmp.Width, NAME_bmp.Height);
                        }

                        break;
                    }

                    case 2d: // R3.00 Stretch/Fit Scaling
                    {
                        Main_Gfx.DrawImage(NAME_bmp, 0, 0, Main_BM.Width, Main_BM.Height);
                        break;
                    }
                }

            switch (LSRank.BorderPanelMode)
            {
                case 0: // R4.40 No border.
                {
                    break;
                }

                case 1: // R4.40 Normal.
                {
                    // Main_Gfx.draw
                    tPen = new Pen(new SolidBrush(LSRank.BorderPanelColor))
                    {
                        Width = LSRank.BorderPanelWidth
                    };
                    Main_Gfx.DrawRectangle(tPen, 2, 2, Main_BM.Width - 5, Main_BM.Height - 5);
                    break;
                }

                case 2: // R4.40 Glow
                {
                    Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(255, LSRank.BorderPanelColor.R, LSRank.BorderPanelColor.G,
                            LSRank.BorderPanelColor.B))), 2, 2, Main_BM.Width - 5, Main_BM.Height - 5);
                    Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(192, LSRank.BorderPanelColor.R, LSRank.BorderPanelColor.G,
                            LSRank.BorderPanelColor.B))), 3, 3, Main_BM.Width - 7, Main_BM.Height - 7);
                    Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(128, LSRank.BorderPanelColor.R, LSRank.BorderPanelColor.G,
                            LSRank.BorderPanelColor.B))), 4, 4, Main_BM.Width - 9, Main_BM.Height - 9);
                    Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(64, LSRank.BorderPanelColor.R, LSRank.BorderPanelColor.G,
                            LSRank.BorderPanelColor.B))), 5, 5, Main_BM.Width - 11, Main_BM.Height - 11);
                    Main_Gfx.DrawRectangle(
                        new Pen(new SolidBrush(Color.FromArgb(32, LSRank.BorderPanelColor.R, LSRank.BorderPanelColor.G,
                            LSRank.BorderPanelColor.B))), 6, 6, Main_BM.Width - 13, Main_BM.Height - 13);
                    break;
                }

                case 3: // R4.40 3D
                {
                    Main_Gfx.DrawLine(new Pen(new SolidBrush(LSRank.BorderPanelColor)), 2, 2, Main_BM.Width - 3, 2);
                    Main_Gfx.DrawLine(new Pen(new SolidBrush(LSRank.BorderPanelColor)), 2, 2, 2, Main_BM.Height - 3);
                    Main_Gfx.DrawLine(new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0))), 2, Main_BM.Height - 3,
                        Main_BM.Width - 3, Main_BM.Height - 3);
                    Main_Gfx.DrawLine(new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0))), Main_BM.Width - 3, 2,
                        Main_BM.Width - 3, Main_BM.Height - 2);
                    break;
                }

                case 4: // R4.40 Rounded Corners 4 pixels.
                {
                    tPen = new Pen(new SolidBrush(LSRank.BorderPanelColor))
                    {
                        Width = LSRank.BorderPanelWidth
                    };
                    DrawRoundedRectangle(Main_Gfx, new Rectangle(2, 2, Main_BM.Width - 5, Main_BM.Height - 5), tPen, 4);
                    break;
                }

                case 5: // R4.40 Rounded Corners 8 pixels.
                {
                    tPen = new Pen(new SolidBrush(LSRank.BorderPanelColor))
                    {
                        Width = LSRank.BorderPanelWidth
                    };
                    DrawRoundedRectangle(Main_Gfx, new Rectangle(2, 2, Main_BM.Width - 5, Main_BM.Height - 5), tPen, 8);
                    break;
                }

                case 6: // R4.40 Rounded Corners LARGE.
                {
                    tPen = new Pen(new SolidBrush(LSRank.BorderPanelColor))
                    {
                        Width = LSRank.BorderPanelWidth
                    };
                    DrawRoundedRectangle_Max(Main_Gfx, new Rectangle(2, 2, Main_BM.Width - 5, Main_BM.Height - 5),
                        tPen);
                    break;
                }
            }

            // *****************************************************************
            // R3.10 Call any background based FX MODE options like SHADOW, etc
            // *****************************************************************
            if (CFX3DActive[(int) clsGlobal.FXMode.LabelBlur]) GFX_FX_LabBlur(Main_Gfx, Main_BM, Xoff, Yoff);

            // *****************************************************************
            // R3.10 Draw the Rank background rectangles
            // *****************************************************************
            for (var T = 1; T <= 8; T++)
                if (!FLAG_HideMissing | !string.IsNullOrEmpty(PlrName[T]) | !string.IsNullOrEmpty(PlrSteam[T]))
                {
                    // R3.00 Draw the RANK background rectangle.
                    linGrBrush = LSRank.BDir == 0
                        ? new LinearGradientBrush(new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Rank[T].Y - 1f)),
                            new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Rank[T].Y + LAB_Rank[T].Height + 1f)),
                            LSRank.B1, LSRank.B2)
                        : new LinearGradientBrush(new Point((int) Math.Round(Xoff + LAB_Rank[T].X - 1f), Yoff + 0),
                            new Point((int) Math.Round(Xoff + LAB_Rank[T].X + LAB_Rank[T].Width + 1f), Yoff + 0),
                            LSRank.B1, LSRank.B2);

                    // R4.40 Added BORDERS.
                    switch (LSRank.BorderMode)
                    {
                        case 0: // R4.40 No border.
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y,
                                LAB_Rank[T].Width, LAB_Rank[T].Height);
                            break;
                        }

                        case 1: // R4.40 Normal.
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y,
                                LAB_Rank[T].Width, LAB_Rank[T].Height);
                            tPen = new Pen(new SolidBrush(LSRank.BorderColor))
                            {
                                Width = LSRank.BorderWidth
                            };
                            Main_Gfx.DrawRectangle(tPen, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y, LAB_Rank[T].Width,
                                LAB_Rank[T].Height);
                            break;
                        }

                        case 2: // R4.40 Glow
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y,
                                LAB_Rank[T].Width, LAB_Rank[T].Height);
                            Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(255, LSRank.BorderColor.R, LSRank.BorderColor.G,
                                    LSRank.BorderColor.B))), Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y,
                                LAB_Rank[T].Width - 1f, LAB_Rank[T].Height - 1f);
                            Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(192, LSRank.BorderColor.R, LSRank.BorderColor.G,
                                    LSRank.BorderColor.B))), Xoff + LAB_Rank[T].X + 1f, Yoff + LAB_Rank[T].Y + 1f,
                                LAB_Rank[T].Width - 3f, LAB_Rank[T].Height - 3f);
                            Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(64, LSRank.BorderColor.R, LSRank.BorderColor.G,
                                    LSRank.BorderColor.B))), Xoff + LAB_Rank[T].X + 2f, Yoff + LAB_Rank[T].Y + 2f,
                                LAB_Rank[T].Width - 5f, LAB_Rank[T].Height - 5f);
                            break;
                        }

                        case 3: // R4.40 3D
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y,
                                LAB_Rank[T].Width, LAB_Rank[T].Height);
                            tPen = new Pen(new SolidBrush(LSRank.BorderColor))
                            {
                                Width = LSRank.BorderWidth
                            };
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y,
                                Xoff + LAB_Rank[T].X + LAB_Rank[T].Width - 1f, Yoff + LAB_Rank[T].Y);
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y, Xoff + LAB_Rank[T].X,
                                Yoff + LAB_Rank[T].Y + LAB_Rank[T].Height - 1f);
                            tPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                            {
                                Width = LSRank.BorderWidth
                            };
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Rank[T].X, Yoff + LAB_Rank[T].Y + LAB_Rank[T].Height,
                                Xoff + LAB_Rank[T].X + LAB_Rank[T].Width, Yoff + LAB_Rank[T].Y + LAB_Rank[T].Height);
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Rank[T].X + LAB_Rank[T].Width, Yoff + LAB_Rank[T].Y,
                                Xoff + LAB_Rank[T].X + LAB_Rank[T].Width, Yoff + LAB_Rank[T].Y + LAB_Rank[T].Height);
                            break;
                        }

                        case 4: // R4.40 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width),
                                    (int) Math.Round(LAB_Rank[T].Height)), linGrBrush, 4);
                            tPen = new Pen(new SolidBrush(LSRank.BorderColor))
                            {
                                Width = LSRank.BorderWidth
                            };
                            DrawRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width),
                                    (int) Math.Round(LAB_Rank[T].Height)), tPen, 4);
                            break;
                        }

                        case 5: // R4.40 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width),
                                    (int) Math.Round(LAB_Rank[T].Height)), linGrBrush, 8);
                            tPen = new Pen(new SolidBrush(LSRank.BorderColor))
                            {
                                Width = LSRank.BorderWidth
                            };
                            DrawRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width),
                                    (int) Math.Round(LAB_Rank[T].Height)), tPen, 8);
                            break;
                        }

                        case 6: // R4.40 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width),
                                    (int) Math.Round(LAB_Rank[T].Height)), linGrBrush);
                            tPen = new Pen(new SolidBrush(LSRank.BorderColor))
                            {
                                Width = LSRank.BorderWidth
                            };
                            DrawRoundedRectangle_Max(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X),
                                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width),
                                    (int) Math.Round(LAB_Rank[T].Height)), tPen);
                            break;
                        }
                    }
                }

            // *****************************************************************
            // R3.10 Draw the Name background rectangles
            // *****************************************************************  
            for (var T = 1; T <= 8; T++)
                if (!FLAG_HideMissing | !string.IsNullOrEmpty(PlrName[T]))
                {
                    // R3.00 Draw the NAME background rectangle.
                    if (LSName.BDir == 0)
                        linGrBrush = new LinearGradientBrush(
                            new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Name[T].Y - 1f)),
                            new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Name[T].Y + LAB_Name[T].Height + 1f)),
                            LSName.B1, LSName.B2);
                    // R3.40 Reverse gradient direction for RIGHT justified text.
                    else if (LAB_Name_Align[T].Alignment == StringAlignment.Far)
                        linGrBrush = new LinearGradientBrush(
                            new Point((int) Math.Round(Xoff + LAB_Name[T].X + LAB_Name[T].Width + 1f), Yoff + 0),
                            new Point((int) Math.Round(Xoff + LAB_Name[T].X - 1f), Yoff + 0), LSName.B1, LSName.B2);
                    else
                        linGrBrush = new LinearGradientBrush(
                            new Point((int) Math.Round(Xoff + LAB_Name[T].X - 1f), Yoff + 0),
                            new Point((int) Math.Round(Xoff + LAB_Name[T].X + LAB_Name[T].Width + 1f), Yoff + 0),
                            LSName.B1, LSName.B2);

                    switch (LSName.BorderMode)
                    {
                        case 0:
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y,
                                LAB_Name[T].Width, LAB_Name[T].Height);
                            break;
                        }

                        case 1: // R4.40 Normal.
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y,
                                LAB_Name[T].Width, LAB_Name[T].Height);
                            tPen = new Pen(new SolidBrush(LSName.BorderColor))
                            {
                                Width = LSName.BorderWidth
                            };
                            Main_Gfx.DrawRectangle(tPen, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y, LAB_Name[T].Width,
                                LAB_Name[T].Height);
                            break;
                        }

                        case 2: // R4.40 Glow
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y,
                                LAB_Name[T].Width, LAB_Name[T].Height);
                            Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(255, LSName.BorderColor.R, LSName.BorderColor.G,
                                    LSName.BorderColor.B))), Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y,
                                LAB_Name[T].Width - 1f, LAB_Name[T].Height - 1f);
                            Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(192, LSName.BorderColor.R, LSName.BorderColor.G,
                                    LSName.BorderColor.B))), Xoff + LAB_Name[T].X + 1f, Yoff + LAB_Name[T].Y + 1f,
                                LAB_Name[T].Width - 3f, LAB_Name[T].Height - 3f);
                            Main_Gfx.DrawRectangle(
                                new Pen(new SolidBrush(Color.FromArgb(64, LSName.BorderColor.R, LSName.BorderColor.G,
                                    LSName.BorderColor.B))), Xoff + LAB_Name[T].X + 2f, Yoff + LAB_Name[T].Y + 2f,
                                LAB_Name[T].Width - 5f, LAB_Name[T].Height - 5f);
                            break;
                        }

                        case 3: // R4.40 3D
                        {
                            Main_Gfx.FillRectangle(linGrBrush, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y,
                                LAB_Name[T].Width, LAB_Name[T].Height);
                            tPen = new Pen(new SolidBrush(LSName.BorderColor))
                            {
                                Width = LSName.BorderWidth
                            };
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y,
                                Xoff + LAB_Name[T].X + LAB_Name[T].Width - 1f, Yoff + LAB_Name[T].Y);
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y, Xoff + LAB_Name[T].X,
                                Yoff + LAB_Name[T].Y + LAB_Name[T].Height - 1f);
                            tPen = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0)))
                            {
                                Width = LSName.BorderWidth
                            };
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Name[T].X, Yoff + LAB_Name[T].Y + LAB_Name[T].Height,
                                Xoff + LAB_Name[T].X + LAB_Name[T].Width - 1f,
                                Yoff + LAB_Name[T].Y + LAB_Name[T].Height);
                            Main_Gfx.DrawLine(tPen, Xoff + LAB_Name[T].X + LAB_Name[T].Width, Yoff + LAB_Name[T].Y,
                                Xoff + LAB_Name[T].X + LAB_Name[T].Width,
                                Yoff + LAB_Name[T].Y + LAB_Name[T].Height - 1f);
                            break;
                        }

                        case 4: // R4.40 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X),
                                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width),
                                    (int) Math.Round(LAB_Name[T].Height)), linGrBrush, 4);
                            tPen = new Pen(new SolidBrush(LSName.BorderColor))
                            {
                                Width = LSName.BorderWidth
                            };
                            DrawRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X),
                                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width),
                                    (int) Math.Round(LAB_Name[T].Height)), tPen, 4);
                            break;
                        }

                        case 5: // R4.40 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X),
                                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width),
                                    (int) Math.Round(LAB_Name[T].Height)), linGrBrush, 8);
                            tPen = new Pen(new SolidBrush(LSName.BorderColor))
                            {
                                Width = LSName.BorderWidth
                            };
                            DrawRoundedRectangle(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X),
                                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width),
                                    (int) Math.Round(LAB_Name[T].Height)), tPen, 8);
                            break;
                        }

                        case 6: // R4.40 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X),
                                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width),
                                    (int) Math.Round(LAB_Name[T].Height)), linGrBrush);
                            tPen = new Pen(new SolidBrush(LSName.BorderColor))
                            {
                                Width = LSName.BorderWidth
                            };
                            DrawRoundedRectangle_Max(Main_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X),
                                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width),
                                    (int) Math.Round(LAB_Name[T].Height)), tPen);
                            break;
                        }
                    }
                }

            // R4.50 Save the background image we just created for fast draws later.
            MainBuffer_BM = new Bitmap(pbStats.Width, pbStats.Height);
            MainBuffer_Gfx = Graphics.FromImage(MainBuffer_BM);
            MainBuffer_Gfx.DrawImage(Main_BM, 0, 0);
            MainBuffer_Valid = true;
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
            tLabHgt = (int) ((long) Math.Round(LAB_Rank[1].Height) / 2L);

            // R4.10 Get STATS offsets.
            OFFSET_Validate(ref Xoff, ref Yoff);

            // *****************************************************************
            // R3.10 Call any MID draw based FX MODE options like SHADOW, etc
            // *****************************************************************
            if (CFX3DActive[(int) clsGlobal.FXMode.Shadow]) GFX_FX_Shadow(Main_Gfx, Xoff, Yoff);

            // *****************************************************************
            // R3.00 Define paint/fill brushes for the RANK stats.
            // *****************************************************************
            var tBrushShadow = new SolidBrush(LSRank.ShadowColor);
            TextHgt12 = (int) Math.Round(Main_Gfx.MeasureString("X", FONT_Rank).Height /
                                         2f); // R3.30 Calc height of gradient color.  'R3.30Changed from Xq.
            for (var T = 1; T <= 8; T++)
            {
                if (FLAG_EloUse == false)
                {
                    tString = PlrRank[T];
                }
                else
                {
                    if (FLAG_EloMode == 0) tString = PlrRank[T];

                    if (FLAG_EloMode == 1) tString = PlrELO[T];

                    if (FLAG_EloMode == 2) tString = PlrLVL[T];
                }

                // R3.00 Create a clipping area so names do not draw past the rectangle.
                Main_Gfx.Clip = new Region(new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X + 1f),
                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width - 2f),
                    (int) Math.Round(LAB_Rank[T].Height)));

                // R4.00 Draw the RANK Shadow text.
                Cx = (int) Math.Round(LAB_Rank[T].X + LAB_Rank[T].Width / 2f -
                                      Main_Gfx.MeasureString(tString, FONT_Rank).Width / 2f);
                Cy = (int) Math.Round(LAB_Rank[T].Y + LAB_Rank[T].Height / 2f -
                                      Main_Gfx.MeasureString(tString, FONT_Rank).Height / 2f);
                if ((LSRank.ShadowX != 0) | (LSRank.ShadowY != 0))
                    Main_Gfx.DrawString(tString, FONT_Rank, tBrushShadow,
                        (float) (Xoff + Cx + LSRank.ShadowX * Conversion.Val(LSRank.ShadowDepth)),
                        (float) (Yoff + Cy + LSRank.ShadowY * Conversion.Val(LSRank.ShadowDepth)));

                // R3.00 Draw the RANK text.
                linGrBrush = LSRank.FDir == 0
                    ? new LinearGradientBrush(
                        new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Rank[T].Ycenter - TextHgt12)),
                        new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Rank[T].Ycenter + TextHgt12)),
                        Color.FromArgb(255, LSRank.F1.R, LSRank.F1.G, LSRank.F1.B),
                        Color.FromArgb(255, LSRank.F2.R, LSRank.F2.G, LSRank.F2.B))
                    : new LinearGradientBrush(new Point((int) Math.Round(Xoff + LAB_Rank[T].X), Yoff + 0),
                        new Point((int) Math.Round(Xoff + LAB_Rank[T].X + LAB_Rank[T].Width), Yoff + 0),
                        Color.FromArgb(255, LSRank.F1.R, LSRank.F1.G, LSRank.F1.B),
                        Color.FromArgb(255, LSRank.F2.R, LSRank.F2.G, LSRank.F2.B));

                Main_Gfx.DrawString(tString, FONT_Rank, linGrBrush, Xoff + Cx, Yoff + Cy);

                // R3.00 Clear the clipping area.
                Main_Gfx.Clip = new Region(new Rectangle(0, 0, Main_BM.Width, Main_BM.Height));
            }

            // *****************************************************************
            // Draw the Faction images to the stats page.
            // ***************************************************************** 
            for (var t = 1; t <= 8; t++)
                if (!string.IsNullOrEmpty(PlrFact[t]))
                {
                    tPic = (PictureBox) Controls["pbFact" + PlrFact[t]];
                    Main_Gfx.DrawImage(tPic.Image, Xoff + LAB_Fact[t].X, Yoff + LAB_Fact[t].Y, LAB_Fact[t].Width,
                        LAB_Fact[t].Height);
                }

            // *****************************************************************
            // R3.00 Define paint/fill brushes for the NAME stats.
            // *****************************************************************  
            tBrushShadow = new SolidBrush(LSName.ShadowColor);
            TextHgt12 = (int) Math.Round(Main_Gfx.MeasureString("X", FONT_Name).Height / 2f); // R3.30 Changed from Xq.
            for (var T = 1; T <= 8; T++)
            {
                // R3.00 Create a clipping area so names do not draw past the rectangle.
                Main_Gfx.Clip = new Region(new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X + 1f),
                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width - 2f),
                    (int) Math.Round(LAB_Name[T].Height)));

                // R3.40 Adjust the text X position if RIGHT justified.
                if (LAB_Name_Align[T].Alignment == StringAlignment.Far)
                    LAB_Name[T].Xtext = LAB_Name[T].X + LAB_Name[T].Width;

                // R4.00 Draw the NAME shadow text.
                Cx = (int) Math.Round(Xoff + LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + LAB_Name[T].Y + tLabHgt - TextHgt12);
                if ((LSName.ShadowX != 0) | (LSName.ShadowY != 0))
                {
                    if (chkCountry.Checked)
                    {
                        // R4.50 Normal X layout style.
                        if (Conversion.Val(cboLayoutX.Text) < 10d)
                        {
                            Main_Gfx.DrawString(PlrName[T], FONT_Name, tBrushShadow,
                                (float) (Cx + 18 + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth)),
                                (float) (Cy + LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(PlrCountry[T]))
                                Main_Gfx.FillRectangle(tBrushShadow,
                                    (float) (Cx + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth) + 4d),
                                    (float) (Yoff + LAB_Name[T].Y + tLabHgt - 6f +
                                             LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), 16f, 11f);
                        }
                        // R4.50 Centered X layout style.
                        else if (Conversions.ToBoolean(T % 2))
                        {
                            // R4.46 ODD players.
                            Main_Gfx.DrawString(PlrName[T], FONT_Name, tBrushShadow,
                                (float) (Cx - 19 + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth)),
                                (float) (Cy + LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(PlrCountry[T]))
                                Main_Gfx.FillRectangle(tBrushShadow,
                                    (float) (Cx + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth) - 20d),
                                    (float) (Yoff + LAB_Name[T].Y + tLabHgt - 6f +
                                             LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), 16f, 11f);
                        }
                        else
                        {
                            // R4.50 Even players.
                            Main_Gfx.DrawString(PlrName[T], FONT_Name, tBrushShadow,
                                (float) (Cx + 19 + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth)),
                                (float) (Cy + LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(PlrCountry[T]))
                                Main_Gfx.FillRectangle(tBrushShadow,
                                    (float) (Cx + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth) + 4d),
                                    (float) (Yoff + LAB_Name[T].Y + tLabHgt - 6f +
                                             LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), 16f, 11f);
                        }
                    }
                    else
                    {
                        Main_Gfx.DrawString(PlrName[T], FONT_Name, tBrushShadow,
                            (float) (Cx + LSName.ShadowX * Conversion.Val(LSName.ShadowDepth)),
                            (float) (Cy + LSName.ShadowY * Conversion.Val(LSName.ShadowDepth)), LAB_Name_Align[T]);
                    }
                }

                // R3.00 Setup the LINEAR brush for NAME text.
                if (LSName.FDir == 0)
                    linGrBrush = new LinearGradientBrush(
                        new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Name[T].Ycenter - TextHgt12)),
                        new Point(Xoff + 0, (int) Math.Round(Yoff + LAB_Name[T].Ycenter + TextHgt12)),
                        Color.FromArgb(255, LSName.F1.R, LSName.F1.G, LSName.F1.B),
                        Color.FromArgb(255, LSName.F2.R, LSName.F2.G, LSName.F2.B));
                // R3.40 Reverse gradient if drawing RIGHT justified text.
                else if (LAB_Name_Align[T].Alignment == StringAlignment.Far)
                    linGrBrush = new LinearGradientBrush(
                        new Point((int) Math.Round(Xoff + LAB_Name[T].X + LAB_Name[T].Width), Yoff + 0),
                        new Point((int) Math.Round(Xoff + LAB_Name[T].X), Yoff + 0),
                        Color.FromArgb(255, LSName.F1.R, LSName.F1.G, LSName.F1.B),
                        Color.FromArgb(255, LSName.F2.R, LSName.F2.G, LSName.F2.B));
                else
                    linGrBrush = new LinearGradientBrush(new Point((int) Math.Round(Xoff + LAB_Name[T].X), Yoff + 0),
                        new Point((int) Math.Round(Xoff + LAB_Name[T].X + LAB_Name[T].Width), Yoff + 0),
                        Color.FromArgb(255, LSName.F1.R, LSName.F1.G, LSName.F1.B),
                        Color.FromArgb(255, LSName.F2.R, LSName.F2.G, LSName.F2.B));

                // R3.00 Draw the NAME Text.
                Cx = (int) Math.Round(Xoff + LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + LAB_Name[T].Y + tLabHgt - TextHgt12);
                if (chkCountry.Checked)
                {
                    // R4.50 Normal X layout style.
                    if (Conversion.Val(cboLayoutX.Text) < 10d)
                    {
                        Main_Gfx.DrawString(PlrName[T], FONT_Name, linGrBrush, Cx + 18, Cy, LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(PlrCountry[T]))
                        {
                            var BM_Country = Resources.ResourceManager.GetObject("flag_" + PlrCountry[T]) as Bitmap;
                            if (!Information.IsNothing(BM_Country))
                                Main_Gfx.DrawImage(BM_Country,
                                    new Point(Cx + 4, (int) Math.Round(Yoff + LAB_Name[T].Y + tLabHgt - 6f)));
                        }
                    }
                    // R4.46 Centered X layout style.
                    else if (Conversions.ToBoolean(T % 2))
                    {
                        // R4.50 ODD players.
                        Main_Gfx.DrawString(PlrName[T], FONT_Name, linGrBrush, Cx - 19, Cy, LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(PlrCountry[T]))
                        {
                            var BM_Country = Resources.ResourceManager.GetObject("flag_" + PlrCountry[T]) as Bitmap;
                            if (!Information.IsNothing(BM_Country))
                                Main_Gfx.DrawImage(BM_Country,
                                    new Point(Cx - 20, (int) Math.Round(Yoff + LAB_Name[T].Y + tLabHgt - 6f)));
                        }
                    }
                    else
                    {
                        // R4.50 Even players.
                        Main_Gfx.DrawString(PlrName[T], FONT_Name, linGrBrush, Cx + 19, Cy, LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(PlrCountry[T]))
                        {
                            var BM_Country = Resources.ResourceManager.GetObject("flag_" + PlrCountry[T]) as Bitmap;
                            if (!Information.IsNothing(BM_Country))
                                Main_Gfx.DrawImage(BM_Country,
                                    new Point(Cx + 4, (int) Math.Round(Yoff + LAB_Name[T].Y + tLabHgt - 6f)));
                        }
                    }
                }
                else
                {
                    Main_Gfx.DrawString(PlrName[T], FONT_Name, linGrBrush, Cx, Cy, LAB_Name_Align[T]);
                }

                // R3.00 Clear the clipping area.
                Main_Gfx.Clip = new Region(new Rectangle(0, 0, Main_BM.Width, Main_BM.Height));
            }

            // *****************************************************************
            // R3.10 Call any foreground based FX MODE options like SHADOW, etc
            // *****************************************************************
            if (CFX3DActive[(int) clsGlobal.FXMode.Emboss]) GFX_FX_Emboss(Main_BM, Xoff, Yoff);


            // *****************************************************************
            // R4.00 Draw the OVERLAY image.
            // *****************************************************************
            if (NAME_OVLBmp is object)
                // R4.00 Scale and Draw the background image.
                switch (Conversion.Val(LSName.OVLScaling))
                {
                    case 0d: // R4.00 Normal Scaling
                    {
                        Main_Gfx.DrawImage(NAME_OVLBmp, 0, 0, NAME_OVLBmp.Width, NAME_OVLBmp.Height);
                        break;
                    }

                    case 1d: // R4.00 Tiled Scaling
                    {
                        var loopTo = Main_BM.Height;
                        for (tY = 0; NAME_OVLBmp.Height >= 0 ? tY <= loopTo : tY >= loopTo; tY += NAME_OVLBmp.Height)
                        {
                            var loopTo1 = Main_BM.Width;
                            for (tX = 0;
                                NAME_OVLBmp.Width >= 0 ? tX <= loopTo1 : tX >= loopTo1;
                                tX += NAME_OVLBmp.Width)
                                Main_Gfx.DrawImage(NAME_OVLBmp, tX, tY, NAME_OVLBmp.Width, NAME_OVLBmp.Height);
                        }

                        break;
                    }

                    case 2d: // R4.00 Stretch/Fit Scaling
                    {
                        // R4.00 MS sux.
                        if ((NAME_OVLBmp.Width < Main_BM.Width) | (NAME_OVLBmp.Height < Main_BM.Height))
                        {
                            var OvlXoff = (float) (Main_BM.Width / (double) NAME_OVLBmp.Width);
                            var OvlYoff = (float) (Main_BM.Height / (double) NAME_OVLBmp.Height);
                            Main_Gfx.DrawImage(NAME_OVLBmp, 0f, 0f, Main_BM.Width + OvlXoff, Main_BM.Height + OvlYoff);
                        }
                        else
                        {
                            Main_Gfx.DrawImage(NAME_OVLBmp, 0, 0, Main_BM.Width, Main_BM.Height);
                        }

                        break;
                    }
                }
        }

        public void GFX_DrawStats()
        {
            // R4.32 Adjust our working BMP if needed.
            STATS_AdjustSize();
            if ((Main_BM.Width != pbStats.Width) | (Main_BM.Height != pbStats.Height))
            {
                MainBuffer_Valid = false;
                Main_BM = new Bitmap(pbStats.Width, pbStats.Height);
                Main_Gfx = Graphics.FromImage(Main_BM);
            }

            Main_Gfx.Clear(LSName.BackC);

            // R4.50 Buffers are not valid, redraw ALL buffer images.
            if (MainBuffer_Valid == false)
            {
                MainBlur_Valid = false;
                MainBuffer1_Valid = false;
                MainBuffer2_Valid = false;
                MainBuffer3_Valid = false;
            }

            // R4.50 If we have stored versions skip the drawing code and show buffered versions.
            // R4.50 Are we using ELO cycle. If yes there are three modes Rank, Level, %.
            if (FLAG_EloUse == false)
            {
                if (MainBuffer1_Valid)
                {
                    Main_Gfx.DrawImage(MainBuffer1_BM, 0, 0);
                    pbStats.Image = Main_BM;
                    return;
                }
            }
            else
            {
                if (MainBuffer1_Valid & (FLAG_EloMode == 0))
                {
                    Main_Gfx.DrawImage(MainBuffer1_BM, 0, 0);
                    pbStats.Image = Main_BM;
                    return;
                }

                if (MainBuffer2_Valid & (FLAG_EloMode == 1))
                {
                    Main_Gfx.DrawImage(MainBuffer2_BM, 0, 0);
                    pbStats.Image = Main_BM;
                    return;
                }

                if (MainBuffer3_Valid & (FLAG_EloMode == 2))
                {
                    Main_Gfx.DrawImage(MainBuffer3_BM, 0, 0);
                    pbStats.Image = Main_BM;
                    return;
                }
            }

            // R4.50 If the current background image is valid use it and skip background drawing else build background image.
            if (MainBuffer_Valid)
                Main_Gfx.DrawImage(MainBuffer_BM, 0, 0);
            else
                GFX_BuildStatsBackground();

            GFX_BuildStatsForeground();


            // R4.50 Store the rendered STATS images so we can skip draws on the next Draw/ELO cycle.
            // R4.50 Are we using ELO cycle. If yes there are three modes Rank, Level, %.
            if (FLAG_EloUse == false)
            {
                if (MainBuffer1_Valid == false)
                {
                    MainBuffer1_BM = new Bitmap(pbStats.Width, pbStats.Height);
                    MainBuffer1_Gfx = Graphics.FromImage(MainBuffer1_BM);
                    MainBuffer1_Gfx.DrawImage(Main_BM, 0, 0);
                    MainBuffer1_Valid = true;
                }
            }
            else
            {
                if ((FLAG_EloMode == 0) & (MainBuffer1_Valid == false))
                {
                    MainBuffer1_BM = new Bitmap(pbStats.Width, pbStats.Height);
                    MainBuffer1_Gfx = Graphics.FromImage(MainBuffer1_BM);
                    MainBuffer1_Gfx.DrawImage(Main_BM, 0, 0);
                    MainBuffer1_Valid = true;
                }

                if ((FLAG_EloMode == 1) & (MainBuffer2_Valid == false))
                {
                    MainBuffer2_BM = new Bitmap(pbStats.Width, pbStats.Height);
                    MainBuffer2_Gfx = Graphics.FromImage(MainBuffer2_BM);
                    MainBuffer2_Gfx.DrawImage(Main_BM, 0, 0);
                    MainBuffer2_Valid = true;
                }

                if ((FLAG_EloMode == 2) & (MainBuffer3_Valid == false))
                {
                    MainBuffer3_BM = new Bitmap(pbStats.Width, pbStats.Height);
                    MainBuffer3_Gfx = Graphics.FromImage(MainBuffer3_BM);
                    MainBuffer3_Gfx.DrawImage(Main_BM, 0, 0);
                    MainBuffer3_Valid = true;
                }
            }

            // R4.30 Put the rendered bitmap into the STATS image.
            pbStats.Image = Main_BM;
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
            if (((3 < LSRank.BorderMode) | (3 < LSName.BorderMode)) & !MainBlur_Valid)
            {
                MainBlur_Valid = true;
                MainBlur_BM = new Bitmap(BM.Width, BM.Height);
                MainBlur_Gfx = Graphics.FromImage(MainBlur_BM);
                MainBlur_Gfx.Clear(Color.Black);
                var tBrush = new SolidBrush(Color.White);
                for (t = 1; t <= 8; t++)
                {
                    switch (LSRank.BorderMode)
                    {
                        case 4: // R4.50 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[t].X),
                                    (int) Math.Round(Yoff + LAB_Rank[t].Y), (int) Math.Round(LAB_Rank[t].Width),
                                    (int) Math.Round(LAB_Rank[t].Height)), tBrush, 4);
                            break;
                        }

                        case 5: // R4.50 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[t].X),
                                    (int) Math.Round(Yoff + LAB_Rank[t].Y), (int) Math.Round(LAB_Rank[t].Width),
                                    (int) Math.Round(LAB_Rank[t].Height)), tBrush, 8);
                            break;
                        }

                        case 6: // R4.50 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Rank[t].X),
                                    (int) Math.Round(Yoff + LAB_Rank[t].Y), (int) Math.Round(LAB_Rank[t].Width),
                                    (int) Math.Round(LAB_Rank[t].Height)), tBrush);
                            break;
                        }

                        default:
                        {
                            MainBlur_Gfx.FillRectangle(tBrush, Xoff + LAB_Rank[t].X, Yoff + LAB_Rank[t].Y,
                                LAB_Rank[t].Width, LAB_Rank[t].Height);
                            break;
                        }
                    }

                    switch (LSName.BorderMode)
                    {
                        case 4: // R4.50 Rounded Corners 4 pixels.
                        {
                            FillRoundedRectangle(MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[t].X),
                                    (int) Math.Round(Yoff + LAB_Name[t].Y), (int) Math.Round(LAB_Name[t].Width),
                                    (int) Math.Round(LAB_Name[t].Height)), tBrush, 4);
                            break;
                        }

                        case 5: // R4.50 Rounded Corners 8 pixels.
                        {
                            FillRoundedRectangle(MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[t].X),
                                    (int) Math.Round(Yoff + LAB_Name[t].Y), (int) Math.Round(LAB_Name[t].Width),
                                    (int) Math.Round(LAB_Name[t].Height)), tBrush, 8);
                            break;
                        }

                        case 6: // R4.50 Rounded Corners LARGE.
                        {
                            FillRoundedRectangle_Max(MainBlur_Gfx,
                                new Rectangle((int) Math.Round(Xoff + LAB_Name[t].X),
                                    (int) Math.Round(Yoff + LAB_Name[t].Y), (int) Math.Round(LAB_Name[t].Width),
                                    (int) Math.Round(LAB_Name[t].Height)), tBrush);
                            break;
                        }

                        default:
                        {
                            MainBlur_Gfx.FillRectangle(tBrush, Xoff + LAB_Name[t].X, Yoff + LAB_Name[t].Y,
                                LAB_Name[t].Width, LAB_Name[t].Height);
                            break;
                        }
                    }
                }
            }

            // R4.50 If doing borders we need to get to the locked data.
            if ((3 < LSRank.BorderMode) | (3 < LSName.BorderMode))
            {
                MainBlur_Data = MainBlur_BM.LockBits(new Rectangle(0, 0, BM.Width, BM.Height), ImageLockMode.ReadWrite,
                    BM.PixelFormat);
                ptrMask = MainBlur_Data.Scan0; // R4.50  Get the address of the first line.
                Marshal.Copy(ptrMask, rgbMask, 0, bytes);
            }


            // R3.40 Setup the BLUR BIAS value.
            BlurBias = 1 < Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeBias])
                ? (float) (1d +
                           Conversion.Val(Strings.Mid(
                               CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeBias], 1,
                               Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.LabelBlur,
                                   (int) clsGlobal.FXVarDefs.ShadeBias]) - 1)) * 0.01d)
                : 1f;

            if (BlurBias < 1f) BlurBias = 1f;

            if (1.1d < BlurBias) BlurBias = 1.1f;

            BlurBias = 1f + (BlurBias - 1f) * 4f; // R3.40 Reworked BLUR routines so Bias needs to be bigger.

            // R3.40 Setup the BLUR amount.
            Blur = 1 < Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeAmount])
                ? (float) (Conversion.Val(Strings.Mid(
                    CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeAmount], 1,
                    Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.LabelBlur, (int) clsGlobal.FXVarDefs.ShadeAmount]) -
                    1)) * 0.01d)
                : 0.4f;

            if (Blur == 0f) Blur = 0.8f;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);


            // R4.50 Blur the rectangles. This needs updated for new curved borders.
            for (t = 1; t <= 8; t++)
                if (!FLAG_HideMissing | !string.IsNullOrEmpty(PlrName[t]) | !string.IsNullOrEmpty(PlrSteam[t]))
                {
                    if (LSRank.BorderMode < 4)
                        FX_BlurRect(ref rgbValues, Stride, Blur, BlurBias, (int) Math.Round(Xoff + LAB_Rank[t].X),
                            (int) Math.Round(Yoff + LAB_Rank[t].Y), (int) Math.Round(LAB_Rank[t].Width),
                            (int) Math.Round(LAB_Rank[t].Height));
                    else
                        FX_BlurRect_Bordered(ref rgbValues, ref rgbMask, Stride, Blur, BlurBias,
                            (int) Math.Round(Xoff + LAB_Rank[t].X), (int) Math.Round(Yoff + LAB_Rank[t].Y),
                            (int) Math.Round(LAB_Rank[t].Width), (int) Math.Round(LAB_Rank[t].Height));

                    if (LSName.BorderMode < 4)
                        FX_BlurRect(ref rgbValues, Stride, Blur, BlurBias, (int) Math.Round(Xoff + LAB_Name[t].X),
                            (int) Math.Round(Yoff + LAB_Name[t].Y), (int) Math.Round(LAB_Name[t].Width),
                            (int) Math.Round(LAB_Name[t].Height));
                    else
                        FX_BlurRect_Bordered(ref rgbValues, ref rgbMask, Stride, Blur, BlurBias,
                            (int) Math.Round(Xoff + LAB_Name[t].X), (int) Math.Round(Yoff + LAB_Name[t].Y),
                            (int) Math.Round(LAB_Name[t].Width), (int) Math.Round(LAB_Name[t].Height));
                }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);
            if ((3 < LSRank.BorderMode) | (3 < LSName.BorderMode)) MainBlur_BM.UnlockBits(MainBlur_Data);

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
            tLabHgt = (int) ((long) Math.Round(LAB_Rank[1].Height) / 2L);

            // R4.32 Adjust our working BMP if needed.
            if ((Main_BM2.Width != pbStats.Width) | (Main_BM2.Height != pbStats.Height))
            {
                Main_BM2 = new Bitmap(pbStats.Width, pbStats.Height);
                Main_Gfx2 = Graphics.FromImage(Main_BM2);
            }

            Main_Gfx2.Clear(Color.FromArgb(0, 0, 0, 0));

            // *****************************************************************
            // R3.10 Draw the Faction images to the stats page.
            // ***************************************************************** 
            // R3.20 RemovedDim POP(0 To 8) As Integer
            // R3.20 Removed If 0 < GUI_Mouse_PlrIndex Then POP(GUI_Mouse_PlrIndex) = LAB_Fact(1).Height / 6

            for (var t = 1; t <= 8; t++)
                if (!string.IsNullOrEmpty(PlrFact[t]))
                {
                    tPic = (PictureBox) Controls["pbFact" + PlrFact[t]];
                    // R3.20 Removed Gfx2.DrawImage(tPic.Image, LAB_Fact(t).X - POP(t), LAB_Fact(t).Y - POP(t), LAB_Fact(t).Width + POP(t) * 2, LAB_Fact(t).Height + POP(t) * 2)
                    Main_Gfx2.DrawImage(tPic.Image, Xoff + LAB_Fact[t].X, Yoff + LAB_Fact[t].Y, LAB_Fact[t].Width,
                        LAB_Fact[t].Height);
                }

            // *****************************************************************
            // R3.00 Paint a blurred shadow.
            // *****************************************************************
            var tBrushShadow = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            for (var T = 1; T <= 8; T++)
            {
                if (FLAG_EloUse == false)
                {
                    tString = PlrRank[T];
                }
                else
                {
                    if (FLAG_EloMode == 0) tString = PlrRank[T];

                    if (FLAG_EloMode == 1) tString = PlrELO[T];

                    if (FLAG_EloMode == 2) tString = PlrLVL[T];
                }

                // R3.00 Create a clipping area so names do not draw past the rectangle.
                Main_Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X + 1f),
                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width - 2f),
                    (int) Math.Round(LAB_Rank[T].Height)));

                // R3.00 Draw the RANK Shadow text.
                Cx = (int) Math.Round(LAB_Rank[T].X + 2f + LAB_Rank[T].Width / 2f -
                                      Main_Gfx2.MeasureString(tString, FONT_Rank).Width / 2f);
                Cy = (int) Math.Round(LAB_Rank[T].Y + 2f + LAB_Rank[T].Height / 2f -
                                      Main_Gfx2.MeasureString(tString, FONT_Rank).Height / 2f);
                switch (CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] ?? "")
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

                if (CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] != "--")
                    Main_Gfx2.DrawString(tString, FONT_Rank, tBrushShadow, Xoff + Cx + Cx2, Yoff + Cy + Cy2);

                // R3.00 Clear the clipping area.
                Main_Gfx2.Clip = new Region(new Rectangle(0, 0, Main_BM2.Width, Main_BM2.Height));
            }

            TextHgt12 = (int) Math.Round(Main_Gfx2.MeasureString("X", FONT_Name).Height / 2f);
            for (var T = 1; T <= 8; T++)
            {
                // R3.00 Create a clipping area so names do not draw past the rectangle.
                Main_Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X + 1f),
                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width - 2f),
                    (int) Math.Round(LAB_Name[T].Height)));

                // R3.40 Adjust the text X position if RIGHT justified.
                if (LAB_Name_Align[T].Alignment == StringAlignment.Far)
                    LAB_Name[T].Xtext = LAB_Name[T].X + LAB_Name[T].Width;

                Cx = (int) Math.Round(Xoff + LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + LAB_Name[T].Y + tLabHgt - TextHgt12);
                switch (CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] ?? "")
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

                if (CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAng] != "--")
                {
                    if (chkCountry.Checked)
                    {
                        // R4.50 Normal X layout style.
                        if (Conversion.Val(cboLayoutX.Text) < 10d)
                        {
                            Main_Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx + 18 + Cx2, Cy + Cy2,
                                LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(PlrCountry[T]))
                                Main_Gfx2.FillRectangle(tBrushShadow, Cx + 4, Yoff + LAB_Name[T].Y + tLabHgt - 6f, 16f,
                                    11f);
                        }
                        // R4.50 Centered X layout style.
                        else if (Conversions.ToBoolean(T % 2))
                        {
                            // R4.46 ODD players.
                            Main_Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx - 19 + Cx2, Cy + Cy2,
                                LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(PlrCountry[T]))
                                Main_Gfx2.FillRectangle(tBrushShadow, Cx - 20 + Cx2,
                                    Yoff + LAB_Name[T].Y + tLabHgt - 6f + Cy2, 16f, 11f);
                        }
                        else
                        {
                            // R4.50 Even players.
                            Main_Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx + 19 + Cx2, Cy + Cy2,
                                LAB_Name_Align[T]);
                            if (!string.IsNullOrEmpty(PlrCountry[T]))
                                Main_Gfx2.FillRectangle(tBrushShadow, Cx + 4 + Cx2,
                                    Yoff + LAB_Name[T].Y + tLabHgt - 6f + Cy2, 16f, 11f);
                        }
                    }
                    else
                    {
                        Main_Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx + Cx2, Cy + Cy2,
                            LAB_Name_Align[T]);
                    }
                }

                // R3.00 Clear the clipping area.
                Main_Gfx2.Clip = new Region(new Rectangle(0, 0, Main_BM2.Width, Main_BM2.Height));
            }

            // Lock the bitmap's bits.
            var rect = new Rectangle(0, 0, Main_BM2.Width, Main_BM2.Height);
            var bmpData = Main_BM2.LockBits(rect, ImageLockMode.ReadWrite, Main_BM2.PixelFormat);
            var ptr = bmpData.Scan0; // R3.40 Get the address of the first byte.
            var Stride = bmpData.Stride;
            var bytes = Main_BM2.Width * Main_BM2.Height * 4; // R3.40 Declare an array to hold the bytes of the bitmap.
            var rgbValues = new byte[bytes]; // R3.40 This code is specific to a bitmap with 32 bits per pixels.
            int Ca = default, Ca2;
            float Blr1, Blr2;
            int TempI;

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            // R3.10 Calculate the Blur Bias value.
            float BlrFac;
            BlrFac = 1 < Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeBias])
                ? (float) (1d +
                           Conversion.Val(Strings.Mid(
                               CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeBias], 1,
                               Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Shadow,
                                   (int) clsGlobal.FXVarDefs.ShadeBias]) - 1)) * 0.01d)
                : 1f;

            if (BlrFac < 1f) BlrFac = 1f;

            if (1.1d < BlrFac) BlrFac = 1.1f;

            BlrFac = 1f + (BlrFac - 1f) * 16f;

            // R3.10 Calculate the Blur Amount.
            Blr1 = 1 < Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAmount])
                ? (float) (Conversion.Val(Strings.Mid(
                               CFX3DVar[(int) clsGlobal.FXMode.Shadow, (int) clsGlobal.FXVarDefs.ShadeAmount], 1,
                               Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Shadow,
                                   (int) clsGlobal.FXVarDefs.ShadeAmount]) - 1)) *
                           0.01d)
                : 0.5f;

            if (Blr1 == 0f) Blr1 = 0.8f;

            Blr2 = 1f - Blr1;

            // R3.00 Fill in the SHADOW color, and BLUR the alpha channel to make it bigger or smaller.
            for (int y = 0, loopTo = Main_BM2.Height - 1; y <= loopTo; y++)
            {
                Idx = y * Stride;
                for (int x = 0, loopTo1 = Main_BM2.Width - 1; x <= loopTo1; x++)
                {
                    rgbValues[Idx] = CFX3DC[1].B; // R3.30 Fill in SOLID color RGB.
                    rgbValues[Idx + 1] = CFX3DC[1].G;
                    rgbValues[Idx + 2] = CFX3DC[1].R;
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3]; // R3.30 We are only BLURRING alpha channel (Idx+3).
                    Idx += 4;
                }
            }

            TempI = (Main_BM2.Width - 2) * 4; // R3.40 Do some precalc here.   
            for (int y = 0, loopTo2 = Main_BM2.Height - 2; y <= loopTo2; y++)
            {
                Idx = y * Stride + TempI;
                for (var x = Main_BM2.Width - 2; x >= 2; x -= 1)
                {
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3];
                    Idx -= 4;
                }
            }

            // TempI = (2 * Stride)                       'R3.40 Do some precalc here.   
            for (int x = 2, loopTo3 = Main_BM2.Width - 2; x <= loopTo3; x++)
            {
                Idx = x * 4;
                for (int y = 0, loopTo4 = Main_BM2.Height - 2; y <= loopTo4; y++)
                {
                    rgbValues[Idx + 3] = (byte) Math.Round(Ca * Blr1 + rgbValues[Idx + 3] * Blr2);
                    Ca = rgbValues[Idx + 3];
                    Idx += Stride;
                }
            }

            TempI = (Main_BM2.Height - 2) * Stride; // R3.40 Do some precalc here.   
            for (int x = 2, loopTo5 = Main_BM2.Width - 2; x <= loopTo5; x++)
            {
                Idx = TempI + x * 4;
                for (var y = Main_BM2.Height - 2; y >= 2; y -= 1)
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
            Main_BM2.UnlockBits(bmpData);

            // R3.00 Draw the modified image.
            Gfx.DrawImage(Main_BM2, 0, 0);

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
            LabYCenter = (int) ((long) Math.Round(LAB_Rank[1].Height) / 2L);

            // *****************************************************************
            // R3.10 Draw the Faction images to the stats page.
            // ***************************************************************** 
            // Dim POP(0 To 8) As Integer
            // If 0 < GUI_Mouse_PlrIndex Then POP(GUI_Mouse_PlrIndex) = LAB_Fact(1).Height / 6

            for (var t = 1; t <= 8; t++)
                if (!string.IsNullOrEmpty(PlrFact[t]))
                {
                    tPic = (PictureBox) Controls["pbFact" + PlrFact[t]];
                    Gfx2.DrawImage(tPic.Image, Xoff + LAB_Fact[t].X, Yoff + LAB_Fact[t].Y, LAB_Fact[t].Width,
                        LAB_Fact[t].Height);
                }

            // *****************************************************************
            // R3.00 Print the RANK and NAME text on the blank bitmap.
            // *****************************************************************
            var tBrushShadow = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            for (var T = 1; T <= 8; T++)
            {
                if (FLAG_EloUse == false)
                {
                    tString = PlrRank[T];
                }
                else
                {
                    if (FLAG_EloMode == 0) tString = PlrRank[T];

                    if (FLAG_EloMode == 1) tString = PlrELO[T];

                    if (FLAG_EloMode == 2) tString = PlrLVL[T];
                }

                // R3.00 Create a clipping area so names do not draw past the rectangle/label.
                Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + LAB_Rank[T].X + 1f),
                    (int) Math.Round(Yoff + LAB_Rank[T].Y), (int) Math.Round(LAB_Rank[T].Width - 2f),
                    (int) Math.Round(LAB_Rank[T].Height)));

                // R3.00 Draw the RANK Shadow text.
                Cx = (int) Math.Round(Xoff + LAB_Rank[T].X + 2f + LAB_Rank[T].Width / 2f -
                                      Gfx2.MeasureString(tString, FONT_Rank).Width / 2f);
                Cy = (int) Math.Round(Yoff + LAB_Rank[T].Y + 2f + LAB_Rank[T].Height / 2f -
                                      Gfx2.MeasureString(tString, FONT_Rank).Height / 2f);
                Cx2 = 0;
                Cy2 = -2;
                Gfx2.DrawString(tString, FONT_Rank, tBrushShadow, Cx + Cx2, Cy + Cy2);

                // R3.00 Clear the clipping area.
                Gfx2.Clip = new Region(new Rectangle(0, 0, BM2.Width, BM2.Height));
            }

            TextHgt12 = (int) Math.Round(Gfx2.MeasureString("X", FONT_Name).Height / 2f); // R3.30 Was Xq.
            for (var T = 1; T <= 8; T++)
            {
                // R3.00 Create a clipping area so names do not draw past the rectangle.
                Gfx2.Clip = new Region(new Rectangle((int) Math.Round(Xoff + LAB_Name[T].X + 1f),
                    (int) Math.Round(Yoff + LAB_Name[T].Y), (int) Math.Round(LAB_Name[T].Width - 2f),
                    (int) Math.Round(LAB_Name[T].Height)));

                // R3.40 Adjust the text X position if RIGHT justified.
                if (LAB_Name_Align[T].Alignment == StringAlignment.Far)
                    LAB_Name[T].Xtext = LAB_Name[T].X + LAB_Name[T].Width;

                Cx = (int) Math.Round(Xoff + LAB_Name[T].Xtext);
                Cy = (int) Math.Round(Yoff + LAB_Name[T].Y + LabYCenter - TextHgt12);

                // R4.46 Added Country flags.
                if (chkCountry.Checked)
                {
                    // R4.50 Normal X layout style.
                    if (Conversion.Val(cboLayoutX.Text) < 10d)
                    {
                        Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx + 18, Cy, LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(PlrCountry[T]))
                            Gfx2.FillRectangle(tBrushShadow, Cx + 4, Yoff + LAB_Name[T].Y + LabYCenter - 6f, 16f, 11f);
                    }
                    // R4.46 Centered X layout style.
                    else if (Conversions.ToBoolean(T % 2))
                    {
                        // R4.50 ODD players.
                        Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx - 19, Cy, LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(PlrCountry[T]))
                            Gfx2.FillRectangle(tBrushShadow, Cx - 20, Yoff + LAB_Name[T].Y + LabYCenter - 6f, 16f, 11f);
                    }
                    else
                    {
                        // R4.50 Even players.
                        Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx + 19, Cy, LAB_Name_Align[T]);
                        if (!string.IsNullOrEmpty(PlrCountry[T]))
                            Gfx2.FillRectangle(tBrushShadow, Cx + 4, Yoff + LAB_Name[T].Y + LabYCenter - 6f, 16f, 11f);
                    }
                }
                else
                {
                    Gfx2.DrawString(PlrName[T], FONT_Name, tBrushShadow, Cx, Cy, LAB_Name_Align[T]);
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
            BlrBias = 1 < Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeBias])
                ? (float) (1d +
                           Conversion.Val(Strings.Mid(
                               CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeBias], 1,
                               Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Emboss,
                                   (int) clsGlobal.FXVarDefs.ShadeBias]) - 1)) * 0.01d)
                : 1f;

            if (BlrBias < 1f) BlrBias = 1f;

            if (1.5d < BlrBias) BlrBias = 1.5f;

            BlrBias = 1f + (BlrBias - 1f) * 16f;
            Blr1 = 1 < Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeAmount])
                ? (float) (Conversion.Val(Strings.Mid(
                               CFX3DVar[(int) clsGlobal.FXMode.Emboss, (int) clsGlobal.FXVarDefs.ShadeAmount], 1,
                               Strings.Len(CFX3DVar[(int) clsGlobal.FXMode.Emboss,
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
            if (pbStats.Width < Left) return;

            if (pbStats.Height < Top) return;

            if (Left < 0) Left = 0;

            if (Top < 0) Top = 0;

            if (pbStats.Width < Left + Width) Width = pbStats.Width - Left;

            if (pbStats.Height < Top + Height) Height = pbStats.Height - Top;

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
            if (pbStats.Width < Left) return;

            if (pbStats.Height < Top) return;

            if (Left < 0) Left = 0;

            if (Top < 0) Top = 0;

            if (pbStats.Width < Left + Width) Width = pbStats.Width - Left;

            if (pbStats.Height < Top + Height) Height = pbStats.Height - Top;


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

        private void cmTestData_Click(object sender, EventArgs e)
        {
            string A;
            A = "Selecting this button will place worst case scenario data on the stats page to test your setup." +
                Constants.vbCr + Constants.vbCr;
            A = A + "Steam names are usually limited to 32 characters." + Constants.vbCr;
            A = A + "Ranks are usually limited to 5 digits." + Constants.vbCr + Constants.vbCr;
            A = A + "Do you wish to continue?";
            if (Interaction.MsgBox(A,
                (MsgBoxStyle) ((int) MsgBoxStyle.Information + (int) MsgBoxStyle.DefaultButton1 +
                               (int) MsgBoxStyle.YesNo)) == MsgBoxResult.No) return;

            // R3.00 Create some worst case scenario data to show n user setup.
            for (var t = 1; t <= 8; t++)
            {
                PlrName[t] = "12345678901234567890123456789012";
                PlrRank[t] = "88888";
            }

            PlrFact[1] = "01";
            PlrFact[2] = "02";
            PlrFact[3] = "03";
            PlrFact[4] = "04";
            PlrFact[5] = "01";
            PlrFact[6] = "02";
            PlrFact[7] = "03";
            PlrFact[8] = "05";

            // R4.50 Force the STATS image redraw.
            MainBuffer_Valid = false;
            GFX_DrawStats();
        }

        private void pbStats_Click(object sender, EventArgs e)
        {
            // R3.30 This is only here to get us close when clicking controls in EDIT mode.
        }

        private void pbStats_MouseDown(object sender, MouseEventArgs e)
        {
            int T;
            var Hit = default(int);
            switch (FLAG_ShowPlayerCard)
            {
                case 0:
                {
                    FLAG_ShowPlayerCard = 1;
                    break;
                }

                case 1:
                {
                    FLAG_ShowPlayerCard = e.Button == MouseButtons.Left ? 0 : 2;

                    break;
                }

                case 2:
                {
                    FLAG_ShowPlayerCard = 0;
                    break;
                }
            }

            if (FLAG_ShowPlayerCard == 0)
            {
                if (FLAG_EloUse) timELOCycle.Enabled = true;

                GFX_DrawStats();
                return;
            }

            // R4.00 Find which player we have selected.
            if (FLAG_ShowPlayerCard == 1)
            {
                for (T = 1; T <= 8; T++)
                    if ((LAB_Name[T].Y < e.Y) & (e.Y < LAB_Name[T].Y + LAB_Name[T].Height))
                        if ((LAB_Name[T].X < e.X) & (e.X < LAB_Name[T].X + LAB_Name[T].Width))
                        {
                            Hit = T;
                            break;
                        }

                FLAG_ShowPlayerCardNum = Hit;

                // R4.00 We did not select a player
                if ((Hit == 0) | string.IsNullOrEmpty(PlrName[Hit]) | (PlrName[Hit] == "---"))
                {
                    FLAG_ShowPlayerCard = Conversions.ToInteger(false);
                    return;
                }
            }

            if (FLAG_EloUse) timELOCycle.Enabled = false;

            if (FLAG_ShowPlayerCard == 1)
            {
                GFX_DrawPlayerCard(Hit);
            }
            else
            {
                Hit = FLAG_ShowPlayerCardNum;
                scrStats.Maximum = TeamListCnt[Hit] * 90;
                T = (PlrTeam[Hit] - 1) * 90;
                if (T < 0) T = 0;

                if (scrStats.Maximum < T) T = scrStats.Maximum;

                scrStats.Value = T;
                GFX_DrawTeams(Hit);
            }
        }

        private void pbStats_MouseLeave(object sender, EventArgs e)
        {
            if (GUI_Active == false) return;

            // R3.00 We are not hovering over any players.
            GFX_DrawStats();
        }

        private void pbStats_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void cmLastMatch_Click(object sender, EventArgs e)
        {
            int t;
            for (t = 1; t <= 8; t++)
            {
                PlrName[t] = PlrName_Last[t];
                PlrRank[t] = PlrRank_Last[t];
                PlrFact[t] = PlrFact_Last[t];
                PlrTeam[t] = PlrTeam_Last[t];
                PlrTWin[t] = PlrTWin_Last[t];
                PlrTLoss[t] = PlrTLoss_Last[t];
                PlrSteam[t] = PlrSteam_Last[t];
                PlrRID[t] = PlrRID_Last[t];
                PlrCountry[t] = PlrCountry_Last[t]; // R4.45 Added.
                PlrCountryName[t] = PlrCountryName_Last[t]; // R4.46 Added.
                for (var T2 = 1; T2 <= 5; T2++)
                for (var T3 = 1; T3 <= 4; T3++)
                {
                    PlrRankALL[t, T2, T3] = PlrRankALL_Last[t, T2, T3];
                    PlrRankWin[t, T2, T3] = PlrRankWin_Last[t, T2, T3];
                    PlrRankLoss[t, T2, T3] = PlrRankLoss_Last[t, T2, T3];
                    PlrRankPerc[t, T2, T3] = PlrRankPerc_Last[t, T2, T3];
                }

                TeamListCnt[t] = TeamListCnt_Last[t];
                for (int T2 = 1, loopTo = TeamList.GetUpperBound(1); T2 <= loopTo; T2++)
                    TeamList[t, T2] = TeamList_Last[t, T2];

                PlrELO[t] = PlrELO_Last[t];
                PlrLVL[t] = PlrLVL_Last[t];
            }

            // R4.50 Force the STATS image redraw.
            MainBuffer_Valid = false;
            GFX_DrawStats();
        }

        private void cmFX3DC_Click(object sender, EventArgs e)
        {
            int N;
            if (FLAG_Loading) return;

            ColorDialog1.Color = CFX3DC[1];
            ColorDialog1.ShowDialog();
            N = cboFXVar1.SelectedIndex;
            if (0 < N) CFX3DC[N] = ColorDialog1.Color;

            GFX_DrawStats();
            Settings.SETTINGS_Save("");
        }

        private void cboFX3D_SelectedIndexChanged(object sender, EventArgs e)
        {
            int N;

            // R3.40 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            N = cboFXVar1.SelectedIndex;
            if (0 < N) CFX3DVar[N, 2] = cboFXVar2.Text;

            // R4.50 Force the BLUR mask to be redrawn.
            MainBlur_Valid = false;

            // R4.50 Force a clean STATS redraw.
            MainBuffer_Valid = false;
            GFX_DrawStats();
            Settings.SETTINGS_Save("");
            FLAG_Drawing = false;
        }

        private void cboFxVar3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int N;

            // R3.40 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            N = cboFXVar1.SelectedIndex;
            if (0 < N) CFX3DVar[N, 3] = cboFxVar3.Text;

            // R4.50 Force the BLUR mask to be redrawn.
            MainBlur_Valid = false;

            // R4.50 Force a clean STATS redraw.
            MainBuffer_Valid = false;
            GFX_DrawStats();
            Settings.SETTINGS_Save("");
            FLAG_Drawing = false;
        }

        private void cboFxVar4_SelectedIndexChanged(object sender, EventArgs e)
        {
            int N;

            // R3.40 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            N = cboFXVar1.SelectedIndex;
            if (0 < N) CFX3DVar[N, 4] = cboFxVar4.Text;

            // R4.50 Force the BLUR mask to be redrawn.
            MainBlur_Valid = false;

            // R4.50 Force a clean STATS redraw.
            MainBuffer_Valid = false;
            GFX_DrawStats();
            Settings.SETTINGS_Save("");
            FLAG_Drawing = false;
        }

        private void cboFXMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int N;
            if (FLAG_Loading) return;

            FLAG_Loading = true;

            // R3.20 Get the updated FX settings.
            N = cboFXVar1.SelectedIndex;
            if (0 < N)
            {
                cboFXVar2.Text = CFX3DVar[N, 2];
                cboFxVar3.Text = CFX3DVar[N, 3];
                cboFxVar4.Text = CFX3DVar[N, 4];
            }

            GFX_UpdateScreenControls();
            FLAG_Loading = false;
        }

        private void GFX_UpdateScreenControls()
        {
            int N;

            // R3.20 Dont save data as we are making screen changes only.
            FLAG_Loading = true;

            // R3.20 Get the selected FX index (1 - 10).
            N = cboFXVar1.SelectedIndex;
            if (0 < N)
            {
                chkFX.Checked = CFX3DActive[N];

                if (string.IsNullOrEmpty(CFX3DVar[N, 2])) CFX3DVar[N, 2] = Conversions.ToString(cboFXVar2.Items[1]);

                cboFXVar2.Text = CFX3DVar[N, 2];
                if (string.IsNullOrEmpty(CFX3DVar[N, 3])) CFX3DVar[N, 3] = Conversions.ToString(cboFxVar3.Items[1]);

                cboFxVar3.Text = CFX3DVar[N, 3];
                if (string.IsNullOrEmpty(CFX3DVar[N, 4])) CFX3DVar[N, 4] = Conversions.ToString(cboFxVar4.Items[1]);

                cboFxVar4.Text = CFX3DVar[N, 4];
            }
            else
            {
                // R3.20 We have no settings for this yet.
                chkFX.Checked = false;
                cboFXVar2.Text = "";
                cboFxVar3.Text = "";
                cboFxVar4.Text = "";
            }

            // R3.10 This should always be MODE.
            // lbFXVar1.Text = "Mode"

            // R3.10 Reset all FX controls to ON.
            cmFX3DC.Enabled = true;
            cboFXVar2.Enabled = true;
            cboFxVar3.Enabled = true;
            cboFxVar4.Enabled = true;

            // R3.10 Adjust screen controls to match  FX mode.
            switch (cboFXVar1.Text ?? "")
            {
                case "None":
                {
                    lbFXVar2.Text = "--";
                    lbFXVar3.Text = "--";
                    lbFXVar4.Text = "--";
                    cmFX3DC.Enabled = false;
                    cboFXVar2.Enabled = false;
                    cboFxVar3.Enabled = false;
                    cboFxVar4.Enabled = false;
                    chkFX.Enabled = false;
                    break;
                }

                case "Shadow":
                {
                    lbFXVar2.Text = "Color/Ang";
                    lbFXVar3.Text = "Blur Size";
                    lbFXVar4.Text = "Bias";
                    chkFX.Enabled = true;
                    break;
                }

                case "Emboss":
                {
                    lbFXVar2.Text = "--";
                    lbFXVar3.Text = "Blur Size";
                    lbFXVar4.Text = "Bias";
                    cmFX3DC.Enabled = false;
                    cboFXVar2.Enabled = false;
                    chkFX.Enabled = true;
                    break;
                }

                case "Lab Blur":
                {
                    lbFXVar2.Text = "--";
                    lbFXVar3.Text = "Blur Size";
                    lbFXVar4.Text = "Bias";
                    cmFX3DC.Enabled = false;
                    cboFXVar2.Enabled = false;
                    chkFX.Enabled = true;
                    break;
                }
            }

            // R3.20 Restore the OK to save settings flag.
            FLAG_Loading = false;
        }

        private void cmFXModeHelp_Click(object sender, EventArgs e)
        {
            var A = "";
            switch (cboFXVar1.Text ?? "")
            {
                case "None":
                {
                    A =
                        "No FX mode has been selected. FX setups allow for image based adjustments that may just add that cool touch to your stats text." +
                        Constants.vbCr + Constants.vbCr;
                    A += "Select an FX and then click the ACTIVE checkbox to add FX." + Constants.vbCr + Constants.vbCr;
                    A +=
                        "Each added FX slows the render time. This only happens when selected and updating the stats. While editing, the GUI may feel sluggish on slower PCs." +
                        Constants.vbCr + Constants.vbCr;
                    A += "Click refresh to update if things get out of sync while scrolling thru options.";
                    break;
                }

                case "Shadow":
                {
                    A =
                        "Shadow places a blurred shadow under all text. This helps text pop out more. Depending on the color and angle chosen, the shadow can be dark for a deep shadow or it can be bright giving the effect of light behind the text glowing." +
                        Constants.vbCr + Constants.vbCr;
                    A = A +
                        "Blur Size adjust how blurry the shadow is. BIAS adds some brightness and contrast to punch up a neutral shadow." +
                        Constants.vbCr + Constants.vbCr;
                    A += "Good start points are: BLUR SIZE (70%), BIAS(5.0%)";
                    break;
                }

                case "Emboss":
                {
                    A =
                        "Emboss tries to add some 3D depth to the text on screen.  Blurring the embossed image can make the embossing larger and smoother." +
                        Constants.vbCr + Constants.vbCr;
                    A +=
                        "Embossing multiplies a B/W image and the normal screen. This will always darken the image. Bias Is used to brighten up the darkened image." +
                        Constants.vbCr + Constants.vbCr;
                    A += "Using less Blur helps create a a stronger embossing effect." + Constants.vbCr +
                         Constants.vbCr;
                    A += "Good start points are: BLUR SIZE (50%), BIAS(5.0%)";
                    break;
                }

                case "Lab Blur":
                {
                    A = "The rectangles under the rank and player name will be blurred." + Constants.vbCr +
                        Constants.vbCr;
                    A += "This tries to make the text stand out from the background a little more." + Constants.vbCr +
                         Constants.vbCr;
                    A +=
                        "Doing this effect in an image processing progam and used as a background image could create a better image." +
                        Constants.vbCr + Constants.vbCr;
                    A += "Good start points are: BLUR SIZE (50%), BIAS(2.0%) and using low background opacity values.";
                    break;
                }
            }

            // R4.30 Updated. 
            Interaction.MsgBox(A, MsgBoxStyle.Information, "HELP - Advanced STATS effects");
        }

        private void chkFX_CheckedChanged(object sender, EventArgs e)
        {
            bool tState;
            int N;
            if (FLAG_Loading) return;

            tState = chkFX.Checked;

            N = cboFXVar1.SelectedIndex;
            CFX3DActive[N] = tState;

            // R4.50 Force the BLUR mask to be redrawn.
            MainBlur_Valid = false;

            // R4.50 Force a clean STATS redraw.
            MainBuffer_Valid = false;
            GFX_DrawStats();
            Settings.SETTINGS_Save("");
        }

        private void cmSave_Click(object sender, EventArgs e)
        {
            // R4.50 Save the current STATS image.
            var fd = new SaveFileDialog();

            // R4.50 Added to grab the current stats image (they cycle on a timer even while in dialog).
            var tmpImg = new Bitmap(pbStats.Image, pbStats.Width, pbStats.Height);
            fd.Title = "Save Stats Image";
            if (!string.IsNullOrEmpty(PATH_SaveStatsImage)) fd.InitialDirectory = PATH_SaveStatsImage;

            fd.Filter = "Png (*.png)|*.png|Gif (*.gif)|*.gif|Jpeg (*.jpg)|*.jpg";
            fd.FilterIndex = 3;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                // R3.30 Save the current stats image to selected filename.
                try
                {
                    tmpImg.Save(fd.FileName); // R4.50 Changed to this from pbStats.
                }
                catch
                {
                    Interaction.MsgBox(
                        "ERROR:" + Information.Err().Description + Constants.vbCr + Constants.vbCr +
                        "Unable to save the stats image.", Constants.vbCritical);
                }

                // R4.50 Save the directory we are using for image save dialog.
                PATH_SaveStatsImage = fd.FileName;
                PATH_SaveStatsImage = Utilities.PATH_StripFilename(PATH_SaveStatsImage);
            }
        }

        private void ToolTip_Setup()
        {
            string A;

            // ToolTip1.SetToolTip(pbStats, "Click a player name to see web pages for:" & vbCr & "Left: Relic Stats" & vbCr & "Ctrl-Left: Google Translate" & vbCr & "Shift-Left: Coh2.org player page" & vbCr & "Right: Coh2.org AT stats" & vbCr & "Ctrl-Right: Coh2.org faction page")
            ToolTip1.SetToolTip(pbStats,
                "Click a player name to see their player card. Right click again to see teams if option is enabled.");
            ToolTip1.SetToolTip(pbNote1,
                "NOTE #1: Used to display stream based information using animated text options.");
            ToolTip1.SetToolTip(pbNote2,
                "NOTE #2: Used to display stream based information using animated text options.");
            ToolTip1.SetToolTip(pbNote3,
                "NOTE #3: Used to display stream based information using animated text options.");
            ToolTip1.SetToolTip(pbNote4,
                "NOTE #4: Used to display stream based information using animated text options.");
            ToolTip1.SetToolTip(tbXsize, "Set the Width in pixels of the STATS image.");
            ToolTip1.SetToolTip(tbYSize,
                "Set the Height in pixels of the STATS image." + Constants.vbCr +
                "Best results if Y is divisible by 4.");
            ToolTip1.SetToolTip(tbXoff, "Adjust the stats value positions within the STATS image.");
            ToolTip1.SetToolTip(tbYoff, "Adjust the stats value positions within the STATS image.");
            ToolTip1.SetToolTip(cmDefaults, "Set all STATS size options to their default values.");
            ToolTip1.SetToolTip(cmStatsModeHelp, "Simple help info for the STATS setup options.");
            ToolTip1.SetToolTip(cmFindLog, "Locate the log file this program uses to show match stats.");
            ToolTip1.SetToolTip(cmCheckLog, "Force a one time only reading of the match stats log file.");
            ToolTip1.SetToolTip(cmScanLog,
                "Toggle ON or OFF a timer that automatically reads the log file periodically.");
            ToolTip1.SetToolTip(cboDelay, "Select how long to wait between LOG file scans.");
            ToolTip1.SetToolTip(cmRankSetup, "Setup the Celo RANK font and colors.");
            ToolTip1.SetToolTip(cmNameSetup, "Setup the Celo Player NAME font, colors, background and overlay image.");
            ToolTip1.SetToolTip(cmNote01Setup, "Setup the Note 1 font, colors, size, and background image.");
            ToolTip1.SetToolTip(cmNote02Setup, "Setup the Note 2 font, colors, size, and background image.");
            ToolTip1.SetToolTip(cmNote03Setup, "Setup the Note 3 font, colors, size, and background image.");
            ToolTip1.SetToolTip(cmNote04Setup, "Setup the Note 4 font, colors, size, and background image.");
            ToolTip1.SetToolTip(cmNote1, "Setup the Note 1 text and animation mode.");
            ToolTip1.SetToolTip(cmNote2, "Setup the Note 2 text and animation mode.");
            ToolTip1.SetToolTip(cmNote3, "Setup the Note 3 text and animation mode.");
            ToolTip1.SetToolTip(cmNote4, "Setup the Note 4 text and animation mode.");
            ToolTip1.SetToolTip(cmSetupLoad, "Load a new presentation setup from a stored file.");
            ToolTip1.SetToolTip(cmSetupSave, "Save the current presentation setup to a stored file.");
            ToolTip1.SetToolTip(cmNote_PlayAll, "Toggle on/off all Animation play buttons.");
            ToolTip1.SetToolTip(cmNote01_Play, "Toggle on/off note 1 Animations.");
            ToolTip1.SetToolTip(cmNote02_Play, "Toggle on/off note 2 Animations.");
            ToolTip1.SetToolTip(cmNote03_Play, "Toggle on/off note 3 Animations.");
            ToolTip1.SetToolTip(cmNote04_Play, "Toggle on/off note 4 Animations.");
            A = "Left click to play a sound." + Constants.vbCr + "Right click to map a sound or set the volume." +
                Constants.vbCr + "Ctrl-Click to delete.";
            ToolTip1.SetToolTip(cmSound01, A);
            ToolTip1.SetToolTip(cmSound02, A);
            ToolTip1.SetToolTip(cmSound03, A);
            ToolTip1.SetToolTip(cmSound04, A);
            ToolTip1.SetToolTip(cmSound05, A);
            ToolTip1.SetToolTip(cmSound06, A);
            ToolTip1.SetToolTip(cmSound07, A);
            ToolTip1.SetToolTip(cmSound08, A);
            ToolTip1.SetToolTip(cmSound09, A);
            ToolTip1.SetToolTip(cmSound10, A);
            ToolTip1.SetToolTip(cmSound11, A);
            ToolTip1.SetToolTip(cmSound12, A);
            ToolTip1.SetToolTip(cmSound13, A);
            ToolTip1.SetToolTip(cmSound14, A);
            ToolTip1.SetToolTip(cmSound15, A);

            // R4.30 Updated.
            ToolTip1.SetToolTip(scrVolume,
                "The master sound volume for this program. Right click pads to adjust individual sounds.");
            ToolTip1.SetToolTip(cmAudioStop, "STOP playing the current sound.");
            ToolTip1.SetToolTip(cmCopy, "Copy the stats page to the clipboard to be pasted in another program.");
            ToolTip1.SetToolTip(cmSave,
                "Save a copy of the stats page to the PC. Good to use as a template in custom backgrounds.");

            // ToolTip1.SetToolTip(cboPageSize, "Set the stats page overall size.")
            ToolTip1.SetToolTip(cboLayoutY, "Set the Y axis orientation of the printed stats.");
            ToolTip1.SetToolTip(cboLayoutX, "Set the X axis orientation of the printed stats.");
            ToolTip1.SetToolTip(cboNoteSpace,
                "Set a border space around NOTES. Set to 0 to use them as a single graphic.");
            ToolTip1.SetToolTip(cmSizeRefresh,
                "Force a redraw of the stats page. Useful when changing sizes or editing a page using a lot of FX.");
            // ToolTip1.SetToolTip(cmGUILite, "Set the program color scheme to Light. Helps when cropping in your streaming app.")
            // ToolTip1.SetToolTip(cmGUIDark, "Set the program color scheme to Dark. Helps when cropping in your streaming app.")

            ToolTip1.SetToolTip(cboFXVar1, "Select a stats page FX item to edit or make active.");
            ToolTip1.SetToolTip(chkFX, "Toggles the currently selected FX mode to ON or OFF.");
            ToolTip1.SetToolTip(cmFX3DC, "Select a color to be used in the selected FX.");
            ToolTip1.SetToolTip(cboFXVar2, "Select an ANGLE to be used in the selected FX.");
            ToolTip1.SetToolTip(cboFxVar3, "Adjusts an FX setting for the selected FX.");
            ToolTip1.SetToolTip(cboFxVar4, "Adjusts an FX setting for the selected FX.");
            ToolTip1.SetToolTip(cmFXModeHelp, "Get FX mode specific help.");
            ToolTip1.SetToolTip(chkPopUp,
                "Toggle the context popup menu on the stats page that shows additional player info on the web.");
            ToolTip1.SetToolTip(chkPosition, "Store the current window size and position on the screen.");
            ToolTip1.SetToolTip(chkSmoothAni, "Try to smooth animations by redrawing whole window (10x CPU usage).");
            ToolTip1.SetToolTip(chkGUIMode,
                "Set the program color scheme to Light/Dark. Helps when cropping in your streaming app.");
            ToolTip1.SetToolTip(chkFoundSound, "Play sound pad #15 (Alert) when AUTO search finds a match.");
            ToolTip1.SetToolTip(chkHideMissing,
                "Do not display blank player information. Use if playing many game modes to hide extra graphics.");
            ToolTip1.SetToolTip(chkShowELO, "If the rank ELO vals are setup, try to show approx ELO as rank.");
            ToolTip1.SetToolTip(chkSpeech, "Use Windows Speech to read ranks aloud at match start.");
            ToolTip1.SetToolTip(chkGetTeams,
                "Use additional web calls to search for possible team ranks. Ranks ending in period are teams.");
            ToolTip1.SetToolTip(cmLastMatch, "Display the last valid match found since this program has been running.");
            ToolTip1.SetToolTip(cmTestData,
                "Test your current setup by filling the stats page" + Constants.vbCr +
                "with the largest values you may see in a COH2 match.");
            ToolTip1.SetToolTip(cmErrLog,
                "Display the web data log transactions. Useful for troubleshooting problems/crashes.");
        }

        private void chkTips_CheckedChanged(object sender, EventArgs e)
        {
            ToolTip1.Active = chkTips.Checked;
        }

        private void NOTE_Animation_Setup(ref clsGlobal.t_NoteAnimation NoteAnim, ref PictureBox pbNote, ref Font tFont,
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

        private void timNote1_Tick(object sender, EventArgs e)
        {
            // R4.10 Tried STOPWATCH to get smoother renders but results were worse.

            // R4.10 We can get slightly smoother animations using INVALIDATE and this sub to update the screen.
            // R4.10 But there is a CPU cost (about 10x).
            if (ANIMATION_Smooth)
            {
                Invalidate();
            }
            else
            {
                if ((cmNote01_Play.Text == "||") & NoteAnim01.Active)
                {
                    var argpbNote = pbNote1;
                    NOTE_Animate(ref argpbNote, ref Note01_Text, ref LSNote01, ref FONT_Note01, ref NoteAnim01,
                        ref Note01_Bmp, ref Note01_Gfx, ref Note01_BackBmp, ref Note01_OVLBmp);
                    pbNote1 = argpbNote;
                }

                if ((cmNote02_Play.Text == "||") & NoteAnim02.Active)
                {
                    var argpbNote1 = pbNote2;
                    NOTE_Animate(ref argpbNote1, ref Note02_Text, ref LSNote02, ref FONT_Note02, ref NoteAnim02,
                        ref Note02_Bmp, ref Note02_Gfx, ref Note02_BackBmp, ref Note02_OVLBmp);
                    pbNote2 = argpbNote1;
                }

                if ((cmNote03_Play.Text == "||") & NoteAnim03.Active)
                {
                    var argpbNote2 = pbNote3;
                    NOTE_Animate(ref argpbNote2, ref Note03_Text, ref LSNote03, ref FONT_Note03, ref NoteAnim03,
                        ref Note03_Bmp, ref Note03_Gfx, ref Note03_BackBmp, ref Note03_OVLBmp);
                    pbNote3 = argpbNote2;
                }

                if ((cmNote04_Play.Text == "||") & NoteAnim04.Active)
                {
                    var argpbNote3 = pbNote4;
                    NOTE_Animate(ref argpbNote3, ref Note04_Text, ref LSNote04, ref FONT_Note04, ref NoteAnim04,
                        ref Note04_Bmp, ref Note04_Gfx, ref Note04_BackBmp, ref Note04_OVLBmp);
                    pbNote4 = argpbNote3;
                }
            }
        }

        private void NOTE_Animate(ref PictureBox pbNote, ref string[] Note, ref clsGlobal.t_LabelSetup LSNote,
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

        public void SOUND_Play(string tFile)
        {
            try
            {
                _soundPlayer.Stop();
                _soundPlayer.SoundLocation = tFile;
                _soundPlayer.Play();
            }
            catch
            {
                lbError1.Text = "Error playing";
                lbError2.Text = "sound file";
                Interaction.MsgBox(
                    "ERROR: " + Information.Err().Description + Constants.vbCr + Constants.vbCr +
                    "Unable to play this sound file.", MsgBoxStyle.Critical);
            } // R4.10 Updated.
        }

        private void SOUND_HandleClicks(object Sender, MouseEventArgs e, int Index)
        {
            // R4.10 Store which object we are working with so we can set tooltip in other subs.
            CELO_PopUpObject = Sender;
            Celo_PopupHit = Index;
            if (0 < (int) (ModifierKeys & Keys.Control))
            {
                SOUND_File[Index] = "";
                ToolTip1.SetToolTip((Control) Sender,
                    "Left click to play a sound." + Constants.vbCr + "Right click to map a sound or set the volume." +
                    Constants.vbCr + "Ctrl-Click to delete.");
            }
            // If ToolTip1.Active = False Then 

            else if ((e.Button == MouseButtons.Left) & !string.IsNullOrEmpty(SOUND_File[Index]))
            {
                AUDIO_SetVolume(scrVolume.Value, Conversions.ToInteger(SOUND_Vol[Index]));
                SOUND_Play(SOUND_File[Index]);
            }
            else if (!string.IsNullOrEmpty(SOUND_File[Index]))
            {
                POPUP_Audio_SetMenu(Conversions.ToInteger(SOUND_Vol[Index]));
                tsmAudio.Show((Control) Sender, new Point(e.X, e.Y));
            }
            else
            {
                AUDIO_SelectFile(Index);
            }
        }

        private void POPUP_Audio_SetMenu(int Vol)
        {
            tsmSetVolTo100.Checked = false;
            if (Vol == 100) tsmSetVolTo100.Checked = true;

            tsmSetVolTo90.Checked = false;
            if (Vol == 90) tsmSetVolTo90.Checked = true;

            tsmSetVolTo80.Checked = false;
            if (Vol == 80) tsmSetVolTo80.Checked = true;

            tsmSetVolTo70.Checked = false;
            if (Vol == 70) tsmSetVolTo70.Checked = true;

            tsmSetVolTo60.Checked = false;
            if (Vol == 60) tsmSetVolTo60.Checked = true;

            tsmSetVolTo50.Checked = false;
            if (Vol == 50) tsmSetVolTo50.Checked = true;

            tsmSetVolTo40.Checked = false;
            if (Vol == 40) tsmSetVolTo40.Checked = true;

            tsmSetVolTo30.Checked = false;
            if (Vol == 30) tsmSetVolTo30.Checked = true;

            tsmSetVolTo20.Checked = false;
            if (Vol == 20) tsmSetVolTo20.Checked = true;

            tsmSetVolTo10.Checked = false;
            if (Vol == 10) tsmSetVolTo10.Checked = true;
        }

        private string SOUND_GetName(string tFile)
        {
            int i;
            int Hit;
            string S;
            Hit = 0;
            for (i = Strings.Len(tFile); i >= 1; i -= 1)
                if (Strings.Mid(tFile, i, 1) == @"\")
                {
                    Hit = i;
                    break;
                }

            S = Conversions.ToBoolean(Hit) ? Strings.Mid(tFile, Hit + 1, 255) : tFile;

            return Strings.LCase(S);
        }

        private void cmAudioStop_Click(object sender, EventArgs e)
        {
            // R4.34 Stop ALL audio being played. Sound Pads and Rank reader.
            _soundPlayer.Stop();
            if (FLAG_SpeechOK) SpeechSynth.SpeakAsyncCancelAll();
        }

        private void cmSound01_Click(object sender, EventArgs e)
        {
            // R4.00 Not used
        }

        private void cmSound01_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 1);
        }

        private void cmSound02_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 2);
        }

        private void cmSound03_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 3);
        }

        private void cmSound04_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 4);
        }

        private void cmSound05_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 5);
        }

        private void cmSound06_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 6);
        }

        private void cmSound07_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 7);
        }

        private void cmSound08_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 8);
        }

        private void cmSound09_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 9);
        }

        private void cmSound10_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 10);
        }

        private void cmSound11_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 11);
        }

        private void cmSound12_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 12);
        }

        private void cmSound13_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 13);
        }

        private void cmSound14_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 14);
        }

        private void cmSound15_MouseDown(object sender, MouseEventArgs e)
        {
            SOUND_HandleClicks(sender, e, 15);
        }

        private void cmSound01_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[1]);
        }

        private void cmSound02_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[2]);
        }

        private void cmSound03_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[3]);
        }

        private void cmSound04_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[4]);
        }

        private void cmSound05_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[5]);
        }

        private void cmSound06_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[6]);
        }

        private void cmSound07_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[7]);
        }

        private void cmSound08_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[8]);
        }

        private void cmSound09_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[9]);
        }

        private void cmSound10_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[10]);
        }

        private void cmSound11_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[11]);
        }

        private void cmSound12_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[12]);
        }

        private void cmSound13_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[13]);
        }

        private void cmSound14_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[14]);
        }

        private void cmSound15_MouseHover(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = SOUND_GetName(SOUND_File[15]);
        }

        private void cmSound01_MouseLeave(object sender, EventArgs e)
        {
            lbSoundCurrent.Text = "";
        }

        private void SCREEN_Organize()
        {
            // R4.00 Adjust pictures Y positions based on their size.
            // R4.00 NOTE_Spacing lets you keep Notes together if wanted.
            pbNote1.Top = pbStats.Top + pbStats.Height + 2;
            pbNote2.Top = pbNote1.Top + pbNote1.Height + NOTE_Spacing;
            pbNote3.Top = pbNote2.Top + pbNote2.Height + NOTE_Spacing;
            pbNote4.Top = pbNote3.Top + pbNote3.Height + NOTE_Spacing;
        }

        private void cmRankSetup_Click(object sender, EventArgs e)
        {
            var LBDialog = new frmLabelSetup(chkTips.Checked) {HideSizeOptions = true, HideSizeAll = true};

            // R4.00 Get the data we are editing.
            LSRank.BackC = LSName.BackC; // R4.00 Name has backround info that is used.
            LSRank.Scaling = LSName.Scaling; // R4.10 Name has scaling info.
            LSRank.OVLScaling = LSName.OVLScaling; // R4.10 Name has scaling info.
            LBDialog.LSetup = LSRank;
            FONT_Setup = FONT_Rank;
            PATH_DlgBmp = PATH_BackgroundImage; // R4.00 Path for back image.
            Note_BackBmp = NAME_bmp; // R4.00 Back Image.
            PATH_DlgBmpPath = Utilities.PATH_StripFilename(PATH_DlgBmp);
            PATH_DlgOVLBmp = PATH_Name_OVLBmp; // R4.00 Path for back image.
            Note_OVLBmp = NAME_OVLBmp; // R4.00 Back Image.
            PATH_DlgOVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            LBDialog.ShowDialog();
            if (LBDialog.Cancel == false)
            {
                LSRank = LBDialog.LSetup;
                FONT_Rank = FONT_Setup;
                FONT_Rank_Name = FONT_Setup.Name;
                FONT_Rank_Size = FONT_Setup.Size.ToString();
                FONT_Rank_Bold = Conversions.ToString(FONT_Setup.Bold);
                FONT_Rank_Italic = Conversions.ToString(FONT_Setup.Italic);
                LSRank.B1 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSRank.O1) * 0.01d)), LSRank.B1.R,
                    LSRank.B1.G, LSRank.B1.B);
                LSRank.B2 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSRank.O2) * 0.01d)), LSRank.B2.R,
                    LSRank.B2.G, LSRank.B2.B);
                NAME_bmp = Note_BackBmp;
                PATH_BackgroundImage = PATH_DlgBmp;
                NAME_OVLBmp = Note_OVLBmp;
                PATH_Name_OVLBmp = PATH_DlgOVLBmp;
                PATH_Name_OVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);
                pbStats.BackColor = LSRank.BackC;
                LSName.BackC = LSRank.BackC;

                // R4.10 We use the NAME info when drawing.
                LSName.Scaling = LSRank.Scaling;
                LSName.OVLScaling = LSRank.OVLScaling;

                // R4.40 Name and Rank CARD BACK must be the same.
                LSName.UseCardBack = LSRank.UseCardBack;
                LSName.BorderPanelMode = LSRank.BorderPanelMode;
                LSName.BorderPanelColor = LSRank.BorderPanelColor;
                LSName.BorderPanelWidth = LSRank.BorderPanelWidth;

                // R4.50 Force the BLUR mask to be redrawn.
                MainBlur_Valid = false;

                // R4.50 Force a clean STATS redraw.
                MainBuffer_Valid = false;
            }

            LBDialog.Dispose();
            Settings.SETTINGS_Save("");
            SCREEN_Organize();
            GFX_DrawStats();
        }

        private void cmNameSetup_Click(object sender, EventArgs e)
        {
            var LBDialog = new frmLabelSetup(chkTips.Checked)
            {
                HideSizeOptions = true,
                HideSizeAll = true,
                LSetup = LSName
            };

            // R4.00 Get the data we are editing.
            FONT_Setup = FONT_Name;
            PATH_DlgBmp = PATH_BackgroundImage; // R4.00 Path for back image.
            Note_BackBmp = NAME_bmp; // R4.00 Back Image.
            PATH_DlgBmpPath = Utilities.PATH_StripFilename(PATH_DlgBmp);
            PATH_DlgOVLBmp = PATH_Name_OVLBmp; // R4.00 Path for back image.
            Note_OVLBmp = NAME_OVLBmp; // R4.00 Back Image.
            PATH_DlgOVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            LBDialog.ShowDialog();
            if (LBDialog.Cancel == false)
            {
                LSName = LBDialog.LSetup;
                FONT_Name = FONT_Setup;
                FONT_Name_Name = FONT_Setup.Name;
                FONT_Name_Size = FONT_Setup.Size.ToString();
                FONT_Name_Bold = Conversions.ToString(FONT_Setup.Bold);
                FONT_Name_Italic = Conversions.ToString(FONT_Setup.Italic);
                LSName.B1 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSName.O1) * 0.01d)), LSName.B1.R,
                    LSName.B1.G, LSName.B1.B);
                LSName.B2 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSName.O2) * 0.01d)), LSName.B2.R,
                    LSName.B2.G, LSName.B2.B);
                NAME_bmp = Note_BackBmp;
                PATH_BackgroundImage = PATH_DlgBmp;
                NAME_OVLBmp = Note_OVLBmp;
                PATH_Name_OVLBmp = PATH_DlgOVLBmp;
                PATH_Name_OVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);
                pbStats.BackColor = LSName.BackC;

                // R4.40 Name and Rank CARD BACK must be the same.
                LSRank.UseCardBack = LSName.UseCardBack;
                LSRank.BorderPanelMode = LSName.BorderPanelMode;
                LSRank.BorderPanelColor = LSName.BorderPanelColor;
                LSRank.BorderPanelWidth = LSName.BorderPanelWidth;

                // R4.50 Force the BLUR mask to be redrawn.
                MainBlur_Valid = false;

                // R4.50 Force a clean STATS redraw.
                MainBuffer_Valid = false;
            }

            LBDialog.Dispose();
            Settings.SETTINGS_Save("");
            SCREEN_Organize();
            GFX_DrawStats();
        }

        private void cmNote_PlayAll_Click(object sender, EventArgs e)
        {
            // R4.00 Turn ON/OFF all NOTE animations.
            if (cmNote_PlayAll.Text == ">")
            {
                cmNote01Setup.Enabled = false;
                cmNote02Setup.Enabled = false;
                cmNote03Setup.Enabled = false;
                cmNote04Setup.Enabled = false;
                cmNote_PlayAll.Text = "||";
                cmNote01_Play.Text = "||";
                cmNote02_Play.Text = "||";
                cmNote03_Play.Text = "||";
                cmNote04_Play.Text = "||";
                var argpbNote = pbNote1;
                NOTE_Animation_Setup(ref NoteAnim01, ref argpbNote, ref FONT_Note01, ref Note01_Text,
                    ref NoteAnim01_Text);
                pbNote1 = argpbNote;
                var argpbNote1 = pbNote2;
                NOTE_Animation_Setup(ref NoteAnim02, ref argpbNote1, ref FONT_Note02, ref Note02_Text,
                    ref NoteAnim02_Text);
                pbNote2 = argpbNote1;
                var argpbNote2 = pbNote3;
                NOTE_Animation_Setup(ref NoteAnim03, ref argpbNote2, ref FONT_Note03, ref Note03_Text,
                    ref NoteAnim03_Text);
                pbNote3 = argpbNote2;
                var argpbNote3 = pbNote4;
                NOTE_Animation_Setup(ref NoteAnim04, ref argpbNote3, ref FONT_Note04, ref Note04_Text,
                    ref NoteAnim04_Text);
                pbNote4 = argpbNote3;
            }
            else
            {
                cmNote01Setup.Enabled = true;
                cmNote02Setup.Enabled = true;
                cmNote03Setup.Enabled = true;
                cmNote04Setup.Enabled = true;
                cmNote_PlayAll.Text = ">";
                cmNote01_Play.Text = ">";
                cmNote02_Play.Text = ">";
                cmNote03_Play.Text = ">";
                cmNote04_Play.Text = ">";
            }
        }

        private void cmNote01_Play_Click(object sender, EventArgs e)
        {
            if (cmNote01_Play.Text == ">")
            {
                cmNote01Setup.Enabled = false;
                cmNote01_Play.Text = "||";
                var argpbNote = pbNote1;
                NOTE_Animation_Setup(ref NoteAnim01, ref argpbNote, ref FONT_Note01, ref Note01_Text,
                    ref NoteAnim01_Text);
                pbNote1 = argpbNote;
            }
            else
            {
                cmNote01Setup.Enabled = true;
                cmNote01_Play.Text = ">";
            }
        }

        private void cmNote02_Play_Click(object sender, EventArgs e)
        {
            if (cmNote02_Play.Text == ">")
            {
                cmNote02Setup.Enabled = false;
                cmNote02_Play.Text = "||";
                var argpbNote = pbNote2;
                NOTE_Animation_Setup(ref NoteAnim02, ref argpbNote, ref FONT_Note02, ref Note02_Text,
                    ref NoteAnim02_Text);
                pbNote2 = argpbNote;
            }
            else
            {
                cmNote02Setup.Enabled = true;
                cmNote02_Play.Text = ">";
            }
        }

        private void cmNote03_Play_Click(object sender, EventArgs e)
        {
            if (cmNote03_Play.Text == ">")
            {
                cmNote03Setup.Enabled = false;
                cmNote03_Play.Text = "||";
                var argpbNote = pbNote3;
                NOTE_Animation_Setup(ref NoteAnim03, ref argpbNote, ref FONT_Note03, ref Note03_Text,
                    ref NoteAnim03_Text);
                pbNote3 = argpbNote;
            }
            else
            {
                cmNote03Setup.Enabled = true;
                cmNote03_Play.Text = ">";
            }
        }

        private void cmNote04_Play_Click(object sender, EventArgs e)
        {
            if (cmNote04_Play.Text == ">")
            {
                cmNote04Setup.Enabled = false;
                cmNote04_Play.Text = "||";
                var argpbNote = pbNote4;
                NOTE_Animation_Setup(ref NoteAnim04, ref argpbNote, ref FONT_Note04, ref Note04_Text,
                    ref NoteAnim04_Text);
                pbNote4 = argpbNote;
            }
            else
            {
                cmNote04Setup.Enabled = true;
                cmNote04_Play.Text = ">";
            }
        }

        private void NOTE_AdjustBitmap(ref PictureBox pbNote, ref clsGlobal.t_LabelSetup LSNote, ref Bitmap BM,
            ref Graphics Gfx)
        {
            pbNote.Width = LSNote.Width;
            pbNote.Height = LSNote.Height;
            BM = new Bitmap(pbNote.Width, pbNote.Height);
            Gfx = Graphics.FromImage(BM);
        }

        private void NOTE_CheckNoteSizes()
        {
            // R4.00 If the NOTE size was changed, update the picture box size.
            if ((LSNote01.Width != pbNote1.Width) | (LSNote01.Height != pbNote1.Height))
            {
                var argpbNote = pbNote1;
                NOTE_AdjustBitmap(ref argpbNote, ref LSNote01, ref Note01_Bmp, ref Note01_Gfx);
                pbNote1 = argpbNote;
            }

            if ((LSNote02.Width != pbNote2.Width) | (LSNote02.Height != pbNote2.Height))
            {
                var argpbNote1 = pbNote2;
                NOTE_AdjustBitmap(ref argpbNote1, ref LSNote02, ref Note02_Bmp, ref Note02_Gfx);
                pbNote2 = argpbNote1;
            }

            if ((LSNote03.Width != pbNote3.Width) | (LSNote03.Height != pbNote3.Height))
            {
                var argpbNote2 = pbNote3;
                NOTE_AdjustBitmap(ref argpbNote2, ref LSNote03, ref Note03_Bmp, ref Note03_Gfx);
                pbNote3 = argpbNote2;
            }

            if ((LSNote04.Width != pbNote4.Width) | (LSNote04.Height != pbNote4.Height))
            {
                var argpbNote3 = pbNote4;
                NOTE_AdjustBitmap(ref argpbNote3, ref LSNote04, ref Note04_Bmp, ref Note04_Gfx);
                pbNote4 = argpbNote3;
            }

            SCREEN_Organize();
        }

        private void cmNote01Setup_Click_1(object sender, EventArgs e)
        {
            var LBDialog = new frmLabelSetup(chkTips.Checked)
            {
                HideFormColor = true,
                LSetup = LSNote01
            };

            // R4.00 Get the data we are editing.
            FONT_Setup = FONT_Note01;
            PATH_DlgBmp = PATH_Note01_Bmp; // R4.00 Path for back image.
            Note_BackBmp = Note01_BackBmp; // R4.00 Back Image.
            PATH_DlgBmpPath = Utilities.PATH_StripFilename(PATH_DlgBmp);
            PATH_DlgOVLBmp = PATH_Note01_OVLBmp; // R4.00 Path for back image.
            Note_OVLBmp = Note01_OVLBmp; // R4.00 Back Image.
            PATH_DlgOVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            LBDialog.ShowDialog();
            if (LBDialog.Cancel == false)
            {
                LSNote01 = LBDialog.LSetup;
                PATH_Note01_Bmp = PATH_DlgBmp; // R4.00 Path for back image.
                Note01_BackBmp = Note_BackBmp; // R4.00 Back Image.
                PATH_Note01_OVLBmp = PATH_DlgOVLBmp; // R4.00 Path for back image.
                Note01_OVLBmp = Note_OVLBmp; // R4.00 Overlay Image.
                FONT_Note01 = FONT_Setup;
                FONT_Note01_Name = FONT_Setup.Name;
                FONT_Note01_Size = FONT_Setup.Size.ToString();
                FONT_Note01_Bold = Conversions.ToString(FONT_Setup.Bold);
                FONT_Note01_Italic = Conversions.ToString(FONT_Setup.Italic);
                LSNote01.B1 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote01.O1) * 0.01d)),
                    LSNote01.B1.R, LSNote01.B1.G, LSNote01.B1.B);
                LSNote01.B2 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote01.O2) * 0.01d)),
                    LSNote01.B2.R, LSNote01.B2.G, LSNote01.B2.B);
                NOTE_CheckNoteSizes();
                Settings.SETTINGS_Save("");
            }

            LBDialog.Dispose();
        }

        private void cmNote02Setup_Click_1(object sender, EventArgs e)
        {
            var LBDialog = new frmLabelSetup(chkTips.Checked)
            {
                HideFormColor = true,
                LSetup = LSNote02
            };

            // R4.00 Get the data we are editing.
            FONT_Setup = FONT_Note02;
            PATH_DlgBmp = PATH_Note02_Bmp; // R4.00 Path for back image.
            Note_BackBmp = Note02_BackBmp; // R4.00 Back Image.
            PATH_DlgBmpPath = Utilities.PATH_StripFilename(PATH_DlgBmp); // R4.00 Path without Filename. 
            PATH_DlgOVLBmp = PATH_Note02_OVLBmp; // R4.00 Path for back image.
            Note_OVLBmp = Note02_OVLBmp; // R4.00 Back Image.
            PATH_DlgOVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            LBDialog.ShowDialog();
            if (LBDialog.Cancel == false)
            {
                LSNote02 = LBDialog.LSetup;
                PATH_Note02_Bmp = PATH_DlgBmp; // R4.00 Path for back image.
                Note02_BackBmp = Note_BackBmp; // R4.00 Back Image.
                PATH_Note02_OVLBmp = PATH_DlgOVLBmp; // R4.00 Path for back image.
                Note02_OVLBmp = Note_OVLBmp; // R4.00 Overlay Image.
                FONT_Note02 = FONT_Setup;
                FONT_Note02_Name = FONT_Setup.Name;
                FONT_Note02_Size = FONT_Setup.Size.ToString();
                FONT_Note02_Bold = Conversions.ToString(FONT_Setup.Bold);
                FONT_Note02_Italic = Conversions.ToString(FONT_Setup.Italic);
                LSNote02.B1 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote02.O1) * 0.01d)),
                    LSNote02.B1.R, LSNote02.B1.G, LSNote02.B1.B);
                LSNote02.B2 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote02.O2) * 0.01d)),
                    LSNote02.B2.R, LSNote02.B2.G, LSNote02.B2.B);
                NOTE_CheckNoteSizes();
                Settings.SETTINGS_Save("");
            }

            LBDialog.Dispose();
        }

        private void cmNote03Setup_Click_1(object sender, EventArgs e)
        {
            var LBDialog = new frmLabelSetup(chkTips.Checked)
            {
                HideFormColor = true,
                LSetup = LSNote03
            };

            // R4.00 Get the data we are editing.
            FONT_Setup = FONT_Note03;
            PATH_DlgBmp = PATH_Note03_Bmp; // R4.00 Path for back image.
            Note_BackBmp = Note03_BackBmp; // R4.00 Back Image.
            PATH_DlgBmpPath = Utilities.PATH_StripFilename(PATH_DlgBmp); // R4.00 Path without Filename. 
            PATH_DlgOVLBmp = PATH_Note03_OVLBmp; // R4.00 Path for back image.
            Note_OVLBmp = Note03_OVLBmp; // R4.00 Back Image.
            PATH_DlgOVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            LBDialog.ShowDialog();
            if (LBDialog.Cancel == false)
            {
                LSNote03 = LBDialog.LSetup;
                PATH_Note03_Bmp = PATH_DlgBmp; // R4.00 Path for back image.
                Note03_BackBmp = Note_BackBmp; // R4.00 Back Image.
                PATH_Note03_OVLBmp = PATH_DlgOVLBmp; // R4.00 Path for back image.
                Note03_OVLBmp = Note_OVLBmp; // R4.00 Overlay Image.
                FONT_Note03 = FONT_Setup;
                FONT_Note03_Name = FONT_Setup.Name;
                FONT_Note03_Size = FONT_Setup.Size.ToString();
                FONT_Note03_Bold = Conversions.ToString(FONT_Setup.Bold);
                FONT_Note03_Italic = Conversions.ToString(FONT_Setup.Italic);
                LSNote03.B1 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote03.O1) * 0.01d)),
                    LSNote03.B1.R, LSNote03.B1.G, LSNote03.B1.B);
                LSNote03.B2 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote03.O2) * 0.01d)),
                    LSNote03.B2.R, LSNote03.B2.G, LSNote03.B2.B);
                NOTE_CheckNoteSizes();
                Settings.SETTINGS_Save("");
            }

            LBDialog.Dispose();
        }

        private void cmNote04Setup_Click_1(object sender, EventArgs e)
        {
            var LBDialog = new frmLabelSetup(chkTips.Checked)
            {
                HideFormColor = true,
                LSetup = LSNote04
            };

            // R4.00 Get the data we are editing.
            FONT_Setup = FONT_Note04;
            PATH_DlgBmp = PATH_Note04_Bmp; // R4.00 Path for back image.
            Note_BackBmp = Note04_BackBmp; // R4.00 Back Image.
            PATH_DlgBmpPath = Utilities.PATH_StripFilename(PATH_DlgBmp); // R4.00 Path without Filename. 
            PATH_DlgOVLBmp = PATH_Note04_OVLBmp; // R4.00 Path for back image.
            Note_OVLBmp = Note04_OVLBmp; // R4.00 Back Image.
            PATH_DlgOVLBmpPath = Utilities.PATH_StripFilename(PATH_DlgOVLBmp);

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            LBDialog.ShowDialog();
            if (LBDialog.Cancel == false)
            {
                LSNote04 = LBDialog.LSetup;
                PATH_Note04_Bmp = PATH_DlgBmp; // R4.00 Path for back image.
                Note04_BackBmp = Note_BackBmp; // R4.00 Back Image.
                PATH_Note04_OVLBmp = PATH_DlgOVLBmp; // R4.00 Path for back image.
                Note04_OVLBmp = Note_OVLBmp; // R4.00 Overlay Image.
                FONT_Note04 = FONT_Setup;
                FONT_Note04_Name = FONT_Setup.Name;
                FONT_Note04_Size = FONT_Setup.Size.ToString();
                FONT_Note04_Bold = Conversions.ToString(FONT_Setup.Bold);
                FONT_Note04_Italic = Conversions.ToString(FONT_Setup.Italic);
                LSNote04.B1 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote04.O1) * 0.01d)),
                    LSNote04.B1.R, LSNote04.B1.G, LSNote04.B1.B);
                LSNote04.B2 = Color.FromArgb((int) Math.Round(255d * (Conversion.Val(LSNote04.O2) * 0.01d)),
                    LSNote04.B2.R, LSNote04.B2.G, LSNote04.B2.B);
                NOTE_CheckNoteSizes();
                Settings.SETTINGS_Save("");
            }

            LBDialog.Dispose();
        }


        // ****************************************************************
        // R4.00 NOTE CLICK
        // ****************************************************************
        private void cmNote1_Click(object sender, EventArgs e)
        {
            var DlgNotes = new FrmNotes(chkTips.Checked);
            for (var t = 1; t <= 10; t++) DlgNotes.NoteText[t] = NoteAnim01_Text[t];

            DlgNotes.NoteAnim = NoteAnim01;
            DlgNotes.ShowDialog();
            if (DlgNotes.Cancel == false)
            {
                for (var t = 1; t <= 10; t++) NoteAnim01_Text[t] = DlgNotes.NoteText[t];

                NoteAnim01 = DlgNotes.NoteAnim;
                Settings.SETTINGS_Save("");
            }

            DlgNotes.Dispose();
        }

        private void cmNote2_Click(object sender, EventArgs e)
        {
            var DlgNotes = new FrmNotes(chkTips.Checked);
            for (var t = 1; t <= 10; t++) DlgNotes.NoteText[t] = NoteAnim02_Text[t];

            DlgNotes.NoteAnim = NoteAnim02;
            DlgNotes.ShowDialog();
            if (DlgNotes.Cancel == false)
            {
                for (var t = 1; t <= 10; t++) NoteAnim02_Text[t] = DlgNotes.NoteText[t];

                NoteAnim02 = DlgNotes.NoteAnim;
                Settings.SETTINGS_Save("");
            }

            DlgNotes.Dispose();
        }

        private void cmNote3_Click(object sender, EventArgs e)
        {
            var DlgNotes = new FrmNotes(chkTips.Checked);
            for (var t = 1; t <= 10; t++) DlgNotes.NoteText[t] = NoteAnim03_Text[t];

            DlgNotes.NoteAnim = NoteAnim03;
            DlgNotes.ShowDialog();
            if (DlgNotes.Cancel == false)
            {
                for (var t = 1; t <= 10; t++) NoteAnim03_Text[t] = DlgNotes.NoteText[t];

                NoteAnim03 = DlgNotes.NoteAnim;
                Settings.SETTINGS_Save("");
            }

            DlgNotes.Dispose();
        }

        private void cmNote4_Click(object sender, EventArgs e)
        {
            var DlgNotes = new FrmNotes(chkTips.Checked);
            for (var t = 1; t <= 10; t++) DlgNotes.NoteText[t] = NoteAnim04_Text[t];

            DlgNotes.NoteAnim = NoteAnim04;
            DlgNotes.ShowDialog();
            if (DlgNotes.Cancel == false)
            {
                for (var t = 1; t <= 10; t++) NoteAnim04_Text[t] = DlgNotes.NoteText[t];

                NoteAnim04 = DlgNotes.NoteAnim;
                Settings.SETTINGS_Save("");
            }

            DlgNotes.Dispose();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // R4.00 Delete all of our Allocated bitmaps, etc
            Note01_Bmp.Dispose();
            Note02_Bmp.Dispose();
            Note03_Bmp.Dispose();
            Note04_Bmp.Dispose();
            Note01_Gfx.Dispose();
            Note02_Gfx.Dispose();
            Note03_Gfx.Dispose();
            Note04_Gfx.Dispose();
            FONT_Rank.Dispose();
            FONT_Name.Dispose();
            FONT_Note01.Dispose();
            FONT_Note02.Dispose();
            FONT_Note03.Dispose();
            FONT_Note04.Dispose();
        }

        private void scrVolume_ValueChanged(object sender, EventArgs e)
        {
            int currentVolume;

            // R4.00 Scrollbars are bugged.
            currentVolume = scrVolume.Value;
            if (currentVolume < 0) currentVolume = 0;

            if (100 < currentVolume) currentVolume = 100;

            AUDIO_SetVolume(currentVolume, 100);
            lbVolume.Text = currentVolume.ToString();
        }

        public void AUDIO_SetVolume(int MasterVol, int SoundVol)
        {
            // R4.10 Receive a MASTER and CHAN/SOUND volume that is 0 - 100 (OFF to FULL volume).

            var NewVolume = (uint) Math.Round(ushort.MaxValue * (MasterVol * 0.01d) * (SoundVol * 0.01d));

            // Set the same volume for both the left And the right channels
            var NewVolumeAllChannels =
                (ulong) ((NewVolume & 0xFFFFL) | (long) Math.Round(NewVolume * Math.Pow(2d, 16d)));

            // Set the volume
            Native.waveOutSetVolume(IntPtr.Zero, (uint) NewVolumeAllChannels);
        }

        private void cboNoteSpace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FLAG_Loading) return;

            NOTE_Spacing = cboNoteSpace.SelectedIndex;
            SCREEN_Organize();
            Settings.SETTINGS_Save("");
        }

        private void frmMain_Closing(object sender, CancelEventArgs e)
        {
            Settings.SETTINGS_Save("");
        }

        // ****************************************************************************
        // R4.00 CONTEXT MENU STRIP (ToolStripMenu) FOR STATS
        // ****************************************************************************
        private void tsmPlayer_Relic_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.companyofheroes.com/leaderboards#profile/steam/" + PlrSteam[Celo_PopupHit] +
                          "/standings");
        }

        private void tsmPlayer_OrgAT_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.coh2.org/ladders/playercard/viewBoard/1/steamid/" + PlrSteam[Celo_PopupHit]);
        }

        private void tsmPlayer_Google_Click(object sender, EventArgs e)
        {
            Process.Start("https://translate.google.com/#view=home&op=translate&sl=auto&tl=en&text=" +
                          PlrName[Celo_PopupHit]);
        }

        private void tsmPlayer_Steam_Click(object sender, EventArgs e)
        {
            Process.Start("https://steamcommunity.com/profiles/" + PlrSteam[Celo_PopupHit] + "/");
        }

        private void tsmPlayer_OrgFaction_Click(object sender, EventArgs e)
        {
            // R4.00 Try to select the correct STATS page from.org.
            switch (PlrFact[Celo_PopupHit] ?? "")
            {
                case "05":
                {
                    Process.Start("https://www.coh2.org/ladders/playercard/viewBoard/12/steamid/" +
                                  PlrSteam[Celo_PopupHit]); // R4.00 UKF
                    break;
                }

                case "02":
                case "01":
                {
                    Process.Start("https://www.coh2.org/ladders/playercard/viewBoard/11/steamid/" +
                                  PlrSteam[Celo_PopupHit]); // R4.00 SOV, OST
                    break;
                }

                case "04":
                case "03":
                {
                    Process.Start("https://www.coh2.org/ladders/playercard/viewBoard/10/steamid/" +
                                  PlrSteam[Celo_PopupHit]); // R4.00 USF, OKW
                    break;
                }

                default:
                {
                    Process.Start("https://www.coh2.org/ladders/playercard/steamid/" + PlrSteam[Celo_PopupHit]);
                    break;
                }
            }
        }

        private void tsmPlayer_OrgPlayercard_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.coh2.org/ladders/playercard/steamid/" + PlrSteam[Celo_PopupHit]);
        }

        private void chkPopUp_CheckedChanged(object sender, EventArgs e)
        {
            // R4.34 This needs restored when Relic fixes the LOG file issues.
            // If chkPopUp.Checked Then
            // Celo_Popup = True
            // ToolTip1.SetToolTip(pbStats, "Click a player name to see web pages for:" & vbCr & "Left: Relic Stats" & vbCr & "Ctrl-Left: Google Translate" & vbCr & "Shift-Left: Coh2.org player page" & vbCr & "Right: Popup menu")
            // Else
            // Celo_Popup = False
            // ToolTip1.SetToolTip(pbStats, "Click a player name to see web pages for:" & vbCr & "Left: Relic Stats" & vbCr & "Ctrl-Left: Google Translate" & vbCr & "Shift-Left: Coh2.org player page" & vbCr & "Right: Coh2.org AT stats" & vbCr & "Ctrl-Right: Coh2.org faction page")
            // End If
        }

        private void tsmSelectFile_Click(object sender, EventArgs e)
        {
            AUDIO_SelectFile(Celo_PopupHit);
        }

        private void AUDIO_SelectFile(int index)
        {
            var fd = new OpenFileDialog
            {
                Title = "Select a sound to play"
            };
            if (string.IsNullOrEmpty(SOUND_File[index]))
            {
                if (!string.IsNullOrEmpty(PATH_SoundFiles)) fd.InitialDirectory = PATH_SoundFiles;
            }
            else
            {
                PATH_SoundFiles = Utilities.PATH_StripFilename(SOUND_File[index]);
                fd.InitialDirectory = PATH_SoundFiles;
            }

            fd.Filter = "Wave Files (*.wav)|*.wav";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                SOUND_File[index] = fd.FileName;
                ToolTip1.SetToolTip((Control) CELO_PopUpObject, SOUND_File[index]);
            }
        }

        private void tsmSetVolTo100_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 100.ToString();
        }

        private void tsmSetVolTo90_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 90.ToString();
        }

        private void tsmSetVolTo80_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 80.ToString();
        }

        private void tsmSetVolTo70_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 70.ToString();
        }

        private void tsmSetVolTo60_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 60.ToString();
        }

        private void tsmSetVolTo50_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 50.ToString();
        }

        private void tsmSetVolTo40_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 40.ToString();
        }

        private void tsmSetVolTo30_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 30.ToString();
        }

        private void tsmSetVolTo20_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 20.ToString();
        }

        private void tsmSetVolTo10_Click(object sender, EventArgs e)
        {
            SOUND_Vol[Celo_PopupHit] = 10.ToString();
        }

        private void tbXoff_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Conversions.ToString(e.KeyChar) != Constants.vbCr) return;

            // R4.10 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;

            // R4.50 Force STATS redraw.
            MainBuffer_Valid = false;
            Settings.SETTINGS_Save("");
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void tbYoff_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Conversions.ToString(e.KeyChar) != Constants.vbCr) return;

            // R4.10 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;

            // R4.50 Force STATS redraw.
            MainBuffer_Valid = false;
            Settings.SETTINGS_Save("");
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void tbXsize_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbXsize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Conversions.ToString(e.KeyChar) != Constants.vbCr) return;

            // R4.10 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            STATS_ClipXY((float) Conversion.Val(tbXsize.Text), (float) Conversion.Val(tbYSize.Text));
            STATS_AdjustSize();
            STATS_DefineY();
            STATS_DefineX(); // R2.00 X gets adjusted to Y size for faction images.
            Settings.SETTINGS_Save("");
            SCREEN_Organize();
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void tbYSize_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbYSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Conversions.ToString(e.KeyChar) != Constants.vbCr) return;

            // R4.10 Added drawing flag.
            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;
            STATS_ClipXY((float) Conversion.Val(tbXsize.Text), (float) Conversion.Val(tbYSize.Text));
            STATS_AdjustSize();
            STATS_DefineY();
            STATS_DefineX(); // R2.00 X gets adjusted to Y size for faction images.
            Settings.SETTINGS_Save("");
            SCREEN_Organize();
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void cmDefaults_Click(object sender, EventArgs e)
        {
            // R4.10 Added a RESTORE DEFAULTS button.

            var result = (int) MessageBox.Show(
                "You have chosen to restore all STATS pages settings to their default state." + Constants.vbCr +
                Constants.vbCr + "Do you wish To continue?", "STATS - Restore Defaults", MessageBoxButtons.YesNoCancel);
            if ((result == (int) DialogResult.Cancel) | (result == (int) DialogResult.No)) return;

            if (FLAG_Loading | FLAG_Drawing) return;

            FLAG_Drawing = true;

            // R4.30 Updated.
            STATS_ClipXY(990f, 180f);
            STATS_AdjustSize();
            tbXsize.Text = STATS_SizeX.ToString();
            tbYSize.Text = STATS_SizeY.ToString();
            tbXoff.Text = "0";
            tbYoff.Text = "0";
            cboLayoutY.SelectedIndex = 3;
            STATS_DefineY();
            cboLayoutX.SelectedIndex = 3;
            STATS_DefineX();
            SCREEN_Organize();
            GFX_DrawStats();
            FLAG_Drawing = false;
        }

        private void cmStatsModeHelp_Click(object sender, EventArgs e)
        {
            // R4.30 Updated.
            var A = "This section defines the size and layout of the STATS page below." + Constants.vbCr;
            A += "Click DEFAULTS to restore the settings to their default sizes." + Constants.vbCr + Constants.vbCr;
            A += "The default size is 990x180 which covers all of the COH2 bottom info area." + Constants.vbCr +
                 Constants.vbCr;
            A +=
                "The XY offset values move the stats data around inside the STATS page. Adjust these values to line up the stats data with a custom made background image." +
                Constants.vbCr + Constants.vbCr;
            Interaction.MsgBox(A, MsgBoxStyle.Information, "HELP - Stats Size and Layout");
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            // R4.10 We can get slightly smoother animations using INVALIDATE and this sub to update the screen.
            // R4.10 But there is a CPU/GPU cost (about 10x).
            if (ANIMATION_Smooth)
            {
                if ((cmNote01_Play.Text == "||") & NoteAnim01.Active)
                {
                    var argpbNote = pbNote1;
                    NOTE_Animate(ref argpbNote, ref Note01_Text, ref LSNote01, ref FONT_Note01, ref NoteAnim01,
                        ref Note01_Bmp, ref Note01_Gfx, ref Note01_BackBmp, ref Note01_OVLBmp);
                    pbNote1 = argpbNote;
                }

                if ((cmNote02_Play.Text == "||") & NoteAnim02.Active)
                {
                    var argpbNote1 = pbNote2;
                    NOTE_Animate(ref argpbNote1, ref Note02_Text, ref LSNote02, ref FONT_Note02, ref NoteAnim02,
                        ref Note02_Bmp, ref Note02_Gfx, ref Note02_BackBmp, ref Note02_OVLBmp);
                    pbNote2 = argpbNote1;
                }

                if ((cmNote03_Play.Text == "||") & NoteAnim03.Active)
                {
                    var argpbNote2 = pbNote3;
                    NOTE_Animate(ref argpbNote2, ref Note03_Text, ref LSNote03, ref FONT_Note03, ref NoteAnim03,
                        ref Note03_Bmp, ref Note03_Gfx, ref Note03_BackBmp, ref Note03_OVLBmp);
                    pbNote3 = argpbNote2;
                }

                if ((cmNote04_Play.Text == "||") & NoteAnim04.Active)
                {
                    var argpbNote3 = pbNote4;
                    NOTE_Animate(ref argpbNote3, ref Note04_Text, ref LSNote04, ref FONT_Note04, ref NoteAnim04,
                        ref Note04_Bmp, ref Note04_Gfx, ref Note04_BackBmp, ref Note04_OVLBmp);
                    pbNote4 = argpbNote3;
                }
            }
        }

        private void chkSmoothAni_CheckedChanged(object sender, EventArgs e)
        {
            // R4.10 We can get slightly smoother animations using INVALIDATE and Form_Paint() to update the screen.
            // R4.10 But there is a CPU/GPU cost (about 10x).
            ANIMATION_Smooth = chkSmoothAni.Checked;
        }

        private void cmSetupLoad_Click(object sender, EventArgs e)
        {
            var fd = new OpenFileDialog();
            string A;
            int N;
            var IsOldStyle = default(bool);

            // R2.00 Find the LOG file. Try top help get to the right location if possible.
            fd.Title = "MakoCELO Load a SETUP file from disk";
            fd.InitialDirectory =
                !string.IsNullOrEmpty(PATH_SetupPath) ? PATH_SetupPath : SpecialDirectories.MyDocuments;

            fd.Filter = "Celo Setup Files (*.mcs)|*.mcs";
            fd.FilterIndex = 1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                PATH_SetupPath = fd.FileName;
                if (Settings.SETTINGS_Load_CheckVersion(PATH_SetupPath, ref IsOldStyle))
                {
                    if (IsOldStyle)
                        Settings.SETTINGS_Load_Old(PATH_SetupPath);
                    else
                        Settings.SETTINGS_Load(PATH_SetupPath);
                }

                // R3.40 Strip off filename so we can use it for init dir later.
                N = Conversions.ToInteger(Utilities.STRING_FindLastSlash(PATH_SetupPath));
                if (3 < N)
                {
                    // R4.50 Add current SETUP file name.
                    A = Strings.Mid(PATH_SetupPath, N + 1, Strings.Len(PATH_SetupPath) - N);
                    SS1_Filename.Text = A;
                    PATH_SetupPath = Strings.Mid(PATH_SetupPath, 1, N);
                }
                else
                {
                    PATH_SetupPath = "";
                }

                SETUP_Apply();
                STATS_Refresh();
            }
        }

        private void cmSetupSave_Click(object sender, EventArgs e)
        {
            var fd = new SaveFileDialog();
            int N;

            // R2.00 Find the LOG file. Try top help get to the right location if possible.
            fd.Title = "MakoCELO Save a SETUP file to disk";
            fd.InitialDirectory =
                !string.IsNullOrEmpty(PATH_SetupPath) ? PATH_SetupPath : SpecialDirectories.MyDocuments;

            fd.Filter = "Celo Setup Files (*.mcs)|*.mcs";
            fd.FilterIndex = 1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                PATH_SetupPath = fd.FileName;
                Settings.SETTINGS_Save(PATH_SetupPath);

                // R3.40 Strip off filename so we can use it for init dir later.
                N = Conversions.ToInteger(Utilities.STRING_FindLastSlash(PATH_SetupPath));
                PATH_SetupPath = 3 < N ? Strings.Mid(PATH_SetupPath, 1, N) : "";
            }
        }

        private void chkGUIMode_CheckedChanged(object sender, EventArgs e)
        {
            // R4.30 Changed from buttons to checkbox.
            if (chkGUIMode.Checked)
            {
                GUI_SetDark();
                GUI_ColorMode = 1;
                Settings.SETTINGS_Save("");
            }
            else
            {
                GUI_SetLite();
                GUI_ColorMode = 0;
                Settings.SETTINGS_Save("");
            }
        }

        private void cboDelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            SCAN_Time = (long) Math.Round(Conversion.Val(cboDelay.Items[cboDelay.SelectedIndex])); // R4.30 Added.
        }

        private void chkHideMissing_CheckedChanged(object sender, EventArgs e)
        {
            // R4.30 Added code to hide not used players slots.
            FLAG_HideMissing = chkHideMissing.Checked;

            Settings.SETTINGS_Save("");
        }

        private void chkFoundSound_CheckedChanged(object sender, EventArgs e)
        {
            // R4.30 Added to play a sound when a match is found.
            Settings.SETTINGS_Save("");
        }

        private void cmTest_Click(object sender, EventArgs e)
        {
            ELO_Setup();
        }

        private void ELO_Setup()
        {
            var PSDialog = new frmMaxPlayerSearch(); // With {} ' {.HideSizeOptions = True, .HideSizeAll = True}

            // R4.00 Get the data we are editing.
            for (var t = 1; t <= 7; t++)
            for (var tt = 1; tt <= 4; tt++)
                PSDialog.LVLs[t, tt] = LVLS[t, tt];

            // R4.00 Call the setup dialog and default to CANCEL being pressed.
            PSDialog.ShowDialog();
            if (PSDialog.Cancel == false)
            {
                for (var t = 1; t <= 7; t++)
                for (var tt = 1; tt <= 4; tt++)
                    LVLS[t, tt] = PSDialog.LVLs[t, tt];

                Settings.SETTINGS_Save("");
            }
        }

        private void ELO_VerifyData()
        {
            var BadData = default(bool);

            // R4.30 Check for VALID max layer counts.
            for (var T1 = 1; T1 <= 5; T1++)
            for (var T2 = 1; T2 <= 4; T2++)
                if (Conversion.Val(LVLS[T1, T2]) < 1d)
                    BadData = true;

            for (var T1 = 6; T1 <= 7; T1++)
            for (var T2 = 2; T2 <= 4; T2++)
                if (Conversion.Val(LVLS[T1, T2]) < 1d)
                    BadData = true;

            FLAG_EloValid = BadData ? false : true;
        }

        private void chkShowELO_CheckedChanged(object sender, EventArgs e)
        {
            string A;

            // R4.30 Added ELO percantage.
            FLAG_EloUse = chkShowELO.Checked;

            // R4.30 Dont do validity checks on load or ELO not used.
            if (FLAG_Loading | (FLAG_EloUse == false))
            {
                FLAG_EloMode = 0;
                // GFX_DrawStats()
                STATS_Refresh();
                return;
            }

            // R4.30 Check for VALID max player counts.
            ELO_VerifyData();
            if (FLAG_EloValid == false)
            {
                A = "NOTE: To calc ELO values, MakoCELO needs the max players" + Constants.vbCr + Constants.vbCr;
                A = A + "for each game mode. This can be done by the RANK setup area." + Constants.vbCr +
                    Constants.vbCr;
                A = A + "Would you like to OPEN the ELO setup dialog now?" + Constants.vbCr + Constants.vbCr;
                var result = (DialogResult) Interaction.MsgBox(A, MsgBoxStyle.YesNo, "MakoCELO ELO Setup");

                // R4.30 User selected YES.
                if (result == DialogResult.No)
                {
                    chkShowELO.Checked = false;
                    FLAG_EloUse = false;
                }
                else
                {
                    // R4.30 Let user get ELO data, then verify that data.
                    ELO_Setup();
                    ELO_VerifyData();
                    if (FLAG_EloValid == false)
                    {
                        A = "NOTE: The ELO data appears to be incomplete." + Constants.vbCr + Constants.vbCr;
                        A = A + "ELO Cyclong will be disabled until the ELO" + Constants.vbCr + Constants.vbCr;
                        A = A + "data is completed." + Constants.vbCr + Constants.vbCr;
                        Interaction.MsgBox(A, MsgBoxStyle.Information, "MakoCELO ELO Setup");
                        chkShowELO.Checked = false;
                        FLAG_EloUse = false;
                    }
                }
            }

            Settings.SETTINGS_Save("");
            STATS_Refresh();
        }


        private string Country_GetName(string CA)
        {
            var tName = "";
            for (int t = 1, loopTo = CountryCount; t <= loopTo; t++)
                if ((CA ?? "") == (Country_Abbr[t] ?? ""))
                {
                    tName = Country_Name[t];
                    break;
                }

            return tName;
        }

        public void STAT_GetFromRID(string RID, int PLRSlot)
        {
            int P2;
            var RawResp = "";
            try
            {
                // R4.30 Request leaderboard data from Relic Web API. Put result JSON data in string for parsing.
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + PLRSlot +
                                 " Web Request sending...");
                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12 'R4.43 Added for Connection issues.

                var A =
                    "https://coh2-api.reliclink.com/community/leaderboard/GetPersonalStat?title=coh2&profile_ids=[" +
                    RID + "]";
                WBrequest = (HttpWebRequest) WebRequest.Create(A);
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + PLRSlot +
                                 " Getting response...");
                WBresponse = (HttpWebResponse) WBrequest.GetResponse();
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + PLRSlot +
                                 " Getting stream...");
                WBreader = new StreamReader(WBresponse.GetResponseStream());
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + PLRSlot +
                                 " Reading stream...");
                RawResp = WBreader.ReadToEnd();
                WBresponse.Close();
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - Get RID - PLR:" + PLRSlot +
                                 " Parsing data..." + Strings.Len(RawResp) + " bytes");

                // R4.41 Added to catch bad data.
                if (Strings.Len(RawResp) < 10)
                {
                    lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID - PLR:" + PLRSlot +
                                     " No data returned.");
                    lbError1.Text = "RID error:" + PLRSlot;
                    lbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // R4.41 Added to catch bad data.
                A = "message" + '"' + ":" + '"' + "SUCCESS";
                var P1 = Strings.InStr(RawResp, A);
                if (P1 < 1)
                {
                    lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID - PLR:" + PLRSlot +
                                     " Server returned error.");
                    lbError1.Text = "RID error:" + PLRSlot;
                    lbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // R4.41 Start to PARSE the JSON data in a crude and broken manner.
                P1 = Strings.InStr(RawResp, "statGroups");

                // R4.45 Get the players COUNTRY.
                if (0 < P1)
                {
                    A = "profile_id" + '"' + ":" + RID;
                    P2 = Strings.InStr(P1, RawResp, A);
                    if (0 < P2)
                    {
                        P2 = Strings.InStr(P2, RawResp, "country");
                        if (0 < P2)
                        {
                            PlrCountry[PLRSlot] = RawResp.Substring(P2 + 9, 2);
                            PlrCountryName[PLRSlot] = Country_GetName(PlrCountry[PLRSlot]);
                        }
                        else
                        {
                            PlrCountry[PLRSlot] = "";
                            PlrCountryName[PLRSlot] = "";
                        }
                    }
                }

                // R4.45 Get the Leadboard ranks (player card).
                P2 = Strings.InStr(P1, RawResp, "leaderboard_id");
                while (0 < P2)
                {
                    var s = RawResp.Substring(P2 + 15, 6);
                    var GMode = (int) Math.Round(Conversion.Val(s));
                    for (var T1 = 1; T1 <= 5; T1++)
                    for (var T2 = 1; T2 <= 4; T2++)
                        if (GMode == Conversions.ToDouble(RelDataLeaderID[T1, T2]))
                        {
                            var P3 = Strings.InStr(P2, RawResp, "wins");
                            PlrRankWin[PLRSlot, T1, T2] =
                                (int) Math.Round(Conversion.Val(RawResp.Substring(P3 + 5, 10)));
                            P3 = Strings.InStr(P2, RawResp, "losses");
                            PlrRankLoss[PLRSlot, T1, T2] =
                                (int) Math.Round(Conversion.Val(RawResp.Substring(P3 + 7, 10)));
                            P3 = Strings.InStr(P2, RawResp, "rank");
                            PlrRankALL[PLRSlot, T1, T2] =
                                (int) Math.Round(Conversion.Val(RawResp.Substring(P3 + 5, 10)));
                            if (PlrRankALL[PLRSlot, T1, T2] == -1) PlrRankALL[PLRSlot, T1, T2] = 0;

                            if (0 < PlrRankWin[PLRSlot, T1, T2])
                                PlrRankPerc[PLRSlot, T1, T2] =
                                    (100 * PlrRankWin[PLRSlot, T1, T2] /
                                     (double) (PlrRankWin[PLRSlot, T1, T2] + PlrRankLoss[PLRSlot, T1, T2]))
                                    .ToString("#0");
                            else
                                PlrRankPerc[PLRSlot, T1, T2] = "";
                        }

                    P2 = Strings.InStr(P2 + 15, RawResp, "leaderboard_id");
                }
            }
            catch (Exception)
            {
                // R4.41 Added logging and color change.
                lbError1.Text = "RID error:" + PLRSlot;
                lbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR RID - PLR:" + PLRSlot + " " +
                                 Information.Err().Description);
            }

            if (!string.IsNullOrEmpty(RawResp)) STAT_GetTeamsFromRID(RawResp, PLRSlot);
        }

        private void STAT_GetTeamsFromRID(string RawResp, int PLRslot) // R4.45 Was  RID As Integer, PLRSlot As Integer)
        {
            string s;
            string PLR1;
            string PLR2;
            string PLR3;
            string PLR4;
            int RID2;
            int RID3;
            int RID4;
            var Cnt = default(int);
            try
            {
                // R4.41 Added to catch bad data.
                if (Strings.Len(RawResp) < 10)
                {
                    lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR TEAM - PLR:" + PLRslot +
                                     " No data returned.");
                    lbError1.Text = "Team error:" + PLRslot;
                    lbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // R4.41 Added to catch bad data.
                var A = "message" + '"' + ":" + '"' + "SUCCESS";
                var P1 = Strings.InStr(RawResp, A);
                if (P1 < 1)
                {
                    lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR TEAM - PLR:" + PLRslot +
                                     " Server returned error.");
                    lbError1.Text = "Team error:" + PLRslot;
                    lbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                    return;
                }

                // *************************************************
                // R4.33 Find all PREMADE TEAMS. Can be team of 1.
                // R4.41 Start to PARSE the JSON data in a crude and broken manner.
                // *************************************************
                P1 = Strings.InStr(RawResp, "statGroups");
                var PEnd = Strings.InStr(P1, RawResp, "leaderboardStats");
                P1 = Strings.InStr(P1 + 13, RawResp, "{" + '"' + "id");
                while ((0 < P1) & (P1 < PEnd) & (Cnt < 500))
                {
                    Cnt += 1;
                    lbStatus.Text = "Team: " + Cnt;
                    lbStatus.Refresh();
                    s = RawResp.Substring(P1 + 5, 9);
                    var RankID = (int) Math.Round(Conversion.Val(s));
                    P1 = Strings.InStr(P1 + 5, RawResp, "type");
                    s = RawResp.Substring(P1 + 5, 9);
                    var PlrCnt = (int) Math.Round(Conversion.Val(s));

                    // R4.34 Get the relicID and Name of each player in this team. Team can be 1-4 players.
                    P1 = Strings.InStr(P1 + 5, RawResp, "profile_id");
                    s = RawResp.Substring(P1 + 11, 32);
                    var P2 = Strings.InStr(P1 + 10, RawResp, Conversions.ToString('"'));
                    var RID1 = (int) Math.Round(Conversion.Val(s.Substring(0, P2 - (P1 + 0))));
                    P1 = Strings.InStr(P1 + 5, RawResp, "alias");
                    s = RawResp.Substring(P1 + 7, 64); // R4.44 Was 32 chars long.
                    P2 = Strings.InStr(P1 + 8, RawResp, Conversions.ToString('"'));
                    PLR1 = s.Substring(0, P2 - (P1 + 8));
                    if (1 < PlrCnt)
                    {
                        P1 = Strings.InStr(P1 + 5, RawResp, "profile_id");
                        s = RawResp.Substring(P1 + 11, 32);
                        P2 = Strings.InStr(P1 + 10, RawResp, Conversions.ToString('"'));
                        RID2 = (int) Math.Round(Conversion.Val(s.Substring(0, P2 - (P1 + 0))));
                        P1 = Strings.InStr(P1 + 5, RawResp, "alias");
                        s = RawResp.Substring(P1 + 7, 64); // R4.44 Was 32 chars long.
                        P2 = Strings.InStr(P1 + 8, RawResp, Conversions.ToString('"'));
                        PLR2 = s.Substring(0, P2 - (P1 + 8));
                    }
                    else
                    {
                        RID2 = 0;
                        PLR2 = "";
                    }

                    if (2 < PlrCnt)
                    {
                        P1 = Strings.InStr(P1 + 5, RawResp, "profile_id");
                        s = RawResp.Substring(P1 + 11, 32);
                        P2 = Strings.InStr(P1 + 10, RawResp, Conversions.ToString('"'));
                        RID3 = (int) Math.Round(Conversion.Val(s.Substring(0, P2 - (P1 + 0))));
                        P1 = Strings.InStr(P1 + 5, RawResp, "alias");
                        s = RawResp.Substring(P1 + 7, 64); // R4.44 Was 32 chars long.
                        P2 = Strings.InStr(P1 + 8, RawResp, Conversions.ToString('"'));
                        PLR3 = s.Substring(0, P2 - (P1 + 8));
                    }
                    else
                    {
                        RID3 = 0;
                        PLR3 = "";
                    }

                    if (3 < PlrCnt)
                    {
                        P1 = Strings.InStr(P1 + 5, RawResp, "profile_id");
                        s = RawResp.Substring(P1 + 11, 32);
                        P2 = Strings.InStr(P1 + 10, RawResp, Conversions.ToString('"'));
                        RID4 = (int) Math.Round(Conversion.Val(s.Substring(0, P2 - (P1 + 0))));
                        P1 = Strings.InStr(P1 + 5, RawResp, "alias");
                        s = RawResp.Substring(P1 + 7, 64); // R4.44 Was 32 chars long.
                        P2 = Strings.InStr(P1 + 8, RawResp, Conversions.ToString('"'));
                        PLR4 = s.Substring(0, P2 - (P1 + 8));
                    }
                    else
                    {
                        RID4 = 0;
                        PLR4 = "";
                    }

                    TeamList[PLRslot, Cnt].PLR1 = PLR1;
                    TeamList[PLRslot, Cnt].PLR2 = PLR2;
                    TeamList[PLRslot, Cnt].PLR3 = PLR3;
                    TeamList[PLRslot, Cnt].PLR4 = PLR4;
                    TeamList[PLRslot, Cnt].RID1 = RID1;
                    TeamList[PLRslot, Cnt].RID2 = RID2;
                    TeamList[PLRslot, Cnt].RID3 = RID3;
                    TeamList[PLRslot, Cnt].RID4 = RID4;
                    TeamList[PLRslot, Cnt].RankID = RankID;
                    TeamList[PLRslot, Cnt].PlrCnt = PlrCnt;
                    P1 = Strings.InStr(P1 + 5, RawResp, "{" + '"' + "id");
                }

                TeamListCnt[PLRslot] = Cnt;

                // ******************************************
                // R4.33 Find all ranks for premade teams.
                // ******************************************
                P1 = PEnd;
                PEnd = Strings.Len(RawResp);
                Cnt = 0;
                P1 = Strings.InStr(P1 + 13, RawResp, "statgroup_id");
                while ((0 < P1) & (P1 < PEnd) & (Cnt < 1500))
                {
                    Cnt += 1;
                    s = RawResp.Substring(P1 + 13, 12);
                    var RankID2 = (int) Math.Round(Conversion.Val(s));
                    P1 = Strings.InStr(P1 + 13, RawResp, "leaderboard_id");
                    s = RawResp.Substring(P1 + 15, 12);
                    var LID = (int) Math.Round(Conversion.Val(s));
                    P1 = Strings.InStr(P1 + 13, RawResp, "wins");
                    s = RawResp.Substring(P1 + 5, 12);
                    var Win = (int) Math.Round(Conversion.Val(s));
                    P1 = Strings.InStr(P1 + 5, RawResp, "losses");
                    s = RawResp.Substring(P1 + 7, 12);
                    var Loss = (int) Math.Round(Conversion.Val(s));
                    P1 = Strings.InStr(P1 + 7, RawResp, "rank");
                    s = RawResp.Substring(P1 + 5, 12);
                    var Rank = (int) Math.Round(Conversion.Val(s));

                    // R4.33 Try to find a rank for this team. 
                    for (int t = 1, loopTo = TeamListCnt[PLRslot]; t <= loopTo; t++)
                        if (TeamList[PLRslot, t].RankID == RankID2)
                        {
                            if ((LID == 20) | (LID == 22) | (LID == 24))
                            {
                                TeamList[PLRslot, t].RankAxis = Rank;
                                TeamList[PLRslot, t].WinAxis = Win;
                                TeamList[PLRslot, t].LossAxis = Loss;
                                break;
                            }

                            if ((LID == 21) | (LID == 23) | (LID == 25))
                            {
                                TeamList[PLRslot, t].RankAllies = Rank;
                                TeamList[PLRslot, t].WinAllies = Win;
                                TeamList[PLRslot, t].LossAllies = Loss;
                                break;
                            }
                        }

                    P1 = Strings.InStr(P1 + 5, RawResp, "statgroup_id");
                }
            }
            catch (Exception)
            {
                // R4.41 Added logging and color change.
                lbError1.Text = "Team Error";
                lbError1.Refresh();
                lbError1.BackColor = Color.FromArgb(255, 255, 0, 0);
                lstLog.Items.Add(DateAndTime.Now.ToLongTimeString() + " - ERROR TEAM - PLR:" + PLRslot + " " +
                                 Information.Err().Description);
            }

            lbStatus.Text = "";
        }

        private void timELOCycle_Tick(object sender, EventArgs e)
        {
            if (FLAG_EloUse & FLAG_EloValid)
            {
                // *****************************************************************
                // R4.30 If doing ELO cycles, calc the current Cycle to show.
                // *****************************************************************
                FLAG_EloMode += 1;
                if (2 < FLAG_EloMode) FLAG_EloMode = 0;

                // R4.20 Moved sub for outside calls.
                GFX_DrawStats();

                // R4.30 Try to do some cleanup.
                Application.DoEvents();
            }
            else
            {
                FLAG_EloMode = 0;
            }
        }

        private void scrStats_Scroll(object sender, ScrollEventArgs e)
        {
        }

        private void scrStats_ValueChanged(object sender, EventArgs e)
        {
            if (FLAG_ShowPlayerCard == 2) GFX_DrawTeams(FLAG_ShowPlayerCardNum);
        }

        private void pbStats_MouseWheel(object sender, MouseEventArgs e)
        {
            if (FLAG_ShowPlayerCard == 2)
            {
                long Yold = 0 < e.Delta ? scrStats.Value - 90 : scrStats.Value + 90;

                if (Yold < scrStats.Minimum) Yold = scrStats.Minimum;

                if (scrStats.Maximum < Yold) Yold = scrStats.Maximum;

                scrStats.Value = (int) Yold;

                // R4.50 Data is drawn in scrollbar code.
            }
        }

        private void chkSpeech_CheckedChanged(object sender, EventArgs e)
        {
            // R4.34 Added.
            Settings.SETTINGS_Save("");
        }

        private void chkGetTeams_CheckedChanged(object sender, EventArgs e)
        {
            // R4.34 Added.
            Settings.SETTINGS_Save("");
        }

        private void FillRoundedRectangle(Graphics Graphics, Rectangle Rectangle, Brush Brush, int radius)
        {
            using var path = RoundedRectangle(Rectangle, radius);
            Graphics.FillPath(Brush, path);
        }

        private void FillRoundedRectangle_Max(Graphics Graphics, Rectangle Rectangle, Brush Brush)
        {
            using var path = RoundedRectangle_Max(Rectangle);
            Graphics.FillPath(Brush, path);
        }

        private GraphicsPath RoundedRectangle(Rectangle r, int radius)
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

        private GraphicsPath RoundedRectangle_Max(Rectangle r)
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

        private void cmErrLog_Click(object sender, EventArgs e)
        {
            var LogDialog =
                new frmErrLog(lstLog.Items.Cast<object>()
                    .ToList()); // With {} ' {.HideSizeOptions = True, .HideSizeAll = True}

            // R4.42 Show the Error Log dialog.
            LogDialog.ShowDialog();
        }

        private void chkCountry_CheckedChanged(object sender, EventArgs e)
        {
            // R4.45 Added to Country Flags.
            Settings.SETTINGS_Save("");
        }

        private void RES_GetCountryData()
        {
            var Cnt = default(int);

            // R4.46 Default to no country names available.
            CountryCount = 0;
            try
            {
                using var MyReader =
                    new TextFieldParser(
                        new StringReader(Resources.country_defs))
                    {
                        TextFieldType = FieldType.Delimited
                    };
                MyReader.SetDelimiters(",");
                while (!MyReader.EndOfData)
                {
                    var CurrentRow = MyReader.ReadFields();
                    Cnt += 1;
                    Country_Name[Cnt] = CurrentRow[0];
                    Country_Abbr[Cnt] = Strings.LCase(CurrentRow[1]);
                }

                CountryCount = Cnt;
            }
            catch
            {
                Interaction.MsgBox(
                    "Error: parsing Country name resource file." + Constants.vbCrLf + Information.Err().Description,
                    MsgBoxStyle.ApplicationModal, "MakoCELO - Loading error");
            }
        }

        private void tbXoff_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbYoff_TextChanged(object sender, EventArgs e)
        {
        }

        private void _chkToggleOverlay_CheckedChanged(object sender, EventArgs e)
        {
            TimerService.EnableHighPrecisionTimers();
        }
    }
}