using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopStall : Interactable {

    private Item item;
    [SerializeField] Text title, desc, price;

    public void Initialize(GameObject itemPrefab) {
        // Spawn Item
        GameObject itemObject = Instantiate(itemPrefab, transform);
        itemObject.transform.position = transform.position;
        item = itemObject.GetComponent<Item>();

        // Setup Text
        title.text = item.TitleText;
        desc.text = item.DescText;
        price.text = item.Cost + "";
    }
    public void Initialize(GameObject itemPrefab, string title, string desc, int cost) {
        // Spawn Item
        GameObject itemObject = Instantiate(itemPrefab, transform);
        itemObject.transform.position = transform.position;
        item = itemObject.GetComponent<Item>();

        // Setup Text
        this.title.text = title;
        this.desc.text = desc;
        this.price.text = cost + "";
    }

    public override void Interact() {
        if (PlayerController.Instance.TryPurchace(item.Cost)) {
            item.ApplyItem();

            // Remove stall
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            PlayerController.Instance.InteractionLeave(this);
        }
    }

    public void ClearStall() {
        // Destroy item
        if (item != null) {
            Destroy(item.gameObject);
        }

        // Disable Stall
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        PlayerController.Instance.InteractionLeave(this);
        title.enabled = false;
        desc.enabled = false;
        price.enabled = false;
    }

    protected override void Enter(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.InteractionEnter(this);
        }

        title.enabled = true;
        desc.enabled = true;
    }

    protected override void Exit(Collider2D collision) {
        if (collision.GetComponent<PlayerController>() != null) {
            PlayerController.Instance.InteractionLeave(this);
        }

        title.enabled = false;
        desc.enabled = false;
    }
}
