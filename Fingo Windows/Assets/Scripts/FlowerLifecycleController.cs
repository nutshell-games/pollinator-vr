using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLifecycleController : MonoBehaviour {

    public float defaultScale;

	// Use this for initialization
	void Start () {

        defaultScale = 16f;

        HideAllFlowers();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyUp(KeyCode.B))
        {
            BlossomAllFlowers();
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            HideAllFlowers();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            ShrinkAllFlowers();
        }
    }


    public void HideAllFlowers()
    {
        foreach (Transform flowerObj in transform)
        {
            flowerObj.localScale = Vector3.zero;
        }
    }

    public void BlossomFlowers(int flowerCount)
    {
        Debug.Log("BlossomFlowers "+flowerCount);

        int flowerCounter = 1;

        Vector3 finalScale = new Vector3(defaultScale, defaultScale, defaultScale);
        float sequenceDelay = 0.2f;
        float delay = 0;

        foreach (Transform flowerObj in transform)
        {
            if (flowerCounter > flowerCount) break;

            iTween.ScaleTo(flowerObj.gameObject, iTween.Hash("scale", finalScale, "easeType", "easeInOutBack", "time", 0.6f, "delay", delay));

            delay += sequenceDelay;
            flowerCount++;
        }
    }

    public void BlossomAllFlowers()
    {
        BlossomFlowers(transform.childCount);
    }

    public void ShrinkAllFlowers()
    {
        Debug.Log("ShrinkAllFlowers");
        //Vector3 finalScale = new Vector3(defaultScale, defaultScale, defaultScale);
        float sequenceDelay = 0.2f;
        float delay = 0;

        foreach (Transform flowerObj in transform)
        {

            iTween.ScaleTo(flowerObj.gameObject, iTween.Hash("scale", Vector3.zero, "easeType", "easeInOutBack", "time", 2.0f, "delay", delay));

            delay += sequenceDelay;
        }
    }

}
