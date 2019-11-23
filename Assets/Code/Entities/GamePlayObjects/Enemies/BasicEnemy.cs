using UnityEngine;
using System.Collections;

public class BasicEnemy : BaseEnemy {
	
	/* Frank Add, moved these two variables to the baseEnemy
	 * 
	//public float CoolDownTime = 5.0f;
	//float cooldownTimer = 0.0f;

	*/
	
	
	// Use this for initialization
//	protected override void Start () {
//	base.Start();
//
//		currentState = ActionState.Prepare;
//		CurrentAim = EntityAim.Bot;
//		
//		
//	}

//	void OnEnable()
//	{
//		Debug.LogError("Enemy enable");
//		
////		currentState = ActionState.Prepare;
////		CurrentAim = EntityAim.Bot;
//	}

	new public void Update()
	{
		base.FixedUpdate();
	}
	
	public override void FixedUpdate()
	{
		//The core baisc stuff here on top of the update function.
		//base.FixedUpdate();
		
		//Always update teh cool down timer ? 
		cooldownTimer += Time.deltaTime;
		
		
		//We check if we are destroyed so we detach the cannon if we are.
		if(IsDestroyed() && currentState != BaseEnemy.ActionState.Destroyed)
		{
			Cannon c = connectorcontroller.GetCannon();
			
			if(!c)
			{
				//print("No Cannon Available");
				return;
			}
			
			//We are no longer gathering.
			gathering = false;
			// frank add
			gatheringCannon = false;
			
			//Detatch the cannon.
			connectorcontroller.DetachCannon();
				
			
			//Change the state
			currentState = BaseEnemy.ActionState.Destroyed;
			
		}
		
		
		switch(currentState)
		{
			// ** Frank Add
		case ActionState.Reset:
		{
			// when an enemy is destroyed and put back into the pool, reset is called.
			// so set the state to reset
			// if we're active then move along for now, just check for cannon
			if (!connectorcontroller.HasCannon()){
				currentState = ActionState.NeedCannon;
			}
			else{
				currentState = ActionState.Prepare;
			}
			break;
		}
			// Frank End
		case ActionState.CoolDown:
		{
		
			if( (cooldownTimer) > CoolDownTime )
			{
				currentState = ActionState.Prepare;
				//cooldownTimer = Time.time;

				if(PreparingEffect)
					PreparingEffect.Play();
			}
			
			break;
		}
		case ActionState.Attack:
		{
			
			//Set the current aim to command center by default.
			CurrentAim = EntityAim.Bot;
			
			bool bothascannon = false;
			if(Eyes.BotView)
			{
				bothascannon = Eyes.BotView.IsCannonAttached();
			}
			
			//Aim for command centers if we dont have to attack bots and have to attack command centers.
			// or aim if we are set to attack both but bot is poped.
			if( (!bothascannon && AttackCommandCenters ) || 
				(!AttackBots && AttackCommandCenters) )
			{
				CurrentAim = EntityAim.CommandCenter;
			}
			
			
		
			//Check for our exit condition.
			if(connectorcontroller.GetBallsInStorage() <= 0)
			{
				currentState = ActionState.CoolDown;
				cooldownTimer = 0.0f;
				
			}
		
		
			//And then we are always shooting!
			Shoot();	
			
			break;
		}
		case ActionState.Prepare:
		{

			//Handle aim.
			if(AttackCommandCenters)
				CurrentAim = EntityAim.CommandCenter;
			
			if(AttackBots)
				CurrentAim = EntityAim.Bot;


			//If we go below a certian amount we gather more
			if(connectorcontroller.HasCannon())
			{
				GatherBalls(minimumDesiredAmount);
			}
			else{
				currentState = ActionState.NeedCannon;
				break;
			}


			
			if(connectorcontroller.GetBallsInStorage() >= minimumDesiredAmount)// Frank Removed || !connectorcontroller.HasCannon())
			{
				currentState = ActionState.Attack;	

				if(PreparingEffect)
					PreparingEffect.Stop();
			}
			
			
			
			break;
		}
		
		case ActionState.NeedCannon:
		{
			GatherCannons();

			if (connectorcontroller.HasCannon()){
				currentState = ActionState.Prepare;
			}
			break;
		}

		case ActionState.Destroyed:
		{
			
			break;
		}
		default:
		
			break;
		}


		
		
		HandleBallGathering();	
		// Franks add
		HandleCannonGathering();

		// Franks End
		HandleAim();
	
		
	}
	
	//Check for ball collisions and apply damage.
	void OnCollisionEnter(Collision col)
	{
		
		
		//If we hit some walls
		if(col.gameObject.CompareTag("Wall"))
		{
			if(GetComponent<Rigidbody>().velocity.magnitude > 10.0f)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
				//Deal the damage
				Damage(500.0f);
			}
			
		}
		
		//If we hit some walls
		if(col.gameObject.CompareTag("Target"))
		{
		if(GetComponent<Rigidbody>().velocity.magnitude > 10.0f)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
				//Deal the damage
				Damage(SelfMomentumDamage);
				
				//Get the ball we collided with.
				Target t = col.gameObject.GetComponent<Target>();
				t.Damage(TargetMomentumDamage);
				
			}
		
		}
		
		
		
		
		
		///If we hit a ball it should turn red now.
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();
			
		
			//Only look for active balls!
			if(b.IsActive())
			{
				if(b.GetBallSourceID() != Ball.BallSourceID.Netural && b.GetBallSourceID() != Ball.BallSourceID.Enemy)
				{
					
					//Set the Damage
					Damage(b.DamageAmount);
				
					//And we pop the ball.
					b.Pop();
					
					
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
					AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				}
			}
			
			//Here we do a test to ONLY pick up active netural balls and nothing else.
			if(b.GetBallSourceID() == Ball.BallSourceID.Netural)
			{
				//We have to make sure we have a cannon attached to pick up a ball and we make sure sheild is not on.
				if(connectorcontroller.HasCannon())
				{
					
					//Call attach for the cannon that we collide with.
					if(connectorcontroller.GetCannon().PickupBall(b) )
					{
						//Set the ID to the ball
						b.SetBallSourceID(Ball.BallSourceID.Enemy);
						
						//Play the effect for picking up a ball.
						if(Collection)
						{
							//Play the collection animation.
							Collection.Play();
						}
						
					}
						
				}
				
			}
			
		}//Ball Collision Check
	
		//Check if we collide with a cannon.
		if(col.gameObject.CompareTag("Cannon") && !IsDestroyed())
		{
			Cannon c = col.gameObject.GetComponent<Cannon>();
			
			if(c.GetComponent<Rigidbody>().velocity.magnitude > 10.0f || c.IsBurning)
				{
					//print("BOOM!");
					
					//Play sound
					AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
					
					//Deal the damage
					Damage(c.GetComponent<Rigidbody>().mass);
					
				c.Explode();
					//Get the ball we collided with.
					//Target t = col.gameObject.GetComponent<Target>();
					//t.Damage(500.0f);
					
				}
			
			//Call attach for the cannon that we collide with.
			if(connectorcontroller.AttachCannon(c))
			{
				cooldownTimer = CoolDownTime - 0.5f; 
				
				currentState = BaseEnemy.ActionState.CoolDown;
			}
			
			
		}
	
		
	}//All Collisions Check
	


	
}
