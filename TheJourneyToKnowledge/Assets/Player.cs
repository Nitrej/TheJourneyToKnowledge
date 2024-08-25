using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOne : MonoBehaviour
{
    public GameMaster.Players player;

    public bool isMyTurn;
    public bool isReadyToEndTurn;
    public Path path;
    public Dice dice;
    private int currentWaypoint;
    void Start()
    {
        isMyTurn = GameMaster.instance.currentPlayerTurn == player;
        transform.position = path.waypoints[0].transform.position;
    }

    void Update()
    {
        //if (isMyTurn)
        //{
        //    StartTurn();
        //}
    }

    public void StartTurn()
    {
        isReadyToEndTurn = false;
        
        int movement = dice.RollDice();
        MoveForward(movement);
        switch (path.waypoints[currentWaypoint].type)
        {
            case WaypointType.Normal:
                break;
            case WaypointType.Connector:
                break;
        }
        
        isReadyToEndTurn = true;

    }

    private void MoveForward(int steps)
    {
        currentWaypoint += steps;
        if (currentWaypoint < path.waypoints.Length)
        {
            Vector3 targetPosition = path.GetWaypointPosition(currentWaypoint);
            StartCoroutine(MoveToPosition(targetPosition, (float)steps));
        }
        else 
        { 
            currentWaypoint = path.waypoints.Length -1 ;
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed/duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}
