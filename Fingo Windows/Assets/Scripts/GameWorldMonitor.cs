using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWorldMonitor : MonoBehaviour {

    public TextMesh labelPollenCount;
    public TextMesh labelFruitCount;
    public TextMesh labelWorkerCount;

    [SerializeField]
    List<SwarmNodeController> swarmNodeCtrls;
    List<TreeLifecycleController> treeCtrls;

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

        treeCtrls = new List<TreeLifecycleController>();

        TreeLifecycleController[] queryTreeCtrls = gameObject.GetComponentsInChildren<TreeLifecycleController>();
        for (int i = 0; i < queryTreeCtrls.Length; i++)
        {
            treeCtrls.Add(queryTreeCtrls[i]);
        }

        ResetCounts();
    }

    public void ShowAllFlowers()
    {
        foreach (TreeLifecycleController treeCtrl in treeCtrls)
        {
            treeCtrl.ShowFlowers();
        }
    }

    public void ResetTreeLifecycles()
    {
        foreach (TreeLifecycleController treeCtrl in treeCtrls)
        {
            treeCtrl.ResetLifecycle();
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

        UpdateScore();
    }

    public void ResetCounts()
    {
        labelPollenCount.text = 0.ToString();
        labelFruitCount.text = 0.ToString();
    }

    public void UpdateWorkerCount(int currentWorkerCount)
    {
        numberWorkers = currentWorkerCount;
        labelWorkerCount.text = numberWorkers.ToString();
    }

    public void UpdateScore()
    {
        labelPollenCount.text = totalResourcesCollectedDuringSample.ToString();
        labelFruitCount.text = totalFlowersPollinated.ToString();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))
        {
            CalculateSwarmGlobalEffect();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTreeLifecycles();
        }
    }
}
