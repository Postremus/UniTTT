using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public interface IGraphicalBrettDarsteller : IBrettDarsteller
    {
        bool Enabled { get; set; }
        void Create();
    }
}
