using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldMonitor : MonoBehaviour {

    [SerializeField]
    List<SwarmNodeController> swarmNodeCtrls;

    // LIVE INSTANCE VARIABLES:
    [SerializeField]
    int numberWorkers; // number of bees
    [SerializeField]
    int numberResourcePoints; // number of flowers

    // DERIVED VARIABLES:
    [SerializeField]
    float countWorkerVisits;
    [SerializeField]
    float resourceCollectedByWorkerPerPeriod;
    [SerializeField]
    float totalWorkerCollectionCapacity;
    [SerializeField]
    float numberOfCollectionPeriods;
    [SerializeField]
    float totalWorkerVisitsDuringSample;
    [SerializeField]
    float totalResourcesCollectedDuringSample;
    [SerializeField]
    float fulfillmentOfPollinationRate;
    [SerializeField]
    float fulfillmentOfCollectionCapacity;
    [SerializeField]
    float totalFlowersPollinated;

    // Use this for initialization
    void Start () {

        swarmNodeCtrls = new List<SwarmNodeController>();

        SwarmNodeController[] querySwarmNodeCtrls = gameObject.GetComponentsInChildren<SwarmNodeController>();
        for (int i=0; i< querySwarmNodeCtrls.Length; i++)
        {
            swarmNodeCtrls.Add(querySwarmNodeCtrls[i]);
        }

    }

    public void CalculateSwarmGlobalEffect()
    {
        Debug.Log("CalculateSwarmGlobalEffect swarmNodes: " + swarmNodeCtrls.Count.ToString());

        float[] reportResourcesCollected = new float[swarmNodeCtrls.Count];
        int[] reportFlowersPollinated = new int[swarmNodeCtrls.Count];
        totalResourcesCollectedDuringSample = 0;
        totalFlowersPollinated = 0;

        int swarmNodeIndex = 0;

        foreach (SwarmNodeController swarmNodeCtrl in swarmNodeCtrls)
        {
            swarmNodeCtrl.CalculateResultOfSwarmActivity();

            reportFlowersPollinated[swarmNodeIndex] = Mathf.RoundToInt(swarmNodeCtrl.totalFlowersPollinated);
            reportResourcesCollected[swarmNodeIndex] = swarmNodeCtrl.totalResourcesCollectedDuringSample;

            totalFlowersPollinated += Mathf.RoundToInt(swarmNodeCtrl.totalFlowersPollinated);
            totalResourcesCollectedDuringSample += swarmNodeCtrl.totalResourcesCollectedDuringSample;

        }

        Debug.Log("CalculateSwarmGlobalEffect reportFlowersPollinated: " + reportFlowersPollinated);
        Debug.Log("CalculateSwarmGlobalEffect reportResourcesCollected: " + reportResourcesCollected);

        Debug.Log("CalculateSwarmGlobalEffect totalFlowersPollinated: " + totalFlowersPollinated);
        Debug.Log("CalculateSwarmGlobalEffect totalResourcesCollectedDuringSample: " + totalResourcesCollectedDuringSample);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))
        {
            CalculateSwarmGlobalEffect();
        }
	}
}
