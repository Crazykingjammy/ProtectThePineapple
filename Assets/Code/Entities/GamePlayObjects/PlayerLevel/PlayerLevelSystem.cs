using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//This class will handle all the data neeeded to figure out the player level.
// Essentally this is a class to calcualte the levels based on the prefab data.
//All that should be needed to feed into this class is the total XP count.
public class PlayerLevelSystem : MonoBehaviour {

	//Internal count to calcualte levels based on the total xp value.
	int _totalXP = 0;

	//Internal values for calculations
	int currentXP, remainingXP, currentLevel = 0;
	int currentCompletionXP;

	float currentLevelPercent = 0.0f;
	float totalLevelPercent = 0.0f;

	[System.Serializable]
	public class PlayerLevelData
	{
		public string Title = "Title";
		public int CompletionXP;
	}

	public class BeforeCurrentData
	{
		public int XP, RemainingXP, Level = 0;
		public float LevelPercent = 0.0f;

	}


	BeforeCurrentData _beforeCurrent = null;



	public List<PlayerLevelData> PlayerLevels;

	public BeforeCurrentData BeforeCurrent
	{
		get{return _beforeCurrent;}
	}

	public int TotalXP
	{
		get{return _totalXP;}
		set
		{
			//Set the total xp here and do some calculations.
			_totalXP = value;

			CalculateData();
		}
	}

	public int LevelsCount
	{
		get {
			return PlayerLevels.Count;
		}
	}

	//Returns the calculated value of the current level's XP 
	public int CurrentLevelXP
	{
		get{
			return currentXP;
		}
	}

	public int CurrentLevelCompletionXP
	{
		get{
			return currentCompletionXP;
		}
	}


	public float CurrentLevelCompletionPercent
	{
		get 
		{
			return currentLevelPercent;
		}
	}

	public float TotalLevelProgressCompletionPercent
	{
		get
		{
			return totalLevelPercent;
		}
	}

	//Calculated XP needed untill next level is reached.
	public int XPToNextLevel
	{
		get{
			return remainingXP;
		}
	}

	//Calculates the current level based on the total XP obtained.
	public int Level
	{
		get{
			return currentLevel;
		}
	}

	void SetBeforeData()
	{
		//Set the values
		_beforeCurrent.XP = currentXP;
		_beforeCurrent.Level = Level;
		_beforeCurrent.LevelPercent = CurrentLevelCompletionPercent;
		_beforeCurrent.RemainingXP = XPToNextLevel;

	}


	void CalculateData()
	{
		int fullxp = TotalXP;
		int levelindex = 0;

		//Set the before data here
		SetBeforeData();

		foreach(PlayerLevelData data in PlayerLevels)
		{
			//Keep subtracting the xp from the current level.
			if(fullxp >= data.CompletionXP)
			{
				fullxp -= data.CompletionXP;

				//Every time we have enough to subtract form total we up a level.
				levelindex++;
			}
			else
			{
				//here is where we dont have enough xp left to up another level.
				currentXP = fullxp;

				//Calculate reminaing from the levels completion xp subracted from what we already have.
				remainingXP = data.CompletionXP - currentXP;

				//Set the level. It should be the index we are currently on.
				currentLevel = levelindex;

				//Set current level completion XP
				currentCompletionXP = data.CompletionXP;

				//Calculate the percent
				currentLevelPercent = ( (float)currentXP ) / ( (float)data.CompletionXP );

				totalLevelPercent =  ( (float)currentLevel) / ( (float)LevelsCount );

				//Since this else came in we dont want to go through adn iterate through the rest of the levels.
				return;
			}


		}
	}

	// Use this for initialization
	void Awake () {
	
		_beforeCurrent = new BeforeCurrentData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
