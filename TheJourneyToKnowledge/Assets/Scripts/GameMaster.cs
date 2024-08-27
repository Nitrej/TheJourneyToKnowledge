using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    
    public Players currentPlayerTurn;

    public Camera currentCamera;

    public Animator PlayerOneDiceAnimator;
    public Animator PlayerTwoDiceAnimator;

    public GameObject playerOneObject;
    public GameObject playerTwoObject;

    private PlayerOne playerOne;
    private PlayerOne playerTwo;

    private Players? firstPlayerToEnd;

    public GameObject playerOneStartTurn;
    public GameObject playerTwoStartTurn;

     //index = stage
    public TextAsset[] negativeJsons;
    public TextAsset[] positiveJsons;
    public TextAsset[] riskJsons;

    private TextMeshProUGUI playerOneTurnText;
    private TextMeshProUGUI playerTwoTurnText;

    public LevelLoader levelLoader;

    public GameObject ChoosePanel;
    public GameObject ChoosePanelBorder;
    public UnityEngine.UI.Button LeftChooseButton;
    public UnityEngine.UI.Button RightChooseButton;
    public TextMeshProUGUI Prompt;
    public TextMeshProUGUI LeftChoise;
    public TextMeshProUGUI RightChoise;


    public TextMeshProUGUI currentStage;

    public AudioSource audio1;
    public AudioSource audio2;

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

        firstPlayerToEnd = null;

        FocusCameraOnCurrentPlayer(playerOneObject);
        ShowTurnStartText();
        //StartCoroutine(playerOne.StartTurn());

        StartCoroutine(PlayMusic(audio1,1));

        Resources.UnloadUnusedAssets();
    }

    void Update()
    {
        if(currentPlayerTurn == Players.PlayerOne)
        {
            FocusCameraOnCurrentPlayer(playerOneObject);
            currentStage.color = Color.blue;
            currentStage.text = playerOne.path.stageName;
        }
        else
        {
            FocusCameraOnCurrentPlayer(playerTwoObject);
            currentStage.color = Color.red;
            currentStage.text = playerTwo.path.stageName;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(EscapePressed());
        }

        if (playerOne.playerAtTheEnd)
        {
            playerOne.rolledDice = true;
            playerOne.isReadyToEndTurn = true;
        }
        if (playerTwo.playerAtTheEnd) 
        {
            playerTwo.rolledDice = true;
            playerTwo.isReadyToEndTurn = true;
        }


        if(playerOne.playerAtTheEnd && playerTwo.playerAtTheEnd)
        {
            playerOne.isReadyToEndTurn = false;
            playerTwo.isReadyToEndTurn = false;
            double playerOneScore = playerOne.GetScore();
            double playerTwoScore = playerTwo.GetScore();
            if (firstPlayerToEnd == Players.PlayerOne)
            {
                playerOneScore += 10;
            }
            else
            {
                playerTwoScore += 10;
            }

            if (playerOneScore > playerTwoScore)
            {
                StartCoroutine(EndGame(1,2,playerOneScore,playerTwoScore,false));
            }
            else if (playerTwoScore > playerOneScore)
            {
                StartCoroutine(EndGame(2, 1, playerTwoScore, playerOneScore, false));
            }
            else
            {
                StartCoroutine(EndGame(1, 2, playerOneScore, playerOneScore, true));
            }

        }

        if(playerOne.playerAtTheEnd && firstPlayerToEnd == null)
        {
            firstPlayerToEnd = Players.PlayerOne;
        }

        if (playerTwo.playerAtTheEnd && firstPlayerToEnd == null)
        {
            firstPlayerToEnd = Players.PlayerTwo;
        }
    }
    private IEnumerator PlayMusic(AudioSource audioSource, int num)
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        yield return StartCoroutine(PlayMusic(num == 1 ? audio2 : audio1, num == 1 ? 2 : 1));
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
            playerOne.rolledDice = true;
            playerTwo.rolledDice = false;
            playerOne.isReadyToEndTurn = false;
            playerTwo.isReadyToEndTurn = false;
            playerOne.AlertPanel.SetActive(false);
            playerOne.AlertPanelBorder.SetActive(false);
            if (playerTwo.playerAtTheEnd)
            {
                playerTwo.AlertPanel.SetActive(true);
                playerTwo.AlertPanelBorder.SetActive(true);
            }
            PlayerOneDiceAnimator.ResetTrigger("Idle");
            PlayerOneDiceAnimator.SetTrigger("Idle");
            //playerOne.dice.ReturnToIdle();
            //Debug.Log("ready two");
            //StartCoroutine(playerTwo.StartTurn());
        }
        else
        {
            currentPlayerTurn = Players.PlayerOne;
            playerOne.isMyTurn = true;
            playerTwo.isMyTurn = false;
            playerOne.rolledDice = false;
            playerTwo.rolledDice = true;
            playerOne.isReadyToEndTurn = false;
            playerTwo.isReadyToEndTurn = false;
            if (playerOne.playerAtTheEnd)
            {
                playerOne.AlertPanel.SetActive(true);
                playerOne.AlertPanelBorder.SetActive(true);
            }
            PlayerTwoDiceAnimator.ResetTrigger("Idle");
            PlayerTwoDiceAnimator.SetTrigger("Idle");
            //playerTwo.dice.ReturnToIdle();
            //Debug.Log("ready one");
            //StartCoroutine(playerOne.StartTurn());
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
    public NegativeEvent GetNegativeEvent(int stage)
    {
        NegativeEvents negativeEvents = JsonUtility.FromJson<NegativeEvents>(negativeJsons[stage].text);

        return negativeEvents.events[Random.Range(0,negativeEvents.events.Count)];
    }

    public NegativeEvent GetChanceEvent(int stage)
    {
        int fate = Random.Range(1,11);

        if (fate == 10)
        {
            return GetNegativeEvent(stage);
        }
        else
        {
            NegativeEvents negativeEvents = JsonUtility.FromJson<NegativeEvents>(positiveJsons[stage].text);

            return negativeEvents.events[Random.Range(0, negativeEvents.events.Count)];
        }
    }

    public RiskEvent GetRiskEvent(int stage)
    {


        RiskEvents riskEvents = JsonUtility.FromJson<RiskEvents>(riskJsons[stage].text);

        return riskEvents.events[Random.Range(0, riskEvents.events.Count)];
    }

    private IEnumerator EndGame(int playerWonNumber, int playerLostNumber, double playerWonPoints, double playerLostPoints, bool draw)
    {
        LeftChoise.text = "Tak";
        RightChoise.text = "Nie";
        if (draw)
        {
            Prompt.text = $"Remis! Wszyscy gracze posiadaj¹ {playerWonPoints} punktów. Czy rozpocz¹æ now¹ grê?";
        }
        else 
        {
            Prompt.text = $"Gratulacje! Gracz {playerWonNumber} wygrywa z {playerWonPoints} punktami. Gracz {playerLostNumber} posiada {playerLostPoints} punktów. Czy rozpocz¹æ now¹ grê?";
        }

        ChoosePanelBorder.SetActive(true);
        ChoosePanel.SetActive(true);

        var waitForButton = new WaitForUIButtons(LeftChooseButton, RightChooseButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == LeftChooseButton)
        {
            levelLoader.LoadLevelAsync(1);
        }
        else
        {
            levelLoader.LoadLevelAsync(0);
        }

    }

    private IEnumerator EscapePressed()
    {
        LeftChoise.text = "Tak";
        RightChoise.text = "Nie";
        Prompt.text = "Czy na pewno chcesz wyjœæ do menu?";
        
        ChoosePanelBorder.SetActive(true);
        ChoosePanel.SetActive(true);

        var waitForButton = new WaitForUIButtons(LeftChooseButton, RightChooseButton);
        yield return waitForButton.Reset();
        if (waitForButton.PressedButton == LeftChooseButton)
        {
            levelLoader.LoadLevelAsync(0);
        }
        else
        {
            ChoosePanelBorder.SetActive(false);
            ChoosePanel.SetActive(false);
        }

    }
}
