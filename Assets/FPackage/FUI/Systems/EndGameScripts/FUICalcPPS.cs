using UnityEngine;
using System.Collections;

public class FUICalcPPS : MonoBehaviour {
	
	public UILabel myLabel;
	
	public string myLabelText;
	
	void Awake(){
		myLabel = gameObject.GetComponent<UILabel>();
		if (myLabel == null){
			Debug.LogError("Unable to find label for PPS");
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float localTimer = GameObjectTracker.GetGOT().localTargetTimer;
		int points = GameObjectTracker.GetGOT().GetCurrentPoints();
		float pps = points/localTimer;
		
		myLabel.text = ""+pps;
	}
}
