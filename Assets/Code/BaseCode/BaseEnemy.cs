using UnityEngine;
using System.Collections;

public class BaseEnemy : Target {
	
	//Cannon Connector to Cannon
	protected CannonConnector connectorcontroller;
	public CannonConnector ConnectController{
		get{ return connectorcontroller; }
		set { connectorcontroller = value;}
	}

	public ParticleSystem Collection;
	public ParticleSystem PreparingEffect;
	
	public SmartBeam Eyes;
	protected BlackHole Hands;
	
	protected bool gathering = false;
	int commandedgather = 0;

	// ** Franks Add 
	//
	protected bool gatheringCannon = false;
	public float gatherCannonFactor = 300;
	// Franks End
	
	//Timer for keeping track of shooting frequency.
	float shoottimer = 0.0f;
	
	//Variables for Behavioras
	public float AttackTimeFrequency = 3.0f;
	public int AverageBallsToGather = 5;
	public bool AttackBots = true;
	public bool AttackCommandCenters = true; 
	public bool PerformAimForBody = true;
	
	public bool ForbiddenWeapons = true;
	
	
	
	//Id
	public int timestamp = 0;
	
	CannonSpawner mySpawner = null;
	

	// ** Franks Add, changed from protected to public.. testing..
	public int minimumDesiredAmount = 2;
	
	//Object to aim at.
	public GameObject objectAim = null;
	/*
	 * Franks Add 
	 * Adding support for aiming at an object to shoot and aim for a direction of movement
	 * the current "objectAim" will be reserved for Object Shooting Aim
	 * 
	 * GameObject "objectMoveAim" is used when needing to move and rotate to the destination while
	 * shooting at a different object.  will come in handy for enemy way points and continue to shoot 
	 * at the Bot or CC
	 * 
	 * Also can have parasites aiming to touch and blowup the bot while shooting down the CC
	 */
	public GameObject objectMoveAim = null;

	// From Basic Enemy, figured why not all BaseEnemies have a cool down stat
	public float CoolDownTime = 5.0f;
	public float cooldownTimer = 0.0f;  // changed from private to protected, then to public for children and script access




	// ** Franks End Add *****
	
	//Local caching
	//Transform myTransform;
	
	//State Information
	public enum ActionState
	{
		Reset,
		Attack,
		CoolDown,
		Prepare,
		NeedCannon,
		Destroyed, 
		Pushed
	}
	
	public enum EntityAim
	{
		Bot,
		CommandCenter,
		Target,
		NULL
	}
	
	EntityAim currentAim = EntityAim.Bot;
	
		public EntityAim CurrentAim{
		get { return currentAim; } 	
		
		set{
			currentAim = value;

			//Perform switch statement to set the aim at the game object.
			switch(currentAim)
			{
			case EntityAim.Bot:
			{
				if(Eyes.BotView)
				objectAim = Eyes.BotView.gameObject;

				break;
			}
				
			case EntityAim.CommandCenter:
			{
				//Assign to the first target if there is one.
				if(Eyes.TargetView.Count > 0)
					objectAim = Eyes.TargetView[0].gameObject;
				break;
			}
			// *** Franks Add - need to support NULL 12/28/13

			case EntityAim.NULL:
			{
				objectAim = Eyes.BotView.gameObject;
				break;
			}
			// *** Franks END //
			default:
			{
				objectAim = Eyes.BotView.gameObject;
				break;
			}
			
			}
			
		}}
	
	
	protected ActionState currentState = ActionState.Prepare;
		public ActionState CurrentState{
		get { return currentState; } 	
		set { currentState = value; }	// Franks Add for behavior script
	}
	
	// Use this for initialization
	protected override void Start () {
		
		base.Start();
		
		Initialize();
		
	}
	
	protected override void Reset()
	{
		base.Reset();
		
		//Get rid of the cannon.
		if(mySpawner)
		{
			mySpawner.KillSpawn();
		}

		// ** Frank Add.
		// when we reset, the enemy is ready in the pool.
		// reset the state, away from destroyed
		currentState = ActionState.Reset;
		currentAim = EntityAim.Bot;

		// Frank End
		
	}

	void OnEnable()
	{
	//	Debug.LogError("Enemy enable");

		if(AttackCommandCenters)
			CurrentAim = EntityAim.CommandCenter;

		if(AttackBots)
			CurrentAim = EntityAim.Bot;

		currentState = ActionState.Prepare;
		//currentAim = EntityAim.Bot;
	}
	
	protected void Initialize()
	{
		if(Collection)
		{
			//Create teh collection Particle Effect
			Collection = Instantiate(Collection) as ParticleSystem;
		
		
			Collection.transform.position = transform.position;
			Collection.transform.parent = transform;
		}

		if(PreparingEffect)
		{
			//Create the effect
			PreparingEffect = Instantiate(PreparingEffect) as ParticleSystem;

			PreparingEffect.transform.parent = transform;
			PreparingEffect.transform.localPosition = Vector3.zero;
		}

		//Get the connect Controller.
		connectorcontroller = GetComponentInChildren<CannonConnector>();
		if (connectorcontroller == null){
			Debug.LogError("can't find cannon connector!!!!!!");
		}
		connectorcontroller.SetHost(gameObject);
		
		//All bad guys have hot cannons so nobody else picks up their cannons after they drop it. 
		connectorcontroller.SetHotCannon(ForbiddenWeapons);
		
		
		//Get the smart beam		
		Eyes = GameObjectTracker.GetGOT().World._objectView;
		
		Hands = GetComponent<BlackHole>();
		
		if(!Hands)
			Debug.Log("No Black Hole created for enemy!");
		
		//Set the ID
		timestamp = (int)Time.time;
		
		if(!Eyes)
		{
			Debug.LogError("No Smartbeam found on enemy! Please make sure to attach Smart Beam!");
		}
		
		CannonSpawner c = gameObject.GetComponentInChildren<CannonSpawner>();
		
		//Lets look for a spawner.
		if(c)
		{		
			mySpawner = c;
		}
		
		myTransform = transform;
		
	}

	new public void Update(){
		//Call Enemies Update here.

		base.Update();

	}
	public virtual void FixedUpdate()
	{

	}
	
	#region Action Functions
	
	public void Shoot()
	{
		if(!connectorcontroller.HasCannon())
		{
			return;
		}
		
		if(!WithinShootingTimer)
		{
			return;
		}
		
		//Whenever Function is called we shoot.
		connectorcontroller.GetCannon().Shoot();
		ResetShootTimer();
	}
	
	
	void ResetShootTimer()
	{
		shoottimer = Time.time;
	}
	
	bool WithinShootingTimer
	{
		get
		{
			if( (Time.time - shoottimer) > AttackTimeFrequency)
			{
				return true;
			}
			
			return false;	
		}
	}

	/*
	 * Franks Add 
	 * ** This is also writen above **
	 * Adding support for aiming at an object to shoot and aim for a direction of movement
	 * the current "objectAim" will be reserved for Object Shooting Aim
	 * 
	 * GameObject "objectMoveAim" is used when needing to move and rotate to the destination while
	 * shooting at a different object.  will come in handy for enemy way points and continue to shoot 
	 * at the Bot or CC
	 * 
	 * Also can have parasites aiming to touch and blowup the bot while shooting down the CC
	 */
	public void HandleAim()
	{
		
		//Obtain 
		if(objectAim)
		{
			if(PerformAimForBody && !IsDestroyed())
			{
				// ** Added if we dont have a move aim (old enemies shouldn't)
				// continue to do the same as usual
				if (objectMoveAim == null){
					myTransform.LookAt(objectAim.transform.position);
					
					//Negate the forward because of bad assets.
					//myTransform.forward = -myTransform.forward;
				}
				else{
					// we are given a move aim, look at it!
					myTransform.LookAt(objectMoveAim.transform.position);
					
					//Negate the forward because of bad assets.
					//myTransform.forward = -myTransform.forward;
				}
			}
			
			connectorcontroller.AimAt(objectAim);	
		}
		// ** Added; if we dont have a shooting aim but have a moveaim, update the move aim
		if (objectMoveAim && objectAim == null){

			//If we have an object we are supposed to aim at we handle it and return.
			if(AttackCommandCenters)
			{
				CurrentAim = EntityAim.CommandCenter;
				//return;
			}
			if(AttackBots)
			{
				CurrentAim = EntityAim.Bot;
				//return;
			}




			// we are given a move aim, look at it!
			myTransform.LookAt(objectMoveAim.transform.position);
			
			//Negate the forward because of bad assets.
			myTransform.forward = -myTransform.forward;

			// no need to update the cannon aim
		}
		
		
		
//		switch(currentAim)
//		{
//		case EntityAim.Bot:
//		{
//			
//			//Break if there is no bot to view.
//			if(!Eyes.BotView)
//				break;
//			
//			//If we are to aim with our bodies.
//			if(PerformAimForBody && !IsDestroyed())
//			{
//				myTransform.LookAt(Eyes.BotView.transform.position);
//				
//				//Negate the forward because of bad assets.
//				myTransform.forward = -myTransform.forward;
//			}
//			
//			connectorcontroller.AimAt(Eyes.BotView.gameObject);
//			
//			break;
//		}
//		case EntityAim.CommandCenter:
//		{
//			if(Eyes.TargetView.Count > 0)
//			{
//				if(Eyes.TargetView[0])
//				{
//					objectAim = Eyes.TargetView[0].gameObject;
//					
//					if(PerformAimForBody && !IsDestroyed())
//					{
//						myTransform.LookAt(objectAim.transform.position);
//						
//						//Negate the forward because of bad assets.
//						myTransform.forward = -myTransform.forward;
//					}
//					
//					connectorcontroller.AimAt(objectAim);	
//				
//				}
//				
//			}
//			
//			break;
//		}
//		default:
//		{
//			break;
//		}
//		}
//		
		//Aim at the first target in the list we see, if there is a target in the list.

	}
	
	
	protected void  HandleBallGathering()
	{
		//Handle the gathering of balls update.
		if(gathering)
		{
			
		Hands.Active = true;
			
			foreach(Ball b in Eyes.BallView)
			{
				if(!b.IsActive())
				Hands.PullToCenter(b.gameObject);
			}
			
			if(connectorcontroller.GetBallsInStorage() >= commandedgather)
			{
				gathering = false;
			}
		}
		else{
			Hands.Active = false;
		}
	}
	
	public void GatherBalls(int count)
	{
		//We set the commanded gather
		commandedgather = count;
		
		//We turn on the gathering mode if this function is called and we have less balls than commanded to gather
		if(connectorcontroller.GetBallsInStorage() < count)
		{
			gathering = true;
		}
		
	}

	///** Franks Ad
	/// 
	public void GatherCannons(){
		// if we already have one, dont bother
		if (connectorcontroller.HasCannon()){
			//gatheringCannon = false;
			return;
		}

		gatheringCannon = true;
	}


	protected void HandleCannonGathering()
	{
		// pull in cannons from what the eyes can see

		if(gatheringCannon)
		{
			
			Hands.Active = true;
			
			foreach(Cannon c in Eyes.CannonView)
			{
				Hands.PullToCenter(c.gameObject, gatherCannonFactor);
			}
			
			if(connectorcontroller.HasCannon())
			{
				gatheringCannon = false;
			}
		}
		else{
			Hands.Active = false;
		}
	}
	/// ** END
	
	#endregion
	
}
