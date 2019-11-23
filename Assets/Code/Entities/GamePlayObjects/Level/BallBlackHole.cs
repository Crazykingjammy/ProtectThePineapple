using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallBlackHole : BlackHole {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		//print("READ");
		
		List<Ball> bList  = GameObjectTracker.instance.World.ObjectSceneView.BallView;
		
		//Go through the scene and pull the balls in slowly.
		foreach(Ball b in bList)
		{
			if(!b.IsActive())
			{
				PullToCenter(b.gameObject);
			}
		}
		
	}
	
	
//	void OnTriggerStay(Collider col)
//	{
//				
//		//We check if object is collidign with balls then apply to the pull of the black hole.
//		if(col.gameObject.CompareTag("Ball") )
//		{
//			//here we check if the ball is active. If it is, we will let it be!
//			if(col.gameObject.GetComponent<Ball>().IsActive())
//			{
//				return;
//			}
//			
//			//Call attach for the cannon that we collide with.
//			PullToCenter(col.gameObject);
//			
//		}
//		
//		
//	}
	
	
}