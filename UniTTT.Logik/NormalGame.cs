using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik.Fields;
using UniTTT.Logik.Player;

namespace UniTTT.Logik
{
    public class NormalGame
    {
        public NormalGame(Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field)
        {
            Initialize(p1, p2, bdar, odar, field);
        }

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

        #region Fields
        public Player.AbstractPlayer Player { get; private set; }
        public Player.AbstractPlayer Player1 { get; private set; }
        public Player.AbstractPlayer Player2 { get; private set; }
        public Fields.IField Field { get; private set; }       
        public Logik.IBrettDarsteller BDarsteller { get; private set; }
        public Logik.IOutputDarsteller ODarsteller { get; private set; }
        #endregion

        #region Methods
        public void Logik()
        {
            PlayerChange();
            if (IsODarstellerValid())
            {
                ODarsteller.PlayerAusgabe(Player.Ausgabe());
            }
            Field.SetField(Player.Play(Field), Player.Symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void Logik(int zug)
        {
            PlayerChange();
            if (IsODarstellerValid())
            {
                ODarsteller.PlayerAusgabe(Player == Player2 ? Player1.Ausgabe() : Player2.Ausgabe());
            }
            Field.SetField(zug, Player.Symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void LogikLoop()
        {
            do
            {
                Logik();
            } while (!HasEnd());
        }


        public void PlayerChange()
        {
            Player = Player1 == Player ? Player2 : Player1;
        }

        public void NewGame()
        {
            Field.Initialize();
            Player = null;
            if (IsBDarstellerGraphical())
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).DeLock();
            }
            BDarsteller.Update(Field);
            BDarsteller.Draw();
        }

        public bool HasEnd()
        {
            if (FieldHelper.GetGameState(Field, Player.Symbol) != UniTTT.Logik.FieldHelper.GameStates.Laufend)
                return true;
            return false;
        }

        public void WinCounter()
        {
            if (FieldHelper.GetGameState(Field, Player.Symbol) == FieldHelper.GameStates.Gewonnen)
            {
                Player1.WinCounter = Player == Player1 ? +1 : 0;
                Player2.WinCounter = Player == Player2 ? +1 : 0;
            }
        }

        public override string ToString()
        {
            return (Player1 is Logik.Player.KIPlayer) && (Player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
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
        #endregion
    }
}