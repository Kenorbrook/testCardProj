using System;
using UnityEngine;

namespace TestGame
{
    [CreateAssetMenu(menuName = "Create Effect/Damage", fileName = "TakeDamageEffect", order = 0),Serializable]
    public class DealDamageEffect : IntegerEffect
    {
        public override string GetName()
        {
            return $"Deal {Value.ToString()} damage";
        }

        internal override void Resolve(RuntimeCharacter instigator, CharacterObject target)
        {
            var runtimeTarget = Target == EffectTargetType.Self ? instigator : target.Character;
            var targetHp = runtimeTarget.Hp;
            var targetShield = runtimeTarget.Shield;
            var hp = targetHp.Value;
            var shield = targetShield.Value;
            var damage = Value;
            

            if (damage >= shield)
            {
                var newHp = hp - (damage - shield);
                if (newHp < 0)
                    newHp = 0;
                targetHp.SetValue(newHp);
                targetShield.SetValue(0);
            }
            else
            {
                targetShield.SetValue(shield - damage);
            }
        }
    }
}
