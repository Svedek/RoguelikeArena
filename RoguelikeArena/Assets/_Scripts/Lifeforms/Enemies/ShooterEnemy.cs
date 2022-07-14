using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy {

    void Update() {
        CalculateMove();
    }
    void FixedUpdate() {
        Move();
    }

    #region Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float preferedDistance;
    [SerializeField] private float preferedDistanceTolerance;
    [SerializeField] private float preferedDistanceSlowDistance; // Greater than preferedDistanceTolerance
    [SerializeField] private float sideMovementWeight; // TODO multiplied by -1 to switch direction (on colision)
    private Vector2 moveDir;
    private void CalculateMove() {
        // Calculate vector in line with player
        Vector2 playerVector = (PlayerController.Instance.transform.position - transform.position);
        var normPV = playerVector.normalized; // nessesary for 
        var pvMag = playerVector.magnitude;
        var dist = pvMag - preferedDistance;

        if (Mathf.Abs(dist) <= preferedDistanceTolerance) {
            playerVector = Vector2.zero;
        } else {
            playerVector *= dist; // Potentially changes the direction of playerVector
            playerVector.Normalize();
            if (Mathf.Abs(dist) < preferedDistanceSlowDistance) {
                playerVector *= Mathf.Abs(dist) / preferedDistanceSlowDistance;
            }
        }

        // Calculate vector perpendicular to previous vector
        Vector2 sideVector = new Vector2(-normPV.y, normPV.x); // points clockwise, perpendicular to playerVector
        sideVector *= sideMovementWeight;


        // Combine vectors
        moveDir = playerVector + sideVector;
        moveDir.Normalize();
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
    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController play = collision.gameObject.GetComponent<PlayerController>();
        if (play != null) {
            play.TakeDamage(damage, moveDir.normalized * knockback);
        }
    }
    #endregion
}
