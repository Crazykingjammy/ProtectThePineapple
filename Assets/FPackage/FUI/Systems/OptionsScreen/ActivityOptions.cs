using UnityEngine;
using System.Collections;

public class ActivityOptions : FUIActivity {
	
	
	//Slider access
	public UISlider MusicSlider;
	public UISlider SFXSlider;
	
	
	bool setup = false;
	
	// Use this for initialization
	new void Start () {
	
		base.Start();
		
	}
	
	public override void OnActivate ()
	{
		SetupOptions();

		ActivityManager.Instance.PullCurtain();
	}
	
	// Update is called once per frame
	void Update () {
		
	
	}
	
	void SetupOptions()
	{
		GameObjectTracker go = GameObjectTracker.instance;
		
		if(go == null)
			return;
		
		//Grab the slider values from player data and set it.
		MusicSlider.sliderValue = (go._PlayerData.Options.Music);
		SFXSlider.sliderValue = (go._PlayerData.Options.SoundFX);
		
		MusicSlider.ForceUpdate();
		SFXSlider.ForceUpdate();
		
		setup = true;
	}
		
	
	void OnMusicChange(float position)
	{
		//Debug.Log("MUSIC CHANGE!!!");
		
		GameObjectTracker.instance._PlayerData.Options.Music = position;
	}
	
	void OnSFXChange(float position)
	{
	//	Debug.Log("SFX Change CHANGE!!!");
		
		GameObjectTracker.instance._PlayerData.Options.SoundFX = position;
	}

	void OnTutorialStart()
	{
		//Set the tutorial to true, and restart...
		GameObjectTracker.instance._PlayerData.IsTutorial = true;
		GameObjectTracker.instance.RestartButtonPushed();
	}
	
	void OnDeleteDataOption(string option)
	{
		
		switch(option)
		{
			
		case "No":
		{
			//Do nothing. 
			break;
		}
		case "YES!":
		{
			GameObjectTracker.instance._PlayerData.DeleteAllData();
			break;
		}
		default:
		{
			//Do nothing.
			break;
		}
		
			
		}
		
		
	}
	
	void OnStatsView()
	{
		//Set the selected stats and push the activity.
		ActivityManager.Instance.SelectedStats = GameObjectTracker.instance.FullStatistics;
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Stats);
	}
	void OnUsePowerUp(bool check)
	{
		//ToyBox.GetPandora().CommandCenter_01.pow
		
		bool isActive = PowerupFactory.GetPUF().ToggleFactory();
		
		Debug.Log("Spawner is " + isActive);
	}
	
	void OnControls()
	{
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Controls,true);
	}

	void OnBack()
	{
		ActivityManager.Instance.PopActivity();
	}
}
