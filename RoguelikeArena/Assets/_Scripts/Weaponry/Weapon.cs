using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent Class to all weapons
public class Weapon : MonoBehaviour {

    [SerializeField] protected Transform firepoint;

    [field: SerializeField] public float attackSpeed { get; private set; }
    [SerializeField] protected float spread;

    public virtual void Shoot() {
        Debug.LogError("Weapon Shoot must be overriden");
    }
}
