using Knife.Interactions.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Interactions
{
    [CreateAssetMenu(fileName = "Rule", menuName = "Interactions/Rule", order = 2)]
    public class InteractionRule : BaseEntry
    {
        #region Variables

        private List<InteractionEventListener> listeners = new List<InteractionEventListener>();

        public Enum_Priority Priority;
        public bool TriggerRuleOnce;
        private bool IsTriggered;

        public InteractionEvent TriggeredBy;
        public InteractionEvent Triggers;

        public List<Criteria> criteria;
        public List<Modifications> modifications;

        #endregion

        #region Validation Methods

        public bool ValidateCriteria()
        {
            if (criteria.Count != 0)
            {
                foreach (Criteria c in criteria)
                {
                    if (!c.Validate())
                    {
                        return false;
                    }
                }
            }
            if (TriggerRuleOnce == true
                && IsTriggered)
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Trigger From Event

        public void RaiseEvent()
        {
            if (ValidateCriteria())
            {
                // Raise each listener
                foreach (InteractionEventListener listener in listeners)
                {
                    listener.OnEventRaised();
                }

                // Modify facts
                foreach (Modifications mod in modifications)
                {
                    mod.ChangeValue();
                }

                // Null check
                if (Triggers != null)
                {
                    // Raise the trigger event
                    Triggers.Raise();
                }
                IsTriggered = true;
            }
        }

        #endregion

        #region Un/Register Listeners

        public void RegisterListener(InteractionEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(InteractionEventListener listener)
        {
            listeners.Remove(listener);
        }

        #endregion
    }
}