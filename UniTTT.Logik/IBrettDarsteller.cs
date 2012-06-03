using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniTTT.Logik.Fields;

namespace UniTTT.Logik
{
    public interface IBrettDarsteller
    {
        int Width { get; }
        int Height { get; }

        void Update(Fields.Field field);
        void Draw();
        void Initialize(int width, int height);
    }
}