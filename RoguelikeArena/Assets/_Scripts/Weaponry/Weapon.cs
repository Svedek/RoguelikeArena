using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    // Default weapon is raycast

    [field: SerializeField] public float attackSpeed { get; private set; }
    [SerializeField] private float range, knockback;
    [SerializeField] protected float damage, spread; // Spread is in degrees
    [SerializeField] private int pierce = 1;

    [HideInInspector] public float damageMod = 1f;

    private BulletTrail bulletTrail;
    private void Awake() {
        bulletTrail = GetComponentInChildren<BulletTrail>();
    }

    private const int layerMask = (1 << ((int)Layers.Enemy)) + (1 << ((int)Layers.Terrain));
    public virtual void Shoot() {
        Vector2 startPos = transform.position;
        Vector2 fireDir = transform.up.normalized;
        RaycastHit2D[] hitInfo = new RaycastHit2D[pierce];

        ContactFilter2D cf = new ContactFilter2D();
        cf = cf.NoFilter();
        LayerMask lm = new LayerMask();
        lm.value = layerMask;
        cf.layerMask = lm;

        int hits = Physics2D.Raycast(startPos, fireDir, cf, hitInfo, range);
        
        bool trailMade = false;
        for (int i = 0; i < hits; i++) {
            
            // Apply hit
            Health target = hitInfo[i].transform.GetComponent<Health>();
            if (target != null) {
                target.TakeDamage(damage * damageMod, fireDir * knockback);
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






    void Update() {
        // Follow
    }
}
