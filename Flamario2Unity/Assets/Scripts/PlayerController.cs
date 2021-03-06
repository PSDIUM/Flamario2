﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject gameController;
	[SerializeField] private LayerMask collisionLayer;
	private PlayerStates playerState;

	private Collisions[] collisions;
	private float speed = 7.5f;
	public float jumpSpeed = 15;
	private float jumpVelocity = 0;
	private float gravity = 35;
    [SerializeField]  private bool grounded;
	private Vector2 currentDirection;

	//Powers
	[SerializeField] private GameObject flowerPower;
	private bool canPower = true;
	private bool hasFlowerPower = true;
	private bool isBig = false;
    public int powerLevel = 1;

    private void Start() {
		playerState = PlayerStates.IDLE;
		currentDirection = Vector2.right;
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
		Powers();
		Movement();
	}

	private void Powers() {
		if (Input.GetKeyDown(KeyCode.P) && hasFlowerPower && canPower) {
			GameObject projectile = Instantiate(flowerPower, transform.position, Quaternion.identity);
			projectile.GetComponent<FlowerPower>().SetDirection(currentDirection);
			canPower = false;
			StartCoroutine(ResetPower());
		}
	}

	private IEnumerator ResetPower() {
		float t = 0.5f;
		float elapsedTime = 0;
		while (elapsedTime < t) {
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		canPower = true;
	}

	private void Movement() {
        if(PlayerPrefs.GetInt("InMenu") == 0) {
            Run();
		    Jump();
		    Fall();
        }
	}

	private void Run() {
		float hInput = Input.GetAxis("Horizontal");
		if (hInput != 0) {
            gameObject.GetComponent<Animator>().SetBool("Moving", true);
			float velocity = hInput * speed * Time.deltaTime;
			Vector2 dir = hInput < 0 ? Vector3.left : Vector3.right;
			currentDirection = dir;
            
            if(dir == Vector2.left && gameObject.GetComponent<SpriteRenderer>().flipX == false) {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            } else if(dir == Vector2.right && gameObject.GetComponent<SpriteRenderer>().flipX == true) {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

			RaycastHit2D ray = GetRaycast(dir);

			if (ray.collider != null) {
				if (!GetCollision(dir)) {
					float movement = ray.distance - (transform.lossyScale.x / 2);
					transform.position += new Vector3(movement * dir.x, 0, 0);
					SetCollisions(dir, true);

                    if (ray.collider.gameObject.tag.Equals("Goomba") || ray.collider.gameObject.tag.Equals("Koopa"))     //collide with enemy horizontal (kill)
                    {
                        PowerLevel(0);
                    }
                }
				return;
			} else {
				SetCollisions(dir, false);
			}

			transform.position += new Vector3(velocity, 0, 0);
		} else {
            gameObject.GetComponent<Animator>().SetBool("Moving", false);
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
            gameObject.GetComponent<Animator>().SetBool("Jumping", true);
			if (!GetCollision(Vector2.up)) {
				RaycastHit2D ray = GetRaycast(Vector2.up);
				if (ray.collider != null) {
					CheckBlock(ray.collider.gameObject);
					jumpVelocity = 0;
					SetCollisions(Vector2.up, true);
                }
			}
		} else {
            gameObject.GetComponent<Animator>().SetBool("Jumping", false);
        }
	}

	private void CheckBlock(GameObject block) {
		switch (block.tag) {
			case "Brick" :
				if (!isBig) {
					block.GetComponent<BlockController>().HitAnimation();
				} else {
					Destroy(block);
				}
				break;
			case "QuestionBlock" :
				block.GetComponent<BlockController>().HitAnimation();
				break;
		}
	}

	private void Fall() {
		if (playerState == PlayerStates.JUMPING) {

			jumpVelocity -= gravity * Time.deltaTime;
            transform.position += new Vector3(0, jumpVelocity * (Time.deltaTime * 1.5f), 0);

			RaycastHit2D ray = GetRaycast(Vector2.down);

			if (ray.collider != null) {
				if (!GetCollision(Vector2.down)) {
					float movement = ray.distance - (transform.lossyScale.x / 2);
                    transform.position += new Vector3(0, -movement * Time.deltaTime, 0);
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 3;
                    SetCollisions(Vector2.down, true);
					SetCollisions(Vector2.up, false);
					playerState = PlayerStates.IDLE;
                    grounded = true;
				}
				if (ray.collider.gameObject.tag.Equals("Goomba")) {
					Destroy(ray.collider.gameObject);
				}
				return;
			}
            else
            {
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
                grounded = false;
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
		for (int x = 0; x < collisions.Length; x++) {
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

    private void PowerLevel(int change)
    {

        if (powerLevel + change > powerLevel)
        {
            powerLevel = change;
            gameObject.GetComponent<Animator>().SetInteger("Power", powerLevel);
        }
        if (powerLevel > 1)
        {
            isBig = true;
        }
        else
        {
            isBig = false;
        }

        if (change == 0)
        {
            StartCoroutine(gameController.GetComponent<GameController>().Death());
        }

        if (powerLevel == 3)
        {
            hasFlowerPower = true;
        }
        else
        {
            hasFlowerPower = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name.Equals("BottomBorder")) {
            StartCoroutine(gameController.GetComponent<GameController>().Death());
        }
    }

}