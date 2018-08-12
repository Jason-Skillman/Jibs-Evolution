using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGetRid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(UIManager.main.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
