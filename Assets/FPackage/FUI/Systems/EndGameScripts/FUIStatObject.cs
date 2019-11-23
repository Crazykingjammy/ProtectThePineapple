using UnityEngine;
using System.Collections;

public class FUIStatObject : MonoBehaviour {
	
	public UILabel statName;	// the UI Label for the name
	public UILabel statValue;	// the UI Label for the value
	public UILabel statWorth = null;
	
	public Statistics.EntryTypes StatType;
		
	Statistics stats = null;
	
	bool liveUpdate = true;
	
	public Statistics myStat
	{
		get{return stats;}
		set{stats = value;
		
			Statistics.DataEntry data = stats.GetEntryData(StatType);
			
			//hold a string for formatting
			string format = "";
			statName.text = data.DataLabel;
			
			format = string.Format("{0:#,#,#}", data.Data);
			
			if(data.Data == 0)
				format = "- 0 -";
			
			statValue.text = format;
			
			format = "";
			if(data.EntryValue > 0)
				format = "+";
			
			//Check if we have a stat worth
			if(statWorth != null)
			statWorth.text  = format + data.EntryValue.ToString();
			
			//Special formatting.
			//ScoreLabel.text = string.Format("{0:#,#,#}", gameStats.Score);
			//string format = string.Format("{0:C}",gemcount);
			
			if(data.EntryType == Statistics.EntryTypes.GemsCollected || data.EntryType == Statistics.EntryTypes.GemsLost)
			{
				float gemcount = (float)data.Data/100.0f;
				format = string.Format("{0:C}",gemcount);
				statValue.text = format;
			}
			
		
		}
	}
	
	public void Update(){
	
		
		if(!liveUpdate)
			return;
		
		//Check if we have stats.
		if(stats == null)
			stats = GameObjectTracker.instance.FullStatistics;
	
		//Update the data.
		myStat = stats;
		
	}
}
