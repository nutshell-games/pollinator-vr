using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Swarm
{

    public abstract class SwarmController : MonoBehaviour
    {
        public struct Agent
        {
            public Vector3 position;
            public Vector3 velocity;
        }

        public Vector3 windDirection;
        public float windPower;

        [Header("Object Effectors")]
        public Transform focus;

        [Header("Separation Effector")]
        public float separation = 0.5f;
        public float separationWeight = 1f;
        public float cohesionWeight = 1f;

        [Header("Alignment Effector")]
        public float alignmentWeight = 1f;

        [Header("Boundary Attraction")]
        public float swarmRadius = 2;
        public float boundsWeight = 1f;

        [Header("Vertical Avoidance")]
        public bool useVerticalAvoidance = true;
        public Transform floorTransform;
        public float relativeFloorHeight = 0;
        public float floorWeight = 2;
        public float relativeCeilingHeight = 2;
        public float ceilingWeight = 2;

        [Header("Object Avoidance")]
        public Transform[] avoid;
        public float avoidDistance = 1f;
        public float avoidWeight = 1f;

        [Header("Object Attraction")]
        public Transform[] attract;
        public float minAttractionRadius = 0.5f;
        public float maxAttractionRadius = 2f;
        public float attractWeight = 1f;

        [Header("Movement Controls")]
        public float speed = 1f;
        public float maxSteer = 1f;

        [Header("Optimisation")]
        public float neighborhoodRadius = 1f;
        public int maxNeighbors = 15;

        SpatialHash<Agent> spatialHash;

        List<Worker<Agent>> workers = new List<Worker<Agent>>();
        protected Vector3 focusPosition, floorPosition;
        protected Vector3[] avoidPositions, attractPositions;
        protected float deltaTime;
        protected Agent[] agents;
        int threadCount = 1;

        protected abstract int GetAgents(Agent[] agents);

        protected abstract void SetAgents(Agent[] agents, int count);


        void OnDrawGizmosSelected()
        {
            if (focus != null)
                Gizmos.DrawWireSphere(focus.position, swarmRadius);

            var floor = focus == null ? transform.position : focus.position;

            floor.y = floorTransform == null ? transform.position.y : floorTransform.position.y;
            Gizmos.DrawWireCube(floor + Vector3.up * relativeFloorHeight, new Vector3(swarmRadius * 2, 0, swarmRadius * 2));
            Gizmos.DrawWireCube(floor + Vector3.up * relativeCeilingHeight, new Vector3(swarmRadius * 2, 0, swarmRadius * 2));
            Gizmos.color = Color.red;
            if (avoid != null)
                foreach (var t in avoid)
                    Gizmos.DrawWireSphere(t.position, avoidDistance);
            if (attract != null)
                foreach (var t in attract)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(t.position, minAttractionRadius);
                    Gizmos.color = Color.green / 2;
                    Gizmos.DrawWireSphere(t.position, maxAttractionRadius);
                }
        }

        protected void Init(int maxAgents)
        {
            threadCount = Mathf.Max(1, SystemInfo.processorCount - 1);
            spatialHash = new SpatialHash<Agent>(Mathf.FloorToInt(neighborhoodRadius * 2), maxAgents, maxAgents);
            for (var i = 0; i < threadCount; i++)
            {
                workers.Add(new Worker<Agent>());
            }
            agents = new Agent[maxAgents];
            StartCoroutine(UpdateSwarm());
        }

        void OnDisable()
        {
            foreach (var w in workers) w.Terminate();
        }

        void HashAgents(int offset, int count)
        {
            for (var i = offset; i < offset + count; i++)
                spatialHash.Insert(agents[i].position, agents[i]);
        }

        void UpdateAgents(int offset, int count)
        {
            if (count == 0) return;
            for (var i = offset; i < offset + count; i++)
            {
                agents[i] = UpdateAgent(agents[i], spatialHash.Query(agents[i].position));
            }
        }

        IEnumerator UpdateSwarm()
        {
            avoidPositions = new Vector3[avoid.Length];
            attractPositions = new Vector3[attract.Length];
            while (true)
            {
                yield return null;
                var activeAgentCount = GetAgents(agents);
                if (activeAgentCount == 0) continue;
                spatialHash.Clear();
                focusPosition = focus.position;
                deltaTime = Time.deltaTime;
                for (var i = 0; i < avoid.Length; i++)
                    avoidPositions[i] = avoid[i].position;
                for (var i = 0; i < attract.Length; i++)
                    attractPositions[i] = attract[i].position;
                floorPosition = floorTransform.position;
                spatialHash.maxPerCell = maxNeighbors;
                HashAgents(0, activeAgentCount);
                var count = activeAgentCount / threadCount;
                var remainder = 0;

                for (var i = 0; i < threadCount; i++)
                {
                    var w = workers[i];
                    if (i == threadCount - 1)
                        w.Run(UpdateAgents, i * count, remainder);
                    else
                        w.Run(UpdateAgents, i * count, count);
                    remainder = activeAgentCount - (i * count + count);
                }

                for (var i = 0; i < threadCount; i++)
                    if (!workers[i].Wait(1000))
                    {
                        workers[i].Terminate();
                        throw new System.InvalidOperationException("Task did not finish");
                    }

                SetAgents(agents, activeAgentCount);
            }
        }

        protected virtual Agent UpdateAgent(Agent agent, List<Agent> localAgents)
        {
            var separationF = Vector3.zero;
            var alignmentF = Vector3.zero;
            var cohesionF = Vector3.zero;
            var boundsF = Vector3.zero;
            var avoidF = Vector3.zero;
            var agentPosition = agent.position;
            var focusDelta = focusPosition - agentPosition;
            var swarmRadius2 = swarmRadius * swarmRadius;
            var avoidDistance2 = avoidDistance * avoidDistance;
            var minAttractionRadius2 = minAttractionRadius * minAttractionRadius;
            var maxAttractionRadius2 = maxAttractionRadius * maxAttractionRadius;

            if (focusDelta.sqrMagnitude > swarmRadius2)
                boundsF = focusDelta / swarmRadius;

            if (useVerticalAvoidance)
            {
                var floorHeight = agentPosition.y - (floorPosition.y + relativeFloorHeight);
                floorHeight = floorHeight > 0 ? 0 : floorHeight;
                avoidF += Vector3.up * floorWeight * (-floorHeight);
                var ceilingHeight = agentPosition.y - (floorPosition.y + relativeCeilingHeight);
                ceilingHeight = ceilingHeight < 0 ? 0 : ceilingHeight;
                avoidF += Vector3.up * ceilingWeight * (-ceilingHeight);
            }

            for (var k = 0; k < avoidPositions.Length; k++)
            {
                var d = (avoidPositions[k] - agentPosition);
                if (d.sqrMagnitude < avoidDistance2)
                {
                    var distanceToAvoid = d.magnitude;
                    var direction = -d / distanceToAvoid;
                    var power = avoidDistance / distanceToAvoid;
                    avoidF += direction * power;
                }
            }

            for (var k = 0; k < attractPositions.Length; k++)
            {
                var d = (attractPositions[k] - agentPosition);
                var sqrMagnitude = d.sqrMagnitude;
                if (sqrMagnitude > minAttractionRadius2 && sqrMagnitude < maxAttractionRadius2)
                {
                    var distanceToAttract = d.magnitude;
                    var direction = d / distanceToAttract;
                    var power = maxAttractionRadius / distanceToAttract;
                    avoidF += direction * power;
                }
            }

            if (localAgents.Count > 0)
            {
                var alignmentCoeff = 1f / localAgents.Count;
                var stride = localAgents.Count / maxNeighbors;
                stride = stride < 1 ? 1 : stride;
                for (var k = 0; k < localAgents.Count; k += stride)
                {
                    var other = localAgents[k];
                    var otherPosition = other.position;
                    if (agentPosition == otherPosition) continue;
                    var otherVelocity = other.velocity;
                    alignmentF += otherVelocity;
                    var delta = agentPosition - otherPosition;
                    var distanceToOther = delta.magnitude;
                    var direction = delta / distanceToOther;
                    float power = separation / distanceToOther;
                    if (distanceToOther < separation)
                    {
                        separationF += (separationWeight * direction * power);
                    }
                    else
                    {
                        separationF -= (cohesionWeight * direction * power);
                    }
                }
                separationF *= alignmentCoeff;
                alignmentF *= alignmentCoeff;
            }

            var newVelocity = Vector3.zero;
            newVelocity += separationF;
            newVelocity += alignmentF * alignmentWeight;
            newVelocity += boundsF * boundsWeight;
            newVelocity += avoidF * avoidWeight;

            newVelocity = newVelocity * deltaTime * maxSteer;
            agent.velocity = Vector3.ClampMagnitude(agent.velocity + newVelocity, speed) + (windDirection * windPower * deltaTime);
            return agent;
        }

    }

}