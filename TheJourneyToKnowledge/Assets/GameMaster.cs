using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public enum Players
    {
        PlayerOne = 1,
        PlayerTwo = 2
    }
    [HideInInspector]
    public Players currentPlayerTurn;

    public Camera currentCamera;

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject playerOneStartTurn;
    public GameObject playerTwoStartTurn;

    private TextMeshProUGUI playerOneTurnText;
    private TextMeshProUGUI playerTwoTurnText;

    public float fadeTime;
    private float currentfadeTime;
    private float alphaValue;
    private float fadeAwayPerSecond;


    void Start()
    {
        currentPlayerTurn = Players.PlayerOne;
        playerOneTurnText = playerOneStartTurn.GetComponent<TextMeshProUGUI>();
        playerTwoTurnText = playerTwoStartTurn.GetComponent<TextMeshProUGUI>();

        FocusCameraOnCurrentPlayer(playerOne);
        ShowTurnStartText();
    }

    void Update()
    {
        
    }

    public bool RequestToEndTurn()
    {
        //isCurrentPlayerReadyToEndTurn
        currentPlayerTurn = currentPlayerTurn == Players.PlayerOne ? Players.PlayerTwo : Players.PlayerOne;
        FocusCameraOnCurrentPlayer(currentPlayerTurn == Players.PlayerOne ? playerOne : playerTwo);
        ShowTurnStartText();
        return true;
    }

    public void ShowTurnStartText()
    {
        currentfadeTime = fadeTime;
        switch (currentPlayerTurn)
        {
            case Players.PlayerOne:
                AnimateTurnStartText(playerOneStartTurn, playerOneTurnText);
                break;
            case Players.PlayerTwo:
                AnimateTurnStartText(playerTwoStartTurn, playerTwoTurnText);
                break;
        }
    }
    private void AnimateTurnStartText(GameObject currentPlayerStartTurn, TMP_Text currentPlayerStartTurnText)
    {
        fadeAwayPerSecond = 1 / fadeTime;
        alphaValue = 1f;
        currentPlayerStartTurn.SetActive(true);
        StartCoroutine(FadeOutText(currentPlayerStartTurn, currentPlayerStartTurnText));
    }

    private IEnumerator FadeOutText(GameObject currentPlayerStartTurn, TMP_Text currentPlayerStartTurnText)
    {
        while (currentfadeTime > 0)
        {
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;
            currentPlayerStartTurnText.color = new Color(currentPlayerStartTurnText.color.r, currentPlayerStartTurnText.color.g, currentPlayerStartTurnText.color.b, alphaValue);
            currentfadeTime -= Time.deltaTime;
            yield return null;
        }
        currentPlayerStartTurn.SetActive(false);
    }

    private void FocusCameraOnCurrentPlayer(GameObject currentPlayer)
    {
        currentCamera.transform.position = new Vector3(currentPlayer.transform.position.x + 5, 40f, -15f);
    }
}
