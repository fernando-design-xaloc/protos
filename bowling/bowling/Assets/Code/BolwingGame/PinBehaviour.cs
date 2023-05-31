using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBehaviour : MonoBehaviour
{
    public bool isPinHit;
    [SerializeField]
    Rigidbody pinRigiBody;

    void Update()
    {
        if (!isPinHit)
        {
            if (pinRigiBody.velocity!= Vector3.zero)
            {
                isPinHit= true;
            }
        }
    }
}
