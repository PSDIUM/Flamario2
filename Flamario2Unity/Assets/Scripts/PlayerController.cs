﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool grounded;

    public float jumpForce = 5; //Force applied to player vertically
    public float jumpTime = 5; //Maximum jump time
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

        grounded = Physics2D.Raycast(transform.position, transform.TransformDirection (Vector3.down), 1, 1<<8);
        
        /* 
        // Raycast testing modified from https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
        if (Physics2D.Raycast(transform.position, transform.TransformDirection (Vector3.down), 1, 1<<8)) 
        {
            Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.down) * 1, Color.yellow);
            Debug.Log("Did Hit");
        } 
        else 
        {
            Debug.DrawRay(transform.position, transform.TransformDirection (Vector3.down) *1, Color.white);
            Debug.Log("Did not Hit");
        }
        // 
        */

        if (grounded)
        {
            Debug.Log("grounded");
            jumpTimer = 0;  //if mario is grounded, reset jump timer
        } else {
            Debug.Log("Not Grounded");
        }

        if (Input.GetButton("Jump"))    //Jumping
        {
            jumpTimer = jumpTimer + Time.deltaTime;
            if (jumpTimer <= jumpTime)  //if the jump timer (jumpTimer) is less than the maximum jump time (jumpTime)
            {
                player.AddForce(transform.up * jumpForce);  //add force upwards
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