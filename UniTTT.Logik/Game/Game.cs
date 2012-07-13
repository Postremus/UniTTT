using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]
namespace UniTTT.Logik.Game
{
    public class Game
    {
        #region privates
        private bool _hasStoped;
        private bool _hasStarted;
        #endregion

        public event PlayerMovedHandler PlayerMovedEvent;
        public event WindowTitleChangeHandler WindowTitleChangeEvent;
        public event PlayerOutputHandler PlayerOutputEvent;
        public event WinMessageHandler WinMessageEvent;
        public event GetIntHandler GetIntEvent;
        public event GetStringHandler GetStringEvent;
        public event ShowMessageHandler ShowMessageEvent;
        public event NewGameHandler NewGameEvent;

        #region Propertys
        public Fields.Field Field
        {
            get;
            set;
        }
        public Logik.IBrettDarsteller BDarsteller { get; set; }
        public Player.Player Player { get; set; }
        public Player.Player Player1 { get; set; }
        public Player.Player Player2 { get; set; }
        public bool HasStoped
        {
            get
            {
                return _hasStoped;
            }
            set
            {
                _hasStoped = value;
            }
        }
        public bool HasStarted
        {
            get
            {
                return _hasStarted;
            }
            set
            {
                _hasStarted = value;
            }
        }
        #endregion

        public Game(Logik.Player.Player p1, Logik.Player.Player p2, Logik.IBrettDarsteller bdar, Logik.Fields.Field field)
        {
            NewGameEvent += NewGame;
            Initialize(p1, p2, bdar, field);
        }

        public void Initialize(Logik.Player.Player p1, Logik.Player.Player p2, Logik.IBrettDarsteller bdar, Logik.Fields.Field field)
        {
            if (field == null)
            {
                Field = new Fields.Brett(bdar.Width, bdar.Height);
            }
            else
            {
                Field = field;
            }
            if (p1 is Player.AIPlayer)
            {
                p1 = RegisterAIEvents((UniTTT.Logik.Player.AIPlayer)p1);
            }
            else if (p2 is Player.AIPlayer)
            {
                p2 = RegisterAIEvents((UniTTT.Logik.Player.AIPlayer)p2);
            }
            BDarsteller = bdar;
            Player1 = p1;
            Player2 = p2;
            Initialize();
        }

        public void Initialize()
        {
            OnWindowTitleChangeEvent("UniTTT - " + this.ToString());
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        private Player.Player RegisterAIEvents(Player.AIPlayer ai)
        {
            ai.AI.GetIntEvent += OnGetIntEvent;
            ai.AI.GetStringEvent += OnGetStringEvent;
            ai.AI.ShowMessageEvent += OnShowMessageEvent;
            return ai;
        }

        public virtual void Logik(Vector2i vect)
        {
            HasStarted = true;
            if (HasStoped)
            {
                return;
            }
            PlayerChange();
            OnPlayerOutputEvent(Player == Player2 ? Player1.Ausgabe() : Player2.Ausgabe());
            OnPlayerMovedEvent(vect);
            Field.SetField(vect, Player.Symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
            if (HasEnd())
            {
                OnWinMessageEvent(Player.Symbol, FieldHelper.GetGameState(Field, Player, Player1));
                BDarsteller.Enabled = false;
            }
        }

        public virtual void Logik()
        {
            HasStarted = true;
            if (HasStoped)
            {
                return;
            }
            PlayerChange();
            OnPlayerOutputEvent(Player.Ausgabe());
            Vector2i vect = Player.Play(Field);
            OnPlayerMovedEvent(vect);
            Field.SetField(vect, Player.Symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
            if (HasEnd())
            {
                OnWinMessageEvent(Player.Symbol, FieldHelper.GetGameState(Field, Player, Player1));
                BDarsteller.Enabled = false;
            }
        }

        public virtual void LogikLoop()
        {
            HasStoped = false;
            HasStarted = true;
            do
            {
                Logik();
            } while (!HasEnd());
            HasStoped = true;
            HasStarted = false;
        }

        public bool IsBDarstellerValid()
        {
            return BDarsteller != null;
        }

        public bool IsFieldValid()
        {
            return Field != null;
        }

        public void PlayerChange()
        {
            Player = Player1 == Player ? Player2 : Player1;
        }

        public bool HasEnd()
        {
            return FieldHelper.GetGameState(Field, Player, Player1) != GameStates.Laufend;
        }

        public virtual void NewGame()
        {
            Field.Initialize();
            Player = null;
            BDarsteller.Initialize(Field.Width, Field.Height);
            BDarsteller.Update(Field);
            BDarsteller.Draw();
            BDarsteller.Enabled = true;
            HasStoped = false;
            HasStarted = true;
        }

        public void WinCounter()
        {
            if (FieldHelper.GetGameState(Field, Player, Player1) == GameStates.Gewonnen)
            {
                if (Player == Player1)
                {
                    Player1.WinCounter++;
                }
                else
                {
                    Player2.WinCounter++;
                }
            }
        }

        public void OnPlayerMovedEvent(Vector2i vect)
        {
            PlayerMovedHandler playerMovedEvent = PlayerMovedEvent;
            if (playerMovedEvent != null)
            {
                playerMovedEvent(vect);
            }
        }

        public void OnNewGameEvent()
        {
            NewGameHandler newGameEvent = NewGameEvent;
            if (newGameEvent != null)
            {
                newGameEvent();
            }
        }

        public void OnWindowTitleChangeEvent(string title)
        {
            WindowTitleChangeHandler windowTitleChangeEvent = WindowTitleChangeEvent;
            if (windowTitleChangeEvent != null)
            {
                windowTitleChangeEvent(title);
            }
        }

        public void OnPlayerOutputEvent(string message)
        {
            PlayerOutputHandler playerOutputEvent = PlayerOutputEvent;
            if (playerOutputEvent != null)
            {
                playerOutputEvent(message);
            }
        }

        public void OnWinMessageEvent(char symbol, GameStates gameState)
        {
            WinMessageHandler winMessageEvent = WinMessageEvent;
            if (winMessageEvent != null)
            {
                winMessageEvent(symbol, gameState);
            }
        }

        public int OnGetIntEvent()
        {
            int ret = -1;
            GetIntHandler getIntEvent = GetIntEvent;
            if (getIntEvent != null)
            {
                ret = getIntEvent();
            }
            return ret;
        }

        public string OnGetStringEvent()
        {
            string ret = null;
            GetStringHandler getStringEvent = GetStringEvent;
            if (getStringEvent != null)
            {
                ret = getStringEvent();
            }
            return ret;
        }

        public void OnShowMessageEvent(string message)
        {
            ShowMessageHandler showMessageEvent = ShowMessageEvent;
            if (showMessageEvent != null)
            {
                showMessageEvent(message);
            }
        }

        public override string ToString()
        {
            if (Player1 is Player.AIPlayer && Player2 is Player.AIPlayer)
            {
                return "KiGame";
            }
            else if (Player1 is Player.NetworkPlayer || Player1 is Player.NetworkPlayer)
            {
                return "NetworkGame";
            }
            else
            {
                return "HumanGame";
            }
        }
    }
}