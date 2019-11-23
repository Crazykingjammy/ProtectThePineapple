using UnityEngine;
using System.Collections;

public class PowerupAffectDeathAura : PowerupAffectBase {

	// how much life to give per second
	public int damageToDeal = 100;
	public float damageDoingFreq = 1.0f;
	protected float timeStartedDoingDamage = 0;  // updates to give time as per the frequency
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();
		
		// for this affect we always point forward
		transform.localRotation.Set(0, 0, 0, 1);// .localPosition.Set(0.0f,0.0f,0.0f);
		transform.forward = Vector3.forward;
	}
	
	public override void ReceiveColliderInfo(Collider col){
				//Check if we collide with a CommandCenter.
		if(col.gameObject.CompareTag("Target"))
		{
			Target t = col.gameObject.GetComponent<Target>();
			// if we touch the command Center add health, 
			// but only certain amount per second
			if (isAffective && !t.IsDestroyed() && (Time.time - timeStartedDoingDamage) > damageDoingFreq){				
				// TODO: for now, we are going to add negative damage, should work
				t.Damage(damageToDeal);
				// print("Doing +" + damageToDeal + " Damage to Target!");
				
				//Call the message
				GameObjectTracker.GetGOT().DeathBeamAttack();
				
				timeStartedDoingDamage = Time.time;
			}
		}
	}
	
	public override void ReceiveCollisionInfo(Collision col){
				//Check if we collide with a CommandCenter.
		if(col.gameObject.CompareTag("Target"))
		{
			Target t = col.gameObject.GetComponent<Target>();
			// if we touch the command Center add health, 
			// but only certain amount per second
			if (isAffective && !t.IsDestroyed() && (Time.time - timeStartedDoingDamage) > damageDoingFreq){				
				// TODO: for now, we are going to add negative damage, should work
				t.Damage(damageToDeal);
				// print("Doing +" + damageToDeal + " Damage to Target!");
				
				//Call the message
				GameObjectTracker.GetGOT().DeathBeamAttack();
				
				timeStartedDoingDamage = Time.time;
			}
		}
	}
	
//	void OnTriggerStay(Collider col)
//	{	
//
//		
//	}//All Collisions Check
}
