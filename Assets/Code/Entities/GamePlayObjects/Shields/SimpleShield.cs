using UnityEngine;
using System.Collections;

public class SimpleShield : Shield {
	
	
	// Use this for initialization
	void Start () {

		//Create and Set the position of the gravity affect
		GravityEffect = Instantiate(GravityEffect) as ParticleSystem;
		GravityEffect.transform.parent = transform;
		GravityEffect.transform.localPosition = Vector3.zero;
		
		
		ShieldActivationEffect = Instantiate(ShieldActivationEffect) as ParticleSystem;
		ShieldActivationEffect.transform.parent = transform;
		ShieldActivationEffect.transform.localPosition = Vector3.zero;
	

//		EmptyShatterEffect = Instantiate(EmptyShatterEffect) as ParticleSystem;
//		EmptyShatterEffect.transform.parent = transform;
//		EmptyShatterEffect.transform.localPosition = Vector3.zero;
//		
//		ShieldShatterEffect = Instantiate(ShieldShatterEffect) as ParticleSystem;
//		//ShieldShatterEffect.transform.parent = transform;
//		ShieldShatterEffect.transform.localPosition = Vector3.zero;
		
		
		
	}
	
	// Update is called once per frame

	
	
	void OnTriggerStay(Collider col)
	{
		
		GameObject go = col.gameObject;
		Rigidbody rbdy = go.GetComponent<Rigidbody>();
		
		//We have to check if the collider has a rigid body since we will pull everything with the shield.
		if(!rbdy)
		{
			return;
		}
		
			//Dont pull targets
		if(go.CompareTag("PhysicsPart") )
		{	
		
			//Pull physcis parts at half strength.
			PullToCenter(go,rbdy.mass * 0.5f);
			return;
		}
		
			//We check if object is collidign with balls then apply to the pull of the black hole.
		if(go.CompareTag("Ball") )
		{
			Ball b = col.gameObject.GetComponent<Ball>();
			
			//here we check if the ball is active. If it is, we will let it be!
			//We leave the function and go to the next object as we dont want to pull this object if its a activated ball.
			if(b.IsActive() && !b.GetGravityControl())
			{
				return;
			}
			
		}
		
		//Dont pull targets
		if(go.CompareTag("Target") )
		{	
			//print("Target Detected" + col.gameObject.name);
			Target t = col.GetComponent<Target>();
			
			if(!t.AffectedByShieldGravity)
				return;
		}
		
		
		if(go.CompareTag("Cannon"))
		{
			//print("Cannon " + col.gameObject.name);
		
			Cannon c = col.GetComponent<Cannon>();
			
			//Dont pull cannons if the bot/host has a cannon already!  
			// nah, forget this rule -Frank

			//if(host.IsCannonAttached() && !c.IsBurning)
			//	return;
			
		}
		
		
		//Dont pull command centeres.a
		if(go.CompareTag("CommandCenter") )
		{	
			//print("Target Detected" + col.gameObject.name);
			return;
		}
		
		
		//Dont pull the a bot?
		if(go.CompareTag("Bot") )
		{
			return;
		}
		
		
		
		
	
		
		
		//Call attach for the cannon that we collide with.
		PullToCenter(go, rbdy.mass);	
		
		
	}
}
