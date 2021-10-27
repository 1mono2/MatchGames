using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchasing : MonoBehaviour, IStoreListener 
{
    //購入実行に使用
    IStoreController Controller { get; set; }
    //リストア処理など拡張機能で使用
    IExtensionProvider Extensions { get; set; }
    AdMobBanner banner;
    bool isInitiatePurchaseFlag =true;



    private void Start()
    {
        Initialize();
        banner = GameObject.FindGameObjectWithTag("Banner").GetComponent<AdMobBanner>();
    }
    void Initialize()
    {

        //builderを作成しプロダクトを登録する。
        StandardPurchasingModule module = StandardPurchasingModule.Instance();
        //Uエディタ検証用の表示を作ってくれる
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser; 
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        //②プロダクト(アイテム)の登録第
        builder.AddProduct("jp.NoAds.480yen", ProductType.Subscription);
        //②以下のようにAndroid、iOSで別のproduct_idでもアプリ上で一つのプロダクトとして登録することもできる。
        /* 
         * builder.AddProduct("jp.hogehoge.1yen", ProductType.Consumable, new IDs
                     {
                     { "jp.hogehoge.android.1yen", GooglePlay.Name },
                     { "jp.hogehoge.ios.1yen", AppleAppStore.Name }

         });
         */


        //IAP初期化
        UnityPurchasing.Initialize(this, builder);

    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Controller = controller;
        Extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //初期化失敗の場合errorには以下の場合がある。
        //エラーによって警告表示やストア表示を変えるなどの分岐、ログ取得などを記述するといい
        switch (error) {
            case InitializationFailureReason.PurchasingUnavailable: // デバイス設定でアプリ内購入が無効になっている
                break;
            case InitializationFailureReason.NoProductsAvailable:   // 購入可能なプロダクトがない
                break;
            case InitializationFailureReason.AppNotKnown:           // 不明なアプリ
                break;
        }
        
    }

    public void PurchasingButton()
    {
        if (isInitiatePurchaseFlag == false) { return; }
        else { StartCoroutine(InitiatePurchaseCountStart()); }
        //登録したプロダクトで購入をかける
        //var product = Controller.products.WithID("jp.hogehoge.1yen");
        //Controller.InitiatePurchase(product);

        //上記はproductを取得し、購入をかけているが実際にはしたのようにproduct}_idの指定だけでも購入はかけられる。
        Controller.InitiatePurchase("jp.NoAds.480yen");
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Productを取得する。
        //レシート情報などが格納されている
        Product product = purchaseEvent.purchasedProduct;
        string receipt = purchaseEvent.purchasedProduct.receipt;
        //通貨コード(Currencyなどが取得可能)
        ProductMetadata metaData = product.metadata;
        //↓通貨コード。サーバで購入時通貨など収集するときは送ってあげたりする
        //metaData.isoCurrencyCode
        //isoCurrencyCodeでの値段
        //metaData.localizedPrice

        //プロダクトの詳細(UnityIAPのID、ストアにおけるID、プロダクトのタイプのみ)
        ProductDefinition definition = product.definition;
        //↓こっちはUnityIAP上のID
        //definition.id   
        //↓こっちは各プラットフォームストアごとのプロダクトのID 
        //definition.storeSpecificId


        //ここらへんでアプリのバックエンドサーバにレシートを渡すなどして、レシート検証とアイテムの付与などを行わせる。


        //PurchaseProcessingResult.Pending とした場合には購入完了扱いにはならない(サーバでのレシート検証を待ってから終了とする場合など)
        //ConfirmPendingPurchase(明示的に消費処理)がかけられるまでProcessPurchaseが呼び出され続けるようになる
        //PurchaseProcessingResult.Completeはすぐに購入消費完了扱いとなりアイテムの消費などが行われる
        if (definition.id.Equals("jp.NoAds.480yen")){
            if(banner != null)
            {
                banner.BannerDestory();
            }
            
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //購入処理が失敗した場合に呼び出される
        //以下のようなパターンがある

        switch (failureReason)
        {
            case PurchaseFailureReason.PurchasingUnavailable:   // 購入機能が無効になっている
                break;
            case PurchaseFailureReason.ExistingPurchasePending: // 既に購入処理が進行している
                break;
            case PurchaseFailureReason.ProductUnavailable:      // 購入不可能なプロダクト
                break;
            case PurchaseFailureReason.SignatureInvalid:        // レシートの署名検証に失敗
                break;
            case PurchaseFailureReason.UserCancelled:           // ユーザーが購入をキャンセルした
                break;
            case PurchaseFailureReason.PaymentDeclined:         // 支払いに問題があった
                break;
            case PurchaseFailureReason.DuplicateTransaction:    // トランザクションの重複
                break;
            case PurchaseFailureReason.Unknown:                 // 上記以外の認識されていないエラー
                break;
        }
    }

    IEnumerator InitiatePurchaseCountStart()
    {
        isInitiatePurchaseFlag = false;

        yield return new WaitForSeconds(3f);

        isInitiatePurchaseFlag = true;
    }



}
