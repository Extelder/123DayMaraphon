using UnityEngine;

public class BloodSplatRandomizer : MonoBehaviour
{
    [SerializeField] private GameObject[] _bloodSplatVariants;

    private void OnEnable()
    {
        RandomizeBloodSplat();
    }
    private void RandomizeBloodSplat()
    {
        _bloodSplatVariants[Random.Range(0, _bloodSplatVariants.Length)].SetActive(true);
    }
}
