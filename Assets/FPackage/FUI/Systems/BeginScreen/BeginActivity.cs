using UnityEngine;
using System.Collections;

public class BeginActivity : FUIActivity {

	public TweenRotation[] Slots;

	public TweenScale StartTween;

	public TweenAlpha BGIntro;


	bool started = false;

	// Use this for initialization
	new void Start () {

		base.Start();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnActivate ()
	{
		base.OnActivate ();

		ActivityManager.Instance.ToggleHud(false);

		ActivityManager.Instance.PullCurtain();
		ToyBox.GetPandora().TimePaused = true;

		StartTween.Reset();

		//AudioPlayer.GetPlayer().pop
		ActivityManager.Instance.PopAudio();

		//Pull curtain on open?
		ActivityManager.Instance.DrawCurtain();

		//Set the bg intro animation
		BGIntro.Reset();
		BGIntro.Play(true);

		//Local hack to disable the intial OnClick call on activate.
		started = false;
	}


	void OnStartGame()
	{
		StartTween.Reset();
		StartTween.Play(true);

		//Apply the bank spending here...
		ToyBox.GetPandora().AssignCannonSlotTypes();


		//Play cash sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CashRegister);

		started = true;
	}


	void OnSlotsStart()
	{
		if(!started)
			return;


		ToyBox.GetPandora().TimePaused = false;
		ToyBox.GetPandora().SceneBallStack.NumberBalls = 30;

		
		//Display the Hud
		ActivityManager.Instance.TriggerTitleAnimation();
		

		ActivityManager.Instance.ToggleHud(true);

		//Pop this activity and spend slots.
		ActivityManager.Instance.PopActivity();


		GameObjectTracker.instance.vWorld.GotoNextRoom();
		//Resetback the animation?
	}


	void OnOptionsButton()
	{
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Options,true);
	}
	
	void OnInventoryButton()
	{
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Inventory,true);
	}
}
