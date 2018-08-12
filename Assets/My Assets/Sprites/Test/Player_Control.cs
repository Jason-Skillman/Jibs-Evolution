using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D rb2d;
    //private Transform trans;
    public Animation AtkAnm;
    public bool grounded = true;
    private bool jumping = false;
    private bool doubleJump = false;
    private int edge = 0;
    public bool dead = false;


    public bool attacking = false;
    public float speed;
    public float jumpHeight;
    public bool jump;
    public bool jumpUp;
	public int health = 200;

    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
        rb2d = this.GetComponent<Rigidbody2D>();
        dead = false;
        UIManager.main.SetHealth(health, 200);
        //trans = this.GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead)
        {
            var vertical = Input.GetAxis("Horizontal");
            //var direct = new Vector3(vertical, 0, 0);

            if (jumpUp)
            {
                jump = Input.GetButtonDown("Jump");
                jumpUp = false;
            }
            var atk1 = Input.GetButton("Fire1");
            //moving
            if (vertical > 0)
            {
                animator.SetInteger("direct", 1);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (vertical < 0)
            {
                animator.SetInteger("direct", 1);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                animator.SetInteger("direct", 0);
            }
            //jump
            if (!doubleJump && (jumping || !grounded) && jump)
            {
                doubleJump = true;
                animator.SetBool("jump", true);
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(new Vector2(0, jumpHeight));
            }
            else if (!jumping && grounded && jump)
            {
                jumping = true;
                animator.SetBool("jump", true);
                animator.SetBool("land", false);
                rb2d.AddForce(new Vector2(0, jumpHeight));
            }
            else if (!grounded && !jump && rb2d.velocity.y < 0)
            {
                animator.SetBool("fall", true);
                animator.SetBool("jump", false);
            }
            else if (!grounded)
            {
                animator.SetBool("jump", false);
            }
            else if (jumping && grounded && edge == 1)
            {
                animator.SetBool("land", true);
                animator.SetBool("fall", false);
                jumping = false;
                doubleJump = false;
                edge = 0;
            }
            //atack
            if (atk1 && !attacking)
            {
                attacking = true;
                animator.SetTrigger("Atk");
            }
            //else
            //{
            //    //animator.SetBool("Atk", false);
            //}

            rb2d.position = new Vector3(transform.position.x + (vertical * speed), transform.position.y, 0);
            //transform.Translate(speed * direct * Time.deltaTime);
            //double jump neabler
            jump = false;
            if (Input.GetButton("Jump") == false)
            {
                jumpUp = true;
            }

            if (health <= 0 && !dead)
            {
                dead = true;
                Die();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;

            animator.SetBool("fall", false);

        }
        if(col.gameObject.tag == "Death")
        {
            Die(); 
        }
    }

    private void OnTriggerExit2D(Collider2D col)
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

    private void Die()
    {
        doubleJump = true;
        jumping = true;
        dead = true;
        animator.SetTrigger("Die");
        GameManager.main.live -= 1;
        UIManager.main.LoseLife();
        if (GameManager.main.live < 0)
        {
            SceneFaderManager.main.FadeToScene("Death");
        }
        else
        {
            SceneFaderManager.main.ReloadScene();
        }
    }

}
