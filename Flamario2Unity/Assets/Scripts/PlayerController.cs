using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private int collisionLayer = 1<<9;
	private PlayerStates playerState;

	private Collisions[] collisions;
	private float speed = 10;
	private float jumpSpeed = 15;
	private float jumpVelocity = 0;
	private float gravity = 20;



	private void Start() {
		playerState = PlayerStates.IDLE;
		SetCollisions();
	}

	private void SetCollisions() {
		collisions = new Collisions[4];
		collisions[0] = new Collisions(Vector2.left, false);
		collisions[1] = new Collisions(Vector2.up, false);
		collisions[2] = new Collisions(Vector2.right, false);
		collisions[3] = new Collisions(Vector2.down, true);
	}

	void FixedUpdate() {
        Movement();
    }

    private void Movement() {
		Run();
		Jump();
		Fall();

	}

	private void CheckCollision() {

	}

	private void Run() {
		float hInput = Input.GetAxis("Horizontal");
		if (hInput != 0) {
			float velocity = hInput * speed * Time.deltaTime;
			Vector2 dir = hInput < 0 ? Vector3.left : Vector3.right;

			RaycastHit2D ray = GetRaycast(dir);

			if (ray.collider!=null) {
				if (!GetCollision(dir)) {
					float movement = ray.distance - (transform.lossyScale.x / 2);
					transform.position += new Vector3(movement, 0, 0);
					SetCollisions(dir, true);
				}
				return;
			} else {
				SetCollisions(dir, false);
			}

			transform.position += new Vector3(velocity, 0, 0);
		}
	}

	private void Jump() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (playerState != PlayerStates.JUMPING) {
				playerState = PlayerStates.JUMPING;
				jumpVelocity = jumpSpeed;
				SetCollisions(Vector2.down, false);
			}
		}
		if (playerState == PlayerStates.JUMPING) {
			if (!GetCollision(Vector2.up)) {
				RaycastHit2D ray = GetRaycast(Vector2.up);
				if (ray.collider != null) {
					jumpVelocity = 0;
					SetCollisions(Vector2.up, true);
				}
			}
		}
	}

	private void Fall() {
		if (playerState == PlayerStates.JUMPING) {

			jumpVelocity -= gravity * Time.deltaTime;
			transform.position += new Vector3(0, jumpVelocity * Time.deltaTime, 0);

			RaycastHit2D ray = GetRaycast(Vector2.down);

			if (ray.collider != null) {
				if (!GetCollision(Vector2.down)) {
					float movement = ray.distance - (transform.lossyScale.x / 2);
					transform.position += new Vector3(0, -movement, 0);
					SetCollisions(Vector2.down, true);
					SetCollisions(Vector2.up, false);
					playerState = PlayerStates.IDLE;
				}
				return;
			}
		}
	}

	/// <summary> Returns a raycast for the specified direction
	/// </summary>
	private RaycastHit2D GetRaycast(Vector2 dir) {
		Vector3 origin = transform.position;
		float distance = transform.lossyScale.x / 2 + 0.2f;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, collisionLayer);
		Debug.DrawRay(origin, dir * distance, Color.yellow);
		return hit;
	}

	/// <summary> Returns a bool of raycast collision success
	/// </summary>
	private bool IsColliding(Vector2 dir) {
		Vector3 origin = transform.position;
		float distance = transform.lossyScale.x / 2 + 0.2f;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, collisionLayer);
		Debug.DrawRay(origin, dir * distance, Color.yellow);
		return hit.collider != null;
	}

	private bool GetCollision(Vector2 dir) {
		for (int x=0; x<collisions.Length; x++) {
			if (collisions[x].dir == dir) {
				return collisions[x].isColliding;
			}
		}
		return false;
	}

	private void SetCollisions(Vector2 dir, bool state) {
		for (int x = 0; x < collisions.Length; x++) {
			if (collisions[x].dir == dir) {
				collisions[x].isColliding = state;
			}
		}
	}

	private enum PlayerStates {
		IDLE,
		WALKING,
		JUMPING,
		FALLING
	}

	private struct Collisions {
		public Vector2 dir;
		public bool isColliding;

		public Collisions(Vector2 dir, bool isColliding) {
			this.dir = dir;
			this.isColliding = isColliding;
		}
	}
}