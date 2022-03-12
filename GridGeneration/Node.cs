using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Graph graph;
    public string nodeID;

    // x position = nodePosition % graphWidth
    // y position = nodePosition / graphWidth
    public int nodePosition;

    public bool wall;

    public Node(int _x, int _y, Graph graph)
    {
        nodePosition = _x + (_y * graph.width);
        nodeID = System.Guid.NewGuid().ToString("N"); 
        float xCenter = graph.xTransform + ((float)_x*graph.sectorWidth) + (graph.sectorWidth/2);
        float yCenter = graph.yTransform - ((float)_y*graph.sectorWidth) - (graph.sectorWidth/2);
        Physics.SyncTransforms();
        Collider[] hitColliders = Physics.OverlapBox(new Vector3(xCenter,yCenter,graph.zTransform), new Vector3(graph.sectorWidth,graph.sectorWidth,.3f), Quaternion.identity, 0xffff);
        if( hitColliders.Length == 0 ) wall = false;
        else wall = true;
    }
}
