using System;

namespace CZToolKit.TimelineLite
{
    /// <summary> menuitem </summary>
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
