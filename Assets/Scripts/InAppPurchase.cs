using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class InAppPurchase : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string kProductIDConsumable = "consumable";
    public static string kProductIDNonConsumable = "nonconsumable";
    public static string kProductIDSubscription = "subscription";
    /// <summary>
    /// //Capital letters are not used in id's//////////////
    /// </summary>
    //Inapp Ids
    public static string remove_ads = "removeads";
    public static string all_buses = "allcars";
    public static string unlock_everything = "unlockeverything";
    public static string unlock_levels = "unlocklevels";




    public static bool Remove_specialOffer_btn;

    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";


    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    public static InAppPurchase instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        try
        {
            if (m_StoreController == null)
            {
                // Start1();
                InitializePurchasing();
            }
            Remove_specialOffer_btn = false;
        }
        catch
        {

        }

    }
    public string environment = "production";

    async void Start1()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName(environment);

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during services initialization.
        }
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }
    public void InitializePurchasing()
    {
        //try{

        if (IsInitialized())
        {

            return;
        }


        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
        builder.AddProduct(kProductIDNonConsumable, ProductType.NonConsumable);

        /////////////// Inapp initializer

        builder.AddProduct(remove_ads, ProductType.NonConsumable);
        builder.AddProduct(unlock_everything, ProductType.NonConsumable);
        builder.AddProduct(all_buses, ProductType.NonConsumable);
        builder.AddProduct(unlock_levels, ProductType.NonConsumable);
        builder.AddProduct(kProductIDSubscription, ProductType.Subscription, new IDs(){
                { kProductNameAppleSubscription, AppleAppStore.Name },
                { kProductNameGooglePlaySubscription, GooglePlay.Name },
            });

        UnityPurchasing.Initialize(this, builder);
        //}
        //catch{

        //}

    }


    private bool IsInitialized()
    {
        try
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }
        catch
        {
            return false;
        }
    }




    //Call inapp methods
    public void Remove_Ads()
    {
        try
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                BuyProductID(remove_ads);
            }
        }
        catch
        {

        }

    }

    public void BuyEverything()
    {
        try
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                BuyProductID(unlock_everything);
            }
        }
        catch
        {

        }

    }
    public void BuyAllBuses()
    {
        try
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                BuyProductID(all_buses);
            }
        }
        catch
        {

        }

    }

    public void BuyAllLevels()
    {
        try
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                BuyProductID(unlock_levels);
            }
        }
        catch
        {

        }

    }



    void BuyProductID(string productId)
    {

        if (IsInitialized())
        {

            Product product = m_StoreController.products.WithID(productId);


            if (product != null && product.availableToPurchase)
            {
                //Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

                m_StoreController.InitiatePurchase(product);
            }

            else
            {

                //Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }

        else
        {

            //Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }



    public void RestorePurchases()
    {

        if (!IsInitialized())
        {

            //Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }


        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {

            //Debug.Log("RestorePurchases started ...");


            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {

                //Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }

        else
        {

            //Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {

        //Debug.Log("OnInitialized: PASS");

        m_StoreController = controller;

        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {

        //Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        if (String.Equals(args.purchasedProduct.definition.id, kProductIDConsumable, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        }
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

        /////////////////////  inapp succecced conditionsw

        if (String.Equals(args.purchasedProduct.definition.id, remove_ads, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("RemoveAds", 1);
            MainMenu.instance.RemoveAdsButton.SetActive(false);
            if (AdsManager.instance != null)
            {
                AdsManager.instance.RemoveAllBanners();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (string.Equals(args.purchasedProduct.definition.id, unlock_everything, StringComparison.Ordinal))
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("Bus" + i, 1);
            }
            MainMenu.instance.UnlockEverythingButton.SetActive(false);
            GarageScript.instance.ActivateBus();
        }
        else if (string.Equals(args.purchasedProduct.definition.id, all_buses, StringComparison.Ordinal))
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetInt("Bus" + i, 1);
            }
            GarageScript.instance.UnlockAllBuses.SetActive(false);
            GarageScript.instance.ActivateBus();
        }

        else if (string.Equals(args.purchasedProduct.definition.id, unlock_levels, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("unlocklevel", 24);
        }

        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

}

