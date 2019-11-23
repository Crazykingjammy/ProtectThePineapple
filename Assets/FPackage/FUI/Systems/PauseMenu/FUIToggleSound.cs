using UnityEngine;
using System.Collections;

public class FUIToggleSound : MonoBehaviour {
	
	public FUIActivity MyActivity;
	public UISprite SelectedBG;
	
	void Update()
	{
//		if(MyActivity.Activated)
//			SelectedBG.alpha = 1.0f;
//		else
//			SelectedBG.alpha = 0.0f;
			
	}
	
	void OnClick(){
		
	//	GameObjectTracker.GetGOT()._PlayerData.Options.MusicLevel = 0;
	//	GameObjectTracker.GetGOT()._PlayerData.Options.SoundFXLevel = 0;
		
		//GameObjectTracker.instance.GameCenterButtonPushed();
		
		if (MyActivity != null){
			Debug.Log("*** Toggeling Inventory Screen! ** ");
			// is inventory open always starts false; 
			// the first time will make it true, then activates the game object
			//isInventoryOpen = !isInventoryOpen;
			//NGUITools.SetActive(InvScreen.gameObject, isInventoryOpen);
			
			if(MyActivity.Activated)
			{
				ActivityManager.Instance.PopActivity();
				
				Debug.Log("Options Popped");
			}
			else
			{
				ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Pause);
				
				Debug.Log("Options Pushed");
			}
			
		}
		
	}
}
