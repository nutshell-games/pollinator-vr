using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLifecycleController : MonoBehaviour {

    public bool isPollinated;

  
    public int numberFlowers;
    public FlowerLifecycleController flowerCtrl;
    public FruitLifecycleController fruitCtrl;



    // Use this for initialization
    void Start () {

        flowerCtrl = gameObject.GetComponentInChildren<FlowerLifecycleController>();
        fruitCtrl = gameObject.GetComponentInChildren<FruitLifecycleController>();

        numberFlowers = flowerCtrl.transform.childCount;

        isPollinated = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetTree()
    {
        isPollinated = false;

    }

    public void ShowFlowers()
    {
        // foreach flower, trigger show flower animation
    }

    public void GrowFruit(int fruitCount)
    {
        if (fruitCount > 0) fruitCtrl.GrowFruit(fruitCount);

        flowerCtrl.ShrinkAllFlowers();
    }

    public void HarvestFruit()
    {

    }
}
