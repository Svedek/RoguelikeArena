using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

    [SerializeField] float damage;
    const int ignoreLayer = (int)Layers.Enemy;

    protected override void Start() {
        lastPos = transform.position;
        GetComponent<Rigidbody2D>().velocity = transform.up * velocity;

        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
    
    protected override void OnTriggerEnter2D(Collider2D collision) { // TODO Look over
        var colLayer = collision.gameObject.layer;

        switch (colLayer) {
            case (int)Layers.Terrain: // TerrainLayer
                DestroyBullet();
                break;
            case (int)Layers.Player: // PlayerLayer
                if (PlayerController.Instance.TakeDamage(damage, GetComponent<Rigidbody2D>().velocity.normalized * knockback)) {
                    DestroyBullet();
                }
                break;
            default:
                break;
        }
    }
    public void InitializeBullet(float damage) {
        // idkfking know
        this.damage = damage;
    }
}
