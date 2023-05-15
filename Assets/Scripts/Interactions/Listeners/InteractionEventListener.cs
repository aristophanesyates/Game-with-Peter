using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knife.Interactions
{
    public class InteractionEventListener : MonoBehaviour
    {
        #region Variables

        public InteractionRule Rule;
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
            Rule.RegisterListener(this);
        }

        private void OnDisable()
        {
            Rule.UnregisterListener(this);
        }

        #endregion
    }
}