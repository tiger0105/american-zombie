﻿using UnityEngine;
using UnityEngine.UI;
using Polaris.GameData;
using TMPro;

public class CoinBuyWindow : MonoBehaviour
{
    public static CoinBuyWindow Instance;
    public ScrollRect scrollRectPanel;
    public GameObject packsParent;
    public GameObject goldParent;
    public GameObject cashParent;
    public GameObject coinBtnPrefab;
    public Text statusText;
    private static string msg;

    public void OnExitBtnClick()
    {
        if (GameManager.Instance.LastEventMessage == EventMessages.TRY_PURCHASE)
        {
            msg = "Please wait for the response of server....";
        }
        else if (GameManager.Instance.LastEventMessage == EventMessages.FAIL_PURCHASE)
        {
            gameObject.SetActive(false);
            msg = "";
        }
        else if (GameManager.Instance.LastEventMessage == EventMessages.SUCCESS_PURCHASE)
        {
            gameObject.SetActive(false);
            ItemBuyWindow.Instance.titleBar.UpdatePlayInfo();
            msg = "";
        }
        else
        {
            gameObject.SetActive(false);
            msg = "";
        }
    }

    public void SetScrollRectContent(RectTransform trans)
    {
        scrollRectPanel.content = trans;
    }

    public void OnCoinBuyButtonClick(GameObject obj)
    {
        TryPurchase(obj.name);
    }

    void Start()
    {
        Instance = this;
        LoadCoinElements();
    }

    void Update()
    {
        statusText.text = msg;
    }

    public void Init()
    {
        msg = "";
    }

    void LoadCoinElements()
    {
        for (int i = 0; i < PurchaseData.dataMap.Count; i++)
        {
            PurchaseData pd = PurchaseData.dataMap[i];
            GameObject coin_sel_btn = Instantiate(coinBtnPrefab);
            coin_sel_btn.name = i.ToString();
            if (i < 4)
                coin_sel_btn.transform.SetParent(packsParent.transform);
            else if (i >= 4 && i < 9)
            {
                coin_sel_btn.transform.SetParent(goldParent.transform);
                TextMeshProUGUI itemAmount = coin_sel_btn.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
                itemAmount.color = new Color32(240, 255, 0, 255);
                switch (i)
                {
                    case 4:
                        itemAmount.text = "2 Bars";
                        break;
                    case 5:
                        itemAmount.text = "6 Bars";
                        break;
                    case 6:
                        itemAmount.text = "10 Bars";
                        break;
                    case 7:
                        itemAmount.text = "18 Bars";
                        break;
                    case 8:
                        itemAmount.text = "70 Bars";
                        break;
                    default:
                        itemAmount.text = "";
                        break;
                }
            }
            else
            {
                coin_sel_btn.transform.SetParent(cashParent.transform);
                TextMeshProUGUI itemAmount = coin_sel_btn.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
                itemAmount.color = new Color32(31, 177, 76, 255);
                switch (i)
                {
                    case 9:
                        itemAmount.text = "$2,000";
                        break;
                    case 10:
                        itemAmount.text = "$10,000";
                        break;
                    case 11:
                        itemAmount.text = "$25,000";
                        break;
                    case 12:
                        itemAmount.text = "$75,000";
                        break;
                    case 13:
                        itemAmount.text = "$200,000";
                        break;
                    default:
                        itemAmount.text = "";
                        break;
                }
            }
            coin_sel_btn.transform.localScale = Vector3.one;
            coin_sel_btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            coin_sel_btn.transform.GetChild(0).GetComponent<Text>().text = pd.DisplayName;
            coin_sel_btn.transform.GetChild(1).GetComponent<Text>().text = string.Format("${0}", pd.PriceTier - 0.01f);
            coin_sel_btn.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(pd.Image);
            coin_sel_btn.transform.GetChild(2).GetComponent<Image>().preserveAspect = true;
            coin_sel_btn.transform.GetChild(2).GetComponent<Image>().color = Color.white;
        }
    }

    public void TryPurchase(string product_id)
    {
        Debug.Log("TryPurchase: " + product_id);

        int result = 0;
        bool canPurchase = int.TryParse(product_id, out result);
        if (canPurchase == true)
            ManiacoIAP.m_ManiacoIAPInstance.PurchaseProduct(result);
    }

    //private void OnPurchaseFlowFinishedAction(UM_PurchaseResult result)
    //{
    //    UM_InAppPurchaseManager.Client.OnPurchaseFinished -= OnPurchaseFlowFinishedAction;
    //    if (result.isSuccess)
    //    {
    //        msg = result.product.id + " purchase Success";
    //        GameManager.Instance.LastEventMessage = EventMessages.SUCCESS_PURCHASE;
    //        for (int i = 0; i < PurchaseData.dataMap.Count; i++)
    //        {
    //            PurchaseData pd = PurchaseData.dataMap[i];
    //            if (pd.ProductId == result.product.id)
    //            {
    //                GameInfo.playerCoin += pd.Coin;
    //                GameInfo.SavePlayerCoin();
    //            }

    //            if (pd.ProductId == result.product.id && (pd.ProductId == "coins_50000_1" || pd.ProductId == "coins_100000"))
    //            {
    //                bool hasAutorifleClip = false;
    //                bool hasHealthUp = false;

    //                for (int j = 0; j < ItemInventory.idList.Count; j++)
    //                {
    //                    if (ItemInventory.idList[j] == 2)
    //                        hasAutorifleClip = true;
    //                    else if (ItemInventory.idList[j] == 6)
    //                        hasHealthUp = true;
    //                }
    //                if (hasAutorifleClip == false)
    //                {
    //                    ItemInventory.idList.Add(2);
    //                    ItemInventory.equipeList.Add(1);
    //                }
    //                if (hasHealthUp == false)
    //                {
    //                    ItemInventory.idList.Add(6);
    //                    ItemInventory.equipeList.Add(1);
    //                }
    //                ItemInventory.Save();
    //                if (WeaponInventory.idList.Contains(2) == false)
    //                {
    //                    WeaponInventory.Append(2);
    //                    WeaponInventory.Save();
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        msg = result.product.id + " purchase Failed";
    //        GameManager.Instance.LastEventMessage = EventMessages.FAIL_PURCHASE;
    //    }
    //}
}
