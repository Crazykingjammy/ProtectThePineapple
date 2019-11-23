using UnityEngine;
using System.Collections;

public class AnimationTimer {
	
	public float Timer = 0.0f;
	
	//Number to signal when animation timer has counted enoughb
	public float Full = 1.0f;
	
	//Values fo rhow fast we will add up to full.
	public float min = 0.001f;
	public float max = 2.7f;
	
	public float MessagePoint = 0.6f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	
		if(IsFull())
		{
			return;
		}
		
		Timer += Mathf.Lerp(max,min,Timer) * Time.fixedDeltaTime;
		
	}
	
	public void ResetAnimationTimer(bool absolute = false)
	{
		//The absolute value will reset the timer no matter what.
		if(absolute)
		{
			Timer = Full;
		}
	
		//This adds to the timer what was left over from being full after resetting.
		float temp = Timer;
		Timer = Full - temp;
		
	}
	
	
	public float GetValue()
	{
		return Timer;
	}
	
	public void Fill()
	{
		Timer = Full;
	}
	
	public bool IsFull()
	{
		if(Timer >= Full)
		{
			Timer = Full;
			return true;
		}
		
		return false;
	}
	
	public bool IsMessageTriggered(int index = 0)
	{
		if(Timer > MessagePoint)
		{
			return true;
		}
		
		return false;
	}
}
