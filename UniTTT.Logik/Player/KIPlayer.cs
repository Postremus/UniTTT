using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Player
{
    public class KIPlayer : AbstractPlayer
    {
        public KI.IKI KI { get; private set; }

        public KIPlayer(int kiZahl, int width, int height, char kispieler) : base(kispieler)
        {
            if (kiZahl == 1)
                KI = new KIReinforcement();
            //else if (kiZahl == 2)
                //KI = new KIRecursion(kispieler, width, height);
            else if (kiZahl == 3)
                KI = new KIMiniMax(width, height, kispieler);
            //else if (kiZahl == 4)
            //    KI = new KILike(width, height);
            else if (kiZahl == 5)
                KI = new KIRandom(width, height);
            else if (kiZahl == 6)
                KI = new KIBot(width, height, kispieler);
        }

        public override Vector2i Play(Fields.IField field)
        {
            if (KI is KI.IPlayableKI)
            {
                return Vector2i.IndexToVector(((UniTTT.Logik.KI.IPlayableKI)KI).Play(field, Symbol), field.Width, field.Height);
            }
            else
                return new Vector2i(-1, -1);
        }

        public override void Learn()
        {
            if (KI is KI.ILearnableKI)
            {
                ((Logik.KI.ILearnableKI)KI).Learn();
            }
        }

        public override string ToString()
        {
            return KI.ToString();
        }

        class KIReinforcement : KI.AbstractKI, KI.IPlayableKI, KI.ILearnableKI
        {
            public KIReinforcement(): base('O', 3, 3)
            {
                db = new WriterReader("KI_Reinforcement");
            }

            #region Fields
            private WriterReader db;
            #endregion

            private int Rundefrage()
            {
                int ret = new int();
                do
                {
                    Console.WriteLine("Wie viele Runden sollen durchlaufen werden? (als Zahl)");
                    if (int.TryParse(Console.ReadLine(), out ret))
                    {
                        return ret;
                    }
                    else
                    {
                        Console.WriteLine("Irgendetwas wurde falsch eingegeben.");
                        Console.WriteLine("Eventuell eine Lerrzeichen, oder ein anderes nicht Zahl Zeichen");
                        Console.WriteLine("Taste drücken für einen neuen Versuch");
                        Console.ReadLine();
                        Console.Clear();
                    }
                } while (true);
            }

            // KI
            public void Learn()
            {
                #region Fields
                char player = '2';
                int runden = Rundefrage(), zug;
                string momsitcode = SitCodeHelper.SetEmpty(9);
                string[,] sit_codes = new string[runden, 9];
                int[,] zuege = new int[runden, 9];
                int[] wertungen = new int[runden];
                bool gewonnen;
                #endregion

                for (int a = 0; a < runden; a++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        player = SitCodeHelper.PlayerChange(player);
                        sit_codes[a, i] = momsitcode;
                        zug = SitCodeHelper.GetRandomZug(momsitcode);
                        zuege[a, i] = zug;

                        momsitcode = momsitcode.Remove(zug, 1).Insert(zug, player.ToString());

                        gewonnen = Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(player), Fields.SitCode.GetInstance(momsitcode, Width, Height));
                        //gewonnen = Logik.WinCheckerTest.Test(momsitcode, player);
                        // Wertungen
                        // Aufwerten
                        if ((gewonnen))
                            wertungen[a] = 2;
                        else if ((i == 8) && (!gewonnen))
                            wertungen[a] = 1;

                        // Ist Spiel Zu Ende?
                        if ((gewonnen) || (i == 8))
                        {
                            momsitcode = SitCodeHelper.SetEmpty(9);
                            i = 9;
                            player = 'X';
                        }
                    }
                }
                db.Zuege = zuege;
                db.Sit_Code = sit_codes;
                db.Wertung = wertungen;
                db.Write();
            }

            public int Play(Fields.IField field, char spieler)
            {
                string sitcode = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field));
                int zug = db.Read(sitcode);
                if (zug == -1)
                    zug = SitCodeHelper.GetRandomZug(sitcode);
                return zug;
            }

            public override string ToString()
            {
                return "Reinforcement";
            }

            public class WriterReader
            {
                public int[,] Zuege;
                public string[,] Sit_Code;
                public int[] Wertung;
                public string FileName { get; set; }
                public WriterReader(string filename)
                {
                    FileName = filename;
                }

                public void Write()
                {
                    BinaryWriter binwriter = new BinaryWriter(File.OpenWrite(FileName));
                    string towrite = null;
                    for (int x = 0; x < Wertung.Length; x++)
                    {
                        runde_ausgabe(x);
                        for (int i = 0; i < 9 && !string.IsNullOrEmpty(Sit_Code[x, i]); i++)
                        {
                            // Sitcode field_id Wertung
                            towrite = string.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", Sit_Code[x, i], Zuege[x, i], Wertung[x]);
                            binwriter.Write(towrite);
                        }
                    }
                }

                public int Read(string sitcode)
                {
                    string[] lines = File.ReadAllLines(FileName, Encoding.Default);
                    int[] fields = new int[9];
                    List<string> substrs;

                    foreach (string item in lines.Where<string>(f => f.Contains(sitcode)))
                    {
                        substrs = GetSubstrs(item, ' ');
                        fields[int.Parse(substrs[1])] += int.Parse(substrs[2]);
                    }

                    int ret = -1;
                    int idx = 0;
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (fields[i] > fields[idx])
                        {
                            ret = idx = i;
                        }
                    }
                    return ret;
                }

                private List<string> GetSubstrs(string value, char seperator)
                {
                    List<string> ret = new List<string>();
                    int lastidx = 0;
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (value[i] == seperator)
                        {
                            ret.Add(value.Substring(lastidx, i - lastidx));
                            lastidx = i;
                        }
                    }
                    ret.Add(value.Substring(lastidx, value.Length - lastidx));
                    return ret;
                }

                private void runde_ausgabe(int runde)
                {
                    if ((runde + 1) % 500 == 0)
                    {
                        int x = runde + 1;
                        OutputManager.Add(x.ToString());
                    }
                }
            }
        }

        class KIRecursion : KI.Recursive, KI.IPlayableKI
        {
            #region Fields
            #endregion

            public KIRecursion(char kispieler, int width, int height) : base(width, height) { }

            // TODO: Überarbeiten
            public int Play(Fields.IField field, char spieler)
            {
                int[] Felder = new int[Length];
                string mom_sit_code = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field));
                Felder = WertungenBerechnen(mom_sit_code, spieler);

                return SelectBestZug(Felder, mom_sit_code);
            }

            private int[] WertungenBerechnen(string mom_sit_code, char spieler)
            {
                int[,] wertungen = new int[Length, 3];
                int[] Felder = new int[Length];
                string mom_sit_code_edited = mom_sit_code;

                for (int i = 0; i < Length; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        //mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, SitCodeHelper.PlayertoSitCode(spieler).ToString());
                        //wertungen[i, 0] = db.Lesen(Database.DB.ToDBLike(mom_sit_code_edited), '1', "Felder_" + Length); // Unentschieden
                        //wertungen[i, 1] = db.Lesen(Database.DB.ToDBLike(mom_sit_code_edited), SitCodeHelper.PlayertoSitCode(spieler), "Felder_" + Length); // Spieler Gewonnen
                        //wertungen[i, 2] = db.Lesen(Database.DB.ToDBLike(mom_sit_code_edited), SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(spieler)), "Felder_" + Length); // Gegner

                        Felder[i] = (wertungen[i, 0] + wertungen[i, 1]) - (wertungen[i, 2] * 5);
                    }
                }
                return Felder;
            }

            public override string ToString()
            {
                return "Recursion";
            }
        }

        class KIMiniMax : KI.AbstractKI, KI.IPlayableKI
        {
            public KIMiniMax(int width, int height, char spieler) : base(spieler, width, height) { }

            public int Play(Fields.IField field, char spieler)
            {
                string mom_sit_code = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field));
                double[] Felder = ZugWertungBerechnen(mom_sit_code, SitCodeHelper.PlayertoSitCode(spieler));
                return BestenZugAuswaehlen(Felder, mom_sit_code);
            }

            private int BestenZugAuswaehlen(double[] Felder, string mom_sit_code)
            {
                int zug = 0;
                double count = double.MinValue + 0.1;
                for (int i = 0; i < Length; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        if (Felder[i] > count)
                        {
                            count = Felder[i];
                            zug = i;
                        }
                    }
                }
                return zug;
            }

            private double[] ZugWertungBerechnen(string mom_sit_code, char spieler)
            {
                string mom_sit_code_edited;
                double[] Felder = new double[Length];
                double[] Feldertmp = new double[Length];
                double[,] wertungen = new double[Length, 3];
                for (int i = 0; i < Length; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, spieler.ToString());
                        wertungen[i, 0] = Bewertung(mom_sit_code_edited, i, '1'); // Unentschieden
                        wertungen[i, 1] = Bewertung(mom_sit_code_edited, i, KIPlayer); // KISpieler Gewonnen
                        wertungen[i, 2] = Bewertung(mom_sit_code_edited, i, SitCodeHelper.PlayerChange(KIPlayer)); // MenschGegner Gewonnen

                        Felder[i] = (wertungen[i, 0] * 20.0) + (wertungen[i, 0] * 10.0) - (wertungen[i, 2] * 50.0);
                        if (Felder[i] == 0.0)
                        {
                            Feldertmp = ZugWertungBerechnen(mom_sit_code_edited, SitCodeHelper.PlayerChange(spieler));
                            for (int y = 0; y < Length; y++)
                                Felder[i] += Feldertmp[y];
                        }
                        else
                            return Felder;
                    }
                }
                return Felder;
            }

            private double Bewertung(string sit_code, int x, char spieler)
            {
                double wertung = 0.0;
                FieldHelper.GameStates state = FieldHelper.GetGameState(Fields.SitCode.GetInstance(sit_code, Width, Height), spieler);

                if (state == FieldHelper.GameStates.Gewonnen)
                    wertung = 10.0 / Convert.ToDouble(x, System.Globalization.CultureInfo.InvariantCulture);
                else if (state == FieldHelper.GameStates.Unentschieden)
                    wertung = Convert.ToDouble(x, System.Globalization.CultureInfo.InvariantCulture);
                return wertung;
            }

            public override string ToString()
            {
                return "MiniMax";
            }
        }

        class KIBot : KI.AbstractKI, KI.IPlayableKI
        {
            public KIBot(int width, int height, char spieler) : base(spieler, width, height) { }

            public int Play(Fields.IField field, char spieler)
            {
                string sitcode = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field));
                int win_zug = TestForOneWin(sitcode);
                int block_zug = TestForHumanBlock(sitcode);
                int zug = SitCodeHelper.GetRandomZug(sitcode);

                if (win_zug != -1)
                    return win_zug;
                else if (block_zug != -1)
                    return block_zug;
                else
                    return zug;
            }

            private int TestForOneWin(string sitcode)
            {
                string momsitcode;
                int win_zug = -1;
                for (int playerpos = 0; (playerpos < sitcode.Length) && (win_zug == -1); playerpos++)
                {
                    if (sitcode[playerpos] == '1')
                    {
                        momsitcode = sitcode.Remove(playerpos, 1).Insert(playerpos, SitCodeHelper.PlayertoSitCode(KIPlayer).ToString());
                        if ((Logik.WinChecker.Pruefe(SitCodeHelper.PlayertoSitCode(KIPlayer), Fields.SitCode.GetInstance(momsitcode, Width, Height))) && (win_zug == -1))
                            win_zug = playerpos;
                    }
                }
                return win_zug;
            }

            private int TestForHumanBlock(string sitcode)
            {
                string momsitcode;
                int block_zug = -1;
                char humanplayer = SitCodeHelper.PlayertoSitCode(KIPlayer) == '3' ? '2' : '3';
                for (int playerpos = 0; (playerpos < sitcode.Length) && (block_zug == -1); playerpos++)
                {
                    if (sitcode[playerpos] == '1')
                    {
                        momsitcode = sitcode.Remove(playerpos, 1).Insert(playerpos, humanplayer.ToString());
                        if ((Logik.WinChecker.Pruefe(humanplayer, Fields.SitCode.GetInstance(momsitcode, Width, Height))) && (block_zug == -1))
                            block_zug = playerpos;
                    }
                }
                return block_zug;
            }

            private int TestForBestPosition(string sitcode)
            {
                int best_zug;
                
                return 0;
            }

            public override string ToString()
            {
                return "Bot";
            }
        }

        class KIRandom : KI.AbstractKI, KI.IPlayableKI
        {
            public KIRandom(int width, int height) : base('O', width, height) { }

            public int Play(Fields.IField field, char spieler)
            {
                string sitcode = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field));
                return SitCodeHelper.GetRandomZug(sitcode);
            }

            public override string ToString()
            {
                return "Random";
            }
        }
    }
}