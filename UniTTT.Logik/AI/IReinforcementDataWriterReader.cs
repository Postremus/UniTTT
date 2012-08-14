using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.AI
{
    public interface IReinforcementDataWriterReader
    {
        void Write(int[,] zuege, int[,] sitCodes, int[] wertungen);
        int[] Read(string sitcode);
    }
}
