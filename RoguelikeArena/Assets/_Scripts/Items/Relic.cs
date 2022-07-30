using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : Item {

    [SerializeField] private RelicID relicID;

    public enum RelicID {
        hand,
        tumbleShield,
        magicMap,
    }

    public override void ApplyItem() {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
