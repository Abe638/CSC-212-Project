using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine.Rendering;

public class Astar : MonoBehaviour {

    public Transform seeker, target;
    private int step;
    string location = @"D:\Unity\PathfindingUnityTest\Assets\scripts\pathfinding\results.txt";
    string results;
    Grid grid;

    void Awake() {
        grid = GetComponent<Grid> ();
    }

    void Update()
    {

    }

    public void RunAstar()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        FindPath(seeker.position, target.position);
        sw.Stop();
        print("A* time: " + sw.ElapsedMilliseconds + " ms");
        results = sw.ElapsedMilliseconds + ", " + step + "\n";
        File.AppendAllText(location, results);
        print("A* found the goal in " + step + " steps.");
    }
    
    void FindPath(Vector3 startPos, Vector3 targetPos) {
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
                    neighbour.hCost = GetDistance(neighbour, targetNode);
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
            step = 0;
            foreach (Node n in path)
            { 
                step++;
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