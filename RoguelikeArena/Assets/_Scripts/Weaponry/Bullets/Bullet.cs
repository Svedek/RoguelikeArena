using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] protected float damage, range = 10f, knockback, velocity;
    [SerializeField] protected int pierce;
    protected Vector2 lastPos;
    protected float distanceTraveled;

    protected Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        SetVelocity(velocity);

        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state) {
        var on = state == GameState.Playing;
        enabled = on;
        rb.simulated = on;
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
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) { // TODO Look over
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
                DestroyBullet();
                break;
        }
    }

    protected virtual void SetVelocity(float velocity) {
        rb.velocity = ((Vector2)transform.up * velocity) + PlayerController.Instance.GetComponent<Rigidbody2D>().velocity * 0.2f;
    }

    protected virtual void DestroyBullet() {
        Destroy(gameObject);
    }
}
