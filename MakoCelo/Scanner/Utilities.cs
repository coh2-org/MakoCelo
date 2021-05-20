using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;

namespace MakoCelo.Scanner
{
    public static class Utilities
    {
        public static string FindPlayerRelicIdInLine(string a)
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
                rid = (long)Math.Round(Conversion.Val(Strings.Mid(a, charStart, charEnd - charStart)));

            return rid.ToString();
        }

        /// <summary>
        /// Names are not Delimited, need to search for end of name from the end of line.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="charStart"></param>
        /// <returns></returns>
        public static string FindPlayerNameInLine(string a, int charStart)
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
