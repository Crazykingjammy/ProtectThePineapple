using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// FUI stats table.
/// This is the NGUI Table Object.
/// The role of this object is to create children of the stat types
/// </summary>
public class FUIStatsTable : MonoBehaviour {

	public UITable myTable; 	// set who my table is, for now...
	
	public FUIStatObject refStatObject;
	public List<FUIStatObject> statObjects;	// the list of all the stat objects to create and 
	
	// check to make sure we're fully enabled
	bool isFullyInit = false;
	
	// add as children to myTable
	
	// on start we need to add statObject types to myTable
	
	Statistics pdata = null;
	
	
	public Statistics statData
	{
		get {return pdata; }
		set { pdata = value;
		
			foreach(FUIStatObject sobj in statObjects)
			{
				sobj.myStat = pdata;
			}
		
		
		}
	}
	
	// Use this for initialization
	void Start () {
		
		Init();
		
		
	}
	
	void PhaseClick()
	{
		Debug.Log("Phase Click!!!!!!!!!!!!!!!!!!!!!!");
	}
	
	void Init(){
		
		if (GameObjectTracker.GetGOT()){
		
			//Set default data.
			if(pdata == null)
				pdata = GameObjectTracker.GetGOT().FullStatistics;
		
			
			int index = 0;
			foreach(Statistics.DataEntry data in pdata.AllDataEntries)
			{
				
			 
				FUIStatObject newStat = null; 
				
				// add the new stat as a child to the table
				newStat = NGUITools.AddChild(myTable.gameObject, refStatObject.gameObject).GetComponent<FUIStatObject>();
		
				//Set the data first so we can assign the value upon assigning the statistic reference.
				newStat.StatType = data.EntryType;
				newStat.myStat = pdata;
				
				statObjects.Add(newStat);
				
				myTable.Reposition();
				index++;
			}			
			
			isFullyInit = true;
		}		
	}
	
	void Update(){
		if(!isFullyInit){
			Debug.LogError("FUIStats Table NOT fully Init - Trying again");
			Init();
		}
	}
}
