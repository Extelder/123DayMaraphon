using UnityEngine;

public class PlayerDashRecoverPickUpable : MonoBehaviour, IPickupable
{
    [SerializeField] private float _valueToRecover;

    [SerializeField] private bool _destroyAfterRecover;

    public void PickUp()
    {
        PlayerJumpAmount.Instance.RecoverSpeed(_valueToRecover);
        if (_destroyAfterRecover)
        {
            Destroy(gameObject);
        }
    }
}
