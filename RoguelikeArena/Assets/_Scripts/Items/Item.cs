using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    [Header("Info")]
    [SerializeField] protected string titleText;
    public string TitleText { get { return titleText; } }
    [SerializeField] protected string descText;
    public string DescText { get { return descText; } }
    [SerializeField] protected int cost;
    public int Cost { get { return cost; } }

    public virtual void ApplyItem() {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}


