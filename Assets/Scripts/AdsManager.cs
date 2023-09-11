using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    
    public static int STARTING_INTERSTITIAL_FREE_PLAYS = 2;
    public static float INTERSTITIAL_DISPLAY_DELAY = 1.5f;
    public static string ADS_DISABLED_KEY = "adsDisabled";
    public static string INTERSTITIAL_FREE_PLAYS_KEY = "InterstitialFreePlays";


    // These ad units are configured to always serve test ads.
    #if UNITY_ANDROID
    private string _adUnitIdBanner = "ca-app-pub-3940256099942544/6300978111";
    #elif UNITY_IPHONE
    // actual prod unit id:
    private string _adUnitIdBanner = "ca-app-pub-1921297679445527/9672515541";
    // app id: ca-app-pub-1921297679445527~3521170629
    // test ios unit id:
    //private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
    #else
    private string _adUnitIdBanner = "unused";
    #endif
    private VisualManager visualManager;

    private List<string> testDeviceIds = new List<string> {
        "3dcff921fafc1646ab8791629574225e"
    };

    BannerView _bannerView;
    public void Start()
    {
        if(PlayerPrefs.GetInt(AdsManager.ADS_DISABLED_KEY, 0) == 1){
            Debug.Log("ads disabled");
        } else {
            RequestConfiguration requestConfiguration = new RequestConfiguration();
            requestConfiguration.TestDeviceIds.AddRange(testDeviceIds);
            MobileAds.SetRequestConfiguration(requestConfiguration);
            MobileAds.Initialize(initStatus => { });
            LoadBanner();
            GameObject visualManagerGO = GameObject.FindGameObjectWithTag("Canvas");
            if(visualManagerGO) {
                visualManager = visualManagerGO.GetComponent<VisualManager>();
                visualManager.applyBannerOffset();
            }
            LoadInterstitialAd();
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
            DestroyBanner();
        }

        // Create a 320x50 banner at top of the screen
        /*
        int[] topBannerPos = AdScreenPlacement.topBannerPos();
        print("Safe area: " + Screen.safeArea.xMin + ", " + Screen.safeArea.yMin + ", " + Screen.safeArea.xMax + ", " + Screen.safeArea.yMax);
        print("topBannerpos: " + topBannerPos[0] + ", " + topBannerPos[1]);
        */
        //_bannerView = new BannerView(_adUnitId, AdSize.Banner, topBannerPos[0],topBannerPos[1]);
       // _bannerView = new BannerView(_adUnitId, AdSize.Banner, 0,0);
        _bannerView = new BannerView(_adUnitIdBanner, AdSize.Banner, AdPosition.Top);
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary
    public void DestroyBanner()
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
    public void LoadBanner()
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

    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
  private string _adUnitIdInterstitial = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
    // test unit id:
  //private string _adUnitIdInterstitial = "ca-app-pub-3940256099942544/4411468910";
  // prod unit id:
  private string _adUnitIdInterstitial = "ca-app-pub-1921297679445527/4777930444";
#else
  private string _adUnitIdInterstitial = "unused";
#endif

  private InterstitialAd interstitialAd;

  /// <summary>
  /// Loads the interstitial ad.
  /// </summary>
  public void LoadInterstitialAd()
  {

      //DestroyBanner();
      // Clean up the old ad before loading a new one.
      DestroyInterstitial();

      Debug.Log("Loading the interstitial ad.");

      // create our request used to load the ad.
      var adRequest = new AdRequest();
      adRequest.Keywords.Add("unity-admob-sample");

      // send the request to load the ad.
      InterstitialAd.Load(_adUnitIdInterstitial, adRequest,
          (InterstitialAd ad, LoadAdError error) =>
          {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
              {
                  Debug.LogError("interstitial ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo().ToString());

              interstitialAd = ad;
          });
  }

  private void DestroyInterstitial()
  {
    print("destroying interstitial");
    if (interstitialAd != null)
    {
        interstitialAd.Destroy();
        interstitialAd = null;
    }
  }

  public void destroyAds()
  {
      DestroyBanner();
      DestroyInterstitial();
  }

  public IEnumerator playerKilledCorout()
  {
    if(PlayerPrefs.GetInt(ADS_DISABLED_KEY, 0) == 1) yield break;
    if(PlayerPrefs.GetInt(INTERSTITIAL_FREE_PLAYS_KEY, STARTING_INTERSTITIAL_FREE_PLAYS) > 0) {
        PlayerPrefs.SetInt(INTERSTITIAL_FREE_PLAYS_KEY, PlayerPrefs.GetInt(INTERSTITIAL_FREE_PLAYS_KEY, STARTING_INTERSTITIAL_FREE_PLAYS) - 1); 
        yield break;
    }
    yield return StartCoroutine("showInterstitialCorout");
  }

  public IEnumerator showInterstitialCorout()
  {
    float time = Time.time;
    yield return new WaitUntil(() => interstitialAd.CanShowAd());
    float timeToWait = INTERSTITIAL_DISPLAY_DELAY - (Time.time - time);
    if (timeToWait > 0) yield return new WaitForSeconds(timeToWait);
    print("showing interstitial");
    interstitialAd.Show();
    yield return null;
  }
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                        "with error : " + error);
        };
    }

}