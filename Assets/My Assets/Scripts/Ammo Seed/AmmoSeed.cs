using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSeed : MonoBehaviour {

	public int damage = 20;
	public GameObject deathObject;

	
	public void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Death" || collision.gameObject.tag == "Monster") {
			GameObject deathObj = Instantiate(deathObject);
			deathObj.transform.position = transform.position + (Vector3.up * 0.1f);
			Destroy(gameObject);
		}
		if(gameObject.tag == "BulletBad" && collision.gameObject.tag == "Player") {
			Debug.Log("bad hit player");
			GameObject deathObj = Instantiate(deathObject);
			deathObj.transform.position = transform.position + (Vector3.up * 0.1f);

			collision.gameObject.GetComponent<Player_Control>().health -= damage;
            UIManager.main.SetHealth(collision.gameObject.GetComponent<Player_Control>().health, 200);
            Destroy(gameObject);
		}
	}

}
