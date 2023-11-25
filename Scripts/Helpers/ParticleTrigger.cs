
using ActionTriggers;
using UnityEngine;

namespace Helpers
{
    public class ParticleTrigger: BaseTrigger
    {
        public enum Mode
        {
            Play, Emit
        }

        public Mode mode;
        [SerializeField] private ParticleSystem[] particles;
#if UNITY_EDITOR || ODIN_INSPECTOR
        [SerializeField] private int emitCount = 1;
#else
        [SerializeField] private int emitCount = 1;
#endif

        private void Awake()
        {
            particles = GetComponents<ParticleSystem>();
        }

#if UNITY_EDITOR || ODIN_INSPECTOR
        [ContextMenu("Trigger")]
        protected override void Trigger()
#else
        [ContextMenu("Trigger")]
        protected override void Trigger()
#endif
        {
            foreach (ParticleSystem system in particles)
            {
                switch (mode)
                {
                    case Mode.Play:
                        system.Play();
                        break;
                    case Mode.Emit:
                        system.Emit(emitCount);
                        break;
                }
            }
        }
    }
}
