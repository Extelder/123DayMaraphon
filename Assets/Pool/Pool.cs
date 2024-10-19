using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private PoolObject _prefabPoolObject;
    [SerializeField] private Transform _container;

    [SerializeField] private int _minObjectsCapacity;
    [SerializeField] private int _maxObjectsCapacity;
    [SerializeField] private bool _autoExpand;

    private List<PoolObject> _poolObjects;

    private void OnValidate()
    {
        if (_autoExpand)
        {
            _maxObjectsCapacity = Int32.MaxValue;
        }
    }

    private void Awake()
    {
        Create();
    }

    private void Create()
    {
        _poolObjects = new List<PoolObject>(_minObjectsCapacity);

        for (int i = 0; i < _minObjectsCapacity; i++)
        {
            CreateElement();
        }
    }

    private PoolObject CreateElement(bool isObjectActive = false)
    {
        var createdElement = Instantiate(_prefabPoolObject, _container);
        createdElement.gameObject.SetActive(isObjectActive);

        _poolObjects.Add(createdElement);

        return createdElement;
    }

    private bool TryGetElement(out PoolObject element)
    {
        foreach (var obj in _poolObjects)
        {
            if (obj.gameObject.activeInHierarchy == false)
            {
                element = obj;
                obj.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public PoolObject GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        return element;
    }

    public PoolObject GetFreeElement(Vector3 position, Quaternion rotation)
    {
        var element = GetFreeElement(position);
        element.transform.rotation = rotation;
        return element;
    }

    public PoolObject GetFreeElement(Vector3 position, Quaternion rotation, Transform parent)
    {
        var element = GetFreeElement(position, rotation);
        element.transform.parent = parent;
        return element;
    }

    public PoolObject GetFreeElement()
    {
        if (TryGetElement(out PoolObject elemnt))
        {
            return elemnt;
        }

        if (_autoExpand || _poolObjects.Count < _maxObjectsCapacity)
        {
            return CreateElement(true);
        }

        throw new Exception("Pool is over!");
    }
}