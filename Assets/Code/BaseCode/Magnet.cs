using UnityEngine;
using System.Collections;

public class Magnet : BlackHole {
	
	public Bot host;
	
	
	// Use this for initialization
	void Start () {
	
		//Get an instance of the cannon from the parent.
		// removed because made the BOT public to set in the editor.. TODO: make better!
		//host = transform.parent.GetComponent<Bot>();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider col)
	{
		
		GameObject go = col.gameObject;

		if (host == null){
			// Moved CannonConnector of the bot as a child of the arm.  
			// It then started to crash here.  Some how this magnet object is
			// losing its connection with its host.
			Debug.LogError("No Host In Magnet.cs");
			return;
		}

		if(!host.IsCannonAttached())
		{
			//We check if object is collidign with balls then apply to the pull of the black hole.
			if(go.CompareTag("Cannon") )
			{
				//print("Magnet Cannon");
				
				//Dont pull burning cannons.
				Cannon c = go.GetComponent<Cannon>();
				if(c.IsBurning)
					return;
				
				//Call attach for the cannon that we collide with.
				PullToCenter(go,col.GetComponent<Rigidbody>().mass );	
				return;
			}
				
		}
			
		
		
		//Dont do any of the following collisions if we dont have cannon.
		if(host.IsCannonAttached())
		{
			
			if(go.CompareTag("Money"))
			{
				PullToCenter(go);
				return;
			}
			
			//We check if object is collidign with balls then apply to the pull of the black hole.
			if(go.CompareTag("Ball") )
			{
				//here we check if the ball is active. If it is, we will let it be!
				if(go.GetComponent<Ball>().IsActive())
				{
					return;
				}	
				
				if(host.IsCannonFull())
				{
					return;
				}
			
				//Call attach for the cannon that we collide with.
				PullToCenter(go);
				return;
				
			}		
			
		}
		
	

		//Pull in the power ups
		if(go.CompareTag("PowerUp"))
		{
			PullToCenter(go);
		}
	
		
	}
		
}
