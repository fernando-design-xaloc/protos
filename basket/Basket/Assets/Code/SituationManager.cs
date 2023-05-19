using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

[System.Serializable]
public struct Situation
{
    public GameObject start;
    public GameObject player1Wins;
    public GameObject player2Wins;
    public GameObject playerDraws;
}


public enum situationToLoad { START, PLAYER_1_WINS, PLAYER_2_WINS, PLAYER_DRAWS }


public class SituationManager : Singleton<SituationManager>
{
    private GameObject situationsDirector;

    [Header("Situations to Play") ]
    [SerializeField]
    private List<Situation> situations = new List<Situation>();
    private Situation activeSituation;

    [Header("Canvas to configure")]
    [SerializeField]
    private GameObject playerSelectionMovementCanvas;

    [SerializeField]
    private GameObject scoreCanvas;

    [SerializeField]
    private GameObject player1ScoreText;

    [SerializeField]
    private GameObject player2ScoreText;



    [SerializeField]
    private GameObject mainMenuCanvas;

    private void PlayableDirector_Starting_stopped(PlayableDirector obj)
    {
        playerSelectionMovementCanvas.SetActive(true);
    }

    private void PlayableDirector_Resolution_stopped(PlayableDirector obj)
    {
        scoreCanvas.SetActive(true);
        player1ScoreText.GetComponent<TextMeshProUGUI>().text = ScoreManager.instance.player1Score.ToString();
        player2ScoreText.GetComponent<TextMeshProUGUI>().text = ScoreManager.instance.player2Score.ToString();

    }

    public void loadSituation(int situationToLoad)
    {
        loadSituation((situationToLoad)situationToLoad);
    }

    public void loadMainMenu()
    {
        playerSelectionMovementCanvas.SetActive(false);
        scoreCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void startMatch()
    {
        mainMenuCanvas.SetActive(false);
        loadSituation(situationToLoad.START);
    }

    public void endMatch()
    {
        if (situationsDirector != null)
        {
            Destroy(situationsDirector);
        }
        mainMenuCanvas.SetActive(true);
    }


    public void loadSituation(situationToLoad situationToLoad)
    {
        if(situationsDirector != null)
        {
            Destroy(situationsDirector);
        }

        if (situationToLoad == situationToLoad.START)
        {

            activeSituation = situations[Random.Range(0, situations.Count)];

            situationsDirector = Instantiate(activeSituation.start);
            situationsDirector.GetComponentInChildren<PlayableDirector>().stopped += PlayableDirector_Starting_stopped;
            playerSelectionMovementCanvas.SetActive(false);
            scoreCanvas.SetActive(false);

            situationsDirector.GetComponentInChildren<PlayableDirector>().Play();
        }
        else 
        {

            playerSelectionMovementCanvas.SetActive(false);
            if (situationToLoad == situationToLoad.PLAYER_1_WINS)
            {
                situationsDirector = Instantiate(activeSituation.player1Wins);
            }
            else if(situationToLoad == situationToLoad.PLAYER_2_WINS)
            {
                situationsDirector = Instantiate(activeSituation.player2Wins);
            }
            else
            {
                situationsDirector = Instantiate(activeSituation.playerDraws);
            }

            situationsDirector.GetComponentInChildren<PlayableDirector>().stopped += PlayableDirector_Resolution_stopped;
            situationsDirector.GetComponentInChildren<PlayableDirector>().Play();
        }
    }


}
