using UnityEngine;
using System.Collections;

public class ActivityStat : FUIActivity {
	
	public FUIStatsTable MyStats;
	
	public UILabel TimeText,ScoreText, StatsName;
	
	// Use this for initialization
	new void Start () {
	
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnActivate ()
	{
		
		//Grab the stats from the manager.
		Statistics stats = ActivityManager.Instance.SelectedStats;
		
		//Assign it to the table
		MyStats.statData = stats;
		
		//Lets check if we are the full game stats, which we will display a little differently.
		if(stats.TimeAmount == -1.0f)
		{
			TimeText.alpha = 0.0f;
			ScoreText.alpha = 0.0f;
			return;
		}

		//Set the name label for the stats.
		StatsName.text = stats.ContainerLabel;
		
		//Set some of the activities stats.
		TimeText.text = FormatSeconds(stats.TimeAmount);
		ScoreText.text = string.Format("{0:#,#,#}", stats.Score);
		
		TimeText.alpha = 1.0f;
		ScoreText.alpha = 1.0f;
	}
	
	
	void OnClose()
	{
		ActivityManager.Instance.PopActivity();
	}
	
	
	string FormatSeconds(float elapsed)
	{
	   int d = (int)(elapsed * 100.0f);
	   int minutes = d / (60 * 100);
	   int seconds = (d % (60 * 100)) / 100;
	   int hundredths = d % 100;
	   return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, hundredths);
	}	
	
}
