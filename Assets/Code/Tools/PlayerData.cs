using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour {
	
	public float FileVersion;
			
	[System.Serializable]
	public class InfiniteModeData
	{
		//List of phase data that is filled up when the phases advance.
		public List<BasePhase.PhaseData> PhaseList;
			
		public void Reset()
		{
			PhaseList.Clear();
		}
	}
	
	#region Data
	
	//Game Data//
	
	//Game Statistics.
	public Statistics BlankStatistics;
	
	
	//Players game data.
	Statistics _gameStatistics = null;
	
	Stack<Statistics> _gameHistory = null;
	
	//This is to keep track of the amount of gems the player has. not the total value accumulated without spending.
	int _gemBank, _localTrophyCountcache, _maxTrophyCount, _totalXP, _gemCart;
	bool _tutorial = false;
	
	public BaseItemCard[] CardDeck;
	bool cardInit = false;

	//Data for player leveling system
	public PlayerLevelSystem PTPLevelingPrefab;
	PlayerLevelSystem _myLeveling = null;

	//End Game Data//
	
	//Mode Data Structures.
	public OptionData Options;
	public InfiniteModeData _infinteModeData;
		
	public EntityFactory.CannonTypes[] _cannonSlots;
	public ItemSlot[] _itemSlots;
	ItemSlot[] _gameSlots = null;

	
	ControlsLoader.DEVICE _device = ControlsLoader.DEVICE.NULL;
	public ControlsLoader.DEVICE currentDevice
	{
		get{return _device;}
		set{_device = value;}
	}
	
	
	
	#endregion
	
	#region Core Functions
	// Use this for initialization
	void Start () {
		
	//	Debug.LogError("Player Data Start");
		
		if(_gameStatistics == null)
		{
			//Instantuate the player statistics.
			_gameStatistics = Instantiate(BlankStatistics) as Statistics;
			_gameStatistics.transform.parent = transform;
			
			_gameStatistics.gameObject.name = "TotalGameStatistics";
			_gameStatistics.ContainerLabel = "All Game Stats";
			
			//Set the multiplier to none since its full game stats.
			_gameStatistics.ValueMultiplier = 0;
			
			_gameStatistics.LoadFromPlayerPrefs();
			
			DontDestroyOnLoad(_gameStatistics.gameObject);	
			
		}
		
		if(_gameHistory == null)
		{
			//Create a list for the game histroy.
			_gameHistory = new Stack<Statistics>();
		}

		if(PTPLevelingPrefab && !MyPTPLevel)
		{
			//Create the level object.
			_myLeveling = Instantiate(PTPLevelingPrefab) as PlayerLevelSystem;
			_myLeveling.transform.parent = transform;

			DontDestroyOnLoad(_myLeveling.gameObject);
		}

		//Create the array.
		_gameSlots = new ItemSlot[_itemSlots.Length];
		ItemToGameSlots();


		//Fill up infinite modes phase mission with data from the item cards.
		if(!cardInit)
		InitalizeItemCards();	
		
		
	}
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		
		
		//Check for file version and delete if not availabe.
		
		Debug.Log("!!!! -- App PAth: " + Application.dataPath + " -- !!!");
	}
	
	// Update is called once per frame
	void Update () {
	
		
		//Always calculate trophies earned.
		//CalculateTrophiesEarned();
	}
	
	void InitalizeItemCards()
	{
		//Se tthe name of the card deck
		
		//Here we create the item cards and load their data.
		for(int i = 0; i < CardDeck.Length; i++)
		{
			CardDeck[i] = Instantiate(CardDeck[i]) as BaseItemCard;
			CardDeck[i].transform.parent = transform;
			CardDeck[i].LoadCardItem();
		}
		
		Debug.Log("Initialized Cards");
		
		cardInit = true;
	}
	
	int FindCannonCard(EntityFactory.CannonTypes type)
	{
		int index = 0;
		foreach(BaseItemCard card in CardDeck)
		{
			//If we match return the index we are at.
			if(card.isCannonItem(type))
				return index;
			
			//Increment the counter
			index++;
				
		}
		
		return -1;
	}
	
	void CalculateTrophiesEarned()
	{
		//Just go through and add the trophies earned in the card dek.
		
		//Lets start with 0 always. shall we. clearing out the cache.
		_localTrophyCountcache = 0;
		_maxTrophyCount = 0;
		
		foreach(BaseItemCard card in CardDeck)
		{
			card.CalculateTrophies();
			_localTrophyCountcache += card.TrophiesEarned;
			
			if(card.Unlocked)
				_maxTrophyCount += card.TrophyCount;
		}
	}
	
	void LoadSlots()
	{
		//Load cannon slot data.
		int index = 0;
		foreach(EntityFactory.CannonTypes type in _cannonSlots)
		{
			//Grab the name
			string slotname = "Cannon_Slot_" + index.ToString();
			
			//if the key dont exist set to zebra!!!!!
			if(!FileFetch.HasKey(slotname))
			{
				_cannonSlots[index] = EntityFactory.CannonTypes.Zebra;
				index++;
				continue;
			}
			
			//Set the int.
			_cannonSlots[index] = (EntityFactory.CannonTypes) FileFetch.FetchInt(slotname);
			
			
			index++;
		}
		
	}



	public void AssignSlot(EntityFactory.CannonTypes type, int index)	
	{

		//Set the slot type.
		_itemSlots[index].Type = type;
		_itemSlots[index].IconName = "Thermometer-Units";

		//Assign the icon name
		if(type != EntityFactory.CannonTypes.NULL)
		_itemSlots[index].IconName = EntityFactory.GetEF().GetCannonIconName(type);


		//Mark Dirty or apply to save here.
		
		//Grab the name
		//string slotname = "Cannon_Slot_" + index.ToString();
			
		//Set the int.
		//FileFetch.SetInt(slotname,(int)type);

		
	}
	
	public EntityFactory.CannonTypes[] CannonSlots
	{
		get{return _cannonSlots;}
	}

	public ItemSlot[] ItemSlots
	{
		get {return _itemSlots;}
	}

	public ItemSlot[] GameSlots
	{
		get{return _gameSlots;}
	}

	void ItemToGameSlots()
	{
		return;

		if(_gameSlots == null)
			return;

		int i = 0;

		foreach(ItemSlot slot in _itemSlots)
		{
			_gameSlots[i].IconName = slot.IconName;
			_gameSlots[i].Type = slot.Type;
			i++;
		}
	}


	public void ClearGameSlots()
	{
		//Lets copy before every clear.
		ItemToGameSlots();

		foreach(ItemSlot i in _itemSlots)
				i.Clear();

		//Spend that money here!
		GemBank -= GemCart;

		//Clear the cart amount as well.
		GemCart = 0;

		//Alays set the default slot upon assign.
		AssignSlot(EntityFactory.CannonTypes.Zebra,0);
	}

	public CannonItemCard FindCardByCannonType(EntityFactory.CannonTypes type)
	{
		//Get the index.
		int cardindex = FindCannonCard(type);
		
		if(cardindex == -1)
			return null;
		
		return CardDeck[cardindex] as CannonItemCard;
	}
	
	
	#endregion
	
	
	public OptionData GameOptions
	{
		get{
			return Options;
		}
	}
	
	
	
	#region Infinte Mode Breathless Functions

	
	public InfiniteModeData Breathless
	{
		get{return _infinteModeData;}
		set {
			_infinteModeData.Reset();
		}
	}
	
	
	public void PunchPhaseTime(float t, EntityFactory.CannonTypes cannonCompletion)
	{
		int index = 0;
		int cardIndex = -1;
		
		//if we have somthign in the list we set the timer to the last phase in the list.
		if(_infinteModeData.PhaseList.Count > 0)
		{
			//Grabbing index for the current phase, which is the one that should be getting punched.
			index = _infinteModeData.PhaseList.Count - 1;
			
			//Setting time for the completion punch.
			_infinteModeData.PhaseList[index].PhaseCompletionPunch = t;
			
			//Set the cannon type in the statistics.
			_infinteModeData.PhaseList[index].PhaseStatistics.CompletionCannon = cannonCompletion;
			
			//We should stop the time for the stats here too.
			_infinteModeData.PhaseList[index].PhaseStatistics.StopTimer();
			
			//Lets add the phases stats to the main game stats.
			//GameObjectTracker.GetGOT().gameRunStatistics.AddStatistics(_infinteModeData.PhaseList[index].PhaseStatistics);
			
			//Perform a test on the completion cannon if there is a matching item card.
			cardIndex = FindCannonCard(cannonCompletion);
			
			//If we didnt return a proper value just leave as there is nothing.
			if(cardIndex == -1)
				return;
			
			
			//We have found a match if we made it here. Lets set the card dirty to be updated.
			CardDeck[cardIndex].IsDirty = true;
			
			//Since the phase data is for the given item card. lets add the statistics.
			CardDeck[cardIndex].ItemStats.AddStatistics(_infinteModeData.PhaseList[index].PhaseStatistics);
			
			//Grab the mission
			BaseMission m = CardDeck[cardIndex].BreathlessMission;
			
			//here we check if we have a breathless mission, or we return.
			if(!m)
				return;
			
			
			//Lets check if our mission objectives are out of the range we willt ry to access it at.
			if(m.MissionObjectives[index] == null)
			{
				print("Trying to assign a phase stat for a mission that is not there!");
				return;
			}
			
			//Set the objective at the same index as phase statistics.
			m.MissionObjectives[index].ObjectiveStatistics = _infinteModeData.PhaseList[index].PhaseStatistics;
			
			//SaveItemCardData();
		}
		
	}
	
	public void ApplyRunStatsToCards(Statistics stats)
	{

		//Sets add on the time and score to the full stats count.		
		_gameStatistics.Score += stats.Score;
		_gameStatistics.TimeAmount += stats.TimeAmount;
		
		//Add the stats to the game histroy
		_gameHistory.Push(stats);

		//Apply score to the xp.
		TotalXP += stats.Score;

		//Get the completion cannon!
		 BaseItemCard card =  FindCardByCannonType(stats.CompletionCannon);
		
		//Check if the card has ac annon. it may not.
		if(card != null)
		card.ApplyStatsToMission(stats,"PTPMission");
		

	}
	
	public void ApplyPhaseStatsToCards()
	{
		//Check the stats cannon completion and find the item card and feed its breathless mission the stats at the index.
		
		int cardindex = -1;
		int i = 0;
		
		//Find matching item card at cannon type and grab the index.
		foreach(BasePhase.PhaseData pd in _infinteModeData.PhaseList)
		{
			cardindex = FindCannonCard(pd.PhaseStatistics.CompletionCannon);
			
			//If we didnt return a proper value just leave as there is nothing.
			if(cardindex == -1)
				return;
			
			//We have found a match if we made it here. Lets set the card dirty to be updated.
			CardDeck[cardindex].IsDirty = true;
			
			//Since the phase data is for the given item card. lets add the statistics.
			CardDeck[cardindex].ItemStats.AddStatistics(pd.PhaseStatistics);
			
			//Grab the mission of the card type.
			BaseMission m = CardDeck[cardindex].BreathlessMission;
			
			//here we check if we have a breathless mission, or we return.
			if(!m)
				return;
			
			
			//Lets check if our mission objectives are out of the range we willt ry to access it at.
			if(m.MissionObjectives[i] == null)
			{
				print("Trying to assign a phase stat for a mission that is not there!");
				return;
			}
			
			//Set the objective at the same index as phase statistics.
			// TODO: try not to overwrite if the stats are lesser.
			m.MissionObjectives[i].ObjectiveStatistics = pd.PhaseStatistics;
			
			//Incrementor so we can access the index of the mission objectives as well.
			i++;
			
		}
		
		
		
		
	}
	
	public void SaveItemCardData()
	{
		//Go through every card. And save every mission.
		foreach(BaseItemCard card in CardDeck)
		{
			//Only go through missions for dirty cards.
			card.SaveCardItem();	
		}
	}
	
	
	public void LoadAllDataFromFileSystem()
	{
	
		if(FileFetch.FetchFloat("VERSION") < FileVersion)
			FileFetch.ClearAllKeys();
		
		//Destroy the old if we have one
		if(_gameStatistics != null)
			Destroy(_gameStatistics.gameObject);

		
		//Instantuate the player statistics.
		_gameStatistics = Instantiate(BlankStatistics) as Statistics;
		_gameStatistics.transform.parent = transform;
		
		_gameStatistics.gameObject.name = "TotalGameStatistics";
		_gameStatistics.ContainerLabel = "All Game Stats";
		
		_gameStatistics.LoadFromPlayerPrefs();
		
		DontDestroyOnLoad(_gameStatistics.gameObject);
		
		
		//Load the gem bank
		_gemBank = FileFetch.FetchInt("GEMBANK");

		//Load the total xp
		_totalXP = FileFetch.FetchInt("TOTALXP");
		//_totalXP = 0;

		//Set the total xp into the leveling system to calculate.
		if(!MyPTPLevel)
		{
			//Create the level object.
			_myLeveling = Instantiate(PTPLevelingPrefab) as PlayerLevelSystem;
			_myLeveling.transform.parent = transform;
			DontDestroyOnLoad(_myLeveling.gameObject);
		}

		TotalXP = _totalXP;
	
	
		
		foreach(BaseItemCard card in CardDeck)
		{
			//Only go through missions for dirty cards.
			card.LoadCardItem();
		}
		
		//Load slots here
		//LoadSlots();
		ClearGameSlots();
		
		//Handle options and other data here.
		Options.Load();
		
		//SocialCenter.Instance.LoadLeaderboard("Highscore_PPS");
		
	}
	
	public void SaveAllData()
	{
		if(_gameStatistics != null)
		{
			//Save total game stats.
			_gameStatistics.SaveToPlayerPrefs();
		}
		
		//Set the version
		FileFetch.SetFloat("VERSION",FileVersion);
		
		//Save item card data.
		SaveItemCardData();
		
		//Save the options data.
		Options.Save();
		
		//Save some more of players data.
		FileFetch.SetInt("GEMBANK",_gemBank);

		//Save total XP earned.
		FileFetch.SetInt("TOTALXP", _totalXP);
		
		
		//Sync after we save.
		//SyncAllData();
		
		Debug.Log(Application.persistentDataPath + "/PlayerPrefs.txt");
		
		
	}
		
	public void DeleteAllData()
	{
		//Delete from the file system.
		FileFetch.ClearAllKeys();
		
		//Set the file version upon deleting.
		FileFetch.SetFloat("VERSION",FileVersion);
		
		//We have to delete the stats in memory.
		if(_gameStatistics)
			DestroyObject(_gameStatistics.gameObject);
		
		
		int index = 0;
		foreach(EntityFactory.CannonTypes type in _cannonSlots)
		{
			//Set to default cannons.
			ToyBox.GetPandora().AssignSlot(EntityFactory.CannonTypes.Zebra,index);
	
			index++;
		}
		
		
		LoadAllDataFromFileSystem();
		
		//Go through each card and recalculate the trophies earned
		
		//Recalculate trophies.
		CalculateTrophiesEarned();
		
		print("All Data Deleted :(((( ");
	}
	
	public void AddPhase(BasePhase.PhaseData data)
	{
		_infinteModeData.PhaseList.Add(data);
		
	}
	
	public void SyncAllData()
	{
		FileFetch.Sync();
	}
	
	//Make sure to apply any card data to the player prefs if the application quits.
	public void OnApplicationQuit() 
	{
		ToyBox.GetPandora().TimePaused = true;
		SaveAllData();
	}
	
	public void OnApplicationPause()
	{
		//SaveAllData();
	}
	
	public void OnApplicationFocus()
	{
		//Lets try to sync when we jump into focus.
		FileFetch.Sync();
		
	}
	
	public Statistics GameStatistics
	{
		get
		{		
			return _gameStatistics;
		}
		
	}
	
	public bool IsTutorial
	{
		get { return _tutorial;}
		set { _tutorial = value;}
	}
	public int GemBank
	{
		get {return _gemBank;}
		set {_gemBank = value;}
	}
	public int GemCart
	{
		get{return _gemCart;}
		set{_gemCart = value;}
	}

	
	public int TrophiesEarned
	{
		
		get {
			CalculateTrophiesEarned();
			return _localTrophyCountcache;}
	}
	
	public int MaxTrophies
	{
		get{return _maxTrophyCount;}
	}
	
	public Stack<Statistics> GameHistory
	{
		get{return _gameHistory;}
	}
	
	#endregion


	#region PlayerLevelingData Functions

	public PlayerLevelSystem MyPTPLevel
	{
		get{

			if(!_myLeveling)
			{
				Debug.LogError("There is no leveling data present!");
				return null;
			}


			return _myLeveling;
		}

	}

	public int TotalXP
	{
		get{ return _totalXP;}
		set{ 
			_totalXP = value;
			//_myLeveling.TotalXP = _totalXP;
			if(MyPTPLevel)
				MyPTPLevel.TotalXP = _totalXP;
		}
	}

	public int MyCurrentLevel
	{
		get{return _myLeveling.Level;}
	}

	#endregion
}
