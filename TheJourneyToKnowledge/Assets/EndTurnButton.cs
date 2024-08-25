using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public MeshRenderer PlayerOneDice;
    public MeshRenderer PlayerTwoDice;

    void Start()
    {

    }

    
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        if (!GameMaster.instance.RequestToEndTurn())
        {
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
