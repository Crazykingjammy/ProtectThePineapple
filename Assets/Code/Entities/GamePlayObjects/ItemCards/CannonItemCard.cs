using UnityEngine;
using System.Collections;

public class CannonItemCard : BaseItemCard {
	
	
	//Variable for the cannon type this card is for.
	public EntityFactory.CannonTypes type;
	

	// Use this for initialization
	 protected override void Start () {
	
		//Init();
		//print(name + " Awake");
		
		base.Start();
		
		//Set the default icon string
		//DisplayInfo.IconName = "CannonIcon" + this.Label;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	#region Cannon Data Acessories
	
	public override bool isCannonItem(EntityFactory.CannonTypes t)
	{
		if(type == t)
		return true;
		
		return false;
	}
	
	public override EntityFactory.CannonTypes ContainedCannonType 
	{
		get{return type;}
	}

	
	#endregion
}
