#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/haloman9527
 *  Blog: https://www.haloman.net/
 *
 */
#endregion

namespace Jiange.TimelineLite
{
    public enum UpdateMode
    {
        /// <summary> Update </summary>
        Normal = 0,
        /// <summary> FixedUpdate </summary>
        AnimatePhysics = 1,
        /// <summary> UnScaledUpdate </summary>
        UnscaledTime = 2,
        Manual = 3
    }
}