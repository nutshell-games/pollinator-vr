using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmNodeController : MonoBehaviour {


    public string ID;

    [SerializeField]
    TreeLifecycleController treeCtrl;

    [SerializeField]
    SwarmTravelController swarmTravelCtrl;

    // Use this for initialization
    void Start () {
        treeCtrl = gameObject.GetComponentInChildren<TreeLifecycleController>();
        swarmTravelCtrl = gameObject.GetComponentInChildren<SwarmTravelController>();

        // SET GLOBALS
        durationCollectionPeriod = 1f;
        amountWorkerCollectsResource = 1f;
        capacityWorkerForResource = 20f;
        durationSamplePeriod = 5f;

        minVisitsPerFlowerForPollination = 2;
        maxVisitsPerFlowerForPollination = 5;
}

    public void PrepareSurveyPhase()
    {
        // make flowers bloom
    }

    public void PrepareScoringPhase()
    {
        // make fruit grow
        // fill fruit meter circle
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // sum fruit score from all trees
    // sum pollen score from all bees

    // only one pollinator can feed from one flower at a time
    // number of visits a pollinator can make is limited by the number of available flowers to feed from

    // time for visit = time to feed
    // optimal number of pollinators to flowers: pollinators = flowers

    // every visit/feeding period:
    // # visits = min(# pollinators, # flowers)
    // # pollen = # visits * pollen collection rate / period

    // tree: calculate flowers pollinated
    // total visits = # pollinators * pollinator visit / time
    // f(total visits / # flowers) = chance of any flower pollinated * # flowers = # flowers pollinated

    // hive: calculate pollen collected

    // GLOBALS:
    public float durationCollectionPeriod;
    public float amountWorkerCollectsResource;
    public float capacityWorkerForResource;
    public float durationSamplePeriod;
    public float minVisitsPerFlowerForPollination;
    public float maxVisitsPerFlowerForPollination;


    // LIVE INSTANCE VARIABLES:
    public int numberWorkers; // number of bees
    public int numberResourcePoints; // number of flowers

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
    float _totalResourcesCollectedDuringSample;
    [SerializeField]
    float fulfillmentOfPollinationRate;
    [SerializeField]
    float fulfillmentOfCollectionCapacity;
    [SerializeField]
    float _totalFlowersPollinated;

    public float totalFlowersPollinated { get { return _totalFlowersPollinated; } }
    public float totalResourcesCollectedDuringSample { get { return _totalResourcesCollectedDuringSample; } }

    float GetProbablityOfPollination(float pollinatorVisits)
    {
        if (pollinatorVisits < minVisitsPerFlowerForPollination * numberResourcePoints) return 0;
        else if (pollinatorVisits >= maxVisitsPerFlowerForPollination * numberResourcePoints) return 1;
        else
        {
            return (pollinatorVisits - (minVisitsPerFlowerForPollination * numberResourcePoints)) / ((maxVisitsPerFlowerForPollination - minVisitsPerFlowerForPollination) * numberResourcePoints);
        }
    }

    public void CalculateResultOfSwarmActivity()
    {
        // set tree / swarm variables
        numberWorkers = swarmTravelCtrl.flockBehaviors.Count;
        numberResourcePoints = treeCtrl.numberFlowers;


        // assuming all workers take equal amount of time (collection period) to visit and collect from each resource point
        countWorkerVisits = (float)Mathf.Min(numberWorkers, numberResourcePoints);
        // pollen collected by each worker
        resourceCollectedByWorkerPerPeriod = countWorkerVisits * amountWorkerCollectsResource;

        totalWorkerCollectionCapacity = numberWorkers * capacityWorkerForResource;
        
        numberOfCollectionPeriods = durationSamplePeriod / durationCollectionPeriod;
        totalWorkerVisitsDuringSample = countWorkerVisits * numberOfCollectionPeriods;


        // SCORES:
        _totalResourcesCollectedDuringSample = resourceCollectedByWorkerPerPeriod * numberWorkers * numberOfCollectionPeriods;

        fulfillmentOfPollinationRate = GetProbablityOfPollination(totalWorkerVisitsDuringSample);
        fulfillmentOfCollectionCapacity = _totalResourcesCollectedDuringSample / (totalWorkerCollectionCapacity);

        _totalFlowersPollinated = Mathf.RoundToInt(fulfillmentOfPollinationRate * numberResourcePoints);

        Debug.Log("============");
        Debug.Log("CalculateResultOfSwarmActivity: "+ ID);

        if (numberWorkers > 0)
        {
            Debug.Log("countWorkerVisits: " + countWorkerVisits + "=" + numberWorkers + "|" + numberResourcePoints);
            Debug.Log("total resources collected: " + _totalResourcesCollectedDuringSample + "= " + resourceCollectedByWorkerPerPeriod + " * " + numberOfCollectionPeriods);
            Debug.Log("fulfillmentOfCollectionCapacity: " + fulfillmentOfCollectionCapacity + "= " + _totalResourcesCollectedDuringSample + " / " + totalWorkerCollectionCapacity);
            Debug.Log("_totalFlowersPollinated: " + _totalFlowersPollinated + "= " + fulfillmentOfPollinationRate + " * " + numberResourcePoints);
            // % success pollination of each flower

            // pollen collected by each worker

            // % success pollination of each flower       
        }

        // TODO run fruit animation during scoring phase
        treeCtrl.GrowFruit((int)_totalFlowersPollinated);

    }
}
