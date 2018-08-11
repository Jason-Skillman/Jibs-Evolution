using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterDialogueManager {

	[CustomEditor(typeof(DialogueManager), true)]
	public class DialogueManagerEditor : Editor {

		bool foldoutTypeSpeed = false;
		bool foldoutButtons = false;

		public override void OnInspectorGUI() {
			//base.OnInspectorGUI();
			DialogueManager main = (DialogueManager)target;


			//Title
			GUILayout.Space(5);
			EditorGUILayout.LabelField("Dialogue Manager", EditorStyles.boldLabel);
			GUILayout.Space(5);


			//Typing speeds
			main.typeSpeed = (DialogueManager.TypeSpeed)EditorGUILayout.EnumPopup("Type Speed", main.typeSpeed);
			foldoutTypeSpeed = EditorGUILayout.Foldout(foldoutTypeSpeed, "Default Typing Speeds");
			if(foldoutTypeSpeed) {
				EditorGUI.indentLevel++;

				main.typeSpeedFast = EditorGUILayout.FloatField(new GUIContent("Fast", "Tooltip"), main.typeSpeedFast);
				main.typeSpeedMedium = EditorGUILayout.FloatField(new GUIContent("Medium", "Tooltip"), main.typeSpeedMedium);
				main.typeSpeedSlow = EditorGUILayout.FloatField(new GUIContent("Slow", "Tooltip"), main.typeSpeedSlow);

				EditorGUI.indentLevel--;
			}
			GUILayout.Space(10);


			//Componets
			main.textName = (Text)EditorGUILayout.ObjectField("Name", main.textName, typeof(Text), true);
			main.textSentence = (Text)EditorGUILayout.ObjectField("Sentence", main.textSentence, typeof(Text), true);
			main.audioTyping = (AudioSource)EditorGUILayout.ObjectField("Audio Typing", main.audioTyping, typeof(AudioSource), true);
			main.audioEndOfTyping = (AudioSource)EditorGUILayout.ObjectField("Audio End of Typing", main.audioEndOfTyping, typeof(AudioSource), true);
			main.clickAreaField = (Button)EditorGUILayout.ObjectField("Click Area Field", main.clickAreaField, typeof(Button), true);
			GUILayout.Space(10);


			//Option Buttons
			foldoutButtons = EditorGUILayout.Foldout(foldoutButtons, "Choice Buttons");
			if(foldoutButtons) {
				EditorGUI.indentLevel++;
				for(int i = 0; i < main.choiceButtons.Count; i++) {
					main.choiceButtons[i] = (ChoiceButton)EditorGUILayout.ObjectField("Choice Button " + (i + 1), main.choiceButtons[i], typeof(ChoiceButton), true);
				}
				EditorGUI.indentLevel--;
			}
			GUILayout.Space(10);


			//Readonly variables
			EditorGUILayout.LabelField("Block Index: " + main.indexBlock + ", Sentence Index: " + main.indexSentence);
			EditorGUILayout.LabelField("Is Dialogue Playing: " + main.IsDialoguePlaying);
			EditorGUILayout.LabelField("Is Dialogue Typing: " + main.IsDialogueTyping);
			EditorGUILayout.LabelField("Is Dialogue Box Open: " + main.IsDialogueBoxOpen);
			GUILayout.Space(10);


			EditorUtility.SetDirty(main);
		}
	}

}
