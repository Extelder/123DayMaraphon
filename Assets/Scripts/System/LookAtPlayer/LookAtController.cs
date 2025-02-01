using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LookAtController : MonoBehaviour
{
    [Inject] private PlayerCharacter _playerCharacter;
    
    [SerializeField] private float _headWeight;
    [SerializeField] private float _bodyWeight;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex) {
        _animator.SetLookAtPosition(_playerCharacter.Transform.position);
        _animator.SetLookAtWeight(1, _bodyWeight, _headWeight);
    }
}
