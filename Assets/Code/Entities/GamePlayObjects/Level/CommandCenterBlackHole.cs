using UnityEngine;
using System.Collections;

public class CommandCenterBlackHole : BlackHole {
	
	CommandCenter _local = null;
	
	// Use this for initialization
	void Start () {
	
		
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(_local == null)
		{
			_local = ToyBox.GetPandora().CommandCenter_01;
		}
		
		
		PullToCenter(_local.gameObject,_local.GetComponent<Rigidbody>().mass);
		
	}
	
	
//	void OnTriggerStay(Collider col)
//	{
//				
//		//We check if object is collidign with balls then apply to the pull of the black hole.
//		if(col.gameObject.CompareTag("CommandCenter") )
//		{
//			Target t = col.gameObject.GetComponent<Target>();
//			
//			//Call attach for the cannon that we collide with.
//			PullToCenter(col.gameObject,t.rigidbody.mass);
//			
//		}
//		
//		
//	}
//	
}
