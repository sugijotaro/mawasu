using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAd : MonoBehaviour
{
    private BannerView bannerView;

    void Start()
    {
        string adUnitId;

        #if UNITY_ANDROID
            adUnitId = "ca-app-pub-6018673196408347/4592151662";
        #elif UNITY_IOS
            adUnitId = "ca-app-pub-6018673196408347/9367396816";
        #else
            adUnitId = "unexpected_platform";
        #endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }
}