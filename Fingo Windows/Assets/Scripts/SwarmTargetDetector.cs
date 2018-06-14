using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwarmTargetDetector : MonoBehaviour {
    
    public Transform swarmTargetMarker;
    public SwarmTravelController targetSwarmCtrl;


    [System.Serializable]
    public class UnityEventTransform : UnityEvent<Transform> { }
    public UnityEventTransform OnSwarmTargetDetectedEvent;

    [System.Serializable]
    public class UnityEventSwarm : UnityEvent<SwarmTravelController> { };
    public UnityEventSwarm OnNewSwarmTarget;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnSwarmTargetDetectedEvent :" + swarmTargetMarker.ToString());

        OnSwarmTargetDetectedEvent.Invoke(swarmTargetMarker);

        targetSwarmCtrl = swarmTargetMarker.parent.gameObject.GetComponentInChildren<SwarmTravelController>();

        Debug.Log("OnNewSwarmTarget " + targetSwarmCtrl.ToString());
        Debug.Log(targetSwarmCtrl);

        OnNewSwarmTarget.Invoke(targetSwarmCtrl);
        ActivateTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        DeactivateTarget();
    }

    private void OnEnable()
    {
        if (OnSwarmTargetDetectedEvent == null)
        {
            OnSwarmTargetDetectedEvent = new UnityEventTransform();
        }

        if (OnNewSwarmTarget == null)
        {
            OnNewSwarmTarget = new UnityEventSwarm();
        }
    }


    public void ActivateTarget()
    {
        swarmTargetMarker.gameObject.SetActive(true);
    }

    public void DeactivateTarget()
    {
        swarmTargetMarker.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        DeactivateTarget();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
