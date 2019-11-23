using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(ObjectiveMission))]
public class GenericMissionInspector : Editor {
	
	public GUIStyle Style;
	
	//Styles and such
	private static GUIContent
		insertMissionContent = new GUIContent("Create Objective","Create a new objective to the mission."),
		deleteMissionContent = new GUIContent("X","Delete this Objective"),
		insertEntryGate = new GUIContent("+","Clone Stat"),
		deleteEntryGate = new GUIContent("x","Delete Stat"),
		insertEntryGateBlank = new GUIContent("Add a Stat Check","Add a Stat Check to have the objective test against."),
	missionContent = GUIContent.none;
	
	
	
	private static GUILayoutOption
		statWidth = GUILayout.MaxWidth(70f),
		typeWidth = GUILayout.MaxWidth(140f),
		closeButton = GUILayout.MaxWidth(50f),
		addButton = GUILayout.MaxHeight(50f),
		buttonWidth = GUILayout.MaxWidth(20f),
		boolWdith  = GUILayout.MaxWidth(10f);
	
	
	
	
	
	//The data itself.
	private SerializedObject mission;
	private SerializedProperty missionObjectives;

	
	
	void OnEnable()
	{
		mission = new SerializedObject(target);
		missionObjectives = mission.FindProperty("MissionObjectives");
		
		Style = new GUIStyle();
		Style.richText = true;
		Style.alignment = TextAnchor.MiddleCenter;
		
		
		
		
	}
	
	void SeperatorLabel(string label)
	{
		EditorGUILayout.BeginVertical();
		
		string decor = "<b> ------------- </b>";
		
		//Space between missions
		EditorGUILayout.Space();
		GUILayout.Label(decor + label + decor,Style);
		EditorGUILayout.Space();
		
		EditorGUILayout.EndVertical();
	}
	
	public override void OnInspectorGUI()
	{
		mission.Update();
		
	
		SeperatorLabel("Info");
		
		EditorGUILayout.BeginVertical();
		
		
		EditorGUILayout.PropertyField(mission.FindProperty("Label"));
		EditorGUILayout.PropertyField(mission.FindProperty("DisplayName"));
		EditorGUILayout.PropertyField(mission.FindProperty("IconName"));
		EditorGUILayout.PropertyField(mission.FindProperty("MissionWorth"));
		
		
		EditorGUILayout.EndVertical();
		
		SeperatorLabel("Info");
		
		//Space between missions
		EditorGUILayout.Space();
	
		
		

		GUILayout.Label("<b> - Objectives - </b>",Style);
		
		
		//Space between missions
		EditorGUILayout.Space();
	
		EditorGUILayout.Separator();
		
		
		for(int i = 0; i < missionObjectives.arraySize; i++)
		{
			
			
			SerializedProperty objective = missionObjectives.GetArrayElementAtIndex(i);
	
					
			//begin to draw the first row. Display add and delete buttons with the label.
			EditorGUILayout.BeginHorizontal();
		
			
			//Grab the label of the Objective.
			EditorGUILayout.PropertyField(objective.FindPropertyRelative("ObjectiveLabel"),missionContent);
			
		
			//Add the delete button next to the Label.
			if(GUILayout.Button(deleteMissionContent,EditorStyles.miniButtonRight,closeButton))
			{
				missionObjectives.DeleteArrayElementAtIndex(i);
				//return;
			}
				
			
			//End first layout.
			EditorGUILayout.EndHorizontal();
			
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(objective.FindPropertyRelative("IconName"));
			EditorGUILayout.EndHorizontal();
			
			
			
			//GUILayout.Label(" - Completion Test - ");
			
			//Grab the stat gate.
			SerializedProperty gate = objective.FindPropertyRelative("CompletionTest");
			
			EditorGUILayout.BeginVertical();
			
			//Begin the PPS line
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(gate.FindPropertyRelative("PPS"));
			EditorGUILayout.PropertyField(gate.FindPropertyRelative("bPPS"),missionContent,boolWdith);
			EditorGUILayout.EndHorizontal();
			
			//begin the time amount line.
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(gate.FindPropertyRelative("timeAmount"));	
			EditorGUILayout.PropertyField(gate.FindPropertyRelative("btimeAmount"),missionContent,boolWdith);
			EditorGUILayout.EndHorizontal();
			
			
			//Begin the Score line.
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(gate.FindPropertyRelative("Score"));
			EditorGUILayout.PropertyField(gate.FindPropertyRelative("bScore"),missionContent,boolWdith);
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();
			
			//here we grab the stats test and its array.
			SerializedProperty statsTest = gate.FindPropertyRelative("StatsTest");
			
						
			//Now we grab each EntryGate and begin to draw them.
			for(int j = 0; j < statsTest.arraySize; j++)
			{
				//Grab the entry gate and store it to access its data.
				SerializedProperty eg = statsTest.GetArrayElementAtIndex(j);
				
				//Start line to display the entry date information.
				EditorGUILayout.BeginHorizontal();
		
					
				//The add and delete buttons.
				if(GUILayout.Button(insertEntryGate,EditorStyles.miniButtonLeft,buttonWidth))
					statsTest.InsertArrayElementAtIndex(j);
							
			
				EditorGUILayout.PropertyField(eg.FindPropertyRelative("bGreater"),missionContent,boolWdith);
				EditorGUILayout.PropertyField(eg.FindPropertyRelative("ValueTest"),missionContent,statWidth);
				EditorGUILayout.PropertyField(eg.FindPropertyRelative("type"), missionContent, typeWidth);
			
				
				//delete button
				if(GUILayout.Button(deleteEntryGate,EditorStyles.miniButtonRight,buttonWidth))
					statsTest.DeleteArrayElementAtIndex(j);
				
				//End the entry gate draw.
				EditorGUILayout.EndHorizontal();
			
				
			}
			
			//Handle a special add button if we dont have any stats test.
			if(statsTest.arraySize == 0)
			{
				if(GUILayout.Button(insertEntryGateBlank,EditorStyles.toolbarButton,addButton))
				{
					//statsTest.InsertArrayElementAtIndex(0);
					statsTest.arraySize++;
				}
					
				
			}
			
			
			
			//add another line for the Add Mission button.
			EditorGUILayout.BeginHorizontal();
			
			//At the bottom we put our big add bar
			if(GUILayout.Button(insertMissionContent,EditorStyles.toolbarButton,addButton))
			missionObjectives.InsertArrayElementAtIndex(i);
		
			
			//End the last line of displaying the list of missions.
			EditorGUILayout.EndHorizontal();
			
			
			
			
			//Space between missions
			SeperatorLabel(" * ");		

			
		}

		
			
		mission.ApplyModifiedProperties();
		
	}
	
	

}
