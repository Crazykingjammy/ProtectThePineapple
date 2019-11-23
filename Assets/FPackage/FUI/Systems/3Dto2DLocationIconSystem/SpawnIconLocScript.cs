using UnityEngine;
using System.Collections;

public class SpawnIconLocScript : MonoBehaviour {
	
	public FUI2dLocManager.IconLocObjectTypes SpawnType;
	
	public bool isTarget3D = true;
	
	// each spawner type will have to take in a prefab.  
	// The predefined types will have a default prefab to fall back on
	// if you want this locScript to spawn a special prefab select Type PRefab
	// and be sure to supply a prefab
	public IconLocObject prefab = null;
	
	// this is so that the DrawIcon can check and return the new Object
	// responsibility of the derived script to manage its own type created, by grabbing the 
	// necessary component
	private IconLocObject cacheObject;	
	
	// have I drawn my Icon - defaults false
	protected bool isDrawn = false;
	
	public float Duration = -1;
	
	public bool allowSpawn = true;
	
	public bool DrawHealthBar = false;
	
	// Check to see if the attached object is a CC.  
	// CC draw health differently, always appearing on UI, not drawn in 3D space
	// CC health will follow the CC and drawn over the UI like the icons work 
	// for Cannons and enemies
	
	
	
	
	
	// Use this for initialization
	void Start () {
//		Debug.Log("SpawnIconLocScript - Start()");
		
		if(!FUI3DManager.GetLM)
			return;
		
		if (DrawHealthBar){
			FUI3DManager.GetLM.CreateHealthBar(this.gameObject);
		}
//		
	}
	
	void OnEnable()
	{
//		if (DrawHealthBar){
//			FUI3DManager.GetLM.CreateHealthBar(this.gameObject);
//		}
		
	}
	
	void OnDisable()
	{
		cacheObject = null;
	}

	
	// Update is called once per frame
	void Update () {

		
		if (cacheObject == null){
			DrawIcon(Duration);		
		}
		//}
		
		// weird bug that sometimes the 2dLocManager will reference an existing
		// icon location object.  
//		if (cacheObject != null){
//			if (!cacheObject.IsActive){
//				DrawIcon();
//			}
//		}
//		
	
	}
		
	/// <summary>
	/// Currently creates and displays the prefab.  
	/// If the manager has one create of the same type of IconLocObject
	/// If will reset that one
	/// 
	/// Returns the newly set iconlocobject
	/// 
	/// Duration of -1 means that the icon will last until its object is gone
	/// </summary>
	protected IconLocObject DrawIcon(float duration = -1){
		FUI2dLocManager ILM = FUI2dLocManager.GetLM;
		
		if (ILM == null){
			Debug.LogWarning("FUI2dLocManager has not been created yet");
			return null;
		}
		else{
			// Debug.LogWarning("**** "+gameObject.name + " Going to spawn Icon");
			if (allowSpawn){
				//cacheObject = null;
				cacheObject = ILM.CreateIcon(prefab, SpawnType, isTarget3D, this.gameObject, duration);
				if (cacheObject != null){
					isDrawn = true;
					return cacheObject;
				}
				else{
					return null;
				}				
			}
			else{
				return null;
			}
		}
	}
}
