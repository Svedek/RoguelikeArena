using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item {

    [SerializeField] private int potency;

    private void Start() {

    }

    public override void ApplyItem() {
        PlayerController.Instance.UsePotion(potency);
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
