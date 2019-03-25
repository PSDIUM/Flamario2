using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerControllerAlt : MonoBehaviour {

    public GameObject gameController;
	private LayerMask obstacleLayer = 1<<9;
    private Rigidbody2D rb;
    private float speed = 10;
	private float jumpSpeed = 15;
	private float jumpVelocity = 0;
	private float gravity = 20;
	private Vector2 currentDirection;
    private bool grounded;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        /* if (Input.GetAxis("Horizontal") != 0) {
        rb.velocity = currentDirection * 5;
        } else {
            rb.velocity = Vector2.zero;
        } */
        Run();
    }

    private void Run() {
		float hInput = Input.GetAxis("Horizontal");
		if (hInput != 0) {
            gameObject.GetComponent<Animator>().SetBool("Moving", true);
			float velocity = hInput * speed * Time.deltaTime;
			Vector2 dir = hInput < 0 ? Vector3.left : Vector3.right;
			currentDirection = dir;
            rb.velocity = currentDirection * 5;
            
            if(dir == Vector2.left && gameObject.GetComponent<SpriteRenderer>().flipX == false) {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            } else if(dir == Vector2.right && gameObject.GetComponent<SpriteRenderer>().flipX == true) {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            
		} else {
            gameObject.GetComponent<Animator>().SetBool("Moving", false);
            rb.velocity = Vector2.zero;
        }
        if(Input.GetButton("Jump") && grounded == true) {
            rb.velocity = new Vector2(rb.velocity.x, 20);
        }
	}

    void OnCollisionStay2D(Collision2D other)
    {
        /* Debug.Log(other.gameObject.name);
        if(other.gameObject.name == "Ground1") {
            grounded = true;
        } else {
            grounded = false;
        } */
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Ground1") {
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.name == "Ground1") {
            grounded = false;
        }
    }
}
