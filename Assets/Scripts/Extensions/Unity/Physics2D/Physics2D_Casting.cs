using UnityEngine;

namespace AntCorp {
    public static partial class Physics2DEx{
        public static bool TryBoxCast(out Transform t, Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance)
        {
            RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, direction, distance);
            if (hit){
                t = hit.transform;
                return true;
            }
            t = null;
            return false;
        }
    }
}
// Author: Viktor Chernikov, 2023