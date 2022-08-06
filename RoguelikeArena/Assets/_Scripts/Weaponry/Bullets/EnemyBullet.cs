using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

    [SerializeField][Range(0, 1f)] float playerPrediction;
    const int ignoreLayer = (int)Layers.Enemy;
    
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

    protected override void SetVelocity(float velocity) {
        rb.velocity = ((Vector2)transform.up * velocity) + PlayerController.Instance.GetComponent<Rigidbody2D>().velocity * playerPrediction;
    }

}
