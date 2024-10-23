using System.Collections;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Dashing : MonoBehaviour
{
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashUpwardForce;
    [SerializeField] protected Transform orientation;

    [SerializeField] protected float targetMoveSpeed;
    [SerializeField] float speedChangeFactor;
    protected bool cooldownRecovered = true;

    protected Rigidbody rigidbody;

    protected Coroutine LerpSpeedCoroutine;

    private float _defaultDashSpeed;

    private void Awake()
    {
        _defaultDashSpeed = dashSpeed;
        rigidbody = GetComponent<Rigidbody>();
    }

    public void AddImpulse(Vector3 forceToApply, float cooldown, CompositeDisposable disposable)
    {
        StopAllCoroutines();

        dashSpeed = _defaultDashSpeed;
        LerpSpeedCoroutine = StartCoroutine(SmoothlyLerpMoveSpeed(forceToApply));
        rigidbody.AddForce(forceToApply, ForceMode.Impulse);

        cooldownRecovered = false;

        CoolDown.Timer(cooldown, () => { cooldownRecovered = true; }, disposable);
    }

    private IEnumerator SmoothlyLerpMoveSpeed(Vector3 velocity)
    {
        float time = 0;
        float difference = Mathf.Abs(targetMoveSpeed - dashSpeed);
        float startValue = dashSpeed;
        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            dashSpeed = Mathf.Lerp(startValue, targetMoveSpeed, time / difference);

            rigidbody.velocity += velocity;

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        dashSpeed = startValue;
    }
}