using UnityEngine;
using System.Collections;

public class BaseGameState : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	virtual public void GameStateUpdate () {}
	

	
	virtual public void OnBotCannonPickUp(BaseGame game){}
	
	virtual public void OnTargetDestroyed(BaseGame game, Target t){}

}
