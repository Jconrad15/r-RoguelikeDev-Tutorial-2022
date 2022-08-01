using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
	[SerializeField]
	private int x, z;

	public int X
	{
		get
		{
			return x;
		}
	}

	public int Y
	{
		get
		{
			return -X - Z;
		}
	}

	public int Z
    {
		get
		{
			return z;
		}
	}

	public HexCoordinates(int x, int z)
	{
		this.x = x;
        this.z = z;
	}

	public static HexCoordinates FromOffsetCoordinates(int x, int y)
	{
        return new HexCoordinates(x - (y / 2), y);
    }

	public static (int, int) ToOffsetCoordinates(
		HexCoordinates hexCoordinates)
    {
		// hexX = x - (y/2)
		// hexZ = y
		return ((hexCoordinates.Z / 2) + hexCoordinates.X,
				 hexCoordinates.Z);
    }

	public Vector3 GetWorldPosition()
    {
		float offsetY = Z;
		float offsetX = X + (offsetY / 2);

        Vector3 position;
        position.x = offsetX * (HexMetrics.innerRadius * 2f);
		position.y = offsetY * (HexMetrics.outerRadius * 1.5f);
		position.z = 0;

		return position;
    }

	public static int HexDistance(HexCoordinates a, HexCoordinates b)
	{
		return (Mathf.Abs(a.X - b.X) + 
				Mathf.Abs(a.Y - b.Y) + 
				Mathf.Abs(a.Z - b.Z)) 
				/ 2;
	}

    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;

		float offset = position.y / (HexMetrics.outerRadius * 3f);
		x -= offset;
		y -= offset;

        int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(y);
		int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
		{
			float dX = Mathf.Abs(x - iX);
			float dY = Mathf.Abs(y - iY);
			float dZ = Mathf.Abs(-x - y - iZ);

			if (dX > dY && dX > dZ)
			{
				iX = -iY - iZ;
			}
			else if (dZ > dY)
			{
				iZ = -iX - iY;
			}
		}

		return new HexCoordinates(iX, iZ);
	}

	public override string ToString()
	{
		return "(" +
			X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines()
	{
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}
}