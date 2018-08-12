using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxScroller : MonoBehaviour {

	public float backgroundSize = 1;
	public float paralaxSpeedX = -0.5f;
	public float paralaxSpeedY = -0.5f;
	public GameObject player;

	private Transform cameraTransform;
	private Transform[] layers;
	private float viewZone = 10;
	private int leftIndex;
	private int rightIndex;
	private float lastCameraX;
	private float lastCameraY;
	private float startY;


	void Start() {
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		lastCameraY = cameraTransform.position.y;
		startY = transform.position.y;
		//Debug.Log(startY);

		layers = new Transform[transform.childCount];
		for(int i = 0; i < transform.childCount; i++) {
			layers[i] = transform.GetChild(i);
		}
		leftIndex = 0;
		rightIndex = layers.Length - 1;
	}

	void Update() {

		float deltaX = cameraTransform.position.x - lastCameraX;
		float deltaY = cameraTransform.position.y - lastCameraY;

		transform.position += Vector3.right * (deltaX * paralaxSpeedX);
		transform.position += Vector3.up * (deltaY * paralaxSpeedY);

		lastCameraX = cameraTransform.position.x;
		lastCameraY = cameraTransform.position.y;

		//Vector2 newVector = new Vector2(transform.position.x, player.transform.position.y);
		//transform.position = newVector;
		
		if(cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone)) {
			ScrollLeft();
		}
		if(cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone)) {
			ScrollRight();
		}
		
	}

	private void ScrollLeft() {
		int lastRight = rightIndex;

		Vector3 newVector = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
		newVector.y = startY;
		layers[rightIndex].position = newVector;

		leftIndex = rightIndex;
		rightIndex--;
		if(rightIndex < 0) {
			rightIndex = layers.Length - 1;
		}
	}

	private void ScrollRight() {
		int lastLeft = leftIndex;

		Vector3 newVector = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
		newVector.y = startY;
		layers[leftIndex].position = newVector;

		rightIndex = leftIndex;
		leftIndex++;
		if(leftIndex == layers.Length) {
			leftIndex = 0;
		}
	}
	
}
