using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobBanner : SingletonMonoBehaviour<AdMobBanner>
{
    const string appId = "ca-app-pub-1767951001352951~4156368944";
    // テスト用広告ユニットID
    const string adUnitId = "ca-app-pub-1767951001352951/1338633918";
    GameObject banner;
    BannerView bannerView;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start()
    {
        
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        RequestBanner();

        banner = GameObject.Find("BANNER(Clone)");
        DontDestroyOnLoad(banner);
    }

    private void RequestBanner()
    {

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-1767951001352951/1338633918";
#elif UNITY_IPHONE
            string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);

    }

    public void BannerDestory()
    {
        bannerView.Destroy();
    }


}