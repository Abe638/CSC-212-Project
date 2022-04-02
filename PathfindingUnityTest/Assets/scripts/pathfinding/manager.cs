using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

// Start is called before the first frame update
    void Awake()
    {
        _aco = ACO.GetComponent<ACO>();
        _astar = Astar.GetComponent<Astar>();
        _mazeObject = Maze.GetComponent<MazeObject>();
        _acoGrid = ACO.GetComponent<Grid>();
        _astarGrid = Astar.GetComponent<Grid>();
    }

    // Update is called once per frame
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
