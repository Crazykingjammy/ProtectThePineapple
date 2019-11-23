using UnityEngine;
using System.Collections;

public class FUIShootButton : MonoBehaviour {
	
//	void OnClick(){
//		
//		ToyBox.GetPandora().Bot_01.Shoot();	
//	}

	public UILabel LabelAmmoCount;

	public Transform moveAmmoLabelTransform;

	void Start(){

	}

	void OnPress(bool isDown)
	{	
		if (isDown)
		{
			ToyBox.GetPandora().Bot_01.Shoot();	
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Tap);

			LabelAmmoCount.transform.position = moveAmmoLabelTransform.position;

		}
		else{
			LabelAmmoCount.transform.position = this.transform.position;
		}
		
	}
	
}
