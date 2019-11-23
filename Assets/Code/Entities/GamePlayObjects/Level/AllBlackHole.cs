using UnityEngine;
using System.Collections;

public class AllBlackHole : BallBlackHole {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnTriggerStay(Collider col)
	{
				
		//We check if object is collidign with balls then apply to the pull of the black hole.
		if(col.gameObject.GetComponent<Rigidbody>() )
		{
			//Call attach for the cannon that we collide with.
			PullToCenter(col.gameObject);
			
		}
		
		
	}
}
