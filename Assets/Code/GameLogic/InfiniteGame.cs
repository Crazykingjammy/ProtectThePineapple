using UnityEngine;
using System.Collections;

public class InfiniteGame : BaseGame {
	
	//Game play variables.
	public int Target;
	
	//Game states
	public BaseGameState TutorialState;
	public BaseGameState IntroState;
	

	
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
	

	}
	
	
	
	
	#region Events
		
	public override void CannonPickedUp()
	{
		//If we are at the intro stage, move on to the start of the game upon picking up the cannon.
	}
	
	
	#endregion
}
