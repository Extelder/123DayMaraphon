using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Level : MonoBehaviour
{
    [SerializeField] private string _name;

    [Inject] private PlayerCharacter _character;

    public static Level Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        _character.Hints.AcceptNewHint("Level - " + _name);
        PlayerPrefs.SetString(_name, "Unlocked");
    }

    public void CompleateLevel()
    {
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, gameObject.name);
    }
}