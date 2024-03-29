using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class TileGrid
{
    public int width;
    public int height;

    [JsonIgnore]
    public Path_TileGraph TileGraph { get; private set; }

	public Tile[] Tiles { get; private set; }

    public (int, int) downStairsXY;

    [JsonIgnore]
    private List<RectangularRoom> rectRooms =
        new List<RectangularRoom>();
    [JsonIgnore]
    private List<HexRoom> hexRooms =
        new List<HexRoom>();
    [JsonIgnore]
    private List<Hallway> hallways =
        new List<Hallway>();

    public void CreateNewTileGraph()
    {
        TileGraph = new Path_TileGraph(
            GameManager.Instance.CurrentGrid);
    }

    public TileGrid(int width, int height, int seed)
    {
        this.width = width;
        this.height = height;

        CreateGrid(seed);
    }

    /// <summary>
    /// Create tile grid from saved data.
    /// </summary>
    /// <param name="saveObject"></param>
    public TileGrid(SaveObject saveObject)
    {
        width = saveObject.savedTileGrid.width;
        height = saveObject.savedTileGrid.height;

        // Load tiles
        SavedTile[] savedTiles = saveObject.savedTileGrid.savedTiles;
        Tiles = new Tile[savedTiles.Length];
        for (int i = 0; i < savedTiles.Length; i++)
        {
            Tiles[i] = new Tile(savedTiles[i]);
        }
    }

    private void CreateGrid(int seed)
    {
        Random.State oldState = Random.state;
        Random.InitState(seed);

        Tiles = new Tile[height * width];

        CreateRooms(seed);
        CreateHallways(seed);

        for (int y = 0, i = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                HexCoordinates coordinates =
                    HexCoordinates.FromOffsetCoordinates(x, y);

                if (IsDownStairs(x, y))
                {
                    Tiles[i] = new Tile(
                        TileType.DownStairs, coordinates);
                }
                else if (IsPointInRoom(x, y))
                {
                    Tiles[i] = new Tile(
                        TileType.Floor, coordinates);
                }
                else if (IsPointInHallway(x, y))
                {
                    Tiles[i] = new Tile(
                        TileType.Floor, coordinates);
                }
                else
                {
                    Tiles[i] = new Tile(
                        TileType.Wall, coordinates);
                }

                // debug
                if (IsCenterPoint(x, y))
                {
                    Tiles[i].backgroundColor = 
                        ColorDatabase.roomCenterTile;
                }

                i++;
            }
        }

        Random.state = oldState;
    }

    private bool IsDownStairs(int x, int y)
    {
        return x == downStairsXY.Item1 &&
               y == downStairsXY.Item2;
    }

    private bool IsCenterPoint(int x, int y)
    {
        foreach (RectangularRoom room in rectRooms)
        {
            if (room.Center.Item1 == x && room.Center.Item2 == y)
            {
                return true;
            }
        }
        foreach(HexRoom room in hexRooms)
        {
            if (room.Center.Item1 == x && room.Center.Item2 == y)
            {
                return true;
            }
        }

        return false;
    }

    private void CreateHallways(int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        // Rectangular rooms
        for (int i = 1; i < rectRooms.Count; i++)
        {
            hallways.Add(new Hallway(
                rectRooms[i - 1].Center,
                rectRooms[i].Center));
        }

        // also add hallway from first to last rectRoom
        hallways.Add(new Hallway(
            rectRooms[0].Center,
            rectRooms[rectRooms.Count - 1].Center));

        // Hexagonal rooms
        for (int i = 1; i < hexRooms.Count; i++)
        {
            hallways.Add(new Hallway(
                hexRooms[i - 1].Center,
                hexRooms[i].Center));
        }
        
        
        // also add hallway from last rect room to first hex room
        hallways.Add(new Hallway(
            rectRooms[0].Center,
            hexRooms[hexRooms.Count - 1].Center));

        // Restore state
        Random.state = oldState;
    }

    private void CreateRooms(int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        int rectRoomCount = Random.Range(7, 9);
        int hexRoomCount = Random.Range(5, 8);

        CreateRectRooms(rectRoomCount, seed);
        CreateHexRooms(hexRoomCount, seed);

        // Restore state
        Random.state = oldState;
    }

    private void CreateDownStairsLocation(Room room)
    {
        // Place down stairs at random place in room
        downStairsXY = room.GetRandomCoordInRoom();
    }

    private void CreateHexRooms(int hexRoomCount, int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        // Determine which room to place down stairs in
        int downStairsRoomIndex = Random.Range(0, hexRoomCount);

        for (int i = 0; i < hexRoomCount; i++)
        {
            int radius = Random.Range(2, 5);

            int x = Random.Range(radius + 1, width - radius - 1);
            int y = Random.Range(radius + 1, height - radius - 1);
            (int, int) center = (x, y);

            HexRoom newRoom = new HexRoom(radius, center);

            // Add room even if there are intersections
            hexRooms.Add(newRoom);

            // If this is the selected room, place down stairs
            if (downStairsRoomIndex == i)
            {
                CreateDownStairsLocation(newRoom);
            }
        }

        // Restore state
        Random.state = oldState;
    }

    private void CreateRectRooms(int rectRoomCount, int seed)
    {
        // Create seed based state
        Random.State oldState = Random.state;
        Random.InitState(seed);

        int maxIterations = 100;

        for (int i = 0; i < rectRoomCount; i++)
        {
            int counter = 0;
            bool isRoomCreated = false;
            while (isRoomCreated == false)
            {
                int roomWidth = Random.Range(5, 9);
                int roomHeight = Random.Range(5, 9);
                int minX = Random.Range(1, width - roomWidth);
                int minY = Random.Range(1, height - roomHeight);

                RectangularRoom newRoom = new RectangularRoom(
                    minX, minY, roomWidth, roomHeight);

                // Add room if no intersections
                if (IntersectionCheck(newRoom) == false)
                {
                    rectRooms.Add(newRoom);
                    isRoomCreated = true;
                }

                // Too many loop - exit
                if (counter >= maxIterations)
                {
                    break;
                }
                counter++;
            }
        }

        // Restore state
        Random.state = oldState;
    }

    /// <summary>
    /// Return true if new room intersects an existing room.
    /// </summary>
    /// <param name="newRoom"></param>
    /// <returns></returns>
    private bool IntersectionCheck(RectangularRoom newRoom)
    {
        foreach (RectangularRoom room in rectRooms)
        {
            if (room.IntersectsRectangularRoom(newRoom))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPointInHallway(int x, int y)
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

    private bool IsPointInRoom(int x, int y)
    {
        foreach (RectangularRoom rectRoom in rectRooms)
        {
            if (rectRoom.Contains(x, y))
            {
                return true;
            }
        }

        foreach(HexRoom hexRoom in hexRooms)
        {
            if (hexRoom.Contains(x, y))
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

    public Tile GetTileInDirection(
        Tile startTile, Direction direction)
    {
        HexCoordinates startCoords = startTile.Coordinates;
        HexCoordinates endCoords;

        // Check if at edge
        (int, int) offsetStartCoords =
            HexCoordinates.ToOffsetCoordinates(startCoords);

        if (direction == Direction.NE ||
            direction == Direction.E ||
            direction == Direction.SE)
        {
            if (offsetStartCoords.Item1 >= width - 2)
            {
                return null;
            }
        }

        if (direction == Direction.NW ||
            direction == Direction.W ||
            direction == Direction.SW)
        {
            if (offsetStartCoords.Item1 <= 0)
            {
                return null;
            }
        }

        if (direction == Direction.NE ||
            direction == Direction.NW )
        {
            if (offsetStartCoords.Item2 >= height - 2)
            {
                return null;
            }
        }

        if (direction == Direction.SE ||
            direction == Direction.SW)
        {
            if (offsetStartCoords.Item2 <= 0)
            {
                return null;
            }
        }

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
        Room[] allRooms = GetAllRoomsArray();
        if (allRooms == null)
        {
            return null;
        }

        Room selectedRoom =
            allRooms[Random.Range(0, allRooms.Length)];

        (int, int) center = selectedRoom.Center;

        return GetTileAtPos(center.Item1, center.Item2);
    }

    public Room[] GetAllRoomsArray()
    {
        if (rectRooms == null || hexRooms == null)
        { return null; }
        if (rectRooms.Count == 0 && hexRooms.Count == 0)
        { return null; }

        Room[] allRooms = new Room[rectRooms.Count + hexRooms.Count];
        for (int i = 0; i < allRooms.Length; i++)
        {
            if (i < rectRooms.Count)
            {
                allRooms[i] = rectRooms[i];
            }
            else
            {
                allRooms[i] = hexRooms[i - rectRooms.Count];
            }
        }

        return allRooms;
    }

    public Tile[] GetAllRoomCenterTiles()
    {
        Room[] allRooms = GetAllRoomsArray();
        if (allRooms == null)
        {
            return null;
        }

        Tile[] roomCenterTiles = new Tile[allRooms.Length];
        for (int i = 0; i < allRooms.Length; i++)
        {
            (int, int) center = allRooms[i].Center;
            Tile t = GetTileAtPos(center.Item1, center.Item2);

            roomCenterTiles[i] = t;
        }

        return roomCenterTiles;
    }

    public void Destroy()
    {
        TileGraph = null;
        rectRooms = null;
        hexRooms = null;
        hallways = null;

        for (int i = Tiles.Length - 1; i >= 0; i--)
        {
            if (Tiles[i] == null) { continue; }
            Tiles[i].Destroy();
        }
    }

}
