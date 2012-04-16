using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;

namespace UniTTT.Logik.Player
{
    public class KIPlayer : AbstractPlayer
    {
        public KI.AbstractKI KI { get; private set; }
        private Type[] KITypes;

        public KIPlayer(int kiZahl, int width, int height, char kispieler) : base(kispieler)
        {
            Initialize(kiZahl, width, height, kispieler);
        }

        public KIPlayer(string ki, int width, int height, char kispieler) : base(kispieler)
        {
            GetKITypes();
            for (int i = 0; i < KITypes.Length; i++)
            {
                if (KITypes[i].Name.ToLower() == ki.ToLower())
                {
                    Initialize(i, width, height, kispieler);
                }
            }
        }

        private void Initialize(int kiZahl, int width, int height, char kispieler)
        {
            GetKITypes();
            if (!Directory.Exists("data/scripts/ki"))
            {
                Directory.CreateDirectory("data/scripts/ki");
            }
            CompileScripts(Directory.GetFiles("data/scripts/ki"));
            KI = (Logik.KI.AbstractKI)Activator.CreateInstance(KITypes[kiZahl - 1], new object[] { width, height, kispieler });
        }

        private void GetKITypes()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            KITypes = asm.GetTypes().Where<Type>(t => t.IsSubclassOf(typeof(KI.AbstractKI))).ToArray();
        }

        private void CompileScripts(string[] scripts)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameter = new CompilerParameters();
            parameter.ReferencedAssemblies.Add("UniTTT.Logik.dll");
            parameter.CompilerOptions = "/t:library";
            parameter.GenerateInMemory = true;

            Type[] types = new Type[scripts.Length];

            for (int i = 0; i < scripts.Length; i++)
            {
                if (File.Exists(scripts[i]))
                {
                    CompilerResults result = provider.CompileAssemblyFromFile(parameter, scripts[i]);
                    if (result.CompiledAssembly.GetTypes().Count(t => t.IsSubclassOf(typeof(KI.AbstractKI))) == 1)
                    {
                        types[i] = result.CompiledAssembly.GetTypes()[0];
                    }
                }
            }
            Array.Resize(ref KITypes, KITypes.Length + types.Length);
            Array.Copy(types, KITypes, types.Length);
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
            public KIReinforcement(int width, int height, char kispieler) : base(kispieler, width, height)
            {
                writerreader = new WriterReader("KI_Reinforcement");
            }

            #region Fields
            private WriterReader writerreader;
            #endregion

            private int Rundefrage()
            {
                int ret = new int();
                do
                {
                    Console.WriteLine("Wie viele Runden sollen durchlaufen werden? (als Zahl)");
                    if (int.TryParse(Console.ReadLine(), out ret))
                    {
                        if (ret < 0)
                        {
                            Console.WriteLine("Zahl zu klein.");
                        }
                        else
                        {
                            return ret;
                        }
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
                int runden = Rundefrage();
                int zug;
                string momsitcode = SitCodeHelper.SetEmpty(9);
                int[,] sit_codes = new int[(int)runden, 9];
                int[,] zuege = new int[(int)runden, 9];
                int[] wertungen = new int[(int)runden];
                bool gewonnen = false;
                #endregion
                Console.WriteLine("Berechne Daten..");
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
                        Console.WriteLine("Spielrunde Nr." + currround);
                    }
                }
                Console.WriteLine("Fertig mit dem Berechnen der Daten.");
                Console.WriteLine("Speichere Daten");
                writerreader.Write(zuege, sit_codes, wertungen);
                Console.WriteLine("Fertig, Taste drücken zum Beenden");
                Console.ReadLine();
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
                    FileName = "data/" + filename;
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
                for (int playerpos = 0; (playerpos < field.Length) && (block_zug == -1); playerpos++)
                {
                    if (field.IsFieldEmpty(playerpos))
                    {
                        field.SetField(playerpos, HumanPlayer);
                        if ((Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(HumanPlayer), field)) && (block_zug == -1))
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
            public KIRandom(int width, int height, char spieler) : base(spieler, width, height) { }

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
                int discovered = int.MaxValue - 1;
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
                    else
                    {
                        ret += 0;
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

            public override string ToString()
            {
                return "MiniMax";
            }
        }

        class KIMiniMaxAlphaBeta : KI.AbstractKI, KI.IPlayableKI
        {
            public KIMiniMaxAlphaBeta(int width, int height, char spieler) : base(spieler, width, height) { }

            private int bestZug;

            public int Play(Fields.IField field, char spieler)
            {
                Max(field.Length - 1, int.MaxValue - 1, int.MinValue + 1, field);
                return bestZug;
            }

            private int Max(int depth, int alpha, int beta, Fields.IField field)
            {
                if (depth == 0)
                {
                    return Valuation(field, HumanPlayer);
                }
                int localAlpha = int.MinValue + 1;
                for (int i = 0; i < field.Length; i++)
                {
                    if (field.IsFieldEmpty(i))
                    {
                        field.SetField(i, HumanPlayer);
                        int wert = Mini(depth - 1, alpha, beta, field);
                        field.SetField(i, ' ');
                        if (wert > localAlpha)
                        {
                            if (wert > beta)
                            {
                                bestZug = i;
                                return wert;
                            }
                            localAlpha = wert;
                            if (wert > alpha)
                            {
                                alpha = wert;
                            }
                        }
                    }
                }
                return localAlpha;
            }

            private int Mini(int depth, int alpha, int beta, Fields.IField field)
            {
                if (depth == 0)
                {
                    return Valuation(field, KIPlayer);
                }
                int localBeta = int.MaxValue - 1;
                for (int i = 0; i < field.Length; i++)
                {
                    if (field.IsFieldEmpty(i))
                    {
                        field.SetField(i, KIPlayer);
                        int wert = Max(depth, alpha, beta, field);
                        field.SetField(i, ' ');
                        if (wert < localBeta)
                        {
                            if (wert < alpha)
                            {
                                bestZug = i;
                                return wert;
                            }
                            localBeta = wert;
                            if (wert < beta)
                            {
                                beta = wert;
                            }
                        }
                    }
                }
                return localBeta;
            }

            private int Valuation(Fields.IField field, char player)
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
                    else
                    {
                        ret += 0;
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

            public override string ToString()
            {
                return "MiniMax";
            }
        }
    }
}