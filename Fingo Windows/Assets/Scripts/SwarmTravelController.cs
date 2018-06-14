using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmTravelController : MonoBehaviour {

    public List<Transform> flockTransforms;
    public List<UnityFlock> flockBehaviors;

    public

    // Use this for initialization
    void Start() {

    }

    public void RegisterFlock(UnityFlock flock)
    {
        if (flockBehaviors.Contains(flock))
            return;

        flockTransforms.Add(flock.GetComponent<Transform>());
        flockBehaviors.Add(flock);

        flock.travelOrigin = this;
        flock.origin = this.transform;
    }

    public UnityFlock UnregisterRandomFlock()
    {
        if (flockBehaviors.Count == 0) return null;
        UnityFlock removedFlock = flockBehaviors[0];
        flockTransforms.Remove(removedFlock.GetComponent<Transform>());
        flockBehaviors.RemoveAt(0);

        return removedFlock;
    }

   
	// Update is called once per frame
	void Update () {
		
	}
}
