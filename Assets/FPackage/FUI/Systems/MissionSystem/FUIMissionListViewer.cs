using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FUIMissionListViewer : MonoBehaviour {
	
	public UIGrid MissionsTable;
	public FUIMissionViewer MissionViewerTemplate;
	
	public int InitialSize = 5;
	
	List<FUIMissionViewer> _missionViewersList;
	
	bool populated = false;
	
	
	// Use this for initialization
	void Awake () {
	
		//Create the list on awake.
		_missionViewersList = new List<FUIMissionViewer>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!populated)
			PopulateList();
	
	}
	
	
	
	public void ReAssignMissions()
	{
		if(!populated)
		{
			PopulateList();
			//return;
		}

		
		//Grab the selected cannon from the activity manager.
		BaseItemCard selectedCard = ActivityManager.Instance.SelectedCard;
		
		int index = 0;
		foreach(BaseMission mission in selectedCard.Missions)
		{
		
			//If our current index is beyond the size then we have gone to far!
			if(index >= _missionViewersList.Count)
			{
				Debug.LogError("Mission viewer table size not big enough!");
					return;
			}
			
			//Grab the current FUIMissionviewer
			FUIMissionViewer mview = _missionViewersList[index];
			mview.gameObject.SetActive(true);
			MissionsTable.Reposition();
			
			//Set teh assigned mission.
			mview.AssignedMission = mission;
			
			index++;
			
		}
		
		

	}
	
	void PopulateList()
	{
		//Grab the selected cannon from the activity manager.
		BaseItemCard selectedCard = ActivityManager.Instance.SelectedCard;
		
		if(selectedCard == null)
			return;
		
		if(_missionViewersList == null)
		{
			Debug.LogError("NO Mission Viewer LIST!! WTF!!");
			_missionViewersList = new List<FUIMissionViewer>();
			
		}
			
		
		//Store a placeholder objective badge
		FUIMissionViewer newMission = null;
		
		for(int i = 0; i < InitialSize; i++)
		{
			//Allocate the new badge.
			newMission = NGUITools.AddChild(MissionsTable.gameObject, MissionViewerTemplate.gameObject).GetComponent<FUIMissionViewer>();
				
			//Set the variables
			//newMission.AssignedMission = mission;

			//Add it to the list.
			_missionViewersList.Add(newMission);
			
			//Repositioning the table.
			MissionsTable.Reposition();
			
			//Disable
			newMission.gameObject.SetActive(false);
				
		}
		
		
		populated = true;
		
		
	}
	
}
