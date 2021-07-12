using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridClass : MonoBehaviour
{
    [SerializeField] Vector3 gridPosition;

    public float width;
    public float height;
    public int cellX;
    public int cellZ;

    public List<AStarNode> nodes = new List<AStarNode>();
    private void Awake()
    {
        for (int z = 0; z < cellZ; z++)
        {
            for (int x = 0; x < cellX; x++)
            {
                int i = x + z * cellX;
                nodes.Add(new AStarNode());
                nodes[i].Position = new Vector3(x * width + width / 2 + gridPosition.x, 0, z * height + height / 2 + gridPosition.z);
                nodes[i].GridPos = new Vector3Int(x, 0, z);
            }
        }
    }

    public Vector3Int GetNodePosition(Vector3 worldPosition)
    {
        int x = (int)(worldPosition.x / width);
        int z = (int)(worldPosition.z / height);
        return new Vector3Int(x, 0, z);
    }

    public AStarNode GetNode(Vector3Int NodePos)
    {
        int i = NodePos.x + NodePos.z * cellX;
        return nodes[i];
    }

    void OnDrawGizmos()
    {
        for (int z = 0; z < cellZ + 1; z++)
        {
            //horz
            //Gizmos.color = Color.black;
            Gizmos.DrawLine(new Vector3(0, 0, z * height) + gridPosition, new Vector3(cellX * width, 0, z * height) + gridPosition);
        }

        for (int x = 0; x < cellX + 1; x++)
        {
            //vert
            //Gizmos.color = Color.black;
            Gizmos.DrawLine(new Vector3(x * width, 0, 0) + gridPosition, new Vector3(x * width, 0, cellZ * height) + gridPosition);
        }
    }
}