using System;

namespace CZToolKit.TimelineLite.Editors
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TLTrackMinHeightAttribute : Attribute
    {
        public float MinHeight;
        public TLTrackMinHeightAttribute(float _minHeight)
        {
            MinHeight = _minHeight;
        }
    }
}
