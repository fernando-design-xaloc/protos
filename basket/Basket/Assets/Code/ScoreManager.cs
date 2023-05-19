using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public int limitScore;

    public int player1Score;
    public int player2Score;


    public void restartScores()
    {
        player1Score = 0;
        player2Score = 0;
    }

    public void increasePlayer1Score()
    {
        player1Score++;
    }

    public void increasePlayer2Score()
    {
        player2Score++;
    }

    public void finalizeMatch()
    {
        if(player1Score >= limitScore || player2Score >= limitScore)
        {
            //finalziar partida
            restartScores();
            SituationManager.instance.endMatch();
        }
        else
        {
            SituationManager.instance.loadSituation(situationToLoad.START);
        }
    }

}
