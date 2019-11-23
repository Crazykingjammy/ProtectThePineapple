using UnityEngine;
using System.Collections;

public class BaseEnemyPickUpBehavior : BaseEnemyBehaviorScript {

	// Use this for initialization
	new void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();
		//		// if this script object is in the scene, make sure it knows who its controlling
//		if (self){
//			
//		}
//		else{
//			// we don't control anyone, we shouldn't be here!
//			print("I'm a " + this.name +", no self, I had to kill myself");
//			//Destroy(gameObject);
//		}
	}
	
	//Check for all possible collisions and apply any effects.
	// mostly taken from BasicEnemy
	new protected void OnCollisionEnter(Collision col)
	{
		base.OnCollisionEnter(col);
		// make sure we're still attached to a valid target object
		if (self == null || !isSelfAsEnemy)
			return;

		///If we hit a ball do some damage...
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();

			/** 
			 * To make a ball eater work, it'll have to have a fake cannon attached.  the 
			 * way ball picking up is handled with the cannon
			 */
			//Here we do a test to ONLY pick up active netural balls and nothing else.
			if(b.GetBallSourceID() == Ball.BallSourceID.Netural)
			{
				//We have to make sure we have a cannon attached to pick up a ball and we make sure sheild is not on.
				if(selfAsEnemy.ConnectController.HasCannon())
				{
					
					//Call attach for the cannon that we collide with.
					if(selfAsEnemy.ConnectController.GetCannon().PickupBall(b) )
					{
						//Set the ID to the ball
						b.SetBallSourceID(Ball.BallSourceID.Enemy);
						
						//Play the effect for picking up a ball.
						if(selfAsEnemy.Collection)
						{
							//Play the collection animation.
							selfAsEnemy.Collection.Play();
						}
						
					}
					
				}
				
			}
			
		}//Ball Collision Check
		
		//Check if we collide with a cannon.
		if(col.gameObject.CompareTag("Cannon") && !self.IsDestroyed())
		{
			Cannon c = col.gameObject.GetComponent<Cannon>();

			//Call attach for the cannon that we collide with.
			if(selfAsEnemy.ConnectController.AttachCannon(c))
			{
				selfAsEnemy.cooldownTimer = selfAsEnemy.CoolDownTime - 0.5f; 
				
				selfAsEnemy.CurrentState = BaseEnemy.ActionState.CoolDown;
				//c.SetDirty(true);
			}
			
			
		}
		
		
	}//All Collisions ON Enter Check
	
	//Check for ball collisions and apply damage.
	new protected void OnCollisionStay(Collision col)
	{	
		base.OnCollisionStay(col);
	}//All Collisions ON Stay Check

}
