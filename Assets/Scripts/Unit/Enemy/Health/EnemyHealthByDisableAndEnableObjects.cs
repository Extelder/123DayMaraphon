using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthByDisableAndEnableObjects : Health
{
    [SerializeField] private GameObject _objectToEnable;
    [SerializeField] private GameObject _objectToDisable;

    public override void Death()
    {
        _objectToDisable.SetActive(false);
        _objectToEnable.SetActive(true);

        _objectToEnable.transform.parent = null;
        _objectToEnable.transform.SetParent(null);
        Destroy(gameObject);
        Destroy(_objectToEnable, 23);
    }
}