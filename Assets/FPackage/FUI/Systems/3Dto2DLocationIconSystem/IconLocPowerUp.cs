using UnityEngine;
using System.Collections;

public class IconLocPowerUp : IconLocObject {
	
	PowerupBase _powerup = null;
	string _powerUpSpriteName = "PowerUpDefaultTxt";
	
	
	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		base.LateUpdate();
		
		if (isFullyInit){
			if(_powerup.IsPickedUp() == true || _powerup.IsBackAtSpawner() == true){
				mySprite.alpha = 0.0f;
			}
			else{
				mySprite.alpha = this.alphaVisible;
			}
		
		}
		else{
			GetPU();
		}
	}
	
	void GetPU(){
		_powerup = IconLocTarget.GetComponent<PowerupBase>();
		if (_powerup != null){
			_powerUpSpriteName = _powerup.PowerUpTextureName;
			mySprite.spriteName = _powerUpSpriteName;
			isFullyInit = true;
		}
	}
}
