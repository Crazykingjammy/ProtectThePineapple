using UnityEngine;
using System.Collections;

public class FUIPauseScript : MonoBehaviour {
	
	static FUIPauseScript PAWS = null;
	
	bool isInGame = true;
	bool isPaused = false;
	
	public FUIEnablerScript InGameScreen;
	public FUIEnablerScript BreakScreen;
	
	public TweenScale CurtainLeft, CurtainRight;
	
	public GameObject myWindow;	
	public Transform positionPaused;
	public Transform positionIngame;
	public Transform positionPausedBig;
	
	// we want to change out color of the background
	public UISprite background, Band;
	public Color pausedColor;
	public Color notPausedColor;
	
	public Color TenseColor, RestColor;
	
	// this is a cache of the window that pops out 
	TweenTransform windowTransform;
	Transform pausedPosition = null;
	Transform band = null;
	bool hack = false;
	
	public float Range, MinWidth,MaxWidth;
	
	/*
	 * Make sure all Activities are shut down when resume to the game
	 */
	public FUIActivity _invActivity = null;
	
	
	static public FUIPauseScript GET(){
		return PAWS;
	}
	
	public void Start(){
		PAWS = this;
		windowTransform = myWindow.GetComponent<TweenTransform>();
		
		band = Band.transform;
		
		ActivityManager.Instance.pause = this;
		
//		//Enable the curtains
//		CurtainLeft.gameObject.SetActive(true);
//		CurtainRight.gameObject.SetActive(true);

	}

	void OnPauseButton()
	{
		hack = true;
		TogglePause();
	}
	
	public void Update(){
		
		
		//Check for position.
		if(pausedPosition == null)
			AssignPausePosition();
		
		if (Input.GetKeyUp(KeyCode.P) || Input.GetButtonDown("Start") ){
		
			hack = true;
			TogglePause();
		}
		
		if (Input.GetKeyUp(KeyCode.I)){		
			
			_invActivity.ToggleActivity();
		}
		
		if (isPaused){
			background.color = pausedColor;

			//Check to restart here.
			if(Input.GetButtonDown("OverHeat"))
			{
				GameObjectTracker.instance.RestartButtonPushed();
			}
		
		}
		else
			background.color = notPausedColor;
		
		
		if(transform.localPosition.x < 0)
			band.rotation = Quaternion.AngleAxis(Angle,Vector3.forward);
		else
			band.rotation = Quaternion.AngleAxis(-Angle,Vector3.forward);
		
		//Set the wdith of the band
		band.localScale = new Vector3(Width,Distance,1.0f);
		
		float ratio = Width/MaxWidth;
		
		Band.color = Color.Lerp(TenseColor,RestColor ,ratio);
		
		
		//Test distance to trigger pause
		//Distance must be bigger than the range to trigger a pause.
		if(!isPaused)
		{
			
			if(Distance > Range)
			{
				TogglePause();
				
			}
			
		}
			
		
		
	}
	
	public bool TogglePause(bool oPause = false){
		
		if(oPause || ToyBox.GetPandora().TimePaused)
			isPaused = true;
		
		OnClick();
		return isPaused;
	}
	
	void AssignPausePosition()
	{
//		Debug.LogError("Assigning control pause position.");
		
		if(GameObjectTracker.instance == null)
			return;
		
		
		
		//Check if we are in need of big pause.
		if(GameObjectTracker.instance._PlayerData.currentDevice == ControlsLoader.DEVICE.iphone)
		{
			//Debug.LogError("Pause Position assigned: BIG1");
			pausedPosition = positionPausedBig;
		}
		else
		{
			//Debug.LogError("Pause Position assigned: Default!");
			pausedPosition = positionPaused;
		}
		
		//Set the transform from the tween.
		windowTransform.from   = pausedPosition;

	}
	
	void OnClick(){


		
		//Distance must be bigger than the range to trigger a pause.
		if(!isPaused)
			if(Distance < Range && !hack)
				return;
			
		
		
		isInGame = !isInGame;
		isPaused = !isPaused;
				
		
		
		

		
		if (windowTransform != null){
			windowTransform.Play(!isPaused);
		}
		
	
		//Set the pause time
		ToyBox.GetPandora().TimePaused = isPaused;
		
		if (isPaused){
			/*
			 * We can enable the activities and prep them for the main menu
			 */
			//_invActivity.ActivityOn();
			
			
			AssignPausePosition();
			
			InGameScreen.TogglePanel(false); //.TogglePanelFade();
			//ActivityManager.Instance.PullCurtain();
			
			ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Pause,true);
			
			//Play audio sound
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Pause);
			
		}
		else{
			
			/* 
			 * Make sure all activities are closed
			 */
			//_invActivity.ActivityOff();
			InGameScreen.TogglePanel(true); //.TogglePanelFade();
			
			
			//Clear everything when unpause!! //No !
			ActivityManager.Instance.ClearActivities();

			//Open Says Me
			//ActivityManager.Instance.DrawCurtain();
//			ActivityManager.Instance.TriggerTitleAnimation();

			//No! lol
			//ActivityManager.Instance.PopActivity();
		}
		
		
	}
	

	
	float Angle
	{
		get
		{
			Vector3 dir =  transform.localPosition - Vector3.zero;
			
			return Vector3.Angle(dir,Vector3.up);
		}
	}
	
	float Distance 
	{
		get
		{
			Vector3 dir = transform.localPosition - Vector3.zero;
			
			float mag = dir.magnitude;
			
			if(mag == 0)
				return 0.01f;
			
			return mag;
		}
	}
	
	
	float Width
	{
		get
		{
			
			float ratio = Range/Distance;
			float number = MinWidth * ratio;
			return Mathf.Clamp(number, MinWidth, MaxWidth);
		}
	}
	
}
