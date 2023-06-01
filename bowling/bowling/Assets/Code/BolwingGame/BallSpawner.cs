using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : Singleton<BallSpawner>
{
    public GameObject spawnedBall;


    [Header("Properties to spawn the Ball")]
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private GameObject spawnZone;


    [Header("Shoot configurable properties")]
    [SerializeField]
    private float minVelocity;
    [SerializeField]
    private float maxVelocity;


    public void performRectShoot(Vector3 directionToShoot, float percentajeVelocityUsed, Vector3 positionToSpawn)
    {
        BowlingGameManager.instance.isBallGenerated = true;
        float forceToApply = (percentajeVelocityUsed * (maxVelocity - minVelocity) / 100) + minVelocity;        
        
        spawnedBall = Instantiate(ball, positionToSpawn,Quaternion.identity);
        spawnedBall.GetComponent<Rigidbody>().AddForce(directionToShoot * forceToApply);
        CanvasBehaviour.instance.holdOn();
        BowlingGameManager.instance.timeFromBallShoot= Time.time;
    }


}
