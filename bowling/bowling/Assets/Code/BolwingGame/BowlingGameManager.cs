using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowlingGameManager : Singleton<BowlingGameManager>
{

    private int previousQuantity;

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
    public float timeFromBallShoot;
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
                disableHitPins();
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
        if (currentFrame != null)
        {
            Destroy(currentFrame.pinsOfTheFrame);
        }
        GameObject pinsOfTheFrame = Instantiate(pins, pinsSpawnPosition.transform.position, pinsSpawnPosition.transform.rotation);
        currentFrame = new FrameLogic(pinsOfTheFrame);
        startAttempt();
        CanvasBehaviour.instance.updateCurrentPins(0, currentFrame.quantityOfRemainingShoots);
    }


    public void startAttempt()
    {
        isAPinHit = false;
        disableHitPins();
        BolwingController.instance.inputMap.BowlingGampley.Enable();
        CanvasBehaviour.instance.shootNow();
    }


    private void disableHitPins()
    {
        IList<PinBehaviour> pins = currentFrame.pinsOfTheFrame.GetComponentsInChildren<PinBehaviour>();
        IList<PinBehaviour> hitPins = new List<PinBehaviour>();
        foreach (PinBehaviour pin in pins)
        {
            if (pin.isPinHit)
            {
                hitPins.Add(pin);
            }
        }

        if(currentFrame.quantityOfRemainingShoots == 1)
        {
            previousQuantity = hitPins.Count;
            CanvasBehaviour.instance.updateCurrentPins(previousQuantity, currentFrame.quantityOfRemainingShoots);
        }
        else
        {
            CanvasBehaviour.instance.updateCurrentPins(previousQuantity+hitPins.Count, currentFrame.quantityOfRemainingShoots);
        }

        foreach (PinBehaviour pin in hitPins)
        {
            pin.gameObject.SetActive(false);
        }

        if (hitPins.Count == 10)
        {
            CanvasBehaviour.instance.performStrike();
            startFrame();
        }else if (currentFrame.quantityOfRemainingShoots == 0 && hitPins.Count+previousQuantity >=10)
        {
            CanvasBehaviour.instance.performAlmostStrike();
        }
    }

    public void endAttempt()
    {
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
