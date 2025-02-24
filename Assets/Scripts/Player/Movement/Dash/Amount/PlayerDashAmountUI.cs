using UnityEngine;
using UnityEngine.UI;

public class PlayerDashAmountUI : MonoBehaviour
{
    [SerializeField] private PlayerDashAmount _dashAmount;
    [SerializeField] private GameObject _chargeObject;

    private void OnEnable()
    {
        _dashAmount.AmountChanged += OnAmountChanged;
    }

    private void OnAmountChanged(float value)
    {
        _chargeObject.transform.localScale = new Vector3(_chargeObject.transform.localScale.x, value,
            _chargeObject.transform.localScale.z);
    }

    private void OnDisable()
    {
        _dashAmount.AmountChanged -= OnAmountChanged;
    }
}