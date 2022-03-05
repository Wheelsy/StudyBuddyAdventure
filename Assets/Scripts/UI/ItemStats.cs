using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemStats : MonoBehaviour
{
    public ItemManager im;
    public GameObject setStatWindow;
    public GameObject potionStatWindow;
    public TextMeshProUGUI setName;
    public TextMeshProUGUI potionName;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI def;
    public TextMeshProUGUI init;
    public TextMeshProUGUI dur;
    public TextMeshProUGUI effect;

    public void ShowPotionOrImage(Image img)
    {
        if (img.sprite.name.Equals("empty")) return;
        Item item = im.GetItemBySpriteMatch(img.sprite);

        if (item.potion.ToString().Equals("NotPotion"))
        {
            SetHover(item);
        }
        else
        {
            PotionHover(item);
        }
    }

    private void SetHover(Item item)
    {      
        setStatWindow.SetActive(true);      
        setName.text = item.name;
        atk.text = item.atk.ToString();
        def.text = item.def.ToString();
        init.text = item.initiative.ToString();
        dur.text = item.durability.ToString();
    }

    private void PotionHover(Item item)
    {
        potionStatWindow.SetActive(true);
        potionName.text = item.name;
        effect.text = item.potionEffect;
    }

    public void OnItemRelease()
    {
        setStatWindow.SetActive(false);
        potionStatWindow.SetActive(false);
    }
}
