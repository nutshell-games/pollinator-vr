using UnityEngine;
using System.Collections;

/// <summary>
/// 头鸟决定飞行的整体方向，在unityflock中被origin引用
/// </summary>
public class UnityFlockController : MonoBehaviour
{

    public Vector3 bound;//范围 
    public float speed = 100.0f;
    public Transform swarmTarget;
     //[HideInInspector]
    public Vector3 initialPosition;
    //[HideInInspector]
    public Vector3 nextMovementPoint;

    
	// Use this for initialization
	void Start ()
	{
	    // initialPosition = transform.position;
        // CalculateNextMovementPoint();
	}

    void MovePoint()
    {
        initialPosition = swarmTarget.position;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextMovementPoint - transform.position), 1.0f * Time.deltaTime);//使用插值方式不断调整飞行角度

        //当接近目标点时获取下一个目标点
        if (Vector3.Distance(nextMovementPoint, transform.position) <= 10.0f)
        {
            CalculateNextMovementPoint();
        }
    }

	// Update is called once per frame
	void Update () {

      

    }

    void CalculateNextMovementPoint()
    {
        float posx = Random.Range(initialPosition.x - bound.x, initialPosition.x + bound.x);
        float posy = Random.Range(initialPosition.y - bound.y, initialPosition.y + bound.y);
        float posz = Random.Range(initialPosition.z - bound.z, initialPosition.z + bound.z);

        nextMovementPoint = initialPosition + new Vector3(posx, posy, posz);
    }


}
