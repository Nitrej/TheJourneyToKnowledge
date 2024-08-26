using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AnimatedValues;
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
    private float currCountdownValue;

    public TextMeshProUGUI lifeSatisfactionPointsText;
    public TextMeshProUGUI knowledgePointsText;
    public TextMeshProUGUI luckyNumberText;

    //ChoisePanel
    public GameObject ChoosePanel;
    public GameObject ChoosePanelBorder;
    public UnityEngine.UI.Button LeftChooseButton;
    public UnityEngine.UI.Button RightChooseButton;
    public TextMeshProUGUI LeftChoise;
    public TextMeshProUGUI RightChoise;

    //NegativeAndChancePanel
    public GameObject ModifyPanel;
    public GameObject ModifyPanelBorder;
    public UnityEngine.UI.Button ModifyConfirm;
    public TextMeshProUGUI ModifyText;
    public TextMeshProUGUI PointsNCType;
    public TextMeshProUGUI PointsNCValue;

    //RiskPanel
    public GameObject RiskyPanel;
    public GameObject RiskPanelBorder;
    public UnityEngine.UI.Button LeftRiskButton;
    public UnityEngine.UI.Button RightRiskButton;
    public TextMeshProUGUI QuestionText;
    public TextMeshProUGUI LeftAnswer;
    public TextMeshProUGUI RightAnswer;
    public TextMeshProUGUI TimerText;

    public ParticleSystem luckyNumberParticles;

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

        if (movementPoints == luckyNumber)
        {
            luckyNumberParticles.Play();
            lifeSatisfactionPoints += 2;
        }

        yield return StartCoroutine(HandleMovement());
        //Debug.Log("moved");

        rolledDice = true;
        isReadyToEndTurn = true;
        
    }
    public IEnumerator HandleMovement()
    {
        yield return StartCoroutine(MoveForward(movementPoints));

        switch (path.waypoints[currentWaypoint].type)
        {
            case WaypointType.Normal:

                //isReadyToEndTurn = true;
                break;
            case WaypointType.Connector:

                if(path.waypoints[currentWaypoint].paths.Length == 1)
                {
                    path = path.waypoints[currentWaypoint].paths[0];
                    currentWaypoint = 0;
                    if (movementPoints > 0) 
                    {
                        yield return StartCoroutine(HandleMovement());

                    }
                    //isReadyToEndTurn = true;
                }
                else if(path.waypoints[currentWaypoint].paths.Length == 2)
                {
                    if (movementPoints > 0 && !isReadyToEndTurn)
                    {
                        isReadyToEndTurn = false;
                        yield return StartCoroutine(ChoosePath());
                        
                    }
                }
                break;
            case WaypointType.Negative:
                yield return StartCoroutine(NegativeTile());
                break;
            case WaypointType.Risk:
                yield return StartCoroutine(RiskTile());
                break;
            case WaypointType.Chance:
                yield return StartCoroutine(ChanceTile());
                break;
            case WaypointType.Matura:
                break;
            case WaypointType.Victory:
                break;
        }
    }
    private IEnumerator NegativeTile()
    {
        NegativeEvent negativeEvent = GameMaster.instance.GetNegativeEvent(path.gameStage);
        PointsNCType.text = negativeEvent.type == 0 ? "Wiedza: ": "¯ycie: ";
        ModifyText.text = negativeEvent.text;
        PointsNCValue.text = negativeEvent.value.ToString();
        ModifyPanelBorder.SetActive(true);
        ModifyPanel.SetActive(true);

        var waitForButton = new WaitForUIButtons(ModifyConfirm);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == ModifyConfirm)
        {
            if (negativeEvent.type == 0)
            {
                knowledgePoints += negativeEvent.value;
            }
            else
            {
                lifeSatisfactionPoints += negativeEvent.value;
            }
            //isReadyToEndTurn = true;
        }

    }
    private IEnumerator StartCountdown(float countdownValue = 5)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            TimerText.text = currCountdownValue.ToString();
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
            TimerText.text = currCountdownValue.ToString();
        }
    }
    private IEnumerator RiskTile()
    {
        RiskEvent riskEvent = GameMaster.instance.GetRiskEvent(path.gameStage);
        QuestionText.text = riskEvent.text;
        LeftAnswer.text = riskEvent.ansL;
        RightAnswer.text = riskEvent.ansR;
        RiskPanelBorder.SetActive(true);
        RiskyPanel.SetActive(true);

        StartCoroutine(StartCountdown(5f));

        var waitForButton = new WaitForUIButtons(LeftRiskButton, RightRiskButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == LeftRiskButton)
        {
            if (currCountdownValue <= 0)
            {
                PointsNCType.text = "Wiedza: ";
                ModifyText.text = "Niestety! Czas min¹³.";
                PointsNCValue.text = $"-{riskEvent.value}";
                ModifyPanelBorder.SetActive(true);
                ModifyPanel.SetActive(true);
                knowledgePoints -= riskEvent.value;
            }
            else
            {
                if (riskEvent.correct == 0)
                {
                    PointsNCType.text = "Wiedza: ";
                    ModifyText.text = "Gratulacje! To poprawna odpowiedŸ.";
                    PointsNCValue.text = riskEvent.value.ToString();
                    ModifyPanelBorder.SetActive(true);
                    ModifyPanel.SetActive(true);
                    knowledgePoints += riskEvent.value;
                }
                else
                {
                    PointsNCType.text = "Wiedza: ";
                    ModifyText.text = "Niestety! To niepoprawna odpowiedŸ.";
                    PointsNCValue.text = $"-{riskEvent.value}";
                    ModifyPanelBorder.SetActive(true);
                    ModifyPanel.SetActive(true);
                    knowledgePoints -= riskEvent.value;
                }
            }
        }
        else
        {
            if (currCountdownValue <= 0)
            {
                PointsNCType.text = "Wiedza: ";
                ModifyText.text = "Niestety! Czas min¹³.";
                PointsNCValue.text = $"-{riskEvent.value}";
                ModifyPanelBorder.SetActive(true);
                ModifyPanel.SetActive(true);
                knowledgePoints -= riskEvent.value;
            }
            else
            {
                if (riskEvent.correct == 1)
                {
                    PointsNCType.text = "Wiedza: ";
                    ModifyText.text = "Gratulacje! To poprawna odpowiedŸ.";
                    PointsNCValue.text = riskEvent.value.ToString();
                    ModifyPanelBorder.SetActive(true);
                    ModifyPanel.SetActive(true);
                    knowledgePoints += riskEvent.value;
                }
                else
                {
                    PointsNCType.text = "Wiedza: ";
                    ModifyText.text = "Niestety! To niepoprawna odpowiedŸ.";
                    PointsNCValue.text = $"-{riskEvent.value}";
                    ModifyPanelBorder.SetActive(true);
                    ModifyPanel.SetActive(true);
                    knowledgePoints -= riskEvent.value;
                }
            }
            
        }
        RiskPanelBorder.SetActive(false);
        RiskyPanel.SetActive(false);
    }
    private IEnumerator ChanceTile()
    {
        NegativeEvent negativeEvent = GameMaster.instance.GetChanceEvent(path.gameStage);
        PointsNCType.text = negativeEvent.type == 0 ? "Wiedza: " : "¯ycie: ";
        ModifyText.text = negativeEvent.text;
        PointsNCValue.text = negativeEvent.value.ToString();
        ModifyPanelBorder.SetActive(true);
        ModifyPanel.SetActive(true);

        var waitForButton = new WaitForUIButtons(ModifyConfirm);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == ModifyConfirm)
        {
            if (negativeEvent.type == 0)
            {
                knowledgePoints += negativeEvent.value;
            }
            else
            {
                lifeSatisfactionPoints += negativeEvent.value;
            }
            //isReadyToEndTurn = true;
        }

    }
    private IEnumerator ChoosePath()
    {
        LeftChoise.text = path.waypoints[currentWaypoint].leftChoise;
        RightChoise.text = path.waypoints[currentWaypoint].rightChoise;
        ChoosePanelBorder.SetActive(true);
        ChoosePanel.SetActive(true);

        var waitForButton = new WaitForUIButtons(LeftChooseButton, RightChooseButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == LeftChooseButton)
        {
            path = path.waypoints[currentWaypoint].paths[0];
            currentWaypoint = 0;
            yield return StartCoroutine(HandleMovement());
            //isReadyToEndTurn = true;
        }
        else
        {
            path = path.waypoints[currentWaypoint].paths[1];
            currentWaypoint = 0;
            yield return StartCoroutine(HandleMovement());
            //isReadyToEndTurn = true;
        }

    }

    private IEnumerator MoveForward(int steps)
    {
        //Debug.LogWarning($"steps {steps}; {currentWaypoint}");

        if (steps >= path.waypoints.Length - 1 - currentWaypoint && steps > 0)
        {
            steps = path.waypoints.Length - 1 - currentWaypoint;
            movementPoints -= steps;
            //Debug.LogWarning($"odjêto {steps}");
            if(movementPoints == 0) isReadyToEndTurn = true;

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
