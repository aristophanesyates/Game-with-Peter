using System.Collections.Generic;
using UnityEngine;

namespace Knife.Interactions
{
    [CreateAssetMenu(fileName = "Event", menuName = "Interactions/Event", order = 1)]
    public class InteractionEvent : BaseEntry
    {
        #region Variables

        private List<InteractionEventListener> listeners = new List<InteractionEventListener>();

        #endregion

        #region Raise Method

        public void Raise()
        {
            foreach(InteractionEventListener listener in listeners)
            {
                listener.OnEventRaised();
            }
        }

        #endregion

        #region Un/Register Methods

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