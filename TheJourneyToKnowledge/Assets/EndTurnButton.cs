using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public MeshRenderer PlayerOneDice;
    public MeshRenderer PlayerTwoDice;
    
    public GameObject DiceAlert;
    public GameObject DiceAlertBorder;

    public void ButtonClicked()
    {
        if (!GameMaster.instance.RequestToEndTurn())
        {
            DiceAlert.SetActive(true);
            DiceAlertBorder.SetActive(true);
            return;
        }
        else 
        {
            if (PlayerOneDice.enabled)
            {
                PlayerOneDice.enabled = false;
                PlayerTwoDice.enabled = true;
            }
            else
            {
                PlayerOneDice.enabled = true;
                PlayerTwoDice.enabled = false;
            }
        }
    }
}
