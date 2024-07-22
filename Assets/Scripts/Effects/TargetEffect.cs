namespace TestGame
{
    public abstract class TargetEffect : Effect
    {
        public EffectTargetType Target;
        
        internal abstract void Resolve(RuntimeCharacter instigator, CharacterObject target);
    }
}
