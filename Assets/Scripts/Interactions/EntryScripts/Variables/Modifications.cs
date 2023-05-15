using Knife.Interactions.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Interactions.Variables
{
    [Serializable]
    public class Modifications
    {
        #region Variables

        public InteractionFact Fact;
        public Enum_FactModifier Operator;
        public int Value;

        #endregion

        #region Change Fact Value Method

        public void ChangeValue()
        {
            switch (Operator)
            {
                case Enum_FactModifier.DecrementBy:
                    Fact.Value = Fact.Value - Value;
                    break;
                case Enum_FactModifier.EqualTo:
                    Fact.Value = Value;
                    break;
                case Enum_FactModifier.IncrementBy:
                    Fact.Value = Value + Fact.Value;
                    break;
                default:
                    Debug.LogError("FACT MODIFIER NOT ASSIGNED");
                    break;
            }
        }

        #endregion
    }
}