using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    public int width = 6;
    public int height = 6;

	public Tile[] Tiles { get; private set; }

	public TileGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

		CreateGrid();
    }

    private void CreateGrid()
	{
		Tiles = new Tile[height * width];

		for (int y = 0, i = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
                HexCoordinates coordinates =
					HexCoordinates.FromOffsetCoordinates(x, y);

				Tiles[i] = new Tile(coordinates);
                i++;
			}
        }
	}

    public Tile GetTileAtPos(int x, int y)
    {
		int selectedIndex = x + (y * width);

		if (selectedIndex < 0 || selectedIndex >= Tiles.Length)
        {
			return null;
        }

		return Tiles[selectedIndex];
    }

    public Tile GetTileAtHexCoords(HexCoordinates hexCoord)
    {
        int offsetCoordY = hexCoord.Z;

        int offsetCoordX = hexCoord.X + (offsetCoordY / 2);

        return GetTileAtPos(offsetCoordX, offsetCoordY);
    }

    public Tile GetTileInDirection(Tile startTile, Direction direction)
    {
        HexCoordinates startCoords = startTile.coordinates;
        HexCoordinates endCoords;

        switch (direction)
        {
            case Direction.NE:
                endCoords = new HexCoordinates(
                    startCoords.X, startCoords.Z + 1);
                break;
                
            case Direction.E:
                endCoords = new HexCoordinates(
                    startCoords.X + 1, startCoords.Z);
                break;

            case Direction.SE:
                endCoords = new HexCoordinates(
                    startCoords.X + 1, startCoords.Z - 1);
                break;

            case Direction.SW:
                endCoords = new HexCoordinates(
                    startCoords.X, startCoords.Z - 1);
                break;

            case Direction.W:
                endCoords = new HexCoordinates(
                    startCoords.X - 1, startCoords.Z);
                break;

            case Direction.NW:
                endCoords = new HexCoordinates(
                    startCoords.X - 1, startCoords.Z + 1);
                break;

            default:
                return null;
        }

        return GetTileAtHexCoords(endCoords);
    }

}
