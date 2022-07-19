using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Health {

    // Base stats
    [SerializeField] protected int baseGold;
    [SerializeField] protected float baseExp;

    protected override void Awake() {
        base.Awake();
    }

    protected override void Die() {
        EnemyManager.Instance.EnemyDied(this);
        PlayerController.Instance.GainExperience(baseExp);
        PlayerController.Instance.GainGold(baseGold);
        Destroy(gameObject);
    }
}
