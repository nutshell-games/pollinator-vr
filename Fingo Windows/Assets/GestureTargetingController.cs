using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fingo;


public class GestureTargetingController : MonoBehaviour {

    public Transform handModelLeft;
    public Transform handModelRight;
    public Transform handNodeLeft;
    public Transform handNodeRight;
    public Transform targetNode;
    public Transform targetQualityNode;
    public Transform targetingVectorNode;

    public float targetNodeMinimumY;
    public float targetNodeMaximumY;


    public bool isTargetLocked;
    public bool isLeftHandWithinCaptureBounds;
    public bool isRightHandWithinCaptureBounds;

    public bool isCalibrating;
    public ControlMode controlMode;

    public enum ControlMode { TargetingMode, CommandMode }

    public float lastVectorMagnitudeRight;
    public float maximumVectorMagnitudeRight;
    public float minimumVectorMagnitudeRight;
    public float meanVectorMagnitudeRight;

    public float lastVectorMagnitudeLeft;
    public float maximumVectorMagnitudeLeft;
    public float minimumVectorMagnitudeLeft;
    public float meanVectorMagnitudeLeft;

    public float referenceYvalue;
    public float clampedTargetingYvalue;
    public float clampedTargetingYrange;
    public float percentageYvalue;

    Fingo.Head headTracked;

    // Use this for initialization
    void Start () {

        headTracked = FingoMain.Instance.GetHead();
        controlMode = ControlMode.TargetingMode;
        isCalibrating = false;

        // TODO reset following values in calibration mode
        // limits defined with Fingo sensor at ~45 deg down mount angle, with headband mount
        targetNodeMinimumY = -0.4f;
        targetNodeMaximumY = 0.4f;

        maximumVectorMagnitudeRight = 0.9f;
        minimumVectorMagnitudeRight = 0.4f;

        maximumVectorMagnitudeLeft = 0.9f;
        minimumVectorMagnitudeLeft = 0.4f;
        // END values for calibration
            
        meanVectorMagnitudeLeft = (maximumVectorMagnitudeLeft + minimumVectorMagnitudeLeft) / 2;
        meanVectorMagnitudeRight = (maximumVectorMagnitudeRight + minimumVectorMagnitudeRight) / 2;

        clampedTargetingYrange = Mathf.Abs(targetNodeMaximumY - targetNodeMinimumY);

    }
	
	// Update is called once per frame
	void Update () {

        setTargetVectorRotation();

        Fingo.Hand handLeft = FingoMain.Instance.GetHand(Fingo.HandType.Left);
        Fingo.Hand handRight = FingoMain.Instance.GetHand(Fingo.HandType.Right);

        bool isDetectedLeft = handLeft.IsDetected();
        bool isDetectedRight = handRight.IsDetected();

        isRightHandWithinCaptureBounds = isDetectedRight;
        isLeftHandWithinCaptureBounds = isDetectedLeft;

        if (isRightHandWithinCaptureBounds)
        {
            updateHandNodePosition(handNodeRight, handModelRight);
        }

        if (isLeftHandWithinCaptureBounds)
        {
            updateHandNodePosition(handNodeLeft, handModelLeft);
        }
        

        if (!isTargetLocked && controlMode==ControlMode.TargetingMode)
        {
            updateTargetNodePosition();
        }

        if (isTargetLocked && controlMode==ControlMode.CommandMode)
        {
            trackVectorReference();
        }
    }


    void setTargetVectorRotation()
    {
        if (headTracked != null)
        {
            Quaternion headRotation = headTracked.GetRotation();

            targetingVectorNode.localRotation = new Quaternion(targetingVectorNode.localRotation.x, headRotation.y, targetingVectorNode.localRotation.z, targetingVectorNode.localRotation.z);
        }
    }

    void updateHandNodePosition(Transform handNode, Transform handModel)
    {
        handNode.position = handModel.position;

        // TODO adjust position of hand node for more visually accurate anchoring of target node between hands
    }

    // https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
    float getMagnitudeVectorBetweenNodes(Transform origin, Transform target)
    {
        Vector3 heading = target.position - origin.position;
        float distance = heading.magnitude;

        //if (heading.sqrMagnitude < maxRange * maxRange)
        //{
        //    // Target is within range.
        //}

        return distance;
    }


    void updateTargetNodePosition()
    {

        //targetNode.position = new Vector3((handNodeLeft.position.x + handNodeRight.position.x) / 2, (handNodeLeft.position.y + handNodeRight.position.y) / 2, (handNodeLeft.position.z + handNodeRight.position.z) / 2);
        // limit movement to Y axis
        referenceYvalue = (handNodeLeft.position.y + handNodeRight.position.y) / 2;

        if (referenceYvalue > targetNodeMinimumY && referenceYvalue < targetNodeMaximumY)
        {
            targetNode.position = new Vector3(targetNode.position.x, referenceYvalue, targetNode.position.z);

            updateTargetingYvalue(referenceYvalue);
        }

        
    }

    void updateTargetingYvalue(float clampedYvalue)
    {
        clampedTargetingYvalue = Mathf.Abs(clampedYvalue - targetNodeMinimumY);

        percentageYvalue = clampedTargetingYvalue / clampedTargetingYrange;
    }

    Vector3 getNormalizedVectorBetweenNodes(Transform origin, Transform target)
    {
        Vector3 heading = target.position - origin.position;
        float distance = heading.magnitude;
        Vector3 normalizedDirection = heading / distance;

        return normalizedDirection;
    }

    // track sum magnitudes of vector between hand nodes and reference node
    void trackVectorReference()
    {
        float rightHandReferenceMagnitude = getMagnitudeVectorBetweenNodes(targetQualityNode, handNodeRight);
        float leftHandReferenceMagnitude = getMagnitudeVectorBetweenNodes(targetQualityNode, handNodeLeft);
        float sumReferenceMagnitude = rightHandReferenceMagnitude + leftHandReferenceMagnitude;

        //Debug.Log("Ref. mag: Sum: "+ sumReferenceMagnitude + " L: " + leftHandReferenceMagnitude.ToString() + " R: " + rightHandReferenceMagnitude.ToString());

        // define event trigger condition as crossing the mean value between local maximum and minimum values

        if (rightHandReferenceMagnitude > meanVectorMagnitudeRight && lastVectorMagnitudeRight < meanVectorMagnitudeRight)
        {
            Debug.Log("RIGHT hand magnitude crossed threshold UP");
        }

        if (rightHandReferenceMagnitude < meanVectorMagnitudeRight && lastVectorMagnitudeRight > meanVectorMagnitudeRight)
        {
            Debug.Log("RIGHT hand magnitude crossed threshold DOWN");
        }

        if (leftHandReferenceMagnitude > meanVectorMagnitudeLeft && lastVectorMagnitudeLeft < meanVectorMagnitudeLeft)
        {
            Debug.Log("RIGHT hand magnitude crossed threshold UP");
        }

        if (leftHandReferenceMagnitude < meanVectorMagnitudeLeft && lastVectorMagnitudeLeft > meanVectorMagnitudeLeft)
        {
            Debug.Log("RIGHT hand magnitude crossed threshold DOWN");
        }

        //float deltaVectorMagnitudeRight = lastVectorMagnitudeRight - rightHandReferenceMagnitude;

        //Debug.Log("delta mag. R: " + deltaVectorMagnitudeRight.ToString()+" last R:" + lastVectorMagnitudeRight);

        //if (lastVectorMagnitudeRight > rightHandReferenceMagnitude)
        //{
        //    Debug.Log("RIGHT hand magnitude INCREASING");
        //} else if (lastVectorMagnitudeRight < rightHandReferenceMagnitude)
        //{
        //    Debug.Log("RIGHT hand magnitude DECREASING");
        //} else
        //{
        //    Debug.Log("RIGHT hand magnitude SAME");
        //}

        lastVectorMagnitudeLeft = leftHandReferenceMagnitude;
        lastVectorMagnitudeRight = rightHandReferenceMagnitude;
    }

    public void lockTargetNode()
    {
        isTargetLocked = true;

        Debug.Log("lockTargetNode");
    }

    public void releaseTargetNode()
    {
        isTargetLocked = false;

        Debug.Log("releaseTargetNode");
    }

    public void enterCommandMode()
    {
        Debug.Log("enterCommandMode");

        controlMode = ControlMode.CommandMode;
    }

    public void exitCommandMode()
    {
        Debug.Log("exitCommandMode");

        controlMode = ControlMode.TargetingMode;
    }
}
