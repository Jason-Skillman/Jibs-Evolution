using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterDialogueManager;

public class OnStartSceneDialouge : MonoBehaviour {

	DialogueBlock block;
	
	void Start () {
		block = GetComponent<DialogueBlock>();
		block.TriggerDialogue();
	}
	
}
