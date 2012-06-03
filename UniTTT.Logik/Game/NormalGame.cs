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
        public NormalGame(Logik.Player.Player p1, Logik.Player.Player p2, Logik.IBrettDarsteller bdar, Logik.Fields.IField field)
        {
            Initialize(p1, p2, bdar, field);
            FieldSetEvent += Field.SetField;
        }
        #region methods
        public override string ToString()
        {
            return (Player1 is Logik.Player.KIPlayer) && (Player2 is Logik.Player.KIPlayer) ? "KiGame" : "HumanGame";
        }
        #endregion
    }
}