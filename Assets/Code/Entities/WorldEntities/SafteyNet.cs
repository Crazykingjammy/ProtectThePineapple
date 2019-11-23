using UnityEngine;
using System.Collections;

public class SafteyNet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		
		
	}
	
	void OnCollisionEnter(Collision col)
	{
		print("ARE WE BEING CALLED?!?!?");
		
		if(col.gameObject.CompareTag("Ball") )
		{
			//We will just print a warning message for not
			print ("THE PHYSICS HAVE FAILED YOU AGAIN! BALL HAS FELL BELOW THE LEVEL!!!");
		}
		
		if(col.gameObject.CompareTag("Cannon") )
		{
			col.gameObject.transform.position = Vector3.zero;
			
			//We will just print a warning message for not
			print ("CANNON HAS FELL BELOW THE LEVEL!!!");
		}
		if(col.gameObject.CompareTag("Target") )
		{
			col.gameObject.transform.position = Vector3.zero;
			
			//We will just print a warning message for not
			print ("Target HAS FELL BELOW THE LEVEL!!!");
		}
	}
}
