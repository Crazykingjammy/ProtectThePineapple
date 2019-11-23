using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectTracker : MonoBehaviour {
	
	//Static object to be accessed project wide.
	static private GameObjectTracker GOT = null;
	
	//This is the game that will be updated as entities register events.
	public BaseGame Game = null;
	
	//The entity factory!
	public EntityFactory _entityFactoryPrefab;
	
	//The Gameplay level for various interactions.
	public GameplayWorld _WorldPrefab;
	static GameplayWorld _worldRef = null;
	public VisualWorld vWorld = null;
	
	//Effect for showing a combo is done. (Will be here for now)
	public ParticleSystem ComboEffect;
	public ParticleSystem LightningEffect;
	public ParticleSystem GemDieEffect;
	
	
	//Audio Files
	public AudioClip ComboEffectSound15x;
	public AudioClip ComboEffectSoundFail15x;
	
	public AudioClip[] Pops;
	
	public FUIPointsCounter _counteRref;

	// what combo min to play awwwww...
	public int ComboFailCount = 15;

	public float CannonPickupSlowMotionTime = 0.25f;

	bool trackCombo = true;
	public bool TrackCombo
	{
		get{return trackCombo;}
		set
		{
			trackCombo = value;
		}
	}
	
	
	#region Short Term Statistics
	
	int currentTargetsDestroyed = 0;
//	int currentGamePoints = 0;
	int currentAmountGems = 0;
	
	#endregion
	
	#region Game Statistics	
	
	//Here lies the player data.
	public PlayerData _playerDataPrefab;
	PlayerData _playerData = null;
	
	public PlayerData _PlayerData
	{
		get{ return _playerData;}
	}
	
	//Here we store the cache statistics.
	Statistics cacheStatistics;
	
	//Store the Game Run's Statistics
	Statistics gameRunStatistics = null;
	
	//This is for debugging and keepting track of all the statistics GOT created.
	public List<Statistics> cacheStatisticsList;
	
	//Here we have a local variable for access to social.
	SocialCenter socialCenter = null;
	
	
	
	int AbsoluteScore = 0;
	
	//Random Other stats such as time
	float StartGameTimeStamp = -1.0f;
//	static bool firstload = false;
	
	
	
	
	#endregion
	
	
	#region Local Calculations Numbers
	
	//Destroy another object within the timer to achieve a combo.
	public float ComboTimer = 3.0f;
	public float localTargetTimer = 0.0f;
	public int multiplierCount = 0;
	public int MaxMultiplierCount = 0;
	
	
	int popCount = 0;
	float popTimer = 0.0f;
	
	
	#endregion
	

	#region PowerUps for Stats
	public PowerupFactory.PowerUpsDirectoryType PU4TargetDestroyed = PowerupFactory.PowerUpsDirectoryType.DeathBeam;

	#endregion
		
	// Use this for initialization
	void Awake () {
		
		//This loads form the file system only at creation of the system.
		if(GOT == null)
		{
			Time.timeScale = 0.0f;
			
			GOT = this;
			
			if(EntityFactory.GetEF() == null)
			{
				Debug.Log("Factory Created");
				Instantiate(_entityFactoryPrefab);
				
			}
			
			if(_worldRef == null)
			{
				_worldRef = Instantiate(_WorldPrefab) as GameplayWorld;
				DontDestroyOnLoad(_worldRef.gameObject);
				
			}
			
			
			//Search for a game and visual world.
			Game =  FindObjectOfType(typeof(BaseGame)) as BaseGame;
			vWorld = FindObjectOfType(typeof(VisualWorld)) as VisualWorld;
			
		}
		
		
		
		//Create the lightning effect
		LightningEffect = Instantiate(LightningEffect) as ParticleSystem;
		
		//Create the gem dying effect.
		GemDieEffect = Instantiate(GemDieEffect) as ParticleSystem;
		
		//Create the combo effect.
		ComboEffect = Instantiate(ComboEffect) as ParticleSystem;
		
		
		
		//Get access to social center.
		socialCenter = GetComponentInChildren<SocialCenter>();
		//socialCenter.LoadLeaderboard("Highscore_PPS");
		
		
		
		//Lets look for the objects already in the scene to assign upon reload.
		foreach(Object o in FindObjectsOfType(typeof(PlayerData)))
		{
			if(o.name == "GOT_PlayerData")
			{
				//Assign player data 
				_playerData = (PlayerData)o;
				
				//Set the phase data 
				_playerData.Breathless = null;
				
				Debug.Log("Player Data Found");
				return;
			}
				
		}
		
		
		//If we are down here then we have found no player data and we will create one.
		_playerData = Instantiate(_playerDataPrefab) as PlayerData;
		Debug.Log("Player Data instantuated.");
		
		//Set the name and dont kill the player data.
		_playerData.name = "GOT_PlayerData";
		DontDestroyOnLoad(_playerData);
		
		//Load
		_playerData.LoadAllDataFromFileSystem();
		Debug.Log("Player Data Loaded Done");
			
		
	}
	
	
//	void Awake()
//	{
//		//DontDestroyOnLoad(this.gameObject);
//		
//		
//	}
	
	// Update is called once per frame
	void Update () {
	
		
		if(!vWorld)
		{
			vWorld = FindObjectOfType(typeof(VisualWorld)) as VisualWorld;
		}
		if(!Game)
		{
			Game = FindObjectOfType(typeof(BaseGame)) as BaseGame;
		}
		
		
		//Always update the combo timer .. .. ??
		localTargetTimer += Time.deltaTime;
		
		//Always update the time stamp.
		//StartGameTimeStamp += Time.deltaTime;
		
		//Update the timer for ball popping.
		popTimer += Time.deltaTime;
		
		if(localTargetTimer > ComboTimer)
		{
			
			//If we lose our combo at over 15.
			if(multiplierCount > ComboFailCount)
			{
				AudioPlayer.GetPlayer().PlaySound(ComboEffectSoundFail15x);
			}
			
			
			//If we are outside of the timer reset the multiplier count back to 0;
			multiplierCount = 0;
		}
		
		
		if(Input.GetKeyDown(KeyCode.U))
		{
			World.Nudge();
		}
		
		if(Input.GetKeyDown(KeyCode.I))
		{
			World.Twitch();
		}
		
		if(Input.GetKeyDown(KeyCode.O))
		{
			World.ShakeWorld();
		}
		
	}
	

	static public GameObjectTracker GetGOT()
	{
		if(GOT)
		{
			return GOT;
		}
		else
		{
			return null;
		}
	}
	
	static public GameObjectTracker instance
	{
		get{
		
			if(GOT)
		{
			return GOT;
		}
		else
		{
			return null;
		}
			
		}
		
		
	}
	
	
	public MainGameCamera GameCamera
	{
		get{
			return  _worldRef.GameCamera;
		}
	}
	
	public GameplayWorld World
	{
		get {
			return _worldRef;
		}
	}
	
	#region Events
	
	
	
	public void Dash()
	{
		//UpdateDataEntry(Statistics.EntryTypes.Dashed);
		
		//Slow down as we dash.
		ToyBox.GetPandora().SetSlowMotion(true,0.1f);
		
		
	}
	
	public void BlockedBall()
	{
		//We reset the target timer when we block a ball.
		ResetLocalTargetTimer();
		
		
		//Shake the world again...
		World.Twitch();
		
		UpdateDataEntry(Statistics.EntryTypes.ShotsBlocked);
		
		
		
	}
	public void CaptureBall()
	{
		//Reset the counter timer.
		ResetLocalTargetTimer();
		
		
		UpdateDataEntry(Statistics.EntryTypes.ShotsCaptured);
		ToyBox.GetPandora().SetSlowMotion(true, 0.07f);
	}
	public void MeleeShatter()
	{
		//Reset the counter timer.
		ResetLocalTargetTimer();
		
		//Play the world shake...
		World.ShakeWorld();
		
		UpdateDataEntry(Statistics.EntryTypes.MeleeShatterCount);
		ToyBox.GetPandora().SetSlowMotion(true, 0.12f);
	}
	public void MeleeHits()
	{
		//Reset the counter timer.
		ResetLocalTargetTimer();
		
		//Nudge the world
		World.Nudge();
		
		UpdateDataEntry(Statistics.EntryTypes.MeleePushCount);
	}
	public void DeflectedBall()
	{
		//Reset the counter timer.
		ResetLocalTargetTimer();
		World.Nudge();
		
		UpdateDataEntry(Statistics.EntryTypes.ShotsDeflected);
	}
	public void TargetHit()
	{
		UpdateDataEntry(Statistics.EntryTypes.PlayerShotsHit);
	}

	public void PlayerHit(float amount)
	{
		World.Nudge();

		//Register Statistics.
	}

	public void EnemyHit()
	{
		UpdateDataEntry(Statistics.EntryTypes.PlayerShotsHit);
	}
	public void TargetDetonated()
	{
		//TODO: Keep track of stat for this.
		if(multiplierCount > 0)
			multiplierCount--;
		
		//Plat twitch upon detonation.
		World.Twitch();
	}
	
	public void DeathBeamAttack()
	{
		//Update statistics
		UpdateDataEntry(Statistics.EntryTypes.DeathBeamKills);
		
		//Nudge the world.
		World.Nudge();
	}
	
	
	public void PlayerOverHeated()
	{
		UpdateDataEntry(Statistics.EntryTypes.TimesOverHeated);
		
		//Slow down the game as well.
		ToyBox.GetPandora().SetSlowMotion(true,0.45f);
		
		//Play the camera shake...
		World.ShakeWorld();
		//World.Twitch();

		//Tell the game homie.
		Game.BotOverheat();
		
		//Zoom in the camera
		ToyBox.GetPandora().Camera_01.DetachCannon(1.0f,1.0f);

	}
	public void PlayerShoot()
	{
		UpdateDataEntry(Statistics.EntryTypes.PlayerShotsFired);
	}
	
	public void AddMoney(Money m)
	{
		//UpdateDataEntry(Statistics.EntryTypes.GemsCollected,m.value);
		UpdateDataEntry(Statistics.EntryTypes.GemsCollected, GetMultiplier() );
		UpdateDataEntry(Statistics.EntryTypes.GemsCount);
		
		
		//Update the current amount of gems here.
		currentAmountGems += GetMultiplier();
		
		//We can affect the bank right here for instant saving!
		_PlayerData.GemBank += GetMultiplier();
		
		//Deactivate the money
		m.GemActive = false;
	}
	
	public void MoneyLost(Money m)
	{
		GemDieEffect.transform.position = m.transform.position;
		GemDieEffect.Play();
		
		//UpdateDataEntry(Statistics.EntryTypes.GemsLost,m.value);
		UpdateDataEntry(Statistics.EntryTypes.GemsLost,GetMultiplier() );
	}
	public void CannonPickedUp()
	{
		//Call the games cannon pick up.
		Game.CannonPickedUp();
		
		//Apply slow motion.
		ToyBox.GetPandora().SetSlowMotion(true, CannonPickupSlowMotionTime);
		
		//And Zoom out
		ToyBox.GetPandora().Camera_01.AttachCannon(1.0f,1.0f);


		ResetLocalTargetTimer();
		
	}
	
	public void WaveCompleted()
	{
		//Update statistics
		UpdateDataEntry(Statistics.EntryTypes.WavesCompleted);
	}
	
	public void BossWaveStart()
	{
		//Update statistics
		UpdateDataEntry(Statistics.EntryTypes.BossBattles);
		
		//Call the games BossWaveStart
		Game.BossStart();
		
	}
	public void BossWaveCompleted()
	{
		//Update statistics
		UpdateDataEntry(Statistics.EntryTypes.BossesDefeated);
		
		//Call the games boss level complete.
		Game.BossCompleted();
	}
	
	
	
	public void TargetDestroyed(Target t, bool forCombo = true)
	{
		//Add up the numbers
		
		//Statistics.Statistics[(int) Statistics.EntryTypes.TargetsDestroyed].ValueData++;
		UpdateDataEntry(Statistics.EntryTypes.TargetsDestroyed);
		
		// maybe spawn a powerup
		//Debug.LogError("Target Destroyed");
		//ToyBox.GetPandora().MyPowerUpSpawner.Spawn(t.transform, PU4TargetDestroyed);


		//Add the points
		//currentGamePoints += t.GetPointWorth() * Game.GetCurrentMultiplierModifier();
		//countTotalGamePoints += t.GetPointWorth() * Game.GetCurrentMultiplierModifier();
		
		//Track if we are in a multiplier 
		if(forCombo)
		if(localTargetTimer < ComboTimer && TrackCombo)
		{			
			multiplierCount += 1;
			
			//Keep track of our highest combo we have.
			if(MaxMultiplierCount <= multiplierCount)
			{
				MaxMultiplierCount = multiplierCount;
				//SetDataEntry(Statistics.EntryTypes.HighestCombo,MaxMultiplierCount);
			}
			
			
			//Play the combo effect.
			ComboEffect.transform.position = t.Position;
			ComboEffect.Play();
			
			//Change the color based on multiplier
			
			//Check if we are fast enough to play the lightning effect
			if(localTargetTimer < 0.2f)
			{
				LightningEffect.transform.position = t.Position;
				LightningEffect.Play();
			}
			
			if(multiplierCount > 10)
			{	
				//Play sound ShootSound
				//audio.PlayOneShot(ComboEffectSound15x);
				AudioPlayer.GetPlayer().PlaySound(ComboEffectSound15x);
				
			}
			
			
		}
		
		//Set the timer back to 0 every time a target is destroyed.
		ResetLocalTargetTimer(false);
		
	
		
		//Call the games target destroy message and pass down T
		Game.TargetDestroyed(t);
	}
	
	public void BallPop()
	{
		
		
		//If we are beyond the popping increment time start from ground level.
		if(popTimer > 1.0f)
		{
			popCount = 0;
		}
		
		if(popCount <= Pops.Length - 1)
		{
			//popCount = Pops.Length - 1;
			
			AudioClip cur = Pops[popCount];
			
			//Play the sound file at pop count.
			AudioPlayer.GetPlayer().PlaySound( cur );
			
			
//			Debug.LogError("RAANGE: " + Pops.Length + " and count: " + popCount  );
			
			//Increment the pop counter and reset the timer
			popCount++;
			
		}
		
		
		popTimer = 0.0f;
	}
	
	public void HealthCC(float amount)
	{
		UpdateDataEntry(Statistics.EntryTypes.HealthHealed,(int)amount);
	}
	public void CoolDownBot(float amount)
	{
		UpdateDataEntry(Statistics.EntryTypes.CoolDownPoints,(int)amount);
	}

	public void DeflectHit()
	{
		UpdateDataEntry(Statistics.EntryTypes.DeflectHits);
	}
	
	public void DeflectKills()
	{
		
	}
	
	public void CaptureHits()
	{
		UpdateDataEntry(Statistics.EntryTypes.CaptureHits);
			
	}
	
	public void MeleeCannon()
	{
		//Reset the counter timer.
		ResetLocalTargetTimer();

		//Nudge the world
		World.Nudge();

		Game.CannonPushed();

	}
	public void EmptyShatter()
	{
		//Nudge-us
		World.Nudge();
	}
	
	public void CaptureKills()
	{
		UpdateDataEntry(Statistics.EntryTypes.CaptureKills);
	}
	
	public void WallKill()
	{
		UpdateDataEntry(Statistics.EntryTypes.PushKills);
	}
	
	public void PushHits()
	{
		UpdateDataEntry(Statistics.EntryTypes.PushHits);
	}
	
	public void PushKills()
	{
		UpdateDataEntry(Statistics.EntryTypes.PushKills);
	}
	public void CCDamage()
	{
		UpdateDataEntry(Statistics.EntryTypes.CCDamage);
	}
	
	public void BallKnock()
	{
		UpdateDataEntry(Statistics.EntryTypes.BallKnocks);
	}
	
	public void BotDamage()
	{
		World.Twitch();

		//UpdateDataEntry(Statistics.EntryTypes.h);
	}
	
	
	#endregion
	
	
	#region Functions

	// Universal call to reset the combo timer
	private void ResetLocalTargetTimer(bool notify = true){
		// sets the localTargetTimer to 0 
		// tell the bot that its combo timer has been reset
		localTargetTimer = 0.0f;

		//If we choose to call the notify grab it form the bot and call the funciton.
		if(notify)
		{
			Bot tempBot = ToyBox.GetPandora().Bot_01;
			tempBot.BotNotificationHUB.PlayCounterResetAnimation();
		}



	}


	
	/// <summary>
	/// Starts the game. Resets all in game statics to keep track of.
	/// </summary>
	public void StartGame()
	{
		//Reset the current targets destroyed.
		currentTargetsDestroyed = 0;
		currentAmountGems = 0;
		multiplierCount = 0;
		
		StartGameTimeStamp = Time.time;
		MaxMultiplierCount = 0;
		
		
		//Create the gameruns statistics and start keeping track
		if(!gameRunStatistics)
		{
			//Create from a blank template.
			//gameRunStatistics = PushStatistics("TotalGameRUNStats");
			
			string label = "TotalGameRUNStats";
			
			//If we dont have one created then we create one and return it.
			gameRunStatistics = Instantiate(_PlayerData.BlankStatistics) as Statistics;
			
			//Set the name and the label of the statistics
			gameRunStatistics.ContainerLabel = label;
			gameRunStatistics.name = label;
			
			//Set the cannon type from the start.
			gameRunStatistics.CompletionCannon = EntityFactory.CannonTypes.Empty;
		}
		
		
	}
	
	public void StopGame(Statistics stats = null )
	{
		
		//Stop the timer
		cacheStatistics.StopTimer();
		
		//Set teh games stats from the stats tracked by the game.
		gameRunStatistics = stats;
		
		//Add games played.
		UpdateDataEntry(Statistics.EntryTypes.GamesPlayed);
		SetDataEntry(Statistics.EntryTypes.HighestCombo,MaxMultiplierCount);
		
		if(stats == null)
			return;
		
		//Lets apply the game stats to player data at game over.
		_PlayerData.ApplyRunStatsToCards(stats);
		
		//Report Score of the game to the social center. 
		socialCenter.ReportRun(stats);
		
		
	}
	
	public int GetCurrentTargetsDestroyedInGame()
	{
		return currentTargetsDestroyed;
	}
	
	public int GetCurrentPoints()
	{
		if(!cacheStatistics)
			return 0;
		
		//Add up the current game score plus the score of the current phase.
		int score = AbsoluteScore + cacheStatistics.Score;
		
		//Here is where absolutescore would be replaced by 
		//int score = Game.RunStatistics.Score + cacheStatistics.Score;
		
		//return _PlayerData.GameStatistics.GetScore();
		return score;
	}
	
	public int GetMultiplier()
	{
		return multiplierCount;
	}
	
	public float GetComboAlpha()
	{
		return 1.0f - (GetMultiplierTimer() / ComboTimer);
	} 
	public float GetMultiplierTimer()
	{
		return localTargetTimer;
	}
	
	public void ResetMultiplier()
	{
		multiplierCount = 0;
	}
	public int GetGemsCollected()
	{	
		return currentAmountGems;
	}
	
	
	
	
	
	
	//Create a push function that returns new cache and stores on stack if previous available
	// Pass in the string to name the closing statistics.
	//Pass in the multiplier to write to the stats comming out.
	public Statistics PushStatistics(string label = "n/a", int scorescale = 1)
	{
		
		//If we already have a cache stats, push it to the list and return it.
		if(cacheStatistics)
		{
			//Add the cached stats to the list.
			cacheStatisticsList.Add(cacheStatistics);
			
			//Add the score of the stats being pushed away.
			AbsoluteScore += cacheStatistics.Score;
			
			//Stop the timer
			cacheStatistics.StopTimer();
			
//			if(gameRunStatistics)
//			{
//				//Then we add to game statistics if available as er pop these stats out. 
//				gameRunStatistics.AddStatistics(cacheStatistics);	
//			}
			
			
		}
		
		
		//If we dont have one created then we create one and return it.
		cacheStatistics = Instantiate(_PlayerData.BlankStatistics) as Statistics;
		
		//Set the name and the label of the statistics
		cacheStatistics.ContainerLabel = label;
		cacheStatistics.name = label;
		
		//Set the score multiplier
		cacheStatistics.ValueMultiplier = scorescale;
		
		
		//We will set the time here untill we figure out how to put it int he start fucntion of stats.
		//cacheStatistics.TimeAmount = Time.time;
			
		return cacheStatistics;
	}
	
	//A function to grab the Game's Statistics
	public float GamePPS
	{
		get
		{
			//Calculate.
			float pps = (float)GetCurrentPoints();
			
			
			return (pps/GameTime);
		}
	}
	
	public float GameTime
	{
		get
		{
			if(StartGameTimeStamp == -1.0f)
				return -1.0f;
			
			return (Time.time - StartGameTimeStamp);
		}
	}
	
	//Grab the game runs statistics
	public Statistics RunStatistics
	{
		get
		{
			if(gameRunStatistics)
			return gameRunStatistics;
			
			
			
			return _PlayerData.GameStatistics;
		}
	}
	
	
	//A function to grab the statistics
	public Statistics FullStatistics
	{
		get
		{
			return _PlayerData.GameStatistics;
		}
	}
	
	
	public void SaveButtonPushed()
	{
		//We will try to call save full statistics function over here.
		FullStatistics.SaveToPlayerPrefs();
		
		//Save the mission data as well. fuck it.
		_PlayerData.SaveItemCardData();
		
		
		//Sure we can do it here for now.
	//	PlayerPrefs.Save();
	}
	
	public void RestartButtonPushed(bool Isintro = false)
	{
		//Just pop the track to avoid boss music and other random tack pops to still be playing.
		AudioPlayer.GetPlayer().PopTrack();

		//Delete the game and its phase.
		//Game.Clear();
		
		//I think we should pause the game here to avoid anything happening during reload.
		ToyBox.GetPandora().TimePaused = true;

		//Reset the CC Health.
		ToyBox.GetPandora().CommandCenter_01.Reset();
		//Reset the world.
		_worldRef.Reset();
		
		//Reset teh entity factory.
		EntityFactory.GetEF().Reset();
		
		ActivityManager.Instance.Reset();
		
		_PlayerData.SaveAllData();
		//_playerData.ClearGameSlots();
		
		Reset();

		//Make sure we are counting the combo
		TrackCombo = true;

//		//Upon restart, if the game world is not active we actiavte it.
//		if(_worldRef.gameObject.activeSelf == false)
//			_worldRef.gameObject.SetActive(true);

		if(Isintro == true)
		{
			//Deactivate the activity manager.




			Application.LoadLevel("Intro");

			//Deactivate the gameplay room.ga
			_worldRef.gameObject.SetActive(false);



			return;
		}
	
		if(_playerData.IsTutorial)
		{
			Application.LoadLevel("Tutorial");
			return;
		}


		Application.LoadLevel("TestLevel");
		
	}
	
	
	public void DeleteButtonPushed()
	{
		_PlayerData.DeleteAllData();
	}
	
	public void GameCenterButtonPushed()
	{
		socialCenter.ShowLeaderboard();
	}
	
	#endregion
	
	#region Internal Functions
	
	void Reset()
	{
		//Null this out upon reset.
		gameRunStatistics = null;

		//Clear the active slots here.


		//Reload the leaderboard upon reset...
		SocialCenter.Instance.LoadLeaderboard("Highscore_PPS");
	}
	
	void UpdateDataEntry(Statistics.EntryTypes type, int amount = 1)
	{
		//Update the player data statistics
		_PlayerData.GameStatistics.UpdateDataEntry(type,amount);
		
		//Update the game runs statistics.
		if(gameRunStatistics)
			gameRunStatistics.UpdateDataEntry(type,amount);
		
		//Update the cache statistics
		if(cacheStatistics)
			cacheStatistics.UpdateDataEntry(type,amount);

//		if(cacheStatistics)
//		_counteRref.AddStatPoint(cacheStatistics.GetStatPointWorth(type));
//		
		
	
	}
	
	//Updating the game statistics as well as the cache statistics.
	void SetDataEntry(Statistics.EntryTypes type, int number)
	{
		//Update the player data statistics
		_PlayerData.GameStatistics.SetDataEntry(type,number);
		
		//Update the game runs statistics.
		if(gameRunStatistics)
			gameRunStatistics.SetDataEntry(type,number);
		
		//Update the cache statistics
		if(cacheStatistics)
		cacheStatistics.SetDataEntry(type,number);
	
	}
	
	
	#endregion
	
	
}
