using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectileMoveAfterPlayer : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    private void OnEnable()
    {
        Transform _playerTransform = PlayerCharacter.Instance.Transform;
        transform.SetParent(_playerTransform);
        StartCoroutine(SetProjectileOutOfParent());
    }

    private IEnumerator SetProjectileOutOfParent()
    {
        yield return new WaitForSeconds(_cooldown);
        transform.parent = null;
    }
}
