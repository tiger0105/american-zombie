using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class vp_SimpleHUDMobile : vp_SimpleHUD
{
	public float FadeDuration = 3f;
	public bool ShowTips = true;
	public GameObject AmmoLabel = null;
	public GameObject HealthLabel = null;
	public GameObject HintsLabel = null;

	private TextMesh m_AmmoLabel = null;
	private TextMesh m_HealthLabel = null;
	private TextMesh m_HintsLabel = null;
	protected vp_UITween.Handle m_HUDTextTweenHandle = new vp_UITween.Handle();
	protected Color m_MessageColorMobile = new Color(2, 2, 0, 2);
	protected Color m_InvisibleColorMobile = new Color(1, 1, 0, 0);
	protected string m_PickupMessageMobile = "";
	protected static GUIStyle m_MessageStyleMobile = null;
	public static GUIStyle MessageStyleMobile
	{
		get
		{
			if (m_MessageStyleMobile == null)
			{
				m_MessageStyleMobile = new GUIStyle("Label");
				m_MessageStyleMobile.alignment = TextAnchor.MiddleCenter;
			}
			return m_MessageStyleMobile;
		}
	}

	protected vp_FPPlayerEventHandler m_PlayerEventHandler = null;
	protected int m_Health
	{
		get
		{
			int health = (int)(m_PlayerEventHandler.Health.Get() * HealthMultiplier);
			return health < 0 ? 0 : health;
		}
	}

	protected void Awake()
	{

		m_PlayerEventHandler = transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();

		RefreshUI();
	}

	void RefreshUI()
	{
		if (AmmoLabel != null) m_AmmoLabel = AmmoLabel.GetComponentInChildren<TextMesh>();
		if (HealthLabel != null) m_HealthLabel = HealthLabel.GetComponentInChildren<TextMesh>();
		if (HintsLabel != null) m_HintsLabel = HintsLabel.GetComponentInChildren<TextMesh>();

		if (m_HintsLabel != null)
		{
			m_HintsLabel.text = "";
			m_HintsLabel.GetComponent<Renderer>().material.color = Color.clear;
		}
	}

	void OnUIManagerFind(vp_UIManager uiManager)
	{
		List<vp_UIControl> controls = uiManager.transform.ChildComponentsToList<vp_UIControl>();
		foreach (vp_UIControl ctrl in controls)
		{
			if (AmmoLabel == null && ctrl.gameObject.name == "Ammo")
				AmmoLabel = ctrl.gameObject;
			else if (HealthLabel == null && ctrl.gameObject.name == "Health")
				HealthLabel = ctrl.gameObject;
			else if (HintsLabel == null && ctrl.gameObject.name == "HintsLabel")
				HintsLabel = ctrl.gameObject;
		}

		RefreshUI();
	}

	protected override void OnEnable()
	{

		if (m_PlayerEventHandler != null)
			m_PlayerEventHandler.Register(this);

	}

	protected override void OnDisable()
	{


		if (m_PlayerEventHandler != null)
			m_PlayerEventHandler.Unregister(this);

	}

	protected override void OnGUI()
	{


	}

	protected virtual void Update()
	{

		if (m_AmmoLabel != null)
		{
			if (m_PlayerEventHandler.CurrentWeaponIndex.Get() > 0)
			{
				m_AmmoLabel.text = (m_PlayerEventHandler.CurrentWeaponAmmoCount.Get() + "/" +
				(m_PlayerEventHandler.CurrentWeaponClipCount.Get())).ToString();
			}
			else
				m_AmmoLabel.text = "0/0";
		}

		if (m_HealthLabel != null)
		{
			int health = m_Health;
			if (health > 100)
				health = 100;
			if (health < 0)
				health = 0;
			m_HealthLabel.text = health.ToString() + "%";
			Debug.Log("health..........." + health + "%");
		}

	}

	protected override void OnMessage_HUDText(string message)
	{

		if (!ShowTips || m_HintsLabel == null)
			return;

		m_PickupMessageMobile = (string)message;
		m_HintsLabel.text = m_PickupMessageMobile;
		vp_UITween.ColorTo(HintsLabel, Color.white, .25f, m_HUDTextTweenHandle, delegate
		{
			vp_UITween.ColorTo(HintsLabel, m_InvisibleColorMobile, FadeDuration, m_HUDTextTweenHandle);
		});

	}
}