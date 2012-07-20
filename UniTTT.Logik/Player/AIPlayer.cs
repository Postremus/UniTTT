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
            else
            {
                GetAITypesFromOuterAssemblie(Directory.GetFiles("data/plugins/ki"));
            }
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
                return Vector2i.FromIndex(((UniTTT.Logik.AI.IPlayableAI)AI).Play(field), field.Width, field.Height);
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
                writerReader = new WriterReader("KI_Reinforcement");
            }

            #region Fields
            private WriterReader writerReader;
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

            public void Learn()
            {
                #region Fields
                int runden = Rundefrage();
                int zug;
                string momsitcode = SitCodeHelper.GetEmpty(Length);
                int[,] sit_codes = new int[runden, Length];
                int[,] zuege = new int[runden, Length];
                int[] wertungen = new int[runden];
                GameStates state = GameStates.Laufend;
                Game.Game game = new Game.Game(new Player('1'), new Player('2'), null, new Fields.SitCode(3, 3));
                #endregion
                OnShowMessageEvent("Berechne Daten..");
                for (int currround = 0; currround < runden; currround++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        momsitcode = SitCodeHelper.StringToSitCode(game.Field.ToString());
                        sit_codes[currround, i] = int.Parse(momsitcode);
                        zug = FieldHelper.GetRandomZug(game.Field);
                        zuege[currround, i] = zug;

                        game.Logik(Vector2i.FromIndex(zug, 3, 3));

                        state = FieldHelper.GetGameState(game.Field, game.Player, game.Player1);
                        // Wertungen
                        // Aufwerten
                        if (state == GameStates.Gewonnen)
                            wertungen[currround] = 1;
                        else if (state == GameStates.Verloren)
                            wertungen[currround] = -1;
                        else if (state == GameStates.Unentschieden)
                            wertungen[currround] = 0;

                        // Ist Spiel Zu Ende?
                        if (game.HasEnd())
                        {
                            game.NewGame();
                        }
                    }
                    if (currround % 100 == 0)
                    {
                        OnShowMessageEvent("Spielrunde Nr. " + currround);
                    }
                }
                OnShowMessageEvent("Fertig mit dem Berechnen der Daten.");
                OnShowMessageEvent("Speichere Daten");
                writerReader.Write(zuege, sit_codes, wertungen);
                OnShowMessageEvent("Fertig, Taste drücken zum Beenden");
                OnGetStringEvent();
            }

            public int Play(Fields.Field field)
            {
                string sitcode = SitCodeHelper.StringToSitCode(field.ToString());
                List<int> fields = new List<int>(writerReader.Read(sitcode));
                int zug = -1;
                do
                {
                    zug = fields.GetHighestIndex();
                    if (field.IsFieldEmpty(zug))
                    {
                        break;
                    }
                    else
                    {
                        fields.Remove(zug);
                    }
                } while (true && fields.Count !=  0);
                if (zug == -1)
                    zug = FieldHelper.GetRandomZug(field);
                return zug;
            }

            public override string ToString()
            {
                return "Reinforcement";
            }

            private class WriterReader
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

                public int[] Read(string sitcode)
                {
                    int[] fields = new int[9];
                    if (File.Exists(FileName))
                    {
                        if (lines == null)
                        {
                            lines = File.ReadAllLines(FileName, Encoding.UTF8);
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
                int win_zug = TestForLineComplettings(field, AIPlayer);
                int block_zug = TestForLineComplettings(field, HumanPlayer);
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

            private int TestForLineComplettings(Fields.Field field, char player)
            {
                int ret = -1;
                for (int playerpos = 0; (playerpos < field.Length) && (ret == -1); playerpos++)
                {
                    if (field.IsFieldEmpty(playerpos))
                    {
                        field.SetField(playerpos, player);
                        if ((Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(player), field)) && (ret == -1))
                            ret = playerpos;
                        field.SetField(playerpos, ' ');
                    }
                }
                return ret;
            }

            private int TestForBestPosition(Fields.Field field)
            {
                int[] posis = new int[Length];

                List<Fields.FieldRegion> fpanel = field.FieldRegions;

                foreach (Fields.FieldRegion region in fpanel)
                {
                    if (!region.Contains<char>(HumanPlayer) && region.Count() >= WinChecker.GewinnBedingung)
                    {
                        foreach (Fields.FieldPlaceData data in region)
                        {
                            if (data.FieldValue == ' ')
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