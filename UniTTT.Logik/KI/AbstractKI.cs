using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class AbstractKI
    {
        protected AbstractKI(char kispieler, int width, int height)
        {
            KIPlayer = kispieler;
            HumanPlayer = SitCodeHelper.ToPlayer(SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(KIPlayer)));
            Width = width;
            Height = height;
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Length { get { return Width * Height; } }
        public char KIPlayer { get; private set; }
        public char HumanPlayer { get; private set; }

        protected int SelectBestZug(int[] felder, string momsitcode)
        {
            int idx = 0;
            for (int i = 0; i < Length; i++)
            {
                if ((momsitcode[i] == '1') && (felder[i] > felder[idx]))
                {
                    idx = i;
                }
            }
            return idx;
        }

        public override string ToString()
        {
            return "KI";
        }
    }
}