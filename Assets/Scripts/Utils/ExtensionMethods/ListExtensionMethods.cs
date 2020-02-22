using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensionMethods
{
    public static bool HasAnyElement<T>(this List<T> list)
    {
        return list.Count > 0;
    }

    public static T PopFirstElement<T>(this List<T> list)
    {
        T firstElement = default(T);

        if(list.HasAnyElement() == true)
        {
            firstElement = list[0];
            list.RemoveAt(0);
        }

        return firstElement;
    }

    public static void DestroyAll<T>(this List<T> list) where T : MonoBehaviour
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(list[i].gameObject);
            list.RemoveAt(i);
        }
    }

    public static T PickRandomElement<T>(this List<T> list)
    {
        T randomElement = default(T);

        if(list.HasAnyElement() == true)
        {
            randomElement = list[Random.Range(0, list.Count)];
        }

        return randomElement;
    }
}
