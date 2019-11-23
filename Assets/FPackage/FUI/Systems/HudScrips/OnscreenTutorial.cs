using UnityEngine;
using System.Collections;

/// <summary>
/// Onscreen tutorial.
/// Used to manage the onscreen HUD pops to aid the player on the controls
/// 
/// The way its going to work is through a series of popups, guiding the player.
/// 
/// First a popup for the joystick
/// When we see the player can move, popup and make the shield button call to action.
/// When player has enough bullets, popup and make the shoot button call to action
/// Then popup kill action on a visible enemy.
/// Finally, popup a message to say "Kill quickly, 3 seconds keeps your combo"  and shows the combo counter
/// </summary>

public class OnscreenTutorial : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
