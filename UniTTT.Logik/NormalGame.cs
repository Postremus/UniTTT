using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class NormalGame
    {
        public NormalGame(int b, int h, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar, Logik.Fields.IField field)
        {
            Field = field;
            BDarsteller = bdar;
            ODarsteller = odar;
            player1 = p1;
            player2 = p2;

            if (ODarsteller != null)
            {
                ODarsteller.Title = "UniTTT" + this.ToString();
                //ODarsteller.ThrowMessage();
            }
            if (BDarsteller != null)
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        #region Fields
        public Player.AbstractPlayer player { get; private set; }
        public Player.AbstractPlayer player1 { get; private set; }
        public Player.AbstractPlayer player2 { get; private set; }
        public Fields.IField Field { get; private set; }
        
        public Logik.IBrettDarsteller BDarsteller { get; private set; }
        public Logik.IOutputDarsteller ODarsteller { get; private set; }
        #endregion

        #region Methods
        public void Logik()
        {
            PlayerChange();
            if (ODarsteller != null)
                ODarsteller.PlayerAusgabe(player.Ausgabe());
            Field.SetField(player.Play(Field), player.Spieler);
            if (BDarsteller != null)
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void Logik(int zug)
        {
            PlayerChange();
            if (ODarsteller != null)
                ODarsteller.PlayerAusgabe(player == player2 ? player1.Ausgabe() : player2.Ausgabe());
            Field.SetField(zug, player.Spieler);
            if (BDarsteller != null)
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
            player = player1 == player ? player2 : player1;
        }

        public void NewGame()
        {
            Field.Initialize();
            player = null;
            if (BDarsteller is Logik.IGraphicalBrettDarsteller)
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).DeLock();
                BDarsteller.Update(Field);
            }
        }

        public bool HasEnd()
        {
            if (FieldHelper.GetGameState(Field, player.Spieler) != UniTTT.Logik.FieldHelper.GameStates.Laufend)
                return true;
            return false;
        }

        public void WinCounter()
        {
            if (FieldHelper.GetGameState(Field, player.Spieler) == FieldHelper.GameStates.Gewonnen)
            {
                player1.WinCounter = player == player1 ? +1 : 0;
                player2.WinCounter = player == player2 ? +1 : 0;
            }
        }

        public override string ToString()
        {
            return (player1 is Logik.Player.KIPlayer) && (player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
        #endregion
    }
}