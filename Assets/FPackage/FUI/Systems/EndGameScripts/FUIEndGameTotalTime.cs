using UnityEngine;
using System.Collections;

public class FUIEndGameTotalTime : MonoBehaviour {


	public UILabel myLabel;
	
	public string myLabelText;
	public float time;
	
	// Use this for initialization
	void Awake() {
		myLabel = gameObject.GetComponent<UILabel>();	
		// this will set our member var to the gameobjects uilabel script
		// now we can edit it.
		if (myLabel == null){
			Debug.LogError("Unable to find label for Point Counter");
		}
	}
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
		//myLabelText = "" + points + "";
		myLabelText = FormatSeconds(GameObjectTracker.GetGOT().FullStatistics.TimeAmount);
		myLabel.text = myLabelText;
			
		}
	
	string FormatSeconds(float elapsed)
	{
	   int d = (int)(elapsed * 100.0f);
	   int minutes = d / (60 * 100);
	   int seconds = (d % (60 * 100)) / 100;
	   int hundredths = d % 100;
	   return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, hundredths);
	}
}
