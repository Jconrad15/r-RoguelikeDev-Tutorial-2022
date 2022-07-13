using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Color32 DarkenColor(
        Color32 startColor, int iterations)
    {
        Color.RGBToHSV(startColor, out float h, out float s, out float v);

        Color32 newColor = startColor;
        for (int i = 0; i < iterations; i++)
        {
            v -= 10f / 100f;
            s -= 5f / 100f;
            newColor = Color.HSVToRGB(h, s, v);
        }

        return newColor;
    }

    public static Color32 LightenColor(
        Color32 startColor, int iterations)
    {
        Color.RGBToHSV(startColor, out float h, out float s, out float v);

        Color32 newColor = startColor;
        for (int i = 0; i < iterations; i++)
        {
            v += 10f / 100f;
            s += 5f / 100f;
            newColor = Color.HSVToRGB(h, s, v);
        }

        return newColor;
    }

    /// <summary>
    /// Returns a random color.
    /// </summary>
    /// <returns></returns>
    public static Color32 RandomColor()
    {
        return new Color32(
            (byte)UnityEngine.Random.Range(0, 255),
            (byte)UnityEngine.Random.Range(0, 255),
            (byte)UnityEngine.Random.Range(0, 255),
            255);
    }

    /// <summary>
    /// Returns a random Enum of given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRandomEnum<T>()
    {
        Array enumArray = Enum.GetValues(typeof(T));
        T selectedEnum = (T)enumArray.GetValue(
            UnityEngine.Random.Range(0, enumArray.Length));
        return selectedEnum;
    }

    /// <summary>
    /// Shuffle array values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rng"></param>
    /// <param name="array"></param>
    public static void ShuffleArray<T>(T[] array)
    {
        var rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}