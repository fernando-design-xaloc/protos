using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.Windows;


//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.6/manual/Interactions.html

public class BolwingController : Singleton<BolwingController>
{
    public bool isDrawingShoot;
    public BowlingInput inputMap;


    [SerializeField]
    private GameObject auxBallToInstantiate;
    private GameObject instantiatedAuxBall;

    [Header("Finger Position Values")]
    [SerializeField]
    private Vector2 fingerCurrentPosition;
    [SerializeField]
    private Vector2 fingerStartingPosition;
    [SerializeField]
    private Vector2 fingerEndPosition;

    [Header("Finger Delta Values")]
    [SerializeField]
    private Vector2 fingerCurrentDelta;
    [SerializeField]
    private float minDeltaToShoot;
    [SerializeField]
    private float maxDeltaToShoot;



    [Header("Hold configurable properties")]
    [SerializeField]
    private float holdTime = 0.5f;
    public float holdingFingerStartTime;


    [Header("Aux Ball configurable properties")]
    [SerializeField]
    private GameObject ballSpawnpointReferenceStart;
    [SerializeField]
    private GameObject ballSpawnpointReferenceEnd;


    #region MonoBehaviour Methods
    private void Awake()
    {
        inputMap = new();

        inputMap.BowlingGampley.DrawDelta.performed += GetDeltaFromFingerMovement;

        inputMap.BowlingGampley.DrawPosition.performed += GetPositionFromFingerMovement;

        inputMap.BowlingGampley.PressAction.started += PressButtonStarted;
        inputMap.BowlingGampley.PressAction.canceled +=  PressButtonCanceled;

        instantiatedAuxBall = Instantiate(auxBallToInstantiate);
        instantiatedAuxBall.SetActive(false);
    }


    private void Update()
    {
        if (!isDrawingShoot&&Time.time >= holdingFingerStartTime + holdTime)
        {
            isDrawingShoot = true;
        }
        else if (isDrawingShoot)
        {
            fingerCurrentPosition = inputMap.BowlingGampley.DrawPosition.ReadValue<Vector2>();
        }
        if (instantiatedAuxBall.activeSelf)
        {
            instantiatedAuxBall.transform.position = generateBallPosition(fingerCurrentPosition);
        }
    }
    #endregion


    #region Delta Events
    public void GetDeltaFromFingerMovement(InputAction.CallbackContext context)
    {
        fingerCurrentDelta = context.ReadValue<Vector2>();
    }
    #endregion


    #region Position Events
    private void GetPositionFromFingerMovement(InputAction.CallbackContext context)
    {
        fingerCurrentPosition = context.ReadValue<Vector2>();
    }
    #endregion


    #region Button Events
    private void PressButtonStarted(InputAction.CallbackContext context)
    {
        holdingFingerStartTime = Time.time;
        fingerStartingPosition = fingerCurrentPosition;
        //TODO: generate aux ball
        instantiatedAuxBall.SetActive(true);
    }

    private void PressButtonCanceled(InputAction.CallbackContext context)
    {
        fingerEndPosition = fingerCurrentPosition;
        if (isDrawingShoot && validateShootingValues())
        {
            isDrawingShoot = false;
            performShoot();
        }
        //TODO: remove aux ball
        instantiatedAuxBall.SetActive(false);
    }
    #endregion


    private bool validateShootingValues()
    {
        if(fingerEndPosition.y <= fingerStartingPosition.y)
        {
            return false;
        }
        if (fingerCurrentDelta.magnitude < minDeltaToShoot)
        {
            return false;
        }

        return true;
    }

    private void performShoot()
    {
        fingerEndPosition = fingerCurrentPosition;

        float percentage = calculatePercentajeOfForceToShoot();

        //Calculo de la dirección de disparo
        Vector3 fingerEndPositionOnWorld = Camera.main.ScreenToWorldPoint(new Vector3(fingerEndPosition.x,fingerEndPosition.y, Camera.main.farClipPlane));
       
        Vector3 spawnPosition = generateBallPosition(fingerEndPosition);

        Vector3 directionToShoot = (fingerEndPositionOnWorld - spawnPosition).normalized;
        directionToShoot.y = 0;

        BallSpawner.instance.performRectShoot(directionToShoot, percentage, spawnPosition);

        BowlingGameManager.instance.endAttempt();
    }

    private float calculatePercentajeOfForceToShoot()
    {
        float magnitude = Mathf.Clamp(fingerCurrentDelta.magnitude, minDeltaToShoot, maxDeltaToShoot);

        float range = maxDeltaToShoot - minDeltaToShoot;
        float correctedStartValue = magnitude - minDeltaToShoot;
        float percentage = (correctedStartValue * 100) / range;

        return percentage;

    }

    private Vector3 generateBallPosition(Vector2 position)
    {
        float range = Camera.main.pixelWidth;
        float percentage = (position.x * 100) / range;

        float xPosition = (percentage * (ballSpawnpointReferenceEnd.transform.position.x - ballSpawnpointReferenceStart.transform.position.x) / 100) + ballSpawnpointReferenceStart.transform.position.x;
        xPosition = Math.Clamp(xPosition, ballSpawnpointReferenceStart.transform.position.x, ballSpawnpointReferenceEnd.transform.position.x);
        return new Vector3(xPosition,ballSpawnpointReferenceStart.transform.position.y, ballSpawnpointReferenceStart.transform.position.z);
    }

}
