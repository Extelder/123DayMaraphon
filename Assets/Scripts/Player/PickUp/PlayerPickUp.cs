using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IPickupable>(out IPickupable pickupable))
        {
            pickupable.PickUp();
        }
    }
}
