using UnityEngine;
using System.Collections;

/// <summary>
/// Controls manager.
/// This controls which ControlsLoader to load!  
/// 
/// This way the controlsLoader can be a prefab for easy tweaking
/// </summary>
public class ControlsManager : MonoBehaviour {
	
	public ControlsLoader controlsLoaderPrefab = null;
	
	ControlsLoader loader;
	
	// Use this for initialization
	void Start () {
		if (controlsLoaderPrefab != null){
			//ControlsLoader loadedLoader = 
			
			loader = NGUITools.AddChild(this.gameObject, controlsLoaderPrefab.gameObject).GetComponent<ControlsLoader>();
		

			//Set self to activity manager
			ActivityManager.Instance.HUDController = this;

			Debug.Log("Loaded the controls Loader");


		
		}
	}
	
	public ControlsLayoutObject LoadedControls
	{
		get{
			return loader.LoadedControls;
		}
	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
