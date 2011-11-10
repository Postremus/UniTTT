using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public class Recursive : AbstractKI
    {
        #region Fields
        protected List<int> Wertungen;
        protected List<string> SitCodes;
        #endregion

        protected Recursive(int b, int h) : base('O', b, h)
        {
            Wertungen = new List<int>();
            SitCodes = new List<string>();
        }

        protected void Recursion(int tiefe, string sitcode, char spieler)
        {
            string momsitcodeedited;
            if ((tiefe == 0) || (pruefer.Pruefe(spieler, SitCodeHelper.ToBrett(sitcode, Breite, Hoehe))))
            {
                SitCodes.Add("END");

                if (pruefer.Pruefe(spieler, SitCodeHelper.ToBrett(sitcode, Breite, Hoehe)))
                    Wertungen.Add(spieler - 48);
                else
                    Wertungen.Add(1);
                return;
            }
            spieler = PlayerChange(spieler);
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

        protected int SelectBestZug(int[] felder, string momsitcode)
        {
            int zug = 0, count = int.MinValue + 1;
            for (int i = 0; i < FelderAnzahl; i++)
            {
                if ((momsitcode[i] == '1') && (felder[i] > count))
                {
                    count = felder[i];
                    zug = i;
                }
            }
            return zug;
        }
    }
}
