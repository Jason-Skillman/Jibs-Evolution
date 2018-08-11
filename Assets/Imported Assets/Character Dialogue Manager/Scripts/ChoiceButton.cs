using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CharacterDialogueManager {

	public class ChoiceButton : Selectable, ISubmitHandler {

		public bool isSelected = false;

		//Componets
		public Text text;               //Text
		public GameObject textIcon;     //Text Icon
		public GameObject buttonIcon;   //Button Icon
		public GameObject selectArrow;   //Select Arrow

		//Sprites
		public Sprite spriteXboxB;
		public Sprite spriteXboxX;
		public Sprite spriteXboxY;

		//Events
		[HideInInspector]
		public UnityEvent onSubmit;


		///<summary>Called when this button has been selected.</summary>
		public override void OnSelect(BaseEventData eventData) {
			isSelected = true;
			SetSelectArrowActive(true);
		}
		///<summary>Called when this button has been deselected.</summary>
		public override void OnDeselect(BaseEventData eventData) {
			isSelected = false;
			SetSelectArrowActive(false);
		}
		///<summary>Called when this button has been clicked on.</summary>
		public void OnSubmit(BaseEventData eventData) {
			onSubmit.Invoke();
		}
		///<summary>Called when this button has been clicked on by the linked button.</summary>
		public void OnLinkedSubmit() {
			//Debug.Log("OnLinkedSubmit: " + linkedButtonType.ToString());
			OnSubmit(null);
		}

		#region SETTERS
		///<summary>Setter for changing this button's active state.</summary>
		public void SetActive(bool value) {
			gameObject.SetActive(value);
		}
		///<summary>Setter for changing this button's Text Icon active state.</summary>
		public void SetTextIconActive(bool value) {
			textIcon.gameObject.SetActive(value);
		}
		///<summary>Setter for changing this button's Button Icon active state.</summary>
		public void SetButtonIconActive(bool value) {
			buttonIcon.gameObject.SetActive(value);
		}
		///<summary>Setter for changing this button's Select Arrow active state.</summary>
		public void SetSelectArrowActive(bool value) {
			selectArrow.SetActive(value);
		}

		///<summary>Setter for changing this button's text field.</summary>
		public void SetText(string text) {
			this.text.text = text;
		}
		///<summary>Setter for changing this button's Text Icon image.</summary>
		public void SetTextIcon(Sprite sprite) {
			textIcon.SetActive(true);
			textIcon.GetComponent<Image>().sprite = sprite;
		}
		///<summary>Setter for changing this button's linked button type.</summary>
		public void SetButtonType(ButtonType type) {
			//Update the icon
			switch(type) {
				case ButtonType.XboxB:
					buttonIcon.SetActive(true);
					buttonIcon.GetComponent<Image>().sprite = spriteXboxB;
					DialogueManager.main.OnXboxB += OnLinkedSubmit;
					break;
				case ButtonType.XboxX:
					buttonIcon.SetActive(true);
					buttonIcon.GetComponent<Image>().sprite = spriteXboxX;
					DialogueManager.main.OnXboxX += OnLinkedSubmit;
					break;
				case ButtonType.XboxY:
					buttonIcon.SetActive(true);
					buttonIcon.GetComponent<Image>().sprite = spriteXboxY;
					DialogueManager.main.OnXboxY += OnLinkedSubmit;
					break;
			}
		}
		#endregion

	}

}
