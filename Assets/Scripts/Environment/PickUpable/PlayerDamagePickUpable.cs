using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class PlayerDamagePickUpable : MonoBehaviour, IPickupable
{
    [SerializeField] private float _damage;
    [Inject] private PlayerHealth _health;

    public void PickUp()
    {
        _health.TakeDamage(_damage);
    }
}
