using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FUIGridOfPhaseResultsBadges : MonoBehaviour {
	
	// and we're going to cache the pointer to the GOT phaseData List
	List<BasePhase.PhaseData> phaseDataList;
	
	int phaseCount = 0;
		// we're going to cache the phase results Objects
	List<FUIPhaseBadgeObject> phaseBadges;
	
	public FUIPhaseBadgeObject refPhaseBadgeObject;
	
	public GameObject Notification;
	public TweenTransform NotiAnimation, CounterAnimation;
	public TweenScale IconIntro, BGIntro;
	public TweenAlpha BGIntroA;
	
	public UISprite NotificationSprite;
	public UILabel CounterText;
	public ParticleSystem NotiPop;


	public UIPanel CCHealthPanel;
	public TweenScale HealthAnimation;
	public float CCmax, CCMin;
	float previousCCHealth = 100.0f;

	public UISprite CurrentPhaseIcon;
	public TweenScale CounterIconAnimation;

	public UISprite TopBarBG;

	public TweenScale WarningTopBar;


	public float WarningPercent = 0.2f;

	UIGrid myGrid = null;
	
	// Use this for initialization
	void Start () {
		// this object needs a grid
		myGrid = GetComponent<UIGrid>();
		if (myGrid == null)
			Debug.LogError("No Grid on the grid for phase results badges");
		
		phaseBadges = new List<FUIPhaseBadgeObject>();
		
		//Deactivate the notification
		Notification.SetActive(false);


		
	}
	
	// Update is called once per frame
	void Update () {
		phaseDataList = GameObjectTracker.GetGOT()._PlayerData.Breathless.PhaseList;
		
		if(Input.GetKeyDown(KeyCode.E))
			ActivateNotification();
		
		if(phaseCount > 0 && phaseDataList.Count < 1 )
		{
			
			Debug.LogError("CLEAR!");
			
			phaseCount = 0;
			foreach(FUIPhaseBadgeObject o in phaseBadges)
				DestroyImmediate(o.gameObject);
			
			phaseBadges.Clear();
			
		}
		
		if (phaseDataList != null && phaseCount < phaseDataList.Count){
			BasePhase.PhaseData lastPhaseData = phaseDataList[phaseCount];
			
			FUIPhaseBadgeObject newBadge = NGUITools.AddChild(myGrid.gameObject, refPhaseBadgeObject.gameObject).GetComponent<FUIPhaseBadgeObject>();
			newBadge.givenSpriteName = lastPhaseData.IconTextureName;
			
			
			phaseBadges.Add(newBadge);

			//Set the current badge
			CurrentPhaseIcon.spriteName = phaseDataList[phaseCount].IconTextureName;

			//Play the incomming animation.
			CounterIconAnimation.Reset();
			CounterIconAnimation.Play(true);
			
			phaseCount++;
			
			myGrid.Reposition();

			
			
		}
		
		for (int i = 0; i <  phaseCount; i++){
			
			
			if (phaseBadges[i].mySprite){
				phaseBadges[i].mySprite.spriteName = phaseBadges[i].givenSpriteName;
				Color spriteColor = phaseBadges[i].mySprite.color;
			
					if (phaseDataList[i].PhaseCompletionPunch > 0){		
						spriteColor.a = 1f;
					
					if(!phaseBadges[i].Used)
					{
						phaseBadges[i].Used = true;
						NotificationSprite.spriteName =  phaseBadges[i].mySprite.spriteName;
						CounterText.text = " " + phaseDataList[i].ComboToComplete + " ";
						ActivateNotification();
					}
					
					
					if (phaseBadges[i].mySprite){
						phaseBadges[i].mySprite.color = spriteColor;
						
						}						
					}
				else{
						spriteColor.a = 0.0f;
					if (phaseBadges[i].mySprite){
						phaseBadges[i].mySprite.color = spriteColor;
					}
				}

						
			}
		}		


		//Do the CC health here..
		if(ToyBox.GetPandora().CommandCenter_01)
		{
			float ccHealth = ToyBox.GetPandora().CommandCenter_01.GetHealthPercentage();

			float range = CCmax - CCMin;
			float scale = range * ccHealth;
			float size = CCMin + scale;

			//Scale the position of the panel clip based on % of CC Health.
			CCHealthPanel.clipRange = new Vector4(size,15.0f,300.0f,50.0f);

			//Check for greater than so only play the animation if we are loosing health over time.
			if(previousCCHealth > ccHealth)
			{
				HealthAnimation.Reset();
				HealthAnimation.Play(true);
				previousCCHealth = ccHealth;

			}


			//handle warning.
			if(ccHealth > WarningPercent)
			{
				//Set the bg color to normal.
				TopBarBG.gameObject.SetActive(false);
				return;
			}

			if(ccHealth < WarningPercent && !TopBarBG.gameObject.activeSelf)
			{
				//activate teh animation.
				TopBarBG.gameObject.SetActive(true);
				WarningTopBar.Reset();
				WarningTopBar.Play(true);

				AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CCWarning);
				return;

			}


		}
		
	}
	
	
	void ActivateNotification()
	{
		Notification.SetActive(true);
		
		NotiAnimation.Reset();
		
		IconIntro.Reset();
		IconIntro.Play(true);
		
		BGIntroA.Reset();
		BGIntroA.Play(true);
		
		BGIntro.Reset();
		BGIntro.Play(true);
		
		CounterAnimation.Reset();
		CounterAnimation.Play(true);
		
		
		NotiPop.Play();
		
		
		//Play sound effect.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.LevelUp);
		
		ToyBox.GetPandora().SetSlowMotion(true);
			
	}
	
	void NotificationIntroFinish()
	{
		//Revert back to normal time.
		
		NotiAnimation.Play(true);
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.PhasePresent);
		
		ToyBox.GetPandora().SetSlowMotion(false);
	}
	
	
	void NotificationFinish()
	{
		
		Notification.SetActive(false);
		NotiPop.Stop();
		
	}
}
