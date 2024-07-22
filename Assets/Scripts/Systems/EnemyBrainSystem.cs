using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestGame
{
    public class EnemyBrainSystem : BaseSystem
    {
        private int currentPatternIdx;
        private int currentCount;

        private bool isThinking;
        private float accTime;
        private const float ThinkingTime = 1.0f;

        private int step = 0;

        [SerializeField]
        private Pattern pattern;

        public void OnEnemyTurnBegan()
        {
            isThinking = true;
        }

        private void Update()
        {
            if (!isThinking) return;

            accTime += Time.deltaTime;
            if (accTime >= ThinkingTime)
            {
                isThinking = false;
                accTime = 0.0f;
                MakeStep();
            }
        }

        private void MakeStep()
        {
            IntegerEffect selectedEffect;
            if (pattern.isRandomPattern)
            {
                var randomIdx = Random.Range(0, pattern.Effects.Count);
                selectedEffect = pattern.Effects[randomIdx];
            }
            else
            {
                if (pattern.Effects.Count == step)
                    step = 0;
                selectedEffect = pattern.Effects[step++];
            }

            List<IntegerEffect> currentEffects = new() {selectedEffect};


            foreach (var effect in currentEffects)
            {
                effect.Resolve(Enemy.First().Character, Player);
            }
        }
    }
}