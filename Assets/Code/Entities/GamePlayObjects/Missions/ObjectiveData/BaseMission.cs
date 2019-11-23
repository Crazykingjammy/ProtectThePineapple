using UnityEngine;
using System.Collections;

public class BaseMission : MonoBehaviour {
	
	
	
	#region Data Structures
	
	[System.Serializable]
	public class GameObjective 
	{
		//Name for the objective.
		public string ObjectiveLabel;
		public string IconName = "Default-Icon";
		public int ObjectiveWorth = 1;
		
		//Stat gate to test if objective is complete.
		public Statistics.StatGate CompletionTest;
		
		//Statistics stored for the objective. (SAVE)
		Statistics _objectiveStats = null;
		
		bool dirty = false;
		
		//Functions for access to private objects
		public void SaveStats()
		{
			//Check validity and set name.
			if(!_objectiveStats)
			{
				print("NO Stats Available!!");
				return;
			}
			
			//Lets now save dirty stats.
			if(!dirty)
				return;
				
			
			_objectiveStats.SaveToPlayerPrefs();
			dirty = false;
			
//			print(_objectiveStats.name + " was saved.");
		}
		
		public void LoadStats()
		{
			//It looks like we have to instansuate stats at load.
			if(!_objectiveStats)
			return;
			
			_objectiveStats.LoadFromPlayerPrefs();
			
			//print(_objectiveStats.name + " was Loaded.");
		}
		
		//Returns true if successful and there has been stats assigned to the mission.
		public bool SetObjectiveNameObject(string missionName, int index = 0)
		{
			string objstatname = missionName + "_ObjStats" + index + "_" + ObjectiveLabel;
			
			//Check if there are no stats yet assigned to the objective then we return
			if(_objectiveStats == null)
			{
				
				//Here is where we can check for the already existing objective stat.
				//Lets look for the objects already in the scene to assign upon reload.
				foreach(Object o in FindSceneObjectsOfType(typeof(Statistics)))
				{
					if(o.name == objstatname)
					{
						_objectiveStats = (Statistics)o;
						
						//remake parent.
						
						DontDestroyOnLoad(_objectiveStats.gameObject);
						
						//Return false that object did not exist.
						return false;
					}
				}
				
				//This is where the creation takes place
				_objectiveStats = Instantiate(GameObjectTracker.GetGOT()._PlayerData.BlankStatistics) as Statistics;
				
				//Apply the mission name to the objective label
				_objectiveStats.name = objstatname;
				
				
				//Lets load since we are created.
				_objectiveStats.LoadFromPlayerPrefs();
				
				//Keep this lil guy around.
				DontDestroyOnLoad(_objectiveStats.gameObject);
				
				//Return false that object did not exist.
				return false;
				//print("Stat Created!!!!!!!!!!!!!!!!!!:" + objstatname);
			}
				
			//Apply the mission name to the objective label
			_objectiveStats.name = objstatname;
				
			
			//Print the name
//			print("ObjStats Name Set: " + _objectiveStats.name);
			return true;
			
		}
		
		//Completed 
		public bool isCompleted
		{
			get
			{
				float p = 0.0f;
				
				if(_objectiveStats)
				{
					p = _objectiveStats.StatTest(CompletionTest);
				}
				
				if(p >= 100.0f)
				{
					//Debug.Log("Objective " + this.ObjectiveLabel + " is Completed at"  + p + " %");
					
					return true;
				}
					
				
				return false;
			}
		}
		
		public Statistics ObjectiveStatistics 
		{
			get
			{
				return _objectiveStats;
			}
			set 
			{
				float incommingtestresult = value.StatTest(CompletionTest);
				float previousTestResult = _objectiveStats.StatTest(CompletionTest);
				
//				Debug.Log("Incomming Result :" + incommingtestresult + "% " + "Punch at: " + value.TimeAmount + " " + _objectiveStats.name);
//				Debug.Log("Loaded Result :" + previousTestResult + "% " + "Punch at: " + _objectiveStats.TimeAmount + " " + _objectiveStats.name);
				
				
				//We can handle our setting of the rules here to apply to newly assigned stats for objectives.
				//If we get a better percentage on the StatTest then apply.
				if( value.StatTest(CompletionTest) >= _objectiveStats.StatTest(CompletionTest))
				{
					//We can also set the dirty flag here.
					dirty = true;
					
					//If we are already allocated we delete ourselves first.
					_objectiveStats.Overwrite(value);
					
//					Debug.Log("!!!!!! Objective Stats Applied: " + _objectiveStats.name);
				}
				
				//print(_objectiveStats.name + " was assigned by " + value.name);
			}
		}
		
	}
	
	
	
	
	
	#endregion
	
	
	#region Member Variables
	public string Label = "Mission Label";
	public string DisplayName = "Protect the Pineapple";
	public string IconName = "Default-Icon";
	public int MissionWorth;
	
	//List of objectives
	public GameObjective[] MissionObjectives;
	
	
	#endregion
	
	
	void CalculateObjectivesCompleted()
	{
		//Create a count and incriment for each completed objective.
		int completed = 0;
		foreach(GameObjective objective in MissionObjectives)
		{
			if(objective.isCompleted)
				completed++;
		}
		
		
		
	}
	
 	void CalculateWorthObtained()
	{	
		
		//Create a count and incriment for each completed objective.
		int points = 0;
		foreach(GameObjective objective in MissionObjectives)
		{
			points += objective.ObjectiveWorth;
		}
		
			
		
	}
	
	
	#region Public Functions
	
	public  void SaveMissionStatus() 
	{	
		//Apply the names.
		ApplyNamesToObjectives();
		
		//Save each objective.
		foreach(BaseMission.GameObjective g in MissionObjectives)
		{
			//Set the name
			g.SaveStats();
			
		}
	}
	
	public  void LoadMissionStatus()
	{
		//Apply names again on load.
		ApplyNamesToObjectives();
		
		//Save each objective.
		foreach(BaseMission.GameObjective g in MissionObjectives)
		{
			//Set the name
			g.LoadStats();
		}
	}
	
	void Start()
	{
		//lets do this on start shall we.
		//ApplyNamesToObjectives();
	}
	
	void Update()
	{
		
	}
	public int ObjectivesCompleted
	{
		get
		{
			//Create a count and incriment for each completed objective.
			int completed = 0;
			foreach(GameObjective objective in MissionObjectives)
			{
				if(objective.isCompleted)
					completed++;
			}
			
			return completed;
		}
	}
	
	public int WorthObtained
	{
		get{
			//Create a count and incriment for each completed objective.
			int points = 0;
			foreach(GameObjective objective in MissionObjectives)
			{
				points += objective.ObjectiveWorth;
			}
			
			return points;
		}
	}
	
	public int ObjectivesCount
	{
		get{
			return MissionObjectives.Length;
		}
	}
	
	public bool isFinished
	{
		get
		{
			//Check all objectives and see if we have not completed any.
			foreach(GameObjective objective in MissionObjectives)
				if(!objective.isCompleted)
					return false;
			
			//if we pass all tests we return true
			return true;
		}
		
	}
	
	public void ApplyNamesToObjectives()
	{
		//Keep track of count.
		int index = 0;
		
		foreach(BaseMission.GameObjective g in MissionObjectives)
		{
			//Set name of object and if it returns true Lets set the child too.
			g.SetObjectiveNameObject(this.name,index);
			g.ObjectiveStatistics.transform.parent = this.transform;
			
			//Increment the index to pass in..
			index++;	
			
		}
	}
	
	
	
	#endregion
}
