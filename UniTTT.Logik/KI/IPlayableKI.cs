using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public interface IPlayableKI
    {
        int Play(Fields.IField field);
    }
}
