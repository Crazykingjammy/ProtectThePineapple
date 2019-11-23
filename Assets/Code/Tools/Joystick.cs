using UnityEngine;
using System.Collections;

public class Joystick : Controller {
	
	//Figures for the swipe threshhold
	public Vector2 _AnalogRange;	
	public Vector2 _DeadzoneRange;
	
	public float HorizontalDeadzone = 3.0f;
	


	public bool DebugVisuals = true;
	
	
	//Store the current touch
	//Touch currentTouch = null;
	
	//Joypad textures.
	GUITexture analogStick;
	GUITexture joypad;
	GUITexture inputArea;
	
	GUITexture shootArea, shieldArea;
	
	
	//Touch positions
	Vector2 CenterPosition;
	Vector2 _joystickPosition;
	Vector3 _joystickScreenPosition;
	Vector2 unitTouchPosition;
	
	//Stores the magnitude of the input.
	Vector2 magnitude;
	
	//This is the vector to store the inputs position data.
	Vector2 currentPosition;
	
	//AnalogScreenCoords
	Vector3 ThumbStickTouch = Vector3.zero;
	bool isTouching = false;
	
	Rect screen;
	
	public Vector2 AnalogRange
	{
		get{
			return _AnalogRange;
		}
		
		set
		{
//			Debug.Log("Setting ax :" + value.x);
			_AnalogRange = value;
		}
	}
	
	public Vector2 DeadzoneRange
	{
		get{
			return _DeadzoneRange;
		}
		
		set
		{
//			Debug.Log("Setting dx :" + value.x);
			_DeadzoneRange = value;
		}
	}
	
	// Use this for initialization
	void Start () {
	
		//Get access to the stick
		analogStick = transform.Find("InputPosition").GetComponent<GUITexture>();
		joypad = transform.Find("JoypadPosition").GetComponent<GUITexture>();
		
		inputArea = transform.Find("InputArea").GetComponent<GUITexture>();
		inputArea.enabled = false;
		
		shootArea = transform.Find("ShootArea").GetComponent<GUITexture>();
		shootArea.enabled = false;
		
		shieldArea = transform.Find("ShieldArea").GetComponent<GUITexture>();
		shieldArea.enabled = false;
		
		
		//We get the screen width and height here incase it changes the next or previous frame.
		screen = new Rect(0.0f,0.0f,Screen.width,Screen.height);
		
		
		//Setting the center position
		
		
		//CenterPosition.x = 150.0f;
		//Set the Y position to the position of the right button 
		//CenterPosition.y = RightButton.transform.position.y/screen.height;
		
		_joystickPosition.x = joypad.transform.position.x;
		_joystickPosition.y = joypad.transform.position.y;
		
		
		//Disable the texutre
		joypad.GetComponent<GUITexture>().enabled = false;
		analogStick.GetComponent<GUITexture>().enabled = false;
		
		
		//Lets load here...
		
		DeadzoneRange = _DeadzoneRange;
		AnalogRange = _AnalogRange;
				
		Vector2 pos = GameObjectTracker.instance._PlayerData.GameOptions.JoystickPosition;
		
		if(pos != Vector2.zero)
		{
//			Debug.Log("loadedddddddddddddddddd: " + pos);
			JoystickPosition = pos;
		}
		
		float deadzone = GameObjectTracker.instance._PlayerData.GameOptions.JoystickDeadzone;
		if(deadzone > 0.0f)
			DeadzoneRange = new Vector2(deadzone,deadzone);
		
		float analogRange = GameObjectTracker.instance._PlayerData.GameOptions.JoystickRange;
		if(analogRange > 0.0f)
			AnalogRange = new Vector2(analogRange,analogRange);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		ProcessInput();
		
	}
	
	
	
	override protected void ProcessInput()
	{
				
		if(!Input.multiTouchEnabled)
		{
			return;
		}
		
		if(Input.touchCount <= 0)
		{
			return;
		}
		
		
		//Store the value of the first touch position.
	
		foreach(Touch t in Input.touches)
		{
			//Get the first touch that is in the joystick area and thenb reak.	
			if(inputArea.HitTest(t.position) )
			{
				ProcessTouch(t);		
				break;
			}
			
			
//			if(shootArea.HitTest(t.position))
//			{
//				ToyBox.GetPandora().Bot_01.Shoot();
//			}
//			
//			if(shieldArea.HitTest(t.position))
//			{
//				ToyBox.GetPandora().Bot_01.ActivateDefense();
//			}
//			else
//			{
//				ToyBox.GetPandora().Bot_01.DeactivateDefense();
//			}
			
			//ProcessTouch(t);
			
		}		
		
		
		//Apply the input vector if we have touches.
		ApplyInputVector();
		
		
	}
	
	

	
	
	void ProcessTouch(Touch t)
	{
		
		
		
		//blah.
		Vector3 joystickImagePosition = Vector3.zero;
		
		//Zero out the current touch postion upon start.
		unitTouchPosition = Vector2.zero;
		
		
		switch (t.phase)
		{
			case TouchPhase.Began:
			{
						
			isTouching = true;
			HandleJoystickVisuals(t);			
			
			
			break;
			}
			
			case TouchPhase.Moved:
			{
			
		
			HandleAnalogJoystickDelta( t);
				
				break;
			}
			case TouchPhase.Stationary:
			{
			
			//Doing quite nothing.
						
			break;
			
			}
				
			case TouchPhase.Ended:
			{
				
			//Close some values of needed.
			EndJoystickTouch();
			
				
				break;
			}
				
			case TouchPhase.Canceled:
			{
			
			//Handle end.
			EndJoystickTouch();
			
				
				break;
				
			}
			
			
			
		}
		
		
	//Here we apply our values incase the touch stick position is within the deadzone.
	//Here sets the current position too
	HandleInputDeadzone(t);
			
		
	}
	
	
	
	
	#region Functions
	
		
	void ResizeOnDoubleTap()
	{
		//Resize on doublt tap.
		if(Input.touches[0].tapCount  > 2 && joypad.HitTest(Input.touches[0].position))
		{
			
			if(joypad.pixelInset.width == 128.0f)
			{
				Rect joysize = new Rect(-32.0f,-32.0f,64.0f,64.0f);
				
				joypad.pixelInset = joysize;
			}
			else
			{
				Rect joysize = new Rect(-64.0f,-64.0f,128.0f,128.0f);
				
				joypad.pixelInset = joysize;
			}
			
		}
		
		
	}
	
	
	void EndJoystickTouch()
	{
	
		if(DebugVisuals)
		{
			//Disable back the texture.
			//joypad.enabled = false;
			analogStick.enabled = false;	
		}
		
		
		//Set the bool.
		isTouching = false;
				
		//Zero out the current positon when we end touch.
		currentPosition = Vector2.zero;
		magnitude = Vector2.zero;

	}
	
	
	
	void HandleInputDeadzone(Touch t)
	{
		//We need to make sure current postion vector is 0.
		currentPosition = Vector3.zero;
		

		//We recalculate the center position. They are based on the joystick position value.
		CenterPosition.x = _joystickPosition.x * screen.width;
		CenterPosition.y = _joystickPosition.y * screen.height;	
		
		//Current position is where your finger is relative to the center
		//or you can say the 'stick' position and how much its tilted. 
		currentPosition =  t.position - CenterPosition;
		//print("Current Position magnitude" + currentPosition.magnitude);
			
		//Now that we have the the postion, we test if pur positions magnitude is beyond our deadzone.
		if(Mathf.Abs(currentPosition.x) < (DeadzoneRange.x/4.0f ))
		{
			currentPosition.x = 0.0f;
		}

		if(Mathf.Abs(currentPosition.y) < (DeadzoneRange.y/4.0f))
		{
			currentPosition.y = 0.0f;
		}
		
		
		//We will look for the delta y anyway!
		//currentPosition.y += deltay;
		//currentPosition.x += deltax;		
		
		//Set the thumb stuck touch position.
		ThumbStickTouch.Set(currentPosition.x,currentPosition.y,0.0f);
		
	}
	
	void HandleAnalogJoystickDelta(Touch t)
	{
		//We set the current position to the previous touch that passed the deadzone.
		
		
		//Set the position of touch to the analog stick.
		//ThumbStickTouch.Set(t.position.x,t.position.y,0.0f);
						
		//update teh visual position.
		HandleJoystickVisuals(t);
		
	}
	

	
	void HandleJoystickVisuals(Touch t)
	{
		
		if(!DebugVisuals)
			return;	
		
		//Set the position of the screen in percentage of the screen size for unity format.
		unitTouchPosition.x = t.position.x/screen.width;
		unitTouchPosition.y = t.position.y/screen.height;
			
		//Temp storage for the new position.
		Vector3 joystickImagePosition = Vector3.zero;
		joystickImagePosition.Set(unitTouchPosition.x,unitTouchPosition.y,0.0f);
			
		//Tranfering the temp position to the transform.
		analogStick.transform.position = joystickImagePosition;
		
			
		

		
		//Display the joystick image.
		joypad.GetComponent<GUITexture>().enabled = true;
		analogStick.enabled = true;	
		
	}
	
	void CalculateMagnitude()
	{
		//Get the absolute value as we dont need negitive to check magnitude.			
		float absX = Mathf.Abs(currentPosition.x);
		float absY = Mathf.Abs(currentPosition.y);
			
		//Clamp the values so we dont get higher
		//If we have delta movement. set the absx to the delta's version. if not then just use regular absx as the values.
		absX = Mathf.Clamp(absX,0.0f,AnalogRange.x);
		absY = Mathf.Clamp(absY,0.0f,AnalogRange.y);	
		
		
		//Calculate Magnitude by deviding the current position in relation to the max position.
		if(absX > 0.0f )
		{
			magnitude.x = (absX) / AnalogRange.x;	
		}
	
		if(absY > 0.0f)
		{
			magnitude.y = (absY) / AnalogRange.y;	
		}

	}
	
	void ApplyInputVector()
	{
		//Set the values again if we are using hte dpad
		InputDirection = Vector3.zero;
		
		CalculateMagnitude();
		
		//Clamp the values between -1 and 1
		float x = Mathf.Clamp(currentPosition.x,-1.0f,1.0f);
		float y = Mathf.Clamp(currentPosition.y,-1.0f,1.0f);
		
		//Checking for the direciton and applying the vector accordingly.
		if(x > 0.0f )
			InputDirection += (Vector3.right) * magnitude.x;
		
		if(x < 0.0f)
			InputDirection += -(Vector3.right) * magnitude.x;
		
		if(y > 0.0f)
			InputDirection += (Vector3.forward) * magnitude.y;
		
		if(y < 0.0f)
			InputDirection += -(Vector3.forward) * magnitude.y;
		
		


		//Zero out in the end.
		currentPosition = Vector3.zero;
		
		
	}
	
	#endregion
	
	#region Accessors
	
	public Vector3 ThumbStickScreenPosition
	{
		get{
			
			if(isTouching == false)
				return Vector3.zero;
			
			return ThumbStickTouch;
			
		}
	}
	
	public Vector2 JoystickPosition
	{
		get{
			//return new Vector2(_joystickPosition.x * screen.width, _joystickPosition.y * screen.height);}
			return _joystickPosition;
			
		}
		set
		{
			//If we are in screen coords
			if(value.x > 1.0f)
			{
				//Set the position of the screen in percentage of the screen size for unity format.
				Vector2 newPos = new Vector2(value.x/1024.0f,value.y/768.0f);
				_joystickPosition = newPos;
				
				//joypad.transform.position = newPos;
				
				if(DebugVisuals)
				{
					joypad.transform.position = new Vector3(_joystickPosition.x,_joystickPosition.y,0.0f);
					Debug.LogError("Screen Position set to :" + _joystickPosition);
				}
				
				
				return;
			}
			
			_joystickPosition = value;
			
			//joypad.transform.position = value;
			
			if(DebugVisuals)
			{
				joypad.transform.position = new Vector3(_joystickPosition.x,_joystickPosition.y,0.0f);
				
				Debug.LogError("Position set to :" + _joystickPosition);
			}
			
			
		}
	}
	
	public Vector3 JoystickScreenPosition
	{
		get
		{
			return new Vector3(_joystickPosition.x * 1024.0f, _joystickPosition.y * 768.0f,0.0f);
		}
	}
	
	#endregion
	
	
}



