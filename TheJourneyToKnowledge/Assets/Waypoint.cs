using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public WaypointType type;
    public WaypointForPlayer player;
    //public Transform transform;

    //private void Start()
    //{
    //    transform = GetComponent<Transform>();
    //}
}


public enum WaypointType
{
    Normal,
    Connector,
}
public enum WaypointForPlayer
{
    One,
    Two,
}
