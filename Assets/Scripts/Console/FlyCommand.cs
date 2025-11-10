using System.Collections;
using System.Collections.Generic;
using ED.SC;
using UnityEngine;

public class FlyCommand : MonoBehaviour
{
    [SerializeField] private GameObject _flyCharacter;

    private GameObject _spawnedCharacter;

    [Command]
    public void Fly()
    {
        if (PlayerCharacter.Instance != null)
        {
            _spawnedCharacter =
                Instantiate(_flyCharacter, PlayerCharacter.Instance.Transform.position, Quaternion.identity);
            PlayerCharacter.Instance.gameObject.SetActive(false);
        }
        else
        {
            _spawnedCharacter =
                Instantiate(_flyCharacter, transform.position, Quaternion.identity);
        }
    }

    [Command]
    public void StopFly()
    {
        if (PlayerCharacter.Instance != null)
            PlayerCharacter.Instance.gameObject.SetActive(true);


        if (_spawnedCharacter != null)
        {
            Destroy(_spawnedCharacter);
            _spawnedCharacter = null;
        }
    }
}