using UnityEngine;
using System.Collections;

public class BaseEnemyDamageBehavior : BaseEnemyBehaviorScript {
	

	public float HitWallMagnitude = 10.0f;
	public float HitWallDamage = 500.0f;
	public float HitTargetMagnitude = 10.0f;
	public float HitTargetDamage = 500.0f;
	public float HitTargetSelfDamage = 500.0f;
	public float HitCannonMagnitude = 10.0f;
	public float HitCannonDamage = 200.0f;
	public float HitCannonBurningDamage = 1000.0f;
	public float HitCannonHeatDamage = 10.0f;

	public bool canTouchDamage = true;
	// when I collide with command center or bot, how much damage do I give
	public float TouchDamageToBot = 250.0f;
	public float TouchDamageToCC = 250.0f;
	// when I collide with command center or bot, how much damage to I do to myself
	// 1000 is pretty much kill myself.
	public float touchDamageToSelf = 1000.0f;

	// how often do I do touch damage?
	public float TouchDamageFreq = 2.0f;



	protected float touchDamageClock = 0.0f;
	protected float touchDamageTimer = 0.0f;  // going to try to add to a delta
	






	// Use this for initialization
	new void Start () {
		base.Start();

		canTouchDamage = true;
	}

	void OnEnable(){
		canTouchDamage = true;
	}

	new void FixedUpdate(){
		base.FixedUpdate();

		//float currTime = Time.time;
		if (!canTouchDamage){

			touchDamageTimer += Time.fixedDeltaTime;
			if (touchDamageTimer > TouchDamageFreq){
				canTouchDamage = true;
				touchDamageTimer = 0.0f;
			}
//			if ((currTime - touchDamageClock) > TouchDamageFreq){
//				canTouchDamage = true;
//			}
		}
	}

	//Check for all possible collisions and apply any effects.
	// mostly taken from BasicEnemy
	new protected void OnCollisionEnter(Collision col)
	{
		base.OnCollisionEnter(col);
		// make sure we're still attached to a valid target object
		if (self == null)
			return;

		// checks bot and CC and does damage based on time
		ProcessBotandCCTouchDamage(col);

		// thinking about doing this in a switch statement instead of a bunch of ifs
		//string objectTag = col.gameObject.tag;

		float selfMagnitude = self.GetComponent<Rigidbody>().velocity.magnitude;
		
		//If we hit some walls
		if(col.gameObject.CompareTag("Wall"))
		{
			if(selfMagnitude > HitWallMagnitude)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(self.HitSoundEffect);
				
				//Deal the damage
				self.Damage(HitWallDamage * selfMagnitude);
			}
			
		}
		
		//If we hit a target
		if(col.gameObject.CompareTag("Target"))
		{

			if(selfMagnitude > HitTargetMagnitude)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(self.HitSoundEffect);
				
				//Deal the damage
				self.Damage(HitTargetSelfDamage * selfMagnitude);
				
				//Get the target we collided with.
				Target t = col.gameObject.GetComponent<Target>();
				t.Damage(HitTargetDamage * selfMagnitude);
				
			}
			
		}
		///If we hit a ball do some damage...
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();
			
			
			//Only look for active balls!
			if(b.IsActive())
			{
				if(b.GetBallSourceID() != Ball.BallSourceID.Netural ) //&& b.GetBallSourceID() != Ball.BallSourceID.Enemy)
				{
					
					//Set the Damage
					self.Damage(b.DamageAmount);
					
					//And we pop the ball.
					if(!b.IsShootThrough())
					{
						b.Pop();
					}
					
					
					//Call the balls hit target function to handle the stat registering
					b.HitTarget();
					
					//				if(b.GetBallSourceID() == Ball.BallSourceID.Bot)
					//				{
					//						
					//					//Send The Message
					//					GameObjectTracker.GetGOT().EnemyHit();
					//				
					//				}
					
					
					
					//Play the sound.
					AudioPlayer.GetPlayer().PlaySound(self.HitSoundEffect);
				}
			}
		}//Ball Collision Damage Check
		
		//Check if we collide with a cannon.
		if(col.gameObject.CompareTag("Cannon") && !self.IsDestroyed())
		{
			Cannon c = col.gameObject.GetComponent<Cannon>();
			float cannonMagnitude = c.GetComponent<Rigidbody>().velocity.magnitude;
			if(cannonMagnitude > HitCannonMagnitude || c.IsBurning)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(self.HitSoundEffect);
				
				//Deal the damage
				if (c.IsBurning)
					self.Damage(HitCannonBurningDamage);
				else
					self.Damage(HitCannonDamage * cannonMagnitude);
				
				c.Heat+=HitCannonHeatDamage;

				//Get the ball we collided with.
				//Target t = col.gameObject.GetComponent<Target>();
				//t.Damage(500.0f);
				
			}			
		}
		
		
	}//All Collisions ON Enter Check for damage

	//Check for ball collisions and apply damage.
	new protected void OnCollisionStay(Collision col)
	{	
		base.OnCollisionStay(col);

		// checks bot and CC and does damage based on time
		ProcessBotandCCTouchDamage(col);

		// DO the damage
		// TODO add animation call here

	}//All Collisions ON Stay Check

	protected void ProcessBotandCCTouchDamage(Collision col){

		//We must make sure we have a self... if not.. well... return!
		if(self == null)
			return;

		// when was the last time we did damage?  able to do damage every two seconds

		if (!canTouchDamage){
			return;
		}

		//Check if we collide with a CommandCenter.
		if(col.gameObject.CompareTag("CommandCenter") && (!self.IsDestroyed() )) //&& selfAsEnemy.AttackCommandCenters))
		{
				CommandCenter c = col.gameObject.GetComponent<CommandCenter>();
				
				c.Damage(TouchDamageToCC);
				self.Damage(touchDamageToSelf);  // this may not kill me
				// we do our damage, set can do damage off and hit the clock
				canTouchDamage = false;
				touchDamageClock = Time.time;
				
				//Send the detonation message to the GOT
				GameObjectTracker.GetGOT().TargetDetonated();		
			
		}
		//Check if we collide with a Player Bot
		//TODO:CRashing here.
		if(col.gameObject.CompareTag("Bot") && (!self.IsDestroyed() ) )//&& selfAsEnemy.AttackBots))
		{
			Bot b = col.gameObject.GetComponent<Bot>();
			
			//Dont blow up if shield is active.
			if(b.IsShieldActive())
				return;
			
			b.Damage(TouchDamageToBot);	
			self.Damage(touchDamageToSelf);  // this may not kill me
			
			//Send the detonation message to the GOT
			GameObjectTracker.GetGOT().TargetDetonated();
			// we do our damage, set can do damage off and hit the clock
			canTouchDamage = false;
			touchDamageClock = Time.time;			
			
		}



		// end processing bot and cc
	}

}
