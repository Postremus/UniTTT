using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public interface IPlayableKI : IKI
    {
        int Play(Fields.IField field, char spieler);
    }
}
