using UnityEngine;
using System.Collections;

public class VisualWorld : MonoBehaviour {

	//Small class to handle specific data for animations.
	[System.Serializable]
	public class LevelTransitionAnimations
	{
		public string IntroAnimationName = "Warp";
		public string ExitAnimationName = "FlipAway";
	}
	
	public float RoomChangeSpeed = 0.14f;
	public float BossFinishSpeed = 0.1945f;
	
	
	public GameObject[] Levels;
	public LevelTransitionAnimations[] LevelAnimationData;

	public GameObject[] BossRooms;
	public LevelTransitionAnimations[] BossAnimationData;

	public string DefaultAnimationName = "Warp";
	
	
	GameObject BossRoom;
	GameObject Window;
	GameObject HeavyRoom;
	GameObject GameRoom;
	
	
	Vector3 moonPosition;
	
	//Local variables for use.
	GameObject _currentRoom = null;
	GameObject _transitionRoom = null;
	GameObject _entryRoom = null;
	GameObject _bossRoom = null;
	int _levelIndex = 0;
	string _transitionAnimationName = "Warp";
	string _exitTransitionAnimationName = "Warp";
	bool _inTransition = false;
	bool _isBossLevel = false;
	bool _NextLevelFromBoss = false;



	enum RoomState
	{
		Game,
		Boss,
		Heavy,
		Count
	}
	
	// Use this for initialization
	void Start () {
		
		//Disable all boss rooms upon start.
		foreach(GameObject go in BossRooms)
		go.SetActive(false);
		
		//Disable all the levels upon start.
		foreach(GameObject level in Levels)
		level.SetActive(false);

	
		_entryRoom =transform.Find("EntryRoom").gameObject;
	
		if(!_entryRoom.activeSelf)
		{
			_entryRoom.SetActive(true);
			_currentRoom = _entryRoom;
		}
	

		_bossRoom = BossRooms[0];

//		_currentRoom = Levels[0];
//		_currentRoom.SetActive(true);


	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.J))
		{
			//GotoHeavyRoom();
			GotoNextRoom();
		}
		
		//Do some animation tests.
		if(Input.GetKeyDown(KeyCode.K))
		{
			ActivateBossRoom(true);
			//ToyBox.GetPandora().SetSlowMotion(true, 0.12f);
		}
		
		if(Input.GetKeyDown(KeyCode.L))
		{
			ActivateBossRoom(false);
			//ToyBox.GetPandora().SetSlowMotion(true, 0.1945f);
		}
		
		

		if(!_currentRoom.GetComponent<Animation>().isPlaying && _inTransition)
		{
			EndRoomTransition();

			//If we are comming from a boss level then we set the flag to false.
			if(_isBossLevel)
			{
				_isBossLevel = false;

				//If there is a flag to level up from boss level go to the next room.
				if(_NextLevelFromBoss)
					GotoNextRoom();
			}



		}
		
		

		
	}

	void ComputeTransitionAnimationName(string inputName)
	{
		_transitionAnimationName = "RiseUp";
		_exitTransitionAnimationName = "Warp";

		_transitionAnimationName = LevelAnimationData[_levelIndex].IntroAnimationName;
		_exitTransitionAnimationName = LevelAnimationData[_levelIndex].ExitAnimationName;

	}
	
	public void GotoNextRoom()
	{

		//Handle if we are in a boss room
		if(_isBossLevel)
		{
			ActivateBossRoom(false);

			//Mark flag to trigger a level upgrade upon boss level deactiate.
			_NextLevelFromBoss = true;

			return;

		}

		//Check the boundaries of the index.
		if(_levelIndex >= Levels.Length)
			_levelIndex = 0;
	

		ComputeTransitionAnimationName("null");

		//Set the reference for hte transition room.
		_transitionRoom = Levels[_levelIndex];

		//here we incriment the level index since we just used the one assigned.
		_levelIndex++;

		
		//Here we check if the entry room is active.
		if(_entryRoom.activeSelf)
		{
			//If the entry room is active then begin the level transitions.
			//_levelIndex = 0;

			_entryRoom.SetActive(false);
			_currentRoom = Levels[Levels.Length - 1];
			_currentRoom.SetActive(true);

		}


		//Handle the animation playing code here
		//Set the timer for the callback.
		_inTransition = true;

		//Enable the transition room
		_transitionRoom.SetActive(true);

		
		//Here we slow down time now.
		ToyBox.GetPandora().SetSlowMotion(true, RoomChangeSpeed);
		
		//Let the transition level begin at the end and play backward.
		//Set the speed to backward and the time to the end of the animation
		//Play the transition animation
		_transitionRoom.GetComponent<Animation>()[_transitionAnimationName].speed = -1.0f;
		_transitionRoom.GetComponent<Animation>()[_transitionAnimationName].time = _transitionRoom.GetComponent<Animation>()[_transitionAnimationName].length;
		_transitionRoom.GetComponent<Animation>().Play(_transitionAnimationName);




		//Play out the current room animation.
		_currentRoom.GetComponent<Animation>()[_exitTransitionAnimationName].speed = 1.0f;
		_currentRoom.GetComponent<Animation>()[_exitTransitionAnimationName].time = 0.0f;
		_currentRoom.GetComponent<Animation>().Play(_exitTransitionAnimationName);




	}

	void EndRoomTransition()
	{
		//Deactivate the current room which is now tranition out.
		_currentRoom.SetActive(false);

		//Update the reference of the current room.
		_currentRoom = _transitionRoom;

		//Set the bool.
		_inTransition = false;
	}



	
	public void GotoHeavyRoom()
	{
		//We will make sure main game room is activate for testing
		GameRoom.SetActive(true);
		
		//turn off the boss room please
		BossRoom.SetActive(false);
		
		//set the heavy room to true.
		HeavyRoom.SetActive(true);			
		ToyBox.GetPandora().SetSlowMotion(true, RoomChangeSpeed);
		GetComponent<Animation>()["TransitionHeavy"].speed = 1.0f;
		GetComponent<Animation>().Play("TransitionHeavy");
		
	}
	
	void HeavyRoomTransitionComplete()
	{
		///Deactivate the game room.
		GameRoom.SetActive(false);
	}
	
	public void ActivateBossRoom(bool active)
	{
		

		//Play the activating animation and objects.
		if(active)
		{
			//Enable the object.
		
			_bossRoom.SetActive(true);

			//Set the bool variable
			_isBossLevel = true;
			
			ToyBox.GetPandora().SetSlowMotion(true, RoomChangeSpeed);
			
			//Set the speed to forward.
			_bossRoom.GetComponent<Animation>()["BossA"].speed = -1.0f;
			_bossRoom.GetComponent<Animation>()["BossA"].time = _bossRoom.GetComponent<Animation>()["BossA"].length;
				
			_bossRoom.GetComponent<Animation>().Play("BossA");

			_transitionAnimationName  = "Warp";

			//Play out the current room animation.
			_currentRoom.GetComponent<Animation>()[_transitionAnimationName].speed = 1.0f;
			_currentRoom.GetComponent<Animation>()[_transitionAnimationName].time = 0.0f;
			_currentRoom.GetComponent<Animation>().Play(_transitionAnimationName);


			//Call the notification.
			ActivityManager.Instance.BossNotif.ActivateNotification();
				
		}
		else
		{
			//Disable the room effects.
			//_bossRoom.SetActive(false);
			_transitionRoom = _currentRoom;
			_currentRoom = _bossRoom;

			//Set the bool variable
			//_isBossLevel = false;

			if(ToyBox.GetPandora())
				ToyBox.GetPandora().SetSlowMotion(true, BossFinishSpeed);


			_transitionAnimationName = "Warp";
			_exitTransitionAnimationName = "BossA";
			
			//Let the transition level begin at the end and play backward.
			//Set the speed to backward and the time to the end of the animation
			//Play the transition animation
			_currentRoom.GetComponent<Animation>()[_exitTransitionAnimationName].speed = 1.0f;
			_currentRoom.GetComponent<Animation>()[_exitTransitionAnimationName].time = 0.0f;
			_currentRoom.GetComponent<Animation>().Play(_exitTransitionAnimationName);
			
			
			
			
			//Play out the current room animation.
			_transitionRoom.GetComponent<Animation>()[_transitionAnimationName].speed = -1.0f;
			_transitionRoom.GetComponent<Animation>()[_transitionAnimationName].time = _transitionRoom.GetComponent<Animation>()[_transitionAnimationName].length;
			_transitionRoom.GetComponent<Animation>().Play(_transitionAnimationName);

			_inTransition = true;

			//Call the notification.
			ActivityManager.Instance.BossNotif.CloseWindow();
		}
		
		
	}
	
	
}
