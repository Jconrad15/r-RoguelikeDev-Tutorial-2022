using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid
{
    public int width;
    public int height;

	public Tile[] Tiles { get; private set; }
    private List<RectangularRoom> rooms = new List<RectangularRoom>();
    private List<Hallway> hallways = new List<Hallway>();

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
        CreateHallways();

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                HexCoordinates coordinates =
                    HexCoordinates.FromOffsetCoordinates(x, y);

                if (IsPointInRoom(x, y))
                {
                    Tiles[i] = new Tile(TileType.Floor, coordinates);
                }
                else if (IsPointInHallway(x, y))
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

    private void CreateHallways()
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            hallways.Add(new Hallway(rooms[i - 1].Center, rooms[i].Center));
        }
    }

    private void CreateRooms()
    {
        // Create two rooms
        int roomCount = Random.Range(6, 10);

        for (int i = 0; i < roomCount; i++)
        {
            int roomWidth = Random.Range(8, 12);
            int roomHeight = Random.Range(8, 12);
            int minX = Random.Range(1, width - roomWidth);
            int minY = Random.Range(1, height - roomHeight);

            rooms.Add(new RectangularRoom(
                minX, minY, roomWidth, roomHeight));
        }
    }

    private bool IsPointInHallway(int y, int x)
    {
        foreach (Hallway h in hallways)
        {
            if (h.Contains(x, y))
            {
                return true;
            }
        }

        return false;
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

    /// <summary>
    /// Returns tile for the center of a room.
    /// </summary>
    /// <returns></returns>
    public Tile GetRandomRoomCenterTile()
    {
        if (rooms == null) { return null; }
        if (rooms.Count == 0) { return null; }

        RectangularRoom selectedRoom = rooms[Random.Range(0, rooms.Count)];

        (int, int) center = selectedRoom.Center;
        HexCoordinates coordinates = HexCoordinates.FromOffsetCoordinates(
            center.Item1, center.Item2);

        return GetTileAtHexCoords(coordinates);
    }

}
