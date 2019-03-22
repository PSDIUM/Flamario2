using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public LayerMask collisionLayer;
	private float speed = 10;

	void FixedUpdate() {
		Movement();
	}

	private void Movement() {
		float hInput = Input.GetAxis("Horizontal");
		Vector3 horizontalMovement = new Vector3();

		if (hInput != 0) {
			if (IsColliding(hInput))
				return;

			horizontalMovement += new Vector3(hInput * speed * Time.deltaTime, 0, 0);
		}

		transform.position += horizontalMovement;
	}

	private bool IsColliding(float input) {
		Vector3 dir = input < 0 ? Vector3.left : Vector3.right;
		Vector3 origin = transform.position + (dir * 0.5f);

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, 0.3f * speed * Time.deltaTime, collisionLayer);
		Debug.DrawRay(origin, dir * 0.3f * speed * Time.deltaTime, Color.yellow);
		return hit.collider != null;
	}
}
