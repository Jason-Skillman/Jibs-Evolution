using UnityEngine;

public class SceneFaderTriggger : MonoBehaviour {

	public string sceneToLoad;
	public string popOfText;


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		SceneFaderManager.main.OnFadeOut += OnFadeOutEnd;
		SceneFaderManager.main.OnFadeIn += OnFadeInEnd;
	}

	void Update() {
		if(Input.GetMouseButtonDown(0)) {	//Left click
			SceneFaderManager.main.FadeToScene(sceneToLoad);
		}
		if(Input.GetMouseButtonDown(1)) {	//Right click
			SceneFaderManager.main.FadeToScene(sceneToLoad, popOfText);
		}
	}

	public void OnFadeOutEnd() {
		Debug.Log("OnFadeOutEnd");
	}

	public void OnFadeInEnd() {
		Debug.Log("OnFadeInEnd");
	}

}