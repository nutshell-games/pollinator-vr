using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{

    public partial class RigidbodySwarm : SwarmController
    {
        public Rigidbody[] transforms;


        protected override int GetAgents(Agent[] agents)
        {
            for (var i = 0; i < transforms.Length; i++)
            {
                agents[i].position = transforms[i].position;
                agents[i].velocity = transforms[i].velocity;
            }
            return transforms.Length;
        }

        protected override void SetAgents(Agent[] agents, int count)
        {
            for (var i = 0; i < count; i++)
            {
                transforms[i].velocity = agents[i].velocity;
            }
        }

        void Start()
        {
            Init(transforms.Length);
        }

        [ContextMenu("Add Children")]
        void AddChildren()
        {
            transforms = new Rigidbody[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                transforms[i] = transform.GetChild(i).GetComponent<Rigidbody>();
                transforms[i].position += Random.onUnitSphere * swarmRadius;
            }
        }


    }

}
