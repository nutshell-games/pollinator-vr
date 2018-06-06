using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{

    public partial class TransformSwarm : SwarmController
    {
        public Transform[] transforms;
        Vector3[] velocities;

        protected override int GetAgents(Agent[] agents)
        {
            for (var i = 0; i < transforms.Length; i++)
            {
                agents[i].position = transforms[i].position;
                agents[i].velocity = velocities[i];
            }
            return transforms.Length;
        }

        protected override void SetAgents(Agent[] agents, int count)
        {
            for (var i = 0; i < count; i++)
            {
                velocities[i] = agents[i].velocity;
                transforms[i].position += velocities[i] * Time.deltaTime;
                transforms[i].LookAt(transforms[i].position + velocities[i]);
            }
        }

        void Start()
        {
            velocities = new Vector3[transforms.Length];
            Init(transforms.Length);
        }

        [ContextMenu("Add Children")]
        void AddChildren()
        {
            transforms = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                transforms[i] = transform.GetChild(i);
                transforms[i].position += Random.onUnitSphere * swarmRadius;
            }
        }


    }

}
