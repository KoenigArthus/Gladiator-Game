using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtension
{
    public static int IndexOf<T>(this T[] array, T value)
    {
        if (value == null)
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i] == null)
                    return i;
        }
        else
        {
            for (int i = 0; i < array.Length; i++)
                if (array[i].Equals(value))
                    return i;
        }

        return -1;
    }

    public static void Shuffle<T>(this T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}