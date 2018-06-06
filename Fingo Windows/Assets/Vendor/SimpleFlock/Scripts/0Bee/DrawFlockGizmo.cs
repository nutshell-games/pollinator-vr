using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DrawFlockGizmo : MonoBehaviour
{
    public UnityFlockController UFController;
    [Range(0f,5f)]
    public float GizmosSize=0.2f;
    private  bool isStart;
    private Vector3 boundPos;
    private Vector3 headerPos;
    private Vector3 nextPos;
    void Start()
    {
        isStart = true;
    }
    //[DrawGizmo(GizmoType.Active|GizmoType.InSelectionHierarchy|GizmoType.Pickable|GizmoType.Selected)]
    private  void OnDrawGizmos()
    {
        if (!isStart)
            boundPos = UFController.transform.position;
        headerPos = UFController.transform.position;
        nextPos = UFController.nextMovementPoint;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boundPos,new Vector3(UFController.bound.x*2,UFController.bound.y*2,UFController.bound.z*2));

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(headerPos,GizmosSize);

        Gizmos.color = Color.green;
        Gizmos.DrawCube(nextPos, Vector3.one*GizmosSize);

        Gizmos.color=Color.white;
        Gizmos.DrawLine(headerPos,nextPos);
    }



}
