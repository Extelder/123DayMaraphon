using System;
using System.Collections;
using UnityEngine;

// Хост корутин для статических эффектов (серии бумов с задержками). Живёт в сцене, пересоздаётся после загрузки.
public class FxRunner : MonoBehaviour
{
    private static FxRunner _instance;

    public static void Delay(float seconds, Action action)
    {
        if (seconds <= 0f)
        {
            action();
            return;
        }

        if (_instance == null)
            _instance = new GameObject("FxRunner").AddComponent<FxRunner>();
        _instance.StartCoroutine(_instance.Run(seconds, action));
    }

    private IEnumerator Run(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
