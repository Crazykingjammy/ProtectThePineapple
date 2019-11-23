using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A Toy box is the stuff that you can epect to find for every game time. Additional objects may be attached to the script itself.
/// </summary>
/// 


public class ToyBox : MonoBehaviour {	
	
	private static ToyBox Pandora = null;
	
	public AudioPlayer SoundAudioPlayer;
	
	//We can always expect to have a camera
	public MainGameCamera Camera_01 = null;
	
	/// <summary>
	/// Variables for the bot.
	/// </summary>
	//We can expect always to have a bot. 
	public Bot Bot_01;
	
	//we will create also the shield for the bot.
	public SimpleShield BotShield_01;
	
	
	//Create the command center for the bot
	public CommandCenter CommandCenter_01;
	public bool CreateCommandCenter  = true;
	

	//Store the position for the cannon already in the scene.
	//public Cannon CommonCannon;
	
	///////////////////////////////////////////////////////////////////
	
	
	//////////Scene Objects ////////////
	
	public BallStack SceneBallStack;
	
	//////////Scene Objects ////////////
	
	
	//Here we will include managers for Target Spawning and Gem Spawning
	
	//Handle the spawning of gems.
	public GemPotManager Rewards;
	

	
	//List of cannons that will be present in the toybox.
	//public List<Cannon> Cannons;
	
	//public Cannon CannonSlotA, CannonSlotB;
	
	//List of players that are present in the toybox.
	//public List<BKPlayer> BKPlayers;
	
	
	public Joystick Controls;
	
	public UIRoot FUIHud;
	public bool createFUI = true;
	
	public FUI3DManager fui3DManager;
	public bool createFui3D = true;
	
	// Time Variables
	public float timescalenormal = 1.0f;
	public float timescaleslow = 0.19f;
	
	private bool IsSlowMotion = false;
	private float SlowMotionTimer = -1.0f;
	
			
	//Set the vector for the direction to move 
	private Vector3 movingDirection = Vector3.zero;
	private Vector3 forceVector = Vector3.zero;


	// Adding powerup spawner to the game
	// using data from the GOT and accessing pandora, the game can make the calls for when and where to spawn
	// Set in the editor.
	public PowerupSpawner MyPowerUpSpawner = null;






	
	//More to come...
	
	bool deviceCheck = false;







	
	// Use this for initialization
	void Awake () {
		
		//Set ourselves.
		Pandora = this;
		
		TimePaused = true;
//		Debug.LogError("Toybox Start");
		
		//Create an audioplayer if we dont ahve one.
		if(AudioPlayer.GetPlayer() == null)
		{
			SoundAudioPlayer = Instantiate(SoundAudioPlayer) as AudioPlayer;
		}
	
		//Lets just always create the toybox at normal scale.
		//Time.timeScale = timescalenormal;
				
		//Create the gem pot manager
		Rewards = Instantiate(Rewards) as GemPotManager;
		
		if(Controls)
		{
			//Little local storage to test if joystick has been found.
			bool found = false;
			
			//See if there is one in the scene first.
			foreach(Object o in FindObjectsOfType ( typeof(Joystick) ) )
			{
				
				if(o.name == "TOYBOX_JOYSTICK")
				{
					Controls = (Joystick)o;
					found = true;
				}	
			}
			
			//If nothign was found then this is the first time.
			if(!found)
			{
				//Create and name.
				Controls = Instantiate(Controls) as Joystick;
				Controls.name = "TOYBOX_JOYSTICK";
				
				//Keep us alive just because man.
				DontDestroyOnLoad(Controls.gameObject);	
			}
		}
		
		
		///////// Bot Creation ///////
		
		//Create the command center.
		CommandCenter_01 = Instantiate(CommandCenter_01) as CommandCenter;
		
		if(!CreateCommandCenter)
			CommandCenter_01.gameObject.SetActive(false);
		
		//Create the bot
		Bot_01 = Instantiate(Bot_01) as BasicBot;		
		
		
		//Create the shield
		BotShield_01 = Instantiate(BotShield_01) as SimpleShield;
		
		//Attach the shield to the bot.
		
		Bot_01.SetDefenseUnit(BotShield_01);
		
		//Trigger the spawns and store them in the cannon list.

		
		
		///////// Bot Creation ///////

		
		
		/////////Scene Setup//////////
		
//		Camera_01 = GameObjectTracker.instance.GameCamera;
//		
//		
//		if(Camera_01)
//		{
//			//Set teh camera to look at the bot.
//			Camera_01.SetBotToFollow(Bot_01);
//			
//			//Set teh game camera as the current game camera.
//			Camera.SetupCurrent(Camera_01.MyCamera);	
//		}
//		
		//Create the ball stack and yeah...
		SceneBallStack = Instantiate(SceneBallStack) as BallStack;
		
		/////////Scene Setup//////////
		
		
		//Create the NGUI last
				
		if((FUIHud && createFUI )&& ActivityManager.Instance == null)
		{
			FUIHud = Instantiate(FUIHud) as UIRoot;
			
			//What happens if we dont destroy?
			DontDestroyOnLoad(FUIHud.gameObject);
			
		}
		else
		{
			//If the activitiy manager is alrady crated and is not active, then we simply activate.
			if(ActivityManager.Instance.SetActive == false)
			{
				ActivityManager.Instance.SetActive = true;

				//here we should activate the gameplay world as well.
				GameObjectTracker.GetGOT().World.gameObject.SetActive(true);
			}

		}
		
		
		if(fui3DManager && createFui3D)
			fui3DManager = Instantiate(fui3DManager) as FUI3DManager;
		
		if (MyPowerUpSpawner == null){
			Debug.LogError("No powerup spawner in testgame");
		}
//		Debug.LogError("Toybox End");
		
	}
	
	void CheckDevice()
	{
		//Debug.Log("Checking Device");
		if(!Controls)
			return;
		
		if(GameObjectTracker.instance._PlayerData.currentDevice == ControlsLoader.DEVICE.NULL)
			return;
		
		if(GameObjectTracker.instance._PlayerData.currentDevice == ControlsLoader.DEVICE.iPad)
		{
			Debug.Log("Big Position set for Ipad.");
			Controls.JoystickPosition = new Vector2(0.12f,0.22f);
		}
		
		
		deviceCheck = true;
	}
	
	
	static public ToyBox GetPandora()
	{
		if(Pandora != null)
			return Pandora;
		else{
//			Debug.Log("Toybox Not Created");
			return null;
		}
	}
	
	#region Input
	
	//Controls for development.
	public void TestControls()
	{
		
		//if(Application.isEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
		if(true)
		{
		
			//We will just add on a bit of heat as we press the H button
			if(Input.GetKey(KeyCode.H) )
			{
				//Bot_01.Damage(55.0f);
				CommandCenter_01.Damage(55.0f);
			}


			if(Input.GetKeyDown(KeyCode.M) )
			{
				//GameObjectTracker.instance._PlayerData.GemBank += 1000;

				ActivityManager.Instance.FadeToBlack();
			}
			
			//We will just add on a bit of heat as we press the H button
			if(Input.GetKey(KeyCode.C) )
			{
				Bot_01.Cool(50.0f);
			}
			
			if(Input.GetKeyDown(KeyCode.P))
			{
				foreach(string s in Input.GetJoystickNames())
				{
					print(s);
				}
			}
			
			if(Input.GetKeyDown(KeyCode.Alpha1) )
			{
				ClearFreeCannons();
			}
			
			
			if(Input.GetKeyDown(KeyCode.Alpha0) )
			{
				AssignCannonSlotTypes();
			}
			
			
			if( Input.GetButtonDown("Shoot") )
			{
				Bot_01.Shoot();
				//AnimateAttack();
			}

			if(Input.GetButtonDown("OverHeat"))
			{
				Bot_01.AddHeat(1000.0f);
			}

			if( Input.GetButtonDown("Shield") || Input.GetKeyDown(KeyCode.B))
			{
				Bot_01.ActivateDefense();
			}
			
			if( Input.GetButtonUp("Shield") || Input.GetKeyUp(KeyCode.B))
			{
				Bot_01.DeactivateDefense();
			}
							
			//Absolute Input controls for testing purposes
			if(Input.GetAxis("Horizontal") < 0.0f  || Input.GetKeyDown(KeyCode.A))
			{	
				movingDirection += -(Vector3.right) * -Input.GetAxis("Horizontal") ;	
				
				
			}
			if(Input.GetAxis("Horizontal") > 0.0f || Input.GetKeyDown(KeyCode.D))
			{		
				movingDirection += (Vector3.right) * Input.GetAxis("Horizontal") ;
				;
			}
			
			if(Input.GetAxis("Vertical") > 0.0f || Input.GetKeyDown(KeyCode.W))
			{
				movingDirection += (Vector3.forward) * Input.GetAxis("Vertical") ;
				
			}
			
			if(Input.GetAxis("Vertical") < 0.0f || Input.GetKeyDown(KeyCode.S))
			{
				movingDirection += -(Vector3.forward) * -Input.GetAxis("Vertical") ;
				
			}
			

		

			
		
			
		}
		
		//movingDirection.Normalize();
		
		
	}
	
	/// <summary>
	/// This function will grab the actual input from the deivces. 
	/// The input grabbed from the devices will be translated to a direction relative to the camera.
	/// </summary>/
	public void MovementControllerUpdate()
	{
		
		if(!Bot_01)
			return;
		
		//Set the force vector.
		forceVector = Vector3.zero;
		
		
		if(Application.isEditor 
            || Application.platform == RuntimePlatform.OSXPlayer 
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			//forceVector.Scale(movingDirection);
			forceVector = movingDirection;
			
		}
		else
		{
			//Scale by the movement direction of the onscreen controls.
			forceVector = Controls.GetInputDirection();			
		
		}
		
		
		
		//forceVector = movingDirection + Controls.GetInputDirection();
		
		
		//Apply the force to the bot.
		Bot_01.Move(forceVector);
		//AnimateMovement(forceVector);
	
		//Do a clear on the input after we move the bot to get a fresh delta input next frame.
		if(Controls)
			Controls.ClearInputDirection();
			
		
		forceVector.Normalize();
		
		//Zero out the movement
		movingDirection = Vector3.zero;
		
		
		
	}

	void AnimateAttack(){
		
		// we dont want to animate the movement if we're animating the cannon pick up
		if (Bot_01.IsAnimatingCannonPickup){
			return;
		}
		Bot_01.GetComponent<Animation>().Play("Attack");
	}

	void AnimateMovement(Vector3 movementDir){

		// we dont want to animate the movement if we're animating the cannon pick up
		if (Bot_01.IsAnimatingCannonPickup){

			return;
		}

		bool moving = false;

		// do moving animations forward back left right
		if (movementDir.x > 0){
			Bot_01.GetComponent<Animation>().CrossFade("MovingRight");

			moving = true;				

		}
		
		if (movementDir.x < 0){
			//Bot_01.animation.Play("MovingLeft");
			Bot_01.GetComponent<Animation>().CrossFade("MovingLeft");
			moving = true;

		}
		
		if (movementDir.x == 0 && movementDir.z < 0){
			//Bot_01.animation.Blend("MovingBackwards");
			Bot_01.GetComponent<Animation>().CrossFade("MovingBackwards");
			moving = true;
		}
		if (movementDir.x == 0 && movementDir.z > 0){
			//Bot_01.animation.Blend("MovingForward");
			Bot_01.GetComponent<Animation>().CrossFade("MovingForward");

			moving = true;
		}
		
//		if (!moving){
//			Bot_01.animation.CrossFade("Idle");
//		}
	
	}

	
	#endregion
	
	
	
	// Update is called once per frame
	void Update () {
	 		
		if(!Camera_01)
		{
			Camera_01 = GameObjectTracker.instance.GameCamera;
			
			//Set teh camera to look at the bot.
			Camera_01.SetBotToFollow(Bot_01);
			//GameObjectTracker.instance.
			
			//Set teh game camera as the current game camera.
			Camera.SetupCurrent(Camera_01.MyCamera);	
		}
		
		//Perform the basic controller upate of pushing the bots based on their input.
		MovementControllerUpdate();
		
		//We will call our test controls here if we need to.
		TestControls();
		
	
		if(!deviceCheck)
			CheckDevice();
	
		//Handle Time
		//Set the slow motion timer if we are in slow motion.
		if(IsSlowMotion && !TimePaused)
		{	
			//Update the timer
			SlowMotionTimer -= Time.deltaTime;
			
			Time.timeScale = timescaleslow;
			
			if(SlowMotionTimer < 0.0f)
			{
				IsSlowMotion = false;
				Time.timeScale = timescalenormal;
				SlowMotionTimer = -1.0f;
			}
				
		}
		
		
	}
	
	
	
	#region Scene Access
	
	public GemPotManager GetGemManager()
	{
		return Rewards;
	}
	
	public void SpawnGems(int amount, Vector3 position, int worth = 1)
	{
		Rewards.Drop(amount, position, worth);
	}
	
	
	#endregion
	
	public bool TimePaused
	{
		get{
			if(Time.timeScale == 0.0f)
			{
				return true;
			}
			return false;
		}
		
		set{
			
			if(value)
			{
				Time.timeScale = 0.0f;
			}
			else
			{
				Time.timeScale = timescalenormal;
				
			}
			
		}
	}
	
	public void SetSlowMotion(bool slowmotion, float duration = 77.0f)
	{	

		//Handle slow motion setting.
		IsSlowMotion = slowmotion;
		
		//If user pass in false to set it off, set the timer here.
		if(!IsSlowMotion)
		{
			Time.timeScale = timescalenormal;

			//Set the timer to 0 here so we dont have it inflated when we try to use it at a lower number than the default 77.0f.
			SlowMotionTimer = -1.0f;


			//Return here since we need to be no longer setting slwo motion timer.
			return;

		}

		//Only set the timer if our new duration time is larger than the timer we have left.
		if(duration > SlowMotionTimer || SlowMotionTimer ==  -1.0f)
		{
			SlowMotionTimer = duration;
		
		}
	}
	
	
	#region Command Center Functions
	
	
	public void ClearFreeCannons()
	{
	
		if(CommandCenter_01.ClearCannonSlot(0))
		{
			print("First Cannon Slot Cleared!")	;
		//	CannonSlotA = null;
		}
		else
		{
			print("You are trying to delete an attached cannon! - : " + CannonSlotA.name);		
		}
		
		if(CommandCenter_01.ClearCannonSlot(1))
		{
			print("Second Cannon Slot Cleared!")	;
		//	CannonSlotB = null;
		}
		else
		{
			print("You are trying to delete an attached cannon! - : " + CannonSlotB.name);		
		}
		
	}
	
	public void AssignCannonSlotTypes()
	{		
		PlayerData _pd = GameObjectTracker.GetGOT()._PlayerData;

		//Set the cannon types.
		for(int i = 0; i < _pd.ItemSlots.Length; i++)
			CommandCenter_01.SetItemSlot(_pd.ItemSlots[i],i);

		CommandCenter_01.TriggerSlotSpawns();

		//Copy slots to active slot array
		_pd.ClearGameSlots();

		//Clear the slots after assigning 
	}
	
	public void AssignSlot(EntityFactory.CannonTypes type, int index)
	{
		//Assign the player data type.
		GameObjectTracker.GetGOT()._PlayerData.AssignSlot(type,index);
		
		//Set the command center.
		//CommandCenter_01.SetCannonSlot(GameObjectTracker.GetGOT()._PlayerData.CannonSlots[index],index);

		//New code for slots.
		//CommandCenter_01.SetItemSlot(GameObjectTracker.GetGOT()._PlayerData.ItemSlots[index],index);
		
//		//Write to the player data.
//		if(index == 0)
//		{
//			//Temp
//			GameObjectTracker.GetGOT()._PlayerData.CannonSlotA = type;
//			GameObjectTracker.GetGOT()._PlayerData.CannonSlots[index] = type;
//			
//			//Set the command center.
//			CommandCenter_01.SetCannonSlot(GameObjectTracker.GetGOT()._PlayerData.CannonSlotA,index);
//			
//		}
//
//		//Write to the player data.
//		if(index == 1)
//		{
//			GameObjectTracker.GetGOT()._PlayerData.CannonSlotB = type;
//			
//			//Set the command center.
//			CommandCenter_01.SetCannonSlot(GameObjectTracker.GetGOT()._PlayerData.CannonSlotB,index);
//			
//		}
	
		
	}
	
	public Cannon CannonSlotA
	{
		get { return CommandCenter_01.CannonSlots[0].Item;}
	}
	
	public Cannon CannonSlotB
	{
		get { return CommandCenter_01.CannonSlots[1].Item;}
	}
	
	public CannonSpawner[] AllCannonSlots
	{
		get{return CommandCenter_01.CannonSlots;}
	}
	
	#endregion
	
	
	void OnApplicationPause()
	{
		//Pause the game.
	}
	
	public Vector3 ForceVector
	{
		get
		{
			return forceVector;
		}
	}
	
	
	#region Events
	
	void CannonPickedUp()
	{
		print("Event Registered: Cannon Picked Up");
	}
	
	#endregion
	
}
