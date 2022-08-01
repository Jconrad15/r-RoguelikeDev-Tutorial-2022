using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path_TileGraph
{
    // This class constructs a simple path-finding compatible graph
    // of the area. Each tile is a node. Each walkable neighbour
    // from a tile is linked via an edge connection.

    public Dictionary<Tile, Path_Node<Tile>> NodesDict
    { get; protected set; }

    public Path_TileGraph(TileGrid tileGrid)
    {
        // Loop through all tiles of the areaData
        // For each tile, create a node

        // Instantiate dictionary map
        NodesDict = new Dictionary<Tile, Path_Node<Tile>>();

        for (int x = 0; x < tileGrid.width; x++)
        {
            for (int y = 0; y < tileGrid.height; y++)
            {
                Tile t = tileGrid.GetTileAtPos(x, y);
                if (t == null)
                {
                    Debug.LogError("Null tile");
                    continue;
                }

                // Don't add not walkable tiles
                if (t.IsWalkable == false)
                {
                    continue;
                }

                Path_Node<Tile> n = new Path_Node<Tile>
                {
                    data = t
                };

                // Add to dictionary
                NodesDict.Add(t, n);
            }
        }
        //Debug.Log("Path_TileGraph: Created " + NodesDict.Count + " nodes.");

        // Now loop through all nodes
        // Create edges
        int edgeCount = 0;

        foreach (Tile t in NodesDict.Keys)
        {
            Path_Node<Tile> n = NodesDict[t];

            List<Path_Edge<Tile>> edges =
                new List<Path_Edge<Tile>>();

            // Get a list of neighbors for the tile
            // Note, some of the array spots could be null.
            Tile[] neighbors = t.GetNeighboringTiles();

            // If neighbour is walkable,
            // create an edge to the relevant node.
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (neighbors[i] != null &&
                    neighbors[i].IsWalkable)
                {
                    // This neighbor exists and is walkable,
                    // so create an edge.

                    Path_Edge<Tile> e = new Path_Edge<Tile>();
                    e.cost = 1; // neighbors[i].MovementCost;
                    e.node = NodesDict[neighbors[i]];

                    // Add the edge to the temporary
                    // and growable list
                    edges.Add(e);
                    edgeCount++;
                }

            }
            // Add edges to node object
            n.edges = edges.ToArray();

        }
        //Debug.Log("Path_TileGraph: Created " + edgeCount + " edges");
    }

}
