using System.Collections.Generic;
using UnityEngine;

namespace CharacterDialogueManager {

	public class DialogueBlock : MonoBehaviour {

		public List<Dialogue> dialogueList = new List<Dialogue>();              //The list of dialogue to display
		public bool hasButtons;                                                 //Does this DialogueBlock have buttons
		public int buttonCount;                                                 //Amount of buttons it has
		public ChoiceButtonData[] choiceBtnDataList = new ChoiceButtonData[5];  //All of the button data to use


		///<summary>Main method to call when you whant to trigger this dialouge block.</summary>
		public void TriggerDialogue() {
			DialogueManager.main.AddDialogueBlock(this);
		}

		///<summary>Called externaly by the Dialogue Manager when this dialogue block has finished.</summary>
		///<param name="index">The index of this Dialogue Block in the manager.</param>
		public void OnDialogueBlockFinish(int index) {
			//Debug.Log("OnDialogueBlockFinish: " + index);
		}

	}

}
