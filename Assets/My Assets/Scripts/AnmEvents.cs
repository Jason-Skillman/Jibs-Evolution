using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnmEvents : MonoBehaviour {

    public GameObject seedPrefab;
    public Transform seedSpawnPointRight;
    public Transform seedSpawnPointLeft;
    public int amount = 5;
    public float timer = 0;
    public float countDown = 1;
    public bool started = false;

    //Vectors
    public Vector2 mousePosSP;
    public Vector2 dir = Vector2.zero;


    void stopAtk()
    {
        gameObject.GetComponent<Player_Control>().attacking = false;
    }

    void fireSeed()
    {
        mousePosSP = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject seed = Instantiate(seedPrefab);

        dir = (mousePosSP - (Vector2)transform.position).normalized;

        if (dir.x > 0)
        { //Right
            seed.transform.position = seedSpawnPointRight.position;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {    //Left
            seed.transform.position = seedSpawnPointLeft.position;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        dir = new Vector2(dir.x * 1.5f, dir.y);



        seed.GetComponent<Rigidbody2D>().AddForce(dir * amount, ForceMode2D.Impulse);
    }
}
