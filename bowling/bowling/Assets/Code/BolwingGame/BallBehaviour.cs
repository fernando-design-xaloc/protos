using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Pin") && !BowlingGameManager.instance.isAPinHit)
        {
            BowlingGameManager.instance.firstPinHitOnTurn();
        }
    }
}
