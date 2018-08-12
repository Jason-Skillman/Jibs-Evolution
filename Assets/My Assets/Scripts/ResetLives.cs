using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLives : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.main.live = 3;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
