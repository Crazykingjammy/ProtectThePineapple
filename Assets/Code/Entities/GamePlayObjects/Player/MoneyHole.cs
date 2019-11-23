using UnityEngine;
using System.Collections;

public class MoneyHole : BlackHole {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider col)
	{
		//Only pull in da money.
		if(col.gameObject.CompareTag("Money") )
		{
			PullToCenter(col.gameObject);
			
			
			
		}
		
		
	}
}
