using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRexetTriggerOnEnableOnDisable : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggerName;
    
    private void OnEnable()
    {
         _animator.SetTrigger(_triggerName);
    }
    
    private void OnDisable()
    {
         _animator.ResetTrigger(_triggerName);    
    }
}
