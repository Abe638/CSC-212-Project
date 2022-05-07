using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine.Rendering;

/// <summary>
/// Class handling the Genetic Algorithms/ACO 
/// </summary>
public class ACO : MonoBehaviour
{

    /// <summary>
    /// Object for the start of the pathfinding
    /// </summary>
    public Transform seeker;
    /// <summary>
    /// Object for the end of the pathfinding
    /// </summary>
    public Transform target;
    /// <summary>
    /// random number for mutaions
    /// </summary>
    public int numGen;
    /// <summary>
    /// counter for steps taken
    /// </summary>
    private int ACOstep;
    /// <summary>
    /// string holding location of results folder
    /// </summary>
    string location = @"D:\Unity\PathfindingUnityTest\Assets\scripts\pathfinding\results.txt";
    /// <summary>
    /// holds results to be output
    /// </summary>
    string results;
    /// <summary>
    /// grid object used for pathfinding operations
    /// </summary>
    Grid grid;

    /// <summary>
    /// Unity specific function that triggers on start of game
    /// </summary>
    void Awake() {
        grid = GetComponent<Grid> ();
    }

    /// <summary>
    /// unity specific function called on every frame
    /// </summary>
    void Update() {

    }

    /// <summary>
    /// main function for ACO
    ///
    /// Uses a modified A* pathfinding with a random mutation, this mutation is then evaluated for fitness. The fittest is then used for the seed of future generations.
    /// </summary>
    public void RunACO()
    {
        Stopwatch sw2 = new Stopwatch();
        System.Random num = new System.Random();
        for (int i = 0; i < numGen; i++)
        {
            sw2.Start();
            int rInt = num.Next(0, 10);
            FindPath(seeker.position, target.position, rInt);
            sw2.Stop();
        }
        print("ACO time: " + sw2.ElapsedMilliseconds + " ms");
        results = sw2.ElapsedMilliseconds + ", " + ACOstep + "\t";
        File.AppendAllText(location, results);
        print("ACO found the goal in " + ACOstep + " steps.");
    }

    /// <summary>
    /// Modified A* for ACO use
    /// @see Astar
    /// </summary>
    /// <param name="startPos">Starting node for pathfinding</param>
    /// <param name="targetPos">Node set as goal for pathfinding</param>
    /// <param name="mutation">Variable determining mutation for GA</param>
    void FindPath(Vector3 startPos, Vector3 targetPos, int mutation) {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i ++) {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode) {
                RetracePath(startNode,targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbors(node)) {
                if (!neighbour.ClassWalkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = mutation*GetDistance(neighbour, targetNode);
                    neighbour.parent = node;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    /// <summary>
    /// Retraces path by checking every node's "parent"
    /// </summary>
    /// <param name="startNode">Start of path</param>
    /// <param name="endNode">End of path</param>
    void RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.path = path;
        
        if (path != null)
        {
            ACOstep = 0;
            foreach (Node n in path)
            { 
                ACOstep++;
            }
        }

    }

    /// <summary>
    /// Evaluates Distance from nodeA to nodeB
    /// </summary>
    /// <param name="nodeA"></param>
    /// <param name="nodeB"></param>
    /// <returns>Distance between nodes</returns>
    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.ClassGridX - nodeB.ClassGridX);
        int dstY = Mathf.Abs(nodeA.ClassGridY - nodeB.ClassGridY);

        if (dstX > dstY)
            return 14*dstY + 10* (dstX-dstY);
        return 14*dstX + 10 * (dstY-dstX);
    }
}
