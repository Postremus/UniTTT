using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.ScreenSaver
{
    [Serializable()]
    public class Config
    {
        private int _playVelocity;
        private int _moveVelocity;
        private bool _matrix;

        public int PlayVelocity { get { return _playVelocity; } set { _playVelocity = value; } }
        public int MoveVelocity { get { return _moveVelocity; } set { _moveVelocity = value; } }
        public bool Matrix { get { return _matrix; } set { _matrix = value; } }
    }
}
