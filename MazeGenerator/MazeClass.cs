using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// This class handles the genereation of the mazes
///
/// 
public class Maze
{
    /// <summary>
    /// Maze dimensions in tiles
    /// </summary>
    public int mazeLengthInTiles;
    /// <summary>
    /// Size of single tile
    /// </summary>
    public float tileLength;
    /// <summary>
    /// True/False for wall/path
    /// </summary>
    private bool[,] tiles;
    /// <summary>
    /// True/False if maze is completable
    /// </summary>
    public bool validMaze;

    /// <summary> Returns location of the tile
    /// 
    /// </summary>
    /// @param i 1st index of tile location
    /// @param j 2nd index of tile location
    /// <returns>tiles[i,j]</returns>
    public bool getTile(int i, int j)
    {
        return tiles[i,j];
    }

    /// <summary> Main loop that handles maze creation
    /// 
    /// </summary> Keeps genereating mazes, based on given parameters, then creates a valid maze.
    /// <param name="_tileLength">how big will one tile will be</param>
    /// <param name="_mazeLengthInTiles">controls the dimensions of the maze in tiles</param>
    /// <param name="cutOff">controls the fill of obstacles</param>
    public Maze(float _tileLength,int _mazeLengthInTiles, float cutOff)
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
    
    /// <summary> Creates new maze using createMaze()
    /// 
    /// </summary> used for button controls in unity
    /// <param name="cutOff">Controls obstacle fill</param>
    /// <returns> New maze</returns>
    public bool recreateMaze(float cutOff)
    {
        return createMaze(cutOff);
    }

    /// <summary> Creates new maze
    /// 
    /// </summary> called whenever new maze must be created
    /// <param name="cutOff"> Controls obstacle fill</param>
    /// <returns> A bool showing if the maze is valid or not</returns>
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

    /// <summary> Class to handle "points" on the graph of the maze
    /// 
    /// </summary>
    private class Point
    {
        /// <summary>
        /// x position
        /// </summary>
        public int xPos;
        /// <summary>
        /// y position
        /// </summary>
        public int yPos;
        /// <summary>
        /// index for maze array
        /// </summary>
        public float g;
        /// <summary>
        /// index for maze array
        /// </summary>
        public float h;
        /// <summary>
        /// index for maze array
        /// </summary>
        public float f;
        /// <summary>
        /// True/False whether tile is wall or path
        /// </summary>
        public bool wall;
        public bool closed;
        public bool open;
    }

    /// <summary> Creates new point
    /// 
    /// </summary>
    /// <param name="_x"> x coordinate of point</param>
    /// <param name="_y"> x coordinate of point</param>
    /// <returns> the coordinates of the new point</returns>
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

    /// <summary> Checks if maze is valid
    /// 
    /// </summary> Validitaty checker used in createMaze(), uses a rudementary A* pathfinding to check if maze is completable
    /// <returns>true/false depending on if the maze is valid</returns>
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
