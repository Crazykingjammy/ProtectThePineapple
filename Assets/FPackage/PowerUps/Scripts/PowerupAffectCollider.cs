using UnityEngine;
using System.Collections;

public class PowerupAffectCollider : MonoBehaviour {
	
	// the parent affect of this collider.
	// this is to tell me the child of the affect that there was a collision
	public PowerupAffectBase myAffect = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
	
	void OnTriggerStay(Collider col)
	{	
		// print (this.ToString() + " onTriggerStay " + col.ToString());
		if (myAffect){
			myAffect.ReceiveColliderInfo(col);
		}
		
	}//All Collisions Check
	
	void OnTriggerEnter(Collider col)
	{	
			// print (this.ToString() + "onTriggerEnter " + col.ToString());
			if (myAffect){
			myAffect.ReceiveColliderInfo(col);
		}
		
	}//All Collisions Check
	void OnTriggerExit(Collider col)
	{	
			// print (this.ToString() + "onTriggerExit " + col.ToString());
			if (myAffect){
			myAffect.ReceiveColliderInfo(col);
		}
		
	}//All Trigger Checks
	
	
	
	void OnCollisionStay(Collision col)
	{	
		//print (this.ToString() + "onCollisionStay " + col.ToString());
		if (myAffect){
			myAffect.ReceiveCollisionInfo(col);
		}
		
	}
	
	void OnCollisionEnter(Collision col)
	{	
		//print (this.ToString() + "onCollisionEnter " + col.ToString());
		if (myAffect){
		myAffect.ReceiveCollisionInfo(col);
		}
		
	}
	void OnCollisionExit(Collision col)
	{	
			//print (this.ToString() + "onCollisionExit " + col.ToString());
			if (myAffect){
			myAffect.ReceiveCollisionInfo(col);
		}
		
	}

}

