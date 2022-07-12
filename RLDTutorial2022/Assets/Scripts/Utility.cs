using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Utility
{
    public static Color32 DarkenColor(Color32 edgeMainColor)
    {
        Color.RGBToHSV(edgeMainColor, out float h, out float s, out float v);

        v -= 10f / 100f;
        s -= 5f / 100f;
        Color32 newColor = Color.HSVToRGB(h, s, v);

        return newColor;
    }

    public static Color32 LightenColor(Color32 edgeMainColor)
    {
        Color.RGBToHSV(edgeMainColor, out float h, out float s, out float v);

        v += 10f / 100f;
        s += 5f / 100f;
        Color32 newColor = Color.HSVToRGB(h, s, v);

        return newColor;
    }

    /// <summary>
    /// Returns a random color.
    /// </summary>
    /// <returns></returns>
    public static Color32 RandomColor()
    {
        return new Color32(
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
            (byte)Random.Range(0, 255),
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
        T selectedEnum = (T)enumArray.GetValue(UnityEngine.Random.Range(0, enumArray.Length));
        return selectedEnum;
    }
}