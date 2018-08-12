using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

	public string sceneName;
	public string sceneText;


	public void OnTriggerEnter2D(Collider2D collision) {
		if(collision.gameObject.tag == "Player") {
			if(sceneText == "") {
				SceneFaderManager.main.FadeToScene(sceneName);
			} else {
				SceneFaderManager.main.FadeToScene(sceneName, sceneText);
			}
			Destroy(gameObject);
		}
	}
}
