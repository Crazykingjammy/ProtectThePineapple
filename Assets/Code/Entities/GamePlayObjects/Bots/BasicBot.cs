using UnityEngine;
using System.Collections;

public class BasicBot : Bot {

	// Use this for initialization
	void Start () {
	
		//Should overwrite the start function but whatever.
		Initialize();
		
		if(DefenseUnitSlot)
		{
			//Deactivate at the start.
			DefenseUnitSlot.DeActivate();
		}
		
		
	}

	new protected void Update(){
		base.Update();
	}

	// Update is called once per frame
	void FixedUpdate () {
	
		//Update Base bot to handle basic functions of a bot.
		BaseBotUpdate();
	
	}
	

}
