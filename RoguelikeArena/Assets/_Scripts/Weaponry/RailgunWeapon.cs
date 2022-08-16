using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunWeapon : RaycastWeapon {

    [SerializeField] Transform visualFirepoint;
    [SerializeField] float beamWidth;

    public override void Shoot() {
        Vector2 startPos = firepoint.position;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, firepoint.rotation.eulerAngles.z + Random.Range(-1f, 1f) * spread);
        Vector2 fireDir = bulletRotation * Vector2.up;

        RaycastHit2D[] hitInfo = new RaycastHit2D[pierce];

        int hits = Physics2D.BoxCast(startPos, new Vector2(beamWidth, beamWidth), Vector2.Angle(Vector2.zero, fireDir), fireDir, contactFilter, hitInfo, range);

        bool trailMade = false;
        for (int i = 0; i < hits; i++) {

            // Apply hit
            Health target = hitInfo[i].transform.GetComponent<Health>();
            if (target != null && target.gameObject.layer != (int)Layers.Player) {
                target.TakeDamage(damage * PlayerController.stats[(int)PlayerController.StatID.damage], fireDir * knockback);
            }

            if (i == (pierce - 1) || hitInfo[i].transform.gameObject.layer == (int)Layers.Terrain) {
                // Make trail
                bulletTrail.MakeTrail(visualFirepoint.position, hitInfo[i].point);
                trailMade = true;
                break;
            }

        }
        if (!trailMade) {
            // Make trail
            bulletTrail.MakeTrail(visualFirepoint.position, startPos + fireDir * range);
        }
    }
    /*
    private void HandleCollisions(Vector2 point, Vector2 fireDir, float distance) {
        Collider2D[] hits = new Collider2D[64];
        int numHits = Physics2D.OverlapBox(point, new Vector2(1.5f,distance), Vector2.Angle(Vector2.zero, fireDir), contactFilter, hits);
        for (int i = 0; i < numHits; i++) {
            //print(hits[i].name +" " + i);
           // var colLayer = collision.gameObject.layer;

            if (hits[i].gameObject.layer == (int)Layers.Enemy) {
                hits[i].GetComponent<Health>().TakeDamage(PlayerController.Instance.DamageMod, fireDir * knockback);
            }
        }

    }*/
}
