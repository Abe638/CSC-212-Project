using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine.Rendering;

public class ACO : MonoBehaviour
{

    public Transform seeker, target;
    public int numGen;
    private int ACOstep;
    string location = @"D:\Unity\PathfindingUnityTest\Assets\scripts\pathfinding\results.txt";
    string results;
    Grid grid;

    void Awake() {
        grid = GetComponent<Grid> ();
    }

    void Update() {

    }

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

    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.ClassGridX - nodeB.ClassGridX);
        int dstY = Mathf.Abs(nodeA.ClassGridY - nodeB.ClassGridY);

        if (dstX > dstY)
            return 14*dstY + 10* (dstX-dstY);
        return 14*dstX + 10 * (dstY-dstX);
    }
}
