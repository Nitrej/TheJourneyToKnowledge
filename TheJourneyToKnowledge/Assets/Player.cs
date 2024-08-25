using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerOne : MonoBehaviour
{
    public GameMaster.Players player;

    public bool isMyTurn;
    public bool isReadyToEndTurn;
    public Path path;
    public Dice dice;
    private int currentWaypoint;

    public int movementPoints;

    public bool rolledDice;

    public int lifeSatisfactionPoints;
    public int knowledgePoints;
    public int luckyNumber;

    public TextMeshProUGUI lifeSatisfactionPointsText;
    public TextMeshProUGUI knowledgePointsText;
    public TextMeshProUGUI luckyNumberText;

    public GameObject ChoosePanel;
    public GameObject ChoosePanelBorder;
    public UnityEngine.UI.Button LeftButton;
    public UnityEngine.UI.Button RightButton;
    public TextMeshProUGUI LeftChoise;
    public TextMeshProUGUI RightChoise;

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
        //Debug.Log("start");

        movementPoints = dice.RollDice();
        //Debug.Log($"roled {movementPoints}");

        yield return StartCoroutine(dice.WaitForDiceToStop(2, movementPoints));
        //Debug.Log("rolled");

        yield return StartCoroutine(HandleMovement());
        //Debug.Log("moved");

        rolledDice = true;
        
        
    }
    public IEnumerator HandleMovement()
    {
        yield return StartCoroutine(MoveForward(movementPoints));

        switch (path.waypoints[currentWaypoint].type)
        {
            case WaypointType.Normal:

                isReadyToEndTurn = true;
                break;
            case WaypointType.Connector:

                if(path.waypoints[currentWaypoint].paths.Length == 1)
                {
                    path = path.waypoints[currentWaypoint].paths[0];
                    currentWaypoint = 0;
                    yield return StartCoroutine(HandleMovement());
                    isReadyToEndTurn = true;
                }
                else if(path.waypoints[currentWaypoint].paths.Length == 2)
                {
                    yield return StartCoroutine(ChoosePath());
                }

                break;
        }
    }
    private IEnumerator ChoosePath()
    {
        LeftChoise.text = path.waypoints[currentWaypoint].leftChoise;
        RightChoise.text = path.waypoints[currentWaypoint].rightChoise;
        ChoosePanelBorder.SetActive(true);
        ChoosePanel.SetActive(true);

        var waitForButton = new WaitForUIButtons(LeftButton, RightButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == LeftButton)
        {
            path = path.waypoints[currentWaypoint].paths[0];
            currentWaypoint = 0;
            yield return StartCoroutine(HandleMovement());
            isReadyToEndTurn = true;
        }
        else
        {
            path = path.waypoints[currentWaypoint].paths[1];
            currentWaypoint = 0;
            yield return StartCoroutine(HandleMovement());
            isReadyToEndTurn = true;
        }

    }

    private IEnumerator MoveForward(int steps)
    {
        //Debug.LogWarning($"steps {steps}; {currentWaypoint}");
        if (steps <= 0) 
        { 
            yield return null;
        }
        if (steps >= path.waypoints.Length - 1 - currentWaypoint)
        {
            steps = path.waypoints.Length - 1 - currentWaypoint;
            movementPoints -= steps;
            //Debug.LogWarning($"odjêto {steps}");

        }
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
