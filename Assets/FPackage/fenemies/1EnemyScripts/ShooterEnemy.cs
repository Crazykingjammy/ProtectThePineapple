using UnityEngine;
using System.Collections;

/// <summary>
/// Shooter enemy.
/// 
/// This is a higher leven Base Enemy.  it is to replace the Basic Enemy class
/// 
/// Currently this removes the collision information.  It has been created to retain legacy objects that use
/// basic enemy
/// 
/// Collisions are handled by behaviors now.
/// 
/// All this class does is manage a state machine for gathering cannons and balls.
/// 
/// in order to actually pickup this object needs to have a pickup behavior to actually be able to pickup 
/// cannons and balls.  This is just a state machine for handling gathering (TODO: this should just be a behavior
/// 
/// </summary>

public class ShooterEnemy : AnimatingEnemy {

	Rigidbody rigidBodyCache = null; // Caching my rigid body

	//public Transform ShooterArm = null;  // going to use the arm to aim

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
	
	new public void OnEnable(){
		base.OnEnable();

		Debug.Log("Enemy shooter enabled");
		
		currentState = ActionState.Prepare;
		
		// this finds out if the bot has a cannon equipt, if not aim for the CC
		FigureOutAim();

		if (this.GetComponent<Rigidbody>() != null){
			rigidBodyCache = this.GetComponent<Rigidbody>();
		}
		}

	new public void Update(){
//		Debug.LogError("ShooterEnemy Update   " + name + "   **");

		base.Update();
	}

	new public void FixedUpdate()
	{
		//The core baisc stuff here on top of the update function.
		base.FixedUpdate();

		// this finds out if the bot has a cannon equipt, if not aim for the CC
		FigureOutAim();

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

		// before checking states, quickly check our rigid body velocity
		// if its greater than X we enter pushed state
		// TODO: state needs to change based on Player pushing!
		if (rigidBodyCache.velocity.magnitude > this.PushedVelocityEnter){
			this.currentState = ActionState.Pushed;
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
			currAnimState = EnemyAnimStates.Idle;
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
			currAnimState = EnemyAnimStates.Attack;

			// this finds out if the bot has a cannon equipt, if not aim for the CC
			FigureOutAim();
			
			
			
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
			currAnimState = EnemyAnimStates.Idle;
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
			currAnimState = EnemyAnimStates.Idle;
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
		case ActionState.Pushed:
		{
			currAnimState = EnemyAnimStates.Pushed;

			if (rigidBodyCache.velocity.magnitude <= this.PushedVelocityExit){
				this.currentState = ActionState.CoolDown;
			}

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

	protected void FigureOutAim(){
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
	}
}