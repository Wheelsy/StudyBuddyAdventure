using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPrice : MonoBehaviour
{
    [SerializeField]
    private int price;

    public int Price { get => price; set => price = value; }
}
