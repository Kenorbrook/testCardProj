using System;
using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(menuName = "Create Effect/Shield", fileName = "GainShieldEffect", order = 2),Serializable]
    public class GainShieldEffect : IntegerEffect
    {
        public override string GetName()
        {
            return $"Gain {Value.ToString()} Shield";
        }

        internal override void Resolve(RuntimeCharacter instigator, CharacterObject target)
        {
            var runtimeTarget = Target == EffectTargetType.Self ? instigator : target.Character;
            var targetShield = runtimeTarget.Shield;
            targetShield.SetValue(targetShield.Value + Value);
        }
    }
}
