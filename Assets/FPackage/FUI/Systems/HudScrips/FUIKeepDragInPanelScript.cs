using UnityEngine;
using System.Collections;

public class FUIKeepDragInPanelScript : MonoBehaviour {
	
	// when we drag this object, we want to make sure we dont leave our panel
	public UIPanel myPanel;
	
	void Awake(){
		//myPanel = gameObject.GetComponent<UIPanel>();
		if (myPanel == null){
			Debug.Log("Failed to find Panel in FUIKeepDragInPanelScript");
		}
	}
	
	void OnDrag(){
		// only need to test if we're outside of the panel when we're dragging
		Vector3 myPos = Vector3.zero;
		if (myPos.x > myPanel.clipRange.x){
			myPos.x = myPanel.clipRange.x - 20;
		}
		if (myPos.x < myPanel.clipRange.x){
			myPos.x = myPanel.clipRange.x + 20;
		}
		if (myPos.y > myPanel.clipRange.y){
			myPos.y = myPanel.clipRange.y - 20;
		}
		if (myPos.y < myPanel.clipRange.y){
			myPos.y = myPanel.clipRange.y + 20;
		}
		
		
	}
}
