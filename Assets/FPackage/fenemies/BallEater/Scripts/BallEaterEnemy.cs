using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallEaterEnemy : AnimatingEnemy {

	/*
	 * Ball Eater Enemy derived from Base Enemy
	 * 
	 * Walk around eating in active ball bullets, maybe even shot bullets
	 * 
	 * Uses BallEaterEnemyBehavior as main behavior script.
	 */

	BallEaterBehavior _mainBehavior = null;
	public BallEaterBehavior MainBehavior {
		get { return _mainBehavior; }
		set { 
			if (_mainBehavior == null)
				_mainBehavior = value;
			// may want to create one if one doesn't exist
		}
	}

	public int MaxBallCount = 100;
	protected int currentBallCount = 0;
	public int CurrentBallCount {
		get{ return currentBallCount; }
		set{ currentBallCount = value; }
	}

	public Material DefaultMaterial = null;
	public Material AttackingMaterial = null;
	public Material DestroyedMaterial = null;
	public GameObject BodyToChangeMaterial = null;

	public Material BallPopMaterial = null;
	public Transform BallPopTransform = null;
	//public BallEaterBallTracker BallTracker = null;

	private List<Ball> _listOfBalls = null;

	// cache what ball we're currently targeting with easy modifiers
	private Ball currentTargetedBall = null;
	public Ball CurrentTargetedBall {
		get{ return currentTargetedBall; }
		set{ currentTargetedBall = value; }
	}

	float currentBallDist = int.MaxValue;
	float closestBallDist = int.MaxValue;
	
	Vector3 currentBallVec = Vector3.zero;



	// Use this for initialization
	protected override void Start () {
		// from Basic Enemy
		base.Start();

		_listOfBalls = new List<Ball>();

	}

	new public void Update()
	{
		base.Update();
		currAnimState =  EnemyAnimStates.Idle;
	
	}

	void OnDestroy(){

		//_listOfBalls = null;
		//_mainBehavior = null;
	}

	new public void FixedUpdate(){
		// From BasicEnemy
		//The core baisc stuff here on top of the update function.
		//base.FixedUpdate();
		base.FixedUpdate();

//		Always update teh cool down timer ? 
//		cooldownTimer += Time.deltaTime;
//
//
//		//We check if we are destroyed so we detach the cannon if we are.
//		if(IsDestroyed() && currentState != BaseEnemy.ActionState.Destroyed)
//		{
//			Debug.LogWarning("Ball Eater fixed update Check");
//			Cannon c = connectorcontroller.GetCannon();
//			
//			if(!c)
//			{
//				//print("No Cannon Available");
//				return;
//			}
//			
//			//We are no longer gathering.
//			gathering = false;
//			// frank add
//			gatheringCannon = false;
//			
//			//Detatch the cannon.
//			connectorcontroller.DetachCannon();
//			
//			
//
//		}
//		
//		
//		switch(currentState)
//		{
//			// ** Frank Add
//		case ActionState.Reset:
//		{
//			// when an enemy is destroyed and put back into the pool, reset is called.
//			// so set the state to reset
//			// if we're active then move along for now, just check for cannon
//			if (!connectorcontroller.HasCannon()){
//				currentState = ActionState.NeedCannon;
//			}
//			else{
//				currentState = ActionState.Prepare;
//			}
//			break;
//		}
//			// Frank End
//		case ActionState.CoolDown:
//		{
//			
//			if( (cooldownTimer) > CoolDownTime )
//			{
//				currentState = ActionState.Prepare;
//				//cooldownTimer = Time.time;
//				
//				if(PreparingEffect)
//					PreparingEffect.Play();
//			}
//			
//			break;
//		}
//		case ActionState.Attack:
//		{
//			
//			//Set the current aim to command center by default.
//			CurrentAim = EntityAim.Bot;
//			
//			bool bothascannon = false;
//			if(Eyes.BotView)
//			{
//				bothascannon = Eyes.BotView.IsCannonAttached();
//			}
//			
//			//Aim for command centers if we dont have to attack bots and have to attack command centers.
//			// or aim if we are set to attack both but bot is poped.
//			if( (!bothascannon && AttackCommandCenters ) || 
//			   (!AttackBots && AttackCommandCenters) )
//			{
//				CurrentAim = EntityAim.CommandCenter;
//			}
//			
//			
//			
//			//Check for our exit condition.
//			if(connectorcontroller.GetBallsInStorage() <= 0)
//			{
//				currentState = ActionState.CoolDown;
//				cooldownTimer = 0.0f;
//				
//			}
//			
//			
//			//And then we are always shooting!
//			Shoot();	
//			
//			break;
//		}
//		case ActionState.Prepare:
//		{
//			//If we go below a certian amount we gather more
//			if(connectorcontroller.HasCannon())
//			{
//				GatherBalls(minimumDesiredAmount);
//			}
//			else{
//				currentState = ActionState.NeedCannon;
//				break;
//			}
//			
//			
//			
//			if(connectorcontroller.GetBallsInStorage() >= minimumDesiredAmount)// Frank Removed || !connectorcontroller.HasCannon())
//			{
//				currentState = ActionState.Attack;	
//				
//				if(PreparingEffect)
//					PreparingEffect.Stop();
//			}
//			
//			
//			
//			break;
//		}
//			
//		case ActionState.NeedCannon:
//		{
//			GatherCannons();
//			
//			if (connectorcontroller.HasCannon()){
//				currentState = ActionState.Prepare;
//			}
//			break;
//		}
//			
//		case ActionState.Destroyed:
//		{
//			Debug.LogError("Action State Destroyed");
//			ReleaseBalls();
//			break;
//		}
//		default:
//			
//			break;
//		}
//		
//		
//		
//		
//
//		 HandleBallGathering();	
//		 Franks add
//		 HandleCannonGathering();
//		
//		 Franks End
		HandleAim();
	
	}

	protected override void Reset ()
	{

		_listOfBalls.Clear();
		CurrentBallCount = 0;

		base.Reset ();
	}

	public bool PickUpBall(Ball ball){

		if (ball.IsActive())
			return false;

		if (currentBallCount >= this.MaxBallCount)
			return false;



		// deactivate the ball and add it to the inventory
		Ball b = ball;
		// set the balls source ID
		b.SetBallSourceID(Ball.BallSourceID.Netural);
		b.DeActivate();
		// add to my list of balls
		_listOfBalls.Add(b);

		currentBallCount++;

		return true;
	}

	public bool ReleaseBalls(){

		foreach (Ball b in _listOfBalls)
		{
			//Debug.LogError("**** Releasing Balls *****");
			//Activate, Set Position, Add Force.
			if (b != null){
				b.Activate();
				b.SetBallSourceID(Ball.BallSourceID.Hot);
				
				if (BallPopTransform)
					b.SetPosition(BallPopTransform.position);
				else
					b.SetPosition(myTransform.position);
				
				b.transform.LookAt(Vector3.forward);
				
				//Set the material
				if (BallPopMaterial)
					b.SetMaterial(BallPopMaterial);
				
				Vector3 explodeforce = Vector3.zero;
				
				explodeforce = Random.onUnitSphere;
				
				//Also call freeball to allow it back into the scene
				b.Fire(explodeforce);			
			}
		}
		
		currentBallCount = 0;
		
		//Play overheat sound
		//AudioPlayer.GetPlayer().PlaySound(OverHeatExplosionSound);
		
		//Clear the list.
		_listOfBalls.Clear();

		return true;
	}

	public Ball FindClosestBall(){
		Ball closestBall = null;

		closestBallDist = int.MaxValue;
		currentBallDist = int.MaxValue;

		// check teh eyes ball view, if the ball is active and chillin, we can target it
		foreach (Ball b in Eyes.BallView){
			// is the actual object active
			if (b.gameObject.activeInHierarchy){
				// active as in game terms, has it been fired by the bot
				// well active isn't working, trying now with source ID
				if (b.GetBallSourceID() == Ball.BallSourceID.Netural){
					currentBallVec = transform.position - b.transform.position;
					currentBallDist = currentBallVec.magnitude;
					
					if (currentBallDist < closestBallDist){
						closestBallDist = currentBallDist;
						closestBall = b;
						CurrentTargetedBall = b;
					}					
				}
			}
		}
		return closestBall;
	}

}
