using UnityEngine;

namespace Kalkuz.Utility.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ProjectXY(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.y);
        }
        
        public static Vector3 ProjectXZ(this Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }
        
        public static Vector3 ProjectYZ(this Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }

        public static float RotationAngleXY(this Vector3 a, Vector3 b, bool inDegrees = false)
        {
            var angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * (inDegrees ? Mathf.Rad2Deg : 1);
            if (angle < 0) angle += 360;
            return angle;
        }
        
        public static float RotationAngleXZ(this Vector3 a, Vector3 b, bool inDegrees = false)
        {
            var angle = Mathf.Atan2(b.z - a.z, b.x - a.x) * (inDegrees ? Mathf.Rad2Deg : 1);
            if (angle < 0) angle += 360;
            return angle;
        }
        
        public static float RotationAngleYZ(this Vector3 a, Vector3 b, bool inDegrees = false)
        {
            var angle = Mathf.Atan2(b.y - a.y, b.z - a.z) * (inDegrees ? Mathf.Rad2Deg : 1);
            if (angle < 0) angle += 360;
            return angle;
        }

        public static Vector3 Direct(this Vector3 vector, Vector3 direction)
        {
            return direction.normalized * vector.magnitude;
        }
    }
}