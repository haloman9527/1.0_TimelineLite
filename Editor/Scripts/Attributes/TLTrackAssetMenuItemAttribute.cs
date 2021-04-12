using System;

namespace CZToolKit.TimelineLite.Editors
{
    /// <summary> menuitem </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TLTrackAssetMenuItemAttribute : Attribute
    {
        public string defaultTrackName;

        public TLTrackAssetMenuItemAttribute() { }

        public TLTrackAssetMenuItemAttribute(string _defaultTrackName)
        {
            defaultTrackName = _defaultTrackName;
        }
    }
}
