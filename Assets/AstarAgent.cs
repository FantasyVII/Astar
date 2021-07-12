using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarAgent : MonoBehaviour
{
    List<AStarNode> neighbourNodesList;
    List<AStarNode> openNodesList;
    List<AStarNode> closeNodesList;
    List<AStarNode> pathNodesList;

    AStarNode currentNode;

    GridClass gridclass;

    Vector3 startingPositionInWorld;
    Vector3 goalPositionInWorld;

    void Awake()
    {
        neighbourNodesList = new List<AStarNode>();
        openNodesList = new List<AStarNode>();
        closeNodesList = new List<AStarNode>();
        pathNodesList = new List<AStarNode>();

        gridclass = FindObjectOfType<GridClass>();

        //transform.position = startingPositionInWorld;
    }

    void CheckNeihbours(AStarNode currentNode)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                {
                    continue;
                }
                if (currentNode.GridPos.x + x <= gridclass.cellX - 1 && currentNode.GridPos.x + x >= 0
                    && currentNode.GridPos.z + z <= gridclass.cellZ - 1 && currentNode.GridPos.z + z >= 0)
                {
                    Vector3Int nodePos = new Vector3Int(currentNode.GridPos.x + x, 0, currentNode.GridPos.z + z);
                    neighbourNodesList.Add(gridclass.GetNode(nodePos));
                }
            }
        }
    }

    int CalculateDistance(AStarNode start, AStarNode destination)
    {
        int x = Mathf.Abs(start.GridPos.x - destination.GridPos.x);
        int z = Mathf.Abs(start.GridPos.z - destination.GridPos.z);
        int h = Mathf.Max(x, z);

        if (x > z)
        {
            return 14 * z + 10 * (x + z);
        }
        return 14 * x + 10 * (z - x);
    }

    public List<AStarNode> FindPath(Vector3Int startNodePosition, Vector3Int destinationNodePosition)
    {
        AStarNode start = gridclass.GetNode(startNodePosition);
        AStarNode destination = gridclass.GetNode(destinationNodePosition);

        startingPositionInWorld = start.Position;
        goalPositionInWorld = destination.Position;

        openNodesList.Add(start);

        while (true)
        {
            openNodesList.Sort();
            currentNode = openNodesList[0];
            openNodesList.Remove(currentNode);
            closeNodesList.Add(currentNode);

            if (currentNode.GridPos == destination.GridPos)
            {
                pathNodesList.Clear();
                WalkOnPath(destination);
                Debug.Log("path found");
                pathNodesList.Reverse();
                return pathNodesList;
            }

            CheckNeihbours(currentNode);

            for (int i = 0; i < neighbourNodesList.Count; i++)
            {
                if (closeNodesList.Contains(neighbourNodesList[i]))
                {
                    continue;
                }

                int newMovCostToNeighbour = currentNode.Gcost + CalculateDistance(currentNode, neighbourNodesList[i]);

                if (newMovCostToNeighbour < neighbourNodesList[i].Gcost || !openNodesList.Contains(neighbourNodesList[i]))
                {
                    neighbourNodesList[i].Gcost = newMovCostToNeighbour;
                    neighbourNodesList[i].Hcost = CalculateDistance(neighbourNodesList[i], destination);
                    neighbourNodesList[i].Fcost = neighbourNodesList[i].Gcost + neighbourNodesList[i].Hcost;
                    neighbourNodesList[i].parent = currentNode;

                    if (!openNodesList.Contains(neighbourNodesList[i]))
                    {
                        openNodesList.Add(neighbourNodesList[i]);
                    }
                }
            }

            neighbourNodesList.Clear();

            if (openNodesList.Count <= 0)
            {
                Debug.Log("PathNotFound");
                return new List<AStarNode>();
            }
        }
    }

    public void WalkOnPath(AStarNode node)
    {
        pathNodesList.Add(node);
        AStarNode n = node.parent;
        if (n != null)
        {
            WalkOnPath(n);
        }
        else
        {
            Debug.Log("Arrived");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(startingPositionInWorld, new Vector3(gridclass.width, 0, gridclass.height));

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(goalPositionInWorld, new Vector3(gridclass.width, 0, gridclass.height));

        for (int i = 0; i < pathNodesList.Count; i++)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawSphere(pathNodesList[i].Position, 0.5f);
        }
    }
}