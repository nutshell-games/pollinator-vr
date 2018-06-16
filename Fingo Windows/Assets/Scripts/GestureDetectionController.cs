/*************************************************************************\
*                           USENS CONFIDENTIAL                            *
* _______________________________________________________________________ *
*                                                                         *
* [2015] - [2017] USENS Incorporated                                      *
* All Rights Reserved.                                                    *
*                                                                         *
* NOTICE:  All information contained herein is, and remains               *
* the property of uSens Incorporated and its suppliers,                   *
* if any.  The intellectual and technical concepts contained              *
* herein are proprietary to uSens Incorporated                            *
* and its suppliers and may be covered by U.S. and Foreign Patents,       *
* patents in process, and are protected by trade secret or copyright law. *
* Dissemination of this information or reproduction of this material      *
* is strictly forbidden unless prior written permission is obtained       *
* from uSens Incorporated.                                                *
*                                                                         *
\*************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Fingo;

public class GestureDetectionController : MonoBehaviour
{
    public UnityEvent OnRightHandOkayGesture;
    public UnityEvent OnLeftHandOkayGesture;

    public UnityEvent OnRightHandPeaceGesture;
    public UnityEvent OnLeftHandPeaceGesture;

    public UnityEvent OnRightHandFistGesture;
    public UnityEvent OnLeftHandFistGesture;

    public UnityEvent OnRightHandThumbsUpGesture;
    public UnityEvent OnLeftHandThumbsUpGesture;

    public UnityEvent OnRightHandPalmGesture;
    public UnityEvent OnLeftHandPalmGesture;

    public UnityEvent OnRightHandPointGesture;
    public UnityEvent OnLeftHandPointGesture;

    public UnityEvent OnRightHandShootEmGesture;
    public UnityEvent OnLeftHandShootEmGesture;

    public UnityEvent OnRightHandCoverEyeGesture;
    public UnityEvent OnLeftHandCoverEyeGesture;

    public UnityEvent OnRightHandPinkyGesture;
    public UnityEvent OnLeftHandPinkyGesture;

    public UnityEvent OnRightHandHornsGesture;
    public UnityEvent OnLeftHandHornsGesture;

    public UnityEvent OnRightHandCallMeGesture;
    public UnityEvent OnLeftHandCallMeGesture;

    public UnityEvent OnClearRight;
    public UnityEvent OnClearLeft;

    //public UnityEvent OnRightHandGesture;
    //public UnityEvent OnLeftHandGesture;

    void OnEnable()
    {
        GestureManager.GestureEvent += ColorChange;

        if (OnRightHandOkayGesture == null)
        {
            OnRightHandOkayGesture = new UnityEvent();
        }
        if (OnLeftHandOkayGesture == null)
        {
            OnLeftHandOkayGesture = new UnityEvent();
        }
        if (OnRightHandPeaceGesture == null)
        {
            OnRightHandPeaceGesture = new UnityEvent();
        }
        if (OnLeftHandPeaceGesture == null)
        {
            OnLeftHandPeaceGesture = new UnityEvent();
        }
        if (OnRightHandFistGesture == null)
        {
            OnRightHandFistGesture = new UnityEvent();
        }
        if (OnLeftHandFistGesture == null)
        {
            OnLeftHandFistGesture = new UnityEvent();
        }
        if (OnClearRight == null)
        {
            OnClearRight = new UnityEvent();
        }
        if (OnClearLeft == null)
        {
            OnClearLeft = new UnityEvent();
        }

       
    }

    public void ColorChange(HandType handType, GestureName gestureType)
    {
        //Debug.Log("On GestureEvent handType: "+handType);
        //Debug.Log("On GestureEvent gestureType: " + gestureType);

        if (handType == HandType.Right)
        {
            //OnRightHandGesture.Invoke();
            //OnLeftHandGesture.Invoke();

            switch (gestureType)
            {
                case GestureName.Okay:
                    OnRightHandOkayGesture.Invoke();
                    break;
                case GestureName.Peace:
                    OnRightHandPeaceGesture.Invoke();
                    break;
                case GestureName.Fist:
                    OnRightHandFistGesture.Invoke();
                    break;

                case GestureName.ThumbsUp:
                    OnRightHandThumbsUpGesture.Invoke();
                    break;

                case GestureName.Palm:
                    OnRightHandPalmGesture.Invoke();
                    break;

                case GestureName.Point:
                    OnRightHandPointGesture.Invoke();
                    break;

                case GestureName.ShootEm:
                    OnRightHandShootEmGesture.Invoke();
                    break;

                case GestureName.CoverEye:
                    OnRightHandCoverEyeGesture.Invoke();
                    break;

                case GestureName.Pinky:
                    OnRightHandPinkyGesture.Invoke();
                    break;

                case GestureName.Horns:
                    OnRightHandHornsGesture.Invoke();
                    break;

                case GestureName.CallMe:
                    OnRightHandCallMeGesture.Invoke();
                    break;

                default: // Gesture.None or any other gesture
                    OnClearRight.Invoke();
                    break;
            }
        }
        else if (handType == HandType.Left)
        {
            switch (gestureType)
            {
                case GestureName.Okay:
                    OnLeftHandOkayGesture.Invoke();
                    break;
                case GestureName.Peace:
                    OnLeftHandPeaceGesture.Invoke();
                    break;
                case GestureName.Fist:
                    OnLeftHandFistGesture.Invoke();
                    break;

                case GestureName.ThumbsUp:
                    OnLeftHandThumbsUpGesture.Invoke();
                    break;

                case GestureName.Palm:
                    OnLeftHandPalmGesture.Invoke();
                    break;

                case GestureName.Point:
                    OnLeftHandPointGesture.Invoke();
                    break;

                case GestureName.ShootEm:
                    OnLeftHandShootEmGesture.Invoke();
                    break;

                case GestureName.CoverEye:
                    OnLeftHandCoverEyeGesture.Invoke();
                    break;

                case GestureName.Pinky:
                    OnLeftHandPinkyGesture.Invoke();
                    break;

                case GestureName.Horns:
                    OnLeftHandHornsGesture.Invoke();
                    break;

                case GestureName.CallMe:
                    OnLeftHandCallMeGesture.Invoke();
                    break;
                default: // Gesture.None or any other gesture
                    OnClearLeft.Invoke();
                    break;
            }
        }
    }
}
