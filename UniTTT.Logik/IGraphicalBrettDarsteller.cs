using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik
{
    public interface IGraphicalBrettDarsteller : IBrettDarsteller
    {
        void Lock();
        void DeLock();
        void Create();
    }
}
