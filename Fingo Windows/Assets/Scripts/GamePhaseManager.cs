﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// why this game:
/// <summary>
/// to demonstrate how one activity infused with fresh technology based on scientific understanding of nature can provide exploration of multiple core academic subjects, while being social, physical, and fun
/// </summary>

// with every game, the location and flower amount of each tree is randomized
// math problem: what is the optimal distribution of bee
// skills: mental math, kinesthetic memory
// concepts: non-verbal communication in nature (communication through movement)

// each tree has different amount of flowers: 
// low      4   
// medium   6   
// high     10  
// player's goal is to allocate the right amount of bees to all trees to maximize resource (nectar & pollen) collection for the hive

// survey phase: 
// player has time to inspect the tree grove, to calculate roughly how many bees to send to each tree


// dance phase: 120s
// player will have limited time to dance for all the trees

// collection phase: 20s
// after dance phase, all bees will fly out of the hive to the trees in the amount designated by the dance 
// last
// each tree will fill up a "pollination meter" based on the number of bees assigned to it

// blossom phase: 
// all bees will return to the hive
// based on how full a tree pollination meter, the tree will bear fruit for a certain number of its flowers

    // ideal bee count for tree = total bees / total flowers * flowers on tree

public class GamePhaseManager : MonoBehaviour {

    public float durationStartPhase;
    public float durationSurveyPhase;
    public float durationDancePhase;
    public float durationCollectionPhase;
    public float durationScoringPhase;

    public CustomTimer timerStartPhase;
    public CustomTimer timerSurveyPhase;
    public CustomTimer timerDancePhase;
    public CustomTimer timerCollectionPhase;

    public Transform UIWorkerCount;
    public Transform UIPollenCount;
    public Transform UIFruitCount;

    public Transform worldTargetingSystem;
    public Transform cameraTargetingSystem;

    public UnityEvent OnStartSurveyPhase;
    public UnityEvent OnStartDancePhase;
    public UnityEvent OnStartCollectionPhase;
    public UnityEvent OnStartScoringPhase;

    public int scoreFruit;
    public float scorePollen;


    // Use this for initialization
    void Start () {
        if (OnStartSurveyPhase == null) OnStartSurveyPhase = new UnityEvent();
        if (OnStartDancePhase == null) OnStartDancePhase = new UnityEvent();
        if (OnStartCollectionPhase == null) OnStartCollectionPhase = new UnityEvent();
        if (OnStartScoringPhase == null) OnStartScoringPhase = new UnityEvent();

        ResetTimers();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (Input.GetKeyUp(KeyCode.S))
        {
            StartGame();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            ResetTimers();
        }
    }



    void ScoreGame()
    {
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


    }

    public void StartGame()
    {
        timerStartPhase.gameObject.SetActive(true);

        timerStartPhase.duration = durationStartPhase;
        timerSurveyPhase.duration = durationSurveyPhase;
        timerDancePhase.duration = durationDancePhase;
        timerCollectionPhase.duration = durationCollectionPhase;

        timerStartPhase.StartTimer();
    }

    public void StartSurveyPhase()
    {
        timerStartPhase.gameObject.SetActive(false);

        OnStartSurveyPhase.Invoke();

        
        timerSurveyPhase.StartTimer();
    }

    public void StartDancePhase()
    {
        OnStartDancePhase.Invoke();

        // enable camera targeting system
        // enable world targeting system
        worldTargetingSystem.gameObject.SetActive(true);
        cameraTargetingSystem.gameObject.SetActive(true);

        
        timerDancePhase.StartTimer();
    }

    public void StartCollectionPhase()
    {
        OnStartCollectionPhase.Invoke();

        // disble camera targeting system
        // disble world targeting system
        worldTargetingSystem.gameObject.SetActive(false);
        cameraTargetingSystem.gameObject.SetActive(false);

        
        timerCollectionPhase.StartTimer();
    }

    public void StartScoringPhase()
    {
        // score nectar and apricots by each tree
        UIFruitCount.gameObject.SetActive(true);
        UIPollenCount.gameObject.SetActive(true);

        OnStartScoringPhase.Invoke();
    }

    public void ResetTimers()
    {
        worldTargetingSystem.gameObject.SetActive(false);
        cameraTargetingSystem.gameObject.SetActive(false);

        timerStartPhase.duration = durationStartPhase;
        timerSurveyPhase.duration = durationSurveyPhase;
        timerDancePhase.duration = durationDancePhase;
        timerCollectionPhase.duration = durationCollectionPhase;

        timerStartPhase.ResetTimer();
        timerSurveyPhase.ResetTimer();
        timerDancePhase.ResetTimer();
        timerCollectionPhase.ResetTimer();

        timerStartPhase.gameObject.SetActive(false);
        UIFruitCount.gameObject.SetActive(false);
        UIPollenCount.gameObject.SetActive(false);
    }
}
