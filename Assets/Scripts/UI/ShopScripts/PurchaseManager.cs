using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;


public class PurchaseManager : MonoBehaviour
{
    [SerializeField]
    private int currency;
    [SerializeField]
    private PlayerSkinSO equippedSkin;
    public static string UNLOCKED_SKINS = "unlockedSkins";
    public static string ALL_SKINS_UNLOCKED = "ALL_SKINS_UNLOCKED";
    public static string EQUIPPED_SKIN = "equippedSkin";
    public static string CURRENCY = "currency";
    public static string DEFAULT_SKIN = "Alpha";
    public static PurchaseManager instance;
    private InAppPurchases inAppPurchases;

    private GameObject processingMenuGO;
    private ProcessingPopupController processingMenuController;

    public List<PlayerSkinSO> allSkins;
    private List<ShopItem> allShopItems = new List<ShopItem>();
    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        inAppPurchases = GetComponent<InAppPurchases>();
        processingMenuGO = GameObject.FindGameObjectWithTag("ProcessingMenu");
        if(processingMenuGO != null)
            processingMenuController = processingMenuGO.GetComponent<ProcessingPopupController>();

        equippedSkin = getEquippedSkin();

        GameObject[] items = GameObject.FindGameObjectsWithTag("ShopItem");
        // in lieu of a map function
        foreach(GameObject go in items) {
            allShopItems.Add(go.GetComponent<ShopItem>());
        }
        
    }

    public void updateShopItems(){
        foreach(ShopItem item in allShopItems) {
            print("broadcasting equippedUpdate to " + item.name);
            item.BroadcastMessage("equippedUpdate");
        }
    }

    // TODO, put it on an event bus
    // Currently called by BOTH our custom InAppPurchases implementation AND the Unity IAP system
    public void purchaseSucceeded(Product product){
        string productId = product.definition.id;
        print("purchase of product " + productId + " succeeded, unlockSkinController null? " + processingMenuController == null);
        switch (productId)
        {
            case InAppPurchases.ALL_SKINS_KEY:
                unlockAllSkins();
                hideProcessingMenu();
                break;
            default:
                Debug.LogError("product key not found: " + productId);
                break;
        }
        updateShopItems();

    }
    
    private void hideProcessingMenu() {
        if(processingMenuController != null && processingMenuController.processingMenu.isActive) {
            print("hiding processing menu");
            processingMenuController.hideProcessingMenu();
        }

    }

    public void purchaseFailed(Product product, PurchaseFailureReason failureReason) {
        hideProcessingMenu();
        updateShopItems();
    }

    public void purchasesRestored(ProductCollection products) {
        hideProcessingMenu();
        updateShopItems();
    }

    public List<string> getUnlockedSkins()
    {
        return new List<string>(PlayerPrefs.GetString(UNLOCKED_SKINS, DEFAULT_SKIN).Split(','));
    }

    public void unlockSkin(PlayerSkinSO skin)
    {
        List<string> unlockedSkins = getUnlockedSkins();
        unlockedSkins.Add(skin.name);
        PlayerPrefs.SetString(UNLOCKED_SKINS, string.Join(",", unlockedSkins.ToArray()));
    }

    public bool allSkinsUnlocked() {
        return PlayerPrefs.GetInt(ALL_SKINS_UNLOCKED, 0) == 1;
    }

    public void setEquippedSkin(string skinName){
        PlayerPrefs.SetString(EQUIPPED_SKIN, skinName);
    }

    public PlayerSkinSO getEquippedSkin(){
        string equippedSkinName = PlayerPrefs.GetString(EQUIPPED_SKIN, DEFAULT_SKIN);
        foreach(PlayerSkinSO skin in allSkins) {
            print("Checking skin " + skin.name);
            if(skin.name == equippedSkinName) {
                return skin;
            }
        }
        Debug.LogWarning("equipped skin not found, returning default skin");
        return allSkins[0];
    }



    public bool isSkinUnlocked(PlayerSkinSO skin)
    {
        List<string> unlockedSkins = getUnlockedSkins();
        return unlockedSkins.Contains(skin.name) || allSkinsUnlocked();
    }

    public int getCurrency() {
        return PlayerPrefs.GetInt(CURRENCY, 0);
    }

    public void setCurrency(int newCurrency) {
        PlayerPrefs.SetInt(CURRENCY, newCurrency);
    }

    public void incrementCurrency(int amount) {
        PlayerPrefs.SetInt(CURRENCY, getCurrency() + amount);
    }

    public void purchaseProduct(string productId, Action callback) {
        print("purchase product: " + productId + " callback null? " + (callback == null));
        inAppPurchases.PurchaseProduct(productId, callback);
    }

    public void iapRestorePurchases() {
        inAppPurchases.RestorePurchase();
    }

    private void unlockAllSkins() {
        PlayerPrefs.SetInt(ALL_SKINS_UNLOCKED, 1);
    }



}
