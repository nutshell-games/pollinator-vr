using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{

    [RequireComponent(typeof(ParticleSystem))]
    public partial class ParticleSwarm : SwarmController
    {

        public new ParticleSystem particleSystem;
        protected ParticleSystem.Particle[] particles;
        protected int particleCount = 0;

        protected override int GetAgents(Agent[] agents)
        {
            particleCount = particleSystem.GetParticles(particles);
            for (var i = 0; i < particleCount; i++)
            {
                agents[i].position = particles[i].position;
                agents[i].velocity = particles[i].velocity;
            }
            return particleCount;
        }

        protected override void SetAgents(Agent[] agents, int count)
        {
            for (var i = 0; i < count; i++)
            {
                particles[i].velocity = agents[i].velocity;
            }
            particleSystem.SetParticles(particles, count);
        }

        protected virtual void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
            Init(particleSystem.main.maxParticles);
        }


    }

}