SwarmAgent is an implementation of the Boids flocking algorithm. 

It is multithreaded and uses a data oriented design. Up to 50000 agents have been
simulated at 30fps on a Nvidia GT970.

The first step:
Create a new component which will contain the swarm, and assign the ParticleSwarm
component. This component has several parameters you will need to adjust. You can 
also use TransformSwarm to control transforms, or RigidbodySwarm to controls 
Rigidbodies.

Parameters:

Object Effectors:
    Focus: The transform that the boids will be attracted to.

Separation Effector:
    Separation: The distance between each boid.
    Separation Weight: How hard the boid tries to keep away from others.
    Cohesion Weight: How hard the boid tries to stay close to others.

Alignment Effector:
    Alignment Weight: How hard the boid tries to travel in the same direction
    as others.

Boundary Attraction:
    Swarm Radius: The radius around the focus which the boid will stay within.
    Boundary Weight: How hard the boid tries to stay within the radius.

Floor Avoidance:
    Floor: The gameobject representing the floor.
    Floor Height: The distance above the floor that the boid will stay above.
    Floor Weight: How hard the boid will try not to fly through the floor.

Object Avoidance:
    Avoid: A list of transforms that the boids will avoid.
    Avoid Distance: How far away from the gameobject the boid stay.
    Avoid Weight: How hard the boid will try to maintain the distance.

Movement Controls:
    Speed: The max velocity of the boid.
    Max Steer: The maximimum change in velocity direction per second.

Optimisation:
    Neighbourhood Radius: The maximum radius a boid will search for neighbours.
    Max Neighbours: The maximium number of neighbors the boid will use to adjust
    it's velocity.


