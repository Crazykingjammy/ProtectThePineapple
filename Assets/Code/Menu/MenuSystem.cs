using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSystem : MonoBehaviour {
	
	//List of buttons to be stored for group functions.
	public List<Label> ButtonsList;
	
	//Values for animation.
	public float FreezeTimer = 0.6f;
	
	protected AnimationTimer timer = new AnimationTimer();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	

	
	protected void ResetAnimationTimer(bool absolute = false)
	{
		//The absolute value will reset the timer no matter what.
		timer.ResetAnimationTimer(absolute);
		
	}
	
	protected void MoveToOriginal(Label lbl, Label point, float scale = 1.0f)
	{	
		
		lbl.transform.position = Vector3.Lerp(point.transform.position,lbl.GetOriginalPosition(),timer.GetValue() * scale);
		
		lbl.transform.localScale = Vector3.Lerp(point.transform.localScale,lbl.GetOriginalScale(),timer.GetValue() * scale);
		
		
	}
	
	protected void MoveToPoint(Label lbl, Label point, float scale = 1.0f)
	{
		lbl.transform.position = Vector3.Lerp(lbl.GetOriginalPosition(),point.transform.position,timer.GetValue() );
		lbl.transform.localScale = Vector3.Lerp(lbl.GetOriginalScale(),point.transform.localScale, timer.GetValue() );
	}
	
	
}
