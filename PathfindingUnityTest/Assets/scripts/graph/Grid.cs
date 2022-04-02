using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;


public class Grid : MonoBehaviour
{

	public bool onlyDisplayPathGizmos;
	public bool GA;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	System.Random num = new System.Random();
	Node[,] grid;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() {
		
	}

	public void RefreshGrid()
	{
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		grid = new Node[gridSizeX,gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) {
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}

	public List<Node> GetNeighbors(Node node) {
		List<Node> neighbors = new List<Node>();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;

				int checkX = node.ClassGridX + x;
				int checkY = node.ClassGridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
					neighbors.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbors;
	}
	
	/*public Node RandNeighbor(Node node)
	{
		//System.Random num = new System.Random();
		int rInt = num.Next(0, 7);
		int checkX = node.ClassGridX;
		int checkY = node.ClassGridY;
		Node newNode = grid[checkX, checkY];
		
		//if (checkX == 0)

		switch (rInt)
		{
			case 0:
				checkX = node.ClassGridX - 1;
				checkY = node.ClassGridY - 1;
				newNode = grid[checkX, checkY];
				break;
			case 1:
				checkX = node.ClassGridX;
				checkY = node.ClassGridY - 1;
				newNode = grid[checkX, checkY];
				break;
			case 2:
				checkX = node.ClassGridX + 1;
				checkY = node.ClassGridY - 1;
				newNode = grid[checkX, checkY];
				break;
			case 3:
				checkX = node.ClassGridX - 1;
				checkY = node.ClassGridY;
				newNode = grid[checkX, checkY];
				break;
			case 4:
				checkX = node.ClassGridX + 1;
				checkY = node.ClassGridY;
				newNode = grid[checkX, checkY];
				break;
			case 5:
				checkX = node.ClassGridX - 1;
				checkY = node.ClassGridY + 1;
				newNode = grid[checkX, checkY];
				break;
			case 6:
				checkX = node.ClassGridX;
				checkY = node.ClassGridY + 1;
				newNode = grid[checkX, checkY];
				break;
			case 7:
				checkX = node.ClassGridX + 1;
				checkY = node.ClassGridY + 1;
				newNode = grid[checkX, checkY];
				break;
		}

		return newNode;
	}*/
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	public List<Node> path;
	void OnDrawGizmos() {

		if (onlyDisplayPathGizmos)
		{
			if (path != null)
			{
				foreach (Node n in path)
				{
					Gizmos.color = Color.red;
					if (GA)
						Gizmos.color = Color.blue;
					Gizmos.DrawCube(n.ClassWorldPosition, Vector3.one * (nodeDiameter));
				}
			}
		}
		else
		{
			Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
			if (grid != null) {
				foreach (Node n in grid) {
					Gizmos.color = (n.ClassWalkable)?Color.white:Color.red;
					if (path != null)
						if (path.Contains(n))
							Gizmos.color = Color.black;
					Gizmos.DrawCube(n.ClassWorldPosition, Vector3.one * (nodeDiameter-.1f));
				}
			}
		}
	}
}