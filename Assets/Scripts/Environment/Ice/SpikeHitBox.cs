using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHitBox : MonoBehaviour, IWeaponVisitor
{
    [SerializeField] private GameObject[] _props;

    public void Visit(WeaponShoot weaponShoot)
    {
        BreakWall();
    }

    public void Visit(KunitanShoot kunitanShoot)
    {
        BreakWall();
    }

    public void Visit(KunitanaUltimateAttack kunitanShoot)
    {
        BreakWall();
    }

    public void Visit(RaycastWeaponShoot raycastWeaponShoot, RaycastHit hit)
    {
        BreakWall();
    }

    public void Visit(Projectile projectile)
    {
        BreakWall();
    }

    public void Visit(Ghost ghost, float damage)
    {
    }

    public void Visit(PlayerSlashProjectile slashProjectile)
    {
        BreakWall();
    }

    private void BreakWall()
    {
        for (int i = 0; i < _props.Length; i++)
        {
            _props[i].SetActive(false);
        }
    }
}
