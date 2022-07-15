using UnityEngine;

public enum Direction
{
    NE,
    E,
    SE,
    SW,
    W,
    NW
}

public static class DirectionExtension 
{
    public static Vector2 UnitDirection(this Direction direction)
    {
        switch (direction)
        {
            case Direction.NE:
                return new Vector2(0.5f, Mathf.Sqrt(3)/2f);

            case Direction.E:
                return new Vector2(1, 0);

            case Direction.SE:
                return new Vector2(0.5f, -Mathf.Sqrt(3) / 2f);

            case Direction.SW:
                return new Vector2(-0.5f, -Mathf.Sqrt(3) / 2f);

            case Direction.W:
                return new Vector2(-1, 0);

            case Direction.NW:
                return new Vector2(-0.5f, Mathf.Sqrt(3) / 2f);

            // Default should never happen
            default:
                return new Vector2(0, 0);
        }
    }
}