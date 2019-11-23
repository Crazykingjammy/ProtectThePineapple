using UnityEngine;
using System.Collections;

public class ContolPanelSocialMenu : MonoBehaviour {
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnButtonGameCenter()
	{
		GameObjectTracker.instance.GameCenterButtonPushed();
	}
	
	void OnButtonFullStats()
	{
//		//Set the selected stats and push the activity.
//		ActivityManager.Instance.SelectedStats = GameObjectTracker.instance.FullStatistics;
//		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Stats);
	}
	
	void OnCamera()
	{
//		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.PCamera,true);
//		ActivityManager.Instance.DrawCurtain();
	}
	
}
