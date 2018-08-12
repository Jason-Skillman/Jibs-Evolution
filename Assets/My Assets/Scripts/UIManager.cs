using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//Singleton
	public static UIManager main;

	public int lives = 3;

	public Slider healthSlider;
	public Image lives1;
	public Image lives2;
	public Image lives3;


	void Awake() {
		DontDestroyOnLoad(gameObject);

		//Singleton
		if(!main) {
			main = this;
		} else {
			Destroy(gameObject);
		}
	}

	public void LoseLife() {
		lives--;
		if(lives < 0) lives = 0;

		if(lives == 2) {
			lives1.color = new Color(255, 255, 255, 1);
			lives2.color = new Color(255, 255, 255, 1);
			lives3.color = new Color(255, 255, 255, 0);
		} else if(lives == 1) {
			lives1.color = new Color(255, 255, 255, 1);
			lives2.color = new Color(255, 255, 255, 0);
			lives3.color = new Color(255, 255, 255, 0);
		}else if(lives == 0) {
			lives1.color = new Color(255, 255, 255, 0);
			lives2.color = new Color(255, 255, 255, 0);
			lives3.color = new Color(255, 255, 255, 0);
		}
	}

	public void SetHealth(float current, float max) {
		float amount = current / max;
		healthSlider.value = amount;
	}
	
}
