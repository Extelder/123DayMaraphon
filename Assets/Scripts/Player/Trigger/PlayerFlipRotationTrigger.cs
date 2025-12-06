using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerFlipRotationTrigger : PlayerTrigger
{
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _targetRotation;
    [SerializeField] private float _gravityModifier;
    [SerializeField] private float _cameraPosOffset = 2.34f;

    private Tween _tweenCin;
    private Tween _tweenPl;
    private Tween _tweenHb;

    public override void OnTriggered()
    {
        _tweenCin = PlayerCharacter.Instance.CinemachineParent.transform.DOLocalRotate(new Vector3(
                PlayerCharacter.Instance.CinemachineParent.transform.localEulerAngles.x,
                PlayerCharacter.Instance.CinemachineParent.transform.localEulerAngles.y, _targetRotation),
            _rotationSpeed,
            RotateMode.LocalAxisAdd);

        _tweenPl = PlayerCharacter.Instance.Transform.DOLocalRotate(new Vector3(
                PlayerCharacter.Instance.Transform.localEulerAngles.x,
                PlayerCharacter.Instance.Transform.localEulerAngles.y, _targetRotation), _rotationSpeed,
            RotateMode.LocalAxisAdd);

        _tweenHb = PlayerCharacter.Instance.HeadBobParent.DOLocalMoveY(_cameraPosOffset, _rotationSpeed);

        Physics.gravity = new Vector3(0, Physics.gravity.y * _gravityModifier, 0);
    }
}