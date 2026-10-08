using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeMovement : MonoBehaviour
{
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _layerMask;

    [Header("Ground stick")]
    [Tooltip("Насколько ниже ног ищем землю, чтобы не взлетать с перегиба склона")]
    [SerializeField] private float _snapDistance = 1.2f;
    [Tooltip("Сколько секунд после схода со склона ещё прижимаем к земле")]
    [SerializeField] private float _slopeMemory = 0.3f;
    [SerializeField] private float _stickAcceleration = 30f;

    private RaycastHit _slopeHit;

    private Rigidbody _rigidbody;
    private float _lastSlopeTime = -10f;
    private float _stickSuspendedUntil;

    // Прыжок, джамппад, отдача и т.п. — их подъём прижимать нельзя.
    public static void SuspendGroundStick(Component player, float seconds)
    {
        if (player != null && player.TryGetComponent(out PlayerSlopeMovement slope))
            slope._stickSuspendedUntil = Mathf.Max(slope._stickSuspendedUntil, Time.time + seconds);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.useGravity = !OnSlope();
    }

    // На склоне гравитация выключена, и быстрое движение/дэш вверх по склону превращается
    // в скорость "от поверхности" — на перегибе игрока подкидывает. Срезаем эту составляющую,
    // пока игрок на склоне или только что с него сошёл; на ровном полу поведение прежнее.
    private void FixedUpdate()
    {
        if (Time.time < _stickSuspendedUntil)
            return;

        bool onSlope = OnSlope();
        if (onSlope)
            _lastSlopeTime = Time.time;
        else if (Time.time - _lastSlopeTime > _slopeMemory)
            return;

        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit ground,
                _playerHeight * 0.5f + _snapDistance, _layerMask))
            return;

        Vector3 velocity = _rigidbody.velocity;
        float awayFromGround = Vector3.Dot(velocity, ground.normal);
        if (awayFromGround > 0f)
            _rigidbody.velocity = velocity - ground.normal * awayFromGround;

        if (onSlope)
            _rigidbody.AddForce(-ground.normal * _stickAcceleration, ForceMode.Acceleration);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, 
            out _slopeHit, _playerHeight * 0.5f + 0.3f, _layerMask))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }

        return false;
    }
}
