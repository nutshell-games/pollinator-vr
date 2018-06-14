using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : MonoBehaviour {

    //public Transform[] treeNodes;
    //public List<Transform> hiveNodes;
    //public List<Transform> swarmNodes;

    public List<SwarmTravelController> swarmControllers;


    public SwarmTravelController originSwarm;
    public SwarmTravelController destinationSwarm;

    // Use this for initialization
    void Start()
    {
        //RegisterSwarms();
        //originSwarm = swarmControllers[0];
        //destinationSwarm = swarmControllers[1];
    }
    
    void SetupTestScenario()
    {
        // TEST: total 20 bees
        AssignWorkersToResourcePoint(swarmControllers[2], 3);
        AssignWorkersToResourcePoint(swarmControllers[3], 4);
        AssignWorkersToResourcePoint(swarmControllers[5], 5);
        AssignWorkersToResourcePoint(swarmControllers[8], 2);
        AssignWorkersToResourcePoint(swarmControllers[10], 6);
    }

    public void AssignWorkersToResourcePoint(SwarmTravelController targetSwarm, int countWorkers)
    {
        SetDestinationSwarm(targetSwarm);
        
        for (int c = 0; c < countWorkers; c++)
        {
            SendFlockBetweenSwarms();
        }
    }

    void RegisterSwarms()
    {
        swarmControllers = new List<SwarmTravelController>();

        SwarmTravelController[] swarms = transform.GetComponentsInChildren<SwarmTravelController>();

        for (int i = 0; i < swarms.Length; i++)
        {
            swarmControllers.Add(swarms[i]);

        }
    }

    void ReturnAllFlocksToHive()
    {
        foreach (SwarmTravelController swarmCtrl in swarmControllers)
        {
            if (swarmCtrl == originSwarm) continue;

            ReturnAllFlocksInSwarmToHive(swarmCtrl);
        }
    }

    void ReturnAllFlocksInSwarmToHive(SwarmTravelController swarmCtrl)
    {
        while (swarmCtrl.flockBehaviors.Count > 0)
        {
            ReturnFlockInSwarmToHive(swarmCtrl);
        }

    }

    void ReturnFlockInSwarmToHive(SwarmTravelController swarmCtrl)
    {
        UnityFlock flock = swarmCtrl.UnregisterRandomFlock();

        if (flock == null) return;

        originSwarm.RegisterFlock(flock);
        //foreach(UnityFlock flock in swarm)
    }


    // Update is called once per frame
    void Update () {

        if (Input.GetKeyUp("a"))
        {
            SetupTestScenario();
        }

        if (Input.GetKeyUp("r"))
        {
            ReturnAllFlocksToHive();
        }

        if (Input.GetKeyUp("space"))
        {
            SendFlockBetweenSwarms();
        }

        //if (Input.GetKeyDown("1"))
        //{
        //    SetDestinationSwarm(1);
        //}
        //if(Input.GetKeyDown("2"))
        //{
        //    SetDestinationSwarm(2);
        //}
        //if(Input.GetKeyDown("3"))
        //{
        //    SetDestinationSwarm(3);
        //}
        //if (Input.GetKeyDown("4"))
        //{
        //    SetDestinationSwarm(4);
        //}


    }


    public void SetDestinationSwarm(SwarmTravelController swarmCtrl)
    {
        print("SetDestinationSwarm: " + swarmCtrl.ToString());
        destinationSwarm = swarmCtrl;
    }

    //public void SetDestinationSwarm(int indexSwarm)
    //{
    //    if (indexSwarm < swarmControllers.Count)
    //    {
    //        print("SetDestinationSwarm: " + indexSwarm.ToString());
    //        destinationSwarm = swarmControllers[indexSwarm];

    //    }
    //}

    public void SendFlockBetweenSwarms()
    {
        print("swarms count:" + swarmControllers.Count);

        UnityFlock flock = originSwarm.UnregisterRandomFlock();
        destinationSwarm.RegisterFlock(flock);

        print("flock A:" + originSwarm.flockTransforms.Count.ToString());
        print("flock B:" + destinationSwarm.flockTransforms.Count.ToString());
    }

    // all flocks have referenced each other 


    // all need to be indexed from the parent

    public void SendFlockToDestination()
    {
        // remove first flock from 
    }
}
