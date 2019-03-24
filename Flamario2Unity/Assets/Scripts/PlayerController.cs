using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private int collisionLayer = 1<<9;
	private PlayerStates playerState;
	private float speed = 10;
	private float jumpSpeed = 15;
	private float jumpVelocity = 0;
	private float gravity = 20;

	private void Start() {
		playerState = PlayerStates.IDLE;
	}

	void Update() {
        Movement();
    }

    private void Movement() {
		Run();
		Jump();
	}

	private void Run() {
		float hInput = Input.GetAxis("Horizontal");
		if (hInput != 0) {
			if (IsColliding(hInput < 0 ? Vector3.left : Vector3.right)) {
				return;
			}
			transform.position += new Vector3(hInput * speed * Time.deltaTime, 0, 0);
		}
	}

	private void Jump() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (playerState != PlayerStates.JUMPING) {
				playerState = PlayerStates.JUMPING;
				jumpVelocity = jumpSpeed;
			}
		}
		if (playerState == PlayerStates.JUMPING) {
			jumpVelocity -= gravity * Time.deltaTime;
			transform.position += new Vector3(0, jumpVelocity * Time.deltaTime, 0);

			if (IsColliding(Vector2.up)) {
				jumpVelocity = 0;
			} 
			else if (IsColliding(Vector2.down)) {
				playerState = PlayerStates.IDLE;
				return;
			}
		}
	}

	private bool IsColliding(Vector2 dir) {
		//Vector3 dir = input < 0 ? Vector3.left : Vector3.right;
		Vector3 origin = transform.position;

		RaycastHit2D hit = Physics2D.Raycast(origin, dir, transform.lossyScale.x/2, collisionLayer);
		Debug.DrawRay(origin, dir * transform.lossyScale.x/2, Color.yellow);
		return hit.collider != null;
    }

	private bool IsFalling() {

		return true;
	}

	private enum PlayerStates {
		IDLE,
		WALKING,
		JUMPING
	}
}