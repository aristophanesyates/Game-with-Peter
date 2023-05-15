using Knife.Interactions.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knife.Interactions.Variables
{
    [Serializable]
    public class Criteria
    {
        #region Variables

        public InteractionFact Fact;
        public Enum_FactOperator Operator;
        public int ComparisonValue;

        #endregion

        #region Validate Method
        public bool Validate()
        {
            switch (Operator)
            {
                case Enum_FactOperator.LessThen:
                    if (Fact.Value < ComparisonValue) { return true; }
                    else { return false; }
                case Enum_FactOperator.LessThenOrEqualTo:
                    if (Fact.Value <= ComparisonValue) { return true; }
                    else { return false; }
                case Enum_FactOperator.EqualTo:
                    if (Fact.Value == ComparisonValue) { return true; }
                    else { return false; }
                case Enum_FactOperator.MoreThenOrEqualTo:
                    if (Fact.Value >= ComparisonValue) { return true; }
                    else { return false; }
                case Enum_FactOperator.MoreThen:
                    if (Fact.Value > ComparisonValue) { return true; }
                    else { return false; }
                default:
                    Debug.LogError("FACT OPERATOR NULL");
                    return false;
            }
        }

        #endregion
    }
}