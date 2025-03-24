using UnityEngine;

public class BloodSplatRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] _bloodSplatVariants;

    private GameObject _currentBlood;

    private void OnEnable()
    {
        _currentBlood?.SetActive(false);
        RandomizeBloodSplat();
    }

    private void RandomizeBloodSplat()
    {
        _currentBlood = _bloodSplatVariants[Random.Range(0, _bloodSplatVariants.Length)];
        _currentBlood.SetActive(true);
    }
}