using UnityEngine;

public class PlayerHip : MonoBehaviour
{
    [SerializeField] private Pool _killHipPool;

    private GameObject _currentHip;

    public void ChangeHip(GameObject hip, Vector3 scaleFactor)
    {
        if (_currentHip != hip)
        {
            _currentHip?.SetActive(false);
            _currentHip = hip;
            _killHipPool.transform.localScale = scaleFactor;
            _currentHip.SetActive(true);
        }
    }
}