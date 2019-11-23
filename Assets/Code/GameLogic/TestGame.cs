using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGame : BaseGame {
		
	
	public enum LevelStates
	{
		Introduction,
		Phasing,

		MiniBoss,
		Boss,
		Dead,

		GameOver,
		Review,
		TestSpawn
	}
	
	
	//Local Information
	float localTimer;
	int targetsDestroyed;
	bool gameover = false;

	
	//Music Tracks
	public AudioClip GameOverMusic;
	public AudioClip GameReviewMusic;
	public AudioClip GameOverIntro;
	
	public float GameReviewMusicTimer = 36.0f;
	float musictimer = 0.0f;
	 
	
	//State Information
	LevelStates currentState;
	
	//Phase Information
	public List<BasePhase> PhaseTypes;
	BasePhase currentPhase;
	
	//Game statistics information.
	Statistics BreathlessStats = null;
	
	public int currentPhaseIndex;
	public int BossLevelIndex = 3;
	

	//Rewards and displays.
	public GameOverView gameReview;
	public Rewards gameRewards;
	
	public GameObjectTracker GOT;
	public ToyBox Pandora;
		
	
	bool started = false;
	bool curtainOpen = false;
	float opentimer = 0.0f;

	int cannonPushes = 0;
	
	void Awake()
	{
		Time.timeScale = 0.0f;
		
//		if(ActivityManager.Instance == null)
//			opentimer = 1.0f;
//		
		
		//Check if GOT is null then create one.
		if(!GameObjectTracker.instance)
			Instantiate(GOT);
			
		if(!ToyBox.GetPandora())
		{
//			Debug.LogError("Toybox Instantuate");
			Instantiate(Pandora);
		}
		
		
	}
	
	// Use this for initialization
	void Start () {
		
		Time.timeScale = 0.0f;
		
		
		
		//Set the current State
		currentState = LevelStates.Introduction;	
		
		//suiosfa
		//gameRewards = Instantiate(gameRewards) as Rewards;
		
		//Create the stats.
		BreathlessStats = Instantiate(GameObjectTracker.instance._PlayerData.BlankStatistics) as Statistics;
				
		//SocialCenter.Instance.LoadLeaderboard("Highscore_PPS");
		//SocialCenter.Instance.ProcessLeaderboardScores();
	
		opentimer = 0.0f;
		
		//Start time.
		if(!ToyBox.GetPandora())
		{
			Debug.LogError("NO F TOYBOX!");
			return;
			
		}
		
		ToyBox.GetPandora().TimePaused = true;
		ToyBox.GetPandora().SceneBallStack.NumberBalls = 30;
		//started = true;

	
	}
	
	// Update is called once per frame
	void Update () {

		if(!started && ActivityManager.Instance != null)
		{
			ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Begin);
			started = true;
		}

//		if(!started && opentimer > 2.0f)
//		{
//			if(ToyBox.GetPandora())
//			{
//				ToyBox.GetPandora().TimePaused = false;
//				ToyBox.GetPandora().SceneBallStack.NumberBalls = 30;
//				started = true;
//
//				//Display the Hud
//				ActivityManager.Instance.DrawCurtain();
//
//				curtainOpen = true;
//				ActivityManager.Instance.ToggleHud(true);
//			}
//				
//		}
//		
//		opentimer += Time.fixedDeltaTime;
		

		//State machine update.
		UpdateState();
		
		
		//we check if we are not in the game over state so we only execute this code once.
		if(!gameover)
		{
			//We always check see if the command center is destroyed. then we give a game over.
			if(PandoraBox.CommandCenter_01.IsDestroyed())
			{
				//Change the current state to game over state and start the timer.
				currentState = LevelStates.Dead;
				localTimer = 0.0f;
				
				//Set local bool
				gameover = true;
				
				//Stop the current phase stat and add to the gamerun stat.
				currentPhase.Data.PhaseStatistics.StopTimer();
				//GameObjectTracker.GetGOT().gameRunStatistics.AddStatistics(currentPhase.Data.PhaseStatistics);
				BreathlessStats.AddStatistics(currentPhase.Data.PhaseStatistics);
				
				
				//Stop the timer for the game statistics and pass in teh locally tracked stats.
				GameObjectTracker.GetGOT().StopGame(BreathlessStats);
				
				//Stop the music for the effect when the command center is destroyed!
				currentPhase.StopMusic();
				
				//Play the game over music.
				AudioPlayer.GetPlayer().PlayAudioClip(GameOverMusic);
				musictimer = Time.time;
				
				//Set the game to slow motion when we are dead.
				PandoraBox.SetSlowMotion(true);
				
				//Set the death camera
				PandoraBox.Camera_01.SetDie(PandoraBox.CommandCenter_01.GetDeathObject(),true);

				//Clear the phases and stuff..
				Clear();
			}
			
			
		}
		
			
	}
	
	void UpdateState()
	{
		
		switch(currentState)
		{
			
		case LevelStates.Introduction:
		{
			break;
		}
			
		case LevelStates.TestSpawn:
		{
			
			
			break;
		}
		case LevelStates.Phasing:
		{
			//If we dont have a current phase, then we will assign it one from the list!
			if(!currentPhase)
			{
				currentPhase =  Instantiate(PhaseTypes[currentPhaseIndex]) as BasePhase;
				
				//Check which phase we are on.
//				if(currentPhaseIndex == 5)
				if(currentPhase.TransitionLevel)
				{
					//GameObjectTracker.instance.vWorld.GotoHeavyRoom();
					GameObjectTracker.instance.vWorld.GotoNextRoom();
					return;
				}
				
				
			}
			
			//If we have finished the phase
			if(currentPhase.IsComplted())
			{
				EntityFactory.CannonTypes completedCannon = EntityFactory.CannonTypes.Empty;
				
				//Find out what cannon we are using and set the phase.
				if(PandoraBox.Bot_01.IsCannonAttached())
					completedCannon = PandoraBox.Bot_01.GetCannon().CannonTypeInfo;
				
				//Handle timing and rewards
				//gameRewards.DisplayFruit(currentPhaseIndex,currentPhase.GetPhaseTime());
				GameObjectTracker.GetGOT()._PlayerData.PunchPhaseTime(currentPhase.GetPhaseTime(),completedCannon);
				BreathlessStats.AddStatistics(currentPhase.Data.PhaseStatistics);
				
				
				//Stop the music fo the current phase.
				//currentPhase.StopMusic();
				
				//Increment the phase index.
				currentPhaseIndex++;
				
				//Delete the current phase.
				Destroy(currentPhase.gameObject);	
				
			}
			
			break;
		}
			

		
		case LevelStates.Dead:
		{
			//Update the timer.
			localTimer += Time.deltaTime;
			
			if(localTimer > 0.5f)
			{
				
				//Go to game over state.
				currentState = LevelStates.GameOver;
				
				//Go back to regular motion.
				//PandoraBox.SetSlowMotion(false);
				
				//Reset the local timer.
				localTimer = 0.0f;
				
				//Set to the game over timer;
				PandoraBox.Camera_01.SetGameOver(true);
				
				//Tell the HUD to activate Game Over
				//PandoraBox.OnScreenControls.ActivateGameOver();	
				ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.GameOver);
				
				//Play the game over music.
				//AudioPlayer.GetPlayer().PlayAudioClip(GameOverMusic);
				AudioPlayer.GetPlayer().PlaySound(GameOverIntro);
				
				//Set the timer for gameover music
				//musictimer = Time.time;
				
				//Lets apply the earnings of the phases here.
				GameObjectTracker.GetGOT()._PlayerData.SaveAllData();
			}
			
			
			
			break;
		}
			
		case LevelStates.GameOver:
		{
			//Update the timer to go into review mode.
			localTimer += Time.deltaTime;
			
			if(localTimer > 0.3f)
			{
				
				PandoraBox.SetSlowMotion(false);
				currentState = LevelStates.Review;
			}
			
			
			break;
		}
			
		case LevelStates.Review:
		{
			//W do nothing here yet.
//			if(!audio.isPlaying)
//			{
//				audio.clip = GameReviewMusic;
//				audio.loop = true;
//				audio.Play();
//				
//				//GameReviewMusic.
//			}
			//If enough time has passed since the time stamp.
			if( (Time.time - musictimer ) > GameOverMusic.length )
			{				
				AudioPlayer.GetPlayer().PlayAudioClip(GameReviewMusic);
			}
			
			break;
		}
		default:
		{
			print("There is no Game State");
			return;
		}

		}
	
	}
	

		
		/// <summary>
	/// Targets the destroyed.
	/// </summary>
	/// <returns>
	/// The destroyed.
	/// </returns>
	/// <param name='position'>
	/// If set to <c>true</c> position.
	/// </param>
	public override void TargetDestroyed(Target t)
	{
		DropRewards(t.Position);
		targetsDestroyed++;
		
	}
	
	
	public override void BossCompleted()
	{
		//If we complete a boss on game over we stick to the boss level.
		if(gameover)
			return;
		
		//Just play the animation for now.
		GameObjectTracker.GetGOT().vWorld.ActivateBossRoom(false);
	}
	
	public override void BossStart()
	{
		//Just play the animation for now.
		GameObjectTracker.GetGOT().vWorld.ActivateBossRoom(true);
	}

	public override void Clear()
	{
		//Delete the current phase.
		Destroy(currentPhase.gameObject);
	}

	public override void CannonPushed ()
	{
		//Get more cannons.
		cannonPushes++;


		//If we are at the intro stage, move on to the start of the game upon picking up the cannon.
//		if(currentState == LevelStates.Introduction && cannonPushes > 4)
//		{
//
//			//Begin the phasing level.
//			currentState = LevelStates.Phasing;
//			
//			//Stop our audio.
//			//audio.Stop();
//			
//			//Start teh game statistics counter.
//			GameObjectTracker.GetGOT().StartGame();
//			
//		}
	}
	
	public override void CannonPickedUp()
	{
//		//If we are at the intro stage, move on to the start of the game upon picking up the cannon.
		if(currentState == LevelStates.Introduction)
		{
			//Begin the phasing level.
			currentState = LevelStates.Phasing;
			
			//Stop our audio.
			//audio.Stop();
			
			//Start teh game statistics counter.
			GameObjectTracker.GetGOT().StartGame();

			
		}
		
	}
	
	public override int GetCurrentMultiplierModifier()
	{
		if(currentPhase)
		{
			return currentPhase.GetScoreMultiplier();
		}
		
		return 0;
	}
	
	public override string GetCurrentPhaseName()
	{
		if(currentPhase)
		{
			return currentPhase.name;
		}
		
		return "No Name";
	}
		
	
	
	
	#region Game Functions
	
	void DropRewards(Vector3 position)
	{
		PandoraBox.SpawnGems(2,position,GameObjectTracker.GetGOT().GetMultiplier());
	}
	
	void SetMissionStatistics()
	{
		
	}
	
	
	#endregion
	
}
