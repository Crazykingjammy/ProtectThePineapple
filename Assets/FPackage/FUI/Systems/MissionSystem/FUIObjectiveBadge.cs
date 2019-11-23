using UnityEngine;
using System.Collections;

public class FUIObjectiveBadge : MonoBehaviour {
	
	public UISprite ObjectiveIcon;
	public UILabel ObjectiveLabel;
	public UISprite ObjectiveCompletionIcon;
	
	public Color UnFinishedColor;
	
	//Local variables needed to store to display an objectiev struct.
	BaseMission.GameObjective myObjective;
	
	public BaseMission.GameObjective AssignedObjective
	{
		get{
			return myObjective;
		}
		set{
			myObjective = value;
			
			
			//Set the variables
			ObjectiveIcon.spriteName = myObjective.IconName;
			ObjectiveLabel.text = myObjective.ObjectiveLabel;
			ObjectiveCompletionIcon.enabled = myObjective.isCompleted;
			
			if(!myObjective.isCompleted)
				ObjectiveIcon.color = UnFinishedColor;
			else
				ObjectiveIcon.color = Color.white;
			
		}
	}
	
//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
//	
	
	void OnClick()
	{
		print("Objective Pushed: " + myObjective.ObjectiveLabel);
		
		//set the selected objective.
		ActivityManager.Instance.SelectedObjective = myObjective;
		
		//Push the activity.
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Objective,true);
		
		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
	}
	
}
