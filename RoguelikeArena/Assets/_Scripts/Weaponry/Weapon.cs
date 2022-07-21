using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    // Default weapon is raycast

    [field: SerializeField] public float attackSpeed { get; private set; }
    [SerializeField] private float range, knockback;
    [SerializeField] protected float damage, spread; // Spread is in degrees
    [SerializeField] private bool pierce;

    [HideInInspector] public float damageMod = 1f;

    private BulletTrail bulletTrail;
    private void Awake() {
        bulletTrail = GetComponentInChildren<BulletTrail>();
    }

    private readonly int layerMask = (1 << ((int)Layers.Enemy)) + (1 << ((int)Layers.Terrain));
    public virtual void Shoot() {
        Vector2 startPos = transform.position;
        Vector2 fireDir = ((Vector2)transform.up - startPos).normalized;
        RaycastHit2D hitInfo;


        if (pierce) {
            hitInfo = Physics2D.Raycast(startPos, fireDir, range, layerMask);
        } else {
            hitInfo = Physics2D.Raycast(startPos, fireDir, range, layerMask);
        }

        // TODO ONLY WORKS FOR NON-PIERCE
        if (hitInfo) {
            // Apply hit
            Health target = hitInfo.transform.GetComponent<Health>();
            if (target != null) {
                target.TakeDamage(damage * damageMod, fireDir * knockback);
            }
            // Make trail
            bulletTrail.MakeTrail(startPos, hitInfo.point);

        } else {
            // Make trail
            bulletTrail.MakeTrail(startPos, startPos + fireDir * range);
        }
    }






void Update() {
        // Follow
    }
}
