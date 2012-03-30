using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public class FieldPlaceData
    {
        private int _locationInField;
        private char _fieldValue;

        public int LocationInField
        {
            get
            {
                return _locationInField;
            }
            set
            {
                _locationInField = value;
            }
        }

        public char FieldValue
        {
            get
            {
                return _fieldValue;
            }
            set
            {
                _fieldValue = value;
            }
        }

        public FieldPlaceData(int locationInField, char fieldValue)
        {
            LocationInField = locationInField;
            FieldValue = fieldValue;
        }
    }
}
