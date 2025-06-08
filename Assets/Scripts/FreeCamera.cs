using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{ 
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minYAngle;
    [SerializeField] private float _maxYAngle;

    private float _rotationX;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;

        _rotationX -= mouseY;
        _rotationX = Mathf.Clamp(_rotationX, _minYAngle, _maxYAngle);

        transform.localRotation = Quaternion.Euler(_rotationX, transform.localRotation.eulerAngles.y + mouseX, 0);

        float moveX = Input.GetAxis("Horizontal") * _moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * _moveSpeed * Time.deltaTime;

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.position += move;
    }
}
