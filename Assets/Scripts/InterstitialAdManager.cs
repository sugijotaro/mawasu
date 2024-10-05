using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;

    public static InterstitialAdManager Instance { get; private set; }
    public event Action OnAdClosed;

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    private void Initialize()
    {
        LoadInterstitialAd();
    }

    public bool IsReady
    {
        get
        {
            // 広告削除を購入しているかチェック
            bool isAdsRemoved = PlayerPrefs.GetInt("AdsRemoved", 0) == 1;

            if (isAdsRemoved)
            {
                return false;
            }

            return interstitialAd != null && interstitialAd.CanShowAd();
        }
    }

    public void ShowInterstitial()
    {
        if (IsReady)
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial ad is not ready or ads are removed.");
        }
    }

    private void LoadInterstitialAd()
    {
        Debug.Log("LoadInterstitialAd");
        // 広告削除を購入しているかチェック
        bool isAdsRemoved = PlayerPrefs.GetInt("AdsRemoved", 0) == 1;

        if (isAdsRemoved)
        {
            Debug.Log("Ads are removed. Interstitial ad will not be loaded.");
            return;
        }

        // 古い広告がある場合は破棄
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        string adUnitId;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-6018673196408347/3541297013"; // あなたの Android 用の広告ユニットIDに置き換えてください
#elif UNITY_IOS
        adUnitId = "ca-app-pub-6018673196408347/1468198831"; // あなたの iOS 用の広告ユニットIDに置き換えてください
#else
        adUnitId = "unexpected_platform";
#endif

        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // 広告が収益を発生させたとき
        interstitialAd.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // 広告のインプレッションが記録されたとき
        interstitialAd.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // 広告がクリックされたとき
        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // 広告がフルスクリーンコンテンツを開いたとき
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // 広告がフルスクリーンコンテンツを閉じたとき
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");

            OnAdClosed?.Invoke();

            LoadInterstitialAd();
        };
        // 広告がフルスクリーンコンテンツの表示に失敗したとき
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd();
        };
    }
}