using UnityEngine;

public class PlayerHip : MonoBehaviour
{
    [SerializeField] private Pool _killHipPool;

    private GameObject _currentHip;

    public void ChangeHip(GameObject hip)
    {
        if (_currentHip != hip)
        {
            _currentHip?.SetActive(false);
            _currentHip = hip;
            _killHipPool.transform.localScale = _currentHip.transform.localScale;
            _currentHip.SetActive(true);
        }
    }
}