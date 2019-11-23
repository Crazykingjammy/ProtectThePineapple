using UnityEngine;
using System.Collections;

public class FUIGemsCounter : MonoBehaviour {

	public UILabel myLabel;
	
	public string myLabelText;
	public int gems;
	
	void Awake(){
		myLabel = gameObject.GetComponent<UILabel>();
		// this will set our member var to the gameobjects uilabel script
		// now we can edit it.
		if (myLabel == null){
			Debug.LogError("Unable to find label for Point Counter");
		}	
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gems = GameObjectTracker.GetGOT().GetGemsCollected();
		
		myLabelText = "" + gems + "";
		myLabel.text = myLabelText;
			
	}
}
