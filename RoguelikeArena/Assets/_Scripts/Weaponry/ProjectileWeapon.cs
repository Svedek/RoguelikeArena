using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

    [SerializeField] GameObject bulletPrefab;

    public override void Shoot() {
        Quaternion bulletRotation = Quaternion.Euler(0, 0, firepoint.rotation.eulerAngles.z + Random.Range(-1f, 1f) * spread);

        Instantiate(bulletPrefab, firepoint.position, bulletRotation, MiscManager.Instance.BulletParent);
        //GameObject bulletObj = Instantiate(bulletPrefab, firepoint.position, bulletRotation, MiscManager.Instance.BulletParent);
        //Bullet bullet = bulletObj.GetComponent<Bullet>();
    }
}
