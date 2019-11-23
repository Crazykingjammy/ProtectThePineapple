using UnityEngine;
using System.Collections;

public class TutorialGame : BaseGame {
	
	//The masters.
	public GameObjectTracker GOT;
	public ToyBox Pandora;
	
	public StandardWave[] Waves;
	
	public Camera IntroCam;
	public AudioClip Music;
	
	public Cannon Turtle;

	public float MovementWaitTimer = 5.0f;

	public AudioClip BossMusic;

	bool started = false;
	bool curtainOpen = false;
	bool playedMusic = false;
	public bool PlayIntro = false;


	bool ShieldIN, ShootIN, OverheatIn = false;
	ControlsLayoutObject loadedControls = null;
	
	enum State
	{
		FirstWave,
		SecondWave,
		ThirdWave,
		ComboWave,
		BossWave,
		Death,
		Count,
		Intro,
		End,
		Null
	}
	
	State currentState = State.Null;
	
	void Awake()
	{
		//Check if GOT is null then create one.
		if(!GameObjectTracker.instance)
			Instantiate(GOT);
			
		if(!ToyBox.GetPandora())
		{
			Instantiate(Pandora);
		}

		
	}

	void GrabControls()
	{

		if(!ActivityManager.Instance)
			return;

		//Store the controls manager
		ControlsManager  mgr = ActivityManager.Instance.HUDController;
		
		if(mgr)
		{
			loadedControls = ActivityManager.Instance.HUDController.LoadedControls;

		}

		if(loadedControls)
		{
			//When we grab... hide all the controls...
			loadedControls.Shoot.gameObject.SetActive(false);
			loadedControls.Shield.gameObject.SetActive(false);
			loadedControls.Overheat.gameObject.SetActive(false);
		}


		//Take the hud off when we grab.
		ActivityManager.Instance.ToggleHud(false);
	}

	// Use this for initialization
	void Start () {
	
		
		
		
		//Start time.
		if(!ToyBox.GetPandora())
		{
			Debug.LogError("NO F TOYBOX!");
			return;
			
		}
		
		ToyBox.GetPandora().TimePaused = false;
		started = true;

		
		currentState = State.Null; 
		
		if(PlayIntro)
		{
			Camera.SetupCurrent(IntroCam);
			GetComponent<Animation>().Play();	
			currentState = State.Intro; 

			//Disable the hud if there is one
//			if(ActivityManager.Instance)
//				ActivityManager.Instance.ToggleHud();
		}


		//This is tutorial mode.. turn combos off.
		if(GameObjectTracker.instance)
			GameObjectTracker.instance.TrackCombo = false;

		//Store the controls manager
	if(loadedControls == null)
			GrabControls();

	}
	
	// Update is called once per frame
	void Update () {
	
		if(loadedControls == null)
			GrabControls();
		
		if(!started)
		{
			if(ToyBox.GetPandora())
			{
				ToyBox.GetPandora().TimePaused = false;
				started = true;

			
			}
				
		}
		
		if(!curtainOpen)
		{
			if(ActivityManager.Instance)
			{
				ActivityManager.Instance.DrawCurtain();
				curtainOpen = true;

			}
		}
		

		if(!playedMusic)
			if(AudioPlayer.GetPlayer())
		{
			AudioPlayer.GetPlayer().PlayAudioClip(Music);
			playedMusic = true;
			Debug.LogError("MUSIC PLAYED!");
			
		}
		else
		{
			Debug.LogError("NO PLAYER!");
		}

		
		switch(currentState)
		{
			
		case State.Intro:
		{
				
//			Camera.SetupCurrent( GameObjectTracker.instance.World.CurrentCamera );
//			currentState = State.Null; 

//			if(!playedMusic)
//				if(AudioPlayer.GetPlayer())
//			{
//				AudioPlayer.GetPlayer().PlayAudioClip(Music);
//				playedMusic = true;
//				Debug.LogError("MUSIC PLAYED!");
//
//			}
//			else
//			{
//				Debug.LogError("NO PLAYER!");
//			}
//			
			break;
		}


		case State.Null:
			{

			//Subtract wiat timer.
			MovementWaitTimer -= Time.deltaTime;

			if(ShieldIN == true)
				break;

			if(MovementWaitTimer <= 0.0f)
			{
				//If the timer is up actiate the shield and trigger the animation.
				loadedControls.Shield.gameObject.SetActive(true);
				loadedControls.AnimateShield();

				ShieldIN = true;

				//Debug.LogError("Shield Bring IN");
			}

				break;
			}
			
		case State.FirstWave:
			{
			
			if(Waves[0].IsCompleted())
			{
				
				ToyBox.GetPandora().SceneBallStack.NumberBalls = 10;
				currentState = State.SecondWave;
				
				Destroy(Waves[0].gameObject);
				
				//Instantuate wave.
				Waves[1] = Instantiate(Waves[1]) as StandardWave;

				//Bring in shoot button.
				//If the timer is up actiate the shield and trigger the animation.
				loadedControls.Shoot.gameObject.SetActive(true);
				loadedControls.AnimateShoot();


			}
			
				break;	
			}
			
		
		case State.SecondWave:
		{
			if(Waves[1].IsCompleted())
			{
				
				currentState = State.ThirdWave;
				Destroy(Waves[1].gameObject);
				
				ToyBox tb = ToyBox.GetPandora();

				tb.SceneBallStack.NumberBalls = 30;
				
				//Spawner the zebra.
				tb.AssignSlot(EntityFactory.CannonTypes.Zebra,0);
				tb.CommandCenter_01.CannonSlots[0].TriggerSpawn();
				
				//Instantuate wave.
				Waves[2] = Instantiate(Waves[2]) as StandardWave;
			}
			
				break;	
		}

		case State.ThirdWave:
			{

			if(Waves[2].IsCompleted())
			{
				currentState = State.ComboWave;
				Destroy(Waves[2].gameObject);

				//Turn on the combos now.
				//This is tutorial mode.. turn combos off.
				if(GameObjectTracker.instance)
					GameObjectTracker.instance.TrackCombo = true;

				//Instantuate wave.
				Waves[3] = Instantiate(Waves[3]) as StandardWave;

			}

				//If combo > 10 do boss. 
				break;
			}

		case State.ComboWave:
		{

			break;
		}

		case State.Death:
		{
			//In dealth state we wait till our timer is out then we switch scenes.
			MovementWaitTimer -= Time.deltaTime;

			if(MovementWaitTimer <= 0.0f)
			{
				//Kill the wave.
				Destroy(Waves[4].gameObject);


				//Set the tutorial mode to false.
				GameObjectTracker.instance._PlayerData.IsTutorial = false;

				//Pull curtain
				ActivityManager.Instance.PullCurtain();

				currentState = State.End;

				ActivityManager.Instance.SetActive = false;

				GameObjectTracker.instance.RestartButtonPushed(true);

			}
			
			break;
		}


		case State.BossWave:
		{
			if(ToyBox.GetPandora().CommandCenter_01.IsDestroyed())
			{
				currentState = State.Death;
				ToyBox.GetPandora().SetSlowMotion(true);



				//Fade to black here?
				ActivityManager.Instance.ToggleHud();
				ActivityManager.Instance.FadeToBlack();

				MovementWaitTimer = 1.33f;
			}
			
			break;
		}


		default:
			{
				break;
			}
			
		}
		
	}
	
	
	public void EndIntro()
	{
		Debug.LogError("END INTRO");
		
		IntroCam.gameObject.SetActive(false);
		
		GameObjectTracker.instance.World.SetGameCamera();

		//Put back the HUD.
		if(ActivityManager.Instance)
			ActivityManager.Instance.ToggleHud(true);
		
		currentState = State.Null; 
	}
	

	public override void BotOverheat ()
	{
		//base.BotOverheat ();

		if(!OverheatIn)
		{
			//If the timer is up actiate the shield and trigger the animation.
			loadedControls.Overheat.gameObject.SetActive(true);
			loadedControls.AnimateOverheat();
			
			OverheatIn = true;

		}
	}

public override void CannonPickedUp ()
	{
		if(currentState != State.Null)
			return;
		
		currentState = State.FirstWave;
		
		
		//Instantuate wave.
		Waves[0] = Instantiate(Waves[0]) as StandardWave;
	}
	
	public override void TargetDestroyed (Target t)
	{
		//Chcek here for boss killing
		if(currentState == State.ComboWave)
		{
			//go tto boss if combo bigger than 10.
			if(GameObjectTracker.instance.GetMultiplier() > 9)
			{
				currentState = State.BossWave;
				Destroy(Waves[3].gameObject);

				AudioPlayer.GetPlayer().PlayAudioClip(BossMusic);
				
				GameObjectTracker.instance.vWorld.GotoHeavyRoom();

				ToyBox.GetPandora().CommandCenter_01.Health = 1000.0f;
				ToyBox.GetPandora().CommandCenter_01.MaxHealth = 1000.0f;

				//Play extended slow motion?
				//ToyBox.GetPandora().SetSlowMotion(true,2.0f);

				//Instantuate wave.
				Waves[4] = Instantiate(Waves[4]) as StandardWave;

			}


		}
		
		if(currentState != State.Null)
			return;
		
		ToyBox tb =  ToyBox.GetPandora();
		
		//tb.CommandCenter_01 = Instantiate(tb.CommandCenter_01,t.transform.position,Quaternion.identity) as CommandCenter;
		
		tb.CommandCenter_01.transform.position = t.transform.position;
		tb.CommandCenter_01.gameObject.SetActive(true);
		
		tb.CommandCenter_01.Damage(-7.0f);

		//if(Music)
		//	AudioPlayer.GetPlayer().PlayAudioClip(Music);
		
		Cannon c1 = Instantiate(Turtle) as Cannon;

//		Vector3 pos = c1.transform.position;
//		pos.x += 20.0f;
//
//		Cannon c2 = Instantiate(Turtle,pos,Quaternion.identity) as Cannon;

		//Turn on hud here.
		ActivityManager.Instance.ToggleHud(true);

		//Pause the game.

		//tb.AssignSlot(EntityFactory.CannonTypes.Turtle,0);
		//tb.CommandCenter_01.CannonSlots[0].TriggerSpawn();
		
		//GOT.World.ObjectSceneView.TargetView.Add(tb.CommandCenter_01);
		
		//Course.gameObject.SetActive(false);
		
		
	}
}

