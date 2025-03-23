using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBool : MonoBehaviour
{
    [SerializeField] private string _boolName;
    [SerializeField] private Animator _animator;

    public void SetBoolTrue()
    {
        _animator.SetBool(_boolName, true);   
    }
    
    public void SetBoolFalse()
    {
        _animator.SetBool(_boolName, false);
    }
}
