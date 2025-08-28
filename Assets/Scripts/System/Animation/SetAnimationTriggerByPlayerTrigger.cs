using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimationTriggerByPlayerTrigger : PlayerTrigger
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;
    
    public override void OnTriggered()
    {
        _animator.SetTrigger(_triggerName);
    }
}
