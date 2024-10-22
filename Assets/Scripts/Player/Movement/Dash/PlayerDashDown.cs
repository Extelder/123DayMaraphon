using UniRx;
using UnityEngine;

public class PlayerDashDown : Dashing
{
    [Header("Settings")]
    [SerializeField] private float _dashDownCooldown;

    private CompositeDisposable _dashDownDisposable = new CompositeDisposable();

    public void DashDown()
    {
        if (!cooldownRecovered)
            return;

        Vector3 forceToApply = (orientation.up * -dashUpwardForce);
        float cooldown = _dashDownCooldown;

        AddImpulse(forceToApply, cooldown, _dashDownDisposable);
    }
    private void OnDisable()
    {
        _dashDownDisposable.Clear();
    }
}
