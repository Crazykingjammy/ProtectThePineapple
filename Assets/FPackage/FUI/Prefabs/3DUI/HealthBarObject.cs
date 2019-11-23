using UnityEngine;
using System.Collections;

public class HealthBarObject : FUISliderBar {
	
	float curHealth = 0;

	float lastFrameHealth = 0;
	
	// Use this for initialization
	new protected void Start () {
		base.Start();
		
		// not sure where this came from...  but commented it out, after updating to ngui 2.3.6a
		// maxHealth = Slider.fullSize.x;
		
		// test....

	
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (MyTarget == null)
		{
			// if I dont have a target, remove me from the scene
			GameObject.Destroy(this.gameObject);
			return;
		}
		
		
		// make sure to follow the target
		//transform.localPosition = MyTarget.transform.position;   // this is what worked... but the main object needs to be 
		// its own root.  not good.
		// object needs to be under the 3D UI Root and under a panel
		//transform.localPosition = Vector3.zero;
		//transform.position = MyTarget.transform.position;
		
		
		//transform.position = MyTarget.transform.position;
		Target myTempTarget = MyTarget.GetComponent<Target>();
		if (myTempTarget == null)
		{	
			Debug.LogError("HealthbarObject: Target host, is not a Target object");
			return;
		}
		
		if (myTempTarget.IsDestroyed()){
			SetSliderAlpha(0);
			
		}
		
		//Reset the location if the target has a UIHandler.
		transform.position = myTempTarget.GetUIHandlePosition();
		transform.rotation = myTempTarget.transform.rotation;
		curHealth = myTempTarget.GetHealthPercentage();
		//curHealth = myTempTarget.Health;
		
//		if (isAlwaysVisible){
//			SetSliderAlpha(1.0f);
//		}
//		else 
		if (lastFrameHealth == curHealth || curHealth <= 0 || myTempTarget.IsDestroyed() )
		{
			SetSliderAlpha(0.0f);
		}
		else {
			SetSliderAlpha(1.0f);
		}
		
		lastFrameHealth = curHealth;
		
		
		if (curHealth <= 0 ) {curHealth = 0.0009f; } 
		
		Slider.foreground.localScale = new Vector3(curHealth * this.maxSliderWidth, Slider.foreground.localScale.y, Slider.foreground.localScale.z);

	}
	

}
