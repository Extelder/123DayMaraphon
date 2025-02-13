using UnityEngine;
using System.Collections;
public class RandomizeObjects : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;
    [SerializeField] private float _coolDown;

    private void Awake()
    {
        StartCoroutine(Randomize());
    }

    private IEnumerator Randomize()
    {
        _objects[Random.Range(0, _objects.Length)].SetActive(true);
        yield return new WaitForSeconds(_coolDown);
    }
}
