using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;

namespace UniTTT.Interfaces
{
    public interface IPlayer
    {
        char Symbol { get; set; }
        int WinCounter { get; set; }

        Vector2i Player(IField field);
        void Learn();
    }
}
