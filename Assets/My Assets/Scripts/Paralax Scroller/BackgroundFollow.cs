using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour {

	public GameObject background;
	public GameObject targetCamera;


	void Update () {

		Vector2 newVector = background.transform.position;
		newVector.x = targetCamera.transform.position.x;
		newVector.y = targetCamera.transform.position.y;
		background.transform.position = newVector;
	}
}
