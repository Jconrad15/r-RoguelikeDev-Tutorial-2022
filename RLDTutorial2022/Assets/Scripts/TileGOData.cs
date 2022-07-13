using UnityEngine;

public struct TileGOData
{
    public GameObject tileGO;
    public Tile tile;

    public TileGOData(GameObject tileGO, Tile tile)
    {
        this.tileGO = tileGO;
        this.tile = tile;
    }

    public bool ContainsTile(Tile compareTile)
    {
        return compareTile == tile;
    }
}