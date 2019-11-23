using UnityEngine;
using System.Collections;

public class BallEaterBehavior : BaseEnemyBehaviorScript {

	protected bool isBallEater = false;
	protected BallEaterEnemy selfBallEater;

	bool foundBall = false;
	bool attacking = false;

	BaseEnemyDamageBehavior myDamageBehavior = null;

	// When Ball Eater Enters attacking state, whats the new Damage values?
	public float DefaultDamageToBot = 0.0f;
	public float DefaultDamageToCC = 0.0f;
	public float DefaultDamageToSelf = 0.0f;
	public float DefaultDamageToTarget = 0.0f;
	public float DefaultTargetMag = 9.0f;

	public float AttackingDamageToBot = 700.0f;
	public float AttackingDamageToCC = 400.0f;
	public float AttackingDamageToSelf = 1000.0f;
	public float AttackingDamageToTarget = 0.0f;
	public float AttackingTargetMag = 0.0f;

	// Use this for initialization
	new void Start () {
		base.Start();

		//Debug.LogError("start Ball eater Behavior");

		//Debug.LogError("Checking is Ball eater");

		selfBallEater = self.GetComponent<BallEaterEnemy>();

		if (selfBallEater != null){
			//Debug.LogError("is Ball eater");
			isBallEater = true;
			// we let the enemy know whos acting on it.
			selfBallEater.MainBehavior = this;			
		}

		if (!isBallEater || !isSelfAsEnemy)
			return;

			
			selfAsEnemy.CurrentState = BaseEnemy.ActionState.Prepare;
			selfAsEnemy.CurrentAim = BaseEnemy.EntityAim.Bot;



		attacking = false;

		if (selfBallEater.BodyToChangeMaterial != null){
			// at the start make sure we at the default material for the body
			selfBallEater.BodyToChangeMaterial.GetComponent<Renderer>().material = selfBallEater.DefaultMaterial;
		}

		// do I have a damage behavior attached?
		myDamageBehavior = this.gameObject.GetComponent<BaseEnemyDamageBehavior>();
		if (myDamageBehavior == null){
			Debug.LogError(" "+this.name +"Ball Eater Enemy Start - I dont have a damage behavior ");
		}
	}
	
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();

		// at start we made sure our self is a ball eater enemy
		if (!isBallEater || !isSelfAsEnemy){
			return;
		}

		if (selfBallEater.IsDestroyed()){
			selfBallEater.ReleaseBalls();
			selfBallEater.CurrentState = BaseEnemy.ActionState.Destroyed;
			attacking = false;
		}


			// if we have a cannon connector and can shoot, set the base enemy shooting aim
		if (selfAsEnemy.objectAim != null){
			selfAsEnemy.objectAim = selfAsEnemy.Eyes.TargetView[0].gameObject;
		}



		/*
		 * handling the finding with the ball tracker first, if no results then we use the eyes
		 * meh, not working yet
		 */

		//
		// if our ball eater is full, attack the bot or the CC
		if (selfBallEater.CurrentBallCount >= selfBallEater.MaxBallCount){
			selfAsEnemy.objectMoveAim = selfAsEnemy.Eyes.BotView.gameObject;
			attacking = true;		
		}

		if (!attacking){
			UpdateAimForBall();

			// we're not attacking, so make sure our default damage values are set
			myDamageBehavior.TouchDamageToBot = DefaultDamageToBot;
			myDamageBehavior.TouchDamageToCC = DefaultDamageToCC;
			myDamageBehavior.touchDamageToSelf = DefaultDamageToSelf;
			myDamageBehavior.HitTargetDamage = DefaultDamageToTarget;
			myDamageBehavior.HitTargetMagnitude = DefaultTargetMag;
		}
		else if (attacking){
			// we're attacking set the new damage values and set who we're attacking!
			myDamageBehavior.TouchDamageToBot = AttackingDamageToBot;
			myDamageBehavior.TouchDamageToCC = AttackingDamageToCC;
			myDamageBehavior.touchDamageToSelf = AttackingDamageToSelf;
			myDamageBehavior.HitTargetDamage = AttackingDamageToTarget;
			myDamageBehavior.HitTargetMagnitude = AttackingTargetMag;


				if (selfAsEnemy.Eyes.BotView.IsCannonAttached()){
					selfAsEnemy.objectMoveAim = selfAsEnemy.Eyes.BotView.gameObject;
				}
				else{
					selfAsEnemy.objectMoveAim = selfAsEnemy.Eyes.TargetView[0].gameObject;
				}

		}

		// if the ball we're targeting is inactive, find another one
		// there is a chance that there are no balls available.  then target the bot or the CC
		// make sure we have a targeted Ball
		// Fixed it, here was crashing when no balls were avaiable
		if (selfBallEater.CurrentTargetedBall){
			// TODO: if all the balls are inactive, then we need to target the bot or the CC
			if (!selfBallEater.CurrentTargetedBall.gameObject.activeInHierarchy){

				UpdateAimForBall();
			}
		}



		// set the material based on the state of attacking
		if (attacking){
			selfBallEater.BodyToChangeMaterial.GetComponent<Renderer>().material = selfBallEater.AttackingMaterial;
		}
		else{
			selfBallEater.BodyToChangeMaterial.GetComponent<Renderer>().material = selfBallEater.DefaultMaterial;
		}

		if (self.IsDestroyed()){
			selfBallEater.BodyToChangeMaterial.GetComponent<Renderer>().material = selfBallEater.DestroyedMaterial;
		}
	}

	void UpdateAimForBall(){
		// what we're currently targeting isn't active... find another
		Ball closestBall = null;
		// moved vars up top
		closestBall = selfBallEater.FindClosestBall();
		if (closestBall){
			if (isSelfAsEnemy){
				selfAsEnemy.objectMoveAim = closestBall.gameObject;
				selfBallEater.CurrentTargetedBall = closestBall;		
			}
		}

	}

	// instead of looking for a cannon to pickup, we're going to make sure our 
	// ball eater gets it
	new protected void OnCollisionEnter(Collision col)
	{
		base.OnCollisionEnter(col);
		
		
		// make sure we're still attached to a valid target object
		if (self == null || !isBallEater || !isSelfAsEnemy)
			return;
		
		/// we want to store it into our inventory
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();



			//Here we do a test to ONLY pick up active netural balls and nothing else.

			if(b.GetBallSourceID() == Ball.BallSourceID.Netural && !b.IsActive())
			{
				// no longer need to make sure we have a cannon.  ball eater can always pick up 
				// psydo code
				/**
				 * self Pickup Ball
				 * 	self deactivates ball 
				 *    adds to inventory, this is done through enemy interface
				 */
				selfBallEater.PickUpBall(b);
				//Set the ID to the ball
				// Setting the Source in the pickupball function.  weird behavior going on
				// ball not going back to neutral
				//b.SetBallSourceID(Ball.BallSourceID.Enemy);

				//Play the effect for picking up a ball.

					if(selfAsEnemy.Collection)
					{
						//Play the collection animation.
						selfAsEnemy.Collection.Play();
					}			
				

			}
			
		}//Ball Collision Check
		
		//		//Check if we collide with a cannon.
		//		if(col.gameObject.CompareTag("Cannon") && !self.IsDestroyed())
		//		{
		//			Cannon c = col.gameObject.GetComponent<Cannon>();
		//			
		//			//Call attach for the cannon that we collide with.
		//			if(self.connectorcontroller.AttachCannon(c))
		//			{
		//				self.cooldownTimer = self.CoolDownTime - 0.5f; 
		//				
		//				self.CurrentState = BaseEnemy.ActionState.CoolDown;
		//			}
		//			
		//			
		//		}
	}

	/*
	 * I want to have it so that there is a collider larger than the enemy to see nearby balls
	 */
//	void OnColliderStay(Collider col){
//		// make sure we're still attached to a valid target object
//		if (self == null)
//			return;
//		
//		//		BallEaterEnemy ballEater = self.GetComponent<BallEaterEnemy>();
//		//
//		//		if (ballEater == null)
//		//			return;
//		
//		// we know we're a ball eater, see a ball, set the move aim
//		
//		/// we want to store it into our inventory
//		if(col.gameObject.CompareTag("Ball"))
//		{
//			//Get the ball we collided with.
//			Ball b = col.gameObject.GetComponent<Ball>();
//			
//			//Here we do a test to ONLY pick up active netural balls and nothing else.
//			if(b.GetBallSourceID() == Ball.BallSourceID.Netural)
//			{
//				self.objectMoveAim = b.gameObject;
//				//ballsNearBy = true;
//			}
//			
//		}
//	}
}
