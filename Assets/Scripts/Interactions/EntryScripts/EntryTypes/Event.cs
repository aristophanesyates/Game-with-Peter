using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Knife.Interactions
{
    [CreateAssetMenu(fileName = "Event", menuName = "Interactions/Event", order = 1)]
    public class InteractionEvent : BaseEntry
    {
        #region Variables

        public List<InteractionRule> rules = new List<InteractionRule>();

        #endregion

        #region Raise Method

        public void Raise()
        {
            InteractionRule ruleToTrigger = null;
            foreach (InteractionRule rule in rules)
            {
                if (rule.ValidateCriteria())
                {
                    // If There is rule to trigger & the new rule is less specific then the current rule...
                    // Do Nothing
                    if (ruleToTrigger != null
                        && rule.criteria.Count < ruleToTrigger.criteria.Count)
                    {
                        continue;
                    }

                    // If there is rule to trigger & both rules are as specific, but the new rule is higher priority...
                    // Replace the rule
                    else if ((ruleToTrigger != null
                        && rule.criteria.Count == ruleToTrigger.criteria.Count)
                        && (rule.Priority > ruleToTrigger.Priority))
                    {
                        ruleToTrigger = rule;
                    }

                    // If there is rule to trigger & both rules are as specific, but the old rule is higher priority...
                    // Do nothing
                    else if (ruleToTrigger != null
                        && rule.criteria.Count == ruleToTrigger.criteria.Count
                        && (rule.Priority < ruleToTrigger.Priority))
                    {
                        continue;
                    }

                    // If there is rule to trigger & both rules are as specific & both rules are the same priority...
                    // Randomise rule
                    else if (ruleToTrigger != null
                        && (rule.criteria.Count == ruleToTrigger.criteria.Count)
                        && rule.Priority == ruleToTrigger.Priority)
                    {
                        int rand = Random.Range(0, 1);
                        if (rand == 0)
                        {
                            ruleToTrigger = rule;
                        }
                    }

                    // If everything fails (there is no rule to trigger)
                    // Replace
                    else
                    {
                        ruleToTrigger = rule;
                    }
                }
            }
            ruleToTrigger.RaiseEvent();
        }

        #endregion
    }
}