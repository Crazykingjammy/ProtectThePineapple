using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_IPHONE
using Prime31;


#endif



public class GCI 
{
#if UNITY_IPHONE
	
	private List<GameCenterLeaderboard> _leaderboards;
	private List<GameCenterAchievementMetadata> _achievementMetadata;
	private List<GameCenterPlayer> _friends = null;
	private List<GameCenterScore> _scores = null;
	
	public void Initalize()
	{
		// Listens to all the GameCenter events.  All event listeners MUST be removed before this object is disposed!
		// Player events
		GameCenterManager.loadPlayerDataFailed += loadPlayerDataFailed;
		GameCenterManager.playerDataLoaded += playerDataLoaded;
		GameCenterManager.playerAuthenticated += playerAuthenticated;
		GameCenterManager.playerFailedToAuthenticate += playerFailedToAuthenticate;
		GameCenterManager.playerLoggedOut += playerLoggedOut;
		GameCenterManager.profilePhotoLoaded += profilePhotoLoaded;
		GameCenterManager.profilePhotoFailed += profilePhotoFailed;
		
		// Leaderboards and scores
		GameCenterManager.loadCategoryTitlesFailed += loadCategoryTitlesFailed;
		GameCenterManager.categoriesLoaded += categoriesLoaded;
		GameCenterManager.reportScoreFailed += reportScoreFailed;
		GameCenterManager.reportScoreFinished += reportScoreFinished;
		GameCenterManager.retrieveScoresFailed += retrieveScoresFailed;
		GameCenterManager.scoresLoaded += scoresLoaded;
		GameCenterManager.retrieveScoresForPlayerIdFailed += retrieveScoresForPlayerIdFailed;
		GameCenterManager.scoresForPlayerIdLoaded += scoresForPlayerIdLoaded;
		
		// Achievements
		GameCenterManager.reportAchievementFailed += reportAchievementFailed;
		GameCenterManager.reportAchievementFinished += reportAchievementFinished;
		GameCenterManager.loadAchievementsFailed += loadAchievementsFailed;
		GameCenterManager.achievementsLoaded += achievementsLoaded;
		GameCenterManager.resetAchievementsFailed += resetAchievementsFailed;
		GameCenterManager.resetAchievementsFinished += resetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailed += retrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoaded += achievementMetadataLoaded;
		
		// Challenges
		GameCenterManager.localPlayerDidSelectChallengeEvent += localPlayerDidSelectChallengeEvent;
		GameCenterManager.localPlayerDidCompleteChallengeEvent += localPlayerDidCompleteChallengeEvent;
		GameCenterManager.remotePlayerDidCompleteChallengeEvent += remotePlayerDidCompleteChallengeEvent;
		GameCenterManager.challengesLoadedEvent += challengesLoadedEvent;
		GameCenterManager.challengesFailedToLoadEvent += challengesFailedToLoadEvent;
	}
	
	
	public void DeInit()
	{
		// Remove all the event handlers
		// Player events
		GameCenterManager.loadPlayerDataFailed -= loadPlayerDataFailed;
		GameCenterManager.playerDataLoaded -= playerDataLoaded;
		GameCenterManager.playerAuthenticated -= playerAuthenticated;
		GameCenterManager.playerLoggedOut -= playerLoggedOut;
		GameCenterManager.profilePhotoLoaded -= profilePhotoLoaded;
		GameCenterManager.profilePhotoFailed -= profilePhotoFailed;
		
		// Leaderboards and scores
		GameCenterManager.loadCategoryTitlesFailed -= loadCategoryTitlesFailed;
		GameCenterManager.categoriesLoaded -= categoriesLoaded;
		GameCenterManager.reportScoreFailed -= reportScoreFailed;
		GameCenterManager.reportScoreFinished -= reportScoreFinished;
		GameCenterManager.retrieveScoresFailed -= retrieveScoresFailed;
		GameCenterManager.scoresLoaded -= scoresLoaded;
		GameCenterManager.retrieveScoresForPlayerIdFailed -= retrieveScoresForPlayerIdFailed;
		GameCenterManager.scoresForPlayerIdLoaded -= scoresForPlayerIdLoaded;
		
		// Achievements
		GameCenterManager.reportAchievementFailed -= reportAchievementFailed;
		GameCenterManager.reportAchievementFinished -= reportAchievementFinished;
		GameCenterManager.loadAchievementsFailed -= loadAchievementsFailed;
		GameCenterManager.achievementsLoaded -= achievementsLoaded;
		GameCenterManager.resetAchievementsFailed -= resetAchievementsFailed;
		GameCenterManager.resetAchievementsFinished -= resetAchievementsFinished;
		GameCenterManager.retrieveAchievementMetadataFailed -= retrieveAchievementMetadataFailed;
		GameCenterManager.achievementMetadataLoaded -= achievementMetadataLoaded;
		
		// Challenges
		GameCenterManager.localPlayerDidSelectChallengeEvent -= localPlayerDidSelectChallengeEvent;
		GameCenterManager.localPlayerDidCompleteChallengeEvent -= localPlayerDidCompleteChallengeEvent;
		GameCenterManager.remotePlayerDidCompleteChallengeEvent -= remotePlayerDidCompleteChallengeEvent;
		GameCenterManager.challengesLoadedEvent -= challengesLoadedEvent;
		GameCenterManager.challengesFailedToLoadEvent -= challengesFailedToLoadEvent;
	}
	
	
	
	#region Public Interface
	
	public void Authenticate()
	{
		GameCenterBinding.authenticateLocalPlayer();
	}
	
	public void ReportScore(long score, string ID)
	{
		GameCenterBinding.reportScore(score,ID);
		
		
	}
	
	public void LoadLeaderboard(string ID)
	{
		GameCenterBinding.retrieveScores(true,GameCenterLeaderboardTimeScope.Today,1, 10,ID);
	}
	
	public void ShowLeaderboard()
	{
		GameCenterBinding.showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope.AllTime);
	}
	
	#endregion
	
	
	#region Player Events
	
	void playerAuthenticated()
	{
		Debug.Log( "playerAuthenticated" );
		
		GameCenterBinding.loadProfilePhotoForLocalPlayer();
		
		GameCenterBinding.retrieveFriends( true,false);
	}
	
	
	void playerFailedToAuthenticate( string error )
	{
		Debug.Log( "playerFailedToAuthenticate: " + error );
	}
	
	
	void playerLoggedOut()
	{
		Debug.Log( "playerLoggedOut" );
	}
	

	void playerDataLoaded( List<GameCenterPlayer> players )
	{
		Debug.Log( "playerDataLoaded" );
		foreach( GameCenterPlayer p in players )
		{
			
			Debug.Log( p );
			
		}
		
		
		
		//Set the list.
		_friends = players;
	}
	
	
	void loadPlayerDataFailed( string error )
	{
		Debug.Log( "loadPlayerDataFailed: " + error );
	}
	
	
	void profilePhotoLoaded( string path )
	{
		Debug.Log( "profilePhotoLoaded: " + path );
	}
	
	
	void profilePhotoFailed( string error )
	{
		Debug.Log( "profilePhotoFailed: " + error );
	}
	
	#endregion;
	
	
	
	#region Leaderboard Events
	
	void categoriesLoaded( List<GameCenterLeaderboard> leaderboards )
	{
		Debug.Log( "categoriesLoaded" );
		foreach( GameCenterLeaderboard l in leaderboards )
			Debug.Log( l );
		
		//Set the leaderboards.
		_leaderboards = leaderboards;
	}
	
	
	void loadCategoryTitlesFailed( string error )
	{
		Debug.Log( "loadCategoryTitlesFailed: " + error );
	}
	
	#endregion;

	
	#region Score Events
	
	void scoresLoaded( List<GameCenterScore> scores )
	{

		//Check for the leaderboard category here and call the right process function.

		_scores = scores;

	

		
		ProcessScores();
		
	}
	
	
	void retrieveScoresFailed( string error )
	{
		Debug.Log( "retrieveScoresFailed: " + error );
	}
	
	
	void retrieveScoresForPlayerIdFailed( string error )
	{
		Debug.Log( "retrieveScoresForPlayerIdFailed: " + error );
	}
	
	
	void scoresForPlayerIdLoaded( List<GameCenterScore> scores )
	{
		Debug.Log( "scoresForPlayerIdLoaded" );
		foreach( GameCenterScore s in scores )
			Debug.Log( s );
	}
	
	
	void reportScoreFinished( string category )
	{
		Debug.Log( "reportScoreFinished for category: " + category );
	}
	

	void reportScoreFailed( string error )
	{
		Debug.Log( "reportScoreFailed: " + error );
	}
	
	#endregion;
	
	
	#region Achievement Events

	void achievementMetadataLoaded( List<GameCenterAchievementMetadata> achievementMetadata )
	{
		Debug.Log( "achievementMetadatLoaded" );
		foreach( GameCenterAchievementMetadata s in achievementMetadata )
			Debug.Log( s );
	}
	
	
	void retrieveAchievementMetadataFailed( string error )
	{
		Debug.Log( "retrieveAchievementMetadataFailed: " + error );
	}
	
	
	void resetAchievementsFinished()
	{
		Debug.Log( "resetAchievmenetsFinished" );
	}
	
	
	void resetAchievementsFailed( string error )
	{
		Debug.Log( "resetAchievementsFailed: " + error );
	}
	
	
	void achievementsLoaded( List<GameCenterAchievement> achievements )
	{
		Debug.Log( "achievementsLoaded" );
		foreach( GameCenterAchievement s in achievements )
			Debug.Log( s );
	}
	

	void loadAchievementsFailed( string error )
	{
		Debug.Log( "loadAchievementsFailed: " + error );
	}
	
	
	void reportAchievementFinished( string identifier )
	{
		Debug.Log( "reportAchievementFinished: " + identifier );
	}
	
	
	void reportAchievementFailed( string error )
	{
		Debug.Log( "reportAchievementFailed: " + error );
	}
	
	#endregion;
	
	
	#region Challenges
	
	public void localPlayerDidSelectChallengeEvent( GameCenterChallenge challenge )
	{
		Debug.Log( "localPlayerDidSelectChallengeEvent : " + challenge );
	}
	
	
	public void localPlayerDidCompleteChallengeEvent( GameCenterChallenge challenge )
	{
		Debug.Log( "localPlayerDidCompleteChallengeEvent : " + challenge );
	}
	
	
	public void remotePlayerDidCompleteChallengeEvent( GameCenterChallenge challenge )
	{
		Debug.Log( "remotePlayerDidCompleteChallengeEvent : " + challenge );
	}
	
	
	void challengesLoadedEvent( List<GameCenterChallenge> list )
	{
		Debug.Log( "challengesLoadedEvent" );
		Prime31.Utils.logObject( list );
	}
	
	
	void challengesFailedToLoadEvent( string error )
	{
		Debug.Log( "challengesFailedToLoadEvent: " + error );
	}
	
	#endregion
	
	
	#region Internal Functions
	public void ProcessScores()
	{
		//Lets clear the list and add the scores as they are loaded.
		//Grab the scores and store them into the loaded scores.
		List<SocialCenter.LeaderBoardScore> loadedscores = SocialCenter.Instance._loadedScores;
		loadedscores.Clear();
		
		
		int index = 0;
		foreach( GameCenterScore s in _scores )
		{
	
			//Create a new score.
			SocialCenter.LeaderBoardScore newScore = new SocialCenter.LeaderBoardScore();
			
			//Set each one.
			newScore.gcScore = s;
		
			Debug.LogError("CATEGORY!  ::::: " + s.category);
		
			//Deady but a nested loop here to check for photos and who the current player is.
			if(_friends != null)
				foreach(GameCenterPlayer p in _friends)
			{
				//Set the profile photo. and break this loop since we found the player.
				if(p.playerId == s.playerId)
				{
				
					//We will attempt to load the image right here and store it in the score.
					newScore.ProfilePicturePath = p.profilePhotoPath;				
					break;
					
					
				}
				
			}
			
			//Set player alias to ME if its ourselves.
			if(GameCenterBinding.playerIdentifier() == s.playerId)
				newScore.Alias = "Me";
			
			//Add to the list.
			loadedscores.Add(newScore);
			
			Debug.Log( s );
			index++;
			
		}
		
	}
	
	#endregion

#endif
}
