using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// FU i2d location manager.
/// 
/// manages getting the requests to draw items on screen
/// from a 3D transform off the main games camera
/// to the 2d Camera Canvas
/// 
/// </summary>
public class FUI2dLocManager : MonoBehaviour {
	
	/// <summary>
	/// All the Types of Icon Loc Objects
	/// The index of this type is equivalent to the index of the ListOfPrefabs.
	/// Except the last one, the prefab is given from the spawner, if no prefab then
	/// a default will be used
	/// 
	/// </summary>
	public enum IconLocObjectTypes{
		// if of Prefab type Create object based on assigned prefab of the spawner
		PrefabIconLoc,
		// These are default prefabs.  the loc manager in the scene has the prefabs assign at build time.  
		// List of prefabs.  
		CannonIconLoc, 
		EnemyIconLoc,
		PowerUpIconLoc,
		PhaseNotifIconLoc, 
		CommandCenterLoc
	}
	public IconLocObjectTypes TypesTemplate;
	/// <summary>
	/// A list of IconLocPrefabs
	/// 
	/// This makes it easier to just add the script to a type of object
	/// with out requiring to assign a prefab
	/// </summary>
	public List<IconLocObject> ListOfPrefabs = null;
	
	// a pool of icon objects
	public int startPoolCannons = 0;
	public int startPoolAlerts = 0;
	public int startPoolPowerups = 0;
	public int startPoolPhaseNotif = 0;
	public int startPoolCCHealth = 0;
	
	
	List<IconLocObject> poolIconLocCannons = null;
	List<IconLocObject> poolIconLocAlerts  = null;
	List<IconLocObject> poolIconLocPowerUps = null;
	List<IconLocObject> poolIconLocPhaseNotif = null;
	List<IconLocObject> poolIconLocCC = null;
	
	///I'm a singleton
	private static FUI2dLocManager _manager = null;
	
	public static FUI2dLocManager GetLM{
		get { return _manager; } 
	}
	
	/// <summary>
	/// The Camera to clamp the icons to
	/// </summary>
	Camera _worldCamera = null;
	public Camera WorldCamera {
		get{ return _worldCamera; }
		set{ _worldCamera = value; } 
	}
	Camera _2dCamera = null;
	public Camera Canvas {
		get{ return _2dCamera; }
		set{ _2dCamera = value; } 
	}
	
	// make sure our core objects are initialized correctly
	bool isFullyInit = false;
	
	// when off, all renders managed here will be off
	public bool isIconsVisible = true;
	
	// Keep track of my panel
	UIPanel _myPanel = null;
	
	static public FUI2dLocManager GetManager(){
		if (_manager){
			return _manager;}
		else{
			return null;}
	}
	
	// Use this for initialization
	void Awake () {
		// set the instance of me
		_manager = this;

		
		Init();

		
	}
	
	bool Init(){
		
		Camera twoDCam = this.GetComponentInChildren<Camera>();
		if (twoDCam == null){
			Debug.LogError("No Camera FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;						
		}
		_2dCamera = twoDCam;
		
		ToyBox tb = ToyBox.GetPandora();
		if (tb == null){
//			Debug.LogError("No tb FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;						
		}
		
		// now check for a world camera.  all targets are the same layer.
		if(ToyBox.GetPandora())
			if(ToyBox.GetPandora().Camera_01)
			_worldCamera = ToyBox.GetPandora().Camera_01.GetComponent<Camera>();
			
		
		if (_worldCamera == null){
			Debug.LogWarning("No World Camera FUI2dLocManager, try again...");
			isFullyInit = false;
			return isFullyInit;						
		}
		
		// we should have a panel to manage
		_myPanel = this.GetComponentInChildren<UIPanel>();
		if (_myPanel == null){
			Debug.LogWarning("No Panel in FUI2dLocManager, try again...");
			isFullyInit = false;
			return isFullyInit;					
		}			
		
		poolIconLocCannons = new List<IconLocObject>();
		if (poolIconLocCannons == null){
			Debug.LogError("failed to create poolIconLocCannons int FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;			
		}		
		poolIconLocAlerts = new List<IconLocObject>();
		if (poolIconLocAlerts == null){
			Debug.LogError("failed to create poolIconLocAlerts int FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;			
		}		
		poolIconLocPowerUps = new List<IconLocObject>();
		if (poolIconLocPowerUps == null){
			Debug.LogError("failed to create poolIconLocAlerts int FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;			
		}	
		
		poolIconLocPhaseNotif = new List<IconLocObject>();
		if (poolIconLocPhaseNotif == null){
			Debug.LogError("failed to create poolIconLocPhaseNotif int FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;			
		}
		poolIconLocCC = new List<IconLocObject>();
		if (poolIconLocCC == null){
			Debug.LogError("failed to create poolIconLocCC int FUI2dLocManager");
			isFullyInit = false;
			return isFullyInit;			
		}	
		
		isFullyInit = true;
		return isFullyInit;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (!isFullyInit){
//			Debug.LogError("did Not fully init 2dLocManager");
			Init();
			//AllocatePool();
			
			return;
		}
	}
	
	
	public IconLocObject CreateIcon(IconLocObject prefab, IconLocObjectTypes type, bool target3D, GameObject target, float duration = -1){
		//Debug.LogWarning("Entered FUI2dLoc Manager Create Icon");
		// We're fully init!  We can create icons objects
		if (isFullyInit){
			//Debug.LogWarning("*****isFullyInit ****");
			IconLocObject newIconObject = null;
			
			// if we're not using a prefab, set it to the index of the type
			if (type != IconLocObjectTypes.PrefabIconLoc){
				// a spawner is requesting to draw an object.  before we just create it, we
				// want to make sure we dont have one available
				// Which list are we looking at?
				bool reusedObject = false;
				switch(type){
					case IconLocObjectTypes.CannonIconLoc:{
						//Debug.LogWarning("Checking Cannons List");
						foreach(IconLocObject tempRef in poolIconLocCannons){
							if(!tempRef.IsActive){
								newIconObject = tempRef;
								reusedObject = true;
								break;
							}
						}
						break;
					}
					case IconLocObjectTypes.EnemyIconLoc:{
					//Debug.LogWarning("Checking EnemyIcon List");
						foreach(IconLocObject tempRef in poolIconLocAlerts){
							if(!tempRef.IsActive){
								newIconObject = tempRef;
								reusedObject = true;
								break;
							}
						}
						break;
					}
					case IconLocObjectTypes.PowerUpIconLoc:{
						foreach(IconLocObject tempRef in poolIconLocPowerUps){
							if(!tempRef.IsActive){
								newIconObject = tempRef;
								reusedObject = true;
								break;
							}
						}
						break;
					}
					case IconLocObjectTypes.PhaseNotifIconLoc:{
						foreach(IconLocObject tempRef in poolIconLocPhaseNotif){
							if(!tempRef.IsActive){
								newIconObject = tempRef;
								reusedObject = true;
								break;
							}
						}
						break;	
					}
					case IconLocObjectTypes.CommandCenterLoc:{
						foreach(IconLocObject tempRef in poolIconLocCC){
							if(!tempRef.IsActive){
								newIconObject = tempRef;
								reusedObject = true;
								break;
							}
						}
						break;	
					}					
				}
				
				
				// we couldn't reuse one, so we create another
				if (!reusedObject){
					newIconObject = NGUITools.AddChild(_myPanel.gameObject, ListOfPrefabs[(int)type].gameObject).GetComponent<IconLocObject>();
					// TODO: Testing just destroyed, weird bug going on...
					switch(type){
						case IconLocObjectTypes.CannonIconLoc:{
							poolIconLocCannons.Add(newIconObject);
							break;
						}
						case IconLocObjectTypes.EnemyIconLoc:{
							poolIconLocAlerts.Add(newIconObject);		
							break;
						}
						case IconLocObjectTypes.PowerUpIconLoc:{
							poolIconLocPowerUps.Add(newIconObject);		
							break;
						}
						case IconLocObjectTypes.PhaseNotifIconLoc:{
							poolIconLocPhaseNotif.Add(newIconObject);		
							break;
						}
						case IconLocObjectTypes.CommandCenterLoc:{
							poolIconLocCC.Add(newIconObject);		
							break;
						}
					}			
				}
			}
			else{
				// we want to use a prefab, but make sure we do have a prefab.
				// if we dont, then use the prefab set to the prefab index, first in the list, and is a backup default
				if (prefab != null){
						// who ever asked to draw the icon did not give a prefab
						// use default
						newIconObject = NGUITools.AddChild(_myPanel.gameObject, prefab.gameObject).GetComponent<IconLocObject>();
					}
				else{
					//newIconObject = NGUITools.AddChild(_myPanel.gameObject, DefaultIconLocPrefab.gameObject).GetComponent<IconLocObject>();
					newIconObject = NGUITools.AddChild(_myPanel.gameObject, ListOfPrefabs[(int)IconLocObjectTypes.PrefabIconLoc].gameObject).GetComponent<IconLocObject>();
					}
			}			
			
			
			
			
			// currently only called the base init, we want to call the derived init
			newIconObject.Init(this, type, target, target3D, duration);		

			// ****** Return the new Icon Object ******* //
			return newIconObject;
		}
		else{
			return null;
		}
	}
	
	public void ReleaseIcon(IconLocObject obj){
		// if using a prefab, destroy it
		if (obj.MyType == IconLocObjectTypes.PrefabIconLoc){
			Destroy(obj.gameObject);
		}
		// other wise free up in the pool
		else{
			obj.Deactivate();
		}
	}
	
	private void AllocatePool(){
		Debug.LogWarning("***** Entered FUI2dLocManager: AllocatePool() ****");
		
		// manual for now
		// for each startpoolCount of a type, create that many locObjects
		for (int i = 0; i < startPoolAlerts; i++){
			CreateIcon(null, IconLocObjectTypes.EnemyIconLoc, false, null, -1);
		}
		for (int i = 0; i < startPoolCannons; i++){
			CreateIcon(null, IconLocObjectTypes.CannonIconLoc, false, null, -1);
		}
		for (int i = 0; i < startPoolPhaseNotif; i++){
			CreateIcon(null, IconLocObjectTypes.PhaseNotifIconLoc, false, null, -1);
		}
		for (int i = 0; i < startPoolPowerups; i++){
			CreateIcon(null, IconLocObjectTypes.PowerUpIconLoc, false, null, -1);
		}		
	}
}
