using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class Recursive : AbstractKI
    {
        #region Fields
        protected List<int> Wertungen { get; private set; }
        protected List<string> SitCodes { get; private set; }
        protected Dictionary<string, FieldHelper.GameStates> dic { get; set; }
        #endregion

        protected Recursive(int width, int height) : base('O', width, height)
        {
            SitCodes = new List<string>();
            Wertungen = new List<int>();
        }

        protected void Recursion(int tiefe, string sitcode, char spieler)
        {
            string momsitcodeedited;
            if ((tiefe == 0) || (Logik.WinChecker.Pruefe(spieler, Fields.SitCode.GetInstance(sitcode, Width, Height))))
            {
                SitCodes.Add("END");

                if (Logik.WinChecker.Pruefe(spieler, Fields.SitCode.GetInstance(sitcode, Width, Height)))
                    Wertungen.Add(spieler - 48);
                else
                    Wertungen.Add(1);
                return;
            }
            spieler = SitCodeHelper.PlayerChange(spieler);
            for (int i = 0; i < 9; i++)
            {
                if (sitcode[i] == '1')
                {
                    momsitcodeedited = sitcode;
                    momsitcodeedited = momsitcodeedited.Remove(i, 1).Insert(i, spieler.ToString());
                    SitCodes.Add(momsitcodeedited);
                    Recursion(tiefe - 1, momsitcodeedited, spieler);
                }
            }
        }

        protected void Recursion2(int depth, string sitCode, char spieler)
        {
            string momSitCode;
            FieldHelper.GameStates state = FieldHelper.GetGameState(Fields.SitCode.GetInstance(sitCode, Width, Height), spieler);
            if ((depth == 0) || state != FieldHelper.GameStates.Laufend)
            {
                dic.Add(sitCode, state);
            }
        }
    }
}