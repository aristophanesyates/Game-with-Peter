using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knife.Interactions
{
    public class InteractionEventListener : MonoBehaviour
    {
        #region Variables

        public InteractionEvent Event;
        public UnityEvent Response;

        #endregion

        #region Event Methods

        public virtual void RaiseEvents()
        {
            Event.Raise();
        }

        public virtual void OnEventRaised()
        {
            Response.Invoke();
        }

        #endregion

        #region Un/Register Observer Methods
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        #endregion
    }
}