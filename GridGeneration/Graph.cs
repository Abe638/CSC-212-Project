using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    // transform location
    public float xTransform;
    public float yTransform;
    public float zTransform;
    public int width;
    public float sectorWidth;
    
    public Node[] nodes;

    public Graph(float _x,float _y,float _z,int _width, float _sectorWidth)
    {
        //fill variables
        xTransform =_x;
        yTransform = _y;
        zTransform = _z;
        width = _width;
        sectorWidth = _sectorWidth;
        //fill nodes with Nodes
        nodes = new Node[width*width];
        for( int i = 0; i < width; ++i ) // y
        {
            for( int j = 0; j < width; ++j ) // x
            {
                nodes[j + (i*width)] = new Node(j,i,this);
            }
        }
    }
}
