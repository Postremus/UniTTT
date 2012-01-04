using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.KI
{
    public interface IKI
    {
        int Width { get; }
        int Height { get; }
        int Length { get; }
        char KIPlayer { get; }
    }
}
