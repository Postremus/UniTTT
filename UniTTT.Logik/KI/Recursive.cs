﻿using System;
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
    }
}