using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager instance;
    bool isAdsPurchased;

    [Space(20)]
    [Header("TestMode")]
    public bool isTestMode;

    [Space(20)]
    [Header("Check Internet Connection")]
    public bool isCheckInternetConnection;
    public GameObject CheckInternetPanel;
    bool isAdmobInitialized;

    [Space(20)]
    [Header("Interstitials")]
    public string appOpenId;
    public string interstitialId;
    public string staticInterstitialId;
    AppOpenAd appOpenAd;
    InterstitialAd interstitialAd, staticInterstitialAd;
    public bool isAppOpenAd, isInterstitialAd, isStaticInterstitialAd;
    bool adShowing;

    [Space(20)]
    [Header("Rewarded")]
    public string rewardedId;
    public string rewardedInterstitialId;
    RewardedAd rewardedAd;
    RewardedInterstitialAd rewardedInterstitial;
    public bool isRewardedAd, isRewardedInterstitial;
    string rewardedAdName;

    BannerView bannerViewtopsmallbanner, bannerViewbottomsmallbanner, bannerViewtoprightbanner, bannerViewtopleftbanner, bannerViewbottomleftbanner, bannerViewbottomrightbanner, bannerViewtoprightcubebanner, bannerViewtopleftcubebanner, bannerViewbottomleftcubebanner, bannerViewbottomrightcubebanner, bannerViewtoplargeportraitbanner, bannerViewbottomlargeportraitbanner, bannerViewtoplargelandscapebanner, bannerViewbottomlargelandscapebanner, bannerviewtopadaptive, bannerviewbottomadaptive;
    [Space(20)]
    [Header("Banners")]
    public string bannerId;
    public string medBannerId;
    public string largeBannerId;
    public string adaptiveBannerId;

    public bool isTopBanner, IsBottomBanner, isTopRightBanner, isTopLeftBanner, isBottomRightBanner, isBottomLeftBanner, isTopRightCubeBanner, isTopLeftCubeBanner, isBottomRightCubeBanner, isBottomLeftCubeBanner, isLargeLandscapeTopBanner, isLargeLandscapeBottomBanner, isLargePortraitTopBanner, isLargePortraitBottomBanner, isTopAdaptiveBanner, isBottomAdaptiveBanner;

    [Space(40)]
    [Header("Unity Ads Plugin")]
    public bool useUnityAds;
    public string unityGameID;
    private string RewardedVideoID = "Rewarded_Android", UnityinterstitialID = "Interstitial_Android";
    [Space(10)]
    public bool IsUnityRewarded, IsUnityInterstitial;

    [Space(30)]
    [Header("Low Memory Threshold")]
    public bool isMemoryThreshold;
    public int NoInitializatioBelow;
    public int NoAnyBannerBelow;
    public int NoAppOpenBelow;
    public int NoAnyInterstitialBelow;
    public int NoAnyRewardBelow;

    [Space(30)]
    [Header("Excluded Devices")]
    public string currentModel;
    bool deviceExcluded;
    public List<string> devicesToExclude;

    void OnEnable()
    {
        DontDestroyOnLoad(this.gameObject);
        if (PlayerPrefs.GetInt("ADSUNLOCK") == 1)
        {
            isAdsPurchased = true;
        }
    }
    private void Awake()
    {
        currentModel = SystemInfo.deviceModel.ToLower();

        GetDeviceExclusion(currentModel);

        Debug.Log("<color=green>" + currentModel + " is Excluded: " + deviceExcluded + "</color>");
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (isAdsPurchased)
            return;

        if (isTestMode)
        {
            bannerId = "ca-app-pub-3940256099942544/6300978111";
            adaptiveBannerId = "ca-app-pub-3940256099942544/6300978111";
            medBannerId = "ca-app-pub-3940256099942544/6300978111";
            appOpenId = "ca-app-pub-3940256099942544/9257395921";
            interstitialId = staticInterstitialId = "ca-app-pub-3940256099942544/1033173712";
            rewardedId = "ca-app-pub-3940256099942544/5224354917";
            rewardedInterstitialId = "ca-app-pub-3940256099942544/5354046379";
        }

        InitializeAdmobAds();
        if (useUnityAds)
        {
            InitializeUnityAds();
        }
#if unity_release
        ANRHandlerThreadStart();
#endif
    }

    void InitializeAdmobAds()
    {
        if (IsLowMemory(NoInitializatioBelow)) return;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            isAdmobInitialized = true;

            if (isRewardedAd)
                RequestRewardedAd();
            if (isRewardedInterstitial)
                RequestRewardedInterstitialAd();

            if (isAdsPurchased)
                return;

            if (isTopBanner || IsBottomBanner || isTopRightBanner || isTopLeftBanner || isBottomRightBanner | isBottomLeftBanner)
            {
                RequestBanner();
            }

            if (isTopRightCubeBanner || isTopLeftCubeBanner || isBottomRightCubeBanner || isBottomLeftCubeBanner)
            {
                RequestMedBanner();
            }

            if (isLargeLandscapeTopBanner || isLargeLandscapeBottomBanner || isLargePortraitTopBanner || isLargePortraitBottomBanner)
            {
                RequestLargeBanner();
            }
            if (isTopAdaptiveBanner || isBottomAdaptiveBanner)
            {
                RequestAdaptiveBanner();
            }
            RemoveAllBanners();

            if (isAppOpenAd)
            {
                RequestAppOpenAd();
            }
            if (isInterstitialAd)
            {
                RequestInterstitial();
            }
            if (isStaticInterstitialAd)
            {
                RequestStaticInterstitial();
            }
        });
    }


    #region Banners
    #region RequestBanners
    void RequestBanner()
    {
        if (isAdsPurchased)
            return;

        string adUnitId = bannerId;

        // Create a 320x50 banner at the top of the screen.
        bannerViewtopsmallbanner = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        // Load a banner ad.
        if (isTopBanner)
        {
            bannerViewtopsmallbanner.LoadAd(new AdRequest());
            bannerViewtopsmallbanner.Hide();
        }

        bannerViewbottomsmallbanner = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Load a banner ad.
        if (IsBottomBanner) {
            bannerViewbottomsmallbanner.LoadAd(new AdRequest());
            bannerViewbottomsmallbanner.Hide();
        }

        bannerViewtoprightbanner = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopRight);
        // Load a banner ad.
        if (isTopRightBanner) {
            bannerViewtoprightbanner.LoadAd(new AdRequest());
            bannerViewtoprightbanner.Hide();
        }

        bannerViewtopleftbanner = new BannerView(adUnitId, AdSize.Banner, AdPosition.TopLeft);
        // Load a banner ad.
        if (isTopLeftBanner) {
            bannerViewtopleftbanner.LoadAd(new AdRequest());
            bannerViewtopleftbanner.Hide();
        }

        bannerViewbottomleftbanner = new BannerView(adUnitId, AdSize.Banner, AdPosition.BottomLeft);
        // Load a banner ad.
        if (isBottomRightBanner) {
            bannerViewbottomleftbanner.LoadAd(new AdRequest());
            bannerViewbottomleftbanner.Hide();
        }

        bannerViewbottomrightbanner = new BannerView(adUnitId, AdSize.Banner, AdPosition.BottomRight);
        // Load a banner ad.
        if (isBottomLeftBanner)
        {
            bannerViewbottomrightbanner.LoadAd(new AdRequest());
            bannerViewbottomrightbanner.Hide();
        }
    }

    void RequestMedBanner()
    {
        if (isAdsPurchased)
            return;

        string adUnitId = medBannerId;

        AdSize adSizecube = new AdSize(300, 250);
        bannerViewtopleftcubebanner = new BannerView(adUnitId, adSizecube, AdPosition.TopLeft);
        // Load a banner ad.
        if (isTopLeftCubeBanner) {
            bannerViewtopleftcubebanner.LoadAd(new AdRequest());
            bannerViewtopleftcubebanner.Hide();
        }

        bannerViewtoprightcubebanner = new BannerView(adUnitId, adSizecube, AdPosition.TopRight);
        // Load a banner ad.
        if (isTopRightCubeBanner) { 
            bannerViewtoprightcubebanner.LoadAd(new AdRequest());
            bannerViewtoprightcubebanner.Hide();
            }

        bannerViewbottomleftcubebanner = new BannerView(adUnitId, adSizecube, AdPosition.BottomLeft);
        // Load a banner ad.
        if (isBottomLeftCubeBanner) {
            bannerViewbottomleftcubebanner.LoadAd(new AdRequest());
            bannerViewbottomleftcubebanner.Hide();
        }

        bannerViewbottomrightcubebanner = new BannerView(adUnitId, adSizecube, AdPosition.BottomRight);
        // Load a banner ad.
        if (isBottomRightCubeBanner)
        {
            bannerViewbottomrightcubebanner.LoadAd(new AdRequest());
            bannerViewbottomrightcubebanner.Hide();
        }
    }

    void RequestLargeBanner()
    {
        if (isAdsPurchased)
            return;

        string adUnitId = largeBannerId;
        AdSize adSizelargelandscape = new AdSize(500, 200);
        bannerViewtoplargelandscapebanner = new BannerView(adUnitId, adSizelargelandscape, AdPosition.Top);
        // Load a banner ad.
        if (isLargeLandscapeTopBanner) {
            bannerViewtoplargelandscapebanner.LoadAd(new AdRequest());
            bannerViewtoplargelandscapebanner.Hide();
        }

        bannerViewbottomlargelandscapebanner = new BannerView(adUnitId, adSizelargelandscape, AdPosition.Bottom);
        // Load a banner ad.
        if (isLargeLandscapeBottomBanner) {
            bannerViewbottomlargelandscapebanner.LoadAd(new AdRequest());
            bannerViewbottomlargelandscapebanner.Hide();
        }

        AdSize adSizelargeportrait = new AdSize(320, 100);
        bannerViewtoplargeportraitbanner = new BannerView(adUnitId, adSizelargeportrait, AdPosition.Top);
        // Load a banner ad.
        if (isLargePortraitTopBanner) {
            bannerViewtoplargeportraitbanner.LoadAd(new AdRequest());
            bannerViewtoplargeportraitbanner.Hide();
        }

        bannerViewbottomlargeportraitbanner = new BannerView(adUnitId, adSizelargeportrait, AdPosition.Bottom);
        // Load a banner ad.
        if (isLargePortraitBottomBanner)
        {
            bannerViewbottomlargeportraitbanner.LoadAd(new AdRequest());
            bannerViewbottomlargeportraitbanner.Hide();
        }
    }

    void RequestAdaptiveBanner()
    {
        if (isAdsPurchased)
            return;

        int deviceWidth = MobileAds.Utils.GetDeviceSafeWidth();
        string adUnitId = adaptiveBannerId;
        bannerviewtopadaptive = new BannerView(adaptiveBannerId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth), AdPosition.Top);
        // Load a banner ad.
        if (isTopAdaptiveBanner)
        {
            bannerviewtopadaptive.LoadAd(new AdRequest());
            bannerviewtopadaptive.Hide();
        }

        bannerviewbottomadaptive = new BannerView(adaptiveBannerId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(deviceWidth), AdPosition.Bottom);
        // Load a banner ad.
        if (isBottomAdaptiveBanner)
        {
            bannerviewbottomadaptive.LoadAd(new AdRequest());
            bannerviewbottomadaptive.Hide();
        }


    }
    #endregion
    #region ShowBanners
    public void ShowTopSmallBanner()
    {
        RemoveTopSmallBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtopsmallbanner != null)
            bannerViewtopsmallbanner.Show();
    }

    public void ShowBottomSmallBanner()
    {
        RemoveBottomSmallBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomsmallbanner != null)
            bannerViewbottomsmallbanner.Show();
    }

    public void ShowTopRightBanner()
    {
        RemoveTopRightBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtoprightbanner != null)
            bannerViewtoprightbanner.Show();
    }

    public void ShowBottomRightBanner()
    {
        RemoveBottomRightBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomrightbanner != null)
            bannerViewbottomrightbanner.Show();
    }

    public void ShowTopLeftBanner()
    {
        RemoveTopLeftBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtopleftbanner != null)
            bannerViewtopleftbanner.Show();
    }

    public void ShowBottomLeftBanner()
    {
        RemoveBottomLeftBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomleftbanner != null)
            bannerViewbottomleftbanner.Show();
    }

    public void ShowTopLeftCubeBanner()
    {
        RemoveTopLeftCubeBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtopleftcubebanner != null)
            bannerViewtopleftcubebanner.Show();
    }

    public void ShowBottomLeftCubeBanner()
    {
        RemoveBottomLeftCubeBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomleftcubebanner != null)
            bannerViewbottomleftcubebanner.Show();
    }

    public void ShowTopRightCubeBanner()
    {
        RemoveTopRightCubeBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtoprightcubebanner != null)
            bannerViewtoprightcubebanner.Show();
    }

    public void ShowBottomRightCubeBanner()
    {
        RemoveBottomRightCubeBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomrightcubebanner != null)
            bannerViewbottomrightcubebanner.Show();
    }

    public void ShowTopLargePortraitBanner()
    {
        RemoveTopLargePortraitBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtoplargeportraitbanner != null)
            bannerViewtoplargeportraitbanner.Show();
    }

    public void ShowBottomLargePortraitBanner()
    {
        RemoveBottomLargePortraitBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomlargeportraitbanner != null)
            bannerViewbottomlargeportraitbanner.Show();
    }

    public void ShowTopLargeLandscapeBanner()
    {
        RemoveTopLargeLandscapeBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewtoplargelandscapebanner != null)
            bannerViewtoplargelandscapebanner.Show();
    }

    public void ShowBottomLargeLandscapeBanner()
    {
        RemoveBottomLargeLandscapeBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerViewbottomlargelandscapebanner != null)
            bannerViewbottomlargelandscapebanner.Show();
    }

    public void ShowTopAdaptiveBanner()
    {
        RemoveTopAdaptiveBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerviewtopadaptive != null)
            bannerviewtopadaptive.Show();
    }

    public void ShowBottomAdaptiveBanner()
    {
        RemoveBottomAdaptiveBanner();
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyBannerBelow)) return;
        if (bannerviewbottomadaptive != null)
            bannerviewbottomadaptive.Show();
    }

    #endregion
    #region RemoveBanners

    public void RemoveTopSmallBanner()
    {
        if (bannerViewtopsmallbanner != null)
            bannerViewtopsmallbanner.Hide();
    }
    public void RemoveBottomSmallBanner()
    {
        if (bannerViewbottomsmallbanner != null)
            bannerViewbottomsmallbanner.Hide();
    }

    public void RemoveTopRightBanner()
    {
        if (bannerViewtoprightbanner != null)
            bannerViewtoprightbanner.Hide();
    }
    public void RemoveBottomRightBanner()
    {
        if (bannerViewbottomrightbanner != null)
            bannerViewbottomrightbanner.Hide();
    }

    public void RemoveTopLeftBanner()
    {
        if (bannerViewtopleftbanner != null)
            bannerViewtopleftbanner.Hide();
    }
    public void RemoveBottomLeftBanner()
    {
        if (bannerViewbottomleftbanner != null)
            bannerViewbottomleftbanner.Hide();
    }

    public void RemoveTopLeftCubeBanner()
    {
        if (bannerViewtopleftcubebanner != null)
            bannerViewtopleftcubebanner.Hide();
    }
    public void RemoveBottomLeftCubeBanner()
    {
        if (bannerViewbottomleftcubebanner != null)
            bannerViewbottomleftcubebanner.Hide();
    }

    public void RemoveTopRightCubeBanner()
    {
        if (bannerViewtoprightcubebanner != null)
            bannerViewtoprightcubebanner.Hide();
    }
    public void RemoveBottomRightCubeBanner()
    {
        if (bannerViewbottomrightcubebanner != null)
            bannerViewbottomrightcubebanner.Hide();
    }

    public void RemoveTopLargePortraitBanner()
    {
        if (bannerViewtoplargeportraitbanner != null)
            bannerViewtoplargeportraitbanner.Hide();
    }
    public void RemoveBottomLargePortraitBanner()
    {
        if (bannerViewbottomlargeportraitbanner != null)
            bannerViewbottomlargeportraitbanner.Hide();
    }

    public void RemoveTopLargeLandscapeBanner()
    {
        if (bannerViewtoplargelandscapebanner != null)
            bannerViewtoplargelandscapebanner.Hide();
    }
    public void RemoveBottomLargeLandscapeBanner()
    {
        if (bannerViewbottomlargelandscapebanner != null)
            bannerViewbottomlargelandscapebanner.Hide();
    }

    public void RemoveTopAdaptiveBanner()
    {
        if (bannerviewtopadaptive != null)
            bannerviewtopadaptive.Hide();
    }
    public void RemoveBottomAdaptiveBanner()
    {
        if (bannerviewbottomadaptive != null)
            bannerviewbottomadaptive.Hide();
    }

    public void RemoveAllBanners()
    {
        if (bannerViewtopsmallbanner != null)
            bannerViewtopsmallbanner.Hide();
        if (bannerViewbottomsmallbanner != null)
            bannerViewbottomsmallbanner.Hide();
        if (bannerViewtoprightbanner != null)
            bannerViewtoprightbanner.Hide();
        if (bannerViewbottomrightbanner != null)
            bannerViewbottomrightbanner.Hide();
        if (bannerViewtopleftbanner != null)
            bannerViewtopleftbanner.Hide();
        if (bannerViewbottomleftbanner != null)
            bannerViewbottomleftbanner.Hide();
        if (bannerViewtopleftcubebanner != null)
            bannerViewtopleftcubebanner.Hide();
        if (bannerViewbottomleftcubebanner != null)
            bannerViewbottomleftcubebanner.Hide();
        if (bannerViewtoprightcubebanner != null)
            bannerViewtoprightcubebanner.Hide();
        if (bannerViewbottomrightcubebanner != null)
            bannerViewbottomrightcubebanner.Hide();
        if (bannerViewtoplargeportraitbanner != null)
            bannerViewtoplargeportraitbanner.Hide();
        if (bannerViewbottomlargeportraitbanner != null)
            bannerViewbottomlargeportraitbanner.Hide();
        if (bannerViewtoplargelandscapebanner != null)
            bannerViewtoplargelandscapebanner.Hide();
        if (bannerViewbottomlargelandscapebanner != null)
            bannerViewbottomlargelandscapebanner.Hide();
        if (bannerviewtopadaptive != null)
            bannerviewtopadaptive.Hide();
        if (bannerviewbottomadaptive != null)
            bannerviewbottomadaptive.Hide();
    }
    #endregion
    #endregion

    #region AppOpen
    public void RequestAppOpenAd()
    {
        if (isAdsPurchased)
            return;
        // send the request to load the ad.
        AppOpenAd.Load(appOpenId,  new AdRequest(),
            (AppOpenAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("app open ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("App open ad loaded with response : "
                          + ad.GetResponseInfo());

                appOpenAd = ad;
                RegisterEventHandlers(ad);
            });

    }
    private void RegisterEventHandlers(AppOpenAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("App open ad full screen content opened.");
            if (!appOpenAd.CanShowAd())
                RequestAppOpenAd();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("App open ad full screen content closed.");
            if (!appOpenAd.CanShowAd())
                RequestAppOpenAd();
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("App open ad failed to open full screen content " +
                           "with error : " + error);
            if (!appOpenAd.CanShowAd())
                RequestAppOpenAd();
        };
    }

    public bool isAppopenReady()
    {
        if (appOpenAd != null && appOpenAd.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ShowAppOpen()
    {
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAppOpenBelow)) return;

        if (isAdmobInitialized && appOpenAd.CanShowAd())
        {
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
            adShowing = true;
            appOpenAd.Show();
        }
    }
    #endregion

    #region Interstitial
    public void RequestInterstitial()
    {
        // send the request to load the ad.
        InterstitialAd.Load(interstitialId, new AdRequest(),
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
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(ad);
            });

    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
            if (!interstitialAd.CanShowAd())
                RequestInterstitial();
        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    public bool isAdmobInterstitialReady()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ShowInterstitial()
    {
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyInterstitialBelow)) return;

        if (isAdmobInitialized && interstitialAd.CanShowAd())
        {
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
            adShowing = true;
            interstitialAd.Show();
        }
    }
    #endregion

    #region Static Interstitial
    public void RequestStaticInterstitial()
    {
        if (isAdsPurchased)
            return;

        if (IsLowMemory(NoAnyInterstitialBelow)) return;

        // send the request to load the ad.
        InterstitialAd.Load(staticInterstitialId, new AdRequest(),
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("staticinterstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("StaticInterstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                staticInterstitialAd = ad;
                RegisterEventHandler(ad);
            });

    }

    private void RegisterEventHandler(InterstitialAd staticInterstitialAd)
    {
        // Raised when the ad is estimated to have earned money.
        staticInterstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        staticInterstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        staticInterstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        staticInterstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
            if (!staticInterstitialAd.CanShowAd())
                RequestStaticInterstitial();
        };
        // Raised when the ad closed full screen content.
        staticInterstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
        };
        // Raised when the ad failed to open full screen content.
        staticInterstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    public bool isAdmobStaticInterstitialReady()
    {
        if (staticInterstitialAd != null && staticInterstitialAd.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ShowStaticInterstitial()
    {
        if (isAdsPurchased)
            return;
        if (isAdmobInitialized && staticInterstitialAd.CanShowAd())
        {
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
            adShowing = true;
            staticInterstitialAd.Show();
        }
    }
    #endregion

    #region Rewarded Ads
    public void RequestRewardedAd()
    {
        // send the request to load the ad.
        RewardedAd.Load(rewardedId, new AdRequest(),
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(ad);
            });
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            if (!rewardedAd.CanShowAd())
            {
                RequestRewardedAd();
            }
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            if (!rewardedAd.CanShowAd())
            {
                RequestRewardedAd();
            }
        };
    }

    public bool isAdmobRewardedReady()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ShowAdmobRewardedAdWithName(string adname)
    {
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyRewardBelow)) return;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CheckInternetPanel.SetActive(true);
        }
        else if (isAdmobInitialized && rewardedAd.CanShowAd())
        {
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
            adShowing = true;
            rewardedAdName = adname;
            rewardedAd.Show((Reward reward) =>
            {
                StartCoroutine(GetReward());
            });
        }
        else
        {
            InitializeAdmobAds();
        }
    }
    public void ShowAdmobRewardedAd()
    {
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyRewardBelow)) return;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CheckInternetPanel.SetActive(true);
        }
        else if (isAdmobInitialized && rewardedAd.CanShowAd())
        {
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
            adShowing = true;
            
            rewardedAd.Show((Reward reward) =>
            {
                StartCoroutine(GetReward());
            });
        }
        else
        {
            InitializeAdmobAds();
        }
    }


    public void RequestRewardedInterstitialAd()
    {
        // send the request to load the ad.
        RewardedInterstitialAd.Load(rewardedInterstitialId, new AdRequest(),
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedInterstitial = ad;
                RegisterEventHandlers(ad);
            });
    }

    private void RegisterEventHandlers(RewardedInterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content closed.");
            if (!rewardedInterstitial.CanShowAd())
            {
                RequestRewardedInterstitialAd();
            }
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded interstitial ad failed to open " +
                           "full screen content with error : " + error);
            if (!rewardedInterstitial.CanShowAd())
            {
                RequestRewardedInterstitialAd();
            }
        };
    }

    public bool isAdmobRewardedInterstitialReady()
    {
        if (rewardedInterstitial != null && rewardedInterstitial.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowRewardedInterstitialAd(string adname)
    {
        if (isAdsPurchased)
            return;
        if (IsLowMemory(NoAnyRewardBelow)) return;

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            CheckInternetPanel.SetActive(true);
        }
        else if (isAdmobInitialized && rewardedInterstitial.CanShowAd())
        {
#if ANRSupervisor
            ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
            adShowing = true;
            rewardedAdName = adname;
            rewardedInterstitial.Show((Reward reward) =>
            {
                StartCoroutine(GetReward());
            });
        }
        else
        {
            InitializeAdmobAds();
        }
    }

    #endregion

    #region GetReward
    IEnumerator GetReward()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (rewardedAdName == "BusAd")
        {
            if (GarageScript.instance != null)
            {
                GarageScript.instance.UnlockedBusByVideo(GarageScript.instance.busNum);
            }
        }
        else if (rewardedAdName == "FreeCoins")
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
            for (int i = 0; i < MainMenu.instance.totalCoinsText.Length; i++)
            {
                MainMenu.instance.totalCoinsText[i].text = PlayerPrefs.GetInt("Coins").ToString();
            }
        }
        else if (rewardedAdName == "DoubleReward")
        {
            PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 500);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 3000);
            GameplayScript.instance.totalCashText.text = PlayerPrefs.GetInt("Cash").ToString();
            GameplayScript.instance.totalCoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
            GameplayScript.instance.victoryPanel.SetActive(false);
            GameplayScript.instance.completePanel.SetActive(true);
        }
        else if (rewardedAdName == "SkipLevel")
        {
            GameplayScript.instance.GotLevelSkipped();
        }

        else if (rewardedAdName == "GetCoins")
        {
            ShopScript.instance.GotCoinsByReward(ShopScript.instance.coinsoffernum);
            for (int i = 0; i < MainMenu.instance.totalCoinsText.Length; i++)
            {
                MainMenu.instance.totalCoinsText[i].text = PlayerPrefs.GetInt("Coins").ToString();
            }
        }

        else if (rewardedAdName == "GetCash")
        {
            ShopScript.instance.GotCashByReward(ShopScript.instance.cashoffernum);
            for (int i = 0; i < MainMenu.instance.totalCashText.Length; i++)
            {
                MainMenu.instance.totalCashText[i].text = PlayerPrefs.GetInt("Cash").ToString();
            }
        }
        rewardedAdName = "";
    }
    #endregion

    #region UnityAds

    public void InitializeUnityAds()
    {
        if (isAdsPurchased)
            return;
#if UNITY_EDITOR
        print("UnityAds Initialization");
#endif
        Advertisement.Initialize(unityGameID, isTestMode, this);
    }

    public bool IsVideoReady()
    {
        if (IsUnityInterstitial)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsRewardedVideoReady()
    {
        if (IsUnityRewarded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShowUnityVideoAd()
    {

        if (isAdsPurchased)
            return;

        adShowing = true;
        Advertisement.Show(UnityinterstitialID, this);
    }

    public void ShowUnityRewardedVideoAd(string adname)
    {
#if UNITY_EDITOR
        Debug.Log($"MUSTAFA");
#endif

        adShowing = true;
        rewardedAdName = adname;
        Advertisement.Show(RewardedVideoID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
#if UNITY_EDITOR
        Debug.Log($"KAMAL: {placementId}");
#endif
        if (placementId == RewardedVideoID)
        {
            IsUnityRewarded = true;
#if UNITY_EDITOR
            Debug.Log("KAMAL 2");
#endif
        }
        else if (placementId == UnityinterstitialID)
            IsUnityInterstitial = true;
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }
    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Time.timeScale = 1;
        if (placementId == RewardedVideoID)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                try
                {
                    Time.timeScale = 1;
                    StartCoroutine(GetReward());
                }
                catch { }
            }
            IsUnityRewarded = false;
            Advertisement.Load(RewardedVideoID, this);
        }
        else if (placementId == UnityinterstitialID)
        {
            IsUnityInterstitial = false;
            Advertisement.Load(UnityinterstitialID, this);
        }
    }


    public void OnInitializationComplete()
    {
        Advertisement.Load(UnityinterstitialID, this);
        Advertisement.Load(RewardedVideoID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }


    #endregion

    #region OnFocus For AppOpenAds
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (appOpenAd != null && !adShowing)
            {
                ShowAppOpen();
            }
            else
            {
                adShowing = false;
            }
        }
    }

    #endregion

    #region ANR Supervisor

    bool isANRSupervisorInitilized = false;
#if UNITY_ANDROID
    AndroidJavaClass ANRSupervisorClass;
#endif
    void ANRSupervisorObservation(ANRSupervisorObservingANRState state)
    {
#if UNITY_ANDROID
        if (!isANRSupervisorInitilized)
        {
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    try
                    {
                        ANRSupervisorClass = new AndroidJavaClass("ANRSupervisor");
                        ANRSupervisorClass.CallStatic("create");
                        Debug.Log("ANRSupervisor create");
                        isANRSupervisorInitilized = true;

                    }
                    catch (Exception) { return; }
                }
                else return;
            }
        }
        switch (state)
        {
            case ANRSupervisorObservingANRState.Start:
                {
                    try
                    {
                        ANRSupervisorClass.CallStatic("start");
                        Debug.Log("ANRSupervisor start");
                    }
                    catch (Exception) { }
                    break;
                }
            case ANRSupervisorObservingANRState.Stop:
                {
                    try
                    {
                        ANRSupervisorClass.CallStatic("stop");
                        Debug.Log("ANRSupervisor stop");
                    }
                    catch (Exception) { }
                    break;
                }
        }
#endif
    }
    enum ANRSupervisorObservingANRState
    {
        Start,
        Stop
    }
    #endregion

    #region ANR Handler

#if unity_release
    void ANRHandlerThreadStart()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass pluginClass = null;
            try
            {
                pluginClass = new AndroidJavaClass("com.anr.unity.ProcessExecuter");
            }
            catch (Exception) { return; }

            if (pluginClass != null)
            {
                try
                {
                    pluginClass.CallStatic("create");
                    pluginClass.CallStatic("start");
                    Debug.Log("ANR Handler Thread Started");
                }
                catch (Exception) { return; }
            }
        }
    }
#endif

    #endregion

    #region EditorWindow
#if UNITY_EDITOR
    public class CustomScriptDefineSymbols : EditorWindow
    {
        class FileStructure
        {
            public string filePath;
            public string fileName;
            public string fileType;

            public FileStructure(string path, string name, string type)
            {
                filePath = path;
                fileName = name;
                fileType = type;
            }
        }

        static System.Collections.Generic.List<FileStructure> files = new System.Collections.Generic.List<FileStructure>()
        {
            new FileStructure("Assets/AdsManager/","AdsManager",".cs"),
            new FileStructure("Assets/Firebase/Plugins/","Firebase.Analytics",".dll"),
            new FileStructure("Assets/Plugins/Android/","ANRSupervisor",".java"),
            new FileStructure("Assets/Plugins/Android/","unity-release",".aar")
        };

        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                foreach (FileStructure file in files)
                {
                    bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                    if (isAssetPresent) SetEnabled(file.fileName, true);
                }
                EditorApplication.projectChanged += OnProjectChanged;
            }
        }
        static void SetEnabled(string defineName, bool enable)
        {
            defineName = new System.Text.RegularExpressions.Regex("['*-.,&#^@]").Replace(defineName, "_");
            foreach (var group in buildTargetGroups)
            {
                var defines = GetDefinesList(group);

                if (enable)
                {
                    if (defines.Contains(defineName))
                        return;
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                        return;
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }
        static void OnProjectChanged()
        {
            foreach (FileStructure file in files)
            {
                bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                if (!isAssetPresent) SetEnabled(file.fileName, false);
            }
        }
        static System.Collections.Generic.List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new System.Collections.Generic.List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
        private static readonly BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS, };
    }
#endif
    #endregion

    #region Memory Information

    readonly int memoryThreshHold = 500; //in MBs
    static int memoryAvailable = 0;
    static System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"\d+");

    // return True if memory low by defined threshHold
    public bool IsLowMemory(int threshold = -1)
    {
        #region IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor) return false;
        #endregion
        try
        {
            if (!isMemoryThreshold) return false;

            threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
            return LoadMemoryInfo().Equals(true) ? (memoryAvailable / 1024) <= threshold : false;
        }
        catch (Exception) { return true; }
    }
    static bool LoadMemoryInfo()
    {
        try
        {
            //if file not exist retrun from here
            if (!System.IO.File.Exists("/proc/meminfo")) return false;
            System.IO.FileStream fs = new System.IO.FileStream("/proc/meminfo", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.ToLower().Replace(" ", "");
                if (line.Contains("memavailable")) { memoryAvailable = int.Parse(re.Match(line).Value); }
            }
            sr.Close(); fs.Close(); fs.Dispose();
            return true;
        }
        catch (Exception) { return false; }
    }

    bool LowMemoryAndEventSend(int threshold = -1)
    {
        try
        {
            bool returnValue = false;
            string parameterValue = "defaultValue";
            if (LoadMemoryInfo().Equals(true))
            {
                threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
                float availableInMB = (memoryAvailable / 1024);
                if (availableInMB <= threshold)
                {
                    if (availableInMB < 50)
                        parameterValue = "less than 50";
                    else if (availableInMB < 100)
                        parameterValue = "less than 100";
                    else if (availableInMB < 150)
                        parameterValue = "less than 150";
                    else if (availableInMB < 200)
                        parameterValue = "less than 200";
                    else if (availableInMB < 250)
                        parameterValue = "less than 250";
                    else if (availableInMB < 300)
                        parameterValue = "less than 300";
                    else if (availableInMB < 350)
                        parameterValue = "less than 350";
                    else if (availableInMB < 400)
                        parameterValue = "less than 400";
                    else if (availableInMB < 450)
                        parameterValue = "less than 450";
                    else if (availableInMB < 500)
                        parameterValue = "less than 500";
                    else
                        parameterValue = "greater than 500";
                    returnValue = true;
                }
                else
                {
                    parameterValue = "greater than 500";
                    returnValue = false;
                }
            }
            //            if (isLowMemoryAnalytics)
            //            {
            //#if Firebase_Analytics
            //                //check if Firebase is initilized successfully, then send these events
            //                Firebase.Analytics.FirebaseAnalytics.LogEvent("MemoryInfo", new Firebase.Analytics.Parameter("lowMemory", parameterValue));
            //#endif
            //            }
            return returnValue;
        }
        catch (Exception) { return true; }
    }

    #endregion

    #region Excluded Devices
    private void GetDeviceExclusion(string currentDeviceModel)
    {
        for (var i = 0; i < devicesToExclude.Count; i++)
        {
            if (currentDeviceModel.Contains(devicesToExclude[i]))
            {
                deviceExcluded = true;
                return;
            }
            else
            {
                deviceExcluded = false;
            }
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < devicesToExclude.Count; i++)
        {
            devicesToExclude[i] = devicesToExclude[i].ToLower();
        }
    }
#endif
    #endregion

    #region Calling BothAds
    static int ads;
    public void ShowBothInterstitial()
    {
        if (ads == 0)
        {
            if (isAdmobInterstitialReady())
                ShowInterstitial();
            else if (IsUnityInterstitial)
                ShowUnityVideoAd();
            ads = 1;
        }
        else if (ads == 1)
        {
            if (isAdmobInterstitialReady())
                ShowInterstitial();
            else if (IsUnityInterstitial)
                ShowUnityVideoAd();
            ads = 2;
        }
        else if (ads == 2)
        {
            if (IsUnityInterstitial)
                ShowUnityVideoAd();
            else if (isAdmobInterstitialReady())
                ShowInterstitial();
            ads = 0;
        }
    }

    #endregion

    #region CustomCode
    // Start Adnan Code

    // End Adnan Code
    #endregion
}
