using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceButton : MonoBehaviour
{
    public PlayerOne PlayerOne;
    public PlayerOne PlayerTwo;

    public GameObject AlertPanel;
    public GameObject AlertPanelBorder;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        if(GameMaster.instance.currentPlayerTurn == GameMaster.Players.PlayerOne && !PlayerOne.rolledDice)
        {
            StartCoroutine(PlayerOne.StartTurn());
        }else if(GameMaster.instance.currentPlayerTurn == GameMaster.Players.PlayerOne && PlayerOne.rolledDice)
        {
            AlertPanel.SetActive(true);
            AlertPanelBorder.SetActive(true);
        }
        if(GameMaster.instance.currentPlayerTurn == GameMaster.Players.PlayerTwo && !PlayerTwo.rolledDice) 
        {
            StartCoroutine(PlayerTwo.StartTurn());
        }else if(GameMaster.instance.currentPlayerTurn == GameMaster.Players.PlayerTwo && PlayerTwo.rolledDice)
        {
            AlertPanel.SetActive(true);
            AlertPanelBorder.SetActive(true);
        }
    }
}
