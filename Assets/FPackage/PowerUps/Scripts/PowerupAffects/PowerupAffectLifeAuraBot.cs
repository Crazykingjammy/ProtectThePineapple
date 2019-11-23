using UnityEngine;
using System.Collections;

public class PowerupAffectLifeAuraBot : PowerupAffectLifeAura {

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
		if(col.gameObject.CompareTag("Bot"))
		{
			Bot b = col.gameObject.GetComponent<Bot>();
			// if we touch the command Center add health, 
			// but only certain amount per second
			if (b && (isAffective && (Time.time - timeStartedGivingHealth) > healthGivingFreq)){				
				// TODO: for now, we are going to add negative damage, should work
				b.AddHeat(-healthToGive);
				
				//Send the message.
				GameObjectTracker.instance.CoolDownBot(healthToGive);
				
				// print("Giving +" + healthToGive + " to Player!");
				timeStartedGivingHealth = Time.time;
			}
		}
		
	}//All Collisions Check
	
}
