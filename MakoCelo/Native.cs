using System;
using System.Runtime.InteropServices;

namespace MakoCelo
{
    public static class Native
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, int dwVolume);
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    }
}
