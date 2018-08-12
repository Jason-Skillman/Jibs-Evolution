using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamaControlScript : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb2d;
    private bool dashStop = false;
    public bool grounded = true;
    private bool jumping = false;
    private int edge = 0;
    public float speed;
    public float jumpHeight;


    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb2d = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        var vertical = Input.GetAxis("Horizontal");
        var jump = Input.GetButton("Jump");
        var dash = Input.GetButton("Dash");
        var atk1 = Input.GetButton("Fire1");
        if (dash && vertical != 0 && grounded)
        {
            if (!dashStop)
            {
                animator.SetInteger("Dash", 1);
                vertical *= 3;
                rb2d.AddForce(new Vector2(0, 150f));
                dashStop = true;
                
            }

        }
        else
        {
            animator.SetInteger("Dash", 0);
            dashStop = false;
            
        }
        if (vertical > 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else if (vertical < 0)
        {
            animator.SetInteger("Direction", 2);
        }
        else
        {
            animator.SetInteger("Direction", 0);
        }
        if (!jumping && grounded && jump && !dashStop)
        {
            jumping = true;
            animator.SetBool("Jump", true);
            animator.SetBool("Land", false);
            rb2d.AddForce(new Vector2(0, jumpHeight));
        }
        else if (!grounded)
        {
            animator.SetBool("Jump", false);
        }
        else if(jumping && grounded && edge == 1)
        {
            animator.SetBool("Land", true);
            jumping = false;
            edge = 0;
        }

        if (atk1)
        {
            animator.SetBool("Attack1", true);
        }
        else
        {
            animator.SetBool("Attack1", false);
        }

        rb2d.position = new Vector3(transform.position.x + (vertical * speed), transform.position.y, 0);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
            
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = false;
            if (jumping)
            {
                edge = 1;
            }
        }
    }

}
