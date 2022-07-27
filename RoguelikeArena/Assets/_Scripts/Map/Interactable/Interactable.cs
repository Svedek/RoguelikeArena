using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inheritable class for interactable objects
public class Interactable : MonoBehaviour {
    
    public virtual void Interact() {
        Debug.LogError("Interactable Interact needs to be overwritten!");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Enter(collision);
    }
    private void OnTriggerExit2D(Collider2D collision) {
        Exit(collision);
    }

    protected virtual void Enter(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.InteractionEnter(this);
        }
    }

    protected virtual void Exit(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.InteractionLeave(this);
        }
    }
}
