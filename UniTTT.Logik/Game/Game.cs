using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Game
{
    public class Game
    {
        #region privates
        private bool _hasStoped;
        private bool _hasStarted;
        #endregion

        public event PlayerMovedHandler PlayerMovedEvent;
        public event PlayerChangeHandler PlayerChangeEvent;
        public event WindowTitleChangeHandler WindowTitleChangeEvent;
        public event PlayerOutputHandler PlayerOutputEvent;
        public event WinMessageHandler WinMessageEvent;
        public event GetIntHandler GetIntEvent;
        public event GetStringHandler GetStringEvent;
        public event ShowMessageHandler ShowMessageEvent;

        #region Propertys
        public Fields.IField Field
        {
            get;
            set;
        }
        public Logik.IBrettDarsteller BDarsteller { get; set; }
        public Player.AbstractPlayer Player { get; set; }
        public Player.AbstractPlayer Player1 { get; set; }
        public Player.AbstractPlayer Player2 { get; set; }
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

        public void Initialize(Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.Fields.IField field)
        {
            if (field == null)
            {
                Field = new Fields.Brett(bdar.Width, bdar.Height);
            }
            else
            {
                Field = field;
            }
            if (p1 is Player.KIPlayer)
            {
                p1 = RegisterKIEvents((UniTTT.Logik.Player.KIPlayer)p1);
            }
            else if (p2 is Player.KIPlayer)
            {
                p2 = RegisterKIEvents((UniTTT.Logik.Player.KIPlayer)p2);
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

        private Player.AbstractPlayer RegisterKIEvents(Player.KIPlayer ki)
        {
            ki.KI.GetIntEvent += OnGetIntEvent;
            ki.KI.GetStringEvent += OnGetStringEvent;
            ki.KI.ShowMessageEvent += OnShowMessageEvent;
            return ki;
        }

        public virtual void Logik(Vector2i vect)
        {

        }

        public virtual void Logik()
        {
        }

        public virtual void LogikLoop()
        {
            do
            {
                Logik();
            } while (!HasEnd());
        }

        public bool IsBDarstellerValid()
        {
            return BDarsteller != null;
        }

        public bool IsFieldValid()
        {
            return Field != null;
        }

        public bool IsBDarstellerGraphical()
        {
            return BDarsteller is IGraphicalBrettDarsteller;
        }

        public void PlayerChange()
        {
            Player = Player1 == Player ? Player2 : Player1;
        }

        public bool HasEnd()
        {
            if (FieldHelper.GetGameState(Field, Player, Player1) != UniTTT.Logik.FieldHelper.GameStates.Laufend)
                return true;
            return false;
        }

        public virtual void NewGame()
        {
            Field.Initialize();
            Player = null;
            if (IsBDarstellerGraphical())
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).Enabled = true;
            }
            BDarsteller.Initialize(Field.Width, Field.Height);
        }

        public void WinCounter()
        {
            if (FieldHelper.GetGameState(Field, Player, Player1) == FieldHelper.GameStates.Gewonnen)
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

        public void OnPlayerChangeEvent()
        {
            PlayerChangeHandler playerChangeEvent = PlayerChangeEvent;
            if (playerChangeEvent != null)
            {
                playerChangeEvent();
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

        public void OnWinMessageEvent(char symbol, FieldHelper.GameStates gameState)
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
    }
}