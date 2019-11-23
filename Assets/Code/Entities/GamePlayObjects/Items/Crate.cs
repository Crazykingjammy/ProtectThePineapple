using UnityEngine;
using System.Collections;

public class Crate : Target {
	
	
	public bool Bomb = false;
	public float BombDamage = 100.0f;
	
	
	
		// Update is called once per frame
	new void Update () {
		
		//call moma's update.
		base.Update();
		
		
		if(IsDestroyed())
		{
			//Hide our mesh
			GetComponent<Renderer>().enabled = false;
		}
	
	}
	
	protected override void Reset()
	{
		base.Reset();
		
		//Get rid of the cannon.
		
		//Make sure the renderer is enabled.
		GetComponent<Renderer>().enabled = true;
		
		
	}
	
	
//	void OnCollisionEnter(Collision col)
//	{
//		
//		GameObject go = col.gameObject;
//		
//		if(go.CompareTag("Cannon"))
//		{
//			Cannon c = go.GetComponent<Cannon>();
//			
//			if(rigidbody.velocity.magnitude > 6.0f || c.IsBurning)
//				{
//					//print("BOOM!");
//					
//					//Play sound
//					AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
//					
//					//Deal the damage
//					Damage(col.rigidbody.mass * 3.0f);
//
//				//TODO: fix this. hack for the thing.
//				if(c.IsBurning)
//					Damage(120.0f);
//				
//				c.Explode();
//					
//					//Get the ball we collided with.
//					//Target t = col.gameObject.GetComponent<Target>();
//					//t.Damage(500.0f);
//					
//				}
//			
//		}
//		
//		
//
//				//If we hit some walls
//		if(go.CompareTag("Wall") && MomentumDamage)
//		{
//			if(rigidbody.velocity.magnitude > 10.0f)
//			{
//				//print("BOOM!");
//				
//				//Play sound
//				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
//				
//				//Deal the damage
//				Damage(TargetMomentumDamage);
//			}
//			
//		}
//		
//		//If we hit some walls
//		if(go.CompareTag("Target"))
//		{
//		if(rigidbody.velocity.magnitude > 10.0f)
//			{
//				//print("BOOM!");
//				
//				//Play sound
//				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
//				
//				//Deal the damage
//				Damage(SelfMomentumDamage);
//				
//				//Get the ball we collided with.
//				Target t = go.GetComponent<Target>();
//				t.Damage(TargetMomentumDamage);
//				
//			}
//		
//		}
//		
//		
//		///If we hit a ball it should turn red now.
//		if(col.gameObject.CompareTag("Ball"))
//		{
//			//Get the ball we collided with.
//			Ball b = go.GetComponent<Ball>();
//			
//		
//			//Only look for active balls!
//			if(b.IsActive() && b.GetBallSourceID() != Ball.BallSourceID.Netural)
//			{
//				//Damage
//				Damage(b.DamageAmount);
//				
//				//audio.clip = HitSoundEffect;
//				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
//				
//				if(b.GetBallSourceID() == Ball.BallSourceID.Bot)
//				{
//					//Send the message. Its about sending the message
//					GameObjectTracker.GetGOT().TargetHit();	
//				}
//				
//				
//				//Pop the ball if it isnt shoot through.
//				if(!b.IsShootThrough())
//				{
//					b.Pop();
//				}
//				
//			}
//			
//		}
//		
//		
//		//Check if we collide with a Player Bot
//		if(go.CompareTag("Bot") && (!IsDestroyed() && Bomb))
//		{
//			Debug.LogError("BOT HIT");
//
//			Bot b = go.GetComponent<Bot>();
//
//			//If shield is active do nothing...
//			if(b.IsShieldActive())
//				return;
//
//			b.Damage(BombDamage);	
//			
//			Damage(MaxHealth);  // this may not kill me
//			
//			//Send the detonation message to the GOT
//			GameObjectTracker.GetGOT().TargetDetonated();
//			
//		}
//		
//		
//		
//	}//OnCollisionEnter()
//	
	
	
}

