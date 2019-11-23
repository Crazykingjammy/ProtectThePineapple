using UnityEngine;
using System.Collections;

public class GameOverView : MonoBehaviour {
	
	//Public variables for creating.
	public GUITexture BackgroundScrollImage;
	public GUITexture ImageStyle;
	
	
	//Local variables 
	Vector2 scrollViewVector = Vector2.zero;
	float heightInset = 0.0f;
	Vector2 positionVector = Vector2.zero;
	Vector2 dimensionVector = Vector2.zero;
	Rect TextDraw;
	
	//Display Information
	public Statistics GameStatistics;
	public GUIText ScoreText;
	
	public Rewards RewardStats;
	
	
	//Controls
	public Label QuitOption;
	public Label RetryOption;
	
	
	//Styles
	public GUIStyle RewardsStyle;
	public GUIStyle TimeStyle;
	public GUIStyle TextStyle;
	public GUIStyle DataStyle;
	
	public GUISkin skin;
	
	
	//Variables
	public float HeightOffset;
	public float HeightPadding = 200.0f;
	public float TopBorder = 20.0f;
	
	public float RewardsWidthOffset = 2.0f;
	
	
	float scrollVelocity = 0f;
	float timeTouchPhaseEnded = 0f;
	const float inertiaDuration = 0.5f;
	
	
	
	
	
	
	
	// Use this for initialization
	void Start () {
		
		//Set the position of the scroll bar.
		positionVector.Set((BackgroundScrollImage.transform.position.x * Screen.width) + BackgroundScrollImage.pixelInset.x,
			(BackgroundScrollImage.transform.position.y * Screen.height) + BackgroundScrollImage.pixelInset.y);
		
		//Set the dimensions of the scroll bar.
		dimensionVector.Set(BackgroundScrollImage.pixelInset.width,BackgroundScrollImage.pixelInset.height);
		
		//Create the view rect.
		TextDraw = new Rect(0,heightInset,400,400);
		
		QuitOption.SetAction("OnQuit");
		RetryOption.SetAction("OnRetry");
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(scrollVelocity != 0.0f)
		{
			float t = (Time.time - timeTouchPhaseEnded)/inertiaDuration;
			float framevelocity = Mathf.Lerp(scrollVelocity,0,t);
			
			scrollViewVector.y += framevelocity * Time.deltaTime;
			
			if(t >= inertiaDuration)
				scrollVelocity = 0.0f;
		}
		
		
		foreach(Touch t in Input.touches)
		{
			if(t.phase == TouchPhase.Moved)
			{
				scrollViewVector.y += t.deltaPosition.y;	
			}
			
			if(t.phase == TouchPhase.Ended)
			{
				if(Mathf.Abs(t.deltaPosition.y) >= 10)
				{
					scrollVelocity = (int)(t.deltaPosition.y/t.deltaTime);
					timeTouchPhaseEnded = Time.time;
				}
			}
			
		}
		
	}
	
	
	void NOT_OnGUI()
	{
		
		//Set the gui skin.
		GUI.skin = skin;
		
		//Set the rects
		Rect position = new Rect(positionVector.x, positionVector.y, dimensionVector.x,dimensionVector.y);
		Rect scrollview =  new Rect(0,0,dimensionVector.x,heightInset + HeightPadding);
		
		//Set the scroll view
		scrollViewVector = GUI.BeginScrollView(position,scrollViewVector,scrollview);
		
		//Prepare before we insert anything.
		heightInset = 0;
		TextDraw.y = heightInset;
		
		string timeformat = FormatSeconds(120.99f);
		string fruitstring = "You Earned a FRUIT!";
		
		
		heightInset += TopBorder;
		TextDraw.y = heightInset;
		
		//Rewards.
		
		for(int i = 0; i < RewardStats.GetRewardCount(); i++)
		{
			GUI.Label(TextDraw,RewardStats.GetFruitImage(i),RewardsStyle);
			GUI.Label(TextDraw, FormatSeconds(RewardStats.GetFruitTime(i)),TimeStyle);
			GUI.Label(TextDraw,fruitstring,TextStyle);
			
			heightInset += RewardStats.GetFruitImage(i).height + HeightOffset;// + RewardStats.GetFruitImage(i).height;
			TextDraw.y = heightInset;	
		}
		
		
		
		
		

		
		
		
		
		///Statistics
		
		//Give a gap before and after.
		heightInset += HeightOffset * 2;
		TextDraw.y = heightInset;
		
		GUI.Label(TextDraw,"- Statistics -", RewardsStyle);
		
		heightInset += HeightOffset * 2;
		TextDraw.y = heightInset;
		
		
//		if(GameStatistics)
//		{
//		
//			for(int i = 0; i < GameStatistics.Statistics.Length; i++)
//			{
//				InsertDataEntry(GameStatistics.Statistics[i].DataLabel,
//					GameStatistics.Statistics[i].ValueData.ToString());
//			}
//			
//			timeformat = FormatSeconds(GameStatistics.TotalTimePlayed);
//		}
//		else
//		{
//			for(int i = 0; i < 20; i++)
//			InsertDataEntry("There Is No Statistics","N/A");
//		}
//		
		
		
		
		//////Time Elapsed.
		
		//Here we update the time elapsed differently
		heightInset += HeightOffset * 2;
		TextDraw.y = heightInset;
		
		GUI.Label(TextDraw,"Time Elapsed",TextStyle);
		GUI.Label(TextDraw,timeformat,DataStyle);
		
		heightInset += HeightOffset ;
		TextDraw.y = heightInset;
		
		
		//End the draw call.
		GUI.EndScrollView();
		
	
		
		
	}
	
	
	void InsertDataEntry(string name, string data)
	{
		//Draw the first data entry. And update the height position.
		GUI.Label(TextDraw,name,TextStyle);
			
		GUI.Label(TextDraw,data,DataStyle);
		
		heightInset += HeightOffset;
		TextDraw.y = heightInset;
		
		
	}
	
	
	public void SetDataEntry(Statistics stats)
	{
		GameStatistics = stats;
	
		//Calculate pps.
//		int score =  GameStatistics.Score;
//		float pps = (float)score/GameStatistics.TimeAmount;

		
		//Set the score text to PPS.
		//ScoreText.text = GameObjectTracker.GetGOT().GamePPS.ToString();
		ScoreText.text = GameObjectTracker.GetGOT().RunStatistics.PPS.ToString();
		
	}
	
	public void SetRewards(Rewards r)
	{
		RewardStats = r;
	}
	
	void OnQuit()
	{
		//Application.LoadLevel("MainMenu");
	}
	
	void OnRetry()
	{
		GameObjectTracker.GetGOT().RestartButtonPushed();
	}
	
string FormatSeconds(float elapsed)
{
   int d = (int)(elapsed * 100.0f);
   int minutes = d / (60 * 100);
   int seconds = (d % (60 * 100)) / 100;
   int hundredths = d % 100;
   return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
}
	
}
