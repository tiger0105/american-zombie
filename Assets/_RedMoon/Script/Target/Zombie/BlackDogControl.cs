using UnityEngine;
using Polaris.GameData;

public class BlackDogControl : ZombieBehaviour
{
    bool isBarking;
    public bool IsBarking
    {
        get { return isBarking; }
        set { isBarking = value; }
    }

    bool isDead = false;
    bool isAtking = false;
    bool iswalking = false;
    
    bool isHurting = false;
    public float normalY;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public bool IsAtking
    {
        get { return isAtking; }
        set { isAtking = value; }
    }
    public bool Iswalking
    {
        get { return iswalking; }
        set { iswalking = value; }
    }
    public bool IsHurting
    {
        get { return isHurting; }
        set { isHurting = value; }
    }

    private float m_MaxHealth = 100;
    private float m_MaxHealthScale = 1;
    [SerializeField] private GameObject m_HealthBar;

    protected override void Start()
    {
        //increate the speed of zombie as the game progresses
        int mission_index = MissionProgress.GetCurMissionType();
        float mission_rate = (float)MissionProgress.GetMissionCurIndex(mission_index) / MissionData.GetMissionCount(mission_index);
        initSpeed = 3f + Random.Range(0f, 7f) * mission_rate;

        anim = GetComponent<Animator>();
        anim.speed = initSpeed;

        GetComponent<AudioSource>().clip = zombieSound;
        if (GameManager.Instance.CurState != GameStates.PAUSE) GetComponent<AudioSource>().Play();
        ragdollHandler = GetComponent<RagdollTest>();
        damageHandler = GetComponent<vp_DamageHandler>();

        m_MaxHealth = GetComponent<vp_DamageHandler>().MaxHealth;

        if (GameSetting.soundOn == false)
        {
            GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "Static")
            {
                m_MaxHealthScale = m_HealthBar.transform.localScale.x;
                break;
            }
        }
    }

    protected override void UpdateAnimation()
    {
        if (m_HealthBar != null)
        {
            float currentHealth = GetComponent<vp_DamageHandler>().CurrentHealth;
            if (currentHealth <= 0)
            {
                m_HealthBar.transform.localScale = new Vector3(0, m_HealthBar.transform.localScale.y, m_HealthBar.transform.localScale.z);
            }
            else
            {
                m_HealthBar.transform.localScale = new Vector3(currentHealth / m_MaxHealth * m_MaxHealthScale, m_HealthBar.transform.localScale.y, m_HealthBar.transform.localScale.z);
            }
        }

        DoCheckDeath();
        //Check Game is play/pause
        if (GameManager.Instance.CurState == GameStates.PAUSE)
        {
            if (anim.speed > 0) anim.speed = 0f;
            return;
        }

        if (GameManager.Instance.CurState == GameStates.PLAY)
            anim.speed = initSpeed;

        if (GameManager.Instance.CurState == GameStates.SLOWDOWN)
            anim.speed = initSpeed / GameManager.Instance.SLOWDOWN_RATE;

        ////////////**********zombie tatics************//////////
        SetTarget();
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        if (IsDead == true)
        {
            anim.SetBool("die", true);
            anim.SetBool("idle", false);
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
            anim.SetBool("bark", false);
            anim.SetBool("attack", false);
            anim.SetBool("hurt", false);
        }
        else if (IsHurting == true)
        {
            //Check death
            DoCheckDeath();
            anim.SetBool("die", false);
            anim.SetBool("idle", false);
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
            anim.SetBool("bark", false);
            anim.SetBool("attack", false);
            anim.SetBool("hurt", true);
            IsHurting = false;
        }
        else if (isFreezed)
        {
            anim.speed = 0;
            return;
        }
        else if (Vector3.Distance(target.position, transform.position) > 10f)
        {
            anim.speed = initSpeed;
            transform.Translate(0.08f * Vector3.forward, Space.Self);
            anim.SetBool("die", false);
            anim.SetBool("idle", false);
            anim.SetBool("run", true);
            anim.SetBool("walk", false);
            anim.SetBool("bark", false);
            anim.SetBool("attack", false);
            anim.SetBool("hurt", false);
        }
        else if (Vector3.Distance(target.position, transform.position) > 1.5f)
        {
            anim.speed = initSpeed;
            normalY = transform.position.y;
            transform.Translate(0.02f * Vector3.forward, Space.Self);
            anim.SetBool("die", false);
            anim.SetBool("idle", false);
            anim.SetBool("run", false);
            anim.SetBool("walk", true);
            anim.SetBool("bark", false);
            anim.SetBool("attack", false);
            anim.SetBool("hurt", false);
        }
        else if (IsAtking == true)
        {
            anim.SetBool("die", false);
            anim.SetBool("idle", false);
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
            anim.SetBool("bark", false);
            anim.SetBool("attack", true);
            anim.SetBool("hurt", false);
        }
        else
        {
            anim.SetBool("die", false);
            anim.SetBool("idle", false);
            anim.SetBool("run", false);
            anim.SetBool("walk", false);
            anim.SetBool("bark", true);
            anim.SetBool("attack", false);
            anim.SetBool("hurt", false);
        }
    }

    protected override void OnHitFromUFPS(DamageHit damagehit)
    {
        RaycastHit hit = damagehit.hit;
        float damage = damagehit.damage;

        Transform clone; // for the FX

        IsHurting = true;
        float damage_amount;
        switch (hit.collider.transform.name)
        {
            case "Head":
                damage_amount = headShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeHeadShot", transform);
                break;
            case "Spine":
                damage_amount = TorsoShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "LeftUpLeg":
                damage_amount = upperLegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "LeftLeg":
                damage_amount = LegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "RightUpLeg":
                damage_amount = upperLegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            case "RightLeg":
                damage_amount = LegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
            default:
                damage_amount = upperLegShoot;
                GameObject.FindGameObjectWithTag("CombatObserver").GetComponent<CombatObserver>().SendMessage("OnTakeBodyShot", transform);
                break;
        }

        if (damageHandler != null)
            damageHandler.Damage(damage_amount * damage);

        shootRayOrigin = Camera.main.transform;
        clone = Instantiate(prefabBloodShoot, hit.point, shootRayOrigin.rotation) as Transform;
        clone.LookAt(shootRayOrigin.position);
        Destroy(clone.gameObject, 0.3f);

        if (ragdollHandler != null)
        {
            Vector3 dir = hit.point - Camera.main.transform.position;
            Ray ray = new Ray(Camera.main.transform.position, dir * 1000);
            ragdollHandler.CurrentRay = ray;

            if (damageHandler.CurrentHealth > 0)
                ragdollHandler.doHitReaction();
            else
                ragdollHandler.doRagdoll();
        }
    }

    protected override void DoCheckDeath()
    {
        if (damageHandler.CurrentHealth <= 0 && isDead == false)
        {
            isDead = true;
            GetComponent<CharacterController>().enabled = false;

            foreach (Collider child in transform.GetComponentsInChildren<Collider>())
                if (nameTextObj)
                    nameTextObj.SetActive(false);

            GetComponent<AudioSource>().clip = null;

            if (GameManager.Instance.CurState != GameStates.PAUSE)
                GetComponent<AudioSource>().PlayOneShot(zombieDieSound);

            isDead = true;
        }
    }
}