using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PurchaseManager : MonoBehaviour
{
    [SerializeField]
    private int currency;
    [SerializeField]
    private PlayerSkinSO equippedSkin;
    public static string UNLOCKED_SKINS = "unlockedSkins";
    public static string ALL_SKINS_UNLOCKED = "ALL_SKINS_UNLOCKED";
    public static string CURRENCY = "currency";
    public static string DEFAUKT_SKIN = "Alpha";
    public static PurchaseManager instance;
    private InAppPurchases inAppPurchases;

    private GameObject processingMenuGO;
    private ProcessingPopupController processingMenuController;
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
    }

    // TODO, put it on an event bus
    public void purchaseSucceeded(string productId){
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

    }
    
    private void hideProcessingMenu() {
        if(processingMenuController != null) {
            print("hiding processing menu");
            processingMenuController.hideProcessingMenu();
        }

    }

    public void purchaseFailed() {
        hideProcessingMenu();
    }

    public void purchasesRestored() {
        hideProcessingMenu();
    }

    public List<string> getUnlockedSkins()
    {
        return new List<string>(PlayerPrefs.GetString(UNLOCKED_SKINS, DEFAUKT_SKIN).Split(','));
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

    public PlayerSkinSO getEquippedSkin() {
        return equippedSkin;
    }

    public void setEquippedSkin(PlayerSkinSO newSkin) {
        equippedSkin = newSkin;
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
