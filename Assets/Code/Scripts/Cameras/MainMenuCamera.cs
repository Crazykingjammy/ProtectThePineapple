using UnityEngine;
using System.Collections;

public class MainMenuCamera : MonoBehaviour {

	//Object to focus on.
	public GameObject FocusObject;
	
	// Use this for initialization
	void Start () {
	
		Time.timeScale = 1.0f;
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(FocusObject)
		{
			transform.LookAt(FocusObject.transform.position);
		}
		
	}
}
