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

    private int movementPoints;
    void Start()
    {
        movementPoints = 0;
        isMyTurn = GameMaster.instance.currentPlayerTurn == player;
        transform.position = path.waypoints[0].transform.position;
    }

    void Update()
    {

    }

    public IEnumerator StartTurn()
    {
        Debug.Log("start");
        isReadyToEndTurn = false;
        
        movementPoints = dice.RollDice();
        Debug.Log($"roled {movementPoints}");

        yield return StartCoroutine(dice.WaitForDiceToStop(2, movementPoints));
        Debug.Log("rolled");

        yield return StartCoroutine(HandleMovement());
        Debug.Log("moved");

        isReadyToEndTurn = true;
        
        dice.ReturnToIdle();
    }
    public IEnumerator HandleMovement()
    {
        yield return StartCoroutine(MoveForward(movementPoints));

        switch (path.waypoints[currentWaypoint].type)
        {
            case WaypointType.Normal:
                break;
            case WaypointType.Connector:
                break;
        }
    }

    private IEnumerator MoveForward(int steps)
    {
        currentWaypoint += steps;
        if (currentWaypoint < path.waypoints.Length)
        {
            Vector3 targetPosition = path.GetWaypointPosition(currentWaypoint);
            yield return StartCoroutine(MoveToPosition(targetPosition, (float)steps));
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
