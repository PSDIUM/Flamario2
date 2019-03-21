using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool grounded;

    public float jumpForce = 5; //Force applied to player vertically
    public float jumpTime = 5; //How long a jump will last
    public Transform groundCheck;     //Where to check for ground

    private float input;
    private float jumpTimer;    //initialise timer for jump
    private GameObject mario;   //actual mario game object
    private Rigidbody2D player; //player's RigidBody
    private Vector3 newScale;   //used to flip player

    private void Start()
    {
        mario = GameObject.Find("Mario");
        player = GetComponent<Rigidbody2D>();   //find rigidbody
        newScale = player.transform.localScale; //get current transform info
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        input = Input.GetAxis("Horizontal");    //Horizontal movement
        if (input != 0)
        {
            transform.position += new Vector3(input * speed * Time.deltaTime, 0, 0);

            FlipPlayer();
        }

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (grounded)
        {
            jumpTimer = 0;
        }

        if (Input.GetButton("Jump"))    //Jumping
        {
            jumpTimer = jumpTimer + Time.deltaTime;
            if (jumpTimer <= jumpTime)
            {
                player.AddForce(transform.up * jumpForce);
                Debug.Log("junp");
            }
        }
    }

    private void FlipPlayer()
    {
        //flipping sprite
        if (input > 0)
        {
            newScale.x = 1;
        }
        else if (input < 0)
        {
            newScale.x = -1;
        }
        mario.transform.localScale = newScale;
    }
}