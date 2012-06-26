using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    public class FieldRegion : IEnumerable<FieldPlaceData>
    {
        private List<FieldPlaceData> _fieldDataList;

        public FieldRegion()
        {
            _fieldDataList = new List<FieldPlaceData>();
        }

        public void Add(int locationInField, char fieldValue)
        {
            _fieldDataList.Add(new FieldPlaceData(locationInField, fieldValue));
        }

        public IEnumerator<FieldPlaceData> GetEnumerator()
        {
            return (IEnumerator<FieldPlaceData>)_fieldDataList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)_fieldDataList.GetEnumerator();
        }

        public bool Contains<T1>(char fieldValue, int locationInField)
        {
            if (this.Count<char>(fieldValue, locationInField) > 0)
                return true;
            return false;
        }

        public bool Contains<T1>(char fieldValue)
        {
            if (this.Count<char>(fieldValue) > 0)
                return true;
            return false;
        }

        public bool Contains<T1>(int locationInField)
        {
            if (this.Count<int>(locationInField) > 0)
                return true;
            return false;
        }

        public int Count<T1>(char fieldValue)
        {
            return _fieldDataList.Count(r => r.FieldValue == fieldValue);
        }

        public int Count<T1>(int locationInField)
        {
            return _fieldDataList.Count(r => r.LocationInField == locationInField);
        }

        public int Count<t1>(char fieldValue, int locationInField)
        {
            return _fieldDataList.Count(r => r.FieldValue == fieldValue && r.LocationInField == locationInField);
        }

        public int BigestRelatedPice(char fieldValue)
        {
            int ret = 0;
            int tmp = 0;
            foreach (FieldPlaceData placeData in _fieldDataList)
            {
                if (placeData.FieldValue == fieldValue)
                {
                    tmp++;
                }
                else if (tmp > ret)
                {
                    ret = tmp;
                }
            }
            return ret;
        }
    }
}
