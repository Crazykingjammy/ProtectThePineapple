using UnityEngine;
using System.Collections;

public class PowerupAffectSoccerBall : PowerupAffectBase {
	
	// this effect is a bit different than the other.
	// when the affect starts, it will spawn a target object, the soccerball
	// it is this class that just manages the object.  keeps a cache of it and possibly 
	// deactivates when the base affect is complete.
	
	// this is the base target object to spawn, enable and disable
	// the physics object that does all the damage
	public PowerupSoccerBallTarget PowerupSoccerBall;
	
	
	
	// Use this for initialization
	new void Start () {
		base.Start();
		
		// make sure this affect does stick to the bot
		this.basePickup.KeepAffectOnHost = false;
	}
	
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();
		
		if (this.isAffective == false){
			PowerupSoccerBall.transform.position = this.transform.position;
			
		}
	}
}
