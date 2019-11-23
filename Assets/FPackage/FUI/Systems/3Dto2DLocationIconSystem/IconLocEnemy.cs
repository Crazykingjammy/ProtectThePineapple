using UnityEngine;
using System.Collections;

public class IconLocEnemy : IconLocObject {
	
	public UISprite Icon, CannonIcon;
	public UISprite BG, Focus;
	BaseEnemy _enemy = null;
	
	public string BubbleSpriteName, ThoughtSpriteName;
	
	public Transform _tBG, _tIcon;
	
	private bool _isSeeker = false;
	
	new void Start(){
		base.Start();
		isFullyInit = Init();
		
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		base.LateUpdate();
		
		if (!isFullyInit || _enemy == null){
			Init();
			return;
		}

	
		// because kevaad is pooling, for some reason i had to check 
		// the actual GO active state.  
		// now testing is destroyed
		// now testing is Disabled
		if ( _enemy.IsDestroyed() || !_enemy.gameObject.activeSelf)
		{ 
			this.Deactivate();
		}
		else{
			// set visibility 
			// play with notification
			switch(_enemy.CurrentState){
			
			case BaseEnemy.ActionState.Attack:{
					
				// Set the BG
				BG.alpha = alphaVisible;
				
				if(_enemy.ConnectController.HasCannon())
				{
					//When attacking lets display the animal.
					//Set the Cannon icon to visible
					CannonIcon.spriteName = _enemy.ConnectController.GetCannon().CannonTextureName;	
				
					CannonIcon.alpha = alphaVisible;
					
				}
				
				//Set the sprite if the enemy is gunning for the CC.
//				if(_enemy.CurrentAim == BaseEnemy.EntityAim.CommandCenter)
//				{
//					Focus.spriteName = "EnemyAlertTargetPineapple";
//					//mySprite.color  = Color.white;
//				}
//				
//				if(_enemy.CurrentAim == BaseEnemy.EntityAim.Bot)
//				{
//					Focus.spriteName = "Hud_Score";
//				}
				
				
				break;
			}
			case BaseEnemy.ActionState.Prepare:{
				
				break;
			}
						
			default:{

				//Disable visuals upon default
				BG.alpha = 0.0f;
				CannonIcon.alpha = 0.0f;

				if (_isSeeker){

					//Icon.alpha = alphaVisible;
					//Icon.spriteName = "EnemyAlertSeeker";
					//mySprite.color = Color.white;
					
					//Set the bg
					//BG.alpha = 0.0f;
					//BG.spriteName = ThoughtSpriteName;
				}

				break;
				}
			}
		}

	}
	
	bool Init(){
		// make sure we're connected to a BaseEnemy
		if (this.IconLocTarget == null){
			isFullyInit = false;
			return isFullyInit;
		}
		
		// check to make sure its active, as the enemies are now pooled
		if (!IconLocTarget.activeSelf){
			isFullyInit = false;
			return isFullyInit;		
		}
		
		BaseEnemy tempEnemy = this.IconLocTarget.GetComponent<BaseEnemy>();
		
		if (tempEnemy == null){
			Debug.Log("" + gameObject.name + " No Enemy attached to IconLocEnemy");
			isFullyInit = false;
			return isFullyInit;
		}
		
		// We're attached to an enemy, now we can processes it
		_enemy = tempEnemy;	
		
				
		// check to see if its a seeker
		_isSeeker = false;
		SeekerScript tempSeeker = this.IconLocTarget.GetComponent<SeekerScript>();
		if (tempSeeker != null){
			_isSeeker = true;
		}
		else{
			_isSeeker = false;
		}
		
		//Hide the cannon icon untill use
		//CannonIcon.gameObject.SetActive(false);
		BG.alpha = 0.0f;
		CannonIcon.alpha = 0.0f;
		
		
		
		// make sure we're on
		//Activate();
		
		isFullyInit = true;
		
		return isFullyInit;
	}
	
	new public void Deactivate(){
//		Debug.Log("IconLocEnemy - Deactivating IconLocEnemy");
		_isSeeker = false;
		_enemy = null;
		
		//CannonIcon.gameObject.SetActive(false);
		
		base.Deactivate();
		

	}
	
	new public void Activate(){
//		Debug.Log("IconLocEnemy - Activating IconLocEnemy");
		base.Activate();
		
		_isSeeker = false;
	}
	
	
	public override Vector3 myPosition
	{
		get
		{			
			
			if(_enemy)
				return _enemy.CrownPosition;
			
			return base.myPosition;
			
		}
	}
}
