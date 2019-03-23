using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    void Update() {
        Movement();
    }

    private void Movement() {
		float hInput = Input.GetAxis("Horizontal");
		if (hInput != 0) {
			if (IsColliding(hInput)){
				return;
            }
			transform.position += new Vector3(hInput * speed * Time.deltaTime, 0, 0);
		}
    }

    private bool IsColliding(float input) {
		Vector3 dir = input < 0 ? Vector3.left : Vector3.right;
		Vector3 origin = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, transform.lossyScale.x/2, 1<<8);
		//Debug.DrawRay(origin, dir * transform.lossyScale.x/2, Color.yellow);
		return hit.collider != null;
    }
}
