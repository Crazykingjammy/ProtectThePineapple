using UnityEngine;
using System.Collections;

public class BaseItemCard : MonoBehaviour {
	
	//Internal use variables.
	bool dirty = false;
	bool _completed = false;
	int _trophiesEarned = 0;
	int _possibleTrohpies = 0;
	
	//Card Variables
	public string Label = "BlankCard";
	public bool _unlocked = false;

	//Variable for offsetting the icon.
	public Vector3 IconOffset, IconScale;


	//Information Data
	public CardInfoData DisplayInfo;
	
	Statistics _itemStatistics = null;
	
	
	//List of missions.
	public BaseMission[] Missions;
	
	
	//Local cache storage.
	ObjectiveMission _breathlessMission;
	
	[System.Serializable]
	public class CardInfoData
	{
		public string IconName = "Default-Icon";
		public string Description = "This is a beast cannon!";
		public Color BGColor = Color.white;
		public int TrophyRequirements;
		public int GemRequirements;
		public int LevelRequirement;
		
	}
	
	
	
	//Information linking the actual item.
	protected virtual void Start(){
		
		
	
		
		if(_itemStatistics == null)
		{
			//Create the stat object if there is none. and set its name and parent.
			_itemStatistics = Instantiate(GameObjectTracker.GetGOT()._PlayerData.BlankStatistics) as Statistics;
			_itemStatistics.name = "IC_" + this.Label + "_ItemStats";
			_itemStatistics.transform.parent = this.transform;	
			
			//And load if we are awake and there is no item.
			_itemStatistics.LoadFromPlayerPrefs();
		}
			
		
		Init();
		
	//	print("Base Item Card Start : End");
		
	}
	void Update(){}
	
	void Awake()
	{
	
		//Load the item card stats
		if(_itemStatistics == null)
		{
			//Lets look for the objects already in the scene to assign upon reload.
			foreach(Object o in FindSceneObjectsOfType(typeof(Statistics)))
			{
				string statname = "IC_" + this.Label + "_ItemStats";
				
				if(o.name == statname)
				{
					_itemStatistics = (Statistics)o;
					
					//Remake parent
					_itemStatistics.transform.parent = transform;
					
					DontDestroyOnLoad(_itemStatistics);
					break;
				}
			}
				
		}
		
			

		
	}
	
	public void CalculateTrophies()
	{
		int count = 0;
		_possibleTrohpies = 0;
		
			foreach(BaseMission bm in Missions)
			{
				//Add a trophie for each objective completed in the mission.
				count += bm.ObjectivesCompleted;
				_possibleTrohpies += bm.ObjectivesCount;
			}
			
			//Set the count to number of trophies earned.
			_trophiesEarned = count;
	}
	
	bool CalculateCompleted()
	{
		foreach(BaseMission bm in Missions)
				if(!bm.isFinished)
					return false;
			
			return true;
	}
	protected void Init()
	{
		//Create the missions.
		//print("baseCardInit");
		
		//Here we create the missions and load their data.
		for(int i = 0; i < Missions.Length; i++)
		{
			Missions[i] = Instantiate(Missions[i]) as BaseMission;
			
			//Set the name
			string mina = "IC_" + this.Label + "_" + Missions[i].Label;
			Missions[i].name = mina;
			
			//Set as child.
			Missions[i].transform.parent = this.transform;
			
			//Create the objectives
			//The data loads when the objectives names are applied.
			//The names are applied as the objective data is created
			//Load from file happens upon creation.
			Missions[i].ApplyNamesToObjectives();
			
		}
		
		//Calculate values.
		CalculateTrophies();
		_completed = CalculateCompleted();
		
		//Load from file to fill in all the information if we have any.
		//LoadCardItem();
		
		//Apply names all the Missions and objectives. 
		//Loading from file system will be done automatically if objects arent created.
		
		//Lets set the item card to not be destroyed on load so we dont do any reloading.
		//DontDestroyOnLoad(gameObject);
		
	}
	
	public void WriteCardStatsToPlayerPreferences()
	{
		//here maybe we can go through and save all the missions data too.
	}
	
	public bool IsDirty
	{
		get{return dirty;}
		set{dirty = value;}
	}
	
	public void SaveCardItem(bool overrideDirty = false)
	{
		//If we arent dirty or dont have an override. then dont go through with saving.
		if(!IsDirty)
			return;
		
		Debug.Log(this.name + "StartSave");
		
		//Save all the Missions.
		foreach(BaseMission m in Missions)
		{
			m.SaveMissionStatus();
		}
	
		//Save other data.
		if(_itemStatistics == null)
		{
			Debug.LogWarning("No Stats for Card_" + this.Label);
		}
		else{
			_itemStatistics.SaveToPlayerPrefs();
		}
		

		//Save if we are unlcoked or not.
		int unlockedBool = 1;
		if(Unlocked)
			unlockedBool = 2;
		
		//Save our unlock state.
		FileFetch.SetInt(this.name + "_Unlocked",unlockedBool);
		
		//no longer dirty.
		IsDirty = false;
		
		Debug.Log(this.name + "EndSave");
		
	
	}
	
	public void LoadCardItem()
	{
		//Debug.Log(this.name + "StartLoad");
		
		//load all the Missions.
//		foreach(BaseMission m in Missions)
//		{
//			m.LoadMissionStatus();
//		}
//		
		
		
		//Load other data.
		int unlockedBool = FileFetch.FetchInt(this.name + "_Unlocked");
		if(unlockedBool == 1 || unlockedBool == 0)
			_unlocked = false;
		else
			_unlocked = true;
		
		
	//	Debug.Log(this.name + "EndLoad");
		
	}
	
	public bool isCompleted
	{
		//Going through and testing if we have completed all missions assigned.
		get
		{
			return _completed;
		}
	}
	
	public bool Unlocked
	{
		get{return _unlocked;}
	}
	public int TrophiesEarned
	{
		get
		{
			return _trophiesEarned;
		}
	}
	
	public int TrophyCount
	{
		get
		{
			return _possibleTrohpies;
		}
	}
	
	public BaseMission BreathlessMission
	{	
		get
		{
			if(_breathlessMission)
				return _breathlessMission;
			
			foreach(BaseMission m in Missions)
				if(m.Label == "BreathlessMission")
			{
				_breathlessMission = (ObjectiveMission)m;
				return _breathlessMission;
			}
					
			
			return null;
		}
		
	}
	
	public BaseMission GetMissionByName(string missionName)
	{
		foreach(BaseMission m in Missions)
			if(m.Label == missionName)
		{
			return m;
		}
		
		return null;
	}
	
	//Applies stat to given mission.
	//Assgns stats to every objective in the mission.
	public void ApplyStatsToMission(Statistics stats, string missionName)
	{
		//Lets get the mission first.
		BaseMission m = GetMissionByName(missionName);
		
		//Then go through all the missions objectives and assign the passed in stats.
		foreach(BaseMission.GameObjective g in m.MissionObjectives)
		{			
			if(g == null)
			{
				print("Trying to assign a phase stat for a mission that is not there!");
				return;
			}
			
			g.ObjectiveStatistics = stats;
			
		}
		
		//Mark this card dirty for the le ule buggars 
		IsDirty = true;
	}
	
	public Statistics ItemStats {
		get 
		{
			return _itemStatistics;
		}
	}
	
	
	#region Children Virtual Functions
	
	public virtual bool isCannonItem(EntityFactory.CannonTypes t){return false;}
	public virtual EntityFactory.CannonTypes ContainedCannonType 
	{
		get{return EntityFactory.CannonTypes.NULL;}
	}
	
	//Returns true if the item can and has been be unlocked.
	public virtual bool UnlockCard(bool force = false)
	{
		//Return false if we are already unlocked.
		if(Unlocked)
			return false;
		
		if(force)
		{
			_unlocked = true;
			IsDirty = true;
			return true;
		}
		
		if(GameObjectTracker.instance._PlayerData.MyCurrentLevel >= DisplayInfo.LevelRequirement )
		{
			_unlocked = true;
			
			//Subtract the bank amount!!
			//GameObjectTracker.instance._PlayerData.GemBank  -= cost;		
			IsDirty = true;

			//FileFetch.SetInt(this.name + "_Unlocked",1);
			return true;
		}

		
		//Check the gems in the bank and gems in our requirements to purchase.
//		int bank = GameObjectTracker.instance._PlayerData.GemBank;
//		int cost = DisplayInfo.GemRequirements;
//		
//		//if we have it.
//		if(bank >= cost)
//		{
//			_unlocked = true;
//			
//			//Subtract the bank amount!!
//			GameObjectTracker.instance._PlayerData.GemBank  -= cost;		
//			IsDirty = true;
//
//			//FileFetch.SetInt(this.name + "_Unlocked",1);
//			return true;
//		}



		
		return false;
	}

	
	#endregion
		
}
