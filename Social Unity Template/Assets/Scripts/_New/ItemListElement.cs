using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListElement : MonoBehaviour
{
    public Item item { get; set; }

    public int count {  get; set; }

    public ItemListElement(Item item) {
        this.item = item;
        count = 1;
    }
}
