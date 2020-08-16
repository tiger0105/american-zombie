using UnityEngine;

namespace MLSpace
{
    public class BodyColliderScript : ColliderScript
    {
        public bool critical = false;                   // you can apply additional damage if critial
        public BodyParts bodyPart = BodyParts.None;    // collider body part
        public int index = -1;                          // index of collider

        [SerializeField, HideInInspector]
        private RagdollManager m_ParentRagdollManager;  // reference to parents ragdollmanager script

        public void SetParentRagdollManager(RagdollManager rm)
        {
            m_ParentRagdollManager = rm;
        }

        public RagdollManager ParentRagdollManager
        {
            get { return m_ParentRagdollManager; }
        }
    }
}