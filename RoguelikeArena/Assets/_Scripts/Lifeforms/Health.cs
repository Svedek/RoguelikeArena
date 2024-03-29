using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    protected Rigidbody2D rb;
    protected virtual void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }


    [Header("Health")]
    [SerializeField] protected float health;
    [SerializeField] protected float kbMod;
    public bool Dead { get { return dead; } }
    private bool dead = false;

    public virtual bool TakeDamage(float damage, Vector2 knockback) {
        if ((health -= damage) <= 0) Die();
        rb.AddForce(knockback * kbMod, ForceMode2D.Impulse);
        return true;
    }
    protected virtual void Die() {
        dead = true;
    }
}
