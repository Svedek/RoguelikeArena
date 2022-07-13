using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : Interactable {

    public override void Interact() {
        // PlayerController.Instance.transform.position = new Vector3(-20, -20, 0);
        PlayerController.Instance.InteractionLeave(this);
        LevelManager.Instance.CreateLevel();
    }
}
