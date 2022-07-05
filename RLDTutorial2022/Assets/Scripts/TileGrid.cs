using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    public int width;
    public int height;

	public Tile[] Tiles { get; private set; }
    private List<RectangularRoom> rooms = new List<RectangularRoom>();

    public TileGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

		CreateGrid();
    }

    private void CreateGrid()
    {
        Tiles = new Tile[height * width];

        CreateRooms();

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                HexCoordinates coordinates =
                    HexCoordinates.FromOffsetCoordinates(x, y);

                if (IsPointInRoom(y, x))
                {
                    Tiles[i] = new Tile(TileType.Floor, coordinates);
                }
                else
                {
                    Tiles[i] = new Tile(TileType.Wall, coordinates);
                }

                i++;
            }
        }
    }

    private void CreateRooms()
    {
        // Create two rooms
        rooms.Add(new RectangularRoom(10, 15, 10, 15));
        rooms.Add(new RectangularRoom(25, 15, 10, 15));
    }

    private bool IsPointInRoom(int y, int x)
    {
        foreach (RectangularRoom room in rooms)
        {
            if (room.Contains(x, y))
            {
                return true;
            }
        }

        return false;
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
        HexCoordinates startCoords = startTile.Coordinates;
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
