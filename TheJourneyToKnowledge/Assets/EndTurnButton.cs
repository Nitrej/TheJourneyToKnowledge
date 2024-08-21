using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    private GameMaster gameMaster;
    void Start()
    {
        gameMaster = FindFirstObjectByType<GameMaster>();
    }

    
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        if (!gameMaster.RequestToEndTurn())
        {

        }
    }
}
