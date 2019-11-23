using UnityEngine;
using System.Collections;

public class XPActivity : FUIActivity {

	//Access to the progress window.
	public ProgressWindow MyProgressWindow;

	//Animation references.
	public TweenAlpha IntroAlpha, Timer;

	public UILabel PPSLabel;
	public TweenPosition PPSButtonTween;

	public TweenPosition IntroPosition;

	string PPSText, ScoreText = "N/A";
	bool viewPPS = false;

	// Use this for initialization
	new void Start () {
	
		base.Start();

		//Grab the other animations.
		//IntroPosition = IntroAlpha.GetComponent<TweenPosition>();

	}

	public override void OnActivate ()
	{
		//Set the progress window data.
		MyProgressWindow.Data = GameObjectTracker.instance._PlayerData.MyPTPLevel;

		//Grab the games statistics. By now the game stats should be of a completed game.
		Statistics gameStats = GameObjectTracker.instance.RunStatistics;

		//Set the final score text
		PPSText = string.Format("{0:#,#,#} PPS", gameStats.PPS);
		ScoreText = string.Format("{0:#,#,#}", gameStats.Score);
		PPSLabel.text = ScoreText;
		viewPPS = false;

		IntroAlpha.Reset();
		IntroPosition.Reset();

		Timer.Reset();
		Timer.Play(true);


	}

	// Update is called once per frame
	void Update () {
	
	}


	void IntroTweenDone()
	{
		//Now hide it.
		IntroAlpha.Reset();
		//IntroPosition.Reset();


		//Play sound.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotPushed);

		MyProgressWindow.TriggerProgressAnimation();

	}

	void OnTimerDone()
	{
		//Play gain animation
		IntroAlpha.Play(true);
		IntroPosition.Play(true);
		
		
		//Play sound.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.LevelUp);

	}

	void OnPPS()
	{
		//Set active stats and pop screen.
		//ActivityManager.Instance.SelectedStats = GameObjectTracker.instance.RunStatistics;
		//ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Stats);
		
		//Toggle
		viewPPS = !viewPPS;
		
		if(viewPPS)
		{
			//Set the text.
			PPSLabel.text = PPSText;
			
		}
		else
		{
			//Set the text.
			PPSLabel.text = ScoreText;
		}
		
		//Play the animation.
		PPSButtonTween.Reset();
		PPSButtonTween.Play(true);
		
	//	ScoreButtonIntro.Reset();
	//	ScoreButtonIntro.Play(true);
		
		//TODO: Play sound.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		
	}


	void OnRetry()
	{
		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PopActivity();

		GameObjectTracker.instance.RestartButtonPushed();
	}

	void OnBack()
	{
		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		
		ActivityManager.Instance.PopActivity();
	}
}
