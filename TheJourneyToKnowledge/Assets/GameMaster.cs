using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public enum Players
    {
        PlayerOne = 1,
        PlayerTwo = 2
    }
    public Players PlayerTurn;
    void Start()
    {
        PlayerTurn = Players.PlayerOne;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RequestToEndTurn(Players applicant)
    {
        if(PlayerTurn == applicant){
            PlayerTurn = applicant == Players.PlayerOne ? Players.PlayerTwo : Players.PlayerOne;
            return true;
        }
        return false;
    }
}
