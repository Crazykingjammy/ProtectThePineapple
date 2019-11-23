using UnityEngine;
using System.Collections;

public class FUIPushControlPanelButton : MonoBehaviour {

	public FUIActivity MyActivity;
	public ActivityManager.ManagerActivities ActivityType = ActivityManager.ManagerActivities.Options;
	
	public UISprite SelectedBG;

	// Use this for initialization
	void Start () {
		MyActivity = ActivityManager.Instance.Activities[(int)ActivityType];
	}
	
	// Update is called once per frame
	void Update () {
		if(MyActivity.Activated)
		{
			SelectedBG.alpha = 1.0f;
		}
		else{
			SelectedBG.alpha = 0.0f;
		}
	}

	protected void OnClick(){
		
		if (MyActivity != null){
			Debug.Log("*** Toggeling " + ActivityType + " ");
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
				ActivityManager.Instance.PushActivity(ActivityType);
				
				Debug.Log("Options Pushed");
			}
			
		}
		
		
		
	}
}
