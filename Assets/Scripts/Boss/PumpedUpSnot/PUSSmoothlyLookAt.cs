using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUSSmoothlyLookAt : MonoBehaviour
{
    [SerializeField] private float _turnSpeed;

    private float _defaultSpeed;

    public Transform Target { get; set; }

    public Transform Player { get; set; }

    private void Awake()
    {
        Player = PlayerCharacter.Instance.Transform;
        Target = Player;
        _defaultSpeed = _turnSpeed;
    }

    public void ReturnToDefaultSpeed()
    {
        _turnSpeed = _defaultSpeed;
    }

    public void SetTurnSpeed(float value)
    {
        _turnSpeed = value;
    }

    private void Update()
    {
        Vector3 targetDir = Target.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir),
            Time.deltaTime * _turnSpeed);
    }
}