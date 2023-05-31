using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : Singleton<BallSpawner>
{
    public GameObject spawnedBall;


    [Header("Properties to spawn the Ball")]
    [SerializeField]
    private GameObject ball;
    public  GameObject spawningReferenceLeft;
    public  GameObject spawningReferenceRight;


    [Header("Shoot configurable properties")]
    [SerializeField]
    private float minVelocity;
    [SerializeField]
    private float maxVelocity;


    public void performRectShoot(Vector3 playerDirectionOnScreen, float percentajeVelocityUsed, Vector2 playerInitialFingerPosition)
    {
        BowlingGameManager.instance.isBallGenerated = true;
        float forceToApply = (percentajeVelocityUsed * (maxVelocity - minVelocity) / 100) + minVelocity;

        playerDirectionOnScreen.y = 0;
        Debug.Log("Player direction " + playerDirectionOnScreen*forceToApply);

        calculatePointToSpawn(playerInitialFingerPosition);
        spawnedBall = Instantiate(ball, spawningReferenceLeft.transform.position, spawningReferenceLeft.transform.rotation);
        spawnedBall.GetComponent<Rigidbody>().AddForce(playerDirectionOnScreen*forceToApply);   
        


    }
    /*
        Mates jiji
        https://math.stackexchange.com/questions/717746/closest-point-on-a-line-to-another-point
    */
    private void calculatePointToSpawn(Vector2 playerInitialFingerPosition)
    {
        Vector3 fingerStartPositionOnWorld = Camera.main.ScreenToWorldPoint(new Vector3(playerInitialFingerPosition.x, playerInitialFingerPosition.y, Camera.main.farClipPlane));
        
        Vector3 spawnDirection = spawningReferenceRight.transform.position - spawningReferenceLeft.transform.position;




    }


}
