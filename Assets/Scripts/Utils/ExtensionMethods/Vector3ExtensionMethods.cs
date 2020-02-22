using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3ExtensionMethods
{
    public static Vector3 Inverse(this Vector3 vector)
    {
        return -vector;
    }

    public static Vector3 InverseX(this Vector3 vector)
    {
        Vector3 newVector = vector;
        newVector.x = -vector.x;

        return newVector;
    }

    public static Vector3 InverseY(this Vector3 vector)
    {
        Vector3 newVector = vector;
        newVector.y = -vector.y;

        return newVector;
    }

    public static Vector3 InverseZ(this Vector3 vector)
    {
        Vector3 newVector = vector;
        newVector.z = -vector.z;

        return newVector;
    }
}
