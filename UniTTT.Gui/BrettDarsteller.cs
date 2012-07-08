using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik;

namespace UniTTT.Gui
{
    public delegate void BrettUpdateHandler();
    public delegate void BrettDrawHandler();
    public delegate void BrettEnableHandler();

    public class BrettDarsteller : IBrettDarsteller
    {
        private int _width;
        private int _height;
        private bool _enabled;

        public event BrettUpdateHandler BrettUpdateEvent;
        public event BrettDrawHandler BrettDrawEvent;
        public event BrettEnableHandler BrettEnableEvent;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                OnBrettEnable();
            }
        }

        public BrettDarsteller(int width, int height)
        {
            Initialize(width, height);
        }

        public void Initialize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Update(Logik.Fields.Field field)
        {
            OnBrettUpdate();
        }

        public void Draw()
        {
            OnBrettDraw();
        }

        public void OnBrettUpdate()
        {
            BrettUpdateHandler brettUpdateEvent = BrettUpdateEvent;
            if (brettUpdateEvent != null)
            {
                brettUpdateEvent();
            }
        }

        public void OnBrettDraw()
        {
            BrettDrawHandler brettDrawEvent = BrettDrawEvent;
            if (brettDrawEvent != null)
            {
                brettDrawEvent();
            }
        }

        public void OnBrettEnable()
        {
            BrettEnableHandler brettEnabledEvent = BrettEnableEvent;
            if (brettEnabledEvent != null)
            {
                brettEnabledEvent();
            }
        }
    }
}
