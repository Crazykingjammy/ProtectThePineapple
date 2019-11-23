using UnityEngine;
using System.Collections;

public class FUIPushAllStats : FUIPushControlPanelButton {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){

		//Set the selected stats and push the activity.
		ActivityManager.Instance.SelectedStats = GameObjectTracker.instance.FullStatistics;

		base.OnClick();


	}
}
