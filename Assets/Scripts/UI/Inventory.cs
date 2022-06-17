using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public GameObject invFullTxt;
    public GameObject loadout;

    [SerializeField]
    private Image[] slots;
    [SerializeField]
    private ItemManager im;
    [SerializeField]
    private PetManager pm;
    [SerializeField]
    private Buddy buddy;
    [SerializeField]
    private Sprite emptySlot;
    private List<string> invItemsKey = new List<string>();
    private List<int> invItemsValue = new List<int>();
    private CharacterPanel cp;

    public GameObject invFull;
    public List<int> InvItemsValue { get => invItemsValue; set => invItemsValue = value; }
    public List<string> InvItemsKey { get => invItemsKey; set => invItemsKey = value; }

    private void Start()
    {
        cp = gameObject.GetComponent<CharacterPanel>();
    }

    //Search for an empty slot or a slot with the same item in it
    //return the image or null if inventory full
    public Image FindEmptySlot(Sprite sprite)
    {
        Item item = im.GetItemBySpriteMatch(sprite);
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].sprite == sprite && !item.isSet)
            {
                return slots[i];
            }
            else if(slots[i].sprite == emptySlot)
            {
                return slots[i];
            }
        }
        return null;
    }

    private void AdjustSlotQuantity(string itemName, int amount)
    {
        if (invItemsKey.Contains(itemName))
        {
            Debug.Log("adding 1 to " + itemName);
            invItemsValue[invItemsKey.IndexOf(itemName)] += amount;
        }
        GameMaster.SaveData();
    }

    //Button click event for item on quest outcome
    //Find empty slot and update the sprite
    //Turn off the button after click
    public void AddToInventory(GameObject item)
    {
        Image slot = FindEmptySlot(item.GetComponent<Image>().sprite);
        if (slot != null)
        {
            item.transform.GetChild(0).gameObject.SetActive(false);       
            invFullTxt.SetActive(false);
            Item i = im.GetItemBySpriteMatch(item.GetComponent<Image>().sprite); 
            TextMeshProUGUI existingQuantity = slot.GetComponentInChildren<TextMeshProUGUI>();
            //if its an empty slot
            if (slot.sprite == emptySlot)
            {            
                slot.sprite = i.itemArt;
                existingQuantity.text = "1";
                invItemsKey.Add(i.name);
                invItemsValue.Add(1);
            }
            //else its adding quantity to an existing item
            else
            {
                existingQuantity.text = (int.Parse(existingQuantity.text) + 1).ToString();
                AdjustSlotQuantity(i.name, 1);
            }
        }
        else
        {
            loadout.SetActive(true);
            invFullTxt.SetActive(true);
        }
        GameMaster.SaveData();
    }

    //Swap an equipped item back to the inventory
    private void SwapFromCp(Sprite sprite)
    {
        Image slot = FindEmptySlot(sprite);
        if (slot != null)
        {
            Item i = im.GetItemBySpriteMatch(sprite);
            TextMeshProUGUI existingQuantity = slot.GetComponentInChildren<TextMeshProUGUI>();
            //if its an empty slot
            if (slot.sprite == emptySlot)
            {
                slot.sprite = i.itemArt;
                existingQuantity.text = "1";
                invItemsKey.Add(i.name);
                invItemsValue.Add(1);
                Debug.Log("setting slot " + slot.name + " to 1");
            }
            //else its adding quantity to an existing item
            else
            {
                existingQuantity.text = (int.Parse(existingQuantity.text) + 1).ToString();
                //AdjustSlotQuantity(i.name, 1);
            }
        }
        else
        {
            Debug.Log("inventory full");
            invFull.SetActive(true);
        }
        GameMaster.SaveData();
    }

    //Retrieve the scriptable obj by sprite match
    //Move the sprite from inventory to character panel if there is only 1 of the item in the inventory
    //If there is more than 1 just reduce its quantity
    //Update all of the stats in character panel by adding the values form the scriptable obj
    public void AttachInventoryItem(Image img)
    {
        Sprite setSprite = cp.SetSlot.sprite;
        Sprite potionSprite = cp.PotionSlot.sprite;

        if (img.sprite != null || img.sprite != setSprite || img.sprite != potionSprite) //check its not already in cp panel
        {
            Item itemToCp = im.GetItemBySpriteMatch(img.sprite);

            //If the set has a pet turn it on
            //If theres already a pet in the scene turn it off
            if (itemToCp.hasPet)
            {
                if(pm.CurActivePet != 99)
                {
                    pm.TurnOffPet();
                }
                pm.TurnOnPet(itemToCp.petIndex);
            }
            else if(GameObject.FindGameObjectWithTag("Pet") != null)
            {
                pm.TurnOffPet();
            }

            if (itemToCp != null)
            {
                //If the item is a set then reasign image to set slot
                if (itemToCp.isSet)
                {                 
                    cp.UpdateSetImage(img.sprite);
                    cp.UpdateStats(img.sprite);
                    cp.UpdateCharacterImage(itemToCp.skin[0]);
                    buddy.UpdateCurrentSkin(itemToCp.skin[0], itemToCp.skin[1]);
                }
                //If the item is a potion then reasign image to set slot
                else
                {
                    cp.UpdatePotionImage(img.sprite);
                }

                //Adjust the inventory to reflect the now missing item
                TextMeshProUGUI invQuantity = img.GetComponentInChildren<TextMeshProUGUI>();
                if (int.Parse(invQuantity.text) > 1)
                {
                    invQuantity.text = (int.Parse(invQuantity.text) - 1).ToString();
                }
                else if (int.Parse(invQuantity.text) == 1)
                {
                    RemoveItemFromInventory(img);
                    invQuantity.text = "0";
                    img.sprite = emptySlot;
                }

                if (itemToCp.isSet)
                {
                    //If theres already a set equipped swap it out.
                    if (!setSprite.name.Equals("empty"))
                    {
                        SwapFromCp(setSprite);
                    }
                }
                else
                {
                    //If theres already a potion equipped swap it out.
                    if (!potionSprite.name.Equals("empty"))
                    {
                        Debug.Log("swapping potion back to inventory");
                        SwapFromCp(potionSprite);
                    }
                }
            }
        }
        GameMaster.SaveData();
    }

    //Revome item from the dictionary and update the ui
    public void RemoveItemFromInventory(Image img)
    {
        Item item = im.GetItemBySpriteMatch(img.sprite);
        TextMeshProUGUI invQuantity = img.GetComponentInChildren<TextMeshProUGUI>();

        if (int.Parse(invQuantity.text) == 1)
        {
            invQuantity.text = "0";
            img.sprite = emptySlot;
            invItemsKey.Remove(item.name);
            invItemsValue.Remove(invItemsKey.IndexOf(item.name));
        }
        else if (int.Parse(invQuantity.text) > 1)
        {
            Debug.Log("subtracting 1 from " + invQuantity.text);
            invQuantity.text = (int.Parse(invQuantity.text) - 1).ToString();
            invItemsValue[invItemsKey.IndexOf(item.name)] -= 1;
            Debug.Log("to give " + invItemsValue[invItemsKey.IndexOf(item.name)]);
        }

        GameMaster.SaveData();
    }
}
