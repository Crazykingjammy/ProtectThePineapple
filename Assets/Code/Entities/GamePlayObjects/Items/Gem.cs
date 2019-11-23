using UnityEngine;
using System.Collections;

public class Gem : Money {
	
	//Public value for the life spand of the entity.
	public float LifeSpand = 10.0f;	
	
	//Public value for the gem to be self destroying.
	public bool SelfKill = true;

	public GemPotManager Host;
	
	
	// Use this for initialization
	void Start () {
				age = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(SelfKill)
		{
			if(age > LifeSpand)
			{
				KillMe();
				
			}
		}
	
		//update the age.
		age += Time.deltaTime;
	}
	

	
	public void KillMe()
	{
		GameObjectTracker.GetGOT().MoneyLost(this);
		GemActive = false;
		
	}	

}
