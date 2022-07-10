using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inheritable class for interactable objects
public class Interactable : MonoBehaviour {
    
    public void Interact() {
        Debug.LogError("Interactable Interact needs to be overwritten!");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.Interact
        }
    }
}
