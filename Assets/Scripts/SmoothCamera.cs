using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour 
{
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
    public float offsetx = 0f;
    public float offsety = 0f;
    public GameObject BackGround;
    public WheelJoint2D sc;

    // Update is called once per frame
    void FixedUpdate () 
	{
		if (target)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y + 0.75f, target.position.z));
			Vector3 delta = new Vector3(target.position.x + offsetx, target.position.y + 0.75f + offsety, target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;


			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

        Vector3 BGPos = new Vector3(BackGround.transform.position.x, BackGround.transform.position.y, BackGround.transform.position.z);
        if(sc.motor.motorSpeed <= -350)
        {
            BGPos.x = Camera.main.transform.position.x + 1f;
        }
        else if(sc.motor.motorSpeed >= 150)
        {
            BGPos.x = Camera.main.transform.position.x - 1f;
        }
        
        BackGround.transform.position = Vector3.SmoothDamp(BackGround.transform.position, BGPos, ref velocity, 1f);

    }
}
