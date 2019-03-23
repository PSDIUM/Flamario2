using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;
    private Vector3 offset;
    public float leftBorderX;
    public float rightBorderX;
    public float smoothDampTime = 0.2F;
    private float smoothDampVel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float targetX = Mathf.Max(leftBorderX, Mathf.Min(rightBorderX, target.transform.position.x));
        if(targetX > transform.position.x) {
            float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVel, smoothDampTime);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        } 
    }
}