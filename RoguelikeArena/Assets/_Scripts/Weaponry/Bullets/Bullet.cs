using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    protected float knockback, velocity, range = 999f;
    protected float distanceTraveled;
    protected Vector2 lastPos;
    private int pierce;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start() {
        lastPos = transform.position;
    }

    private void Update() {
        
        distanceTraveled += Vector2.Distance(lastPos, transform.position);
        lastPos = rb.position;

        if (distanceTraveled >= range) {
            DestroyBullet();
        }
        /*
        range -= Time.deltaTime * 10;
        if (range <= 0) {
            DestroyBullet();
        }
        */
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) { // TODO Look over
        var colLayer = collision.gameObject.layer;

        switch (colLayer) {
            case (int)Layers.Enemy: // EnemyLayer
                if (collision.GetComponent<Health>().TakeDamage(PlayerController.Instance.WeaponDamage, rb.velocity.normalized * knockback)) {
                    if (--pierce <= 0) {
                        DestroyBullet();
                    }
                }
                break;
            case (int)Layers.Terrain: // TerrainLayer
                DestroyBullet();
                break;
        }
    }

    public void InitializeBullet(float knockback, float velocity, float range, int pierce) {
        this.knockback = knockback;
        this.velocity = velocity;
        this.range = range;
        this.pierce = pierce;

        rb.velocity = ((Vector2)transform.up * velocity) + PlayerController.Instance.GetComponent<Rigidbody2D>().velocity * 0.5f;


        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    protected virtual void DestroyBullet() {
        Destroy(gameObject);
    }
}
