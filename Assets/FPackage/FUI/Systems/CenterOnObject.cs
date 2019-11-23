using UnityEngine;
using System.Collections;

public class CenterOnObject : MonoBehaviour {
	
	public GameObject dragPanel;
	public Transform t;
	
	void OnClick(){
		// can be used this way because the button is in the drag panel
		SpringPanel.Begin(dragPanel.gameObject, -transform.localPosition, 8f);
		
		// if object is not in its drag panel but somewhere else
		// use this code
		/*
		Vector3 newPos = dragPanel.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
		SpringPanel.Begin(dragPanel.gameObject, -newPos, 8f);
		
		*/
		
	}
	void OnDrag(){
		// can be used this way because the button is in the drag panel
		SpringPanel.Begin(dragPanel.gameObject, -transform.localPosition, 8f);		
	}
	
	
}
