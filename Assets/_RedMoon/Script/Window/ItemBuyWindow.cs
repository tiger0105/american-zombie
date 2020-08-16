using UnityEngine;
using UnityEngine.UI;
using Polaris.GameData;

public class ItemBuyWindow : MonoBehaviour
{
    public static ItemBuyWindow Instance;
    public TitleBar titleBar;
    public GameObject itemSelBtnPrefab;
    public GameObject itemScrollObj;
    public Text nameText;
    public Text minPowerText;
    public Text maxPowerText;
    public Text infoText;
    public Text priceText;
    [SerializeField] private Image m_CashIcon;
    [SerializeField] private Image m_GoldIcon;
    GameObject curItemModel;
    string curItemStr = string.Empty;
    [SerializeField] private GameObject m_WeaponUnlockDialog;
    [SerializeField] private GameObject m_OverGuiCamera;

    public void OnBackBtnClick()
    {
        GameManager.Instance.ProcEventMessages(EventMessages.ENTER_ITEMUPGRADE_WINDOW);
    }
    public void OnItemSelectBtnClick(GameObject button)
    {
        ShowElement(button.name);
    }
    public void OnBuyBtnClick()
    {
        if (curItemStr.Substring(0, 1).Equals("i"))
        {
            GameManager.Instance.ProcEventMessages(EventMessages.ITEM_BUY, curItemStr.Substring(1));
        }
        else
        {
            GameManager.Instance.ProcEventMessages(EventMessages.WEAPON_BUY, curItemStr.Substring(1));
        }
    }

    public void OnBuySuccess()
    {
        ShowElement(curItemStr);
    }

    void Awake()
    {
        Instance = this;
    }

    void ShowPlayerInfo()
    {
        titleBar.UpdatePlayInfo();
    }
    public void ShowElement(string item_str)
    {
        ShowPlayerInfo();
        curItemStr = item_str;
        if (item_str.Substring(0, 1).Equals("w"))
        {
            ShowWeapon(int.Parse(item_str.Substring(1)));
        }
        else if (item_str.Substring(0, 1).Equals("i"))
        {
            ShowItem(int.Parse(item_str.Substring(1)));
        }
    }
    void ShowWeapon(int id)
    {
        WeaponData wp = WeaponData.dataMap[id + 1];
        GameObject obj = (GameObject)Resources.Load(wp.ShopModel);
        curItemModel = Instantiate(obj) as GameObject;
        ItemModelBase.Instance.ShowModel(curItemModel);
        nameText.text = wp.Name;
        int min_power = wp.GetMinPower();
        int max_power = wp.GetMaxPower();
        minPowerText.text = min_power.ToString();
        maxPowerText.text = max_power.ToString();
        priceText.text = wp.Price.ToString();
        infoText.text = wp.Description;
        minPowerText.gameObject.SetActive(true);
        maxPowerText.gameObject.SetActive(true);
        m_CashIcon.gameObject.SetActive(true);
        m_GoldIcon.gameObject.SetActive(false);
    }
    void ShowItem(int id)
    {
        ItemData wp = ItemData.dataMap[id + 1];
        GameObject obj = (GameObject)Resources.Load(wp.ShopModel);
        curItemModel = Instantiate(obj);
        curItemModel.layer = 16;
        ItemModelBase.Instance.ShowModel(curItemModel);
        nameText.text = wp.Name;
        int price = (int)wp.Price;
        priceText.text = price.ToString();

        if (wp.Description.Contains("{color=#D02830}"))
            wp.Description = wp.Description.Replace("{color=#D02830}", "<color=#D02830>");
        if (wp.Description.Contains("{/color}"))
            wp.Description = wp.Description.Replace("{/color}", "</color>");
        if (wp.Description.Contains("{size=20}"))
            wp.Description = wp.Description.Replace("{size=20}", "<size=20>");
        if (wp.Description.Contains("{/size}"))
            wp.Description = wp.Description.Replace("{/size}", "</size>");
        if (wp.Description.Contains("{br}"))
            wp.Description = wp.Description.Replace("{br}", "\n");

        infoText.text = wp.Description;
        minPowerText.gameObject.SetActive(false);
        maxPowerText.gameObject.SetActive(false);
        if (id >= 12 && id < 15)
        {
            m_CashIcon.gameObject.SetActive(false);
            m_GoldIcon.gameObject.SetActive(true);
        }
        else
        {
            m_CashIcon.gameObject.SetActive(true);
            m_GoldIcon.gameObject.SetActive(false);
        }
    }

    public void OnCloseWeaponUnlockDialog()
    {
        m_WeaponUnlockDialog.SetActive(false);
        m_OverGuiCamera.SetActive(true);
    }

    void Start()
    {

        MissionData data = MissionProgress.GetCurMissionData();
        if (data.Index == 14)
        {
            string dataAsJson = FileTool.ReadFile("Data", false);
            GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
            if (GameManager.Instance.persistantData.isPlasmaLocked == false)
            {
                m_WeaponUnlockDialog.SetActive(true);
                m_OverGuiCamera.SetActive(false);
                GameManager.Instance.persistantData.isPlasmaLocked = false;
                string st = JsonUtility.ToJson(GameManager.Instance.persistantData);
                FileTool.createORwriteFile("Data", st);
            }
        }

        for (int i = 0; i < WeaponData.dataMap.Count; i++)
        {
            string dataAsJson = FileTool.ReadFile("Data", false);
            MissionData curMission = MissionProgress.GetCurMissionData();
            GameManager.Instance.persistantData = JsonUtility.FromJson<PersistantData>(dataAsJson);
            if (curMission.Index < 14 && i == 6 && GameManager.Instance.persistantData.isPlasmaLocked == true)
            {
                continue;
            }
            else
            {
                GameObject item_sel_btn = Instantiate(itemSelBtnPrefab);
                item_sel_btn.name = string.Format("w{0}", i);
                item_sel_btn.transform.SetParent(itemScrollObj.transform);
                item_sel_btn.transform.localScale = Vector3.one;
                item_sel_btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                item_sel_btn.transform.GetChild(0).GetComponent<Text>().text = WeaponData.dataMap[i + 1].Name;
                item_sel_btn.transform.GetChild(1).GetComponent<Text>().text = "W";
            }
        }

        for (int i = 0; i < ItemData.dataMap.Count; i++)
        {
            ItemData ida = ItemData.dataMap[i + 1];
            if (ida.Name == "COIN_BAG")
            {
                continue;
            }
            MissionData curMission = MissionProgress.GetCurMissionData();
            if (curMission.Index < 14 && i == 6)
            {
                continue;
            }
            else
            {
                GameObject item_sel_btn = Instantiate(itemSelBtnPrefab);
                item_sel_btn.name = string.Format("i{0}", i);
                item_sel_btn.transform.SetParent(itemScrollObj.transform);
                item_sel_btn.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                item_sel_btn.transform.localScale = Vector3.one;
                item_sel_btn.GetComponentInChildren<Text>().text = ItemData.dataMap[i + 1].Name;
                if (i >= 0 && i < 8)
                {
                    Color col;
                    if (ColorUtility.TryParseHtmlString("#60126DFF", out col) == true)
                        item_sel_btn.GetComponent<Image>().color = col;
                    item_sel_btn.transform.GetChild(1).GetComponent<Text>().text = "C";
                }
                if (i >= 8 && i < 12 || i == 14 || i == 15)
                {
                    Color col;
                    if (ColorUtility.TryParseHtmlString("#003E85FF", out col) == true)
                        item_sel_btn.GetComponent<Image>().color = col;
                    item_sel_btn.transform.GetChild(1).GetComponent<Text>().text = "P";
                }
                else if (i >= 12 && i < 14)
                {
                    Color col;
                    if (ColorUtility.TryParseHtmlString("#272727FF", out col) == true)
                        item_sel_btn.GetComponent<Image>().color = col;
                    item_sel_btn.transform.GetChild(1).GetComponent<Text>().text = "B";
                }
            }
        }
        ShowElement("w0");
    }
}