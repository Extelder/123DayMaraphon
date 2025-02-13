using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnterDoor : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _boolName = "Enter";

    private bool _currenState;

    public void ClikcOnDoor()
    {
        _currenState = !_currenState;
        _animator.SetBool(_boolName, _currenState);
    }
}