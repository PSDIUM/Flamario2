using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	[SerializeField] protected LayerMask collisionLayer;
	[SerializeField] protected float speed = 5;
	private Vector2 dir;


	private void Start() {
		dir = Vector2.left;
	}

	void Update() {
		Movement();
	}

	private void Movement() {
		if (IsColliding()) {
			dir *= -1;
		}
		transform.position += new Vector3(dir.x * speed * Time.deltaTime, 0, 0);
	}

	private bool IsColliding() {
		Vector3 origin = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, transform.lossyScale.x / 2, collisionLayer);
		//Debug.DrawRay(origin, dir * transform.lossyScale.x/2, Color.yellow);
		return hit.collider != null;
	}
}
