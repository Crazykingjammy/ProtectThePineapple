using UnityEngine;
using System.Collections;

public class ActivityObjective : FUIActivity {
	
	//Viewers to control.
	public FUIObjectiveViewer viewer;
	
	//Objects to display.
	public UILabel Name;
	
	public UISprite Icon;
	
	public GameObject Decor;
	
	public Color UnFinishedObjective;
	
	

	// Use this for initialization
	new void Start () {
	
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnActivate ()
	{
		//Grab the objective.
		BaseMission.GameObjective gobj = ActivityManager.Instance.SelectedObjective;
		
		//Set the name.
		Name.text = gobj.ObjectiveLabel;
		
		Icon.spriteName = gobj.IconName;
		
		
		UISprite[] decorSprites = Decor.GetComponentsInChildren<UISprite>(true);
		
		//Set the decor icons.
		foreach(UISprite sprite in decorSprites)
		{
			sprite.spriteName = gobj.IconName;
		}
		
		
		//See if the objective icon shoudl be finished or not.
		if(gobj.isCompleted)
		{
			Icon.color = Color.white;
			Decor.SetActive(true) ;
		}
		else{
			Icon.color = UnFinishedObjective;
			Decor.SetActive(false);
		}
			
		
		//Populate the list.
		viewer.AssignObjective();
		
	}
	
	
	
	void OnObjectiveClose()
	{
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PopActivity();
	}
}
