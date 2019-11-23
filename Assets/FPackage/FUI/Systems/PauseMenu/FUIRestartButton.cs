using UnityEngine;
using System.Collections;

public class FUIRestartButton : MonoBehaviour {

	// When this object is clicked we restart the run
	void OnClick(){
		
		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		
		GameObjectTracker.instance.RestartButtonPushed();
	}
}
