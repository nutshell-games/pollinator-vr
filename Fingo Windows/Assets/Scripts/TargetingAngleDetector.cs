using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetingAngleDetector : MonoBehaviour {

    public int targetingRangeID;

    [System.Serializable]
    public class UnityEventInt : UnityEvent<int> {}

    public UnityEventInt OnTargetingAngleDetectedEvent;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TargetingAngleDetector :"+targetingRangeID.ToString());

        OnTargetingAngleDetectedEvent.Invoke(targetingRangeID);
    }

    private void OnEnable()
    {
        if (OnTargetingAngleDetectedEvent == null)
        {
            OnTargetingAngleDetectedEvent = new UnityEventInt();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
