using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGameActivity : FUIActivity {
	
	public FUIWindowToggles GameOverWindow;
	
	public FUIWindowToggles PhasesCountWindow;
	public FUIWindowToggles BG;
	
	
	public FUIPhaseResultObject BadgeReference;
	public UIGrid EndGameTable;
	
	public UIDraggablePanel panel;
	
	
	public UILabel ScoreLabel, BonusLabel, PPSLabel, RetryButtonLabel, MoneyLabel; 
	public UISprite Animal;
	
	public UIButton PPSButton;
	
	public TweenRotation TweenTimer;
	
	public TweenAlpha GOAlpha;
	public TweenPosition GOPosition;
	public TweenScale GOBringIN;

	public TweenPosition PPSButtonTween;
	public TweenRotation ScoreButtonIntro;
	
	public AudioClip IncommingPhase;
	public AudioClip IncommingPPS;
	public AudioClip GameReviewMusic;
	


	public TweenTransform BadgesGridTween;
	
	List<FUIPhaseResultObject> _resultList = null;
	bool prepredPhaseGrid = false;
	UICenterOnChild center;
	bool presented = false;
	
	bool gamereviewtrigger = false;

	int ScoresState = 0;

	
	//Setups
	public int initialCount = 10;
	public float Duration = 0.7f;
	public float GameOverDuration = 5.0f;
	public float SpeedDuration = 0.1f;
	
	float localDuration = 0.7f;

	bool viewPPS = false;

	string PPSText, ScoreText, CoinsText = "N/A";

	//Animation components for scores.
	TweenScale MoneyTween, PointsTween, BonusTween = null;
	
	enum state 
	{
		GameOver,
		Count,
		Scores,
		Present,
		CloseCurtain,
		NULL
		
	}
	
	state curState = state.NULL;
	// Use this for initialization
	new void Start () {
	
		base.Start();
		
		
		//Hide the pps button.
		PPSButton.gameObject.SetActive(false);
		Animal.gameObject.SetActive(false);

		ResetScoresLabel();

		//Set to defaul duration.
		localDuration = Duration;

		//Grab the tween components
		MoneyTween = MoneyLabel.GetComponent<TweenScale>();
		PointsTween = ScoreLabel.GetComponent<TweenScale>();
		BonusTween = BonusLabel.GetComponent<TweenScale>();
		
		PreparePhaseGrid();
	}

	void ResetScoresLabel()
	{
		//Hide the scoreslabels.
		MoneyLabel.alpha = 0.0f;
		ScoreLabel.alpha = 0.0f;
		BonusLabel.alpha = 0.0f;
	}
	
	public override void OnDeActivate ()
	{
		//base.OnDeActivate ();
		//ActivityManager.Instance.ToggleHud(false);
		
		//PhasesCountWindow.ToggleWindowOff();
		BG.ToggleWindowOff();
		
		//Manually toggle off the game over window on deactivate.
		PhasesCountWindow.ToggleWindowOff();
	}
	
	public override void OnReset ()
	{
		Debug.LogError("END GAME RESET");
		
		//Hide the pps button.
		PPSButton.gameObject.SetActive(false);
		Animal.gameObject.SetActive(false);


		ResetScoresLabel();
		//PhasesCountWindow.ToggleWindowOff();
		//BG.ToggleWindowOff();
		
		//Set to defaul duration.
		localDuration = Duration;
		TweenTimer.duration = 0.7f;
		
		presented = false;
		curState = state.NULL;
		
		//ResetTimer(0.7f);
		ScoresState = 0;
		
		//Set the game review and timer
		gamereviewtrigger = false;

		string PPSText, ScoreText, CointsText = "N/A";
		
		ResetPhaseGrid();
	}
	
	public override void OnActivate ()
	{
		//Prepare the grid.
		if(!prepredPhaseGrid)
			PreparePhaseGrid();
		
		if(presented)
		{
			
			//Toggle back on the game over window.
			PhasesCountWindow.ToggleWindowOn();
			BG.ToggleWindowOn();
			ActivityManager.Instance.ToggleHud(false);
			
			return;
		}
	
		//Fill up the list.
		int index = 0;
		foreach(BasePhase.PhaseData phase in GameObjectTracker.instance._PlayerData.Breathless.PhaseList)
		{
			_resultList[index].PhaseData = phase;
			index++;
		}
		
		
		GameOverWindow.SetWindowAlpha(1.0f);
		//Set the information
		GameOverWindow.ToggleWindowOn();
		
		//Reset the game over window animations for bring in.
		GOAlpha.Reset();
		GOAlpha.Play(true);
		GOPosition.Reset();
		GOPosition.Play(true);
		GOBringIN.Reset();
		GOBringIN.Play(true);
			
		
		//Set the timer to game over duration upon activate.
		ResetTimer(GameOverDuration);
		curState = state.GameOver;
		
		//Grab the games statistics. By now the game stats should be of a completed game.
		Statistics gameStats = GameObjectTracker.instance.RunStatistics;
		
		if(gameStats == null)
			return;
		
		//Set the score.
		ScoreLabel.text = string.Format("{0:#,#,#}", gameStats.Score);
		
		//Set the time.
		//TimeLabel.text = FormatSeconds(gameStats.TimeAmount);

		//Set the bonus amount
		BonusLabel.text = "N/A";

		//Set the coins collected.
		//MoneyLabel.text =  string.Format("{0:C}",gameStats.Money);
		//MoneyLabel.text =  gameStats.Money.ToString();
		
		//Set the PPS label
		//PPSLabel.text = gameStats.PPS.ToString() + " PPS";
		PPSText = string.Format("{0:#,#,#} PPS", gameStats.PPS);
		ScoreText = string.Format("{0:#,#,#}", gameStats.Score);
		CoinsText = string.Format("{0:C}",gameStats.Money);

		PPSLabel.text = ScoreText;
		viewPPS = false;

		MoneyLabel.text = CoinsText;

		//Set the animal.
		
		//Grab player data.
		BaseItemCard card = GameObjectTracker.instance._PlayerData.FindCardByCannonType(gameStats.CompletionCannon);
		
		//Check if its a valid card theng grab the icon name.
		if(card == null || gameStats.CompletionCannon == EntityFactory.CannonTypes.NULL 
			|| gameStats.CompletionCannon == EntityFactory.CannonTypes.Empty)
		{
			Animal.spriteName = "phaseunknown";
			return;
		}
		
		Animal.spriteName = card.DisplayInfo.IconName;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!prepredPhaseGrid)
		{
			PreparePhaseGrid();
			return;
		}
				

		if(presented)
		{
			//Check for button controls here to restart.
			if(Input.GetButtonDown("Shoot"))
			{
				//Call on retry.
				OnRetry();
			}
		}

		
	}
	

	void OnPhaseGridTween()
	{
		//PhasesCountWindow.ToggleWindowOn();
		BG.ToggleWindowOn();
	}

	void OnBringIn()
	{
		Debug.LogError("ON BRING IN");
		
		//Turn off slow motion when the game over is brought in...
		ToyBox.GetPandora().SetSlowMotion(false);
	}
	
string FormatSeconds(float elapsed)
	{
	   int d = (int)(elapsed * 100.0f);
	   int minutes = d / (60 * 100);
	   int seconds = (d % (60 * 100)) / 100;
	   int hundredths = d % 100;
	   return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
	}	
	
	
	void OnTimerDone()
	{
	//	Debug.Log("ON TIMER!");
		
		//If this is set to true when timer is done.
		if(gamereviewtrigger)
		{
			//Lets play the game review music here... sure...
			AudioPlayer.GetPlayer().PlayAudioClip(GameReviewMusic);
			
		}
		
		if(curState == state.Present && !PPSButton.gameObject.activeSelf)
		{
			PPSButton.gameObject.SetActive(true);
			ScoreButtonIntro.Reset();
			ScoreButtonIntro.Play(true);

			Animal.gameObject.SetActive(true);
			
			if(IncommingPPS)
				AudioPlayer.GetPlayer().PlaySound(IncommingPPS);
			
			presented = true;
			
			//Set the game review and timer
			gamereviewtrigger = true;
			
			float t  = AudioPlayer.GetPlayer().RemainingTime;
			ResetTimer(t);



			//Debug.LogError("Track change timer set :" + t.ToString());
		}

		//Close the curtains here.
		if(curState == state.GameOver)
		{
			curState = state.CloseCurtain;

			//Hide the hud.
			ActivityManager.Instance.ToggleHud(false);
			
			//Pull curtain
			ActivityManager.Instance.PullCurtain();
			
			//Pause the game
			ToyBox.GetPandora().TimePaused = true;

			ResetTimer(localDuration * 2.0f);

		}
		
		//This is when we finish displaying the game over sign.
		if(curState == state.CloseCurtain)
		{
			GameOverWindow.SetWindowAlpha(0.0f);
			GameOverWindow.ToggleWindowOff();

			PhasesCountWindow.ToggleWindowOn();
			//BG.ToggleWindowOn();
			
			curState = state.Count;
			//Set the tween animation for badges.
			BadgesGridTween.Reset();

			ResetTimer(localDuration);
		}


		if(curState == state.Scores)
		{

			switch(ScoresState)
			{
			
				//Present coins.
			case 0:
			{
				MoneyLabel.alpha = 1.0f;
				MoneyTween.Reset();
				MoneyTween.Play(true);

				AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CashRegister);
				break;
			}
				//Present score
			case 1:
			{
				ScoreLabel.alpha = 1.0f;
				PointsTween.Reset();
				PointsTween.Play(true);

				AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotSelect);
				break;
			}
				//Present Bonus
			case 2:
			{
				BonusLabel.alpha = 1.0f;
				BonusTween.Reset();
				BonusTween.Play(true);

				AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotSelect);
				break;
			}
			

			default:
			{
				break;
			}
			}


			ScoresState++;
			if(ScoresState > 3)
			{
				curState = state.Present;
				ResetTimer(localDuration);
				return;
			}


			//Resetting the timer after being displayed.
			ResetTimer();
			return;
			
		}

		
		if(curState == state.Count)
		{
			int index = 0;
			foreach(FUIPhaseResultObject badge in _resultList)
			{
				//If we hit a badge with null data then we are finished and change state.
				//Testing, should be a test if there is valid phase data.
				if(badge.PhaseData == null)
				{
					curState = state.Scores;

					//Play animation and sound effects.
					BadgesGridTween.Play(true);
					AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotPushed);

					//Turn on windows here.
					PhasesCountWindow.ToggleWindowOn();
					BG.ToggleWindowOn();

					ResetTimer();
					return;
				}
				
				if(!badge.gameObject.activeSelf)
				{
					//Repositioning on the start too aparently...
					panel.ResetPosition();
					
					//Else we will activate the current object, reset the timer, and return from the function. 
					badge.gameObject.SetActive(true);
							
					//Repositioning
					EndGameTable.Reposition();	
					panel.ResetPosition();
					
					//Resetting the timer after being displayed.
					ResetTimer();
					
					if(IncommingPhase)
						AudioPlayer.GetPlayer().PlaySound(IncommingPhase);
					
					return;
				}
					
				index++;	
			}
		}
	
	}
	
	void ResetTimer(float amount = -1.0f)
	{
		//Set the timer amount if one is applied.
		if(amount != -1.0f)
			TweenTimer.duration = amount;
		
		TweenTimer.Reset();
		TweenTimer.Play(true);
		
	}
	void PreparePhaseGrid()
	{
		if(_resultList == null)
		{
			_resultList = new List<FUIPhaseResultObject>();
			//return;
		}
		
		for(int i = 0; i < initialCount; i++)
		{
			//Create teh new result.
			FUIPhaseResultObject newResult = NGUITools.AddChild(EndGameTable.gameObject,
				BadgeReference.gameObject).GetComponent<FUIPhaseResultObject>();
				
			//Testing purposes.
			if(i < 5)
			newResult.TEMPBOOL = true;
			
			
			//Add to list and deactivate.
			newResult.gameObject.SetActive(false);
			_resultList.Add(newResult);
			
				
		}
		
		//BasePhase.PhaseData[] phases = GameObjectTracker.instance._PlayerData.Breathless.PhaseList
		
		//Fill up the list.
		int index = 0;
		foreach(BasePhase.PhaseData phase in GameObjectTracker.instance._PlayerData.Breathless.PhaseList)
		{
			_resultList[index].PhaseData = phase;
			index++;
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
			pResult.PhaseData= null;
			pResult.gameObject.SetActive(false);
			
		}
		
	}

	void OnFullStats()
	{
		//Set active stats and pop screen.
		ActivityManager.Instance.SelectedStats = GameObjectTracker.instance.RunStatistics;
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Stats,true);
		

	}

	void OnPPS()
	{
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

		ScoreButtonIntro.Reset();
		ScoreButtonIntro.Play(true);

		//TODO: Play sound.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
	
	}
	
	void OnRetry()
	{
		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.LevelUp,true);
	}
	
	void OnSkip()
	{
//		Debug.LogError("ON SKIP");
		
		if(presented)
			return;
		
		localDuration = SpeedDuration;
		OnTimerDone();
		
		
	}
	
}
