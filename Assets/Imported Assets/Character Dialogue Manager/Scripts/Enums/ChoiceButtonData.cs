using UnityEngine;

namespace CharacterDialogueManager {

	[System.Serializable]
	public struct ChoiceButtonData {
		public string text;
		public DialogueBlock DialogueBlock;
		public ButtonType buttonType;
		public bool hasTextIcon;
		public Sprite textIcon;
	}

}
