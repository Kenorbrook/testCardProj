using System;
using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(menuName = "Create Effect/Heal", fileName = "GainHealEffect", order = 1),Serializable]
    public class GainHealEffect : IntegerEffect
    {
        public override string GetName()
        {
            return $"Gain {Value.ToString()} Heal";
        }

        internal override void Resolve(RuntimeCharacter instigator, CharacterObject target)
        {
            var runtimeTarget = Target == EffectTargetType.Self ? instigator : target.Character;
            var targetHp = runtimeTarget.Hp;
            int sum = targetHp.Value + Value;
            int hp = sum > runtimeTarget.MaxHp ? runtimeTarget.MaxHp : sum;
            targetHp.SetValue(hp);
        }

    }
}