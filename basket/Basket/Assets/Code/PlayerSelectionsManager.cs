using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMovements { FORCE=0, DEFENSE=1, TRICK=2}

public class PlayerSelectionsManager : MonoBehaviour
{

    PlayerMovements playerChoosenMovement;
    PlayerMovements otherPlayerChoosenMovement;
    
    public void playerSelectsAmovement(int movement)
    {
        playerChoosenMovement = (PlayerMovements)movement;
        otherPlayerChoosenMovement = (PlayerMovements)Random.Range(0, 3);

        switch (movementSolution(playerChoosenMovement,otherPlayerChoosenMovement)) {
            case 0:
                Debug.Log("Jugador ha empatado");
                SituationManager.instance.loadSituation(situationToLoad.PLAYER_DRAWS);
                break;

            case 1:
                Debug.Log("Jugador ha ganado");
                ScoreManager.instance.increasePlayer1Score();
                SituationManager.instance.loadSituation(situationToLoad.PLAYER_1_WINS);
                break;

            case -1:
                Debug.Log("Jugador ha perdido");
                ScoreManager.instance.increasePlayer2Score();
                SituationManager.instance.loadSituation(situationToLoad.PLAYER_2_WINS);
                break;       
        }
    }

    private int movementSolution(PlayerMovements player1Movement, PlayerMovements player2Movement)
    {
        if (player1Movement == PlayerMovements.FORCE)
        {
            if (player2Movement == PlayerMovements.FORCE)
            {
                return 0;
            }else if (player2Movement == PlayerMovements.TRICK)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }else if (player1Movement == PlayerMovements.TRICK)
        {
            if (player2Movement == PlayerMovements.FORCE)
            {
                return -1;
            }
            else if (player2Movement == PlayerMovements.TRICK)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            if (player2Movement == PlayerMovements.FORCE)
            {
                return 1;
            }
            else if (player2Movement == PlayerMovements.TRICK)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }





}