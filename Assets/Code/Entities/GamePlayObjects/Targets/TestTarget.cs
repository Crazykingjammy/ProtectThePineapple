using UnityEngine;
using System.Collections;

public class TestTarget : Target {
	
	
	
	// Update is called once per frame
	new void Update () {
		
		//call moma's update.
		base.Update();
		
	
	}
	
	void OnCollisionEnter(Collision col)
	{
		///If we hit a ball it should turn red now.
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();
			
		
			//Only look for active balls!
			if(b.IsActive() && b.GetBallSourceID() == Ball.BallSourceID.Bot)
			{
				Damage(b.DamageAmount);
				
				//And we pop the ball.
				b.Pop();
			}
			
		}
	}
}
