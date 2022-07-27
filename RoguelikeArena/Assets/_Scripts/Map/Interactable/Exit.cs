using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactable {

    public override void Interact() {
        PlayerController.Instance.InteractionLeave(this);
        LevelManager.Instance.CreateLevel();
    }
}
