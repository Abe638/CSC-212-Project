using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool ClassWalkable;
    public Vector3 ClassWorldPosition;
    public int ClassGridX;
    public int ClassGridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool walkable, Vector3 worldPos, int gridX, int gridY) {
        ClassWalkable = walkable;
        ClassWorldPosition = worldPos;
        ClassGridX = gridX;
        ClassGridY = gridY;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
