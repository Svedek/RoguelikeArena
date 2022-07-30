using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item {

    [SerializeField] private int potency;

    public override void ApplyItem() {
        base.ApplyItem();
        PlayerController.Instance.UsePotion(potency);
    }
}
