using UnityEngine;
using MLSpace;

public class RagdollTest : MonoBehaviour
{
	private RagdollManagerHum m_Ragdoll;

	private float m_HitForce = 21f;

	private bool m_DoubleClick = false;
	private float m_DoubleClickMaxTime = 0.25f;
	private float m_DoubleClickCurTime = 0.0f;
	private int clickCount = 0;
	private Ray currentRay;
	public Ray CurrentRay
	{
		get { return currentRay; }
		set { currentRay = value; }
	}

	void Start()
	{
		m_Ragdoll = GetComponent<RagdollManagerHum>();

		Animator anim = GetComponent<Animator>();

		m_Ragdoll.OnHit = () =>
		{
			anim.applyRootMotion = false;
		};
		m_Ragdoll.LastEvent = () =>
		{
			anim.applyRootMotion = true;
		};


		m_Ragdoll.RagdollEventTime = 3.0f;
		m_Ragdoll.OnTimeEnd = () =>
		{
			m_Ragdoll.BlendToMecanim();
		};
	}

	public void doHitReaction()
	{
		int mask = LayerMask.GetMask("ColliderLayer", "ColliderInactiveLayer");
		RaycastHit rhit;
		if (Physics.Raycast(currentRay, out rhit, 120.0f, mask))
		{
			BodyColliderScript bcs = rhit.collider.GetComponent<BodyColliderScript>();
			int[] parts = new int[] { bcs.index };
			m_Ragdoll.StartHitReaction(parts, currentRay.direction * m_HitForce);
		}
	}

	public void doRagdoll()
	{
		int mask = LayerMask.GetMask("ColliderLayer", "ColliderInactiveLayer");
		RaycastHit rhit;
		if (Physics.Raycast(currentRay, out rhit, 120.0f, mask))
		{
			BodyColliderScript bcs = rhit.collider.GetComponent<BodyColliderScript>();
			int[] parts = new int[] { bcs.index };
			m_Ragdoll.StartRagdoll(parts, currentRay.direction * m_HitForce);
		}
	}
}