using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class AbstractKI
    {
        public event GetIntHandler GetIntEvent;
        public event GetStringHandler GetStringEvent;
        public event ShowMessageHandler ShowMessageEvent;

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

        protected int Valuation(Fields.IField field, char player)
        {
            List<Fields.FieldRegion> fPanel = field.Panels;
            int ret = 0;
            foreach (Fields.FieldRegion region in fPanel)
            {
                int humanCount = 0;
                int kiCount = 0;
                foreach (Fields.FieldPlaceData placeData in region)
                {
                    if (placeData.FieldValue == HumanPlayer)
                    {
                        humanCount++;
                    }
                    else if (placeData.FieldValue == KIPlayer)
                    {
                        kiCount++;
                    }
                }

                int multi = 1;
                if (humanCount == 0)
                {
                    if (player == HumanPlayer)
                        multi = 3;
                    ret += -Pow(10, kiCount) * multi;
                }
                else if (kiCount == 0)
                {
                    if (player == KIPlayer)
                        multi = 3;
                    ret += Pow(10, humanCount) * multi;
                }
            }
            return ret;
        }

        private int Pow(int x, int y)
        {
            if (y == 0)
                return 0;
            int ret = 1;
            for (int i = 0; i < y; i++)
            {
                ret *= x;
            }
            return ret;
        }

        public int OnGetIntEvent()
        {
            int ret = -1;
            GetIntHandler getIntEvent = GetIntEvent;
            if (getIntEvent != null)
            {
                ret = getIntEvent();
            }
            return ret;
        }

        public string OnGetStringEvent()
        {
            string ret = null;
            GetStringHandler getStringEvent = GetStringEvent;
            if (getStringEvent != null)
            {
                ret = getStringEvent();
            }
            return ret;
        }

        public void OnShowMessageEvent(string message)
        {
            ShowMessageHandler showMessageEvent = ShowMessageEvent;
            if (showMessageEvent != null)
            {
                showMessageEvent(message);
            }
        }
    }
}