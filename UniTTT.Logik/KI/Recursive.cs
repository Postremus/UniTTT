//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace UniTTT.Logik.KI
//{
//    public class Recursive : AbstractKI
//    {
//        #region Fields
//        protected Dictionary<string, int> dic { get; private set; }
//        #endregion

//        protected Recursive(int width, int height) : base('O', width, height)
//        {
//            dic = new Dictionary<string, int>();
//        }
        
//        protected void Recursion(int tiefe, string sitcode, char spieler)
//        {
//            string momsitcodeedited;
//            if ((tiefe == 0) || (Logik.WinChecker.Pruefe(spieler, Fields.SitCode.GetInstance(sitcode, Width, Height))))
//            {
//                SitCodes.Add("END");

//                if (Logik.WinChecker.Pruefe(spieler, Fields.SitCode.GetInstance(sitcode, Width, Height)))
//                    Wertungen.Add(spieler - 48);
//                else
//                    Wertungen.Add(1);
//                return;
//            }
//            spieler = SitCodeHelper.PlayerChange(spieler);
//            for (int i = 0; i < 9; i++)
//            {
//                if (sitcode[i] == '1')
//                {
//                    momsitcodeedited = sitcode;
//                    momsitcodeedited = momsitcodeedited.Remove(i, 1).Insert(i, spieler.ToString());
//                    SitCodes.Add(momsitcodeedited);
//                    Recursion(tiefe - 1, momsitcodeedited, spieler);
//                }
//            }
//        }

//        protected void ShrinkLists()
//        {
//            List<int> wertungen_tmp = new List<int>();
//            List<string> sitcodes_tmp = new List<string>();
//        }

//        protected int SelectBestZug(int[] felder, string momsitcode)
//        {
//            int zug = 0, count = int.MinValue + 1;
//            for (int i = 0; i < Length; i++)
//            {
//                if ((momsitcode[i] == '1') && (felder[i] > count))
//                {
//                    count = felder[i];
//                    zug = i;
//                }
//            }
//            return zug;
//        }
//    }
//}