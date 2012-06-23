using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom.Compiler;
using System.IO.Compression;

namespace UniTTT.Logik.Player
{
    public class AIPlayer : Player
    {
        public AI.AbstractAI AI { get; private set; }
        private List<Type> AITypes;

        public AIPlayer(int aiZahl, int width, int height, char aiPlayer) : base(aiPlayer)
        {
            GetAITypes();
            Initialize(aiZahl -1, width, height, aiPlayer);
        }

        public AIPlayer(string ai, int width, int height, char aiPlayer)
            : base(aiPlayer)
        {
            GetAITypes();
            for (int i = 0; i < AITypes.Count; i++)
            {
                if (AITypes[i].Name.ToLower() == ai.ToLower())
                {
                    Initialize(i, width, height, aiPlayer);
                }
            }
        }

        private void Initialize(int aiZahl, int width, int height, char aiPlayer)
        {
            AI = (Logik.AI.AbstractAI)Activator.CreateInstance(AITypes[aiZahl], new object[] { width, height, aiPlayer });
        }

        private void GetAITypes()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            AITypes = new List<Type>(asm.GetTypes().Where<Type>(t => t.IsSubclassOf(typeof(AI.AbstractAI))));
            if (!Directory.Exists("data/plugins/ki"))
            {
                Directory.CreateDirectory("data/plugins/ki");
            }
            GetAITypesFromOuterAssemblie(Directory.EnumerateFiles("data/plugins/ki").ToArray());
        }

        private void GetAITypesFromOuterAssemblie(string[] files)
        {
            foreach (string file in files)
            {
                try
                {
                    Assembly asm = Assembly.LoadFile(file);
                    AITypes.AddRange(asm.GetTypes().Where<Type>(t => t.IsSubclassOf(typeof(AI.AbstractAI))));
                }
                catch (Exception)
                {
                }
            }
        }

        public override Vector2i Play(Fields.Field field)
        {
            if (AI is AI.IPlayableAI)
            {
                return Vector2i.IndexToVector(((UniTTT.Logik.AI.IPlayableAI)AI).Play(field), field.Width, field.Height);
            }
            else
                return new Vector2i(-1, -1);
        }

        public override void Learn()
        {
            if (AI is AI.ILearnableAI)
            {
                ((Logik.AI.ILearnableAI)AI).Learn();
            }
        }

        public override string ToString()
        {
            return AI.ToString();
        }

        class AIReinforcement : AI.AbstractAI, AI.IPlayableAI, AI.ILearnableAI
        {
            public AIReinforcement(int width, int height, char aiPlayer)
                : base(aiPlayer, width, height)
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
                    OnShowMessageEvent("Wie viele Runden sollen durchlaufen werden? (als Zahl)");
                    if (int.TryParse(OnGetIntEvent().ToString(), out ret))
                    {
                        if (ret < 0)
                        {
                            OnShowMessageEvent("Zahl zu klein.");
                        }
                        else
                        {
                            return ret;
                        }
                    }
                    else
                    {
                        OnShowMessageEvent("Irgendetwas wurde falsch eingegeben.");
                        OnShowMessageEvent("Eventuell ein Lerrzeichen, oder ein anderes nicht Zahl Zeichen");
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
                string momsitcode = SitCodeHelper.GetEmpty(9);
                int[,] sit_codes = new int[(int)runden, 9];
                int[,] zuege = new int[(int)runden, 9];
                int[] wertungen = new int[(int)runden];
                bool gewonnen = false;
                #endregion
                OnShowMessageEvent("Berechne Daten..");
                for (int currround = 0; currround < runden; currround++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        player = SitCodeHelper.PlayerChange(player);
                        sit_codes[currround, i] = int.Parse(momsitcode);
                        zug = FieldHelper.GetRandomZug(Fields.SitCode.GetInstance(momsitcode, Width, Height));
                        zuege[currround, i] = zug;

                        momsitcode = momsitcode.Remove(zug, 1).Insert(zug, player.ToString());

                        gewonnen = Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(player), Fields.SitCode.GetInstance(momsitcode, Width, Height));
                        // Wertungen
                        // Aufwerten
                        if ((gewonnen) && (player != AIPlayer))
                            wertungen[currround] = 1;
                        else if ((gewonnen) && (player == AIPlayer))
                            wertungen[currround] = -1;
                        else if ((i == 8) && (!gewonnen))
                            wertungen[currround] = 0;

                        // Ist Spiel Zu Ende?
                        if ((gewonnen) || (i == 8))
                        {
                            momsitcode = SitCodeHelper.GetEmpty(9);
                            i = 9;
                            player = '2';
                        }
                    }
                    if (currround % 100 == 0)
                    {
                        OnShowMessageEvent("Spielrunde Nr. " + currround);
                    }
                }
                OnShowMessageEvent("Fertig mit dem Berechnen der Daten.");
                OnShowMessageEvent("Speichere Daten");
                writerreader.Write(zuege, sit_codes, wertungen);
                OnShowMessageEvent("Fertig, Taste drücken zum Beenden");
                OnGetStringEvent();
            }

            public int Play(Fields.Field field)
            {
                string sitcode = SitCodeHelper.StringToSitCode(field.ToString());
                List<int> Fields = new List<int>(writerreader.Read(sitcode));
                int zug = -1;
                do
                {
                    zug = Fields.ToArray().GetHighestIndex();
                    if (field.IsFieldEmpty(zug))
                    {
                        break;
                    }
                    else
                    {
                        Fields.Remove(zug);
                    }
                } while (true);
                if (zug == -1)
                    zug = FieldHelper.GetRandomZug(field);
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
                    BinaryWriter binwriter = new BinaryWriter(File.OpenWrite(FileName + "_tmp"), Encoding.UTF8);
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
                    FileStream stream = new FileStream(FileName + "_tmp", System.IO.FileMode.OpenOrCreate);
                    GZipStream zipStream = new GZipStream(new FileStream(FileName, FileMode.OpenOrCreate), CompressionMode.Compress);
                    byte[] bufffer = new byte[stream.Length];
                    stream.Read(bufffer, 0, bufffer.Length);
                    stream.Close();
                    zipStream.Write(bufffer, 0, bufffer.Length);
                    zipStream.Flush();
                    zipStream.Close();
                    File.Delete(FileName + "_tmp");
                }

                private string[] lines = null;

                public int[] Read(string sitcode)
                {
                    int[] fields = new int[9];
                    if (File.Exists(FileName))
                    {
                        if (lines == null)
                        {
                            GZipStream zipStream = new GZipStream(new FileStream(FileName, FileMode.Open), CompressionMode.Decompress);
                            FileStream stream = new FileStream(FileName + "_tmp", System.IO.FileMode.Create);
                            byte[] buffer = new byte[4096];
                            int bytesReadCount;
                            do
                            {
                                bytesReadCount = zipStream.Read(buffer, 0, buffer.Length);
                                if (bytesReadCount != 0)
                                {
                                    stream.Write(buffer, 0, bytesReadCount);
                                }

                            } while (bytesReadCount >= buffer.Length);
                            stream.Close();
                            zipStream.Close();

                            lines = File.ReadAllLines(FileName + "_tmp", Encoding.UTF8);
                            File.Delete(FileName + "_tmp");
                        }

                        List<string> substrs;

                        foreach (string item in lines.Where<string>(f => f.Contains(sitcode)))
                        {
                            substrs = item.GetSubstrs();
                            fields[int.Parse(substrs[1])] += int.Parse(substrs[2]);
                        }
                    }
                    return fields;
                }
            }
        }

        class AIBot : AI.AbstractAI, AI.IPlayableAI
        {
            public AIBot(int width, int height, char aiPlayer) : base(aiPlayer, width, height) { }

            public int Play(Fields.Field field)
            {
                int win_zug = TestForOneWin(field);
                int block_zug = TestForHumanBlock(field);
                int set_zug = TestForBestPosition(field);

                if (win_zug != -1)
                    return win_zug;
                else if (block_zug != -1)
                    return block_zug;
                else if (set_zug != -1)
                    return set_zug;
                else
                    return FieldHelper.GetRandomZug(field);
            }

            private int TestForOneWin(Fields.Field field)
            {
                int win_zug = -1;
                for (int playerpos = 0; (playerpos < field.Length) && (win_zug == -1); playerpos++)
                {
                    if (field.IsFieldEmpty(playerpos))
                    {
                        field.SetField(playerpos, AIPlayer);
                        if ((Logik.WinChecker.Pruefe(AIPlayer, field)) && (win_zug == -1))
                            win_zug = playerpos;
                        field.SetField(playerpos, ' ');
                    }
                }
                return win_zug;
            }

            private int TestForHumanBlock(Fields.Field field)
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

            private int TestForBestPosition(Fields.Field field)
            {
                int[] posis = new int[Length];

                List<Fields.FieldRegion> fpanel = field.Panels;

                foreach (Fields.FieldRegion region in fpanel)
                {
                    if (region.Count() < WinChecker.GewinnBedingung) continue;
                    if (!region.Contains<char>(HumanPlayer))
                    {
                        foreach (Fields.FieldPlaceData data in region)
                        {
                            if (data.FieldValue != AIPlayer && data.FieldValue != HumanPlayer)
                            {
                                posis[data.LocationInField]++;
                            }
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

        class AIRandom : AI.AbstractAI, AI.IPlayableAI
        {
            public AIRandom(int width, int height, char aiPlayer) : base(aiPlayer, width, height) { }

            public int Play(Fields.Field field)
            {
                return FieldHelper.GetRandomZug(field);
            }

            public override string ToString()
            {
                return "Random";
            }
        }

        class AIMiniMax : AI.AbstractAI, AI.IPlayableAI
        {
            public AIMiniMax(int width, int height, char aiPlayer) : base(aiPlayer, width, height) { }

            private int bestZug;

            public int Play(Fields.Field field)
            {
                Max(field.Length - 1, field);
                return bestZug;
            }

            private int Max(int depth, Fields.Field field)
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

            private int Mini(int depth, Fields.Field field)
            {
                int zugValue = 0;
                int discovered = int.MaxValue - 1;
                for (int i = 0; i < field.Length; i++)
                {
                    if (field.IsFieldEmpty(i))
                    {
                        field.SetField(i, AIPlayer);
                        if (depth <= 1)
                        {
                            zugValue = Valuation(field, AIPlayer);
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

            public override string ToString()
            {
                return "MiniMax";
            }
        }

        class AIMiniMaxAlphaBeta : AI.AbstractAI, AI.IPlayableAI
        {
            public AIMiniMaxAlphaBeta(int width, int height, char aiPlayer) : base(aiPlayer, width, height) { }

            private int bestZug;

            public int Play(Fields.Field field)
            {
                Max(field.Length - 1, int.MaxValue - 1, int.MinValue + 1, field);
                return bestZug;
            }

            private int Max(int depth, int alpha, int beta, Fields.Field field)
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

            private int Mini(int depth, int alpha, int beta, Fields.Field field)
            {
                if (depth == 0)
                {
                    return Valuation(field, AIPlayer);
                }
                int localBeta = int.MaxValue - 1;
                for (int i = 0; i < field.Length; i++)
                {
                    if (field.IsFieldEmpty(i))
                    {
                        field.SetField(i, AIPlayer);
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

            public override string ToString()
            {
                return "MiniMax";
            }
        }
    }
}