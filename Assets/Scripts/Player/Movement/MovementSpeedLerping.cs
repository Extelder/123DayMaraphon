using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementSpeedLerping : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float startValue;
    [SerializeField] protected float speedChangeFactor;
    [SerializeField] protected float targetMoveSpeed;

    public IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(targetMoveSpeed - moveSpeed);
        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, targetMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }
    }

    public IEnumerator SmoothlyLerpMoveSpeedToStartValue()
    {
        float time = 0;
        float difference = Mathf.Abs(targetMoveSpeed - startValue);
        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, startValue, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }
    }
}