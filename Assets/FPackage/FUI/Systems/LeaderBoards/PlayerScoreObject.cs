using UnityEngine;
using System.Collections;
using System.IO;

public class PlayerScoreObject : MonoBehaviour {
	
	
	//Link to data to display
	public UILabel ScoreText, NameText, PositionText, TimeText;
	public UITexture ProfilePicture;
	public UISprite BG;
	
	SocialCenter.LeaderBoardScore _scoreReference = null;
	
	
	
	System.DateTime testStamp;
	//Assign variables via leaderboard Score strcture;
	public SocialCenter.LeaderBoardScore MyScore
	{
		set{
			_scoreReference = value;
			
			ScoreText.text = value.FormattedScore;
			NameText.text = value.Alias;
			PositionText.text = value.rank.ToString();
			
			
			//ProfilePicture.mainTexture = value.ProfilePicture;
			//print("!! -- Loading Resource: " + value.ProfilePicturePath);
			//string[] peices = value.PlayerID.Split(':');
			//string fixedPath = "../../Documents/" + 'G' + peices[peices.Length - 1];
			//print ("!!! -- Fixed:" + fixedPath);
			//Debug.Log("!!! -- Photo:",value.ProfilePicture);
			
			if(value.HasPhoto)
				Debug.Log("!!!! --  HasPhoto" + value.Alias);
		
			ProfilePicture.mainTexture = value.ProfilePoto;
			
			//Calculate the time
			System.TimeSpan stamp =  System.DateTime.UtcNow  - value.Stamp;
			int mins = (int)stamp.TotalMinutes;
			string timeDisplay = "Just Now";
			
			Debug.LogError(stamp);
			
			//If we are over 0 mins we display the text respondingly
			if(mins > 0)
			{
				timeDisplay = mins.ToString() + " mins ago";
				
				//If we are over 60 mins, display in hours.
				if(mins > 60 ){
					int h = (int)stamp.TotalHours;
					timeDisplay = h.ToString() + " hours ago";
				}
			}
			
			//TimeText.text = stamp.ToString();
			
			//Set the text.
			TimeText.text = timeDisplay;
			
		}
	}
	
	// Use this for initialization
	void Start () {
	
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
