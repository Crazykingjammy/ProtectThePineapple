using UnityEngine;
using System.Collections;

public class ControlsLoader : MonoBehaviour {
	
	public enum DEVICE { 
		iphone, iPad, ANDROID_S, ANDROID_M, ANDROID_L, PC, CONSOLE, NULL	};
	
	public DEVICE DeviceControlsStartIndex = 0;	// the controls to start with
	DEVICE currentDeviceControlsIndex = 0;  // this is for internal use.  ability to switch in game.
	
	/// <summary>
	/// The controls layout prefabs.
	/// Different layouts for different strokes.  Assign prefabs in the Unity Editor
	/// The loader will handle switching
	/// </summary>
	public ControlsLayoutObject [] controlsLayoutPrefabs;
	public ControlsLayoutObject defaultControlsLayout = null;
	
	public bool loadAtStart = true;
	
	ControlsLayoutObject loadedObject = null;
	
	
	
	public ControlsLayoutObject LoadedControls
	{
		get
		{
			return loadedObject;
		}
	}
	
	
	// Use this for initialization
	void Start () {
		
		//This be the default?
		if(GameObjectTracker.instance == null)
			Debug.LogError("NO GOT!!");
		
		
		//DeviceControlsStartIndex = DEVICE.iphone;
		//GameObjectTracker.instance._PlayerData.currentDevice = DEVICE.iphone;
		
	
		
#if UNITY_IPHONE
		
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			//Check the device and assign the index.
			if( (iPhone.generation == iPhoneGeneration.iPad2Gen) || 
				(iPhone.generation == iPhoneGeneration.iPad3Gen) ||
				(iPhone.generation == iPhoneGeneration.iPad4Gen)  )
			{
				DeviceControlsStartIndex = DEVICE.iPad;
				
				//let the player data know waht device we have
				GameObjectTracker.instance._PlayerData.currentDevice = DEVICE.iPad;
				
			}
			else
			{
				DeviceControlsStartIndex = DEVICE.iphone;
				
				if(iPhone.generation == iPhoneGeneration.iPhone4S || 
					iPhone.generation == iPhoneGeneration.iPodTouch5Gen)
				{
					QualitySettings.SetQualityLevel(0,true);
				}
				
				//let the player data know waht device we have
				GameObjectTracker.instance._PlayerData.currentDevice = DEVICE.iphone;
				
			}		
			
		}
		
		
#endif
		
		
		
		// start the current Device controls index to the starting index.
		currentDeviceControlsIndex = DeviceControlsStartIndex;
		
		//Then based on the current device index we load the prefab for the device. 
		
		// we going to instantiate with the ngui tools because it needs to be correctly
		// inserted below us.
		if (loadAtStart){
			if (controlsLayoutPrefabs.Length > (int)currentDeviceControlsIndex){
				ControlsLayoutObject loadPrefab = controlsLayoutPrefabs[(int)currentDeviceControlsIndex];
				if (loadPrefab){ // we have a prefab to load
					
					loadedObject = NGUITools.AddChild(this.gameObject, loadPrefab.gameObject).GetComponent<ControlsLayoutObject>();
					if (loadedObject){
//						Debug.Log("We have a controls Layout Object " + loadedObject.name);
					}					
				}				
			}
			else{ // we're out of bounds, load the default or nothing
				if (defaultControlsLayout != null){
					loadedObject = NGUITools.AddChild(this.gameObject, defaultControlsLayout.gameObject).GetComponent<ControlsLayoutObject>();
					if (loadedObject){
						Debug.LogWarning("Loaded Default Controls Layout Object " + loadedObject.name);
					}
				}
			}
		}		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
