using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inheritable class for interactable objects
public class Interactable : MonoBehaviour {
    
    public virtual void Interact() {
        Debug.LogError("Interactable Interact needs to be overwritten!");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        print("perish");
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.InteractionEnter(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.InteractionLeave(this);
        }
    }
}
