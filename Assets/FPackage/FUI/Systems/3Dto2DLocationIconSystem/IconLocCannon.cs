using UnityEngine;
using System.Collections;

public class IconLocCannon : IconLocObject {
	
	Cannon _cannon = null;
	string cannontexturename = "test";
	
	bool hiding = false;
	
	public UISprite CannonBG;
	public UISprite CannonOnFireSprite;
	
	// Use this for initialization
	new void Start () {
		base.Start();
	}
		
	// Update is called once per frame
	new void LateUpdate () {
		base.LateUpdate();
		
		if (isFullyInit){
			
			SetCannon();
			CheckAlpha();
			
			if(!_cannon.gameObject.activeSelf)
			{
				Deactivate();
			}
		}
		else{
			SetCannon();
		}
	}
	
	void CheckAlpha(){
		if (ToyBox.GetPandora().Bot_01.IsCannonAttached()){
			mySprite.alpha = alphaFaded;
			CannonBG.alpha = 0.0f;
			
		}
		else{
			mySprite.alpha = alphaVisible;
			CannonBG.alpha = alphaVisible;
		}
		if (_cannon == null){
			mySprite.alpha = 0.0f;
			CannonBG.alpha = 0.0f;
			return;
		}
		if(_cannon.InUse || IconLocTarget == null || !_cannon.gameObject.activeSelf)
		{
	
			mySprite.alpha = 0.0f;
			CannonBG.alpha = 0.0f;
		}

		if (_cannon.IsBurning){
			CannonOnFireSprite.gameObject.SetActive(true);
		}
		else{
			CannonOnFireSprite.gameObject.SetActive(false);
		}
		
	}
	
	bool SetCannon(){
		// if we're active in the scene, then we should have a target to follow
		// when we're added into the scene the 2d manager assigns the target based on who
		// spawned me
		if (IconLocTarget == null){
			return false;
		}
		_cannon = IconLocTarget.GetComponent<Cannon>();
		if (_cannon){
			cannontexturename = _cannon.CannonTextureName;
			mySprite.spriteName = cannontexturename;
			isFullyInit = true;
			return true;
		}
		
		return isFullyInit;
	}
	
	new public bool Init(FUI2dLocManager locManager, FUI2dLocManager.IconLocObjectTypes type, GameObject target, bool target3d, float dur){
		Debug.LogWarning("Init from IconLocCannon.cs");
		
		
		bool isInit = false;
		
		isInit = base.Init(locManager, type, target, target3d, dur);
		
		if (!isInit){
			// we failed, return false;
			return isInit;
		}
		else{
			isInit = false;	// reset to false
			// test to see if we have cannon
			isInit = SetCannon();
		}
		
		return isInit;
	}
}

