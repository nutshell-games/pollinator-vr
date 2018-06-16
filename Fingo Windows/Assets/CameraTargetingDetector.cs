using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraTargetingDetector : MonoBehaviour {

    public int targetRangeValue;

    [System.Serializable]
    public class UnityEventInt : UnityEvent<int> { }
    public UnityEventInt OnTargetRangeDetected;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTargetRangeDetected :" + targetRangeValue.ToString());

        OnTargetRangeDetected.Invoke(targetRangeValue);

    }
    
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
