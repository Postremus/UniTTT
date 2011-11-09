using System;
using System.Collections.Generic;
using System.Text;

namespace UniTTT.Logik
{
    public class Brett
    {
        #region Fields
        private char[,] m_brett;
        private int m_hoehe;
        private int m_breite;
        private GewinnPrüfer pruefer;
        #endregion

        #region Methods
        // Konstruktor
        public Brett(int breite, int hoehe)
        {
            Hoehe = hoehe;
            Breite = breite;
            VarBrett = new char[Breite, Hoehe];
            BrettArraySetzen();
            pruefer = new GewinnPrüfer(hoehe < breite ? hoehe : breite);
        }

        public int Length
        {
            get { return VarBrett.Length; }
        }

        public int Hoehe
        {
            get { return m_hoehe; }
            private set { m_hoehe = value; }
        }

        public int Breite
        {
            get { return m_breite; }
            private set { m_breite = value; }
        }

        public char[,] VarBrett
        {
            get { return m_brett; }
            private set { m_brett = value; }
        }

        public void Setzen(char spieler, Vector vect)
        {
            VarBrett[vect.X, vect.Y] = spieler;
        }

        // Brett mittels zug(int) Setzen
        public void Setzen(char spieler, int zug)
        {
            int wert = 0;
            for (int x = 0; x < Breite; x++)
            {
                for (int y = 0; y < Hoehe; y++)
                {
                    if (wert == zug)
                    {
                        VarBrett[x, y] = spieler;
                        return;
                    }
                    wert++;
                }
            }
        }

        // Den Array brett komplett auf ' ' (Leerzeichen) setzen.
        private void BrettArraySetzen()
        {
            for (int i = 0; i < Breite; i++)
                for (int x = 0; x < Hoehe; x++)
                    VarBrett[i, x] = ' ';
        }

        public bool HasEmptyFields()
        {
            foreach (char field in VarBrett)
                if (field == ' ')
                    return true;
            return false;
        }

        public bool IsFieldEmpty(int spalte, int zeile)
        {
            return VarBrett[spalte, zeile] == ' ';
        }

        public BrettHelper.GameStates GetGameState(char[,] brett, char spieler)
        {
            BrettHelper.GameStates state = BrettHelper.GameStates.Laufend;
            bool gewbl = pruefer.Pruefe(spieler, brett);

            if (gewbl)
                state = BrettHelper.GameStates.Gewonnen;
            if ((!gewbl) && (!BrettHelper.HasEmptyFields(brett)))
                state = BrettHelper.GameStates.Unentschieden;
            return state;
        }
        #endregion
    }
}