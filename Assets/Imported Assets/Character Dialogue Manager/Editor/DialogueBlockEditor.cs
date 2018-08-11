using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace CharacterDialogueManager {

	[CanEditMultipleObjects]
	[CustomEditor(typeof(DialogueBlock))]
	public class DialogueBlockEditor : Editor {

		public DialogueBlock main;
		private ReorderableList reorderableList;        //The reorderable list

		//Constants
		const float k_SingleTextLineHeight = 16f;       //Height of a single text field
		const float k_MarginBetweenCards = 20f;         //Margin between each card


		private void Awake() {
			main = (DialogueBlock)target;

			reorderableList = new ReorderableList(main.dialogueList, typeof(Dialogue), true, true, true, true);
			reorderableList.drawHeaderCallback += OnDrawHeader;
			reorderableList.drawElementCallback += OnDrawElement;
			reorderableList.elementHeightCallback += GetElementHeight;
			reorderableList.onReorderCallbackWithDetails += ReorderCallbackDelegateWithDetails;
		}

		public override void OnInspectorGUI() {
			//base.OnInspectorGUI();
			EditorGUILayout.Space();

			//Print the Reorderable List
			if(reorderableList != null && main.dialogueList != null)
				reorderableList.DoLayoutList(); //Note: Must be called in OnInspectorGUI() method

			//Print the button info
			#region BUTTON_INFO
			main.hasButtons = EditorGUILayout.BeginToggleGroup("Has Buttons", main.hasButtons);
			main.buttonCount = EditorGUILayout.IntSlider("Button Count", main.buttonCount, 1, 5);
			EditorGUILayout.Space();

			//Print each individual button info
			for(int i = (main.buttonCount - 1); i >= 0; i--) {
				EditorGUILayout.LabelField("Button " + (i + 1), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;

				main.choiceBtnDataList[i].text = EditorGUILayout.TextField("Text", main.choiceBtnDataList[i].text);
				main.choiceBtnDataList[i].DialogueBlock = (DialogueBlock)EditorGUILayout.ObjectField("Dialogue Block", main.choiceBtnDataList[i].DialogueBlock, typeof(DialogueBlock), allowSceneObjects: true);
				main.choiceBtnDataList[i].buttonType = (ButtonType)EditorGUILayout.EnumPopup("Designated Button", main.choiceBtnDataList[i].buttonType);

				main.choiceBtnDataList[i].hasTextIcon = EditorGUILayout.Toggle("Has Text Icon", main.choiceBtnDataList[i].hasTextIcon);
				if(main.choiceBtnDataList[i].hasTextIcon) main.choiceBtnDataList[i].textIcon = (Sprite)EditorGUILayout.ObjectField("Sprite", main.choiceBtnDataList[i].textIcon, typeof(Sprite), allowSceneObjects: true);

				EditorGUI.indentLevel--;
				EditorGUILayout.Space();
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndToggleGroup();
			#endregion

			EditorUtility.SetDirty(target);
		}

		private void OnDrawHeader(Rect rect) {
			GUI.Label(rect, "Reorderable Dialogue List");
		}
		private void OnDrawElement(Rect rect, int index, bool isactive, bool isfocused) {
			Dialogue dialogue = main.dialogueList[index];   //Current dialogue

			float y = rect.yMin + 2;
			float width = rect.width;
			float height = rect.height;

			EditorGUI.BeginChangeCheck();   //Note: Keep this

			//Element 1: Name field
			Rect newRect;
			newRect = new Rect(rect.xMin, y, width, k_SingleTextLineHeight);
			dialogue.name = EditorGUI.TextField(newRect, "Name", dialogue.name);
			y += k_MarginBetweenCards;

			//Element 2: Sentence label
			newRect = new Rect(rect.xMin, y, width, k_SingleTextLineHeight);
			EditorGUI.LabelField(newRect, "Sentence");

			//Element 3: Sentence field
			int offset = 120;
			newRect = new Rect(rect.xMin + offset, y, width - offset, k_SingleTextLineHeight * 3);
			dialogue.sentence = EditorGUI.TextArea(newRect, dialogue.sentence);
			y += k_MarginBetweenCards;

			if(EditorGUI.EndChangeCheck()) SaveTile();
		}
		private float GetElementHeight(int index) {
			return 90;
		}
		private void ReorderCallbackDelegateWithDetails(ReorderableList list, int oldIndex, int newIndex) {
			SaveTile();
		}

		private void SaveTile() {
			EditorUtility.SetDirty(target);
			SceneView.RepaintAll();
		}

	}

}
