using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    public Waypoint[] waypoints;

    public int gameStage;

    public Vector3 GetWaypointPosition(int index)
    {
        if(index >= 0 && index < waypoints.Length)
        {
            return waypoints[index].transform.position;
        }
        return Vector3.zero;
    }
}
