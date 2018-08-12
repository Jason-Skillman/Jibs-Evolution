using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss : Monster {

    public GameObject[] points;
    public GameObject player;
    public int maxSeeds;
    public int seed = 0;
   // public int speed;
    public int nextPoint;
    public string next;
    public string text;
    //public int health = 300;
    //public int damage = 30;

    public GameObject seedPrefab;
    public Transform seedSpawnPointRight;
    public Transform seedSpawnPointLeft;
    public int amount = 5;
    public float timer = 0;
    public float time;
    public float countDown = 1;
    public bool started = false;

    //Vectors
    public Vector2 mousePosSP;
    public Vector2 dir = Vector2.zero;

    // Use this for initialization
    public override void Start () {
        base.Start();
        nextPoint = Random.Range(0, 3);
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
        if (Timer() || seed < maxSeeds)
        {
            seed++;
            Fire();
        }
        else
        {
            var step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, points[nextPoint].transform.position, step);
        }
        
        
	}

    void Fire()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Atk");
        mousePosSP = player.transform.position;

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

    public bool Timer()
    {
        timer += Time.deltaTime;
        if (timer >= time)
        {
            timer = 0;
            maxSeeds++;
            seed = 0;
           
            nextPoint = Random.Range(0, 3);
            return true;
            
        }
        else
        {
            return false;
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        SceneFaderManager.main.FadeToScene(next, text);
        Death();
    }

}
