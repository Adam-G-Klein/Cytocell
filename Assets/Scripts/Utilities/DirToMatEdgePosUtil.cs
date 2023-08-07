using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP. Takes a directiion and returns the position on the edge of
// a material that is on the ray drawn by that direction
public class DirToMatEdgePosUtil : MonoBehaviour {

    public static Vector2 DirToMatEdgePos(Vector2 dir)
    {
        float phi = Vector2.Angle(Vector2.right, dir);
        int edgeNum = Mathf.FloorToInt((phi + 45) / 90);
        //TODO
        return Vector2.zero;
    }

    
}
