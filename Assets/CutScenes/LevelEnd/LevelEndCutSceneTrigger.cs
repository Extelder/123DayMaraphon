using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndCutSceneTrigger : PlayerTrigger
{
    [SerializeField] private LevelEndCutScene _levelEndCutScene;

    public override void Triggered()
    {
        _levelEndCutScene.StartCutScene();
    }
}