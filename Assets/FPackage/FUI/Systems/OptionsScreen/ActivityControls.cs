using UnityEngine;
using System.Collections;

public class ActivityControls : FUIActivity {
	
	//Variables for editing buttons
	public float ScaleRange = 2.0f;
	public float MinScaleRange = 0.5f;
	
	//Store the transofrm of the main object.
	public Transform JoystickPosition, ShieldButton, ShootButton;
	public ControlsManager controlsManager;
	
	//Sprites for visuals.
	public UISprite DeadZone, AnalogeRange, ShootArea, ShieldArea;
	public UILabel DeadZoneValue, RangeValue;
	
	//Sliders to change values.
	public UISlider sliderDeadzone, sliderRange, sliderShield, sliderShoot;
	
	//Calculate the range of configureation in relation to 0 and 1 for sliders.
	public float Deadzone_Min, Deadzone_Max, Range_Min, Range_Max = 50.0f;
	float DZRange, RangeR;
	
	Vector2 RValue = Vector2.zero;
	Vector2 DZValue = Vector2.zero;
	
	public Transform ShootKnob, ShieldKnob;
	
	
	//Caches
	Joystick _joystick = null;
	Transform _joystickTransform;
	static bool closed = true;
	
	ControlsLayoutObject loadedControls = null;
	
	
	// Use this for initialization
	new void Start () {
	base.Start();
		
		//Calculate the ranges.
		DZRange = Deadzone_Max - Deadzone_Min;
		RangeR = Range_Max - Range_Min;
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		
//		if(ShootButton.position.x > ShieldButton.position.x)
//		{
//			ShootArea.pivot = UIWidget.Pivot.Right;
//			ShieldArea.pivot = UIWidget.Pivot.Left;
//		}
//		else
//		{
//			ShootArea.pivot = UIWidget.Pivot.Left;
//			ShieldArea.pivot = UIWidget.Pivot.Right;
//			
//		}
	
	}
	
	
	public override void OnActivate ()
	{
		//Just incase, we shuld always be 'closed' as we are actiavted. right?
		closed = true;
		
		
		//Calculate the ranges.
		DZRange = Deadzone_Max - Deadzone_Min;
		RangeR = Range_Max - Range_Min;
		
		//Grab the joystick upon activate.
		_joystick = ToyBox.GetPandora().Controls;
	
		
		//Set the position of the joystick. 
		JoystickPosition.localPosition = _joystick.JoystickScreenPosition;
		
		//Get the loaded controls to access
		loadedControls = controlsManager.LoadedControls;
		
		//Set the deadzone and the analog range.
		DeadZone.transform.localScale = new Vector3(_joystick.DeadzoneRange.x,_joystick.DeadzoneRange.y,1.0f);
		AnalogeRange.transform.localScale = new Vector3(_joystick.AnalogRange.x,_joystick.AnalogRange.y,1.0f);
		
		
		//Load the adjusted values from here.
		ShieldButton.localPosition = loadedControls.Shield.localPosition;
		ShootButton.localPosition = loadedControls.Shoot.localPosition;
		
		
		//Load values from controler and apply to the knobs.
		ShootKnob.transform.localPosition = loadedControls.ShootButton.transform.localPosition;
		ShieldKnob.transform.localPosition = loadedControls.ShieldButton.transform.localPosition;
		
		ShootKnob.transform.localScale = loadedControls.ShootButton.transform.localScale;
		ShieldKnob.transform.localScale = loadedControls.ShieldButton.transform.localScale;
		

		
		
		RValue = _joystick.AnalogRange;
		DZValue = _joystick.DeadzoneRange;
		
		sliderRange.onValueChange += OnRangeChange;
		sliderDeadzone.onValueChange += OnDeadzoneChange;
		
		closed = false;
		
		//Update the sliders to match.
		UpdateSliders();
		
		UpdateVisual();
		
		
	//	Debug.LogError("Finish Start");
		
	}
	
	
	void OnClose()
	{
		//Debug.LogError("are we here?");
		
		//Do the saving here.
		Vector2 jp =  new Vector2(JoystickPosition.transform.localPosition.x,JoystickPosition.transform.localPosition.y);
		//_joystick.JoystickPosition = jp;
		
		
		
		//Set the position of the joystick.
		GameObjectTracker.instance._PlayerData.GameOptions.JoystickPosition = jp;
		
		//Set the range and scale.
		GameObjectTracker.instance._PlayerData.GameOptions.JoystickRange = RValue.x;
		GameObjectTracker.instance._PlayerData.GameOptions.JoystickDeadzone = DZValue.x;
		
		
		loadedControls.Shield.localPosition = ShieldButton.localPosition;
		loadedControls.Shoot.localPosition = ShootButton.localPosition;
		
		loadedControls.Shield.localScale = ShieldButton.localScale;
		loadedControls.Shoot.localScale = ShootButton.localScale;
		
		GameObjectTracker.instance._PlayerData.GameOptions.ShootButton = ShootButton;
		GameObjectTracker.instance._PlayerData.GameOptions.ShieldButton = ShieldButton;
		
		//Call close function
		Close();
	}
	
	
	void OnCancle()
	{
		//Just close
		Close();
	}
	
	void Close()
	{
		closed = true;
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);
		ActivityManager.Instance.PopActivity();	
	}
	
	
	
	void OnRangeChange(float pos)
	{
		if(_joystick == null || closed)
		{
		//	Debug.LogError("Why are we here??!");
			return;
			
			
		}
		
		float m = (RangeR * pos);
		float v = Range_Min + m;
		
		//Update the value on joystick then update teh visuals.
		RValue = new Vector2(v,v);
		
		UpdateVisual();
		
		
	}
	
	void OnDeadzoneChange(float pos)
	{
		if(_joystick == null || closed)
		{
			//Debug.LogError("shit...");
			return;
		}
		
	//	Debug.LogError("RangeChange: " + pos);
		
		float m = (DZRange * pos);
		float v = Deadzone_Min + m;
		
		DZValue = new Vector2( v, v );
		
		UpdateVisual();
	}
	
	void OnShieldScale(float pos)
	{
	
		//Debug.LogError("Slider POS: " + pos );
		
	 
		//Calculate the scale.
		float current = MinScaleRange + (MinScaleRange *  (ScaleRange * pos));
		

		//Apply the scale.
		ShieldButton.localScale = new Vector3(current,current,1.0f);
		
		 
		
	}
	
	void OnShootScale(float pos)
	{
		//Calculate the scale
		float current = MinScaleRange + (MinScaleRange *  (ScaleRange * pos));
		//float current = pos * ScaleRange;
		
		//Apply the scale.
		ShootButton.localScale = new Vector3(current,current,1.0f);
		
	}
	
	
	void UpdateSliders()
	{

		if(closed)
			return;
		
		
		//Set the position of the sliders accordingly.
		//We will currently use X only, making the ranges uniforn for now.
		float dx = DZValue.x;
		float ax = RValue.x;
		float dv = (dx - Deadzone_Min)/DZRange;
		float av = (ax - Range_Min)/RangeR;
		
		//Debug.LogError("dx: " +dx + " ax: " + ax + " dv: " + dv + " av: " + av);
		
		sliderDeadzone.sliderValue = dv;
		
		
		//Set the range value.
		sliderRange.sliderValue = av;
	
		
		//loadedControls.gameObject.SetActive(true);
		sliderShield.sliderValue = loadedControls.Shield.localScale.x/ScaleRange;
		sliderShoot.sliderValue = loadedControls.Shoot.localScale.x/ScaleRange;
		
	
		//Update all sliders.
		sliderDeadzone.ForceUpdate();
		sliderRange.ForceUpdate();
		sliderShield.ForceUpdate();
		sliderShoot.ForceUpdate();
//		
		
	//	Debug.LogError("Update Slider FInish");
		
		
	}
	
	
	void UpdateVisual()
	{
		if(_joystick == null)
			return;
		
		
		//Set the position of the joystick. 
		JoystickPosition.localPosition = _joystick.JoystickScreenPosition;
		
		//Set the deadzone and the analog range.
		DeadZone.transform.localScale = new Vector3(DZValue.x ,DZValue.y,1.0f);
		AnalogeRange.transform.localScale = new Vector3(RValue.x,RValue.y,1.0f);
		
		//sliderDeadzone.ForceUpdate();
		//sliderRange.ForceUpdate();
		int d = (int)DZValue.x;
		int r = (int)RValue.x;	
		
		DeadZoneValue.text = d.ToString();
		RangeValue.text = r.ToString();
		
		
	}
	
	
	void OnDefaultControls(string option)
	{
		
		switch(option)
		{
			
		case "No":
		{
			//Do nothing. 
			break;
		}
		case "YES!":
		{
			
			//Set default values.
			ShootButton.localScale.Set(1.0f,1.0f,1.0f);
			ShieldButton.localScale.Set(1.0f,1.0f,1.0f);
			
			ShootButton.localPosition = Vector3.zero;
			ShieldButton.localPosition = Vector3.zero;
			
			UpdateSliders();
			
			break;
		}
		default:
		{
			//Do nothing.
			break;
		}
		
			
		}
		
		
	}
	
	
		
	void OnDefaultJoystick(string option)
	{
		
		switch(option)
		{
			
		case "No":
		{
			//Do nothing. 
			break;
		}
		case "YES!":
		{
			
			//Set default values.
			
			
		_joystick.DeadzoneRange = new Vector2 (50.0f,50.0f);
		_joystick.AnalogRange = new Vector2(120.0f,120.0f);
	
		_joystick.JoystickPosition = new Vector2(0.16f,0.22f);
			
		//We will currently use X only, making the ranges uniforn for now.
		//sliderDeadzone.sliderValue = _joystick.DeadzoneRange.x/Deadzone_Max;

		//Set the range value.
		//sliderRange.sliderValue = _joystick.AnalogRange.x/Range_Max;
	
		UpdateSliders();
		UpdateVisual();
			
			
			
			break;
		}
		default:
		{
			//Do nothing.
			break;
		}
		
			
		}
		
		
	}
	
	
	
}
