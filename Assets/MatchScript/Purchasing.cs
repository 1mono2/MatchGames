using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchasing : MonoBehaviour, IStoreListener 
{
    //�w�����s�Ɏg�p
    IStoreController Controller { get; set; }
    //���X�g�A�����ȂǊg���@�\�Ŏg�p
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

        //builder���쐬���v���_�N�g��o�^����B
        StandardPurchasingModule module = StandardPurchasingModule.Instance();
        //U�G�f�B�^���ؗp�̕\��������Ă����
        module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser; 
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        //�A�v���_�N�g(�A�C�e��)�̓o�^��
        builder.AddProduct("jp.NoAds.480yen", ProductType.Subscription);
        //�A�ȉ��̂悤��Android�AiOS�ŕʂ�product_id�ł��A�v����ň�̃v���_�N�g�Ƃ��ēo�^���邱�Ƃ��ł���B
        /* 
         * builder.AddProduct("jp.hogehoge.1yen", ProductType.Consumable, new IDs
                     {
                     { "jp.hogehoge.android.1yen", GooglePlay.Name },
                     { "jp.hogehoge.ios.1yen", AppleAppStore.Name }

         });
         */


        //IAP������
        UnityPurchasing.Initialize(this, builder);

    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Controller = controller;
        Extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //���������s�̏ꍇerror�ɂ͈ȉ��̏ꍇ������B
        //�G���[�ɂ���Čx���\����X�g�A�\����ς���Ȃǂ̕���A���O�擾�Ȃǂ��L�q����Ƃ���
        switch (error) {
            case InitializationFailureReason.PurchasingUnavailable: // �f�o�C�X�ݒ�ŃA�v�����w���������ɂȂ��Ă���
                break;
            case InitializationFailureReason.NoProductsAvailable:   // �w���\�ȃv���_�N�g���Ȃ�
                break;
            case InitializationFailureReason.AppNotKnown:           // �s���ȃA�v��
                break;
        }
        
    }

    public void PurchasingButton()
    {
        if (isInitiatePurchaseFlag == false) { return; }
        else { StartCoroutine(InitiatePurchaseCountStart()); }
        //�o�^�����v���_�N�g�ōw����������
        //var product = Controller.products.WithID("jp.hogehoge.1yen");
        //Controller.InitiatePurchase(product);

        //��L��product���擾���A�w���������Ă��邪���ۂɂ͂����̂悤��product}_id�̎w�肾���ł��w���͂�������B
        Controller.InitiatePurchase("jp.NoAds.480yen");
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //Product���擾����B
        //���V�[�g���Ȃǂ��i�[����Ă���
        Product product = purchaseEvent.purchasedProduct;
        string receipt = purchaseEvent.purchasedProduct.receipt;
        //�ʉ݃R�[�h(Currency�Ȃǂ��擾�\)
        ProductMetadata metaData = product.metadata;
        //���ʉ݃R�[�h�B�T�[�o�ōw�����ʉ݂Ȃǎ��W����Ƃ��͑����Ă������肷��
        //metaData.isoCurrencyCode
        //isoCurrencyCode�ł̒l�i
        //metaData.localizedPrice

        //�v���_�N�g�̏ڍ�(UnityIAP��ID�A�X�g�A�ɂ�����ID�A�v���_�N�g�̃^�C�v�̂�)
        ProductDefinition definition = product.definition;
        //����������UnityIAP���ID
        //definition.id   
        //���������͊e�v���b�g�t�H�[���X�g�A���Ƃ̃v���_�N�g��ID 
        //definition.storeSpecificId


        //������ւ�ŃA�v���̃o�b�N�G���h�T�[�o�Ƀ��V�[�g��n���Ȃǂ��āA���V�[�g���؂ƃA�C�e���̕t�^�Ȃǂ��s�킹��B


        //PurchaseProcessingResult.Pending �Ƃ����ꍇ�ɂ͍w�����������ɂ͂Ȃ�Ȃ�(�T�[�o�ł̃��V�[�g���؂�҂��Ă���I���Ƃ���ꍇ�Ȃ�)
        //ConfirmPendingPurchase(�����I�ɏ����)����������܂�ProcessPurchase���Ăяo���ꑱ����悤�ɂȂ�
        //PurchaseProcessingResult.Complete�͂����ɍw������������ƂȂ�A�C�e���̏���Ȃǂ��s����
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
        //�w�����������s�����ꍇ�ɌĂяo�����
        //�ȉ��̂悤�ȃp�^�[��������

        switch (failureReason)
        {
            case PurchaseFailureReason.PurchasingUnavailable:   // �w���@�\�������ɂȂ��Ă���
                break;
            case PurchaseFailureReason.ExistingPurchasePending: // ���ɍw���������i�s���Ă���
                break;
            case PurchaseFailureReason.ProductUnavailable:      // �w���s�\�ȃv���_�N�g
                break;
            case PurchaseFailureReason.SignatureInvalid:        // ���V�[�g�̏������؂Ɏ��s
                break;
            case PurchaseFailureReason.UserCancelled:           // ���[�U�[���w�����L�����Z������
                break;
            case PurchaseFailureReason.PaymentDeclined:         // �x�����ɖ�肪������
                break;
            case PurchaseFailureReason.DuplicateTransaction:    // �g�����U�N�V�����̏d��
                break;
            case PurchaseFailureReason.Unknown:                 // ��L�ȊO�̔F������Ă��Ȃ��G���[
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
