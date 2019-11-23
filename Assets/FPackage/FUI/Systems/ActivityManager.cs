using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActivityManager : MonoBehaviour {
	
	//Self/
	static private ActivityManager _self = null;
	
	
	public enum ManagerActivities
	{
		Inventory,
		Missions,
		Pause,
		Options,
		Objective,
		GameOver,
		Stats,
		Controls,
		PCamera,
		LevelUp,
		Begin,
		Count,
	}
	
	//List of activities to be included.
	public FUIEnablerScript HUD;
	public FUIPauseScript pause = null;
	public GameObject Panel;
	public FUIActivity[] Activities;
	public ManagerActivities TypesReference = ManagerActivities.Controls;
	public GameObject RootAnchor;

	
	
	public TweenScale CurtainLeft, CurtainRight;
	public TweenTransform TitleAnimation;
	bool curtainsDrawn = false;

	public TweenAlpha BlackBGAlpha;
	public UISprite BlackBG; 

	public FUIBossNotification BossNotif;
	
	//Stack info.
	Stack _activityQue;
	
	// this is the list contained internally for all the selectable cannons
	List<CannonSelectionObject> _CardSelectors = null;

	
	
	//Local scratch cache info.
	//Pointer to current selected card.
	BaseItemCard _selectedCard = null;
	BaseItemCard _highlightedCard = null;
	BaseMission.GameObjective _selectedObjective = null;
	Statistics _selectedStatistics = null;
	
	bool pushedAudio = false;
	
	//Controls manager access.
	ControlsManager _myControlsManager = null;

	public void FadeToBlack()
	{
		BlackBG.gameObject.SetActive(true);

		BlackBGAlpha.Reset();
		BlackBGAlpha.Play(true);
	}


	public bool SetActive
	{
		get{return gameObject.activeSelf;}
		set { gameObject.SetActive(value);}
	}

	public ControlsManager HUDController
	{
		get {
			return _myControlsManager;
		}
		set
		{
			_myControlsManager = value;
		}
	}
	

		
	//Our singleton access.
	static public ActivityManager Instance
	{
		get
		{
			if(_self != null)
				return _self;
			
//			Debug.LogError("Activity Manager Nought created!!!!!");
			return null;		
		}
	}
	
	public List<CannonSelectionObject> CardSelectors
	{
		get
		{
			return _CardSelectors;
		}
		
		set
		{
			_CardSelectors = value;
		}
	}

	public BaseItemCard HighlitedCard
	{
		set{
			_highlightedCard = value;
		}
		get{
			return _highlightedCard;
		}
	}


	public BaseItemCard SelectedCard
	{
		get{
			
			if(_selectedCard != null)
				return _selectedCard;
			
			//If we dont have a selected card we just return the first card in the deck.
			return GameObjectTracker.instance._PlayerData.CardDeck[0];
		}
		
		set{

			_selectedCard = value;
		}
	}
	
	public Statistics SelectedStats
	{
		get{
			
			if(_selectedStatistics != null)
				return _selectedStatistics;
			
			//If we dont have a selected card we just return the first card in the deck.
			return GameObjectTracker.instance.FullStatistics;
		}
		
		set{
			_selectedStatistics = value;
		}
	}
	
	
	
	public BaseMission.GameObjective SelectedObjective
	{
		get{
			
			if(_selectedObjective != null)
				return _selectedObjective;
			
			//Return null for now if there is no selected objective. It would be nice to return a default asset.
			return null;
		}
		
		set{
			_selectedObjective = value;
		}
	}
	
	//Pushes new activity on top of current one.
	public void PushActivity(ManagerActivities newAcitivty, bool HidePanel = false)
	{
		//Hack to hide the panel if requested.
		if(HidePanel)
			Panel.SetActive(false);

		//New activity is indexed and grabbed.
		FUIActivity activty = Activities[ (int) newAcitivty ];
	
		//Grab the current activity;
		FUIActivity curActivity = currentAcitivty;
		
		//Deactivitate current activity.
		if(curActivity != null)
		{
			curActivity.ActivityOff();
			//_activityQue.Pop();
		}
	
		
		//Activate the new activity.
		activty.ActivityOn();
		
		//push the new activity. 
		_activityQue.Push(activty);
		
		//Push the audio of the activitiy
		PushAudio();
		
		//Play audio sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Open);
		
	}
	
	//Pops the current activity and returns it. 
	public FUIActivity PopActivity(bool forceShow = false)
	{
		//Little hack to hide panel.
		if(!Panel.activeSelf && forceShow)
			Panel.SetActive(true);
		
		if(_activityQue.Count == 0)
			return null;
		
		//Grab the acivity at the top.
		FUIActivity curActivity = _activityQue.Pop() as FUIActivity;
	
		//We didnt have any activities to pop so we return null.
		if(curActivity == null)
			return null;
		
	
		//deactivate the current activity and activate the new one.
		curActivity.ActivityOff();
		
		//Grab the now current activity now that this one has been popped..
		FUIActivity returnedActivity = currentAcitivty;
		
		//If we have one, then we activate.
		if(returnedActivity != null)
			returnedActivity.ActivityOn();
		
		//Play audio sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Close);
		
		return curActivity;
	}
	
	//Accessor for current acitivty.
	public FUIActivity currentAcitivty
	{
		get{
			
			//If there is nothing in the queue we return null.
			if(_activityQue.Count == 0)
				return null;
			
			return _activityQue.Peek() as FUIActivity;
		}
	}
	
	public void ClearActivities()
	{
		Debug.Log("Activities Cleared");

		//Little hack to hide panel.
		if(!Panel.activeSelf)
			Panel.SetActive(true);
		
		foreach(FUIActivity faktiv in Activities)
		{
			faktiv.ActivityOff();
		}
		
		//Clear the list.
		_activityQue.Clear();
		
		//Only pop on clear.
		PopAudio();

		//Draw the curtains upon clearing.
		//DrawCurtain();
		
		//Play audio sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Unpause);
	}

	public void TriggerTitleAnimation()
	{

		//Enable the object and play the animation.
		TitleAnimation.gameObject.SetActive(true);


		CurtainLeft.Reset();
		CurtainRight.Reset();
		curtainsDrawn = false;

		TitleAnimation.Reset();
		TitleAnimation.Play(true);

		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotPushed);
	}
	
	public void PushAudio()
	{
		 if(currentAcitivty != null)
		{
			if(currentAcitivty.ActivityTune != null)
			{
				Debug.Log("Pushed Audio Track " + currentAcitivty.ActivityTune.name);
				
				//Returns false if track is already playing so only play xip if we change track.
				if(AudioPlayer.GetPlayer().PushTrack(currentAcitivty.ActivityTune))
					AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.XIP);
				
				pushedAudio = true;
			}
		}
		
		
		
	}
	
	public void PopAudio()
	{
		if(pushedAudio)
		{
			AudioPlayer.GetPlayer().PopTrack();
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.UnXip);
			pushedAudio = false;
		}

	}
	
	public void ToggleHud()
	{
				
		HUD.TogglePanel();
		
	}
	
	public void ToggleHud(bool force)
	{
				
		HUD.TogglePanel(force);
		
	}


	//Awake funtion to assign self to.
	void Awake()
	{
		//Assign Self.
		_self = this;

	}
	
	// Use this for initialization
	void Start () {
	
		//Assign Self.
		_self = this;
		
		//What happens if we dont deleto?
		pause = HUD.GetComponent<FUIPauseScript>();
		
		//Get root
		//mRoot = GetComponent<UIRoot>();
		
		//Check resolution
		
		
		_activityQue = new Stack();
		
		//DrawCurtain();

		//Populate activity list
//		int i = 0;
//		foreach(FUIActivity fak in Activities)
//		{
//			Activities[i] = Instantiate(Activities[i]) as FUIActivity;
//
//			Activities[i].transform.parent = RootAnchor.transform;
//			i++;
//		}
//		


		//Enable the curtains
		CurtainLeft.gameObject.SetActive(true);
		CurtainRight.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
//		if(Input.GetKeyDown(KeyCode.G))
//		{
//			PushActivity(ManagerActivities.GameOver);
//		}

//		// if we do not have an activity active, force the free camera activity
//		bool haveAtLeastOneActivityOn = false;
//		for (int i = 0; i < 9; i++){
//			if (Activities[i].Activated)
//			{
//				haveAtLeastOneActivityOn = true;
//				break;
//			}
//		}
//
//		if (!haveAtLeastOneActivityOn && !HUD.Active ){
//			// we have no activities active, call free camera
//			ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.PCamera,true);
//			ActivityManager.Instance.DrawCurtain();
//		}
//		
			
	}
	
	
	public void PullCurtain()
	{
		if(!curtainsDrawn)
			return;

	//	Debug.LogError("Curtain Pull");
		
		CurtainLeft.Play(false);
		CurtainRight.Play(false);
		
		curtainsDrawn = false;

		//Disable the title on curtain pull.
		TitleAnimation.gameObject.SetActive(false);
	}

	public bool IsCurtainOpen
	{
		get{return curtainsDrawn;}
	}
	
	public void DrawCurtain(bool forceClosed = false)
	{

//		Debug.LogError("Curtain Drawn");

		if(curtainsDrawn)
			return;

		if(forceClosed)
		{
			CurtainLeft.Reset();
			CurtainRight.Reset();
		}


		//TriggerTitleAnimation();
		//return;

		//CurtainLeft.Reset();
		CurtainLeft.Play(true);
		
		//CurtainRight.Reset();
		CurtainRight.Play(true);
		
		curtainsDrawn = true;
		
		
	}

	//Called when fade to black is called.
	//Closes curtains when screen fade.
	void OnFadeToBlack()
	{
		CurtainLeft.Reset();
		CurtainRight.Reset();
	}

	//When title animation finishes play we call the curtain open.
	void TitleAnimationEnd()
	{

		//Draw curtain upon animation end
		DrawCurtain();

		//		//CurtainLeft.Reset();
//		CurtainLeft.Play(true);
//		
//		//CurtainRight.Reset();
//		CurtainRight.Play(true);
//		
//		curtainsDrawn = true;

		//Disable the title object.
		//TitleAnimation.gameObject.SetActive(false);

		//Play Sound effect.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotSelect);
	}
	
	public void Reset(bool KeepBlackBG = false)
	{
		
		//Just clear activitites upon reset for now.
		if(pause)
			pause.TogglePause(true);
		
		HUD.TogglePanel(true);
		
		//Reset all activities
		foreach(FUIActivity a in Activities)
		{
			a.OnReset();
		}

		PullCurtain();

		//make sure contorls are turned on upon reset.

		if(_myControlsManager)
			if(_myControlsManager.LoadedControls)
		{
			ControlsLayoutObject loadedControls = _myControlsManager.LoadedControls;

			//When we grab... hide all the controls...
			loadedControls.Shoot.gameObject.SetActive(true);
			loadedControls.Shield.gameObject.SetActive(true);
			loadedControls.Overheat.gameObject.SetActive(true);

		}


		//Toggle off boss notif
		BossNotif.CloseWindow();

		if(!KeepBlackBG)
		{
			//Make sure the black BG is always 0 alpha.
			BlackBG.alpha = 0.0f;
			BlackBG.gameObject.SetActive(false);

		}
	}
}
