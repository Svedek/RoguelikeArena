using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : Weapon {

    [SerializeField] protected float range, knockback, damage;
    [SerializeField] protected int pierce = 1;

    protected BulletTrail bulletTrail;
    private void Awake() {
        bulletTrail = GetComponentInChildren<BulletTrail>();
    }

    private const int layerMask = (1 << ((int)Layers.Enemy)) + (1 << ((int)Layers.Terrain));
    protected readonly ContactFilter2D contactFilter = GetContactFilter();
    private static ContactFilter2D GetContactFilter(){
        ContactFilter2D cf = new ContactFilter2D();
        LayerMask lm = new LayerMask();
        lm.value = layerMask;
        cf.layerMask = lm;
        return cf;
    }

    public override void Shoot() {
        Vector2 startPos = firepoint.position;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, firepoint.rotation.eulerAngles.z + Random.Range(-1f, 1f) * spread);
        Vector2 fireDir = bulletRotation * Vector2.up;

        RaycastHit2D[] hitInfo = new RaycastHit2D[pierce];

        int hits = Physics2D.Raycast(startPos, fireDir, contactFilter, hitInfo, range);
        
        bool trailMade = false;
        for (int i = 0; i < hits; i++) {
            
            // Apply hit
            Health target = hitInfo[i].transform.GetComponent<Health>();
            if (target != null && target.gameObject.layer != (int)Layers.Player) {
                target.TakeDamage(damage * PlayerController.stats[(int)PlayerController.StatID.damage], fireDir * knockback);
            }
            
            if (i == (pierce - 1) || hitInfo[i].transform.gameObject.layer == (int)Layers.Terrain) {
                // Make trail
                bulletTrail.MakeTrail(startPos, hitInfo[i].point);
                trailMade = true;
                break;
            }

        }
        if (!trailMade) {
            // Make trail
            bulletTrail.MakeTrail(startPos, startPos + fireDir * range);
        }
    }
}
