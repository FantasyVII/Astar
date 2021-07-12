using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarNode : IComparable
{
    public AStarNode parent;

    public int Fcost;
    public int Gcost;
    public int Hcost;

    public Vector3 Position;
    public Vector3Int GridPos;

    public int CompareTo(object node)
    {
        if (((AStarNode)node).Fcost < Fcost)
            return 1;
        else if (((AStarNode)node).Fcost > Fcost)
            return -1;
        return 0;
    }
}