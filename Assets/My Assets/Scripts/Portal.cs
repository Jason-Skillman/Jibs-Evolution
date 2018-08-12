using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject exit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        var nPos = new Vector3(exit.transform.position.x + 3, exit.transform.position.y, col.gameObject.transform.position.z);
        col.gameObject.transform.position = nPos;
    }
}
