using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAd : MonoBehaviour
{
    private BannerView bannerView;

    void Start()
    {
        bool isAdsRemoved = PlayerPrefs.GetInt("AdsRemoved", 0) == 1;

        if (!isAdsRemoved)
        {
            RequestBanner();
        }
        else
        {
            Debug.Log("広告は削除されています。バナー広告を表示しません。");
        }
    }

    void RequestBanner()
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
