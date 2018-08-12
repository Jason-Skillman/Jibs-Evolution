using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Monster {

	public int fireSpeed = 3;
	public GameObject player;
	public GameObject ammoSeedPrefab;
	public GameObject spawnRight;
	public GameObject spawnLeft;
	public float timer = 0;
    public float time = 2f;

	public override void Start() {
		base.Start();
	}

	public override void Update() {
		base.Update();
		
        if(rigidbody2D != null)
        {
            rigidbody2D.velocity = Vector3.zero;
            rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Impulse);

            if (!spriteRenderer.flipX)
            { //Right
                if (Timer())
                {
                    GameObject ammoSeed = Instantiate(ammoSeedPrefab);
                    ammoSeed.transform.position = spawnRight.transform.position;
                    ammoSeed.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 8, ForceMode2D.Impulse);

                }
            }
            else
            {    //Left
                if (Timer())
                {
                    GameObject ammoSeed = Instantiate(ammoSeedPrefab);
                    ammoSeed.transform.position = spawnLeft.transform.position;
                    ammoSeed.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 8, ForceMode2D.Impulse);
                }
            }
        }
		
		
	}

	public override void Flip() {
		base.Flip();

	}

    public bool Timer()
    {
        timer += Time.deltaTime;
        if(timer >= time)
        {
			timer = 0;
			return true;
        }
        else
        {
            return false;
        }
    }

}
