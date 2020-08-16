using Michsky.UI.Dark;
using Polaris.GameData;
using UnityEngine;
using UnityEngine.UI;

public class OfferDialogManager : MonoBehaviour {

    [SerializeField] private GameObject m_OfferDialog;
    [SerializeField] private GameObject m_Bundle;
    [SerializeField] private Text m_Title;
    [SerializeField] private Text m_OriginalPrice;
    [SerializeField] private Text m_DiscountedPrice;
    [SerializeField] private Image m_ProductImage;

    [SerializeField] private GameObject m_OverGuiCamera;
    private bool wasGuiCameraActive = true;

    private void Start()
    {
        int randomNum = Random.Range(1, 5000);
        if (randomNum >= 0 && randomNum <= 1000 && GameSetting.showedOfferDialog == false)
        {
            wasGuiCameraActive = m_OverGuiCamera.activeSelf;
            m_OverGuiCamera.SetActive(false);
            m_OfferDialog.SetActive(true);
            m_OfferDialog.GetComponent<BlurManager>().BlurInAnim();
            int packID = randomNum % 4;
            PurchaseData pd = PurchaseData.dataMap[packID];
            m_Title.text = pd.DisplayName;
            m_OriginalPrice.text = string.Format("${0}", pd.PriceTier - 0.01f);
            m_DiscountedPrice.text = string.Format("${0}", (int)(pd.PriceTier / 2) - 0.01f);
            m_ProductImage.sprite = Resources.Load<Sprite>(pd.Image);
            m_Bundle.name = pd.ProductId;
            GameSetting.showedOfferDialog = true;
        }
    }

    public void PurchaseBundle()
    {
        PackOfferManager.Instance.OnCoinBuyButtonClick(m_Bundle);
    }

    public void ClosedOfferDialog()
    {
        m_OverGuiCamera.SetActive(false);
    }
}
