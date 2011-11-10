using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class NormalGame
    {
        public NormalGame(int b, int h, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.IOutputDarsteller odar)
        {
            brett = new Logik.Brett(b, h);
            BDarsteller = bdar;
            ODarsteller = odar;
            player1 = p1;
            player2 = p2;
            
            ODarsteller.Title = "UniTTT" + this.ToString();
            BDarsteller.Update(brett.VarBrett);
            BDarsteller.Draw();
        }

        #region Fields
        public Player.AbstractPlayer player { get; private set; }
        public Player.AbstractPlayer player1 { get; private set; }
        public Player.AbstractPlayer player2 { get; private set; }
        public Logik.Brett brett { get; private set; }
        public Logik.IBrettDarsteller BDarsteller { get; private set; }
        public Logik.IOutputDarsteller ODarsteller { get; private set; }
        #endregion

        #region Methods
        public void Logik()
        {
            PlayerChange();
            ODarsteller.PlayerAusgabe(player.Ausgabe());
            brett.Setzen(player.Spieler, player.Spiele(brett));
            BDarsteller.Update(brett.VarBrett);
            BDarsteller.Draw();
        }

        public void Logik(int zug)
        {
            PlayerChange();
            ODarsteller.PlayerAusgabe(player == player2 ? player1.Ausgabe() : player2.Ausgabe());
            brett.Setzen(player.Spieler, zug);
            BDarsteller.Update(brett.VarBrett);
            BDarsteller.Draw();
        }

        public void PlayerChange()
        {
            player = player1 == player ? player2 : player1;
        }

        public void NewGame()
        {
            brett = new Logik.Brett(brett.Breite, brett.Hoehe);
            player = null;
            if (BDarsteller is Logik.IGraphicalBrettDarsteller)
            {
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).DeLock();
                ((Logik.IGraphicalBrettDarsteller)BDarsteller).Lock();
            }
        }

        public bool HasEnd()
        {
            if (brett.GetGameState(brett.VarBrett, player.Spieler) != UniTTT.Logik.BrettHelper.GameStates.Laufend)
                return true;
            return false;
        }

        public void WinCounter()
        {
            if (brett.GetGameState(brett.VarBrett, player.Spieler) == BrettHelper.GameStates.Gewonnen)
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