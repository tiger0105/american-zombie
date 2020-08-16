using UnityEngine;
using System.Collections;
using Polaris.GameData;

public class MaleZombie : ZombieBehaviour
{
    private float m_MaxHealth = 100;
    private float m_MaxHealthScale = 1;
    private GameObject m_HealthBar;

    private bool isDead = false;

    private GameObject m_ScratchEffects;

    [Header("Explosion")]
    public GameObject intestinesA;
    public Transform intestinesARig;
    public GameObject intestinesB;
    public Transform intestinesBRig;
    public GameObject heart;
    public GameObject explodeFX;
    public Material intenstinesMaterial;
    public Material heartMaterial;

    protected override void Start()
    {
        //increate the speed of zombie as the game progresses
        missionIndex = MissionProgress.GetCurMissionType();
        float mission_rate = (float)MissionProgress.GetMissionCurIndex(missionIndex) / MissionData.GetMissionCount(missionIndex);
        initSpeed = 1;
        
        missionNum = MissionProgress.GetMissionCurIndex(missionIndex);

        anim = GetComponent<Animator>();
        anim.speed = initSpeed;
        m_mainState = MainState.IDLE;
        //damage handler
        damageHandler = GetComponent<vp_DamageHandler>();

        if (GameSetting.soundOn == false)
        {
            GetComponent<AudioSource>().mute = true;
        }
        else
        {
            GetComponent<AudioSource>().mute = false;
        }

        m_MaxHealth = GetComponent<vp_DamageHandler>().MaxHealth;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "Static")
            {
                m_HealthBar = gameObject.transform.GetChild(i).GetChild(0).gameObject;
                m_MaxHealthScale = m_HealthBar.transform.localScale.x;
                break;
            }
        }

        Color color = intenstinesMaterial.color;
        color.a = 1.0f;
        intenstinesMaterial.color = color;
        heartMaterial.color = color;

        m_ScratchEffects = GameObject.Find("ScratchEffects");
    }

    protected override void UpdateAnimation()
    {
        if (Time.timeScale == 0)
            return;

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

        if (GameManager.Instance.CurState == GameStates.PAUSE)
        {
            if (anim.speed > 0) 
                anim.speed = 0f;
            
            return;
        }

        if (GameManager.Instance.CurState == GameStates.PLAY)
            anim.speed = 1;

        if (GameManager.Instance.CurState == GameStates.SLOWDOWN)
            anim.speed = 1 / GameManager.Instance.SLOWDOWN_RATE;

        SetTarget();

        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

        if (m_mainState == MainState.DEAD)
        {
            if (isDead == true)
                return;

            anim.speed = 1;

            anim.ResetTrigger("Idle");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Hit");

            anim.SetTrigger("Die");

            isDead = true;

            vp_ItemIdentifier identifier = GameObject.FindWithTag("Player").GetComponent<vp_PlayerInventory>().CurrentWeaponIdentifier;
            int weapon_id = int.Parse(identifier.name.Substring(0, 1)) - 1;
            int inventory_id = WeaponInventory.GetInventoryID(weapon_id);
            int newLevel = WeaponInventory.levelList[inventory_id] - 1;
            if (newLevel > 0)
                StartCoroutine("Explosion");
        }
        else if (m_mainState == MainState.HURT)
        {
            anim.speed = 1;

            anim.ResetTrigger("Idle");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Hit");
            anim.ResetTrigger("Die");

            m_mainState = MainState.IDLE;
        }
        else if (isFreezed) { anim.speed = 0; return; }
        else if (Vector3.Distance(target.position, transform.position) > 1.3f)
        {
            transform.Translate(0.025f * anim.speed * Vector3.forward, Space.Self);
            anim.ResetTrigger("Idle");
            if (m_mainState != MainState.MOVE)
                anim.SetTrigger("Walk");
            anim.ResetTrigger("Attack");
            anim.ResetTrigger("Hit");
            anim.ResetTrigger("Die");

            m_mainState = MainState.MOVE;
        }
        else
        {
            if (anim.speed > 1f)
            {
                anim.speed = 1f;
            }
            
            if (m_mainState != MainState.ATTACK)
            {
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Walk");
                anim.SetTrigger("Attack");
                anim.ResetTrigger("Hit");
                anim.ResetTrigger("Die");
            }

            m_mainState = MainState.ATTACK;

            //m_ScratchEffects.GetComponent<ScratchRandom>().Change();
            if (!m_ScratchEffects.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Scratch"))
                m_ScratchEffects.GetComponent<Animator>().SetTrigger("Scratch");
        }
    }

    private IEnumerator Explosion()
    {
        explodeFX.SetActive(true);
        intestinesA.SetActive(true);

        intestinesARig.GetComponent<Rigidbody>().AddForce(Random.Range(0, 10), Random.Range(25, 50), Random.Range(0, 10), ForceMode.Impulse);
        intestinesBRig.GetComponent<Rigidbody>().AddForce(Random.Range(0, 10), Random.Range(50, 75), Random.Range(0, 10), ForceMode.Impulse);
        heart.GetComponent<Rigidbody>().AddForce(Random.Range(0, 5), Random.Range(10, 20), Random.Range(0, 5), ForceMode.Impulse);

        yield return new WaitForSeconds(4.7f);

        StartCoroutine("RemoveDynamicObjects");

        yield return new WaitForSeconds(2.0f);
        explodeFX.SetActive(false);
    }

    IEnumerator RemoveDynamicObjects()
    {
        while ((intenstinesMaterial.color.a >= 0))
        {
            Color colorA = intenstinesMaterial.color;
            Color colorB = heartMaterial.color;

            if (colorA.a >= 0)
            {
                colorA.a -= 0.05f;
                colorB.a -= 0.05f;
                intenstinesMaterial.color = colorA;
                heartMaterial.color = colorB;
            }

            yield return 0;
        }
    }
}
