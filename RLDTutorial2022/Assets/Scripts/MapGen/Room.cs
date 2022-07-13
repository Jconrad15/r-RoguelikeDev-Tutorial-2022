using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room
{
    public List<(int, int)> InnerArea { get; protected set; }

    /// <summary>
    /// Center in offset coordinates.
    /// </summary>
    public (int, int) Center { get; protected set; }

    protected abstract void CreateInnerArea();

    public bool Contains(int x, int y)
    {
        return InnerArea.Contains((x, y));
    }
}
