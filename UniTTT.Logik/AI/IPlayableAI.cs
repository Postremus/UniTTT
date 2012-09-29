using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.AI
{
    public interface IPlayableAI
    {
        int Play(Fields.Field field);
    }
}
