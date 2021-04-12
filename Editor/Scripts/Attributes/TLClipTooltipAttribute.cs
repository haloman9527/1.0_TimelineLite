using System;

namespace CZToolKit.TimelineLite.Editors
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TLClipTooltipAttribute : Attribute
    {
        public string Tooltip;
        public TLClipTooltipAttribute(string _tooltip)
        {
            Tooltip = _tooltip;
        }
    }
}
