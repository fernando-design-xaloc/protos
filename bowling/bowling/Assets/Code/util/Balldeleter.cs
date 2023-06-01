using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balldeleter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Ball"))
        {
            Destroy(other.gameObject);
            if(!BowlingGameManager.instance.isAPinHit) 
            { 
                BowlingGameManager.instance.firstPinHitOnTurn();                       
            }
        }
    }
}
