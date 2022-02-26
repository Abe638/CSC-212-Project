using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public int mazeLengthInTiles;
    public float tileLength;

    private bool[,] tiles;
    public bool validMaze;

    public bool getTile(int i, int j )
    {
        return tiles[i,j];
    }

    public Maze(float _tileLength, int _mazeLengthInTiles, float cutOff)
    {
        tileLength = _tileLength;
        mazeLengthInTiles = _mazeLengthInTiles;

        tiles = new bool[mazeLengthInTiles+2,mazeLengthInTiles+2];
        validMaze = false;
        int i = 0;
        while( validMaze == false && i < 1000)
        {
            validMaze = createMaze(cutOff);
            ++i;
        }
        if(validMaze)
        {
           Debug.Log("validMaze");
        }
        else
        {
            Debug.Log("InvalidMaze");
        }
    }
    public bool recreateMaze(float cutOff)
    {
        return createMaze(cutOff);
    }

    private bool createMaze(float cutOff)
    {
        for( int i = 0; i < mazeLengthInTiles+2; ++i )
        {
            for( int j = 0; j < mazeLengthInTiles+2; ++j )
            {
                if( i == 0 || i == mazeLengthInTiles+1
                    || j == 0 || j == mazeLengthInTiles+1 )
                    tiles[i,j] = false;
                else if( Random.Range(0.0f,1.0f) < cutOff ) tiles[i,j] = false;
                else tiles[i,j] = true;
            }
        }

        return testMazeViability();
    }

    private class Point
    {
        public int xPos;
        public int yPos;
        public float g;
        public float h;
        public float f;
        public bool wall;
        public bool closed;
        public bool open;
    }

    private Point createPoint( int _x, int _y )
    {
        Point point = new Point();
        point.xPos = _x;
        point.yPos = _y;
        if( tiles[ point.xPos, point.yPos ] == false ) point.wall = true;
        else point.wall = false;
        point.g = Mathf.Sqrt( Mathf.Pow((float)_x-1.0f,2)+Mathf.Pow((float)_y-1.0f,2) );
        point.h = Mathf.Sqrt( Mathf.Pow((float)mazeLengthInTiles-(float)_x,2)+Mathf.Pow((float)mazeLengthInTiles-(float)_y,2) );
        point.f = point.g+point.h;
        point.closed = false;
        point.open = false;
        return point;
    }

    private bool testMazeViability()
    {
        Point[,] calcList = new Point[mazeLengthInTiles+2,mazeLengthInTiles+2];
        Queue<Point> openList = new Queue<Point>();
        for( int i = 0; i < mazeLengthInTiles+2; ++i )
        {
            for( int j = 0; j < mazeLengthInTiles+2; ++j )
            {
                calcList[i,j] = createPoint(i,j);
            }
        }
        if( calcList[1,1].wall == true ) return false;
        calcList[1,1].open = true;
        openList.Enqueue(calcList[1,1]);
        while( openList.Count != 0)
        {
            Point currentNode = openList.Dequeue();
            currentNode.open = false;
            currentNode.closed = true;
            if ( currentNode.xPos == mazeLengthInTiles && currentNode.yPos == mazeLengthInTiles ) {
                return true;
            }
            // look through neighbors
            Point adjNode = calcList[currentNode.xPos+1,currentNode.yPos];
            if( adjNode.wall == false && adjNode.closed == false 
                && (adjNode.f < currentNode.h || adjNode.open == false) )
            {
                adjNode.open = true;
                openList.Enqueue(adjNode);
            }
            adjNode = calcList[currentNode.xPos,currentNode.yPos-1];
            if( adjNode.wall == false && adjNode.closed == false && (adjNode.f < currentNode.h || adjNode.open == false) )
            {
                adjNode.open = true;
                openList.Enqueue(adjNode);
            }
            adjNode = calcList[currentNode.xPos-1,currentNode.yPos];
            if( adjNode.wall == false && adjNode.closed == false && (adjNode.f < currentNode.h || adjNode.open == false) )
            {
                adjNode.open = true;
                openList.Enqueue(adjNode);
            }
            adjNode = calcList[currentNode.xPos,currentNode.yPos+1];
            if( adjNode.wall == false && adjNode.closed == false && (adjNode.f < currentNode.h || adjNode.open == false) )
            {
                adjNode.open = true;
                openList.Enqueue(adjNode);
            }
        }
        return false;
    }
}
