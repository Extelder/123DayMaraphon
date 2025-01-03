using UnityEngine;
using Zenject;

public class PlayerHealPickUpable : MonoBehaviour, IPickupable
{
    [Inject] private PlayerHealth _playerHealth;
    [SerializeField] private float _valueToHeal;

    [SerializeField] private bool _destroyAfterHeal;

    public void PickUp()
    {
        _playerHealth.Heal(_valueToHeal);

        if (_destroyAfterHeal)
        {
            Destroy(gameObject);
        }
    }
}