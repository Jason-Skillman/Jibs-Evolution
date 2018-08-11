using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterDialogueManager {

	public class DialogueManager : MonoBehaviour {

		//Singleton
		public static DialogueManager main;

		//The list that holds all of the DialogueBlocks
		private List<DialogueBlock> dialogueBlocks = new List<DialogueBlock>();

		//The indexes to remember which sentence were are currently on
		public int indexBlock = 0;
		public int indexSentence = 0;

		//Is the dialogue currently playing
		private bool isDialoguePlaying = false;
		public bool IsDialoguePlaying {
			get { return isDialoguePlaying; }
		}

		//Is the sentence still being typed out
		private bool isDialogueTyping = false;
		public bool IsDialogueTyping {
			get { return isDialogueTyping; }
		}

		//Is the dialogue box closed
		private bool isDialogueBoxOpen = false;
		public bool IsDialogueBoxOpen {
			get { return isDialogueBoxOpen; }
		}

		//Type speed variables and enum
		public enum TypeSpeed {
			Fast,
			Medium,
			Slow
		}
		public TypeSpeed typeSpeed = TypeSpeed.Fast;
		public float typeSpeedFast = 0.01f;
		public float typeSpeedMedium = 0.05f;
		public float typeSpeedSlow = 0.1f;

		//Components
		public Text textName;
		public Text textSentence;
		public AudioSource audioTyping;
		public AudioSource audioEndOfTyping;
		public Button clickAreaField;

		private Animator animator;

		//Buttons
		public List<ChoiceButton> choiceButtons = new List<ChoiceButton>(5);    //Note: Make sure their are only 5 buttons

		//Delegates
		public delegate void OnXboxButton(ButtonType type);
		public OnXboxButton OnAnyXboxButton;

		public delegate void XboxButtonEvent();
		public event XboxButtonEvent OnXboxB, OnXboxX, OnXboxY;

		//Coroutines
		private Coroutine coroutineTypeSentence;
		private Coroutine coroutineFadeOutAudio;


		private void Awake() {
			DontDestroyOnLoad(gameObject);

			//Singleton
			if(main == null) {
				main = this;
			} else {
				Destroy(gameObject);
			}

			//Error check
			foreach(ChoiceButton choice in choiceButtons) {
				if(choice == null) Debug.LogError("No buttons are assigned to the Dialogue Manager!");
			}

			clickAreaField.onClick.AddListener(OnClickAreaField);

			animator = GetComponent<Animator>();
		}

		private void Update() {
			//Event
			/*
			if(OnAnyXboxButton != null) {
				if(Input.GetButtonDown("XboxB")) {
					OnAnyXboxButton.Invoke(ButtonType.XboxB);
				} else if(Input.GetButtonDown("XboxX")) {
					OnAnyXboxButton.Invoke(ButtonType.XboxX);
				} else if(Input.GetButtonDown("XboxY")) {
					OnAnyXboxButton.Invoke(ButtonType.XboxY);
				}
			}
			*/
			if(IsDialoguePlaying) {
				if(Input.GetMouseButtonDown(0)) {
					OnClickAreaField();
				}
				/*
				if(Input.GetButtonDown("XboxB")) {
					if(OnXboxB != null) OnXboxB.Invoke();
					else OnClickAreaField();
				} else if(Input.GetButtonDown("XboxX")) {
					if(OnXboxB != null) OnXboxX.Invoke();
				} else if(Input.GetButtonDown("XboxY")) {
					if(OnXboxB != null) OnXboxY.Invoke();
				}
				*/
			}
		}

		///<summary>Main method to call when adding a new Dialogue Block to the Dialogue Manager. Also starts up the dialogue box.</summary>
		///<param name="dialogueListScript">The Dialogue Block to add the the Dialogue Manager's list.</param>
		public void AddDialogueBlock(DialogueBlock dialogueListScript) {
			//Add the incoming data to the master list
			dialogueBlocks.Add(dialogueListScript);

			//Startup for the first time
			if(isDialoguePlaying == false) {
				isDialoguePlaying = true;
				animator.SetBool("IsOpen", true);

				UpdateDialogue();
			}
		}

		///<summary>Displays the next sentecne if their is one.</summary>
		public void DisplayNextSentence() {
			if(isDialoguePlaying) {
				//Go to next sentence
				indexSentence++;

				//Checks if all of the sentences have finished in the current block
				if(indexSentence >= dialogueBlocks[indexBlock].dialogueList.Count) {
					indexSentence = 0;      //Reset the sentence index
					indexBlock++;           //Move to the next block

					OnEndBlockDialogue();   //Call the OnEndBlockDialogue() method

					//Checks if all of the blocks have finished
					if(indexBlock >= dialogueBlocks.Count) {
						indexSentence = 0;  //Reset the sentence index
						indexBlock = 0;     //Reset the block index

						OnEndAllDialogue(); //Call the OnEndAllDialogue() method
						return;
					}
				}

				UpdateDialogue();
			}
		}

		///<summary>Updates the dialogue box.</summary>
		private void UpdateDialogue() {
			DialogueBlock currentBlock = dialogueBlocks[indexBlock];                //The current Dialogue Block we are on
			Dialogue currentDialogue = currentBlock.dialogueList[indexSentence];    //The current Dialogue Sentence we are on

			//Reset all button components
			SetAllButtons_Active(false);
			SetAllButtons_TextIconsActive(false);
			SetAllButtons_ButtonIconsActive(false);
			SetAllButtons_SelectArrowActive(false);
			SetAllButtons_ButtonLinkType(ButtonType.None);

			//Set the areaField as the default selected button
			clickAreaField.gameObject.SetActive(true);
			clickAreaField.Select();

			#region BUTTON_CHECK
			//Check if we are at the end of the current block
			if(indexSentence >= currentBlock.dialogueList.Count - 1) {
				//Check if the current block has any buttons
				if(currentBlock.hasButtons) {

					//Turn off the areaField
					clickAreaField.gameObject.SetActive(false);
					//Select the last button as the default selected button
					choiceButtons[currentBlock.buttonCount - 1].Select();
					choiceButtons[currentBlock.buttonCount - 1].OnSelect(null);

					//Loop through and update each individual button
					for(int i = 0; i < currentBlock.buttonCount; i++) {
						choiceButtons[i].SetActive(true);                                                                                           //Turn the button on
						choiceButtons[i].SetText(currentBlock.choiceBtnDataList[i].text);                                                           //Set the Text
						if(currentBlock.choiceBtnDataList[i].hasTextIcon) choiceButtons[i].SetTextIcon(currentBlock.choiceBtnDataList[i].textIcon); //Set the TextIcon
						choiceButtons[i].SetButtonType(currentBlock.choiceBtnDataList[i].buttonType);                                               //Set the ButtonIcon

						int delegateIndex = i;  //Note: Dont use i in a delegate, use delegateIndex instead
						choiceButtons[i].onSubmit.RemoveAllListeners();
						choiceButtons[i].onSubmit.AddListener(delegate {
							if(isDialoguePlaying) {
								OnButtonSubmit(delegateIndex);

								DialogueBlock nextDialogueBlock = currentBlock.choiceBtnDataList[delegateIndex].DialogueBlock;
								//Checks if their is a dialogue block to trigger
								if(nextDialogueBlock != null) {
									nextDialogueBlock.TriggerDialogue();
								}


								DisplayNextSentence();
							}
						});
					}
				}
			}
			#endregion

			//Play the typing audio
			audioTyping.Stop();
			audioTyping.Play();

			//Set the name and sentance
			textName.text = currentDialogue.name;
			string sentence = currentDialogue.sentence;
			//Start typing out sentance
			if(coroutineTypeSentence != null) StopCoroutine(coroutineTypeSentence);
			coroutineTypeSentence = StartCoroutine(TypeSentence(sentence));
		}

		#region BUTTON_ACTIVE_CONTROL
		///<summary>Sets all of the main buttons to be active.</summary>
		private void SetAllButtons_Active(bool value) {
			foreach(ChoiceButton choice in choiceButtons) {
				choice.SetActive(value);
			}
		}
		///<summary>Sets all of the button's text icons to be active.</summary>
		private void SetAllButtons_TextIconsActive(bool value) {
			foreach(ChoiceButton choice in choiceButtons) {
				choice.SetTextIconActive(value);
			}
		}
		///<summary>Sets all of the button's button icons to be active.</summary>
		private void SetAllButtons_ButtonIconsActive(bool value) {
			foreach(ChoiceButton choice in choiceButtons) {
				choice.SetButtonIconActive(value);
			}
		}
		///<summary>Sets all of the button's select arrow to be active.</summary>
		private void SetAllButtons_SelectArrowActive(bool value) {
			foreach(ChoiceButton choice in choiceButtons) {
				choice.SetSelectArrowActive(value);
			}
		}
		///<summary>Sets all of the button's button linked type.</summary>
		private void SetAllButtons_ButtonLinkType(ButtonType type) {
			foreach(ChoiceButton choice in choiceButtons) {
				choice.SetButtonType(type);
			}
		}


		#endregion

		#region METHOD_MESSAGES
		///<summary>Method message called when a dialogue block has finished.</summary>
		private void OnEndBlockDialogue() {
			//Debug.Log("OnEndBlockDialogue");
			dialogueBlocks[indexBlock - 1].OnDialogueBlockFinish(indexBlock - 1);
		}
		///<summary>Method message called when all of the dialogue blocks have finished.</summary>
		private void OnEndAllDialogue() {
			//Debug.Log("OnEndAllDialogue");
			animator.SetBool("IsOpen", false);
			isDialoguePlaying = false;
			dialogueBlocks.Clear();
		}

		///<summary>Method message called when the dialogue manager has finished typing out the sentence.</summary>
		private void OnFinishedTyping() {
			if(!IsDialoguePlaying) return;  //Fixed a bug

			if(coroutineTypeSentence != null) StopCoroutine(coroutineTypeSentence);
			isDialogueTyping = false;

			textSentence.text = dialogueBlocks[indexBlock].dialogueList[indexSentence].sentence;

			//Fades out the typing audio
			if(coroutineFadeOutAudio != null) StopCoroutine(coroutineFadeOutAudio);
			coroutineFadeOutAudio = StartCoroutine(FadeOutAudio(audioTyping, 0.5f));

			//Start the EndOfTyping audio
			audioEndOfTyping.Stop();
			audioEndOfTyping.Play();
		}
		///<summary>Method message called when the area field has been clicked.</summary>
		private void OnClickAreaField() {
			//If the dialogue is still being typed out then stop it else display the next sentence
			if(IsDialogueTyping) {
				OnFinishedTyping();
			} else {
				DisplayNextSentence();
			}
		}
		///<summary>Method message called when a button is clicked on.</summary>
		///<param name="index">The index of the button submited.</param>
		private void OnButtonSubmit(int index) {
			//Debug.Log("OnButtonSubmit: " + index);

			//Clear all of the linked buttons
			OnXboxB = null;
			OnXboxX = null;
			OnXboxY = null;
		}
		///<summary>Method message called externally by the animator when the dialogue box has been completly opened</summary>
		public void OnDialogueBoxOpen() {
			isDialogueBoxOpen = true;
		}
		///<summary>Method message called externally by the animator when the dialogue box has been completly closed</summary>
		public void OnDialogueBoxClosed() {
			isDialogueBoxOpen = false;
		}
		#endregion

		#region COROUTINES
		///<summary>Types out the sentence provided into the dialogue text field.</summary>
		///<param name="sentence">The sentence to type out.</param>
		private IEnumerator TypeSentence(string sentence) {
			isDialogueTyping = true;
			textSentence.text = "";

			float typeSpeedDelay = typeSpeedFast;
			switch(typeSpeed) {
				case TypeSpeed.Fast:
					typeSpeedDelay = typeSpeedFast;
					break;
				case TypeSpeed.Medium:
					typeSpeedDelay = typeSpeedMedium;
					break;
				case TypeSpeed.Slow:
					typeSpeedDelay = typeSpeedSlow;
					break;
			}

			foreach(char letter in sentence.ToCharArray()) {
				textSentence.text += letter;
				yield return new WaitForSeconds(typeSpeedDelay);
			}

			OnFinishedTyping();
		}

		///<summary>Fades the audio out by the fade time.</summary>
		///<param name="audioSource">The audio source to fade out.</param>
		///<param name="fadeTime">Time it takes until the sound will be completly faded out.</param>
		private IEnumerator FadeOutAudio(AudioSource audioSource, float fadeTime) {
			audioSource.volume = 1; //1 is Max volume
			while(audioSource.volume > 0) {
				audioSource.volume -= 1 * Time.deltaTime / fadeTime;

				yield return null;
			}
			audioSource.Stop();
			audioSource.volume = 1;
		}
		#endregion

	}

}
