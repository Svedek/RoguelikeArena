using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemy : Enemy {

    void Update() {
        CalculateMove();
    }
    void FixedUpdate() {
        Move();
    }

    #region Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDir;
    private void CalculateMove() {
        moveDir = (PlayerController.Instance.transform.position - transform.position).normalized;
        moveDir *= moveSpeed;
    }

    private void Move() {
        rb.AddForce(moveDir, ForceMode2D.Force);
    }
    #endregion

    #region attack
    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    private const float attackDelay = 0.2f;
    private bool attackReady = true;
    /*private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController play = collision.gameObject.GetComponent<PlayerController>();
        if (play != null) {
            play.TakeDamage(damage, moveDir.normalized * knockback);
        }
    }*/
    private void OnCollisionStay2D(Collision2D collision) {
        if (!attackReady) return;

        PlayerController play = collision.gameObject.GetComponent<PlayerController>();
        if (play != null) {
            StartCoroutine("HandleReady");
            play.TakeDamage(damage, moveDir.normalized * knockback);
        }
    }

    private IEnumerator HandleReady() {
        attackReady = false;
        yield return new WaitForSeconds(attackDelay);
        attackReady = true;
    }
    #endregion
}
