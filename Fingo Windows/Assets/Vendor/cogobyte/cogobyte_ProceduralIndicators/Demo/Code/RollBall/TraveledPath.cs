using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralIndicators;

//Stores the path travelled by ball and draws an arrow along that path
public class TraveledPath : MonoBehaviour {
    public ArrowObject arrowObject;
    public Transform ball;
    List<Vector3> path;
    public bool traveling = false;
    public void StartTravel()
    {
        traveling = true;
        path = new List<Vector3>();
        path.Add(ball.position+new Vector3(0,1,0));
        arrowObject.arrowHead.arrowTipMode = ArrowTip.ArrowTipMode.Extrude;
        arrowObject.arrowPath.arrowPathType = ArrowPath.ArrowPathType.PointArray;
        arrowObject.arrowTail.arrowTipMode = ArrowTip.ArrowTipMode.Extrude;
        arrowObject.arrowPath.arrowPathMode = ArrowPath.ArrowPathMode.BrokenExtrude;
    }

    public void EndTravel()
    {
        traveling = false;
        arrowObject.arrowHead.arrowTipMode = ArrowTip.ArrowTipMode.None;
        arrowObject.arrowTail.arrowTipMode = ArrowTip.ArrowTipMode.None;
        arrowObject.arrowPath.arrowPathMode = ArrowPath.ArrowPathMode.None;
        arrowObject.updateArrowMesh();
    }

    // Update is called once per frame
    void Update () {
        if (traveling)
        {
            if (((ball.position + new Vector3(0, 1, 0)) - path[path.Count - 1]).magnitude > 0.1f)
            {
                path.Add(ball.position + new Vector3(0, 1, 0));
                arrowObject.arrowPath.editedPath = path;
                arrowObject.updateArrowMesh();
            }
        }
    }
}
