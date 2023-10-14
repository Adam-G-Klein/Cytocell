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

    public static string ALL_SKINS_KEY = "UnlockAllSkins";
    private IStoreController StoreController;
    private IExtensionProvider ExtensionProvider;

    private async void Awake()
    {
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPCatalogLoaded;
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
        Debug.Log($"Successfully Initialized Unity IAP. Store Controller has {controller.products.all.Length} products");
    }


    public void RestorePurchase() // Use a button to restore purchase only in iOS device.
    {
#if UNITY_IOS
        ExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions((result, str) => {
            Debug.Log($"Restore purchase result: {result}, string: {str}");
        });
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error initializing IAP because of {error}." +
            $"\r\nShow a message to the player depending on the error.");
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {failureReason}");
    }

    public void PurchaseAllSkinsUnlock(){
        Product product = StoreController.products.all.FirstOrDefault(p => p.definition.id == ALL_SKINS_KEY);
        Debug.Log("All skins product: " + product.ToString());
        StoreController.InitiatePurchase(product);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription desc)
    {
        Debug.Log($"Failed to purchase {product.definition.id} because {desc}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}");

        PlayerPrefs.SetInt(AdsManager.ADS_DISABLED_KEY, 1);
        return PurchaseProcessingResult.Complete;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Error initializing IAP because of {error}." +
            " message: " + message);
    }
}