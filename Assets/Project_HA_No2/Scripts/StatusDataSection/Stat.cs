using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HA
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private int baseValue;

        public List<int> modifiers;
        public int GetValue()
        {
            int finalValue = baseValue;
            foreach(int modifier in modifiers)
            {
                finalValue += modifier;
            }

            return finalValue;
        }

        public void SetDefaultValue(int value)
        {
            baseValue = value;
        }

        public void AddModifier(int _mod)
        {
            modifiers.Add(_mod);
        }

        public void RemoveModifier(int _mod)
        {
            modifiers.Remove(_mod);
        }
    }
}
