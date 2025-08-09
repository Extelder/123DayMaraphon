using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostExplosionAnimator : MonoBehaviour
{
    [SerializeField] private GhostHitBox _ghostHitBox;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _triggername;

    private void OnEnable()
    {
        _ghostHitBox.RPGProjectilHitted += OnExploded;
    }

    private void OnExploded()
    {
        _animator.SetBool(_triggername, true);
    }

    private void OnDisable()
    {
        _animator.SetBool(_triggername, false);
        _ghostHitBox.RPGProjectilHitted -= OnExploded;
    }
}
