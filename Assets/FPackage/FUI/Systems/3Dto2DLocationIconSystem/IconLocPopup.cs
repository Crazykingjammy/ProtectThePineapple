using UnityEngine;
using System.Collections;

public class IconLocPopup : IconLocObject {
	
	new void Start(){
		// this is a simple Icon, default basically.  so everything is taken care of from the 
		// base
		base.Start();
		isFullyInit = true;
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		base.LateUpdate();


	}
}
