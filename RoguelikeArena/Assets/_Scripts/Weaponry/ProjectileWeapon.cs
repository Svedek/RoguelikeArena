using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletVelocity;

    void Awake() {
        
    }

    public override void Shoot() {
        Quaternion bulletRotation = Quaternion.Euler(0, 0, firepoint.rotation.eulerAngles.z + Random.Range(-1f, 1f) * spread);

        GameObject bulletObj = Instantiate(bulletPrefab, firepoint.position, bulletRotation); // Instanciate under parent?
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        bullet.InitializeBullet(knockback, bulletVelocity, range, pierce);
    }
}
