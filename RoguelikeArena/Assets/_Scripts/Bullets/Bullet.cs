using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private float damage;
    [SerializeField] private float knockback;
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
