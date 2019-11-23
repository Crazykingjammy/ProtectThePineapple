using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePhase : MonoBehaviour {
	
	
	/// <summary>
	/// The Phase Struct.
	/// </summary>
	[System.Serializable]
	public class PhaseData
	{
		//Phase punch time
		public float PhaseCompletionPunch = -1.0f;
		
		//String for the texture name.
		public int ComboToComplete;
		public int BossesToComplete;
		public int WavesToBoss;
		public int ScoreMultiplier;
		
		public string IconTextureName;
		public string Description;
		public string Label;
		
		//Statistics for the wave. Filled up upon time stamping.
		public Statistics PhaseStatistics;
		
	}
	
	
	//The flag to determine if the phase has been completed.
	protected bool Completed = false;
	
	//If we are on boss waves or not.
	protected bool Boss = false;
	
	//Our current wave.
	BaseWave currentWave;
	int wavesDestroyedCount = 0;
	int totalWavesDestroyed = 0;
	int bossWaveCount = 0;
	float phaseLifeTime = 0.0f;
	PhaseData data;


	//List of Waves
	public List<BaseWave> MeatWaves;
	public List<BaseWave> BossWaves;
	
	//Basic numbers for completion
	public int ComboCompletionRequirement = 10;
	public int BossCompletionRequirement = 3;
	public int WaveCountForBoss = 20;
	public int ScoreMultiplier = 1;
	
	//Other variables
	public AudioClip PhaseSoundtrack;
	public AudioClip BossSoundtrack;
	public bool TransitionLevel = true;
	
	//Description and Texture Files
	public string Title = "BasePhaseTitle";
	public string Description = "Phase Description String. And that is pretty much it. Or is it? Codes? Clues? What?";
	public string IconTextureName = "Apple-icon";
		
	// Use this for initialization
	protected virtual void Start () {
	
		//Add a phase to the statistics every time we create a new phase.
		GameObjectTracker.GetGOT()._PlayerData.AddPhase(CopyPhaseData());
		
		//Play the audio track.
		AudioPlayer.GetPlayer().PlayAudioClip(PhaseSoundtrack);
		
		
		
	}
		
	
	// Update is called once per frame
	protected virtual void Update () {
	
		//Update the phase time 
		phaseLifeTime += Time.deltaTime;
		
		
		
	}
	
	//Ask if we are finished.
	public bool IsComplted()
	{
		if(Completed && currentWave.IsCompleted())
		{
			return true;
		}
		
		return false;
	}
	public bool IsCurrentWaveCompleted()
	{
		return currentWave.IsCompleted();
	}
	public int GetWavesCompleted()
	{
		return wavesDestroyedCount;
	}
	public int GetScoreMultiplier()
	{
		return ScoreMultiplier;
	}
	public float GetPhaseTime()
	{
		return phaseLifeTime;
	}
	//Accessor for the phase data.
	public PhaseData Data
	{
		get
		{
			return data;
		}
	}
	//This is called from the game or who has access to the phase and wants to stop music. 
	public void StopMusic()
	{
		//audio.Stop();
		AudioPlayer.GetPlayer().StopMusic();
	}
	
	protected BaseWave CreateWave()
	{
		//Randomize an index 
		int i = Random.Range(0,MeatWaves.Count);
		
		//Set the wave equal to the brand new wave from the list.
		return Instantiate(MeatWaves[i]) as BaseWave;
	}
	
	protected BaseWave CreateBossWave()
	{
		//Randomize an index 
		int i = Random.Range(0,BossWaves.Count);
		
		//Set the wave equal to the brand new wave from the list.
		return Instantiate(BossWaves[i]) as BaseWave;
	}
	
	protected void HandleWaveCreation()
	{
		if(!currentWave)
		{
			//If boss flag is true spawn a boss wave!
			if(Boss)
			{
				//Create a boss wave.
				currentWave = CreateBossWave();
				
				//Send the message for boss started
				GameObjectTracker.GetGOT().BossWaveStart();
				
				//Store the time off the current music so we can resume.
				//currentaudiotime = AudioPlayer.GetPlayer().audio.time;
				
				//Set and play the boss music.
				//AudioPlayer.GetPlayer().PlayAudioClip(BossSoundtrack);
				
				//Push the boss sound track.
				AudioPlayer.GetPlayer().PushTrack(BossSoundtrack);
				
				return;
			}
			
			currentWave = CreateWave();
		}
		
		if(currentWave.IsCompleted())
		{
			//If it was a boss wave do boss clean up stuff here.
			if(Boss)
			{
				//turn boss off.
				Boss = false;
				
				//Incriment the count of bosses destroyed.
				bossWaveCount++;
				
				//Send the message for boss being completed
				GameObjectTracker.GetGOT().BossWaveCompleted();
				
				//Return to our regular music programming.
				//AudioPlayer.GetPlayer().PlayAudioClip(PhaseSoundtrack,currentaudiotime);
				
				//Pop from the boss track.
				AudioPlayer.GetPlayer().PopTrack();
			}
			
			//print("Wave Completed!");
			Destroy(currentWave.gameObject);	
			wavesDestroyedCount++;
			totalWavesDestroyed++;
			
			//Send the message for wave being completed
			GameObjectTracker.GetGOT().WaveCompleted();
			
		}
	}
	
	//Checks the combo required to mark the phase as complete.
	protected void CheckComboCompletion()
	{
		//Get the GOT's multiplier count and check if we reached the combo required.
		if(GameObjectTracker.GetGOT().GetMultiplier() >= ComboCompletionRequirement
			|| bossWaveCount >= BossCompletionRequirement)
		{
			Completed = true;
		}
	}
	
	//Checks if we have reached our destroyed wave count to spawn some boss waves.
	protected void CheckWaveCountsForBoss()
	{
		//If our count reaches the set amount we set boss = to true;
		if(wavesDestroyedCount >= WaveCountForBoss)
		{
			Boss = true;
			
			//Reset the waves destroyed count then!
			wavesDestroyedCount = 0;
		}
	}
	
	
	PhaseData CopyPhaseData()
	{
		//Copy over the phase data to the struct for player data.
		data = new PhaseData();
		
		data.ComboToComplete = ComboCompletionRequirement;
		data.BossesToComplete = BossCompletionRequirement;
		data.WavesToBoss = WaveCountForBoss;
		data.ScoreMultiplier = ScoreMultiplier;
		data.Description = Description;
		data.IconTextureName = IconTextureName;
		
		data.Label = "Phase Name: " + this.name;
		
		data.PhaseStatistics = GameObjectTracker.GetGOT().PushStatistics("PhaseStat_" + this.name,ScoreMultiplier);
		
		//Set the name of the phase so we can find in the scene view easily.
		name = "GamePhase_" + name;
		
		return data;
		
	}
	
	
	void OnDestroy()
	{
		//Do a check for GOT incase its not there.
		if(!GameObjectTracker.GetGOT())
			return;
		
		if(currentWave)
		{
			//If we are destroyed at a boss level that means we have updated to a new phase while boss still exists 
			//or updated via boss battle itself.
		
			if(Boss)
			{
				//Send the message for boss being completed
				GameObjectTracker.GetGOT().BossWaveCompleted();	
				
				//Pop the music.
				AudioPlayer.GetPlayer().PopTrack();
			}
			
			
			//Clean up our waves upon our destruction
			Destroy(currentWave.gameObject);		
			
			
		}
		
		
		
	}
			
	
	
}
