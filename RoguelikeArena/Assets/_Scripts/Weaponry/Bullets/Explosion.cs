using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [SerializeField] float size, damage, knockback;
    void Awake() {
    }
    private void Start() {
        transform.localScale *= size;
        Invoke("DestroyExplosion", 0.1f);
    }

    private void DestroyExplosion() {
        Destroy(gameObject);
    }


    [SerializeField] Layers ignoreLayer;
    private void OnTriggerEnter2D(Collider2D collision) {
        Health health = collision.GetComponent<Health>();

        if (collision.gameObject.layer == (int)ignoreLayer) return;
        if (health) {
            health.TakeDamage(damage, (collision.transform.position - transform.position) * knockback);
        }
    }
}
