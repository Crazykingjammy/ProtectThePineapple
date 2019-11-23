using UnityEngine;
using System.Collections;

public class MainMenuStats : Label {
	
	public Camera MainCamera;
	
	public GameObject ColliderBlock;
	public GUITexture Board;
	
	// Use this for initialization
	protected override void  Start () {
	
	base.Start();
		
	}
	
	// Update is called once per frame
	protected override void Update () {
	
		base.Update();
		
		
		if(IsCaptured())
		{
			//Enable stats
			ColliderBlock.GetComponent<Collider>().isTrigger = false;
			ColliderBlock.GetComponent<Rigidbody>().isKinematic = true;
			//ColliderBlock.light.enabled = true;
			
			
			//Get the position
			Rect boardrect = Board.GetScreenRect(MainCamera);
			Vector3 screenpoint = new Vector3(boardrect.x + (boardrect.width/2.0f),boardrect.y + (boardrect.height/2.0f));
			screenpoint.z = 14.0f;
			
			//Get teh position
			Vector3 position = MainCamera.ScreenToWorldPoint(screenpoint);
			
			//Set the forward fo the finger.
			ColliderBlock.transform.forward = MainCamera.transform.forward;
			ColliderBlock.transform.position = position;
			//ColliderBlock.light.transform.position = position;
			
			
			
		}
		else
		{
			//Turn off the stuff
			//ColliderBlock.collider.isTrigger = true;
			//ColliderBlock.rigidbody.isKinematic = true;
			//ColliderBlock.light.enabled = false;
			
		}
	
		
		
	}
}
