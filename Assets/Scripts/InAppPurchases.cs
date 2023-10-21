using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

public class InAppPurchases : MonoBehaviour, IDetailedStoreListener
{

    public const string ALL_SKINS_KEY = "UnlockAllSkins";
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;
    private Action currentPurchaseCallback;

    private async void Awake()
    {
        try {
            InitializationOptions options = new InitializationOptions()
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
                        .SetEnvironmentName("test");
            #else
                        .SetEnvironmentName("production");
            #endif
            await UnityServices.InitializeAsync(options);
            ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
            operation.completed += HandleIAPCatalogLoaded;
        } catch (Exception e) {
            Debug.LogError("Error initializing Unity Services: " + e.ToString());
        }
        
    }

    private void HandleIAPCatalogLoaded(AsyncOperation Operation)
    {
        ResourceRequest request = Operation as ResourceRequest;

        Debug.Log($"Loaded Asset: {request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.GooglePlay)
        );
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.AppleAppStore)
        );
#else
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(
            StandardPurchasingModule.Instance(AppStore.NotSpecified)
        );
#endif
        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }

        Debug.Log($"Initializing Unity IAP with {builder.products.Count} products");
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        StoreController = controller;
        ExtensionProvider = extensions;
        Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {controller.products.all.Length} products");
    }


    public void RestorePurchase() // Use a button to restore purchase only in iOS device.
    {
#if UNITY_IOS
        print("ExtensionProvider null? " + (ExtensionProvider == null));
        print("AppleExtensions null? " + (ExtensionProvider.GetExtension<IAppleExtensions>() == null));
        ExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, str) => {
            Debug.Log($"Restore purchase result: {result}, string: {str}");
            PurchaseManager.instance.purchasesRestored();
        });
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error initializing IAP because of {error}." +
            $"\r\nShow a message to the player depending on the error.");
    }

    public void PurchaseProduct(string key, Action callback = null){
        Product product = StoreController.products.all.FirstOrDefault(p => p.definition.id == key);
        Debug.Log("purchasing product: " + product.ToString());
        currentPurchaseCallback = callback;
        StoreController.InitiatePurchase(product);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription desc)
    {
        PurchaseManager.instance.purchaseFailed();
        Debug.Log($"Failed to purchase {product.definition.id} because {desc}");
    }

    //need both for interface implementation 
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        PurchaseManager.instance.purchaseFailed();
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");
        PurchaseManager.instance.purchaseSucceeded(purchaseEvent.purchasedProduct.definition.id);
        // TODO: convert to an event
        processCallback();
        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP because of {error}." +
            " message: " + message);
    }

    private void processCallback(){
        print("attempting to process callback, currentPurchaseCallback null? " + (currentPurchaseCallback == null) + "");
        if(currentPurchaseCallback != null) {
            currentPurchaseCallback();
            currentPurchaseCallback = null;
        }
    }
}