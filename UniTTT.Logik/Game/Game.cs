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

        public event Network.PlayerMovedHandler PlayerMovedEvent;
        public event Network.PlayerChangeHandler PlayerChangeEvent;

        #region Propertys
        public Fields.IField Field
        {
            get;
            set;
        }
        public Logik.IBrettDarsteller BDarsteller { get; set; }
        public Logik.IOutputDarsteller ODarsteller { get; set; }
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
        public bool HastStarted
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

        public void Initialize(Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field)
        {
            if (field == null)
            {
                Field = new Fields.Brett(bdar.Width, bdar.Height);
            }
            else
            {
                Field = field;
            }
            BDarsteller = bdar;
            ODarsteller = odar;
            Player1 = p1;
            Player2 = p2;
            Initialize();
        }

        public void Initialize()
        {
            if (IsODarstellerValid())
            {
                ODarsteller.Title = "UniTTT - " + this.ToString();
            }
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void Logik(Vector2i vect)
        {

        }

        public bool IsBDarstellerValid()
        {
            return BDarsteller != null;
        }

        public bool IsODarstellerValid()
        {
            return ODarsteller != null;
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

        public void OnPlayerMovedEvent(Vector2i vect)
        {
            Network.PlayerMovedHandler playerMovedEvent = PlayerMovedEvent;
            if (playerMovedEvent != null)
            {
                playerMovedEvent(vect);
            }
        }

        public void OnPlayerChangeEvent()
        {
            Network.PlayerChangeHandler playerChangeEvent = PlayerChangeEvent;
            if (playerChangeEvent != null)
            {
                playerChangeEvent();
            }
        }
    }
}