using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public List<ShopItem> shopItems = new List<ShopItem>();
    public TextMeshProUGUI currencyText;
    public int testCurrency = -1;

    void Start() {
        shopItems = new List<ShopItem>(GetComponentsInChildren<ShopItem>());
        setCurrencyText(PurchaseManager.instance.getCurrency());
    }

    void Update() {
        if(testCurrency != -1) {
            PurchaseManager.instance.setCurrency(testCurrency);
            setCurrencyText(PurchaseManager.instance.getCurrency());
        }
    }

    public void setEquippedSkin(PlayerSkinSO skin)
    {
        PurchaseManager.instance.setEquippedSkin(skin);
        foreach (ShopItem item in shopItems)
        {
            item.equippedUpdate();
        }
    }

    private void setCurrencyText(int val) {
        currencyText.text = "Current ¤:\n" + val.ToString();
    }

    public void purchaseSkin(PlayerSkinSO skin)
    {
        PurchaseManager.instance.unlockSkin(skin);
        PurchaseManager.instance.setCurrency(PurchaseManager.instance.getCurrency() - skin.price);
        setCurrencyText(PurchaseManager.instance.getCurrency());
    }
}
