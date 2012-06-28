using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.AI
{
    public class AbstractAI
    {
        public event GetIntHandler GetIntEvent;
        public event GetStringHandler GetStringEvent;
        public event ShowMessageHandler ShowMessageEvent;

        protected AbstractAI(char aiPlayer, int width, int height)
        {
            AIPlayer = aiPlayer;
            HumanPlayer = Player.Player.PlayerChange(aiPlayer);
            Width = width;
            Height = height;
        }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public int Length { get { return Width * Height; } }
        public char AIPlayer { get; private set; }
        public char HumanPlayer { get; private set; }

        protected int Valuation(Fields.Field field, char player)
        {
            List<Fields.FieldRegion> fPanel = field.Panels;
            int ret = 0;
            foreach (Fields.FieldRegion region in fPanel)
            {
                int humanCount = 0;
                int aicount = 0;
                foreach (Fields.FieldPlaceData placeData in region)
                {
                    if (placeData.FieldValue == HumanPlayer)
                    {
                        humanCount++;
                    }
                    else if (placeData.FieldValue == AIPlayer)
                    {
                        aicount++;
                    }
                }

                int multi = 1;
                if (humanCount == 0)
                {
                    if (player == HumanPlayer)
                        multi = 3;
                    ret += -Pow(10, aicount) * multi;
                }
                else if (aicount == 0)
                {
                    if (player == AIPlayer)
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

        public override string ToString()
        {
            return "AI";
        }
    }
}