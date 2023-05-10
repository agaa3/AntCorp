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
        public static bool TryCurvecast(out RaycastHit2D result, Vector2 origin, Vector2 direction, float segmentLength, float maxLength, float tilt = 0f, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            result = Curvecast(origin, direction, segmentLength, maxLength, tilt, layerMask, minDepth, maxDepth);
            return result != default;
        }
        public static RaycastHit2D Curvecast(Vector2 origin, Vector2 direction, float segmentLength, float maxLength, float tilt = 0f, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            direction.Normalize();
            tilt *= Mathf.Deg2Rad;
            float sin = Mathf.Sin(tilt);
            float cos = Mathf.Cos(tilt);
            int i = 0;
            do
            {
                float l = maxLength > segmentLength ? segmentLength : maxLength;

                RaycastHit2D hit = Physics2D.Linecast(origin, origin + direction * l, layerMask, minDepth, maxDepth);
                if (hit != default)
                {
                    return hit;
                }
                origin = origin + direction * l;
                float x = direction.x * cos - direction.y * sin;
                direction.y = direction.x * sin + direction.y * cos;
                direction.x = x;
                maxLength -= l;
                i++;
            }
            while (maxLength > float.Epsilon);
            return default(RaycastHit2D);
        }
        public static RaycastHit2D Curvecast(Vector2 origin, Vector2 direction, float segmentLength, int segmentCount, float tilt = 0f, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            direction.Normalize();
            tilt *= Mathf.Deg2Rad;
            float sin = Mathf.Sin(tilt);
            float cos = Mathf.Cos(tilt);
            for (int i = 0; i < segmentCount; i++)
            {
                RaycastHit2D hit = Physics2D.Linecast(origin, origin + direction * segmentLength, layerMask, minDepth, maxDepth);
                if (hit != default)
                {
                    return hit;
                }
                origin = origin + direction * segmentLength;
                float x = direction.x * cos - direction.y * sin;
                direction.y = direction.x * sin + direction.y * cos;
                direction.x = x;
            }
            return default(RaycastHit2D);
        }
        public static int CurvecastAll(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float segmentLength, float maxLength, float tilt = 0f, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            direction.Normalize();
            tilt *= Mathf.Deg2Rad;
            float sin = Mathf.Sin(tilt);
            float cos = Mathf.Cos(tilt);
            int i = 0;
            do
            {
                float l = maxLength > segmentLength ? segmentLength : maxLength;
                int c = Physics2D.LinecastNonAlloc(origin, origin + direction * l, results, layerMask, minDepth, maxDepth);
                if (c > 0)
                {
                    return c;
                }
                origin = origin + direction * l;
                float x = direction.x * cos - direction.y * sin;
                direction.y = direction.x * sin + direction.y * cos;
                direction.x = x;
                maxLength -= l;
                i++;
            }
            while (maxLength > float.Epsilon);
            return 0;
        }
        public static int CurvecastAll(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float segmentLength, int segmentCount, float tilt = 0f, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity)
        {
            direction.Normalize();
            tilt *= Mathf.Deg2Rad;
            float sin = Mathf.Sin(tilt);
            float cos = Mathf.Cos(tilt);
            for (int i = 0; i < segmentCount; i++)
            {
                int c = Physics2D.LinecastNonAlloc(origin, origin + direction * segmentLength, results, layerMask, minDepth, maxDepth);
                if (c > 0)
                {
                    return c;
                }
                origin = origin + direction * segmentLength;
                float x = direction.x * cos - direction.y * sin;
                direction.y = direction.x * sin + direction.y * cos;
                direction.x = x;
            }
            return 0;
        }
    }
}
// Author: Viktor Chernikov, 2023