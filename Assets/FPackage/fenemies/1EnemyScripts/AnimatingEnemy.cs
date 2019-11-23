using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatingEnemy : BaseEnemy {

	public enum EnemyAnimStates {
		Idle = 0,
		Attack,
		MovingForward,
		Implode,
		Taunt, 
		Pushed
	}

	// flag to disable animation on the object
	public bool CanAnimate = true;
	protected bool _canAnimate = true;

	// we can set starting in unity
	public EnemyAnimStates StartingAnimState = EnemyAnimStates.Idle;
	// managed protected
	protected EnemyAnimStates currAnimState = EnemyAnimStates.Idle;

	
	
	// the corresponding EnemyAnimState index is the index of the string.  these must match
	// Idle as index 0 = EnemyAnimStateString[0] ("Idle" Animation)
	public List<string> EnemyAnimStateStrings;

	public float PushedVelocityEnter = 12.0f;
	public float PushedVelocityExit = 3.0f;
	

	// Use this for initialization
	protected override void Start () {
		// from Basic Enemy
		base.Start();

		_canAnimate = CanAnimate;

		currAnimState = StartingAnimState;



	}
	
//	// Update is called once per frame
//	void Update () {
//		//base.FixedUpdate();
//
//	}

	new public void Update(){

		base.Update();

		// using enemystates as animation states we can link animations to toggle states
		if (_canAnimate){
			HandleAnimation();
		}		
	}

	new public void FixedUpdate(){

		base.FixedUpdate();

	}

	 public void OnEnable(){
		currAnimState = StartingAnimState;
	}

	public void HandleAnimation(){

		//Debug.LogWarning("Handling Animation********");

		if (!IsDestroyed()){
			if (currAnimState != EnemyAnimStates.Implode){
				if (EnemyAnimStateStrings[(int)currAnimState] != null)
				{
					string fname = EnemyAnimStateStrings[(int)currAnimState];
//					Debug.LogError("ANIM PLAY: " + fname);
					GetComponent<Animation>().CrossFade(fname);


				}
			}
			
		}
		else{
			// we're destroyed
			GetComponent<Animation>().Stop();

		}
	}
}
