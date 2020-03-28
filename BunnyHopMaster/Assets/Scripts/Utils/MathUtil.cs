using UnityEngine;

public static class MathUtil
{
    public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
    {
        return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
    }

    public static Vector4 PosToV4(Vector3 v) { return new Vector4(v.x, v.y, v.z, 1.0f); }
    public static Vector3 ToV3(Vector4 v) { return new Vector3(v.x, v.y, v.z); }

    public static Vector3 ZeroV3 = new Vector3(0.0f, 0.0f, 0.0f);
    public static Vector3 OneV3 = new Vector3(1.0f, 1.0f, 1.0f);
<<<<<<< HEAD

    public static bool DoBoxesIntersect(Collider a, Collider b)
    {
        Vector3 aMin = a.bounds.min;
        Vector3 aMax = a.bounds.max;
        Vector3 bMin = a.bounds.min;
        Vector3 bMax = b.bounds.max;

        bool xIntersect = aMin.x < bMax.x && aMax.x < bMin.x;
        bool yIntersect = aMin.y < bMax.y && aMax.y < bMin.y;
        bool zIntersect = aMin.z < bMax.z && aMax.z < bMin.z;

        if (xIntersect && yIntersect && zIntersect)
        {
            return true;
        }

        return false;
    }
=======
>>>>>>> 5f7eea4... Initial commit
}
