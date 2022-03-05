using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemStats : MonoBehaviour
{
    public ItemManager im;
    public GameObject statWindow;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI def;
    public TextMeshProUGUI init;
    public TextMeshProUGUI dur;
    public void OnItemHover(Image img)
    {
        if (img.sprite.name.Equals("empty")) return;
        statWindow.SetActive(true);
        Item item = im.GetItemBySpriteMatch(img.sprite);
        itemName.text = item.name;
        atk.text = item.atk.ToString();
        def.text = item.def.ToString();
        init.text = item.initiative.ToString();
        dur.text = item.durability.ToString();
    }

    public void OnItemRelease()
    {
        statWindow.SetActive(false);
    }
}
