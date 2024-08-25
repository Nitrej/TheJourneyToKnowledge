using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
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

        }
    }
}
