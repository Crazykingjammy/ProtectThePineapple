using UnityEngine;
using System.Collections;

public class SmileyTarget : Target {


		// Update is called once per frame
	new void Update () {
		
		//call moma's update.
		base.Update();
		
	
	}
	
	void OnCollisionEnter(Collision col)
	{
		
			//If we hit some walls
		if(col.gameObject.CompareTag("Cannon"))
		{
			Cannon c = col.gameObject.GetComponent<Cannon>();
			
			if(GetComponent<Rigidbody>().velocity.magnitude > 10.0f || c.IsBurning)
				{
					//print("BOOM!");
					
					//Play sound
					AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
					
					//Deal the damage
					Damage(col.rigidbody.mass);
				
				c.Explode();
					
					//Get the ball we collided with.
					//Target t = col.gameObject.GetComponent<Target>();
					//t.Damage(500.0f);
					
				}
			
		}
		
		
		///If we hit a ball it should turn red now.
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();
			
		
			//Only look for active balls!
			if(b.IsActive() && b.GetBallSourceID() != Ball.BallSourceID.Netural)
			{
				//Damage
				Damage(b.DamageAmount);
					
				//audio.clip = HitSoundEffect;
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
	
				//Call the balls hit target function to handle the stat registering
				b.HitTarget();
				
//				if(b.GetBallSourceID() == Ball.BallSourceID.Bot)
//				{
//					//Send the message. Its about sending the message
//					GameObjectTracker.GetGOT().TargetHit();	
//				}
				
				
				//Pop the ball if it isnt shoot through.
				if(!b.IsShootThrough())
				{
					b.Pop();
				}
				
			}
			
		}
	}
}
