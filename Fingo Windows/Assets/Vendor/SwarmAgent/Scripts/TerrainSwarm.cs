using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{

    public partial class TerrainSwarm : ParticleSwarm
    {
        public Terrain terrain;
        public float heightAboveTerrain = 0.1f;
        float yOffset = 0;

        protected override void SetAgents(Agent[] agents, int count)
        {
            for (var i = 0; i < count; i++)
            {
                var pos = agents[i].position;
                var terrainHeight = yOffset + terrain.SampleHeight(pos);
                pos.y = Mathf.Lerp(pos.y, terrainHeight + heightAboveTerrain, deltaTime * speed);
                agents[i].position = pos;
                particles[i].position = pos;
                particles[i].velocity = agents[i].velocity;
            }
            particleSystem.SetParticles(particles, count);
        }

        protected override void Start()
        {
            if (terrain != null)
                yOffset = terrain.transform.position.y;

            useVerticalAvoidance = false;
            base.Start();
        }



    }

}