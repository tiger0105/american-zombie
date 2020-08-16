using UnityEngine;
using UnityEngine.UI;
using Polaris.GameData;
public class ItemUpgradeWindow : MonoBehaviour
{
    static public ItemUpgradeWindow Instance;
    public TitleBar titleBar;
    public Text nameText;
    public Text upPriceText;
    public RawImage priceImage;
    [SerializeField] private Image m_GoldIcon;
    public GameObject itemSelBtnPrefab;
    public GameObject itemsParent;
    public GameObject upgradeBtn;

    [SerializeField] private GameObject m_ItemDamage;
    [SerializeField] private GameObject m_ItemFireRate;
    [SerializeField] private GameObject m_ItemAttackRange;
    [SerializeField] private GameObject m_ItemReloadSpeed;
    [SerializeField] private GameObject m_ItemAmmoCapacity;
    [SerializeField] private GameObject m_ItemLevel;

    [SerializeField] private GameObject m_ItemAttributes;

    [SerializeField] private GameObject m_SpectaclesAttributes;

    public void OnBackBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_MAINMENU_WINDOW);
    }
    public void OnItemsBuyClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_ITEMBUY_WINDOW);
    }
    public void OnUpgradeBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ITEM_UPGADE, curItemStr.Substring(1), isWeapon);
    }
    public void OnMissionBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_MISSION_WINDOW);
    }
    public void OnItemSelectBtnClick(GameObject button)
    {
        ShowElement(button.name);
    }
    public void OnUpgradeSuccess()
    {
        ShowElement(curItemStr);
        //GameManager.Instance.GetUIMan().Inform("Upgrading Now....");
    }
    public void OnUpgradeFailed()
    {
        //GameManager.Instance.GetUIMan().Inform("Not Enough Money!");
    }
    GameObject curItemModel;
    public GameObject CurItemModel
    {
        get { return curItemModel; }
    }
    string curItemStr = string.Empty;

    private bool isWeapon = true;

    void Awake()
    {
        Instance = this;
    }

    public void LoadInventory()
    {
        DoLoadInventory();
    }
    void DoLoadInventory()
    {
        m_ItemAttributes.SetActive(true);
        m_ItemLevel.SetActive(true);
        upgradeBtn.GetComponent<Button>().interactable = true;

        while (itemsParent.transform.childCount > 0)
        {
            GameObject child = itemsParent.transform.GetChild(0).gameObject;
            DestroyImmediate(child);
        }

        for (int i = 0; i < WeaponInventory.idList.Count; i++)
        {
            //create weapon element
            int weapon_id = WeaponInventory.idList[i];
            GameObject item_sel_btn = GameObject.Instantiate(itemSelBtnPrefab);
            item_sel_btn.name = string.Format("w{0}", weapon_id);
            item_sel_btn.transform.SetParent(itemsParent.transform);
            item_sel_btn.transform.localScale = Vector3.one;
            item_sel_btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            //show weapon name
            GameObject btn = item_sel_btn.transform.GetChild(0).gameObject;
            item_sel_btn.GetComponentInChildren<Text>().text = WeaponData.dataMap[weapon_id + 1].Name;
            //show image image
            string path_image = WeaponData.dataMap[weapon_id + 1].Image;
            Sprite sprite = Resources.Load<Sprite>(path_image);
            Vector2 sample_size = btn.GetComponent<RectTransform>().sizeDelta;
            float sample_ratio = sample_size.x / sample_size.y;
            float sprite_ratio = (float)sprite.texture.width / (float)sprite.texture.height;
            int w, h;
            if (sprite_ratio >= sample_ratio)
            {
                w = (int)sample_size.x;
                h = (int)(w / sprite_ratio);
            }
            else
            {
                h = (int)sample_size.y;
                w = (int)(h * sprite_ratio);
            }
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            btn.GetComponent<Image>().sprite = sprite;

            //show equipe buton
            int equip = WeaponInventory.equipeList[i];
            WeaponData wp = WeaponData.dataMap[weapon_id + 1];
            int inventory_id = WeaponInventory.GetInventoryID(weapon_id);
            int newLevel = WeaponInventory.levelList[inventory_id] - 1;

            if (WeaponData.dataMap[weapon_id + 1].GetDamage_LevelUp() == 0 || newLevel == 2)
                m_ItemDamage.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            else
                m_ItemDamage.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + WeaponData.dataMap[weapon_id + 1].GetDamage_LevelUp();
            m_ItemDamage.transform.GetChild(1).GetComponent<Slider>().value = WeaponData.dataMap[weapon_id + 1].GetDamage() + newLevel * WeaponData.dataMap[weapon_id + 1].GetDamage_LevelUp();
            m_ItemDamage.transform.GetChild(2).GetComponent<Text>().text = (WeaponData.dataMap[weapon_id + 1].GetDamage() + newLevel * WeaponData.dataMap[weapon_id + 1].GetDamage_LevelUp()).ToString();

            if (WeaponData.dataMap[weapon_id + 1].GetFireRate_LevelUp() == 0 || newLevel == 2)
                m_ItemFireRate.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            else
            {
                float rate = WeaponData.dataMap[weapon_id + 1].GetFireRate_LevelUp() * 20;
                m_ItemFireRate.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + rate.ToString();
            }
            m_ItemFireRate.transform.GetChild(1).GetComponent<Slider>().value = 1 / (WeaponData.dataMap[weapon_id + 1].GetFireRate() - newLevel * WeaponData.dataMap[weapon_id + 1].GetFireRate_LevelUp());
            m_ItemFireRate.transform.GetChild(2).GetComponent<Text>().text = Mathf.Ceil(1 / (WeaponData.dataMap[weapon_id + 1].GetFireRate() - newLevel * WeaponData.dataMap[weapon_id + 1].GetFireRate_LevelUp())).ToString();

            if (WeaponData.dataMap[weapon_id + 1].GetRange_LevelUp() == 0 || newLevel == 2)
                m_ItemAttackRange.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            else
                m_ItemAttackRange.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + WeaponData.dataMap[weapon_id + 1].GetRange_LevelUp();
            m_ItemAttackRange.transform.GetChild(1).GetComponent<Slider>().value = WeaponData.dataMap[weapon_id + 1].GetRange() + newLevel * WeaponData.dataMap[weapon_id + 1].GetRange_LevelUp();
            m_ItemAttackRange.transform.GetChild(2).GetComponent<Text>().text = (WeaponData.dataMap[weapon_id + 1].GetRange() + newLevel * WeaponData.dataMap[weapon_id + 1].GetRange_LevelUp()).ToString();

            if (WeaponData.dataMap[weapon_id + 1].GetReloadSpeed_LevelUp() == 0 || newLevel == 2)
                m_ItemReloadSpeed.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            else
                m_ItemReloadSpeed.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + (WeaponData.dataMap[weapon_id + 1].GetReloadSpeed_LevelUp() * 10).ToString();
            m_ItemReloadSpeed.transform.GetChild(1).GetComponent<Slider>().value = 1 / (WeaponData.dataMap[weapon_id + 1].GetReloadSpeed() - newLevel * WeaponData.dataMap[weapon_id + 1].GetReloadSpeed_LevelUp());
            m_ItemReloadSpeed.transform.GetChild(2).GetComponent<Text>().text = Mathf.Ceil(1 / (WeaponData.dataMap[weapon_id + 1].GetReloadSpeed() - newLevel * WeaponData.dataMap[weapon_id + 1].GetReloadSpeed_LevelUp())).ToString();

            if (WeaponData.dataMap[weapon_id + 1].GetAmmoCapacity_LevelUp() == 0 || newLevel == 2)
                m_ItemAmmoCapacity.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            else
                m_ItemAmmoCapacity.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + WeaponData.dataMap[weapon_id + 1].GetAmmoCapacity_LevelUp();
            m_ItemAmmoCapacity.transform.GetChild(1).GetComponent<Slider>().value = WeaponData.dataMap[weapon_id + 1].GetAmmoCapacity() + newLevel * WeaponData.dataMap[weapon_id + 1].GetAmmoCapacity_LevelUp();
            m_ItemAmmoCapacity.transform.GetChild(2).GetComponent<Text>().text = (WeaponData.dataMap[weapon_id + 1].GetAmmoCapacity() + newLevel * WeaponData.dataMap[weapon_id + 1].GetAmmoCapacity_LevelUp()).ToString();

            for (int j = 0; j < m_ItemLevel.transform.childCount; j++)
            {
                m_ItemLevel.transform.GetChild(j).GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
            }
            for (int j = 0; j <= newLevel; j++)
            {
                m_ItemLevel.transform.GetChild(j).GetComponent<Image>().color = new Color(1, 1, 1);
            }
        }

        for (int i = 0; i < ItemInventory.idList.Count; i++)
        {
            //create item element
            int id = ItemInventory.idList[i];
            GameObject item_sel_btn = Instantiate(itemSelBtnPrefab);
            item_sel_btn.name = string.Format("i{0}", id);
            item_sel_btn.transform.SetParent(itemsParent.transform);
            item_sel_btn.transform.localScale = Vector3.one;
            item_sel_btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            //show item name
            GameObject btn = item_sel_btn.transform.GetChild(0).gameObject;
            item_sel_btn.GetComponentInChildren<Text>().text = ItemData.dataMap[id + 1].Name;
            //show item image
            string path_image = ItemData.dataMap[id + 1].Image;
            Sprite sprite = Resources.Load<Sprite>(path_image);
            Vector2 sample_size = btn.GetComponent<RectTransform>().sizeDelta;
            float sample_ratio = sample_size.x / sample_size.y;
            float sprite_ratio = sprite.texture.width / (float)sprite.texture.height;
            int w, h;
            if (sprite_ratio >= sample_ratio)
            {
                w = (int)sample_size.x;
                h = (int)(w / sprite_ratio);
            }
            else
            {
                h = (int)sample_size.y;
                w = (int)(h * sprite_ratio);
            }
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);
            btn.GetComponent<Image>().sprite = sprite;
        }
        // show player info
        ShowPlayerInfo();
        // show default weapon info
        ShowElement("w" + WeaponInventory.idList[0].ToString());
    }
    void ShowPlayerInfo()
    {
        titleBar.UpdatePlayInfo();
    }
    void ShowElement(string item_str)
    {
        ShowPlayerInfo();
        curItemStr = item_str;
        if (item_str.Substring(0, 1).Equals("w"))
        {
            ShowWeapon(int.Parse(item_str.Substring(1)));
            m_SpectaclesAttributes.SetActive(false);
            isWeapon = true;
        }
        else if (item_str.Substring(0, 1).Equals("i"))
        {
            int idx = int.Parse(item_str.Substring(1));
            ShowItem(idx);
            if (idx >= 12 && idx < 15)
            {
                priceImage.gameObject.SetActive(false);
                m_GoldIcon.gameObject.SetActive(true);
            }
            else
            {
                priceImage.gameObject.SetActive(true);
                m_GoldIcon.gameObject.SetActive(false);
            }
            isWeapon = false;
        }
    }
    void ShowWeapon(int id)
    {
        m_ItemAttributes.SetActive(true);
        m_ItemLevel.SetActive(true);

        WeaponData wp = WeaponData.dataMap[id + 1];
        GameObject obj = (GameObject)Resources.Load(wp.ShopModel);
        curItemModel = Instantiate(obj) as GameObject;
        ItemModelBase.Instance.ShowModel(curItemModel);
        
        nameText.text = wp.Name;
        int inventory_id = WeaponInventory.GetInventoryID(id);
        int cur_power = WeaponInventory.powerList[inventory_id];
        int max_power = wp.GetMaxPower();
        int add_price = (int)(cur_power * GameInfo.upgradeRate * GameInfo.priceRate * GameInfo.upgradeDifficulty);
        int add_power = (int)(cur_power * GameInfo.upgradeRate);
        upPriceText.gameObject.SetActive(true);
        upgradeBtn.SetActive(true);
        
        int newLevel = WeaponInventory.levelList[inventory_id] - 1;

        if (WeaponData.dataMap[id + 1].GetDamage_LevelUp() == 0 || newLevel == 2)
            m_ItemDamage.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        else
            m_ItemDamage.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + WeaponData.dataMap[id + 1].GetDamage_LevelUp();
        m_ItemDamage.transform.GetChild(1).GetComponent<Slider>().value = WeaponData.dataMap[id + 1].GetDamage() + newLevel * WeaponData.dataMap[id + 1].GetDamage_LevelUp();
        m_ItemDamage.transform.GetChild(2).GetComponent<Text>().text = (WeaponData.dataMap[id + 1].GetDamage() + newLevel * WeaponData.dataMap[id + 1].GetDamage_LevelUp()).ToString();

        if (WeaponData.dataMap[id + 1].GetFireRate_LevelUp() == 0 || newLevel == 2)
            m_ItemFireRate.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        else
        {
            float rate = WeaponData.dataMap[id + 1].GetFireRate_LevelUp() * 20;
            m_ItemFireRate.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + rate.ToString();
        }
        m_ItemFireRate.transform.GetChild(1).GetComponent<Slider>().value = 1 / (WeaponData.dataMap[id + 1].GetFireRate() - newLevel * WeaponData.dataMap[id + 1].GetFireRate_LevelUp());
        m_ItemFireRate.transform.GetChild(2).GetComponent<Text>().text = Mathf.Ceil(1 / (WeaponData.dataMap[id + 1].GetFireRate() - newLevel * WeaponData.dataMap[id + 1].GetFireRate_LevelUp())).ToString();

        if (WeaponData.dataMap[id + 1].GetRange_LevelUp() == 0 || newLevel == 2)
            m_ItemAttackRange.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        else
            m_ItemAttackRange.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + WeaponData.dataMap[id + 1].GetRange_LevelUp();
        m_ItemAttackRange.transform.GetChild(1).GetComponent<Slider>().value = WeaponData.dataMap[id + 1].GetRange() + newLevel * WeaponData.dataMap[id + 1].GetRange_LevelUp();
        m_ItemAttackRange.transform.GetChild(2).GetComponent<Text>().text = (WeaponData.dataMap[id + 1].GetRange() + newLevel * WeaponData.dataMap[id + 1].GetRange_LevelUp()).ToString();

        if (WeaponData.dataMap[id + 1].GetReloadSpeed_LevelUp() == 0 || newLevel == 2)
            m_ItemReloadSpeed.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        else
            m_ItemReloadSpeed.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + (WeaponData.dataMap[id + 1].GetReloadSpeed_LevelUp() * 10).ToString();
        m_ItemReloadSpeed.transform.GetChild(1).GetComponent<Slider>().value = 1 / (WeaponData.dataMap[id + 1].GetReloadSpeed() - newLevel * WeaponData.dataMap[id + 1].GetReloadSpeed_LevelUp());
        m_ItemReloadSpeed.transform.GetChild(2).GetComponent<Text>().text = Mathf.Ceil(1 / (WeaponData.dataMap[id + 1].GetReloadSpeed() - newLevel * WeaponData.dataMap[id + 1].GetReloadSpeed_LevelUp())).ToString();

        if (WeaponData.dataMap[id + 1].GetAmmoCapacity_LevelUp() == 0 || newLevel == 2)
            m_ItemAmmoCapacity.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
        else
            m_ItemAmmoCapacity.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "+ " + WeaponData.dataMap[id + 1].GetAmmoCapacity_LevelUp();
        m_ItemAmmoCapacity.transform.GetChild(1).GetComponent<Slider>().value = WeaponData.dataMap[id + 1].GetAmmoCapacity() + newLevel * WeaponData.dataMap[id + 1].GetAmmoCapacity_LevelUp();
        m_ItemAmmoCapacity.transform.GetChild(2).GetComponent<Text>().text = (WeaponData.dataMap[id + 1].GetAmmoCapacity() + newLevel * WeaponData.dataMap[id + 1].GetAmmoCapacity_LevelUp()).ToString();

        for (int j = 0; j < m_ItemLevel.transform.childCount; j++)
        {
            m_ItemLevel.transform.GetChild(j).GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
        }
        for (int j = 0; j <= newLevel; j++)
        {
            m_ItemLevel.transform.GetChild(j).GetComponent<Image>().color = new Color(1, 1, 1);
        }

        if (newLevel == 2)
        {
            upgradeBtn.GetComponent<Button>().interactable = false;
            upPriceText.gameObject.SetActive(false);
        }
        else
        {
            upgradeBtn.GetComponent<Button>().interactable = true;
            upPriceText.gameObject.SetActive(true);
        }

        if (newLevel == 0)
        {
            priceImage.gameObject.SetActive(true);
            m_GoldIcon.gameObject.SetActive(false);
            if (wp.Name == "Shotgun")
                upPriceText.text = "5000";
            else if (wp.Name == "M16 Carbine Assault Rifle")
                upPriceText.text = "9500";
            else if (wp.Name == "M4 Carbon Rifle")
                upPriceText.text = "15000";
            else if (wp.Name == "Grenade Launcher")
                upPriceText.text = "25000";
            else
                upPriceText.text = add_price.ToString();
        }
        else if (newLevel == 1)
        {
            priceImage.gameObject.SetActive(false);
            m_GoldIcon.gameObject.SetActive(true);
            upPriceText.text = ((int)(add_price / 200)).ToString();
        }
    }
    void ShowItem(int id)
    {
        ItemData wp = ItemData.dataMap[id + 1];
        GameObject obj = (GameObject)Resources.Load(wp.ShopModel);
        curItemModel = Instantiate(obj) as GameObject;
        ItemModelBase.Instance.ShowModel(curItemModel);

        nameText.text = wp.Name;

        upgradeBtn.SetActive(true);

        if (id != 14)
        {
            upPriceText.gameObject.SetActive(false);
            m_ItemLevel.SetActive(false);
            m_SpectaclesAttributes.SetActive(false);

        }
        else
        {
            int spectaclesLevel = PlayerPrefs.GetInt("SpectaclesLevel", 1);
            
            for (int j = 0; j < m_ItemLevel.transform.childCount; j++)
            {
                m_ItemLevel.transform.GetChild(j).GetComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
            }
            for (int j = 0; j < spectaclesLevel; j++)
            {
                m_ItemLevel.transform.GetChild(j).GetComponent<Image>().color = new Color(1, 1, 1);
            }
            m_ItemLevel.SetActive(true);
            m_SpectaclesAttributes.SetActive(true);
            if (spectaclesLevel  == 3)
            {
                upgradeBtn.GetComponent<Button>().interactable = false;
                upPriceText.gameObject.SetActive(false);
            }
            else
            {
                upgradeBtn.GetComponent<Button>().interactable = true;
                upPriceText.gameObject.SetActive(true);
            }

            if (spectaclesLevel == 1)
            {
                upPriceText.text = "20";
            }
            else if (spectaclesLevel == 2)
            {
                upPriceText.text = "30";
            }
        }
        m_ItemAttributes.SetActive(false);
    }
}
