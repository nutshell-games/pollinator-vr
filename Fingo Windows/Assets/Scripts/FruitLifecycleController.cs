using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitLifecycleController : MonoBehaviour {

    public float defaultScale;

    // Use this for initialization
    void Start()
    {
        defaultScale = 100f;

        HideAllFruit();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            GrowAllFruit();
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            HideAllFruit();
        }
    }


    public void HideAllFruit()
    {
        foreach (Transform flowerObj in transform)
        {
            flowerObj.localScale = Vector3.zero;
        }
    }

    public void GrowFruit(int countFruit)
    {
        Debug.Log("GrowFruit " + countFruit);

        int fruitCounter = 1;

        Vector3 finalScale = new Vector3(defaultScale, defaultScale, defaultScale);
        float sequenceDelay = 0.2f;
        float delay = 0;

        foreach (Transform flowerObj in transform)
        {
            if (fruitCounter > countFruit) break;

            iTween.ScaleTo(flowerObj.gameObject, iTween.Hash("scale", finalScale, "easeType", "easeInOutBack", "time", 2.0f, "delay", delay));

            delay += sequenceDelay;
            fruitCounter++;
        }
    }

    public void GrowAllFruit()
    {
        GrowFruit(transform.childCount);
    }
}
