using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CZToolKit.TimelineLite
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true,Inherited = true)]
    public class TLClipTooltipAttribute : Attribute
    {
        public string Tooltip;
        public TLClipTooltipAttribute(string _tooltip)
        {
            Tooltip = _tooltip;
        }
    }
}
