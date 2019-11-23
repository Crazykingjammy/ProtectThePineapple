using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FUIObjectiveViewer : MonoBehaviour {
	
	//Reference to the table to populate.
	public UITable StatListTable;
	
	public FUIStatGate StatGateObjectPrefab;
	
	public int InitalSize = 10;
	
	List<FUIStatGate> _statGatesList = null;
	
	bool populated = false;
	
	
	
	// Use this for initialization
	void Awake () {
	
		//Create the list object.
		_statGatesList = new List<FUIStatGate>();
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(!populated)
			PopulateList();
		
	}
	
	public void ClearList()
	{
		foreach(FUIStatGate gate in _statGatesList)
		{
			gate.gameObject.SetActive(false);
		}
	}
	
	public void AssignObjective()
	{
		if(!populated)
		{
			PopulateList();
			///return;
		}
		
		//Grab the objective.
		BaseMission.GameObjective gobj = ActivityManager.Instance.SelectedObjective;
		
		//Grab the stat gate for the objective.
		Statistics.StatGate gate  = gobj.CompletionTest;
		
		//Lets clear the list first.
		ClearList();
		
		//Here we check the stat gate specifics.
		int index = 0;
		
//		//Test if we need to display PPS requirement
//		if(gate.PPS != -1)
//		{
//			//Grab the FUIstat gate to assign the labels.
//			FUIStatGate fuigate = _statGatesList[index];
//			
//			///fuigate.Command.text = gate.
//		}
		
		//Specific PPS test.
		if(gate.Score != -1)
		{
			//Grab the fui stat gate and assign the labels.
			FUIStatGate fuigate = _statGatesList[index];
			fuigate.gameObject.SetActive(true);
			StatListTable.Reposition();
			
			//Set the commanding text.
			fuigate.Command.text = "Reach";
			
			//Set the condition text.
			if(gate.bScore)
				fuigate.Condition.text = "higher than";
			else
				fuigate.Condition.text = "below";
			
			fuigate.Command.text += " " + fuigate.Condition.text;
			
			//Set the value.
			string format = string.Format("{0:#,#,#}", gate.Score);
			fuigate.Value.text = format;
			
			//Set the unit type
			fuigate.UnitType.text = "points";
			
			//Set the data entry shadow 
			format = string.Format("{0:#,#,#}", gobj.ObjectiveStatistics.Score);
			fuigate.Data.text = format;
			
			//Add the count then add the label.
			fuigate.Command.text += " " + fuigate.Value.text + " " + fuigate.UnitType.text;
			
			//Incriment the index
			index++;
		}
		
		//Specific PPS test.
		if(gate.PPS != -1.0f)
		{
			//Grab the fui stat gate and assign the labels.
			FUIStatGate fuigate = _statGatesList[index];
			fuigate.gameObject.SetActive(true);
			StatListTable.Reposition();
			
			//Set the commanding text.
			fuigate.Command.text = "Get";
			
			//Set the condition text.
			if(gate.bPPS)
				fuigate.Condition.text = "over";
			else
				fuigate.Condition.text = "under";
			
			fuigate.Command.text += " " + fuigate.Condition.text;
			
			//Set the value.
			string format = string.Format("{0:#,#,#}", gate.PPS);
			fuigate.Value.text = format;
			
			//Set the unit type
			fuigate.UnitType.text = "PPS";
			
			//Set the data entry shadow 
			format = string.Format("{0:#,#,#}", gobj.ObjectiveStatistics.PPS);
			fuigate.Data.text = format;
			
			//Add the count then add the label.
			fuigate.Command.text += " " + fuigate.Value.text + " " + fuigate.UnitType.text;
			
			//Incriment the index
			index++;
		}
		
		//Specific time check test.
		if(gate.timeAmount != -1)
		{
			//Grab the fui stat gate and assign the labels.
			FUIStatGate fuigate = _statGatesList[index];
			fuigate.gameObject.SetActive(true);
			StatListTable.Reposition();
			
			//Set the commanding text.
			fuigate.Command.text = "Beat";
			
			//Set the condition text.
			if(gate.btimeAmount)
				fuigate.Condition.text = "with more than";
			else
				fuigate.Condition.text = "under";
			
			fuigate.Command.text += " " + fuigate.Condition.text;
			
			//Set the value.
			fuigate.Value.text = gate.timeAmount.ToString();
			
			//Set the unit type
			fuigate.UnitType.text = "Seconds";
			
			float time = gobj.ObjectiveStatistics.TimeAmount;
			
			//Set the data entry shadow 
			fuigate.Data.text = FormatSeconds(time);
			
			//Add the count then add the label.
			fuigate.Command.text += " " + fuigate.Value.text + " " + fuigate.UnitType.text;
			
			//Incriment the index
			index++;
		}
		
		//Grab a statistics template for text reference.
		Statistics StatReference = GameObjectTracker.instance._PlayerData.BlankStatistics;
		
		//Populate the rest of the stat gate tests.
		foreach(Statistics.EntryGate entry in gate.StatsTest)
		{
			//Grab the fui stat gate and assign the labels.
			FUIStatGate fuigate = _statGatesList[index];
			fuigate.gameObject.SetActive(true);
			StatListTable.Reposition();
			
			
			//Store the current data entry type we are on to access all the text.
			Statistics.DataEntry dataEntry = StatReference.GetEntryData(entry.type);
			
			//set the condition text.
			fuigate.Command.text = dataEntry.BeginningLabel;
			
			//Set the condition text.
			if(entry.bGreater)
				fuigate.Condition.text = dataEntry.CheckLabelg;
			else
				fuigate.Condition.text = dataEntry.CheckLabell;
			
			
			fuigate.Command.text += " " + fuigate.Condition.text;
			
			//Set the value type.
			string format = string.Format("{0:#,#,#}", entry.ValueTest);
			if(entry.ValueTest == 0)
				format = "0";
			
			if(entry.type == Statistics.EntryTypes.GemsCollected || entry.type == Statistics.EntryTypes.GemsLost)
			{
				float gemcount = (int)entry.ValueTest;
				gemcount = gemcount/100.0f;
				format = string.Format("{0:C}", gemcount);
			}
				
			
			
			fuigate.Value.text = format;
			
			//Set the unit type.
			fuigate.UnitType.text = dataEntry.DataLabel;
			
			//Set the data entry shadow.
			format = string.Format("{0:#,#,#}", gobj.ObjectiveStatistics.GetEntryData(entry.type).Data);
			
			//Check for gems and set to money format.
			if(entry.type == Statistics.EntryTypes.GemsCollected || entry.type == Statistics.EntryTypes.GemsLost)
			{
				float gemcount = (int)gobj.ObjectiveStatistics.GetEntryData(entry.type).Data;
				gemcount = gemcount/100.0f;
				format = string.Format("{0:C}", gemcount);
			}
				
			
			//Check for 0
			if(gobj.ObjectiveStatistics.GetEntryData(entry.type).Data == 0)
				format = "0";
			
			fuigate.Data.text = format;
			
			//Add the count then add the label.
			fuigate.Command.text += " " + fuigate.Value.text + " " + fuigate.UnitType.text;
			
			//Incriment the index.
			index++;
		}

	}
	
	//Create a array of stat gates to have in stock.
	void PopulateList()
	{
		
		if(_statGatesList == null)
		{
			Debug.LogError("NO OBJECTIVE LIST!! WTF!!");
			_statGatesList = new List<FUIStatGate>();
			
		}
		
		FUIStatGate gateobj; 
		
		//Create the cache.
		for(int i = 0; i < InitalSize; i++)
		{
			//Create the object.
			gateobj = NGUITools.AddChild(StatListTable.gameObject, StatGateObjectPrefab.gameObject).GetComponent<FUIStatGate>();
				
			//Add to the list.
			_statGatesList.Add(gateobj);
			
			//Reposition table.
			StatListTable.Reposition();
			
			//Disable
			gateobj.gameObject.SetActive(false);
		}
		
		populated = true;
	}
	
	
string FormatSeconds(float elapsed)
{
		if(elapsed == -1.0f)
			return "?";
		
   int d = (int)(elapsed * 100.0f);
   int minutes = d / (60 * 100);
   int seconds = (d % (60 * 100)) / 100;
   int hundredths = d % 100;
   return string.Format("{0:}:{1:00}.{2:00}", minutes, seconds, hundredths);
}
	
}
