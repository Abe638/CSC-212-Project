using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class for grid object
/// </summary>
public class Grid : MonoBehaviour
{
	/// <summary>
	/// bool for switching unity display on/off
	/// </summary>
	public bool onlyDisplayPathGizmos;
	/// <summary>
	/// switch for Using Genitic Algo/A*
	/// </summary>
	public bool GA;
	/// <summary>
	/// display for walkable tiles
	/// </summary>
	public LayerMask unwalkableMask;
	/// <summary>
	/// variable controlling grid size
	/// </summary>
	public Vector2 gridWorldSize;
	/// <summary>
	/// dictates node size
	/// </summary>
	public float nodeRadius;
	/// <summary>
	/// Empty grid object
	/// </summary>
	Node[,] grid;
	/// <summary>
	/// node size (radius*2)
	/// </summary>
	float nodeDiameter;
	int gridSizeX, gridSizeY;

	/// <summary>
	/// resets grid
	/// </summary>
	public void RefreshGrid()
	{
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	/// <summary>
	/// Creates a new grid
	/// </summary>
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
	/// <summary>
	/// Creates list of neighboring nodes
	/// </summary>
	/// <param name="node">Node object to find neighbors of</param>
	/// <returns>Neighbors if node</returns>
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

	/// <summary>
	/// Finds grid coords based on world position
	/// </summary>
	/// <param name="worldPosition"></param>
	/// <returns>coordinates</returns>
	public Node NodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y];
	}

	/// <summary>
	/// List storing path
	/// </summary>
	public List<Node> path;
	/// <summary>
	/// Handles Unity display
	/// </summary>
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