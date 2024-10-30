using UnityEngine;

public class PlayerHip : MonoBehaviour
{
    private GameObject _currentHip;

    public void ChangeHip(GameObject hip)
    {
        if (_currentHip != hip)
        {
            _currentHip?.SetActive(false);
            _currentHip = hip;
            _currentHip.SetActive(true);
        }
    }
}