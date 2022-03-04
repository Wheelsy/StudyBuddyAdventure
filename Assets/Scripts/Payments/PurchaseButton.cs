using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    public enum PurchaseType { gold100, gold500, gold1500};
    public PurchaseType purchaseType;

    public void Purchase()
    {
        switch (purchaseType)
        {
            case PurchaseType.gold100:
                IAPManager.instance.Buy100Gold();
                break;

            case PurchaseType.gold500:
                IAPManager.instance.Buy500Gold();
                break;

            case PurchaseType.gold1500:
                IAPManager.instance.Buy1500Gold();
                break;
        }
    }
}
