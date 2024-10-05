using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;
using UnityEngine.SceneManagement;

public class SettingsSceneManager : MonoBehaviour, IStoreListener
{
    public Button removeAdsButton;
    public Button restorePurchasesButton;
    public Button writeReviewButton;

    // Unity IAP 関連
    private IStoreController storeController;
    private IExtensionProvider extensionProvider;

    // 商品 ID
    private const string removeAdsProductID = "removeads";

    void Start()
    {
        // ボタンのクリックイベントを設定
        removeAdsButton.onClick.AddListener(OnRemoveAdsButtonClicked);
        restorePurchasesButton.onClick.AddListener(OnRestorePurchasesButtonClicked);
        writeReviewButton.onClick.AddListener(OnWriteReviewButtonClicked);

        // Unity IAP の初期化
        InitializePurchasing();
    }

    // Unity IAP の初期化
    public void InitializePurchasing()
    {
        if (storeController != null) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // 商品の追加
        builder.AddProduct(removeAdsProductID, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    // 「広告を削除」ボタンがクリックされたとき
    public void OnRemoveAdsButtonClicked()
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(removeAdsProductID);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"購入処理を開始します: {product.definition.id}");
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("購入できる商品が見つかりません");
            }
        }
        else
        {
            Debug.Log("IAP が初期化されていません");
        }
    }

    // 「購入を復元」ボタンがクリックされたとき
    public void OnRestorePurchasesButtonClicked()
    {
        if (IsInitialized())
        {
#if UNITY_IOS
            var apple = extensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions(result =>
            {
                if (result)
                {
                    Debug.Log("購入の復元が成功しました");
                }
                else
                {
                    Debug.Log("購入の復元に失敗しました");
                }
            });
#elif UNITY_ANDROID
            var google = extensionProvider.GetExtension<IGooglePlayStoreExtensions>();
            google.RestoreTransactions(result =>
            {
                if (result)
                {
                    Debug.Log("購入の復元が成功しました");
                }
                else
                {
                    Debug.Log("購入の復元に失敗しました");
                }
            });
#else
            Debug.Log("このプラットフォームでは購入の復元はサポートされていません");
#endif
        }
        else
        {
            Debug.Log("IAP が初期化されていません");
        }
    }

    // 「レビューを書く」ボタンがクリックされたとき
    public void OnWriteReviewButtonClicked()
    {
        // プラットフォームに応じてレビュー画面を開く
#if UNITY_IOS
        // iOS のレビューURLに遷移
        Application.OpenURL("https://apps.apple.com/app/id6736353585?action=write-review");
#elif UNITY_ANDROID
        // Android のレビューURLに遷移
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.infinity.spinningball");
#else
        Debug.Log("このプラットフォームではレビューを書く機能はサポートされていません");
#endif
    }

    // IStoreListener の実装
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP 初期化成功");
        storeController = controller;
        extensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"IAP 初期化失敗: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, removeAdsProductID, StringComparison.Ordinal))
        {
            Debug.Log("広告削除の購入が成功しました");

            // 広告を削除する処理をここに追加
            // 例: ゲーム内で広告を表示するスクリプトのフラグをオフにする
            // AdManager.Instance.DisableAds();

            // 購入情報を保存
            PlayerPrefs.SetInt("AdsRemoved", 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log($"不明な商品を購入しました: {args.purchasedProduct.definition.id}");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"購入に失敗しました: {product.definition.id}, 理由: {failureReason}");
    }

    private bool IsInitialized()
    {
        return storeController != null && extensionProvider != null;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public void ToTitleButtonTapped()
    {
        bool isAdsRemoved = PlayerPrefs.GetInt("AdsRemoved", 0) == 1;

        if (!isAdsRemoved)
        {
            if (InterstitialAdManager.Instance.IsReady)
            {
                Debug.Log("インタースティシャル");
                InterstitialAdManager.Instance.ShowInterstitial();
                InterstitialAdManager.Instance.OnAdClosed += OnInterstitialAdClosed;
            }
            else
            {
                Debug.Log("インタースティシャル広告が準備できていません。直接タイトル画面に戻ります。");
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            Debug.Log("広告は削除されています。直接タイトル画面に戻ります。");
            SceneManager.LoadScene(0);
        }
    }

    private void OnInterstitialAdClosed()
    {
        // 広告が閉じられた後にタイトル画面に戻る
        SceneManager.LoadScene(0);
        
        // イベントハンドラを解除
        InterstitialAdManager.Instance.OnAdClosed -= OnInterstitialAdClosed;
    }
}