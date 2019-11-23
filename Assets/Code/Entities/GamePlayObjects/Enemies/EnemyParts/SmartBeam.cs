using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmartBeam : MonoBehaviour {
	
	
	public List<Ball> BallView;
	public List<CommandCenter> TargetView;

	// ** Franks add /// <summary>
	/// 
	/// SmartBeam knows about cannons.  Enemies want cannons
	/// 
	/// Contain a list of cannons visible to this
	/// May Needs to clean up list when can't see it anymore or cannon is destroyed
	/// </summary>
	public List<Cannon> CannonView;

	/// End Franks add  ///

	public Bot BotView;
	
//	public int BallCount;
	
//	bool gathering = false;
//	int commandedgather = 0;
	
	// Use this for initialization
	void Start () {
	
		Reset();
	}


	public void Reset()
	{
		//Set all the view to null and clear all the lists.
		BallView.Clear();

		TargetView.Clear();

		CannonView.Clear();

		BotView = null;

		GetComponent<Collider>().isTrigger = true;
	}

//	// Update is called once per frame
//	void FixedUpdate () {
//	
//		HandleBallGathering();
//		
//	}
	
	
//	void HandleBallGathering()
//	{
//		//We cant and wont gather balls if we dont have a cannon!
//		
//		
//		//Handle the gathering of balls update.
//		if(gathering)
//		{
//			Active = true;
//			
//			foreach(Ball b in BallView)
//			{
//				if(!b.IsActive())
//				PullToCenter(b.gameObject);
//			}
//			
//			if(BallCount >= commandedgather)
//			{
//				gathering = false;
//			}
//		}
//		else{
//			Active = false;
//		}
//	}
//	
//	public void GatherBalls(int count)
//	{
//		//We set the commanded gather
//		commandedgather = count;
//		
//		//We turn on the gathering mode if this function is called and we have less balls than commanded to gather
//		if(BallCount < count)
//		{
//			gathering = true;
//		}
//		
//	}
//	
	void OnTriggerStay(Collider col)
	{
		//print("SmartBeam Collision!");
		
		//Check if we collide with balls.
		if(col.gameObject.CompareTag("Ball") )
		{
			Ball b = col.gameObject.GetComponent<Ball>();
			
			if(b.IsActive() || BallView.Contains(b) )
			{
				return;
			}
			
			BallView.Add(b);
			
		}
		
		if(col.gameObject.CompareTag("CommandCenter") )
		{
			CommandCenter c = col.gameObject.GetComponent<CommandCenter>();
			
			if(TargetView.Contains(c) )
			{
				return;
			}
			
			TargetView.Add(c);
			
		}

		///** Franks add
		/// 
		if(col.gameObject.CompareTag("Cannon"))
		{
			Cannon c = col.gameObject.GetComponent<Cannon>();
			if(c.InUse || CannonView.Contains(c))
			{
				return;
			}
			
			CannonView.Add(c);
			
		}
		/// 


		if(col.gameObject.CompareTag("Bot") )
		{
			if(!BotView)
				BotView = col.gameObject.GetComponent<Bot>();
			
			
		}
		
		
	}
	
	
//	public void SetBallCount(int c)
//	{
//		BallCount = c;
//	}
}
