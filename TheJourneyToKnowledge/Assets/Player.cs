using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public bool rolledDice;

    public int lifeSatisfactionPoints;
    public int knowledgePoints;
    public int luckyNumber;

    public TextMeshProUGUI lifeSatisfactionPointsText;
    public TextMeshProUGUI knowledgePointsText;
    public TextMeshProUGUI luckyNumberText;
    void Start()
    {
        lifeSatisfactionPoints = 0;
        lifeSatisfactionPointsText.text = "0";

        knowledgePoints = 0;
        knowledgePointsText.text = "0";

        luckyNumber = dice.RollDice();
        luckyNumberText.text = $"{luckyNumber}";

        movementPoints = 0;
        isMyTurn = GameMaster.instance.currentPlayerTurn == player;
        transform.position = path.waypoints[0].transform.position;
    }

    void Update()
    {
        lifeSatisfactionPointsText.text = $"{lifeSatisfactionPoints}";
        knowledgePointsText.text = $"{knowledgePoints}";
    }

    public IEnumerator StartTurn()
    {
        Debug.Log("start");

        movementPoints = dice.RollDice();
        Debug.Log($"roled {movementPoints}");

        yield return StartCoroutine(dice.WaitForDiceToStop(2, movementPoints));
        Debug.Log("rolled");

        yield return StartCoroutine(HandleMovement());
        Debug.Log("moved");

        rolledDice = true;
        isReadyToEndTurn = true;
        
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
