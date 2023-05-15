using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Interactions.Listeners
{
    [RequireComponent(typeof(Collider))]
    public class ColliderInteractionListeners : InteractionEventListener
    {
        #region Variables

        private Collider m_Collider;

        #endregion

        #region Init

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
        }

        #endregion

        #region Event Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponentInParent<PlayerMovement>())
            {
                RaiseEvents();
            }
        }

        #endregion
    }
}