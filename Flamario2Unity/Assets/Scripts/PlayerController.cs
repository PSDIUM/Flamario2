using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed; 
    
    void Update() {
        Movement();
    }

	private void Movement() {
		float input = Input.GetAxis("Horizontal");
		if (input!=0) {
			transform.position += new Vector3(input * speed * Time.deltaTime, 0, 0);
		}
	}
}
