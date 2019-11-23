using UnityEngine;
using System.Collections;

public class LayoutJoystick : MonoBehaviour {

	//Public UI stuff
	public UISprite JoystickBase, Band, BaseJoint, ThumbStick, Dpad;
	public TweenTransform EndTouchAnimation;
	public float MinWidth, MaxWidth;
	
	public Color RestColor,TenseColor;
	
	
	Vector3 JoystickPosition, ThumbstickPosition, InputPosition;
	Transform thumbstickTransform, band, dpadTransform;
	
	public float AnalogRange = 400.0f;
	
	// Use this for initialization
	void Start () {
	
		//AnalogRange = ToyBox.GetPandora().Controls.AnalogRange.x;
		
		//Cache the local transform.
		thumbstickTransform = ThumbStick.transform;
		band = Band.transform;
		Dpad.alpha = 0.0f;
		dpadTransform = Dpad.transform;
		
		
		
	}
	
	
	
	// Update is called once per frame
	void Update () {
	
			//Get the position of the joystick
		JoystickPosition = ToyBox.GetPandora().Controls.JoystickScreenPosition;
		ThumbstickPosition = ToyBox.GetPandora().Controls.ThumbStickScreenPosition;
		InputPosition = ToyBox.GetPandora().ForceVector;
		
		//Set the Sprite.
		transform.localPosition = JoystickPosition;
		//transform.position = JoystickPosition;
				
		
		//Calculate the angle of the thumbstick.
		//Transform band = Band.transform;
		
		if(thumbstickTransform.localPosition.x < 0)
			band.rotation = Quaternion.AngleAxis(Angle,Vector3.forward);
		else
			band.rotation = Quaternion.AngleAxis(-Angle,Vector3.forward);
		
		
		//Set the wdith of the band
		band.localScale = new Vector3(Width,Distance,1.0f);
		
		float ratio = Width/MaxWidth;
		
		Band.color = Color.Lerp(TenseColor,RestColor ,ratio);
		
		if(InputPosition != Vector3.zero && thumbstickTransform.localPosition == Vector3.zero)
		{
			Dpad.alpha = 1.0f;
			
			Vector3 dir =  InputPosition - Vector3.zero;
			
			if(InputPosition.x > 0)
				dpadTransform.rotation = Quaternion.AngleAxis(-InputAngle,Vector3.forward);
			else
				dpadTransform.rotation = Quaternion.AngleAxis(InputAngle,Vector3.forward);
			
			//print(InputAngle);
		}
		else
		{
			Dpad.alpha = 0.0f;
		}
			
		
		
//		string message = string.Format("Angle : {0}, Distance: {1}, Wdith: {2} Touch:{3}",Angle,Distance,Width,ThumbstickPosition);
//		Debug.Log(message);
//			
		
	}
	
	
	void OnRest()
	{
		//Debug.LogError("REST");
		
		//Zero out to null out any calculation chode.
		thumbstickTransform.localPosition = Vector3.zero;
	}
	
	void EndTouch()
	{
		EndTouchAnimation.Reset();
		EndTouchAnimation.Play(true);
		
		
		
	}
	
	
	
	float Width
	{
		get
		{
			
			float ratio = AnalogRange/Distance;
			float number = MinWidth * ratio;
			return Mathf.Clamp(number, MinWidth, MaxWidth);
		}
	}
	
	float InputAngle
	{
		get
		{
			Vector3 dir = new Vector3(InputPosition.x,InputPosition.z);
			dir = dir - Vector3.zero;
			
			return Vector3.Angle(dir,Vector3.up);
		}
	}
	
	float Angle
	{
		get
		{
			Vector3 dir =  ThumbStick.transform.localPosition - Vector3.zero;
			
			return Vector3.Angle(dir,Vector3.up);
		}
	}
	
	float Distance 
	{
		get
		{
			Vector3 dir = ThumbStick.transform.localPosition - Vector3.zero;
			
			float mag = dir.magnitude;
			
			if(mag == 0)
				return 0.01f;
			
			return mag;
		}
	}
}
