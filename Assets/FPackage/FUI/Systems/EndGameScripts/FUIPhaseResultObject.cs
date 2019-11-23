using UnityEngine;
using System.Collections;

public class FUIPhaseResultObject : MonoBehaviour {
	
	public UISprite phaseSprite;
	public UILabel phaseLabel;
	public UISprite phaseBestTimeBadgeSprite;
	public UISprite CannonBadge;
	public string givenSpriteName = "phaseiconunknown";
	
	
	
	public bool isBestTime = false;
	BasePhase.PhaseData _data = null;
	public TweenScale myAnimation;
	bool completed = false;
	
	
	public bool TEMPBOOL = false;
	
	public BasePhase.PhaseData PhaseData
	{
		get {return _data;}
		set{
			
			_data = value;
			
			//set the phase data as a new comming data.
			completed = false;
			phaseLabel.text = "Failed!";
			phaseSprite.spriteName = "phaseunknown";
			
			//Hide the animal and completion
			CannonBadge.alpha = 0.0f;
			phaseBestTimeBadgeSprite.alpha = 0.0f;
			
			
		}
	}
	
	// Use this for initialization
	void Start () {
		
		//Grab the animation component.
		
	}
	void OnEnable()
	{
		//Animate if we must.
		myAnimation.Reset();
		myAnimation.Play(true);
		
	}
	
	void Update(){
		
		if(_data == null)
		{
			phaseLabel.text = "XX:XX";
			phaseSprite.spriteName = "phaseunknown";
			return;
		}
		
		if(completed)
			return;
		
		if(_data.PhaseCompletionPunch != -1.0f)
		{
			//Debug.Log("Phase Completion Punch Pass at " + FormatSeconds(_data.PhaseCompletionPunch));
			
			//set the phase data
			phaseLabel.text = FormatSeconds(_data.PhaseCompletionPunch);
			phaseSprite.spriteName = _data.IconTextureName;
			phaseBestTimeBadgeSprite.alpha = 0.0f;
			CannonBadge.alpha = 1.0f;
			
			
			//Grab player data.
			EntityFactory.CannonTypes cType = _data.PhaseStatistics.CompletionCannon;
			BaseItemCard card = GameObjectTracker.instance._PlayerData.FindCardByCannonType(cType);
			
			//Check if its a valid card theng grab the icon name.
			if(card == null || cType == EntityFactory.CannonTypes.NULL 
				|| cType == EntityFactory.CannonTypes.Empty)
			{
				CannonBadge.spriteName = "phaseunknown";
				completed = true;
				return;
			}
			
			CannonBadge.spriteName = card.DisplayInfo.IconName;
			
			//Maybe check for objective completion here.
			
			//Set completed.
			completed  = true;
			
		}
		
	}
	
	void OnClick(){
		isBestTime = !isBestTime;
		
		ActivityManager.Instance.SelectedStats = _data.PhaseStatistics;
		
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Stats);
	}
	
	string FormatSeconds(float elapsed)
	{
	   int d = (int)(elapsed * 100.0f);
	   int minutes = d / (60 * 100);
	   int seconds = (d % (60 * 100)) / 100;
	   int hundredths = d % 100;
	   return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
	}

}
