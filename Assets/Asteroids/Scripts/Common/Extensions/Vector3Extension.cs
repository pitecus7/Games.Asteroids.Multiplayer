
using UnityEngine;

public static class Vector3Extension
{
    public static float GetAngleFromVector(this Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static float GetAngleFromVector(this Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static float Angle360(Vector2 _vectorOne, Vector2 axis)
    {
        Vector2 v1, v2;

        v1 = _vectorOne.normalized;
        v2 = axis.normalized;

        float angle = Vector2.Angle(v1, v2);
        return Mathf.Sign(Vector3.Cross(v1, v2).z) < 0 ? (360 - angle) % 360 : angle;
    }
}
