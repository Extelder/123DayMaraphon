using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlayerHealPickUpable : MonoBehaviour, IPickupable
{
    [SerializeField] private float _valueToHeal;

    [SerializeField] private bool _destroyAfterHeal;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _upwardForce;
    [SerializeField] private float _randomForceMax;
    [SerializeField] private float _randomForceMin;

    private void OnEnable()
    {
        _rigidbody.AddForce(
            new Vector3(Random.Range(_randomForceMin, _randomForceMax), _upwardForce, Random.Range(_randomForceMin, _randomForceMax)),
            ForceMode.Impulse);
    }

    public void PickUp()
    {
        PlayerHealth.Instance.Heal(_valueToHeal);

        if (_destroyAfterHeal)
        {
            gameObject.SetActive(false);
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
}