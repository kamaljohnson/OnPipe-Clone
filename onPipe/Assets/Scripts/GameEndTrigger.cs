using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Ring"))
        {
            Game.gameState = GameStatus.GameWonUi;
        }
        
    }
}
