using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public class AbstractGame
    {
        protected AbstractGame(int b, int h, Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller dar)
        {
            brett = new Logik.Brett(b, h);
            darsteller = dar;
            player1 = p1;
            player2 = p2;

            darsteller.Update(brett.VarBrett);
            darsteller.Draw();
        }

        #region Fields
        public Player.AbstractPlayer player { get; private set; }
        public Player.AbstractPlayer player1 { get; private set; }
        public Player.AbstractPlayer player2 { get; private set; }
        public Logik.Brett brett { get; private set; }
        public Logik.IBrettDarsteller darsteller { get; private set; }
        #endregion

        #region Methods
        protected void Logik()
        {
        }

        protected void PlayerChange()
        {
            player = player1 == player ? player2 : player1;
        }

        protected void NewGame()
        {
            brett = new Logik.Brett(brett.Breite, brett.Hoehe);
            player = null;
        }
        #endregion
    }
}
