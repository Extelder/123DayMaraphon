using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnitHitBox : UnitHitBox, IMovable
{
    [field: SerializeField] public Transform UnitParent { get; set; }
    [SerializeField] private float _speedMultiplier;

    public override void Visit(PlayerSlashProjectile slashProjectile)
    {
        Debug.Log("SLASHHHH");
        base.Visit(slashProjectile);
        Vector3 delta = transform.position - PlayerCharacter.Instance.Transform.position;
        UnitParent.position = Vector3.MoveTowards(UnitParent.position,
            delta,
            slashProjectile.CharacterForce * Time.deltaTime * _speedMultiplier);
    }
}