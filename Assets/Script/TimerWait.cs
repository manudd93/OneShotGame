using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerWait : MonoBehaviour
{


    private static TimerWait instance;

    public static TimerWait Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("CoroutineHelper");
                instance = obj.AddComponent<TimerWait>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }

    public static void EseguiDopo(float secondi, Action azione)
    {
        Instance.StartCoroutine(Instance.Attendi(secondi, azione));
    }

    private IEnumerator Attendi(float secondi, Action azione)
    {
        yield return new WaitForSeconds(secondi);
        azione?.Invoke();
    }
}

