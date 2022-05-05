using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager that executes Maze generation, Pathfinding, and results.
///
/// Many objects that are declared here will not be explained as they are just used to call functions in other classes
/// </summary>
public class manager : MonoBehaviour
{
    private Astar _astar;
    private ACO _aco;
    private MazeObject _mazeObject;
    private Grid _astarGrid;
    private Grid _acoGrid;
    public int numTests;
    [SerializeField] GameObject Astar;
    [SerializeField] GameObject ACO;
    [SerializeField] GameObject Maze;

    /// <summary>
    /// Built in Unity function that executes on game start
    ///
    /// initializes pathfinding, maze generating objects
    /// </summary>
    void Awake()
    {
        _aco = ACO.GetComponent<ACO>();
        _astar = Astar.GetComponent<Astar>();
        _mazeObject = Maze.GetComponent<MazeObject>();
        _acoGrid = ACO.GetComponent<Grid>();
        _astarGrid = Astar.GetComponent<Grid>();
    }
    /// <summary>
    /// Built in Unity function called once per frame
    ///
    /// Generates maze and finds path when the space bar is pressed
    /// </summary>
    void Update()
    {
        for (int i = 0; i < numTests; i++)
        {
            if (Input.GetButtonDown("Find Path"))
            {
                //Creates new maze
                _mazeObject.maze = new Maze(_mazeObject.tileLength, _mazeObject.mazeLengthInTiles, _mazeObject.cutOff);
                _mazeObject.BuildMaze();
                //Refreshes Grid used for pathfinding
                _astarGrid.RefreshGrid();
                _acoGrid.RefreshGrid();
                //finds paths and outpus to results.txt
                _aco.RunACO();
                _astar.RunAstar();
            }
        }
    }
}
