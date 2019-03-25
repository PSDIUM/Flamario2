using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPower : MonoBehaviour {

	[SerializeField] private LayerMask collisionLayer;
	private Vector2 direction;
	private float speed = 10;

	private bool hasCollided;
	private float maxTime = 0.1f;
	private float currentTime;

	private void Start() {
		direction.y = -1;
		currentTime = maxTime;
	}

	void Update() {
		Movement();
    }

	public void SetDirection(Vector2 dir) {
		direction.x = dir.x;
	}

	private void Movement() {
		RaycastHit2D ray = GetRaycast(direction);

		if (ray.collider!=null) {
			if (ray.collider.gameObject.tag.Equals("Ground") && !hasCollided) {
				direction.y *= -1;
				hasCollided = true;
			}
			if (ray.collider.gameObject.tag.Equals("Obstacle")) {
				Die();
			}
		}

		if (hasCollided) {
			Bounce();
		}

		Vector2 velocity = speed * direction * Time.deltaTime; ;
		transform.position += new Vector3(velocity.x, velocity.y, 0);
	}

	private void Bounce() {
		currentTime -= Time.deltaTime;
		if (currentTime<=0) {
			currentTime = maxTime;
			direction.y *= -1;
		}
	}

	private RaycastHit2D GetRaycast(Vector2 dir) {
		Vector3 origin = transform.position;
		float distance = transform.lossyScale.x / 2 + 0.2f;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, collisionLayer);
		Debug.DrawRay(origin, dir * distance, Color.yellow);
		return hit;
	}


	private void Die() {
		Destroy(this.gameObject);
	}
}
