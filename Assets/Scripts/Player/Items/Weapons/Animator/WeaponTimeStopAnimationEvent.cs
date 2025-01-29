using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTimeStopAnimationEvent : MonoBehaviour
{
    [SerializeField] private float _stoppedTime;

    private void TimeStopAnimationEvent()
    {
        PlayerTime.Instance.TimeStop(_stoppedTime);
    }
}