using UnityEngine;
using System.Collections;

public class PowerupAffectLifeAura : PowerupAffectBase {
	
	// how much life to give per second
	public int healthToGive = 100;
	public float healthGivingFreq = 1.0f;
	protected float timeStartedGivingHealth = 0;  // updates to give time as per the frequency
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();
	}
	
	void OnTriggerStay(Collider col)
	{	
		//Check if we collide with a CommandCenter.
		if(col.gameObject.CompareTag("CommandCenter"))
		{
			CommandCenter c = col.gameObject.GetComponent<CommandCenter>();
			// if we touch the command Center add health, 
			// but only certain amount per second
			if (isAffective && (Time.time - timeStartedGivingHealth) > healthGivingFreq){				
				
				// TODO: for now, we are going to add negative damage, should work
				c.Damage(-healthToGive);
				
				//Update teh stat.
				GameObjectTracker.instance.HealthCC(healthToGive);
				
				// print("Giving +" + healthToGive + " to Comman Center!");
				timeStartedGivingHealth = Time.time;
			}
		}
		
	}//All Collisions Check
}
