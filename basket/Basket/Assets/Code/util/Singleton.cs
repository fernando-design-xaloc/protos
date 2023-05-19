using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instace;

    public static T instance
    {
        get
        {
            if (_instace == null)
            {
                _instace = GameObject.FindObjectOfType<T>();
            }
            return _instace;
        }
    }
}
