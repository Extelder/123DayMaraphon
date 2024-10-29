using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpAmountUI : MonoBehaviour
{
    [SerializeField] private PlayerJumpAmount _jumpAmount;
    [SerializeField] private Image _valueAmount;

    private void OnEnable()
    {
        _jumpAmount.AmountChanged += JumpsValueChanged;
    }

    private void OnDisable()
    {
        _jumpAmount.AmountChanged -= JumpsValueChanged;
    }

    private void JumpsValueChanged(float value)
    {
        _valueAmount.fillAmount = value;
    }
}
