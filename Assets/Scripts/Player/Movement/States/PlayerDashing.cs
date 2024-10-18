using System;
using System.Collections;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
    [SerializeField] private KeyCode _dashKey;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private float _dashSpeed;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private Transform _orientation;    

    private Rigidbody _rigidbody;

    public event Action PlayerDashed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(Dashing());
    }

    private IEnumerator Dashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.002f);
            if(Input.GetKey(_dashKey))
            {
                PlayerDashed?.Invoke();
                yield return new WaitForSeconds(_dashCooldown);
            }
        }
    }

    public void Dash()
    {
        Vector3 forceToApply = (_orientation.forward * _dashSpeed + _orientation.up * _dashUpwardForce);
        _rigidbody.AddForce(forceToApply, ForceMode.Impulse);
    }
}
