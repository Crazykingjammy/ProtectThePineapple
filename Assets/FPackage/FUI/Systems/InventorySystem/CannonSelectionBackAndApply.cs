using UnityEngine;
using System.Collections;

/*
 * This object when pressed is to tell the activity 
 * that the cannon
 */

public class CannonSelectionBackAndApply : MonoBehaviour {
	
	public CannonSelectionObject myCannonSelectionObject = null;
	
	// Use this for initialization
	void Start () {
		// the open cannon script better be part of a slot selection object with in an activity
		myCannonSelectionObject = gameObject.transform.parent.GetComponent<CannonSelectionObject>();
	}
	
	/// <summary>
	/// tell my selection object that I was the button that was clicked
	///  to the slot.
	/// </summary>
	void OnClick(){
		if (myCannonSelectionObject != null){
			myCannonSelectionObject.ButtonCannonSelectedPressed();
		}
	}
}
