using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFly : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void Update()
    {
        float yAxis = 0;

        if (Input.GetKey(KeyCode.Space))
            yAxis = 1;

        if (Input.GetKey(KeyCode.LeftShift))
            yAxis = -1;

        Vector3 moveDirection =
            new Vector3(Input.GetAxis("Horizontal"), yAxis, Input.GetAxis("Vertical"));

        Vector3 final = transform.TransformDirection(moveDirection);

        transform.Translate(final * Time.deltaTime * _speed, Space.World);
    }
}