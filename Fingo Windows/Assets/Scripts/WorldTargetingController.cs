﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cogobyte.ProceduralIndicators;

public class WorldTargetingController : MonoBehaviour {



    public ArrowObject arrowIndicator;

    public Transform[] targetRanges;
    public Transform currentTargetRange;
    public Transform targetingVector;

    public Transform targetOrigin;
    public Transform targetDetector;
    public Transform currentTarget;

    public int currentDirectionIndex;
    public int currentRangeIndex;
    
    public bool isTargetLocked;
    public bool isAcquiringTarget;

    public bool isControlEnabled;

    float[] targetingDirections = { -55.0f, -37.2f, -28.7f, -22.6f, -13.3f, -7.7f, 4.06f, 11.5f, 19.3f, 28.6f, 45.9f };



    public void SwitchTargetRange(int range)
    {
        print("SwitchTargetRange: " + range.ToString());

        if (range > targetRanges.Length - 1) return;

        currentTargetRange = targetRanges[range];

        targetDetector.position = new Vector3(currentTargetRange.position.x, targetDetector.position.y, currentTargetRange.position.z);

        // TESTING
        //currentTarget.position = targetDetector.position;
    }

    void DetectTarget()
    {
        Transform swarmTarget = null;
        // TODO get treeNode as swarm target when current range target collides with treeNode
        currentTarget = swarmTarget;

        // TODO highlight target
    }

    public void ActivateTargeting()
    {
        isControlEnabled = true;

        arrowIndicator.hideIndicator = false;
        //arrowIndicator.updateArrowMesh();
    }


    public void DeactivateTargeting()
    {
        isControlEnabled = false;

        arrowIndicator.hideIndicator = true;
        //arrowIndicator.updateArrowMesh();
    }

    public void HighlightTarget(Transform swarmTarget)
    {
        Debug.Log("HighlightTarget: " +swarmTarget.ToString());
        
        currentTarget = swarmTarget;
        //currentTarget.position = new Vector3(swarmTarget.position.x, swarmTarget.position.y, swarmTarget.position.z);

        targetDetector.position = new Vector3(currentTarget.position.x, targetDetector.position.y, currentTarget.position.z);

    }

    public void ResetTargeting()
    {
        // SET TAREGETING STRAIGHT AHEAD, CENTER
        isTargetLocked = false;
        SwitchTargetRange(1);
        SetTargetingDirection(0);
    }

	// Use this for initialization
	void Start () {
        ActivateTargeting();
    }

    private void OnEnable()
    {
        ResetTargeting();
    }

    public void SetTargetingDirection(float directionAngle)
    {
        if (!isTargetLocked)
        {
            Debug.Log("SetTargetingDirection " + directionAngle.ToString());

            //targetingVector.localRotation = new Quaternion(targetingVector.localRotation.x, targetingDirections[directionIndex], targetingVector.localRotation.z, targetingVector.localRotation.w);
            //targetingVector.rotation = new Quaternion(targetingVector.rotation.x, targetingDirections[directionIndex], targetingVector.rotation.z, targetingVector.rotation.w);
            targetingVector.eulerAngles = new Vector3(0, directionAngle, 0);
        }

        
    }

    public void SwitchTargetingDirection(int directionIndex)
    {
        Debug.Log("SwitchTargetingDirection " + directionIndex.ToString());

        //targetingVector.localRotation = new Quaternion(targetingVector.localRotation.x, targetingDirections[directionIndex], targetingVector.localRotation.z, targetingVector.localRotation.w);
        //targetingVector.rotation = new Quaternion(targetingVector.rotation.x, targetingDirections[directionIndex], targetingVector.rotation.z, targetingVector.rotation.w);
        targetingVector.rotation = Quaternion.identity;
        targetingVector.Rotate(new Vector3(0, currentDirectionIndex*10.0f, 0));
    }

    // DEBUG CONTROLS FOR PC
    void KeyboardTargetingDirection()
    {
        if (!isControlEnabled) return;

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Debug.Log("KeyboardTargetingDirection Left");
            //if (currentDirectionIndex > 0)
            if (currentDirectionIndex > -10)
            {
                currentDirectionIndex--;

            }

            SwitchTargetingDirection(currentDirectionIndex);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Debug.Log("KeyboardTargetingDirection Right");

            //if (currentDirectionIndex < targetingDirections.Length)
            if (currentDirectionIndex < 10)
            {
                currentDirectionIndex++;

            }

            SwitchTargetingDirection(currentDirectionIndex);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Debug.Log("KeyboardTargetingDirection UP");
            if (currentRangeIndex < 4)
            {
                currentRangeIndex++;
            }
            SwitchTargetRange(currentRangeIndex);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Debug.Log("KeyboardTargetingDirection DOWN");
            if (currentRangeIndex > 0)
            {
                currentRangeIndex--;
            }
            SwitchTargetRange(currentRangeIndex);
        }
    }
	
	// Update is called once per frame
	void Update () {

        KeyboardTargetingDirection();

        arrowIndicator.arrowPath.endPoint = currentTarget.position;
        arrowIndicator.arrowPath.startPoint = targetOrigin.position;
        arrowIndicator.updateArrowMesh();

        //if (Input.GetKeyDown("1"))
        //{
        //    SwitchTargetRange(1);
        //}
        //if (Input.GetKeyDown("2"))
        //{
        //    SwitchTargetRange(2);
        //}
        //if (Input.GetKeyDown("3"))
        //{
        //    SwitchTargetRange(3);
        //}
        //if (Input.GetKeyDown("4"))
        //{
        //    SwitchTargetRange(4);
        //}
    }
}
