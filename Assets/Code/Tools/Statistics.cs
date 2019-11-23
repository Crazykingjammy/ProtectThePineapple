using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Prime31;

public class Statistics : MonoBehaviour {
	
	#region Data Structs
	
	public enum EntryTypes
	{
		PlayerShotsFired,
		PlayerShotsHit,
		HitPercentage,
		TargetsDestroyed,
		GemsCollected,
		GemsLost,
		GemsCount,
		GemsPercentage,
		HighestCombo,
		TimesOverHeated,
		ShotsBlocked,
		ShotsDeflected,
		ShotsCaptured,
		MeleePushCount,
		MeleeShatterCount,
		DeathBeamKills,
		HealthHealed,
		CoolDownPoints,
		WavesCompleted,
		BossBattles,
		BossesDefeated,
		DeflectHits,
		DeflectKills,
		CaptureHits,
		CaptureKills,
		PushHits,
		PushKills,
		CCDamage,
		BallKnocks,
		GamesPlayed,
		
		EntryTypesCount
		
	}
	
	[System.Serializable]
	public class DataEntry
	{
		public string DataLabel = "N/A";
		
		public string BeginningLabel = "Get";
		public string CheckLabelg = "More than";
		public string CheckLabell = "Less than";
			
		public int EntryValue = 5;
		public  EntryTypes EntryType;
		
		 int ValueData = 0;
		
		
		public int Data
		{
			get {return ValueData;}
		}
		
		public DataEntry(string name, int data, EntryTypes type)
		{
			DataLabel = name;
			ValueData = data;
			EntryType = type;
		}
		
		public void UpdateData(int amount = 1)
		{
			ValueData += amount;
		}
		
		public void SetData(int number )
		{
			ValueData = number;
		}
		
		//Write to player prefs function
		public void WriteToPlayerPrefs(string statsName)
		{
			
			FileFetch.SetInt(statsName + "_EntryDataValue_" + DataLabel,EntryValue);
			FileFetch.SetInt(statsName + "_EntryType_" + DataLabel,(int)EntryType);
			FileFetch.SetInt(statsName + "_EntryData_" + DataLabel,ValueData);
			
		}
		
		public void ReadFromPlayerPrefs(string statsName)
		{
		
			EntryValue = FileFetch.FetchInt(statsName + "_EntryDataValue_" + DataLabel);
			EntryType = (Statistics.EntryTypes)FileFetch.FetchInt(statsName + "_EntryType_" + DataLabel);
			ValueData = FileFetch.FetchInt(statsName + "_EntryData_" + DataLabel);
		}
		
		public void WriteToDictionary(IDictionary dict)
		{
			dict.Add("_EntryDataValue_" + DataLabel,EntryValue);
			dict.Add("_EntryType_" + DataLabel,(int)EntryType);
			dict.Add("_EntryData_" + DataLabel,ValueData);
		}
		
		public void LoadFromDictionary(Dictionary<string,int> dict)
		{
			
			EntryValue = dict["_EntryDataValue_" + DataLabel] ;
			EntryType = (Statistics.EntryTypes)dict["_EntryType_" + DataLabel];
			ValueData = (int)dict["_EntryData_" + DataLabel];
			
		}
		//plus operator for the data entry
		
		
	}
	
		
	[System.Serializable]
	public class EntryGate
	{
		public int ValueTest = 0;
		public Statistics.EntryTypes type;
		public bool bGreater = true;
	}
	
	//Stats Gate class to test against a statistics class to see if the conditions are met.
	[System.Serializable]
	public class StatGate
	{
		//Outside stats that are not part of the data set.
		public float PPS = -1.0f;
		public float timeAmount = -1.0f;
		public int Score = -1;
		
		//Bools to test if greater than gate value.
		public bool bPPS = true;
		public bool btimeAmount = false;
		public bool bScore = true;
		
		
		
		//The list of data types to test against.
		public EntryGate[] StatsTest;
		
		
	}
	
	
	#endregion
	
	
	#region Data
	
	//The array of data entries.
	public DataEntry[] _statisticsEntries;
	
	//The amount of time spent.
	float TotalTimePlayed = -1.0f;
	float startTime = 0.0f;
	float endTime = -1.0f;
	
	//Int for holding the score.
	int StatsScore = 0;
	
	//The score modifier.
	int StatsValueModifier = 1;
	
	//Store the cannon we completed with.
	EntityFactory.CannonTypes completedCannonType = EntityFactory.CannonTypes.Empty;
	
	//Assigned label
	public string ContainerLabel = "Blank";

	
	#endregion
	
	
	// Use this for initialization
	void Start () {
	
		startTime = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	#region Functions
	
	//Returns value between 0 - 1 for percent matched.
	public float StatTest(StatGate gate)
	{
		//Create the percentage float to start with.
		float percentage = 0.0f;
		
		//Percentage scale values.
		float ppsPercentageScale = 10.0f;
		float timePercentageScale = 10.0f;
		float scorePercentageScale = 10.0f;
		
		//mark true if we are checking for stats outside of the data entries.
		bool dirtyStat = false;
		
		//Calculate Percentages
		
		
		//Check if we need to even check the item.
		if(gate.PPS != -1.0f)
		{
			dirtyStat = true;
			
			//Then we apply the type of check to test against.
			if(gate.bPPS)
			{
				if(PPS >= gate.PPS)
					percentage += ppsPercentageScale;
			}
			else
			{
				if(PPS <= gate.PPS)
					percentage += ppsPercentageScale;
			}
		}
		
		if(gate.timeAmount != -1.0f)
		{
			dirtyStat = true;
			
			//We check if time is -1 when we are checking for less than time as -1 means no entry.
			if(gate.btimeAmount )
			{
				if(TimeAmount >= gate.timeAmount)
					percentage += timePercentageScale;			
			}
			else{
				
				if(TimeAmount <= gate.timeAmount && endTime != -1.0f)
					percentage += timePercentageScale;
			}
		}
	
		
		//Check if we need to even check the item.
		if(gate.Score != -1)
		{
			dirtyStat = true;
			
			//Then we apply the type of check to test against.
			if(gate.bScore)
			{
				if(Score >= gate.Score)
					percentage += scorePercentageScale;
			}
			else
			{
				if(Score <= gate.Score)
					percentage += scorePercentageScale;
			}
		}
		

			
		//Here we will see if we have any stat gates to test against.
		//If there are no stat gates in the list. then we have to judge completion with out it.
		//To do that, i am checking if the percentage is anything other than 0.
		//if it is, and if the stat gate checks for any of the above just return 100%
		//This means that if there are no stat gates, we can only test against 1 of the 3 outside variables (score, pps or time)
		//Then we continue to return anyway if stat length is 0 to avoid diviging by 0 in the gate worth calculation function.
		if(gate.StatsTest.Length == 0)
		{
			if( (percentage > 0.0f) &&  (gate.Score != -1 || gate.PPS != -1.0f || gate.timeAmount != 1.0f)  )
			{
				return 100.0f;	
			}
			
			return percentage;
		}
			
		
		//Calculate the remainder percentages to help determine gate worth.
		float remainder = 100.0f - percentage;
		
		//if we had to check for any of the upper stats and failed the remainder would still be 100.0f
		//So we just do this dirty check and set the remainder to at least 90%
		if(dirtyStat && remainder >= 100.0f)
			remainder = 90.0f;
			
		
		
		//Devide the remaining 
		float gateworth = ( (remainder) / gate.StatsTest.Length );
		
		//Here we check against the stats.
		foreach(EntryGate eg in gate.StatsTest)
		{
			//If we testing against greater.
			if(eg.bGreater)
			{
				//Test against the stat with the states in the gate.
				if(GetEntryData(eg.type).Data >= eg.ValueTest)
					percentage += gateworth;
			}
			else{
				
				//Test against the stat with the states in the gate.
				if(GetEntryData(eg.type).Data <= eg.ValueTest)
					percentage += gateworth;
			}
			
	
		}
		
		
		//If we pass all the above tests then we return true!
		return percentage;
	}
	
	
	public void SaveToPlayerPrefs()
	{
		//Write the header to see if this stats is in the player prefs.
		//FileFetch.SetString(name,name);
	
		IDictionary save = new Dictionary<string,int>();
		
		//Write each data entry.
		foreach(DataEntry data in _statisticsEntries)
		{
			//data.WriteToPlayerPrefs(this.name);
			data.WriteToDictionary(save);
		}
		
		//Write the total time played.
		FileFetch.SetFloat(name + "_TotalTime",TimeAmount);
		
		//Write the score
		//FileFetch.SetInt(name + "_StatsScore", StatsScore);
		save.Add(name +  "_StatsScore", StatsScore);
		
		//Save the multiplier as well.... just incase.
		//FileFetch.SetInt(name + "_Modifier", StatsValueModifier);
		save.Add(name + "_Modifier", StatsValueModifier);
		
		
		
		FileFetch.SetDictionary(name,save);
		
		
	//	print("Data Write Function Complete: " + this.name);
	}
	
	
	//Returns false if entry is not found.
	public bool LoadFromPlayerPrefs()
	{
		//Check if we have somthing to load
		if(!FileFetch.HasKey(name) )
		{
			return false;
		}
		
		
		//Here we create a ductionary to fill up from the iDictionary.
		Dictionary<string,int> load =  new Dictionary<string,int>();
		IDictionary dict = FileFetch.FetchDictonary(name);
		
		
		
		//Transfer the data.
		foreach(DictionaryEntry data in dict)
		{
			load.Add(data.Key as string,int.Parse(string.Format("{0}",data.Value)));
		}
			
		//Get the data from the files.
		foreach(DataEntry data in _statisticsEntries)
		{
			//data.ReadFromPlayerPrefs(this.name);
			data.LoadFromDictionary(load);
		}
		
		//Get the total time player.
		TimeAmount = FileFetch.FetchFloat(name + "_TotalTime");
		
		//Write the score
		//StatsScore = FileFetch.FetchInt(name + "_StatsScore");
		StatsScore = load[name + "_StatsScore"];
		
		
		//Save the multiplier as well.... just incase.
		//StatsValueModifier = FileFetch.FetchInt(name + "_Modifier");
		StatsValueModifier = load[name + "_Modifier"];
		
		
		
		return true;
	}
	

	
	public void AddStatistics(Statistics stats)
	{
		//Add up the Data entries.
		foreach(DataEntry data in _statisticsEntries)
		{
			//Exceptions
			
			//We dont add the highest combo part. 
			if(data.EntryType == EntryTypes.HighestCombo)
			{
				int incommingCombo = stats.GetEntryData(EntryTypes.HighestCombo).Data;
				
				//Test to see which combo is higher and set it.
				if(incommingCombo >= data.Data )
					data.SetData(incommingCombo);
				
			}
				
			
			//Add up the counts.
			data.UpdateData(stats.GetEntryData(data.EntryType).Data);
			
			//No adding of values needed. This data is lost. 	
			
		}
		
		//If this == incomming, do nothing, check nothing.
		//If this is empty write the incomming.
		//If this != incomming and not empty, this is null.
		
		
		//Only overriting the current cannon type if we pass a few checks.
		if(CompletionCannon == EntityFactory.CannonTypes.Empty)
		{
			Debug.Log("Stat: " + this.name + " is empty and was taken over by " + stats.name);
			CompletionCannon  = stats.CompletionCannon;
		}
		
		if(CompletionCannon != stats.CompletionCannon && stats.CompletionCannon != EntityFactory.CannonTypes.Empty)
		{
			Debug.Log("Stat: " + stats.name + " is NOT empty and nullified stat: " + this.name );
			CompletionCannon = EntityFactory.CannonTypes.NULL;
		}

				
		//Add up Total Score at the values of the passed in stats.
		StatsScore += stats.Score;
		
		
		//Add up Time played.
		TotalTimePlayed += stats.TimeAmount;
	
		
	//	print("Stats added: " + stats.name + " was added to " + name);

	}
	
	public DataEntry GetEntryData(EntryTypes type)
	{
		//If we are asking for accuracy we have to calculate it
		if(type == Statistics.EntryTypes.HitPercentage)
		{
			//Get the data we need (ball shit. hehe)
			int ballsfired = _statisticsEntries[(int) EntryTypes.PlayerShotsFired].Data;
			int ballshit =   _statisticsEntries[(int) EntryTypes.PlayerShotsHit].Data;
			
			float accuracy = 0.0f;
			
			if(ballsfired != 0)
			{
				//Calculate the accuracy here.
				accuracy = ((float)ballshit / (float)ballsfired) * 100.0f;	
			}
			
			
			//Set the data in teh statistics before returning.
			_statisticsEntries[(int) EntryTypes.HitPercentage].SetData((int)accuracy);
			
		}
		
		//If we are asking for gems percentage we calculate it here.
		if(type == Statistics.EntryTypes.GemsPercentage)
		{
			//Get the data we need (ball shit. hehe)
			int ballsfired = _statisticsEntries[(int) EntryTypes.GemsCollected].Data + _statisticsEntries[(int) EntryTypes.GemsLost].Data;
			int ballshit =   _statisticsEntries[(int) EntryTypes.GemsLost].Data;
			
			float accuracy = 0.0f;
			
			if(ballsfired != 0)
			{
				//Calculate the accuracy here.
				accuracy = ((float)ballshit / (float)ballsfired) * 100.0f;	
			}
			
			
			//Set the data in teh statistics before returning.
			_statisticsEntries[(int) EntryTypes.GemsPercentage].SetData((int)accuracy);
			
		}
		
		
		//Return the data being asked for.
		return _statisticsEntries[(int)type];
		
	}
	
	public int GetStatPointWorth(Statistics.EntryTypes type)
	{
		return _statisticsEntries[(int)type].EntryValue;
	}
	
	
	public int GetScore(bool multiplier)
	{	
		return StatsScore;
	}
	
	public int ReCalculateScore()
	{
		//Reset the stats score value.
		int score = 0;
		
		//Iterate throught the list and add the values.
		foreach(DataEntry de in _statisticsEntries)
		{
			score += (StatsValueModifier * de.EntryValue) * de.Data;
		}
		
		return score;
	}
	
	public void UpdateDataEntry(EntryTypes type, int amount = 1)
	{
		//Update the data.
		_statisticsEntries[(int)type].UpdateData(amount);
		
		//Update the final score as well.
		StatsScore += _statisticsEntries[(int)type].EntryValue * (StatsValueModifier);
	}
	
	public void SetDataEntry(EntryTypes type, int number)
	{
		//Exception for highest combo, need not set if the nubmer is lower than current.
		if(type == EntryTypes.HighestCombo)
		{
			if(_statisticsEntries[(int)type].Data > number)
				return;
		}
		
		_statisticsEntries[(int)type].SetData(number);
		
		//Tally up the score of the number set by the value.
	}
	
	public void StopTimer()
	{
		endTime = Time.time;
		TotalTimePlayed = endTime - startTime;
	}
	
	public DataEntry[] AllDataEntries
	{
		get {return _statisticsEntries;}
	}
	
	public float TimeAmount 
	{
		get 
		{
			if(endTime >= -1.0f)
			return TotalTimePlayed;
			
			return Time.time - startTime;
		}
		set {endTime = value; TotalTimePlayed = value;}
	}
	public int ValueMultiplier
	{
		get {return StatsValueModifier;}
		set {StatsValueModifier = value;}
	}
	public int Score
	{
		get{return StatsScore;}
		set{StatsScore = value;}
	}
	public float Money
	{
		get
		{
			float o =  (float)( _statisticsEntries[(int)EntryTypes.GemsCollected].Data ) /100.0f;

			return o;
		}
	}
	public float PPS
	{
		get{return ((float)Score/TimeAmount); }
	}
	
	public EntityFactory.CannonTypes CompletionCannon
	{
		get{ return completedCannonType;}
		set{completedCannonType = value;}
	}
	
	
	#endregion
	
	
	#region Operator Functions
	
	public void Overwrite(Statistics stats)
	{
		//Copy the main data.
		TimeAmount = stats.TimeAmount;
		startTime = stats.startTime;
		endTime = stats.endTime;
		
		
		//Int for holding the score.
		StatsScore = stats.StatsScore;
		
		//The score modifier.
		StatsValueModifier = stats.StatsValueModifier;
		
		//Store the cannon we completed with.
		completedCannonType = stats.completedCannonType;
		
		
		int index = 0;
		//Copy the data entries.
		foreach(DataEntry de in _statisticsEntries)
		{
			de.SetData(stats._statisticsEntries[index].Data);
			index++;
		}
	}
	
	
	#endregion
	
}
