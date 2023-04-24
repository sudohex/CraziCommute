using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
Rigidbody rb;
[SerializeField] float forwardSpeed = 3f;
[SerializeField] float horizontalSpeed = 5f;
[SerializeField] float jumpHeight = 5f;
[SerializeField] Transform groundCheck;
[SerializeField] float groundDistance = 0.4f;
[SerializeField] LayerMask groundMask;
[SerializeField] AudioSource jumpSound;
[SerializeField] AudioSource backgroundMusic;
private Vector2 fingerDown;
private Vector2 fingerUp;
private bool detectSwipeOnlyAfterRelease = false;

// Start is called before the first frame update
void Start()
{
    rb = GetComponent<Rigidbody>();      
}

// Update is called once per frame
void Update()
{
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            fingerUp = touch.position;
            fingerDown = touch.position;
        }

        if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
        {
            fingerDown = touch.position;
            CheckSwipeDirection();
        }

        if (touch.phase == TouchPhase.Ended)
        {
            fingerDown = touch.position;
            CheckSwipeDirection();
        }
    }

    float horiInput = Input.GetAxis("Horizontal");
    float vertiInput = Input.GetAxis("Vertical");
    rb.velocity = new Vector3(horiInput * horizontalSpeed,rb.velocity.y,forwardSpeed);

    if (Input.GetButtonDown("Jump") && isGrounded())
    {
        jumpSound.Play();
        rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
    }
}

bool isGrounded()
{
    return Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
}

void CheckSwipeDirection()
{
    Vector2 swipeDirection = fingerDown - fingerUp;

    if (swipeDirection.magnitude < 20)
        return;

    float x = swipeDirection.x;
    float y = swipeDirection.y;

    if (Mathf.Abs(x) > Mathf.Abs(y))
    {
        if (x > 0)
        {
            Debug.Log("Right Swipe");
            // move player right
            rb.velocity = new Vector3(horizontalSpeed, rb.velocity.y, forwardSpeed);
        }
        else
        {
            Debug.Log("Left Swipe");
            // move player left
            rb.velocity = new Vector3(-horizontalSpeed, rb.velocity.y, forwardSpeed);
        }
    }
    else
    {
        if (y > 0)
        {
            Debug.Log("Up Swipe");
            // make player jump
            if (isGrounded())
            {
                jumpSound.Play();
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
        }
    }

    fingerUp = fingerDown;
}
}