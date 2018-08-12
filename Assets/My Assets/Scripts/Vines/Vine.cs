using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour {

	private Animator animator;

	public void Start() {
		animator = GetComponent<Animator>();
	}

	public void Grow() {
		animator.SetTrigger("Grow");
	}

}
