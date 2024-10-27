using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerArmorPickUpable : MonoBehaviour, IPickupable
{
    [Inject] private PlayerHealth _playerHealth;
    [SerializeField] private float _valueToReservation;

    [SerializeField] private bool _destroyAfterReservation;

    public void PickUp()
    {
        _playerHealth.Armor(_valueToReservation);
        if (_destroyAfterReservation)
        {
            Destroy(gameObject);
        }
    }
}
