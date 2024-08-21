using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOne : MonoBehaviour
{
    public GameMaster.Players player;

    private bool isMyTurn;
    private bool isReadyToEndTurn;
    private GameMaster gameMaster;
    void Start()
    {
        gameMaster = FindFirstObjectByType<GameMaster>();
        isMyTurn = gameMaster.currentPlayerTurn == player;
    }

    void Update()
    {
        
    }

}
