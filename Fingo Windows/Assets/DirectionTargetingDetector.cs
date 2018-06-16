using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirectionTargetingDetector : MonoBehaviour {

    public float targeDirectionValue;

    [System.Serializable]
    public class UnityEventFloat : UnityEvent<float> { }
    public UnityEventFloat OnTargetDirectionDetected;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTargetDirectionDetected :" + targeDirectionValue.ToString());

        OnTargetDirectionDetected.Invoke(targeDirectionValue);

    }


    // Use this for initialization
    void Start () {

        targeDirectionValue = transform.parent.localEulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
