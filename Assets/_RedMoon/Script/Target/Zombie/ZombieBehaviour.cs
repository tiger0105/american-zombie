using UnityEngine;
using Polaris.GameData;
using MLSpace;

public enum BossAnimState
{
    IDLE = 0,
    WALK = 1,
    RANGEATTACK = 2,
    RUN = 3,
    MELEEATTACK = 4,
    HIT = 5,
    DIE = 6
}

public enum MainState{
	IDLE,
	MOVE,
	HURT,
	ATTACK,
	DEAD,
	SCREAM,
	COUNT
}
public enum MoveStates{
	WALK,
	RUN,
	STRAFE_LEFT,
	STRAFE_RIGHT,
	COUNT
}
[RequireComponent(typeof(Animator))]
public class ZombieBehaviour : MonoBehaviour
{
	public static int PreAtkingZombiCount = 0;
	public MonsterAI monAI;
	public Transform prefabBloodShoot;
	public GameObject nameTextObj;
	[HideInInspector] public bool isLover = false;
	public bool isFreezed = false;
	Color[] initColors;

	protected Transform target;
	protected float initSpeed;
	public vp_DamageHandler damageHandler;
	protected RagdollTest ragdollHandler;
	protected bool isPreAtking = false;

	protected MainState m_mainState;
	protected int m_subState;
	protected bool runFlag = false;
	protected bool screamFlag = false;
	protected bool attackFlag = false;
	protected bool deadFlag = false;
	protected int missionIndex;
	protected int missionNum;
	public MainState M_mainState
	{
		get { return m_mainState; }
		set { m_mainState = value; }
	}
	public int M_subState
	{
		get { return m_subState; }
		set { m_subState = value; }
	}
	public bool RunFlag
	{
		get { return runFlag; }
		set { runFlag = value; }
	}
	public bool ScreamFlag
	{
		get { return screamFlag; }
		set { screamFlag = value; }
	}
	public bool AttackFlag
	{
		get { return attackFlag; }
		set { attackFlag = value; }
	}
	public bool IsPreAtking
	{
		get { return isPreAtking; }
		set { isPreAtking = value; }
	}

	public Transform createPoint;
	protected Animator anim;
	public AudioClip zombieSound;
	public AudioClip zombieDieSound;
	protected Transform attackHand;
	private AnimatorStateInfo currentBaseState;
	public int headShoot = 100;
	public int upperLegShoot = 10;
	public int LegShoot = 5;
	public int TorsoShoot = 20;
	protected Transform shootRayOrigin;
	protected static int AttackState = Animator.StringToHash("Base Layer.Attack");
	protected string zombieName;
	protected Vector3 namePos;

	public void SetZombieName(string name)
	{
		if (GameSetting.nameOn == true) nameTextObj.GetComponent<TextMesh>().text = name;
	}

	protected virtual void Start()
	{
		int mission_index = MissionProgress.GetCurMissionType();
		float mission_rate = (float)MissionProgress.GetMissionCurIndex(mission_index) / MissionData.GetMissionCount(mission_index);
		initSpeed = 1f + Random.Range(0f, 1f) * mission_rate;
		anim = GetComponent<Animator>();
		anim.speed = initSpeed;
		GetComponent<AudioSource>().clip = zombieSound;
		if (GameManager.Instance.CurState != GameStates.PAUSE) GetComponent<AudioSource>().Play();
		damageHandler = GetComponent<vp_DamageHandler>();
		ragdollHandler = GetComponent<RagdollTest>();
		Collider[] colliders = GetComponentsInChildren<Collider>();
	}

	protected virtual void Update()
	{
		UpdateAnimation();
		monAI.transform.position = transform.position;
	}

	protected virtual void UpdateAnimation()
	{
		if (GameManager.Instance.CurState == GameStates.PAUSE)
		{
			if (anim.speed > 0) anim.speed = 0f;
			return;
		}

		if (GameManager.Instance.CurState == GameStates.PLAY)
			anim.speed = initSpeed;

		if (GameManager.Instance.CurState == GameStates.SLOWDOWN)
			anim.speed = initSpeed / GameManager.Instance.SLOWDOWN_RATE;

		SetTarget();

		transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
		if (m_mainState == MainState.DEAD)
		{
			if (anim.GetInteger("MainState") != (int)MainState.DEAD) anim.SetInteger("MainState", (int)MainState.DEAD);
		}
		else if (m_mainState == MainState.HURT)
		{
			anim.SetInteger("MainState", (int)MainState.HURT);
		}
		else if (isFreezed) { anim.speed = 0; return; }
		else if (Vector3.Distance(target.position, transform.position) > 10f)
		{
			m_mainState = MainState.MOVE;
			anim.speed = initSpeed * 3;
			transform.Translate(0.01f * anim.speed * Vector3.forward, Space.Self);
			if (anim.GetInteger("MainState") != (int)MainState.MOVE) anim.SetInteger("MainState", (int)MainState.MOVE);
		}
		else if (Vector3.Distance(target.position, transform.position) > 1.3f)
		{
			m_mainState = MainState.MOVE;
			anim.speed = initSpeed;
			transform.Translate(0.01f * anim.speed * Vector3.forward, Space.Self);
			if (anim.GetInteger("MainState") != (int)MainState.MOVE) anim.SetInteger("MainState", (int)MainState.MOVE);
		}
		else
		{
			m_mainState = MainState.ATTACK;
			if (anim.GetInteger("MainState") != (int)MainState.ATTACK) anim.SetInteger("MainState", (int)MainState.ATTACK);
		}
	}

	protected virtual void OnHitFromUFPS(DamageHit damagehit)
	{
		if (m_mainState != MainState.DEAD)
		{
			RaycastHit hit = damagehit.hit;
			float damage = damagehit.damage;
			Transform clone; // for the FX
			float damage_amount;

			if (hit.transform.tag.Contains("BOSS") == true)
			{
				return;
			}

			if (hit.collider.name.Contains("head"))
			{
				damage_amount = headShoot;
				GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeHeadShot", transform);
			}
			else if (hit.collider.name.Contains("leg") || hit.collider.name.Contains("arm"))
			{
				damage_amount = LegShoot;
				GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
			}
			else
			{
				damage_amount = TorsoShoot;
				GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
			}

			if (damageHandler != null)
			{
				damageHandler.Damage(damage_amount * damage);
				DoCheckDeath();
			}
		}
	}
	protected virtual void DoCheckDeath()
	{
		if (damageHandler.CurrentHealth > 0)
		{
			m_mainState = MainState.HURT;
		}
		else if (deadFlag == false)
		{
			deadFlag = true;
			m_mainState = MainState.DEAD;
			if (nameTextObj) nameTextObj.SetActive(false);
			GetComponent<AudioSource>().clip = null;
			if (GameManager.Instance.CurState != GameStates.PAUSE) GetComponent<AudioSource>().PlayOneShot(zombieDieSound);
			m_mainState = MainState.DEAD;
		}
	}
	public void ResetFlags()
	{
		runFlag = false;
		screamFlag = false;
		attackFlag = false;
	}

	protected void SetTarget()
	{
		if (target == null || !isLover)
			target = TargetManager.Instance.playerBase.transform;

		if (isLover && target.GetComponent<PlayerCtrl>())
		{
			float minDis = float.PositiveInfinity;

			foreach (var mon in RespawnControl.Instance.playingZombieList)
			{
				if (mon.Equals(this))
					continue;

				float dis = Vector3.Distance(transform.position, mon.transform.position);

				if (dis < minDis)
				{
					target = mon.transform;
					minDis = dis;
				}
			}
		}
	}

	void ChangeColor(bool isInit)
	{
		Renderer[] renderers = GetComponentsInChildren<Renderer>();

		if (initColors == null && !isInit)
		{
			initColors = new Color[renderers.Length];

			for (int i = 0; i < renderers.Length; i++)
				initColors[i] = renderers[i].material.color;
		}

		for (int i = 0; i < renderers.Length; i++)
		{
			if (isInit)
				renderers[i].material.color = initColors[i];
			else
				renderers[i].material.color = new Color(0.5f, 0.8f, 1, 1);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		ZombieBehaviour otherZombie = other.GetComponentInParent<ZombieBehaviour>();

		if (otherZombie == null || !otherZombie.isLover || otherZombie == this)
			return;

		if (other.transform.name.Contains("AttackPart"))
		{
			MonsterData md = MonsterData.GetMonsterData(other.gameObject.tag);
			damageHandler.Damage(md.Attack / 3);
		}

		if (other.gameObject.GetComponent<BodyColliderScript>() != null)
		{
			MonsterData md = MonsterData.GetMonsterData(other.GetComponent<BodyColliderScript>().ParentObject.name);
			damageHandler.Damage(md.Attack / 3);
		}
	}
}