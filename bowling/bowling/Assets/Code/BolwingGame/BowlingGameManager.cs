using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowlingGameManager : Singleton<BowlingGameManager>
{
    [Header("Initial properties")]
    [SerializeField]
    private GameObject pins;
    [SerializeField]
    private GameObject pinsSpawnPosition;
    private FrameLogic currentFrame;


    [Header("Attempt Properties")]
    [SerializeField]
    private float timeAfterFirstPinHit = 1;
    private float timeHit;
    public bool isAPinHit;


    [Header("Missing Ball properties")]
    private float timeFromBallShoot;
    [SerializeField]
    private float timeToTakeABallAsMiss = 10;
    public bool isBallGenerated;


    private void Start()
    {
        isBallGenerated= false;
        startFrame();     
    }


    private void Update()
    {
        if ((isAPinHit && Time.time >= timeHit + timeAfterFirstPinHit) || (isBallGenerated && Time.time >= timeFromBallShoot + timeToTakeABallAsMiss))
        {
            if (BallSpawner.instance.spawnedBall != null)
            {
                Debug.Log("Ball destroyed because of time");
                Destroy(BallSpawner.instance.spawnedBall);
            }

            if (currentFrame.quantityOfRemainingShoots <= 0)
            {
                startFrame();
            }
            else
            {
                startAttempt();
            }
        }
    }

    public void startFrame()
    {
        Debug.Log("Frame Started");
        if (currentFrame != null)
        {
            Destroy(currentFrame.pinsOfTheFrame);
        }

        GameObject pinsOfTheFrame = Instantiate(pins, pinsSpawnPosition.transform.position, pinsSpawnPosition.transform.rotation);
        currentFrame = new FrameLogic(pinsOfTheFrame);
        startAttempt();
    }

    public void startAttempt()
    {
        Debug.Log("Attempt Started");
        isAPinHit = false;
        disableHitPins();
        BolwingController.instance.inputMap.BowlingGampley.Enable();
    }

    private void disableHitPins()
    {
        Debug.Log("Pins disabled");
        IList<PinBehaviour> hitPins = currentFrame.pinsOfTheFrame.GetComponentsInChildren<PinBehaviour>().Where(pin => pin.isPinHit == true).ToList();
        foreach (PinBehaviour pin in hitPins)
        {
            pin.gameObject.SetActive(false);
        }
        if (hitPins.Count == 10)
        {
            startFrame();
        }

    }

    public void endAttempt()
    {
        Debug.Log("Attempt Ended");
        isBallGenerated = false;
        currentFrame.quantityOfRemainingShoots--;
        BolwingController.instance.inputMap.BowlingGampley.Disable();

    }

    public void firstPinHitOnTurn()
    {
        timeHit = Time.time;
        isAPinHit = true;
    }

}
