﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    public GameObject player;
    public LayerMask layer;
    public float movement, jumpForce;
    public bool onGround, onGroundCoyote, underSomething, toLeft, toRight;
    [Header("Audio Sources")]
    public AudioSource jumpAudio;

    [SerializeField] private Animator _animator;
    private string animationCurrentState;
    private string walkAnim = "run";
    private string jumpAnim = "jump";
    private string idleAnim = "idle";

    [SerializeField] private float coyoteTime, coyoteTimeSet;
    // COYOTETIMESET MUST BE 0.07 FOR A PERFECT MATCH WITH THIS GAME (RAYCAST DOWNWARDS)


    void Start()
    {
             
        animationCurrentState = idleAnim;

    }

    void Update()
    {
        float xFromAxis = Input.GetAxisRaw("Horizontal");
        // float yFromAxis = Input.GetAxisRaw("Vertical"); // WONT BE USED BECAUSE WE HAVE THE RIGID FORCE APPLIED ON INPUT.GETKEY W 

        // GAMEOBJECT MOVEMENT DIRECTION
        Vector2 directionToMoveFromAxis = new Vector2(xFromAxis, 0f).normalized;

        // TO AVOID GOING THROUGH A WALL WHEN WALKING TOWARDS ONE
        if (toRight && directionToMoveFromAxis.x >= 0)
        {
            directionToMoveFromAxis.x = 0;
        }

        else if (toLeft && directionToMoveFromAxis.x <= 0)
        {
            directionToMoveFromAxis.x = 0;
        }

        // GAMEOBJECT MOVEMENT
        transform.position = new Vector2(transform.position.x, transform.position.y) + directionToMoveFromAxis * movement * Time.deltaTime;


        // CHECKING DOWNWARDS FOR COYOTE DISTANCE TO GROUND
        RaycastHit2D[] hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up, 4);

        foreach (RaycastHit2D hit in hitsDown)
        {
            if (hit.transform.tag == "ground")
            {
                Debug.DrawRay(transform.position, -Vector2.up * hit.distance, Color.red);
                onGroundCoyote = true;
            }
            else
            {
                onGroundCoyote = false;
            }
            
        }

        // CHECKING UPWARDS RAYCAST FOR NO JUMP UNDER SOMETING
        RaycastHit2D[] hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up, 3);

        foreach (RaycastHit2D hit in hitsUp)
        {
            if (onGround)
            {
                if (hit.transform.tag == "ground")
                {
                    Debug.DrawRay(transform.position, Vector2.up * hit.distance, Color.red);
                    underSomething = true;
                }
                else
                {
                    underSomething = false;
                }
            }

        }

        //CHECKING IF NEXT TO A WALL == LEFT
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 0.7f);

        foreach (RaycastHit2D hit in hitsLeft)
        {
            if (hit.transform.tag == "ground")
            {
                Debug.DrawRay(transform.position, Vector2.left * hit.distance, Color.red);
                toLeft = true;
            }
            else
            {
                toLeft = false;
            }

        }

        //CHECKING IF NEXT TO A WALL == RIGHT
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, 0.7f);

        foreach (RaycastHit2D hit in hitsRight) 
        {
            if (hit.transform.tag == "ground")
            {
                Debug.DrawRay(transform.position, Vector2.right * hit.distance, Color.red);
                toRight = true;
            }
            else
            {
                toRight = false;
            }

        }


        // JUMP ACTION ON GROUND AND WITH COYOTETIME
        if (Input.GetKeyDown("w") && onGround || Input.GetKeyDown("w") && coyoteTime >= 0 && !onGroundCoyote)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0, jumpForce, 0f);
            jumpAudio.Play();

        }

        // SIMPLE FLIP
        else if (Input.GetKey("d")) { GetComponent<SpriteRenderer>().flipX = false; }

        // SIMPLE FLIP
        else if (Input.GetKey("a")) { GetComponent<SpriteRenderer>().flipX = true; }

        // IDLE
        if (onGround && !Input.GetKey("d") && !Input.GetKey("a"))
        {
            
            changeAnimationStateTo(idleAnim);

        }

        // IF RUNNING ON GROUND
        if (onGround && (Input.GetKey("d") || Input.GetKey("a")) && (!toLeft || !toRight)) //|| onGround && (Input.GetKey("d") || Input.GetKey("a")) && !toRight)
        {
            changeAnimationStateTo(walkAnim);
        }

        // ON AIR
        // not underSomething
        if (!onGround && !underSomething)
        {
            coyoteTime -= Time.deltaTime;
            changeAnimationStateTo(jumpAnim);

        }
        // underSomething
        else if (!onGround && underSomething)
        {
            coyoteTime -= Time.deltaTime;
            changeAnimationStateTo(idleAnim);
        }
        // restarting the coyote when touching the ground
        else if (onGround) { coyoteTime = coyoteTimeSet; }

         


    }

    private void changeAnimationStateTo(string newState)
    {
        if (animationCurrentState == newState) { return; }

        _animator.Play(newState);

        animationCurrentState = newState;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
        {
            //Debug.Log(collision.gameObject.GetComponent<thisScriptable>().itemInformation.life);
            Debug.Log("x");
            collision.GetComponent<CircleCollider2D>().enabled = false;
            collision.GetComponent<thisScriptable>().touchedByPlayer();
        }
    }
}
