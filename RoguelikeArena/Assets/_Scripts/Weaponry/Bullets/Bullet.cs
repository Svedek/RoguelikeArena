using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] float knockback, velocity, bulletTime;
    private float damage, endTime;

    private void Start() {
        endTime = Time.time + bulletTime;
    }

    private bool initialized = false;
    public void InitializeBullet(float damage) {
        // Initialize data
        this.damage = damage;

        // Setup functioning
        endTime = Time.time + bulletTime;
        GetComponent<Rigidbody2D>().velocity = transform.up * velocity;
        GetComponent<Collider2D>().enabled = true;
    }

    private void Update() {
        if (initialized && Time.time >= endTime) {
            DestroyBullet();
        }
    }

    [SerializeField] private Layers ignoreLayer;
    protected virtual void OnTriggerEnter2D(Collider2D collision) { // TODO Look over
        var colLayer = collision.gameObject.layer;

        if (colLayer == (int)ignoreLayer) return;
        print("col");
        switch (colLayer) {
            case (int)Layers.Enemy: // EnemyLayer
                if(collision.GetComponent<Health>().TakeDamage(damage, GetComponent<Rigidbody2D>().velocity.normalized * knockback)) {
                    DestroyBullet();
                }
                break;
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


    protected virtual void DestroyBullet() {
        Destroy(gameObject);
    }
}
