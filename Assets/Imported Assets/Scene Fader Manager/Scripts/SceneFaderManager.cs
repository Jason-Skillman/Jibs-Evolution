using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFaderManager : MonoBehaviour {

	//Singleton
	public static SceneFaderManager main;

	//Delegates
	public delegate void Callback();
	public Callback OnFadeOut, OnFadeIn;

	public Text text;

	private Animator animator;
	public string levelName;
	private string popUpText;
	

	void Awake() {
		DontDestroyOnLoad(gameObject);

		//Singleton
		if(!main) {
			main = this;
		} else {
			Destroy(gameObject);
		}

		animator = GetComponent<Animator>();
	}

	void Start() {
		//Animation is already set to run once the very first time the game starts, so we used the Start message to prevent it from running twice in the beginning.
		SceneManager.sceneLoaded += OnLevelLoaded;
        levelName = SceneManager.GetActiveScene().name;
	}

	///<summary>Main method to call when you what to fade the screen and load the next level</summary>
	///<param name="levelName">Level to load as a string</param>
	public void FadeToScene(string levelName, string popUpText = null) {
		this.levelName = levelName;
		this.popUpText = popUpText;

		if(Application.CanStreamedLevelBeLoaded(levelName)) {
			animator.SetTrigger("FadeOut");
		} else {
			Debug.LogError("SceneFaderManager: Level/Scene " + levelName + " does not exist");
		}
	}

    public void ReloadScene()
    {
        this.popUpText = null;
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            animator.SetTrigger("FadeOut");
        }
        else
        {
            Debug.LogError("SceneFaderManager: Level/Scene " + levelName + " does not exist");
        }
    }

    ///<summary>Called externaly throught the animation controller when the FadeOut animation has ended</summary>
    private void OnFadeOutComplete() {
		if(OnFadeOut != null)
			OnFadeOut();

		if(popUpText != null) {
			text.text = popUpText;
			animator.SetTrigger("FadeText");
		} else {
			SceneManager.LoadScene(levelName);
		}
	}

	///<summary>Called externaly throught the animation controller when the FadeIn animation has ended</summary>
	private void OnFadeInComplete() {
		if(OnFadeIn != null)
			OnFadeIn();
	}

	///<summary>Called externaly throught the animation controller when the FadeText animation has ended</summary>
	private void OnFadeTextComplete() {
		SceneManager.LoadScene(levelName);
	}

	///<summary>Called internaly throught the Unity SceneManagement delegate when the scene has finished loading</summary>
	private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode) {
		animator.SetTrigger("FadeIn");
	}

}