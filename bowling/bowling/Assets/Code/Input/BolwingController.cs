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
    

    #region MonoBehaviour Methods
    private void Awake()
    {
        inputMap = new();

        inputMap.BowlingGampley.DrawDelta.performed += GetDeltaFromFingerMovement;

        inputMap.BowlingGampley.DrawPosition.performed += GetPositionFromFingerMovement;

        inputMap.BowlingGampley.PressAction.started += PressButtonStarted;
        inputMap.BowlingGampley.PressAction.canceled +=  PressButtonCanceled;

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
    }

    private void PressButtonCanceled(InputAction.CallbackContext context)
    {
        fingerEndPosition = fingerCurrentPosition;
        if (isDrawingShoot && validateShootingValues())
        {
            isDrawingShoot = false;
            performShoot();
        }
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

        float magnitude = Mathf.Clamp(fingerCurrentDelta.magnitude,minDeltaToShoot,maxDeltaToShoot);

        float range = maxDeltaToShoot - minDeltaToShoot;
        float correctedStartValue = magnitude - minDeltaToShoot;
        float percentage = (correctedStartValue * 100) / range;


        Vector3 fingerEndPositionOnWorld = Camera.main.ScreenToWorldPoint(new Vector3(fingerEndPosition.x,fingerEndPosition.y,Camera.main.farClipPlane));
        Vector3 directionNormalized = (fingerEndPositionOnWorld - BallSpawner.instance.spawningReferenceLeft.transform.position).normalized;


        BallSpawner.instance.performRectShoot(directionNormalized, percentage,fingerStartingPosition);
        BowlingGameManager.instance.endAttempt();
    }

}
