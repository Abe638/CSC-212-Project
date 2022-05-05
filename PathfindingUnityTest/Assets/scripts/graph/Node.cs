using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for the Node object
/// </summary>
public class Node
{
    /// <summary>
    /// bool declaring true/false for walkable tiles
    /// </summary>
    public bool ClassWalkable;
    /// <summary>
    /// coordinate position on grid
    /// </summary>
    public Vector3 ClassWorldPosition;
    /// <summary>
    /// x coordinate of grid position
    /// </summary>
    public int ClassGridX;
    /// <summary>
    /// y coordinate of grid position
    /// </summary>
    public int ClassGridY;
    /// <summary>
    /// cost of the path from the start node to currNode
    /// </summary>
    public int gCost;
    /// <summary>
    /// heuristic function that estimates the cost of the cheapest path from currNode to the goal
    /// </summary>
    public int hCost;
    /// <summary>
    /// Node object of parent node
    /// </summary>
    public Node parent;

    /// <summary>
    /// Local declaration of the Node object
    /// </summary>
    /// <param name="walkable"></param>
    /// <param name="worldPos"></param>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY) {
        ClassWalkable = walkable;
        ClassWorldPosition = worldPos;
        ClassGridX = gridX;
        ClassGridY = gridY;
    }

    /// <summary>
    /// Sum of g cost and h cost
    /// </summary>
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
