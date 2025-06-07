using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PickUpCollectables : MonoBehaviour, IPickupable
{
    [SerializeField] private AudioSource _pickUpSound;

    [Inject] private Pools _pool;

    [SerializeField] private int _id;
    [SerializeField] private string _key;

    private void Start()
    {
        if (PlayerPrefs.GetInt(_key + _id, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    public void PickUp()
    {
        PlayerPrefs.SetInt(_key + _id, 1);
        PlayerPrefs.SetInt(_key + "Collected", PlayerPrefs.GetInt(_key + "Collected", 0) + 1);

        _pool.DefaultImpactPool.GetFreeElement(transform.position);

        _pickUpSound.Play();
        Destroy(gameObject);
    }
}