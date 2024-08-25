using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public WaypointType type;

    public Path[] paths;

    public string leftChoise;
    public string rightChoise;
}


public enum WaypointType
{
    Normal,
    Connector,
}

