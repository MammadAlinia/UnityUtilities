using UnityEngine;

namespace UnityUtilities.Runtime.ServiceLocator
{
    public static class TransformExtensions
    {
        public static Transform OrNull(this Transform transform)
        {
            return transform == null ? null : transform;
        }
    }
}