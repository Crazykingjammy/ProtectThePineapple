using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FUIMissionViewer : MonoBehaviour {
	
	public UITable ObjectivesListTable;
	
	public FUIObjectiveBadge ObjectiveBadgePrefab;
	
	public UILabel MissionName;
	
	BaseMission _assignedMission = null;
	
	List<FUIObjectiveBadge> _objectiveslist = null;
	
	bool populated = false;
	
	
	// Use this for initialization
	void Awake () {
	
		//Populate the list shall we!
		//PopulateList();
		
		_objectiveslist = new List<FUIObjectiveBadge>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!populated)
			PopulateList();
	
	}
	
	public BaseMission AssignedMission
	{
		set{
			_assignedMission  = value;
			
			//Call reassign mission right here.
			ReAssignMission();
		}
		
		get{return _assignedMission;}
	}
	
	public void ReAssignMission()
	{
		if(!populated)
		{
			PopulateList();
			//return;
		}

		
		//Grab the selected cannon from the activity manager.
		BaseItemCard selectedCard = ActivityManager.Instance.SelectedCard;
		
		//Grab the first list of missions on the list.
		if(AssignedMission == null)
			AssignedMission = selectedCard.Missions[0];
		
		//Set teh mission label
		MissionName.text = AssignedMission.DisplayName;
		
		int index = 0;
		//Go through the mission and assign the badge at index of the objective
		foreach(BaseMission.GameObjective gobjective in  AssignedMission.MissionObjectives)
		{	
			_objectiveslist[index].AssignedObjective = gobjective;
			index++;
		}
	}
	
	void PopulateList()
	{
		//Grab the selected cannon from the activity manager.
		BaseItemCard selectedCard = ActivityManager.Instance.SelectedCard;
		
		if(selectedCard == null)
			return;
		
		if(_objectiveslist == null)
		{
			Debug.LogError("NO OBJECTIVE LIST!! WTF!!");
			_objectiveslist = new List<FUIObjectiveBadge>();
			
		}
			
		
		//Grab the first list of missions on the list.
		BaseMission mission = selectedCard.Missions[0];
		
		//Store a placeholder objective badge
		FUIObjectiveBadge newBadge = null;
		
		foreach(BaseMission.GameObjective gobjective in mission.MissionObjectives)
		{
			//Allocate the new badge.
			newBadge = NGUITools.AddChild(ObjectivesListTable.gameObject, ObjectiveBadgePrefab.gameObject).GetComponent<FUIObjectiveBadge>();
				
			//Set the variables
			newBadge.AssignedObjective = gobjective;
			
			//Add it to the list.
			_objectiveslist.Add(newBadge);
			
			//Repositioning the table.
			ObjectivesListTable.Reposition(); 
				
		}
		
		
		populated = true;
		
		
	}
}
