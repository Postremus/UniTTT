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
        public KI.AbstractKI KI { get; private set; }

        public enum KISystems
        {
            Reinforcement = 1,
            Recursion,
            MiniMax,
            Like,
            Random,
            Bot,
        }

        public KIPlayer(int kiZahl, int width, int height, char kispieler, IOutputDarsteller odarsteller) : base(kispieler)
        {
            if (odarsteller == null)
                throw new NullReferenceException();
            if (kiZahl == 1)
                KI = new KIReinforcement(odarsteller);
            else if (kiZahl == 2)
                KI = new KIRecursion(kispieler, width, height);
            else if (kiZahl == 3)
                KI = new KIMiniMax(width, height, kispieler);
            else if (kiZahl == 4)
                KI = new KILike(width, height);
            else if (kiZahl == 5)
                KI = new KIRandom(width, height);
            else if (kiZahl == 6)
                KI = new KIBot(width, height, kispieler);
        }

        public KIPlayer(string ki, int width, int height, char kispieler, IOutputDarsteller odarsteller) : base(kispieler)
        {
            if (odarsteller == null)
                throw new NullReferenceException();
            if (Enum.IsDefined(typeof(KISystems), ki))
            {
                int kiZahl = (int)Enum.Parse(typeof(KISystems), ki);

                if (kiZahl == 1)
                    KI = new KIReinforcement(odarsteller);
                else if (kiZahl == 2)
                    KI = new KIRecursion(kispieler, width, height);
                else if (kiZahl == 3)
                    KI = new KIMiniMax(width, height, kispieler);
                else if (kiZahl == 4)
                    KI = new KILike(width, height);
                else if (kiZahl == 5)
                    KI = new KIRandom(width, height);
                else if (kiZahl == 6)
                    KI = new KIBot(width, height, kispieler);
            }
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
            public KIReinforcement(IOutputDarsteller odar): base('O', 3, 3)
            {
                writerreader = new WriterReader("KI_Reinforcement");
                ODarsteller = odar;
            }

            #region Fields
            private WriterReader writerreader;
            private IOutputDarsteller ODarsteller;
            #endregion

            private int Rundefrage()
            {
                int ret = new int();
                do
                {
                    ODarsteller.ShowMessage("Wie viele Runden sollen durchlaufen werden? (als Zahl)");
                    if (int.TryParse(Console.ReadLine(), out ret))
                    {
                        if (ret < 0)
                        {
                            ODarsteller.ShowMessage("Zahl zu klein.");
                        }
                        else
                        {
                            return ret;
                        }
                    }
                    else
                    {
                        ODarsteller.ShowMessage("Irgendetwas wurde falsch eingegeben.");
                        ODarsteller.ShowMessage("Eventuell eine Lerrzeichen, oder ein anderes nicht Zahl Zeichen");
                        ODarsteller.ShowMessage("Taste drücken für einen neuen Versuch");
                        ODarsteller.Wait();
                        ODarsteller.Clear();
                    }
                } while (true);
            }

            // KI
            public void Learn()
            {
                #region Fields
                char player = '2';
                int runden = Rundefrage();
                int zug;
                string momsitcode = SitCodeHelper.SetEmpty(9);
                int[,] sit_codes = new int[(int)runden, 9];
                int[,] zuege = new int[(int)runden, 9];
                int[] wertungen = new int[(int)runden];
                bool gewonnen = false;
                #endregion
                ODarsteller.ShowMessage("Berechne Daten..");
                for (int currround = 0; currround < runden; currround++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        player = SitCodeHelper.PlayerChange(player);
                        sit_codes[currround, i] = int.Parse(momsitcode);
                        zug = SitCodeHelper.GetRandomZug(momsitcode);
                        zuege[currround, i] = zug;

                        momsitcode = momsitcode.Remove(zug, 1).Insert(zug, player.ToString());

                        gewonnen = Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(player), Fields.SitCode.GetInstance(momsitcode, Width, Height));
                        // Wertungen
                        // Aufwerten
                        if ((gewonnen) && (player != KIPlayer))
                            wertungen[currround] = 1;
                        else if ((gewonnen) && (player == KIPlayer))
                            wertungen[currround] = -1;
                        else if ((i == 8) && (!gewonnen))
                            wertungen[currround] = 0;

                        // Ist Spiel Zu Ende?
                        if ((gewonnen) || (i == 8))
                        {
                            momsitcode = SitCodeHelper.SetEmpty(9);
                            i = 9;
                            player = 'X';
                        }
                    }
                    if (currround % 100 == 0)
                    {
                        ODarsteller.ShowMessage("Spielrunde Nr." + currround);
                    }
                }
                ODarsteller.ShowMessage("Fertig mit dem Berechnen der Daten.");
                ODarsteller.ShowMessage("Speichere Daten");
                writerreader.Write(zuege, sit_codes, wertungen);
                ODarsteller.ShowMessage("Fertig, Taste drücken zum Beenden");
                ODarsteller.Wait();
            }

            public int Play(Fields.IField field, char spieler)
            {
                string sitcode = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field));
                int zug = writerreader.Read(sitcode);
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
                public string FileName { get; set; }
                public WriterReader(string filename)
                {
                    FileName = filename;
                }

                public void Write(int[,] Zuege, int[,] Sit_Code, int[] Wertung)
                {
                    BinaryWriter binwriter = new BinaryWriter(File.OpenWrite(FileName), Encoding.UTF8);
                    string towrite = null;
                    for (int x = 0; x < Wertung.Length; x++)
                    {
                        for (int i = 0; i < 9 && Sit_Code[x, i] != 0; i++)
                        {
                            // Sitcode field_id Wertung
                            towrite = string.Format(CultureInfo.CurrentCulture, "{0} {1} {2}", Sit_Code[x, i], Zuege[x, i], Wertung[x]);
                            binwriter.Write(towrite);
                        }
                    }
                    binwriter.Flush();
                    binwriter.Close();
                }

                private string[] lines = null;

                public int Read(string sitcode)
                {
                    int ret = -1;
                    if (File.Exists(FileName))
                    {
                        if (lines == null)
                        {
                            lines = File.ReadAllLines(FileName, Encoding.UTF8);
                        }
                        int[] fields = new int[9];
                        List<string> substrs;

                        foreach (string item in lines.Where<string>(f => f.Contains(sitcode)))
                        {
                            substrs = item.GetSubstrs();
                            fields[int.Parse(substrs[1])] += int.Parse(substrs[2]);
                        }
                        ret = fields.GetHighestIndex();
                    }
                    return ret;
                }
            }
        }

        class KIRecursion : KI.Recursive, KI.IPlayableKI, KI.ILearnableKI
        {
            #region Fields
            private WriterReader writerreader;
            #endregion

            public KIRecursion(char kispieler, int width, int height) : base(width, height) 
            {
                writerreader = new WriterReader("KI_Recursion");
            }

            public void Learn()
            {
                Console.WriteLine("Berechne Daten - Teil 1..");
                Recursion(Width * Height, SitCodeHelper.SetEmpty(Width * Height), '2');
                Console.WriteLine("Berechne Daten - Teil 2..");
                Recursion(Width * Height, SitCodeHelper.SetEmpty(Width * Height), '3');
                Console.WriteLine("Speicher Daten..");
                writerreader.Write(base.SitCodes, base.Wertungen);
                Console.WriteLine("Fertig, Taste drücken zum beenden");
                Console.ReadLine();
            }

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
                        mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, SitCodeHelper.PlayertoSitCode(spieler).ToString());
                        wertungen[i, 0] = writerreader.Read(Database.DB.ToVBLike(mom_sit_code_edited), '1'); // Unentschieden
                        wertungen[i, 1] = writerreader.Read(Database.DB.ToVBLike(mom_sit_code_edited), SitCodeHelper.PlayertoSitCode(spieler)); // Spieler Gewonnen
                        wertungen[i, 2] = writerreader.Read(Database.DB.ToVBLike(mom_sit_code_edited), SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(spieler))); // Gegner

                        Felder[i] = (wertungen[i, 0] + wertungen[i, 1]) - (wertungen[i, 2] * 5);
                    }
                }
                return Felder;
            }

            public override string ToString()
            {
                return "Recursion";
            }

            public class WriterReader
            {
                public WriterReader(string fname)
                {
                    FileName = fname;
                }

                public string FileName { get; private set; }

                public void Write(List<string> Sit_Code, List<int> Wertung)
                {
                    List<string> towrite = new List<string>();
                    string str = null;
                    int count = 0;
                    BinaryWriter binwriter = new BinaryWriter(File.OpenWrite(FileName), Encoding.UTF8);
                    for (int x = 0; x < Wertung.Count; x++, count++)
                    {
                        for (; (count < Sit_Code.Count) && (Sit_Code[count] != "END"); count++)
                        {
                            str = string.Format(CultureInfo.CurrentCulture, "{0} {1}", Sit_Code[count], Wertung[x]);
                            if (!towrite.Contains(str))
                            {
                                towrite.Add(str);
                                binwriter.Write(str);
                                binwriter.Write(Environment.NewLine);
                            }
                        }
                    }
                    binwriter.Flush();
                    binwriter.Close();
                }

                public int Read(string sitcode, char bedingung)
                {
                    string[] lines = File.ReadAllLines(FileName, Encoding.UTF8);
                    int ret = 0;
                    List<string> substrs;

                    foreach (string item in lines.Where<string>(f => f.Contains(bedingung)))
                    {
                        substrs = item.GetSubstrs();
                        if (Database.DB.Like(sitcode, substrs[0]))
                        {
                            ret++;
                        }
                    }
                    return ret;
                }
            }
        }

        class KILike : KI.Recursive, KI.IPlayableKI
        {

            public KILike(int width, int height)
                : base(width, height)
            {
                Recursion(Length, SitCodeHelper.SetEmpty(Length), '3');
                Recursion(Length, SitCodeHelper.SetEmpty(Length), '2');
            }

            // TODO: Überarbeiten
            public int Play(Fields.IField field, char spieler)
            {
                string mom_sit_code = SitCodeHelper.StringToSitCode(FieldHelper.Calculate(field)); 

                int[] Felder = WertungenBerechnen(mom_sit_code, spieler);
                return Felder.GetHighestIndex();
            }

            private int WertungenZugZuordnen(List<int> list, int bedingung)
            {
                int rt_int = 0, count = 0;
                for (int i = 0; i < Wertungen.Count; i++, count++)
                {
                    for (; count < (SitCodes.Count) && (SitCodes[count] != "END"); count++)
                    {
                        if (count == list[0])
                        {
                            list.RemoveAt(0);
                            if (Wertungen[i] == bedingung)
                    
                                rt_int++;
                            else
                                break;
                            if (list.Count == 0)
                            {
                                i = Wertungen.Count;
                                break;
                            }
                        }
                    }
                }
                return rt_int;
            }

            private int[] WertungenBerechnen(string mom_sit_code, char spieler)
            {
                int[] wertungen = new int[3];
                int[] Felder = new int[Length];
                string mom_sit_code_edited = mom_sit_code;

                for (int i = 0; i < Length; i++)
                {
                    if (mom_sit_code[i] == '1')
                    {
                        mom_sit_code_edited = mom_sit_code.Remove(i, 1).Insert(i, SitCodeHelper.PlayertoSitCode(spieler).ToString());
                        wertungen[0] = WertungenZugZuordnen(Database.DB.Like(SitCodes, Database.DB.ToVBLike(mom_sit_code_edited)), '1' - 48); // unentschieden
                        wertungen[1] = WertungenZugZuordnen(Database.DB.Like(SitCodes, Database.DB.ToVBLike(mom_sit_code_edited)), SitCodeHelper.PlayertoSitCode(spieler) - 48); // Spieler Gewonnen
                        wertungen[2] = WertungenZugZuordnen(Database.DB.Like(SitCodes, Database.DB.ToVBLike(mom_sit_code_edited)), SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(spieler)) - 48); // Gegner

                        Felder[i] = (wertungen[0] + wertungen[1]) - (wertungen[2] * 5);
                    }
                }
                return Felder;
            }

            public override string ToString()
            {
                return "Like";
            }
        }

        class KIMiniMax : KI.AbstractKI, KI.IPlayableKI
        {
            public KIMiniMax(int width, int height, char spieler) : base(spieler, width, height) { }

            private int bestZug;

            public int Play(Fields.IField field, char spieler)
            {
                Max(field.Length - 1, field);
                return bestZug;
            }

            private int Max(int depth, Fields.IField field)
            {
                int zugValue = 0;
                int discovered = int.MinValue + 1;
                for (int i = 0; i < field.Length; i++)
                {
                    if (field.IsFieldEmpty(i))
                    {
                        field.SetField(i, HumanPlayer);
                        if (depth <= 1)
                        {
                            zugValue = Valuation(field, HumanPlayer);
                        }
                        else
                        {
                            zugValue = Mini(depth - 1, field);
                        }
                        field.SetField(i, ' ');
                        if (zugValue > discovered)
                        {
                            discovered = zugValue;
                        }
                    }
                }
                return discovered;
            }

            private int Mini(int depth, Fields.IField field)
            {
                int zugValue = 0;
                int discovered = int.MaxValue -1;
                for (int i = 0; i < field.Length; i++)
                {
                    if (field.IsFieldEmpty(i))
                    {
                        field.SetField(i, KIPlayer);
                        if (depth <= 1)
                        {
                            zugValue = Valuation(field, KIPlayer);
                        }
                        else
                        {
                            zugValue = Max(depth - 1, field);
                        }
                        field.SetField(i, ' ');
                        if (zugValue < discovered)
                        {
                            discovered = zugValue;
                            bestZug = i;
                        }
                    }
                }
                return discovered;
            }

            private int Valuation(Fields.IField field, char player)
            {
                List<Fields.FieldRegion> fPanel = field.Panels;
                int ret = 0;
                foreach (Fields.FieldRegion region in fPanel)
                {
                    int xCount = 0;
                    int oCount = 0;
                    foreach (Fields.FieldPlaceData placeData in region)
                    {
                        if (placeData.FieldValue == 'X')
                        {
                            xCount++;
                        }
                        else if (placeData.FieldValue == 'O')
                        {
                            xCount++;
                        }
                    }
                    int multi = 1;
                    if (xCount == 0)
                    {
                        if (player == HumanPlayer)
                            multi = 3;
                        ret += -(int)Math.Pow(10, oCount) * multi;
                    }
                    else if (oCount == 0)
                    {
                        if (player == KIPlayer)
                            multi = 3;
                        ret += (int)Math.Pow(10, xCount) * multi;
                    }
                    else
                    {
                        ret += 0;
                    }
                }
                return ret;
            }

            private char PlayerChange(char player)
            {
                return SitCodeHelper.ToPlayer(SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(player)));
            }

            public override string ToString()
            {
                return "MiniMax";
            }
        }

        class KIBot : KI.AbstractKI, KI.IPlayableKI
        {
            private Fields.IField field;

            public KIBot(int width, int height, char spieler) : base(spieler, width, height) { }

            public int Play(Fields.IField field, char spieler)
            {
                this.field = field;
                int win_zug = TestForOneWin();
                int block_zug = TestForHumanBlock();
                int set_zug = TestForBestPosition();

                if (win_zug != -1)
                    return win_zug;
                else if (block_zug != -1)
                    return block_zug;
                else if (set_zug != -1)
                    return set_zug;
                else
                    return SitCodeHelper.GetRandomZug(SitCodeHelper.StringToSitCode(field.ToString()));
            }

            private int TestForOneWin()
            {
                int win_zug = -1;
                for (int playerpos = 0; (playerpos < field.Length) && (win_zug == -1); playerpos++)
                {
                    if (field.IsFieldEmpty(playerpos))
                    {
                        field.SetField(playerpos, KIPlayer);
                        if ((Logik.WinChecker.Pruefe(KIPlayer, field)) && (win_zug == -1))
                            win_zug = playerpos;
                        field.SetField(playerpos, ' ');
                    }
                }
                return win_zug;
            }

            private int TestForHumanBlock()
            {
                int block_zug = -1;
                char humanplayer = SitCodeHelper.PlayerChange(SitCodeHelper.PlayertoSitCode(KIPlayer));
                for (int playerpos = 0; (playerpos < field.Length) && (block_zug == -1); playerpos++)
                {
                    if (field.IsFieldEmpty(playerpos))
                    {
                        field.SetField(playerpos, humanplayer);
                        if ((Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(humanplayer), field)) && (block_zug == -1))
                            block_zug = playerpos;
                        field.SetField(playerpos, ' ');
                    }
                }
                return block_zug;
            }

            private int TestForBestPosition()
            {
                int[] posis = new int[Width * Height];
                for (int x = 0; x < Width; x++)
                {
                    if (WinChecker.DoCheck(field, WinChecker.Directories.Down, ' ', new Vector2i(x, 0), Vector2i.Zero) == Width)
                    {
                        posis[(x + 1) * (0 + 1) - 1]++;
                    }
                }
                for (int y = 0; y < Height; y++)
                {
                    if (WinChecker.DoCheck(field, WinChecker.Directories.Right, ' ', new Vector2i(0, y), Vector2i.Zero) == Height)
                    {
                        posis[(0 + 1) * (y + 1) - 1]++;
                    }
                }
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (x + (Width - 1) < Width && y + (Width - 1) < Height)
                        {
                            if (WinChecker.DoCheck(field, WinChecker.Directories.RightDown, ' ', new Vector2i(x, y), Vector2i.Zero) == Width)
                            {
                                posis[(x + 1) * (y + 1) - 1]++;
                            }
                        }
                    }
                }
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (WinChecker.DoCheck(field, WinChecker.Directories.LeftDown, ' ', new Vector2i(x, y), new Vector2i(x + (Height - 1), y + (Height - 1))) == Width)
                        {
                            posis[(x + 1) * (y + 1) - 1]++;
                        }
                    }
                }
                int ret = -1;
                ret = posis.GetHighestIndex();
                return ret;
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