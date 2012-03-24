using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public interface IField
    {
        int Width { get; }
        int Height { get; }
        int Length { get; }
        List<FieldRegion> Panels { get; }

        void Initialize();
        void Initialize(int width, int height);
        char GetField(int idx);
        char GetField(Vector2i vect);
        void SetField(int idx, char value);
        void SetField(Vector2i vect, char value);
        FieldRegion Row(int count);
        FieldRegion Column(int count);
        FieldRegion Diagonal(int count);

        bool IsFieldEmpty(int idx);
        bool IsFieldEmpty(Vector2i vect);
    }
}
