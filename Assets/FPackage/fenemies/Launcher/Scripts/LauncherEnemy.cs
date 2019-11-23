using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LauncherEnemy : AnimatingEnemy {

	float _launchClockTime = 0.0f;
	public float LaunchFreq = 3.0f;

	float _moveClockTime = 0.0f;
	public float MoveFreq = 1.0f;

	// a quick hack, when in move state, how long do we move before returning to idle
	// time in sec
	float _movingClockTime = 0.0f;
	public float MovingTimeInState = 4.0f;	
	public bool CanMove = false;



	// set in unity for now, it should be found on its own
	public FTargetSpawner MyTargetSpawner = null;
	public EntityFactory.TargetTypes type = EntityFactory.TargetTypes.StandardCrate;	// what should my target spawner spawn?
	// having control of the spawner, tell it to spawn during attack

	// do we have a seekerscript?
	protected SeekerScript seeker = null;
	protected bool isSeeker = false;

	// Use this for initialization
	protected override void Start () {
		// from Basic Enemy
		base.Start();

		objectAim = ToyBox.GetPandora().Bot_01.gameObject;


		// start the state machine timer
		_launchClockTime = Time.time;

		seeker = this.gameObject.GetComponent<SeekerScript>();
		if (seeker){
			// we have a seeker script attached.
			isSeeker = true;
		}



	}

	new public void OnEnable(){
		base.OnEnable();
		//currAnimState = StartingAnimState;
		_launchClockTime = Time.time;
		_moveClockTime = Time.time;
		_movingClockTime = Time.time;

		//Debug.LogWarning("ON Enable!!!! **********");
	}
	
	 new public void Update()
	{
//		Debug.LogError("Launcher Enemy Update" + this.name + " ** ");

		base.Update();

		// EnemyStates are for game behavior
		ProcessEnemyState();

		// animation processing happens in base animation class.



		// helper from BaseEnemy
		HandleAim();

	}

	new public void FixedUpdate(){
		base.FixedUpdate();
	}

	// the enemy states are working in tandem with the animation system.
	// as the animation states change so does the enemy state
	// defaults to idle states, where most logic can take place
	public void ProcessEnemyState(){

		// we need to make sure our spawner is still facing the right way
		MyTargetSpawner.transform.forward = this.myTransform.forward;

		// if ever destroyed
		if (this.IsDestroyed()){
			currAnimState = EnemyAnimStates.Implode;
		}

		switch(currAnimState){

			case EnemyAnimStates.Idle:{

			if (isSeeker){
				// in idle seeker script is off
				seeker.enabled = false;
			}

			// count the clock are we ready to shoot?
			float currTime = Time.time;
			if ((currTime - _launchClockTime) > LaunchFreq){
				currAnimState = EnemyAnimStates.Attack;
				_launchClockTime = Time.time;
				break;// we have just set a state, break;
			}

			if (CanMove){
				// count the clock are we ready to move?
				if ((currTime - _moveClockTime) > MoveFreq){
					currAnimState = EnemyAnimStates.MovingForward;
					_moveClockTime = currTime;
					_movingClockTime = Time.time;	// we have started to move
					break;// we have just set a state, break;
				}
			}

				break;
			}
				
			case EnemyAnimStates.Attack:{
				// the state is set to attack.  the animation is called to play the attack
				// when that animation is done, it makes the call to exit attack

				
				break;
			}
		case EnemyAnimStates.Implode:{
			GetComponent<Animation>().Stop();
			if (!IsDestroyed()){
				currAnimState = EnemyAnimStates.Idle;
			}
			break;
		}


		case EnemyAnimStates.MovingForward:{
			// when we enter here from idle, we start to move for as long as the 
			// moving time in state
			float currTime = Time.time;
			if ((currTime - _movingClockTime) < MovingTimeInState){
				// means we entered the moving forward state, reset the clock.
				// test the difference in time, if its less than the time in state
				// continue moving forward.  when moved enough, exit state to idle
				if (isSeeker){
					seeker.enabled = true;
				}
				else{
					// we're not a seeker, can't move.
					currAnimState = EnemyAnimStates.Idle;
				}
			}
			else{
				// we have moved enough.  go back ot idle
				currAnimState = EnemyAnimStates.Idle;
				// reset the clock
				_moveClockTime = Time.time;
			}
				break;
			}

		default:{
				break;
			}
		}
	}



	public void EnterAttack(){
		currAnimState = EnemyAnimStates.Attack;

	}

	public void ExitAttack(){
		// exit attack state and go back into idle
		currAnimState = EnemyAnimStates.Idle;
		// reset the attack clock
		_launchClockTime = Time.time;

	}

	public void LaunchTarget(){
		// this gets called during the attack animation of the launcher.  
		// when the attack animation ends it will tell this launcher to exit attack and enter idle


		// TODO add Target spawner calls here.  send a target to attack the CC or Bot
		//Debug.LogWarning("Launching TARGET!!!! **********");
		MyTargetSpawner.type = type;
		MyTargetSpawner.SpawnATarget();
	}
}
