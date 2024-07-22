namespace TestGame
{
    public static class CardUtils
    {
        internal static bool CardCanBePlayed(CardTemplate card, IntVariable playerMana)
        {
            return card.Cost <= playerMana.Value;
        }

        internal static bool CardHasTargetableEffect(CardTemplate card)
        {
            foreach (var effect in card.Effects)
            {
                if (effect is TargetEffect targetableEffect)
                {
                    if (targetableEffect.Target == EffectTargetType.TargetEnemy)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
