using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniTTT.Logik.Fields
{
    [Serializable()]
    public class SitCode : Field
    {

        #region Privates
        private string VarField { get; set; }
        #endregion

        #region Constructor
        public SitCode(int width, int height)
        {
            Initialize(width, height);
        }
        #endregion

        #region interface Methods
        public override void Initialize()
        {
            VarField = SitCodeHelper.GetEmpty(Length);
        }

        public override char GetField(int idx)
        {
            if (IsEntryPointInTheSize(idx))
            {
                return SitCodeHelper.ToPlayer(VarField[idx]);
            }
            else
            {
                return 'n';
            }
        }

        public override char GetField(Vector2i vect)
        {
            return GetField(Vector2i.VectorToIndex(vect, Width));
        }

        public override void SetField(int idx, char value)
        {
            if (IsEntryPointInTheSize(idx))
            {
                VarField = VarField.Remove(idx, 1).Insert(idx, SitCodeHelper.PlayertoSitCode(value).ToString());
            }
        }

        public override void SetField(Vector2i vect, char value)
        {
            SetField(Vector2i.VectorToIndex(vect, Width), value);
        }

        public override bool IsFieldEmpty(Vector2i vect)
        {
            return IsFieldEmpty(Vector2i.VectorToIndex(vect, Width));
        }

        public override bool IsFieldEmpty(int idx)
        {
            return VarField[idx] == '1';
        }
        #endregion

        public static SitCode GetInstance(string sitcode, int width, int height)
        {
            SitCode ret = new SitCode(width, height);
            ret.VarField = sitcode;
            return ret;
        }
    }
} 
