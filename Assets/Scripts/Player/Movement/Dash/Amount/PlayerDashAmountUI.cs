using UnityEngine;
using UnityEngine.UI;

public class PlayerDashAmountUI : MonoBehaviour
{
    [SerializeField] private PlayerDashAmount _dashAmount;
    [SerializeField] private Image _bar;

    private void OnEnable()
    {
        _dashAmount.AmountChanged += OnAmountChanged;
    }

    private void OnAmountChanged(float value)
    {
        _bar.fillAmount = value;
    }

    private void OnDisable()
    {
        _dashAmount.AmountChanged -= OnAmountChanged;
    }
}