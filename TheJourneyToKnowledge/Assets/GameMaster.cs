using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public enum Players
    {
        PlayerOne = 1,
        PlayerTwo = 2
    }
    [HideInInspector]
    public Players currentPlayerTurn;

    public Camera currentCamera;

    public GameObject playerOneObject;
    public GameObject playerTwoObject;

    private PlayerOne playerOne;
    private PlayerOne playerTwo;

    public GameObject playerOneStartTurn;
    public GameObject playerTwoStartTurn;

    private TextMeshProUGUI playerOneTurnText;
    private TextMeshProUGUI playerTwoTurnText;

    public float fadeTime;
    private float currentfadeTime;
    private float alphaValue;
    private float fadeAwayPerSecond;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        currentPlayerTurn = Players.PlayerOne;
        playerOneTurnText = playerOneStartTurn.GetComponent<TextMeshProUGUI>();
        playerTwoTurnText = playerTwoStartTurn.GetComponent<TextMeshProUGUI>();

        playerOne = playerOneObject.GetComponent<PlayerOne>();
        playerTwo = playerTwoObject.GetComponent<PlayerOne>();

        FocusCameraOnCurrentPlayer(playerOneObject);
        ShowTurnStartText();
        StartCoroutine(playerOne.StartTurn());
    }

    void Update()
    {
        if(currentPlayerTurn == Players.PlayerOne)
        {
            FocusCameraOnCurrentPlayer(playerOneObject);
        }
        else
        {
            FocusCameraOnCurrentPlayer(playerTwoObject);
        }
        
    }

    public bool RequestToEndTurn()
    {
        if(currentPlayerTurn == Players.PlayerOne && !playerOne.isReadyToEndTurn)
        {
            return false;
        }
        if(currentPlayerTurn == Players.PlayerTwo && !playerTwo.isReadyToEndTurn)
        {
            return false;
        }

        if (currentPlayerTurn == Players.PlayerOne)
        {
            currentPlayerTurn = Players.PlayerTwo;
            playerOne.isMyTurn = false;
            playerTwo.isMyTurn = true;
            Debug.Log("ready two");
            StartCoroutine(playerTwo.StartTurn());
        }
        else
        {
            currentPlayerTurn = Players.PlayerOne;
            playerOne.isMyTurn = true;
            playerTwo.isMyTurn = false;
            Debug.Log("ready one");
            StartCoroutine(playerOne.StartTurn());
        }

        FocusCameraOnCurrentPlayer(currentPlayerTurn == Players.PlayerOne ? playerOneObject : playerTwoObject);
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

        yield return new WaitForSeconds(fadeTime);

    }

    private void FocusCameraOnCurrentPlayer(GameObject currentPlayer)
    {
        currentCamera.transform.position = new Vector3(currentPlayer.transform.position.x + 5, currentPlayer.transform.position.y + 20f, currentPlayer.transform.position.z - 15f);
    }
}
