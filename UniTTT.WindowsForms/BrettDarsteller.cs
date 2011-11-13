using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.WindowsForms
{
    class BrettDarsteller : Logik.IGraphicalBrettDarsteller
    {
        public BrettDarsteller() 
        {
            
        }

        #region Fields
        public int Height { get; private set; }
        public int Width { get; private set; }
        #endregion

        public void Update(char[,] brett)
        {
        }

        public void Draw()
        {
        }

        public void Lock()
        {
        }

        public void DeLock()
        {
        }

        public void Create()
        {
        }
    }
}
