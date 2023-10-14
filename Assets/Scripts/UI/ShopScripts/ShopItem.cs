using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
Each display needs: 
- a demo player
- a price display
- a buy button
- a background
*/

/* This script: 
- Adds all of its components to the parent's ButtonGroupAlpaControls
- Handles equipping and unequipping skins via the purchaseManager
- Changes the "Buy, Use" button text based on whether the skin is equipped or not
- Hides and reveals the price of an item based on whether its been purchased
*/
public class ShopItem : MonoBehaviour
{
    public PlayerSkinSO playerSkinSO;
    public GameObject shopPlayer;
    public TextMeshProUGUI buyText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    private ShopManager shopManager;
    public float NotEnoughCurrencyFontSize = 0.9f;
    // Start is called before the first frame update
    void Awake() {
        shopPlayer.GetComponent<Image>().material = playerSkinSO.shopMaterial;
        shopManager = GetComponentInParent<ShopManager>();

        ButtonGroupAlphaControls parentMenu = GetComponentInParent<ButtonGroupAlphaControls>();
        foreach (Image image in GetComponentsInChildren<Image>()) {
            parentMenu.gameObjects.Add(image.gameObject);
        }

    }

    void OnEnable()
    {
        StartCoroutine(lateOnEnable());
    }

    IEnumerator lateOnEnable() {
        yield return new WaitForEndOfFrame();

        nameText.text = playerSkinSO.name;

        equippedUpdate();
        
    }

    void Update() {
        equippedUpdate();
    }


    public void buyClick(){
        print("buy click " + playerSkinSO.name);
        if(PurchaseManager.instance.isSkinUnlocked(playerSkinSO)) {
            shopManager.setEquippedSkin(playerSkinSO);
        }
        else if(canAfford()) {
            shopManager.purchaseSkin(playerSkinSO);
        }
    }

    private bool canAfford() {
        return PurchaseManager.instance.getCurrency() >= playerSkinSO.price;
    }

    public void equippedUpdate() {
        if(PurchaseManager.instance.getEquippedSkin() == playerSkinSO) {
            buyText.text = "In Use";
            priceText.text = "";
        }
        else if(PurchaseManager.instance.isSkinUnlocked(playerSkinSO)) {
            buyText.text = "Use";
            priceText.text = "";
        } else if(canAfford()) {
            priceText.text = "¤" + playerSkinSO.price.ToString();
            buyText.text = "Buy";
        } else {
            priceText.text = "¤" + playerSkinSO.price.ToString();
            buyText.text = "Need more ¤";
            buyText.fontSize = NotEnoughCurrencyFontSize;
        }


        
    }

}
