using UnityEngine;
using System.Collections;

public class TargetsHealthBar : MonoBehaviour {
	
	// whos health am I tracking
	Target myTarget = null;
	
	public HealthBarObject refMyHealthBar;
	private HealthBarObject myHealthBar;
	

	
	float curHealth;
	float maxHealth;
	
	// Use this for initialization
	void Start () {
		// make sure we have a target attached 
		if (myTarget == null){
			myTarget = gameObject.GetComponent<Target>();
			if (myTarget == null){
				Debug.LogError("TargetsHealthBar - No Target Attached");
			}
		}
		
		myHealthBar = Instantiate(refMyHealthBar, myTarget.transform.position, myTarget.transform.rotation) as HealthBarObject;
		//myHealthBar.transform.parent = myTarget.transform;
//		myHealthBar.transform.localPosition = Vector3.zero;
//		myHealthBar.transform.localRotation = Quaternion.identity;
//		myHealthBar.transform.localScale = Vector3.one;
		
		// Tell the HealthbarObject who the target is.
		myHealthBar.MyTarget = myTarget.gameObject;
	
		// set the scale of the window.  If a target is scaled larger,
		// keep the health bar the same size
//		Vector3 windowScale = myHealthBar.windowHealthBar.localScale;
//		windowScale.x = myHealthBar.windowHealthBar.localScale.x / myTarget.transform.localScale.x;
//		windowScale.y = myHealthBar.windowHealthBar.localScale.y / myTarget.transform.localScale.y;
//		windowScale.z = myHealthBar.windowHealthBar.localScale.z / myTarget.transform.localScale.z;
//		
//		myHealthBar.windowHealthBar.localScale = windowScale;
		
//		Vector3 currHealthScale = myHealthBar.Slider.foreground.localScale;
//		maxHealth = myHealthBar.Slider.fullSize.x;
//		
//		currHealthScale.x += curHealth;
//		
//		myHealthBar.Slider.foreground.localScale = currHealthScale;
	}
	
}
