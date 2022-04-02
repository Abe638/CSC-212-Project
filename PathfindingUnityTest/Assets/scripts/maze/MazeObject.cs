using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MazeObject : MonoBehaviour
{
    private MazeObjectEditor mazeObjectEditor;

    public Maze maze;
    public float tileLength;
    public int mazeLengthInTiles;
    public float cutOff;

    void Start()
    {
        maze = new Maze(tileLength, mazeLengthInTiles, cutOff);

    }

    void Update()
    {

    }

    void OnDrawGizmos()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Gizmos.color = Color.red;
        if (meshFilter.sharedMesh != null)
        {
            foreach (Vector3 vertex in meshFilter.sharedMesh.vertices)
            {
                Vector3 worldPos = transform.TransformPoint(vertex);
                Gizmos.DrawCube(worldPos, Vector3.one * 0.1f);
            }
        }
    }
    
    public void BuildMaze()
        {
            Mesh mesh = new Mesh();
            //First Create Ring;
            Vector3[] vertices = new Vector3[(mazeLengthInTiles + 3)
                                             * (mazeLengthInTiles + 3) * 2];
            int vecLayer2 = (mazeLengthInTiles + 3)
                            * (mazeLengthInTiles + 3);
            int D = vecLayer2;
            Color[] colors = new Color[vertices.Length];
            Vector3[] normals = new Vector3[vertices.Length];

            for (int i = 0; i < mazeLengthInTiles + 3; ++i)
            {
                for (int j = 0; j < mazeLengthInTiles + 3; ++j)
                {
                    vertices[i + (j * (mazeLengthInTiles + 3))]
                        = new Vector3(((float) i * tileLength),
                            ((-1) * (float) j * tileLength), 0);
                    colors[i + (j * (mazeLengthInTiles + 3))]
                        = Color.red;
                    normals[i + (j * (mazeLengthInTiles + 3))]
                        = new Vector3(0, 0, -1);
                }
            }

            for (int i = 0; i < mazeLengthInTiles + 3; ++i)
            {
                for (int j = 0; j < mazeLengthInTiles + 3; ++j)
                {
                    vertices[vecLayer2 + i + (j * (mazeLengthInTiles + 3))]
                        = new Vector3(((float) i * tileLength),
                            ((-1) * (float) j * tileLength), -1);
                    colors[vecLayer2 + i + (j * (mazeLengthInTiles + 3))]
                        = Color.green;
                    normals[vecLayer2 + i + (j * (mazeLengthInTiles + 3))]
                        = new Vector3(0, 0, -1);
                }
            }

            int[] tri = new int[4 * (mazeLengthInTiles *
                                     mazeLengthInTiles * 6)];
            int triIndex = 0;

            int L = mazeLengthInTiles + 3;
            for (int i = 0; i < mazeLengthInTiles + 2; ++i)
            {
                for (int j = 0; j < mazeLengthInTiles + 2; ++j)
                {
                    if (maze.getTile(i, j) == true) // open tile
                    {
                        tri[triIndex++] = i + (j * L);
                        tri[triIndex++] = 1 + i + (j * L);
                        tri[triIndex++] = i + ((j + 1) * L);

                        tri[triIndex++] = i + ((j + 1) * L);
                        tri[triIndex++] = 1 + i + (j * L);
                        tri[triIndex++] = 1 + i + ((j + 1) * L);
                        if (maze.getTile(i + 1, j) == false)
                        {
                            tri[triIndex++] = D + 1 + i + (j * L);
                            tri[triIndex++] = D + 1 + i + ((j + 1) * L);
                            tri[triIndex++] = 1 + i + (j * L);

                            tri[triIndex++] = 1 + i + (j * L);
                            tri[triIndex++] = D + 1 + i + ((j + 1) * L);
                            tri[triIndex++] = 1 + i + ((j + 1) * L);
                        }

                        if (maze.getTile(i, j - 1) == false)
                        {
                            tri[triIndex++] = D + i + (j * L);
                            tri[triIndex++] = D + 1 + i + (j * L);
                            tri[triIndex++] = i + (j * L);

                            tri[triIndex++] = i + (j * L);
                            tri[triIndex++] = D + 1 + i + (j * L);
                            tri[triIndex++] = 1 + i + (j * L);
                        }

                        if (maze.getTile(i - 1, j) == false)
                        {
                            tri[triIndex++] = D + i + ((j + 1) * L);
                            tri[triIndex++] = D + i + (j * L);
                            tri[triIndex++] = i + ((j + 1) * L);

                            tri[triIndex++] = i + ((j + 1) * L);
                            tri[triIndex++] = D + i + (j * L);
                            tri[triIndex++] = i + (j * L);
                        }

                        if (maze.getTile(i, j + 1) == false)
                        {
                            tri[triIndex++] = D + 1 + i + ((j + 1) * L);
                            tri[triIndex++] = D + i + ((j + 1) * L);
                            tri[triIndex++] = 1 + i + ((j + 1) * L);

                            tri[triIndex++] = 1 + i + ((j + 1) * L);
                            tri[triIndex++] = D + i + ((j + 1) * L);
                            tri[triIndex++] = i + ((j + 1) * L);
                        }
                    }
                    else // walled tile
                    {
                        tri[triIndex++] = D + i + (j * L);
                        tri[triIndex++] = D + 1 + i + (j * L);
                        tri[triIndex++] = D + i + ((j + 1) * L);

                        tri[triIndex++] = D + i + ((j + 1) * L);
                        tri[triIndex++] = D + 1 + i + (j * L);
                        tri[triIndex++] = D + 1 + i + ((j + 1) * L);
                    }
                }
            }

            int[] trueTri = new int[triIndex];
            for (int i = 0; i < triIndex; ++i)
            {
                trueTri[i] = tri[i];
            }

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            mesh.vertices = vertices;
            mesh.triangles = trueTri;
            //mesh.colors = colors;
            mesh.normals = normals;
            meshFilter.mesh = mesh;

            MeshCollider meshCollider = GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;

        }
    
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MazeObject))]
    public class MazeObjectEditor : Editor
    {
        MazeObject mazeObject;
        string validity;
        string mazeImage;

        private void OnEnable()
        {
            mazeObject = (MazeObject) target;

        }

        public override void OnInspectorGUI()
        {

            base.OnInspectorGUI();
            if (GUILayout.Button("Recreate Maze"))
            {
                mazeObject.maze = new Maze(mazeObject.tileLength, mazeObject.mazeLengthInTiles, mazeObject.cutOff);
            }

            if (GUILayout.Button("Build Maze Mesh"))
            {
                if (mazeObject.maze == null)
                {
                    Debug.Log("Cannot build mesh. No maze available!");
                }
                else mazeObject.BuildMaze();

            }

            if (GUILayout.Button("View Maze"))
            {
                validity = "";
                mazeImage = "No maze generated.";
                if (mazeObject.maze != null)
                {
                    if (mazeObject.maze.validMaze == true)
                    {
                        validity = "Valid Maze";
                    }
                    else
                    {
                        validity = "Invalid Maze";
                    }

                    mazeImage = "";
                    for (int i = 0; i < mazeObject.mazeLengthInTiles + 2; ++i)
                    {
                        for (int j = 0; j < mazeObject.mazeLengthInTiles + 2; ++j)
                        {
                            if (mazeObject.maze.getTile(i, j) == true) mazeImage += "0";
                            else mazeImage += "$";
                        }

                        mazeImage += "\n";
                    }
                }
            }

            GUILayout.Label(validity);
            GUILayout.Label(mazeImage);
        }
    }
#endif