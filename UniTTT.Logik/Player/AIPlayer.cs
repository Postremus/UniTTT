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
                if (AITypes[i].Name.ToLower().Contains(ai.ToLower()))
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
            //GetAITypesFromOuterAssemblie();
        }

        private void GetAITypesFromOuterAssemblie()
        {
            AITypes.AddRange(Statics.PManager.GetPlugins<Type>(Plugin.PluginTypes.PlayableAI));
            AITypes.AddRange(Statics.PManager.GetPlugins<Type>(Plugin.PluginTypes.LearnableAI));
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
        }

        public override string ToString()
        {
            return AI.ToString();
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
                        if ((Logik.WinChecker.Pruefe(SitCodeHelper.ToPlayer(player), field)))
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
                    if (region.Count() >= WinChecker.WinCondition)
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