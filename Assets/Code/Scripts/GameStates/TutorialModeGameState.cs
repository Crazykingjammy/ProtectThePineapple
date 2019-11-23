using UnityEngine;
using System.Collections;

public class TutorialModeGameState : BaseGameState {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public override void GameStateUpdate () {
	
	}
	
	
	public override void OnBotCannonPickUp(BaseGame game)
	{
		print ("Cannon Pickup Message from Tutorial State");
	
		
	}
}
