using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Item {

    [SerializeField] ItemManager.WeaponID id;
    [SerializeField] GameObject[] children;

    public override void ApplyItem() {
        base.ApplyItem();
        ItemManager.Instance.GivePlayerWeapon(id);
    }
}