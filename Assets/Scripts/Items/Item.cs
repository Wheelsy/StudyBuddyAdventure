using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Item")]
public class Item : ScriptableObject
{
    public bool isSet;
    public new string name;
    public Sprite itemArt;
    [Header("Set Attributes")]
    public int atk;
    public int def;
    public int initiative;
    public int durability;
    public Sprite[] skin;
    public enum Potion
    {
        NotPotion,
        OneUp,
        DoubleAtk,
        DoubleDef,
        DoubleInit
    }
    [Header("Potion Attributes")]
    public Potion potion;
    public string potionEffect;
}
