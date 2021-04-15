using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace MakoCelo
{
    public class Utilities
    {
        public static string PATH_StripFilename(string tPath)
        {
            // R2.00 Strip the filename off for init dir on dialog.  
            string s;
            var n = Conversions.ToInteger(STRING_FindLastSlash(tPath));
            if (3 < n)
            {
                s = Strings.Mid(tPath, 1, n);
            }
            else
            {
                s = "";
            }
            
            return s;
        }

        public static string PATH_GetAnyPath()
        {
            var tPath = "";

            // R4.00 Get a texture path from defined textures.
            if (!string.IsNullOrEmpty(frmMain.PATH_BackgroundImage))
            {
                tPath = PATH_StripFilename(frmMain.PATH_BackgroundImage);
            }
            else
            {
                if (!string.IsNullOrEmpty(frmMain.PATH_Note01_Bmp))
                {
                    tPath = PATH_StripFilename(frmMain.PATH_Note01_Bmp);
                }

                if (!string.IsNullOrEmpty(frmMain.PATH_Note02_Bmp))
                {
                    tPath = PATH_StripFilename(frmMain.PATH_Note02_Bmp);
                }

                if (!string.IsNullOrEmpty(frmMain.PATH_Note03_Bmp))
                {
                    tPath = PATH_StripFilename(frmMain.PATH_Note03_Bmp);
                }

                if (!string.IsNullOrEmpty(frmMain.PATH_Note04_Bmp))
                {
                    tPath = PATH_StripFilename(frmMain.PATH_Note04_Bmp);
                }
            }
            
            return tPath;
        }


        public static object STRING_FindLastSlash(string a)
        {
            int i;
            var hit = 0;
            for (i = Strings.Len(a); i >= 1; i -= 1)
            {
                if (Strings.Mid(a, i, 1) == @"\")
                {
                    hit = i;
                    break;
                }
            }
            return hit;
        }
    }
}
