using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAds : MonoBehaviour
{
    

    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/6300978111";
    #elif UNITY_IPHONE
    // actual unit id:
    // private string _adUnitId = "ca-app-pub-1921297679445527/9672515541";
    // app id: ca-app-pub-1921297679445527~3521170629
    private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
    #else
    private string _adUnitId = "unused";
    #endif
    private VisualManager visualManager;

    BannerView _bannerView;
    public void Start()
    {
        if(PlayerPrefs.GetInt("adsRemoved", 0) == 1){
            IronSource.Agent.hideBanner();
            Debug.Log("ads disabled");
        } else {
            MobileAds.Initialize(initStatus => { });
            LoadAd();
            GameObject visualManagerGO = GameObject.FindGameObjectWithTag("Canvas");
            if(visualManagerGO) {
                visualManager = visualManagerGO.GetComponent<VisualManager>();
                visualManager.applyBannerOffset();
            }
        }   
        
    }

    /// <summary>
    /// Creates a 320x50 banner at top of the screen.
    /// </summary>
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        /*
        int[] topBannerPos = AdScreenPlacement.topBannerPos();
        print("Safe area: " + Screen.safeArea.xMin + ", " + Screen.safeArea.yMin + ", " + Screen.safeArea.xMax + ", " + Screen.safeArea.yMax);
        print("topBannerpos: " + topBannerPos[0] + ", " + topBannerPos[1]);
        */
        //_bannerView = new BannerView(_adUnitId, AdSize.Banner, topBannerPos[0],topBannerPos[1]);
       // _bannerView = new BannerView(_adUnitId, AdSize.Banner, 0,0);
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Top);
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    /// <summary>
    /// Creates the banner view and loads a banner ad.
    /// </summary>
    public void LoadAd()
    {
        // create an instance of a banner view first.
        if(_bannerView == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }

}