using UnityEngine;
using System.Collections;

public class ActivityMission : FUIActivity {
	
	public UISprite CardIcon, CardIcon1, CardIcon2;
	public UILabel CardLabel;
	public UISprite CardBG;
	
	//public FUIMissionViewer viewer;
	
	public FUIMissionListViewer viewer;
	public FUIMissionViewer mviewer;
	
	public bool UseView = true;
	
	// Use this for initialization
	new void Start () {
	
		base.Start();
		
	}
	
	public override void OnActivate ()
	{
		BaseItemCard theCard = ActivityManager.Instance.SelectedCard;
		
		//Get the card icon
		string IconName = theCard.DisplayInfo.IconName;
		CardIcon.spriteName = IconName;
		CardIcon1.spriteName = IconName;
		CardIcon2.spriteName = IconName;
		
		//Get the name.
		string CardName = theCard.Label;
		CardLabel.text = CardName;
		
		//Set the color.
		CardBG.color = theCard.DisplayInfo.BGColor;
		
		//Set up the mission view list.
		
		if(UseView)
		{
			mviewer.AssignedMission = theCard.BreathlessMission;
			return;
		}
		
		//Set the Mission viewer info.
		viewer.ReAssignMissions();
		
	}
	
	
	
	
	void OnClose()
	{
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PopActivity();
		
	}
}
