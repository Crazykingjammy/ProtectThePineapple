using UnityEngine;
using System.Collections;

public class IconLocObject : MonoBehaviour {
	
	// the Singleton manager of all Icon Location Objects
	FUI2dLocManager myManager;
	public FUI2dLocManager MyManager {
		get{ return myManager; }
	}
	
	// what type am I?
	FUI2dLocManager.IconLocObjectTypes _type;
	public FUI2dLocManager.IconLocObjectTypes MyType {
		get{ return _type; }
	}
	
	GameObject _target = null;
	public GameObject IconLocTarget {
		get{ return _target; }
		set{ _target = value; } 
	}
	
	// Is the target im targeting in 3d?
	private bool _isTarget3d = true;
	
	// Caching the new Screen position, so that it doesn't have to be recreated when we need to update our position
	Vector3 newScreenPos;
	
	// every location icon must have a sprite
	protected UISprite mySprite = null;
	
	// the alpha when visible
	public float alphaVisible = 1;
	// the alpha when disabled		both of these are really specific to the cannon loc icon
	public float alphaFaded = 0.3f;
	
	protected bool isFullyInit = false;
	
	protected bool _isActive = false;
	public bool IsActive {
		get{ return _isActive; }
		set{ _isActive = value; } 
	}
	// a negative -1 means stays on til explicitly turned off
	protected float duration = -1;
	
	protected void Start(){
		// try to find the sprite
		mySprite = gameObject.GetComponentInChildren<UISprite>();
		
//		Debug.Log("New IconLocObject Created");
		//Deactivate up[on start?
	//	Deactivate();
	}
	
	public void UpdateObjectPosition(){
		if (!_isTarget3d){
			UpdateObjectPosition2DTarget();
		}
		else{
			UpdateObjectPosition3DTarget();
		}
	}
	
	public void UpdateObjectPosition2DTarget(){
		
		transform.position = myPosition;
	}
	
	public void UpdateObjectPosition3DTarget(){
		// From the worlds 3d camera, we convert the position to the viewport of
		// the Targets position
		newScreenPos = myManager.WorldCamera.WorldToViewportPoint(myPosition);
		
		// clamp to prepare for the ngui 2d camera.  keeps in the bounds of the 
		// set bounding camera object.  Object is taken from the editor for now.
		newScreenPos.x = Mathf.Clamp01(newScreenPos.x);
		newScreenPos.y = Mathf.Clamp01(newScreenPos.y);
		
		// from the viewport set it to the world position of the camera
		// Now using a second Camera in the 2d UI using that as the viewport 
		newScreenPos = myManager.Canvas.ViewportToWorldPoint(newScreenPos); // fuiCamera.ViewportToWorldPoint(newScreenPos);
		newScreenPos.z = 0;
		
		// set the new position in world coords of the bounding camera
		transform.position = newScreenPos;
	
	}
	
	void OnClick(){
		// when an icon is clicked do what?
		// 
	}
	
	/// <summary>
	/// Initialized this object from the Manager
	/// </param> 
	/// </summary>
	public bool Init(FUI2dLocManager locManager, FUI2dLocManager.IconLocObjectTypes type, GameObject target, bool target3d, float dur){
		if (target == null){
			Debug.LogError("Trying to init IconLocObject with invalid Target");
			return false;
		}
		
		myManager = locManager;
		_type = type;
		_target = target;
		_isTarget3d = target3d;
		duration = dur;		
		
		
		_isActive = true;
		
		// using NGUI Tools to make sure the entire object and children are active.
		// Deactive will not make the parent and children gameobject inactive
		Activate();
		// NGUITools.SetActive(this.gameObject, _isActive);
		
		return _isActive;
	}
	
	
	/// <summary>
	// Being the base, all this does in the late update is updates this objects screen
	// position based on the the set target.
	// Derived objects must call base.lateupdate() in order to work properly
	/// </summary>
	protected void LateUpdate(){
		
		if(_target == null)
		{
			//if(_isActive)
				Deactivate();
			
			return;
		}
		
		if ( _target.gameObject.activeSelf){
			// Just update this position set by my target
			UpdateObjectPosition();
		}
		
//		else{
//			// our target that we're following is GONE!
//			// myManager.ReleaseMe(this);
//			// Debug.Log("" + gameObject.name +" " +"IconLocObject lost target");
//			
//			// TODO testing to just destroy the object,
//			// 6/1/13 weird bug where its not activing correctly when grabbed by the pool
//			 Deactivate();
//			// myManager.ReleaseIcon(this);
//			//Destroy(this.gameObject);
//		}
//	
//		if (!isFullyInit){
//			//Debug.LogError("" + gameObject.name + "icon Loc Object Not fully Init");
//			Deactivate();
//			//myManager.ReleaseIcon(this);
//		}
	}
	
	public void Deactivate(){
//		Debug.Log("IconLocObject - " + gameObject.name + " Deactivating Base IconLocObject");
		isFullyInit = false;
		//mySprite.alpha = 0.0f;
		_isActive = false;	

		NGUITools.SetActive(this.gameObject, _isActive);
	}
	
	public void Activate(){
//		Debug.Log("IconLocObject - Activating IconLocObject");
		
		_isActive = true;	
		NGUITools.SetActive(this.gameObject, _isActive);		
	}
	
	
	public virtual Vector3 myPosition
	{
		get
		{
			return _target.transform.position;	
		}
	}
}
