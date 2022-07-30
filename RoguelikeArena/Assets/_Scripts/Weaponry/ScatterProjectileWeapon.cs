using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterProjectileWeapon : ProjectileWeapon {

    [SerializeField] int bullets;

    public override void Shoot() {
        for (int i = 0; i < bullets; i++) {
            base.Shoot();
        }
    }
}
