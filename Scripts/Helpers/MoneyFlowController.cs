using System;
using System.Collections;
using UnityEngine;

namespace CorePublic.Helpers
{
    public class MoneyFlowController : Singleton<MoneyFlowController>
    {
        ParticleSystem _particleSystem;
        ParticleSystem.Particle[] m_Particles;
        Vector3[] _startPositions;
        public Vector2 ViewPortAim = new Vector2(.2f, .2f);
        private Camera main;
        public float flowStartLifeTime = 1f;
        public float planeDistance = 10f;
        public float deltaBetweenEmits = 1f;
        private bool _bursting;

        public Action moneyDestroyed;

        // Start is called before the first frame update
        private void Start()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            main = Camera.main;
        }

        // You can use ContextMenu or other default Unity ways to expose methods to the editor
        [ContextMenu("Burst")]
        public void Burst(Vector3 startPosition, int numberOfParticles)
        {
            transform.position = startPosition;
            if (deltaBetweenEmits <= 0)
            {
                _particleSystem.Emit(numberOfParticles);
            }
            else
            {
                StartCoroutine(_Burst(numberOfParticles, startPosition));
            }
        }

        IEnumerator _Burst(int numberOfParticles, Vector3 startPosition)
        {
            for (int i = 0; i < numberOfParticles; i++)
            {
                transform.position = startPosition;
                _particleSystem.Emit(1);
                yield return new WaitForSeconds(deltaBetweenEmits);
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            InitializeIfNeeded();

            // GetParticles is allocation free because we reuse the m_Particles buffer between updates
            int numParticlesAlive = _particleSystem.GetParticles(m_Particles);

            Vector3 aim = main.ViewportToWorldPoint(new Vector3(ViewPortAim.x, ViewPortAim.y, planeDistance));

            // Change only the particles that are alive
            for (int i = 0; i < numParticlesAlive; i++)
            {
                var particle = m_Particles[i];
                float particleLifeTime = particle.startLifetime - particle.remainingLifetime;

                if (particleLifeTime > flowStartLifeTime)
                {
                    if (_startPositions[i] == Vector3.zero)
                        _startPositions[i] = m_Particles[i].position;

                    var ratio = (particleLifeTime - flowStartLifeTime) / (particle.startLifetime - flowStartLifeTime);
                    m_Particles[i].position = Vector3.Lerp(_startPositions[i], aim, ratio);

                    if (m_Particles[i].remainingLifetime <= 0.05f)
                    {
                        _startPositions[i] = Vector3.zero;
                        m_Particles[i].remainingLifetime = 0;
                        moneyDestroyed?.Invoke();
                    }
                }
            }

            // Apply the particle changes to the Particle System
            _particleSystem.SetParticles(m_Particles, numParticlesAlive);
        }

        void InitializeIfNeeded()
        {
            if (_particleSystem == null)
                _particleSystem = GetComponent<ParticleSystem>();

            if (m_Particles == null || m_Particles.Length < _particleSystem.main.maxParticles)
            {
                m_Particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
                _startPositions = new Vector3[_particleSystem.main.maxParticles];
            }
        }
    }
}
