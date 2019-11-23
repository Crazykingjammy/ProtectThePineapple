using UnityEngine;
using System.Collections;

public class FUIShieldButton : MonoBehaviour {
	
	bool isShielding = false;
	void OnPress(bool isDown){
		if (isDown)
		{
			isShielding = true;
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Tap);
		}
		else{
			isShielding = false;
			ToyBox.GetPandora().Bot_01.DeactivateDefense();
		}
	}

	void Update(){
		if (isShielding){
			ToyBox.GetPandora().Bot_01.ActivateDefense();	
		}
	}	
}
