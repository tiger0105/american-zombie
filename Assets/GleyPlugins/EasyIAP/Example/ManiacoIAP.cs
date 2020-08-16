using UnityEngine;
using System.Collections.Generic;

public class ManiacoIAP : MonoBehaviour
{
    public class ManiacoStoreProducts
    {
        public ShopProductNames name;
        public bool bought;

        public ManiacoStoreProducts(ShopProductNames name, bool bought)
        {
            this.name = name;
            this.bought = bought;
        }
    }

    private List<ManiacoStoreProducts> products = new List<ManiacoStoreProducts>();

    public static ManiacoIAP m_ManiacoIAPInstance = null;

    private void Awake()
    {
        if (!m_ManiacoIAPInstance)
        {
            m_ManiacoIAPInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void Start()
    {
        if (!IAPManager.Instance.IsInitialized())
        {
            IAPManager.Instance.InitializeIAPManager(InitializeResult);
        }
    }

    public void PurchaseProduct(int index)
    {
        Debug.Log("Processing purchase for product " + products[index].name);
        IAPManager.Instance.BuyProduct(products[index].name, ProductBought);
    }

    private void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (status == IAPOperationStatus.Success)
        {
            if (IAPManager.Instance.debug)
            {
                Debug.Log("Buy product completed: " + product.localizedTitle + " receive value: " + product.value);
            }

            Debug.Log("Successfully purchased product " + product.productName);

            string dataAsJson, st;

            switch (product.productName)
            {
                case "PiggyBank":
                    GameInfo.playerCoin += 10000;
                    GameInfo.SavePlayerCoin();
                    GameInfo.playerGold += 2;
                    GameInfo.SavePlayerGold();
                    break;
                case "KillerPack":
                    GameInfo.playerCoin += 2000;
                    GameInfo.SavePlayerCoin();
                    if (ItemInventory.idList.Contains(10) == false)
                    {
                        ItemInventory.idList.Add(10);
                    }
                    if (ItemInventory.idList.Contains(11) == false)
                    {
                        ItemInventory.idList.Add(11);
                    }
                    if (WeaponInventory.idList.Contains(2) == false)
                    {
                        WeaponInventory.Append(2);
                    }
                    ItemInventory.Save();
                    WeaponInventory.Save();
                    break;
                case "RemoveAds":
                    dataAsJson = FileTool.ReadFile("Data", false);
                    GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
                    GameManager.Instance.persistantData.isAdsRemoved = true;
                    st = JsonUtility.ToJson(GameManager.Instance.persistantData);
                    FileTool.createORwriteFile("Data", st);
                    break;
                case "UltraPack":
                    GameInfo.playerGold += 6;
                    GameInfo.SavePlayerGold();
                    if (ItemInventory.idList.Contains(10) == false)
                    {
                        ItemInventory.idList.Add(10);
                    }
                    if (WeaponInventory.idList.Contains(2) == false)
                    {
                        WeaponInventory.Append(2);
                    }
                    if (WeaponInventory.idList.Contains(2) == false)
                    {
                        WeaponInventory.Append(2);
                    }
                    if (WeaponInventory.idList.Contains(6) == false)
                    {
                        WeaponInventory.Append(6);
                    }
                    ItemInventory.Save();
                    WeaponInventory.Save();
                    dataAsJson = FileTool.ReadFile("Data", false);
                    GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
                    GameManager.Instance.persistantData.isPlasmaLocked = false;
                    st = JsonUtility.ToJson(GameManager.Instance.persistantData);
                    FileTool.createORwriteFile("Data", st);
                    break;
                case "LowGoldPack":
                    GameInfo.playerGold += 2;
                    GameInfo.SavePlayerGold();
                    break;
                case "TinyGoldPack":
                    GameInfo.playerGold += 6;
                    GameInfo.SavePlayerGold();
                    break;
                case "MediumGoldPack":
                    GameInfo.playerGold += 10;
                    GameInfo.SavePlayerGold();
                    break;
                case "LargeGoldPack":
                    GameInfo.playerGold += 18;
                    GameInfo.SavePlayerGold();
                    break;
                case "XLargerGoldPack":
                    GameInfo.playerGold += 70;
                    GameInfo.SavePlayerGold();
                    break;
                case "LowCashPack":
                    GameInfo.playerCoin += 2000;
                    GameInfo.SavePlayerCoin();
                    break;
                case "TinyCashPack":
                    GameInfo.playerCoin += 10000;
                    GameInfo.SavePlayerCoin();
                    break;
                case "MediumCashPack":
                    GameInfo.playerCoin += 25000;
                    GameInfo.SavePlayerCoin();
                    break;
                case "LargeCashPack":
                    GameInfo.playerCoin += 75000;
                    GameInfo.SavePlayerCoin();
                    break;
                case "XLargerCashPack":
                    GameInfo.playerCoin += 200000;
                    GameInfo.SavePlayerCoin();
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (IAPManager.Instance.debug)
            {
                Debug.Log("Buy product failed: " + message);
            }
        }
    }

    private void InitializeResult(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
    {
        products = new List<ManiacoStoreProducts>();
        
        if (status == IAPOperationStatus.Success)
        {
            for (int i = 0; i < shopProducts.Count; i++)
            {
                if (shopProducts[i].productName == "RemoveAds")
                {
                    if (shopProducts[i].active)
                    {
                        
                    }
                }

                products.Add(new ManiacoStoreProducts(IAPManager.Instance.ConvertNameToShopProduct(shopProducts[i].productName), shopProducts[i].active));
            }
        }
        else
        {
            Debug.Log("Error initializing IAP");
        }
    }
}
