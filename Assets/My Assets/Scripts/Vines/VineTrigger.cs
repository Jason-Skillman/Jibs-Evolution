using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTrigger : MonoBehaviour {

	public Vine vine;


	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
			vine.Grow();
		}
	}

}
