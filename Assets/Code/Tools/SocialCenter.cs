using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_IPHONE
using Prime31;
#endif

public class SocialCenter : MonoBehaviour {
	
	
	int ListCount = 25;
	
	[System.Serializable]
	public class LeaderBoardScore
	{
		//Variables that should belong to a score
		public bool isFriend; 
		public int rank;
		public string PlayerID, Alias,FormattedScore, ProfilePicturePath;
		//public Texture2D ProfilePicture;
		public System.DateTime Stamp;
		
		
		bool valid = false; 
		
		
		//Accessors and variables
		
#if UNITY_IPHONE
	
		
		
		public GameCenterScore gcScore
		{
			set{
				
				isFriend = value.isFriend;
				rank = value.rank;
				PlayerID = value.playerId;
				Alias = value.alias;
				FormattedScore = value.formattedValue;
				Stamp = value.date;
				valid = true;
				
			}
		}
		
#endif
		
		public bool IsValid
		{
			get { return valid;}
		}
		public bool HasPhoto
		{
			get {
				if(File.Exists (ProfilePicturePath)) 
					return true;
				
				return false;
			}
		}
		
		public Texture2D ProfilePoto
		{
			get{
				
				if(!HasPhoto)
					return null;
				
				var bytes = File.ReadAllBytes (ProfilePicturePath);
				var tex = new Texture2D (0, 0);
				
				if (!tex.LoadImage (bytes))
					return null;
				
				return tex;
				
			}
			
		}
		
	}
	
	
	//Variables and accessors.
#if UNITY_IPHONE
	//Create a GCI (Game Center Interface)
	static GCI _gameCenter = null;
	
#endif
	
	
	//Have a list of LeaderboardScores for the current leaderboard presentation
	public List<LeaderBoardScore> _loadedScores;
	
		
#region Singleton variables and functions
	private static SocialCenter instance = null;
	
	public static SocialCenter Instance
	{
		get
		{
			if(instance == null)
			{
				return null;
			}
			return instance;
		}
	}
#endregion
	
	
	
	
	
public void Authenticate()
	{

#if UNITY_IPHONE	
		
		//Authenticate the player.
		_gameCenter.Authenticate();
		return;
#endif

	}
	
	// Use this for initialization
	void Start () {
		
		
		//First things first is we set our instance! 
		//Since we are attached to GOT we can do this.
		instance = this;
		
		
		#if UNITY_IPHONE	
		
		//Create the GCI instance
		_gameCenter = new GCI();
		
		//Manually call init, sure.
		_gameCenter.Initalize();
		
#endif
		
		//Create the array of scores to store loaded scores in
		//_loadedScores = new LeaderBoardScore[ListCount];
		
		
//		//Load top leaderboard for today.
//		LoadLeaderboard("Highscore_PPS");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	
	
	#region Interface Functions
	
	public void LoadAchievements()
	{
		
	}
	
	public void LoadLeaderboard(string ID)
	{
		
#if UNITY_IPHONE
		
		_gameCenter.LoadLeaderboard("Highscore_PPS");
		
#endif
		
	}
	
	public void ProcessLeaderboardScores()
	{
#if UNITY_IPHONE
	
		_gameCenter.ProcessScores();
		
#endif
	}
	
	
	public void ShowLeaderboard()
	{
		#if UNITY_IPHONE	
		
		_gameCenter.ShowLeaderboard();
#endif
		
		
		//Social.ShowLeaderboardUI();
	}
	
	
	public void ReportRun(Statistics gameRun)
	{
		
		//Grab the point value and format it to report the score.
		long PPSreportvalue = (long)(gameRun.PPS * 1000.0f);
		long ScoreReportValue = (long)gameRun.Score;
		
	
		
#if UNITY_IPHONE
		
		
		_gameCenter.ReportScore(PPSreportvalue,"Highscore_PPS");
		_gameCenter.ReportScore(ScoreReportValue,"Highscore_Points");
		
		
		
#endif
				
		
	}
	
	#endregion
	
	
	void OnDestroy()
	{
#if UNITY_IPHONE
	
		
		//Call de init since we are jsut a regular class.
		_gameCenter.DeInit();
		
#endif
	}
	
	
	
	public int FriendSize 
	{
		get { return ListCount;}
	}
	
}
