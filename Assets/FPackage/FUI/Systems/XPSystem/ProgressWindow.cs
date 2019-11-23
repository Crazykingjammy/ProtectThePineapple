using UnityEngine;
using System.Collections;

public class ProgressWindow : MonoBehaviour {

	public UISprite PieForeground, PieShadow, Icon;
	public UILabel LevelCount, XPFalta, XPCompletion, XPTotal, XPCurrent;

	public TweenAlpha Timer, BGTween;
	public TweenScale PieIntroTween;
	TweenScale IconTween;

	//Private cache data.
	PlayerLevelSystem leveldata;


	//Internal variables.
	float previousPercent, currentPercent = 0.0f;
	int previousLevel, currentLevel, previousRemain, remainingXP, previousTotal, currentTotal, currentCompletionXP;

	int _levelCouter = 0;

	int deltaRemain = 0;
	//Local values needed for animations and presentation.
	bool isAnimating = false;

	// Use this for initialization
	void Awake () {

		//Grabbing and assigning the one time stuff here.

		IconTween = Icon.GetComponent<TweenScale>();

	}

	public PlayerLevelSystem Data
	{
		set{
			//Set the level system reference.
			leveldata = value;

			SetupInfo();
		}
	}



	void SetupInfo () {
	

		//leveldata = GameObjectTracker.instance._PlayerData.MyPTPLevel;

		//We should calculate how many levels we have increased.
		
		//Set the internal data.
		previousPercent = leveldata.BeforeCurrent.LevelPercent;
		currentPercent = leveldata.CurrentLevelCompletionPercent;

		previousLevel = leveldata.BeforeCurrent.Level;
		currentLevel = leveldata.Level;

		previousRemain =leveldata.BeforeCurrent.RemainingXP;
		remainingXP = leveldata.XPToNextLevel;

		previousTotal = leveldata.BeforeCurrent.XP;
		currentTotal = leveldata.TotalXP;


		currentCompletionXP = leveldata.CurrentLevelCompletionXP;

		_levelCouter = previousLevel;

		//CalculatePieAnimation();

		SetPieFill(previousPercent);

		LevelCount.text = previousLevel.ToString();
		XPCompletion.text = string.Format("{0:#,#,#}", previousTotal);
		XPCurrent.text = string.Format("{0:#,#,#}", previousRemain);

		XPTotal.text = string.Format("{0:#,#,#}",GameObjectTracker.instance._PlayerData.TotalXP);

		XPFalta.text = " - ";

		//Grab the icon tween from the 
		Icon.gameObject.SetActive(false);

		//Set some values
		isAnimating = false;
	}

	
	// Update is called once per frame
	void Update () {
	

		//Update the data only when animating.
		if(isAnimating)
		{
			float a = BGTween.alpha;

			SetPieFill(a);

			//Set the remaining xp label.
			int n = currentCompletionXP - (int)(deltaRemain * a);
			XPFalta.text = string.Format("{0:#,#,#}", n);

			XPCurrent.text = string.Format("{0:#,#,#}", (int)(currentCompletionXP * a) );

		}
	}

	public void TriggerProgressAnimation(bool force = false)
	{
		//isAnimating = true;
		//Debug.LogError("TriggerAnimation");

		if(force)
			_levelCouter = currentLevel;

		//Grab the icon tween from the 
		Icon.gameObject.SetActive(true);

		IconTween.Reset();
		IconTween.Play(true);

	}

	void CalculatePieAnimation()
	{


		//If we have more levels to count, start from 0 and end at 1.
		if(currentLevel == _levelCouter)
		{
			//Set the two and from positions of the alpha to the percentages.
			BGTween.from = previousPercent;
			BGTween.to = currentPercent;

			//Calculate the delta in remaining xp
			//The delta will be added along with the animations
			deltaRemain = remainingXP;


			//Set some labels.
			LevelCount.text = currentLevel.ToString();
			XPFalta.text = string.Format("{0:#,#,#}", previousRemain);
			XPCompletion.text = string.Format("{0:#,#,#}", currentCompletionXP);

			//Set the labels.
			SetPieFill(previousPercent);

		}
		if(currentLevel > _levelCouter)
		{
			//Set the two and from positions of the alpha to the percentages.
			BGTween.from = previousPercent;
			BGTween.to = 1.0f;

			deltaRemain = currentCompletionXP;

			LevelCount.text = _levelCouter.ToString();
			XPCompletion.text = string.Format("{0:#,#,#}", currentCompletionXP);
			XPFalta.text = " - ";

			//Since we are upping a whole level here, set previous percent to 0.
			previousPercent = 0.0f;
		}


		//The pie BG animation
		PieIntroTween.Reset();
		
		//The alpha animation for the pie chart 
		BGTween.Reset();
		






		

	}


	void SetPieFill(float fill)
	{
		PieForeground.fillAmount = fill;
		PieShadow.fillAmount = fill;
	}

	void OnTimerDone()
	{

	}

	void GainTweenDone()
	{
		//Pass in the is animationg bool to mark if this is the first time we are calculating animation.
		//if this is the first time then the animation should be set to false. and the fals value shoudl pass in to calculate accordingly.

		isAnimating = true;

		CalculatePieAnimation();

		//Now lets start the pie chart animation after the icon slams in.
		PieIntroTween.Play(true);
		BGTween.Play(true);

		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotSelect);

		_levelCouter++;

	}

	void PieFillTweenDone()
	{
		//Repeat the previous animation untill level counter is up to current.
		if(_levelCouter <= currentLevel)
		{
			GainTweenDone();
			return;
		}

		//Finished the pie animation now set all values to current.
		isAnimating = false;

		//Debug.LogError("Pie FIll");

		//Set some labels.
		LevelCount.text = currentLevel.ToString();
		XPFalta.text = string.Format("{0:#,#,#}", remainingXP);
		XPCurrent.text = string.Format("{0:#,#,#}", leveldata.CurrentLevelXP);

		SetPieFill(currentPercent);

		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CrowdSuspense);


	}


}
