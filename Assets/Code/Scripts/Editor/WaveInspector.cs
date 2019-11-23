using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[CustomEditor(typeof(StandardWave))]
public class WaveInspector : Editor {
	
	
	//The data itself.
	private SerializedObject wave, tArray;
	private SerializedProperty spawnersArray;
	
	
	//Data access
	private StandardWave myInstance, sceneInstance;
	private TargetSpawner[] spawners;
	
	int selectedSpawner = -1;
	int spawnerIterator = 0;
	Texture icon;
	bool inScene = false;
	
	void OnEnable()
	{
		wave = new SerializedObject(target);
		//spawnersArray = wave.FindProperty("EnemySpawnPoints");
		
		
		//Tools.current = Tool.None;
		
		UpdateSpawnerList();
		
		
		myInstance.transform.position = Vector3.zero;
		
		
		

		
	}
	
	void OnDisable()
	{
		if(sceneInstance)
			DestroyImmediate(sceneInstance.gameObject);
	}
	
	
	//[DrawGizmo(GizmoType.Active)]
	void OnSceneGUI()
	{

		Event e = Event.current;
		int id = GUIUtility.GetControlID( FocusType.Passive);
		//EventType type = e.GetTypeForControl(id);
		
		
		
		Vector3 position = Vector3.zero;
		if(selectedSpawner != -1)
		{
			if(spawners == null)
				return;
			
			//Grab the position
			position = spawners[selectedSpawner].transform.position;
			
			//Render the sphere cap and render the handle for selected spawner if any.
			Handles.SphereCap(id,position,Quaternion.identity,3.0f);
			position = Handles.PositionHandle(position,Quaternion.identity);
			
			//position = Handles.DoPositionHandle(position,Quaternion.identity);
			
			//Reassign the position.
			spawners[selectedSpawner].transform.position = position;
			

			//Hide parent gizmo?
			Tools.current = Tool.None;
			
			
			//Undo.ClearSnapshotTarget();
			
		}
		
		
		
		
	}
	

	
	public override void OnInspectorGUI()
	{
		wave.Update();
		
		
		//Draw info for the wave.
		DrawHeader(wave.targetObject.name);
		
		myInstance.name = NameBox(myInstance.name);
		
		
		//Draw the list.
		DrawWaveSpawnersList();
		
		//Draw the spawner info
		DrawSpawnerInfo();
		
		
		wave.ApplyModifiedProperties();
		
		
	}
	
	void DrawHeader(string title)
	{
		
		GUILayout.Space(3f);
		GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
		
		GUILayout.BeginHorizontal();
		GUILayout.Space(3f);

		GUI.changed = false;

		GUILayout.Toggle(true, "<b><size=11>" + title + "</size></b>", "dragtab");
		

		GUILayout.Space(2f);
		GUILayout.EndHorizontal();
	
		
		GUI.backgroundColor = Color.white;
	}
	
	
	void DrawSpawnerInfo()
	{
		DrawHeader("Spawner Info");
	
		
		
		GUI.backgroundColor = Color.cyan;
		
		EditorGUILayout.BeginVertical();
	
		spawnerIterator = 0;
		foreach(Object o in tArray.targetObjects)
		{
			//Lets grab the spawner object..
			SerializedObject sp = new SerializedObject(o);
			
			if(spawnerIterator == selectedSpawner)
			{
				//Allow us to edit the name shall we.
				sp.targetObject.name =  NameBox(sp.targetObject.name);
			
				//Draw the rest of the properties.
				DrawPropertiesExcluding(sp);
			}
			
			//Apply the properties.
			sp.ApplyModifiedProperties();
				
			//Iterate the iterator.
			spawnerIterator++;
		}
			
		GUILayout.Space(3f);
		
		EditorGUILayout.EndVertical();
		
		GUI.backgroundColor = Color.white;
	}
	
	void DrawWaveSpawnersList()
	{
		EditorGUILayout.BeginVertical();
		
		spawnerIterator = 0;
		foreach(Object o in tArray.targetObjects)
		{
			//Grab the object
			SerializedObject sp = new SerializedObject(o);
			
			EditorGUILayout.BeginHorizontal();
			
			//See if our iterator is the selected index
			if(selectedSpawner == spawnerIterator)
			{
				//Set to highlight color if we are selected.
				GUI.backgroundColor = Color.cyan;
					
			}
			
			//Draw a button for each target object with its name.
			//If the button is pushed we set the selected index.
			if(GUILayout.Button(sp.targetObject.name))
			{
			
				//If the selected index is already this one then we unselected by setting to -1
				if(selectedSpawner == spawnerIterator)
				{
					selectedSpawner = -1;
					
					//Hide parent gizmo?
					//Tools.current = Tool.None;
				}
				else
				{
					//And the setting of the index by default.
					selectedSpawner = spawnerIterator;
					//Tools.current = Tool.Move;
				}
				
			}
			
			
			//See if our iterator is the selected index
			if(selectedSpawner == spawnerIterator && inScene)
			{
				//Set to highlight color if we are selected.
				GUI.backgroundColor = Color.red;
				
				if(GUILayout.Button("x",GUILayout.MaxWidth(30f)))
				{
					//Grab a refereence of the object to destroy, reset values and sdestroy.
					GameObject g = spawners[selectedSpawner].gameObject;
					selectedSpawner = -1;
					spawners = null;
					
					//Destruction
					DestroyImmediate( g ,true);
					
					//Rebuild list.
					UpdateSpawnerList();
				}
			}
			
			//Reset the background color as we set it for highlighting.
			GUI.backgroundColor = Color.white;
			
			
			EditorGUILayout.EndHorizontal();
			
			//Ierate the iterator.
			spawnerIterator++;
		}
		
	
		AddSpawnerButton();
		
		GUILayout.Space(10f);
		
		EditorGUILayout.EndVertical();
		
	}
	
	
	void UpdateSpawnerList()
	{
		
		myInstance = (StandardWave)target;
		
		
		if(sceneInstance)
			DestroyImmediate(sceneInstance);
		
		//instantuate instance to scene
		GameObject o =  null;
		o = GameObject.Find(myInstance.name);
		inScene = true;
		
		//If we dont exist then we are in editor mode
		if(!o)
		{
			inScene = false;
			sceneInstance = Instantiate(myInstance) as StandardWave;
			
		}
		
		
		spawners = myInstance.GetComponentsInChildren<TargetSpawner>(true);
		tArray = new SerializedObject(spawners);
		
		
		
		
	}
	
	
	void AddSpawnerButton()
	{
		
		if(!inScene)
			return;
		
		GUI.backgroundColor = Color.green;
		
		//Draw a button for each target object with its name.
			//If the button is pushed we set the selected index.
		if(GUILayout.Button("Create Spwawner"))
		{
		
			int i = selectedSpawner;
			
			if(i == -1)
				i = 0;
			
			//Add a spawner to wave.
			TargetSpawner o = Instantiate(tArray.targetObjects[i]) as TargetSpawner;
			o.transform.parent = myInstance.transform;
		
			
		
			//Set some default values.
			o.name = "new";
		
			selectedSpawner = -1;
		
			UpdateSpawnerList();
		}
	
		
		
		GUI.backgroundColor = Color.white;
	}
	
	string NameBox(string _name)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name: ", GUILayout.MaxWidth(50.0f));
		string _nameout = GUILayout.TextField(_name);
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		
		return _nameout;
	}
	
}
