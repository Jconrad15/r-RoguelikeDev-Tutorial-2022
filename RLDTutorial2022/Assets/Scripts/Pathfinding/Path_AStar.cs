using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using System.Linq;
using System;

public class Path_AStar
{
    private static readonly float HeuristicWeight = 2f;
    private Queue<Tile> path;

    /// <summary>
    /// Constructor that creates path to destination.
    /// </summary>
    /// <param name="tileStart"></param>
    /// <param name="tileEnd"></param>
    public Path_AStar(Tile tileStart, Tile tileEnd)
    {
        if (tileStart == null || tileEnd == null)
        {
            Debug.Log("tile Start or End is null");
            return;
        }

        // Get pathfinding tile graph from grid
        Path_TileGraph tileGraph =
            GameManager.Instance.CurrentGrid.TileGraph;

        // Check to see if there is a valid tile graph
        if (tileGraph == null)
        {
            GameManager.Instance.CurrentGrid.CreateNewTileGraph();
            tileGraph = GameManager.Instance.CurrentGrid.TileGraph;
        }

        // Get a dictionary of all valid, walkable nodes.
        Dictionary<Tile, Path_Node<Tile>> nodesDict =
            tileGraph.NodesDict;

        // Make sure the start/end tiles are in the list of nodes
        if (nodesDict.ContainsKey(tileStart) == false)
        {
            Debug.LogError("Path_AStar: The starting tile " +
                "isn't in the list of nodes");
            return;
        }

        // Set initial start Path_node 
        Path_Node<Tile> start = nodesDict[tileStart];
        Path_Node<Tile> goal = null;

        // If tileEnd is null, something bad may happen?
        if (tileEnd != null)
        {
            if (nodesDict.ContainsKey(tileEnd) == false)
            {
                Debug.LogError("Path_AStar: The ending tile " +
                    "isn't in the list of nodes");
                return;
            }
            goal = nodesDict[tileEnd];
        }

        // A star algorithm

        List<Path_Node<Tile>> ClosedSet =
            new List<Path_Node<Tile>>();

        SimplePriorityQueue<Path_Node<Tile>> OpenSet =
            new SimplePriorityQueue<Path_Node<Tile>>();
        OpenSet.Enqueue(start, 0); // priority is the f_score

        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From =
            new Dictionary<Path_Node<Tile>, Path_Node<Tile>>();

        // Create g_score and f_score dictionaries
        Dictionary<Path_Node<Tile>, float> g_score =
            new Dictionary<Path_Node<Tile>, float>();
        Dictionary<Path_Node<Tile>, float> f_score =
            new Dictionary<Path_Node<Tile>, float>();

        foreach (Path_Node<Tile> n in nodesDict.Values)
        {
            g_score[n] = Mathf.Infinity;
            f_score[n] = Mathf.Infinity;
        }

        g_score[start] = 0;
        f_score[start] = Heuristic_cost_estimate(start, goal);

        while (OpenSet.Count > 0)
        {
            Path_Node<Tile> current = OpenSet.Dequeue();

            // If we have a positional goal,
            // check to see if we are there
            if (goal != null)
            {
                if (current == goal)
                {
                    // Goal has been reached
                    // Convert this into an actual sequence
                    // of tiles to walk on
                    Reconstruct_Path(Came_From, current);
                    return;
                }
            }

            // The lists in the following foreach
            // loop can cause massive slowdowns.

            ClosedSet.Add(current);
            foreach (Path_Edge<Tile> edge_neighbour in current.edges)
            {
                Path_Node<Tile> neighbor = edge_neighbour.node;

                // If the set already includes the neighbor node,
                // then ignore it.
                if (ClosedSet.Contains(neighbor) == true)
                {
                    continue;
                }

                // If entity is in tile, and blocks movement
                // increase the movement score, so that this entity
                // is more likely to go around.
                float entityScore = 0f;
                if (neighbor.data.entity != null)
                {
                    if (neighbor.data.entity.BlocksMovement == true)
                    {
                        entityScore += 10f;
                    }
                }

                float movement_cost_to_neighbour =
                    Dist_Between(current, neighbor);
                
                if (movement_cost_to_neighbour > 1)
                {
                    Debug.LogError(
                        "Cost to neighbor is greater than 1.");
                }

                float tentative_g_score =
                    g_score[current] +
                    movement_cost_to_neighbour + 
                    entityScore;

                if (OpenSet.Contains(neighbor) &&
                    tentative_g_score >= g_score[neighbor])
                {
                    continue;
                }

                Came_From[neighbor] = current;
                g_score[neighbor] = tentative_g_score;
                f_score[neighbor] =
                    tentative_g_score +
                    (HeuristicWeight *
                    Heuristic_cost_estimate(neighbor, goal));

                if (OpenSet.Contains(neighbor) == false)
                {
                    OpenSet.Enqueue(neighbor, f_score[neighbor]);
                }
                else
                {
                    //upate the f score
                    OpenSet.UpdatePriority(
                        neighbor, f_score[neighbor]);
                }

            } // End foreach neighbour
        } // End While

        // We reached here, it means that we've burned through the entire
        // OpenSet without ever reaching a point where current == goal.
        // This happens when there is no path from start to goal
        // (so there is a wall or missing floor or something).

        // No failure state. The returned path is just null.

        // Explicitly set here, not needed.
        Debug.Log("No path");
        path = null;
    }

    /// <summary>
    /// Function to determine the movement cost estimate between two points.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Heuristic Value</returns>
    private float Heuristic_cost_estimate(
        Path_Node<Tile> a, Path_Node<Tile> b)
    {
        if (b == null)
        {
            // We have no fixed destination
            // just return 0 for the cost estimate
            // (all directions are just as good)
            return 0f;
        }

        return HexCoordinates.HexDistance(
            a.data.Coordinates,
            b.data.Coordinates);
    }

    /// <summary>
    /// Function to determine the distance between two points
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>Distance Value</returns>
    private float Dist_Between(Path_Node<Tile> a, Path_Node<Tile> b)
    {
        return HexCoordinates.HexDistance(
            a.data.Coordinates,
            b.data.Coordinates);
    }

    /// <summary>
    /// Function to create the path based on the determined sequence tiles.
    /// </summary>
    /// <param name="Came_From"></param>
    /// <param name="current"></param>
    private void Reconstruct_Path(
        Dictionary<Path_Node<Tile>, Path_Node<Tile>> Came_From, 
        Path_Node<Tile> current)
    {
        // At this point, current is the goal,
        // So what we want to do is walk backwards
        // through the Came_From map,
        // until we reach the 'end' of that map...
        // which will be the starting node.
        Queue<Tile> total_Path = new Queue<Tile>();
        total_Path.Enqueue(current.data); 
        // This final step of the path is the goal.

        while (Came_From.ContainsKey(current))
        {
            // Came_From is a map where the
            // key => value relation is really saying
            // some_node => we_got_there_from_this_node
            current = Came_From[current];
            total_Path.Enqueue(current.data);
        }

        // At this point, total_path is a queue that is running
        // backwards from the END tile to the START tile,
        // so reverse it
        path = new Queue<Tile>(total_Path.Reverse());
    }

    /// <summary>
    /// Returns the next tile in the path queue.
    /// </summary>
    /// <returns>Dequeued Tile</returns>
    public Tile Dequeue()
    {
        if(path == null) 
        {
            Debug.LogError(
                "Attempting to dequeue from a null path.");
            return null; 
        }
        if (path.Count <= 0)
        {
            Debug.LogError("Trying to dequeue from a path " +
                "with count 0 or less.");
            return null;
        }

        return path.Dequeue();
    }

    /// <summary>
    /// Returns the current length of the path.
    /// </summary>
    /// <returns>Length Value</returns>
    public int Length()
    {
        if (path == null)
        {
            return 0;
        }
        return path.Count;
    }

    public Tile Peek()
    {
        return path.Count > 0 ? path.Peek() : null;
    }

    /// <summary>
    /// Returns the last tile in the path.
    /// </summary>
    /// <returns>path.Last</returns>
    public Tile EndTile()
    {
        if (path == null || path.Count == 0) { return null; }
        return path.Last();
    }

}
