using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

	public bool isInvinsible = false;
	public int health = 100;
	public float speed = 3;
	public int damage = 25;
    public GameObject drop;

	protected new Rigidbody2D rigidbody2D;
	protected SpriteRenderer spriteRenderer;
	protected Animator animator;

	public virtual void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	public virtual void Update() {
		if(health <= 0) {
			OnDeath();
		}
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if(!isInvinsible) {
            if(collision.gameObject.tag != "BulletBad")
            {
                AmmoSeed ammoSeed = collision.gameObject.GetComponent<AmmoSeed>();
                if (ammoSeed)
                {
                    health -= ammoSeed.damage;
                }

            }

            if (collision.gameObject.tag == "Player") {
                
				collision.gameObject.GetComponent<Player_Control>().health -= damage;
                UIManager.main.SetHealth(collision.gameObject.GetComponent<Player_Control>().health, 200);
            }
		}
	}

	public virtual void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "AIWall") {
			Flip();
		}
	}


	public virtual void OnDeath() {
		speed = 0;
		damage = 0;
		Destroy(rigidbody2D);
		Destroy(gameObject.GetComponent<BoxCollider2D>());
		animator.SetBool("IsAlive", false);
        //Death();
	}

	public virtual void Flip() {
		spriteRenderer.flipX = !spriteRenderer.flipX;
		speed = -speed;
	}

	public void Death() {
        if(drop != null)
        {
            Instantiate(drop);
        }
        print("ded");
		Destroy(gameObject);
	}

}
