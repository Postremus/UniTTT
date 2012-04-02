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

        public int PlayVelocity { get { return _playVelocity; } set { _playVelocity = value; } }
        public int MoveVelocity { get { return _moveVelocity; } set { _moveVelocity = value; } }
    }
}
