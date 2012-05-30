using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik.Fields;
using UniTTT.Logik.Player;

namespace UniTTT.Logik.Game
{
    public class NormalGame : Game
    {
        public NormalGame(Logik.Player.AbstractPlayer p1, Logik.Player.AbstractPlayer p2, Logik.IBrettDarsteller bdar, Logik.Fields.IField field)
        {
            Initialize(p1, p2, bdar, field);
        }

        #region Methods
        public override void Logik()
        {
            PlayerChange();
            OnPlayerOutputEvent(Player.Ausgabe());
            Field.SetField(Player.Play(Field), Player.Symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public override void Logik(Vector2i vect)
        { 
            if (!HasStarted)
            {
                HasStarted = true;
            }

            PlayerChange();
            OnPlayerOutputEvent(Player == Player2 ? Player1.Ausgabe() : Player2.Ausgabe());
            Field.SetField(vect, Player.Symbol);
            if (IsBDarstellerValid())
            {
                BDarsteller.Update(Field);
                BDarsteller.Draw();
            }
        }

        public void WinCounter()
        {
            if (FieldHelper.GetGameState(Field, Player, Player1) == FieldHelper.GameStates.Gewonnen)
            {
                Player1.WinCounter = Player == Player1 ? +1 : 0;
                Player2.WinCounter = Player == Player2 ? +1 : 0;
            }
        }

        public override string ToString()
        {
            return (Player1 is Logik.Player.KIPlayer) && (Player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
        #endregion
    }
}