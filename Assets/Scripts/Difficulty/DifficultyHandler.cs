using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyType
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}

public class DifficultyHandler : MonoBehaviour
{
    [SerializeField] private Difficulty _easyDifficulty;
    [SerializeField] private Difficulty _mediumDifficulty;
    [SerializeField] private Difficulty _hardDifficulty;

    [field: NaughtyAttributes.ReadOnly]
    [field: SerializeField]
    public Difficulty CurrentDifficulty { get; private set; }

    public event Action<Difficulty> DifficultyChanged;

    public static DifficultyHandler Instance { get; private set; }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            Bootstrap();
            DontDestroyOnLoad(gameObject);
            return;
        }

        Debug.LogError("There`s one more DifficultyHandler in Scene!");
        Debug.Break();
        Destroy(gameObject);
    }

    private void Bootstrap()
    {
        switch (PlayerPrefs.GetInt("Difficulty", 2))
        {
            case 1:
                ChangeDifficulty(_easyDifficulty);
                break;
            case 2:
                ChangeDifficulty(_mediumDifficulty);
                break;
            case 3:
                ChangeDifficulty(_hardDifficulty);
                break;
        }
    }

    public void ChangeDifficulty(Difficulty difficulty)
    {
        if (CurrentDifficulty != difficulty)
        {
            DifficultyChanged?.Invoke(CurrentDifficulty = difficulty);
        }
    }
}