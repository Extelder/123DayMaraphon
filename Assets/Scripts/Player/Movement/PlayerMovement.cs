using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerWalking _walking;
    [SerializeField] private PlayerDashing _dashing;
    public bool Moving { get; private set; }

    private void Update()
    {
        _walking.Walk();
    }

    private void OnEnable()
    {
        _dashing.PlayerDashed += Dashing;
    }
    private void OnDisable ()
    {
        _dashing.PlayerDashed -= Dashing;
    }

    private void Dashing()
    {
        _dashing.Dash();
    }
}
