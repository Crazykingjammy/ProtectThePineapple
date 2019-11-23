using UnityEngine;
using System.Collections;

public class SeekerScript : BaseEnemyBehaviorScript {

	// moved to BaseEnemyBehaviorScript.cs 12/29/13
	// made this a BaseEnemy Minimum 12/28/13 and made private
	// private BaseEnemy self;
	
	// How fast do I move?
	public float speed = 400;

	
	// Use this for initialization
	new void Start () {
		base.FixedUpdate();
		//Set teh host. or me.
		self = GetComponent<BaseEnemy>();
	}
	
	// Update is called once per frame
	new void FixedUpdate () {
		base.FixedUpdate();
		// if this script object is in the scene, make sure it knows who its controlling
		if (self){
			// get the forward of who im controlling
			Vector3 myDirection = self.transform.forward;
			// TODO: asset issue, we need to flip the forward direction 
			//myDirection = -myDirection * speed;
			myDirection *= speed;
			
			//This is where we ensure that the force on Y is 0 so we dont go up!
			//myDirection.y = 0.0f;
			
			self.GetComponent<Rigidbody>().AddForce(myDirection * GetComponent<Rigidbody>().mass);
		}
		else{
			// we don't control anyone, we shouldn't be here!
			print("I'm a Seeker, I had to kill myself");
			//Destroy(gameObject);
		}
	}
	
		//Check for ball collisions and apply damage.
	new protected void OnCollisionStay(Collision col)
	{	
		base.OnCollisionStay(col);

	}//All Collisions Check
}
