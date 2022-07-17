using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private float damage;
    [SerializeField] private float velocity;
    [SerializeField] private float bulletTime;
    private float endTime;

    private void Start() {
        GetComponent<Rigidbody2D>().velocity = transform.up * velocity;
        endTime = Time.time + bulletTime;
    }

    private void Update() {
        if (Time.time >= endTime) {
            DestroyBullet();
        }
    }

    [SerializeField] private Layers ignoreLayer;
    private readonly static int enemyLayer = 8, terrainLayer = 9, playerLayer = 10;
    protected virtual void OnTriggerEnter2D(Collider2D collision) { // TODO
        var colLayer = collision.gameObject.layer;

        if (colLayer == (int)ignoreLayer) return;
        print("col");
        switch (colLayer) {
            case 8: // EnemyLayer
                if (collision.gameObject.layer == 10) {
                    DestroyBullet();
                }
                break;
            case 9: // TerrainLayer
                break;
            case 10: // PlayerLayer
                PlayerController.Instance.TakeDamage(damage, GetComponent<Rigidbody2D>().velocity.normalized); // REPLACE VEL
                break;
            default:
                break;
        }
    }


    protected virtual void DestroyBullet() {
        Destroy(gameObject);
    }
}
