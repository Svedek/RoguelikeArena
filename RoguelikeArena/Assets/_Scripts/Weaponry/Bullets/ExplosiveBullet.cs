using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet {

    [SerializeField] bool bouncy;
    [SerializeField] GameObject explosion;

    protected override void Start() {
        base.Start();
        if (bouncy) Invoke("DestroyBullet", 5f);
    }

    protected override void OnTriggerEnter2D(Collider2D collision) {
        
        var colLayer = collision.gameObject.layer;

        switch (colLayer) {
            case (int)Layers.Enemy: // EnemyLayer
                if (collision.GetComponent<Health>().TakeDamage(PlayerController.Instance.DamageMod, rb.velocity.normalized * knockback)) {
                    if (--pierce <= 0) {
                        DestroyBullet();
                    }
                }
                break;
            case (int)Layers.Terrain: // TerrainLayer
                if (bouncy) {
                    var direction = Vector3.Reflect(rb.velocity.normalized, collision.transform.right);

                    rb.velocity = rb.velocity.magnitude * direction;
                } else {
                    DestroyBullet();
                }
                break;
        }
    }

    protected override void DestroyBullet() {
        Instantiate(explosion, transform.position, Quaternion.identity, MiscManager.Instance.BulletParent);
        base.DestroyBullet();
    }
}
