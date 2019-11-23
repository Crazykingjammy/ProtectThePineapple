using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityPause : FUIActivity {
	
	public FUIStatsTable Table;
	
	public UILabel Score;
	public UILabel gameTime;
	
	public UILabel gameBank;
	public UILabel PhasePhrase;
	
	public UIGrid PhaseGrid;
	public FUIPhaseResultObject PrefabBadge;

	public ProgressWindow BreakProgressWindow;

	List<FUIPhaseResultObject> _resultList = null;
	bool prepredPhaseGrid = false;
	int initialCount = 10;
	
	
	public LeaderboardWindow LWindow; 
	
	// Use this for initialization
	new void Start () {
	
		base.Start();
		
		
		
	}
	

	// Update is called once per frame
	void Update () {
	
		if(!prepredPhaseGrid)
		{
			PreparePhaseGrid();
			return;
		}
			
		
		//Since we are grabbing from the GOT lets quit if we dont ahve one
		if(GameObjectTracker.instance == null)
			return;
		
		string format = string.Format("{0:#,#,#}", GameObjectTracker.instance.GetCurrentPoints());
		Score.text = format;
		
		format = FormatSeconds(GameObjectTracker.instance.GameTime);
		
		if(GameObjectTracker.instance.GameTime== -1.0f)
			format = "--:--";
		
		gameTime.text = format;
		
		
		float gemcount = (float)GameObjectTracker.instance.GetGemsCollected()/100.0f;
		format = string.Format("{0:C}",gemcount);
		gameBank.text = format;
		
		//Always update the phase results?
		int index = 0;
		foreach(BasePhase.PhaseData pdata in GameObjectTracker.instance._PlayerData.Breathless.PhaseList)
		{
			//If we are not already enabled.
			if(!_resultList[index].gameObject.activeSelf)
			{
				//Activate, set data and reposition.
				_resultList[index].gameObject.SetActive(true);
				_resultList[index].PhaseData = pdata;
				
				//Set the Phase Phrase
				PhasePhrase.text = pdata.Description;
				
				PhaseGrid.Reposition();
			}
			
			index++;
		}
		
		
		
			
	}
	

	void OnOptionsButton()
	{
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Options,true);
	}

	void OnInventoryButton()
	{
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Inventory,true);
	}

	// When this object is clicked we restart the run
	void OnRetryButton(string option){
		
	
			
		switch(option)
		{
			
		case "No":
		{
			//Do nothing. 
			break;
		}
		case "YES!":
		{
			//GameObjectTracker.instance._PlayerData.DeleteAllData();
			//Play the sound
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
			
			GameObjectTracker.instance.RestartButtonPushed();
			break;
		}
		default:
		{
			//Do nothing.
			break;
		}
			
			
		}

	}
		


	void OnCameraButton()
	{
		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.PCamera,true);
		ActivityManager.Instance.DrawCurtain();
	}

	string FormatSeconds(float elapsed)
	{
	   int d = (int)(elapsed * 100.0f);
	   int minutes = d / (60 * 100);
	   int seconds = (d % (60 * 100)) / 100;
	   int hundredths = d % 100;
	   return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
	}
	
	void PreparePhaseGrid()
	{
		if(_resultList == null)
		{
			_resultList = new List<FUIPhaseResultObject>();
			return;
		}
		
		for(int i = 0; i < initialCount; i++)
		{
			//Create teh new result.
			FUIPhaseResultObject newResult = NGUITools.AddChild(PhaseGrid.gameObject,
				PrefabBadge.gameObject).GetComponent<FUIPhaseResultObject>();
			
			//Nothing is yet assigned. 
			
			//Add to list and deactivate.
			newResult.gameObject.SetActive(false);
			_resultList.Add(newResult);
		}
		
		prepredPhaseGrid = true;
		
		
	}
	
	void ResetPhaseGrid()
	{
		
		if(_resultList == null)
			return;
		
		//Go through and deactivate the badge objects.
		foreach(FUIPhaseResultObject pResult in _resultList)
		{
			pResult.gameObject.SetActive(false);
		}
		
	}
	
	public override void OnActivate ()
	{
		Table.statData = GameObjectTracker.instance.RunStatistics;

		//Pull curtain upon activate.
		ActivityManager.Instance.PullCurtain();

		//Activate the progress window.
		BreakProgressWindow.Data = GameObjectTracker.instance._PlayerData.MyPTPLevel;
		BreakProgressWindow.TriggerProgressAnimation(true);

		//Activate the leaderboard as well
		LWindow.Acivate();
	}
	
	public override void OnDeActivate ()
	{
		//base.OnDeActivate ();
		ResetPhaseGrid();

//		Debug.LogError("Pause deactivated...curtain drawn....right?");

		//Pull curtain
		ActivityManager.Instance.DrawCurtain();
	}
}
