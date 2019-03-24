using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPower : MonoBehaviour {

	[SerializeField] private LayerMask collisionLayer;
	private Vector2 direction;
	private float speed = 10;

	private void Start() {
		direction = new Vector2(1,-1);
	}

	void Update() {
		Movement();
    }

	private void Movement() {
		RaycastHit2D ray = GetRaycast(direction);

		if (ray.collider!=null) {
			direction.y *= -1;
		}

		Vector2 velocity = speed * direction * Time.deltaTime; ;
		transform.position += new Vector3(velocity.x, velocity.y, 0);
	}

	private RaycastHit2D GetRaycast(Vector2 dir) {
		Vector3 origin = transform.position;
		float distance = transform.lossyScale.x / 2 + 0.2f;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, collisionLayer);
		Debug.DrawRay(origin, dir * distance, Color.yellow);
		return hit;
	}

}
