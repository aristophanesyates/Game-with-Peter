using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Interactions
{
    [Serializable]
    [CreateAssetMenu(fileName = "Fact", menuName = "Interactions/Fact", order = 3)]
    public class InteractionFact : BaseEntry
    {
        #region Variables

        public int Value;

        #endregion
    }
}