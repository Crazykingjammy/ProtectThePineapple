using UnityEngine;
using System.Collections;

public class HeatBarObject : FUISliderBar {
	
	float curHeat = 0;
	float maxHeat = 0;
	//float lastFrameHeat = 0;
	
	public string NeedCannonSprite, WarningSprite;
	
	public UILabel highTemp;
	public UILabel lowTemp;
	public UILabel curTemp;
	public UISprite Pointer;
	
	public Transform Current;
	
	//Warning info
	public UISprite WarningLabel, Animal, WarningIcon, QuestionMark;
	public TweenTransform WarningAnimation;
	public TweenTransform ThermometerAnimation;

	public TweenTransform AnimalAnimation;
	//public TweenScale AnimalAnimation;
	public TweenAlpha AnimalAlphaA;
	public UIPanel TheometerWindowPanel;
	
	bool temp = false;
	bool botcannon = false;
	
	Bot myTempBot;
	
	// added a light
	public Light HeatLamp;

	// Adding support to animate the counter reset object
	public CounterResetNotification counterResetNotif = null;
	
	// Use this for initialization
	new protected void Start () {
		base.Start();
				// not sure where this came from...  but commented it out, after updating to ngui 2.3.6a

		// setting the local cached bot
		// Check make sure this is actually a BOT!
		myTempBot = MyTarget.GetComponent<Bot>();

		if (myTempBot != null){
			// tell the bot that I am tracking you
			myTempBot.BotNotificationHUB = this;		
		}

		//maxHeat = Slider.fullSize.x;
		//maxHeat = Slider.foreground.localScale.y;
		
		maxHeat = Slider.fullSize.y;
		
		
		highTemp.alpha = 0.0f;
		lowTemp.alpha = 0.0f;
		curTemp.alpha = 0.0f;
		Pointer.alpha = 0.0f;
		
		highTemp.text = myTempBot.MaxTemperature.ToString();
		lowTemp.text = myTempBot.MinTemperature.ToString();
		
		
		//Disable the label on start.
		WarningAnimation.Play(false);
		//ThermometerAnimation.Play(false);
			
		temp = false;
		
		//Animal.alpha = 0.0f;
		Animal.enabled = false;
		
	}
	
	void FixedUpdate()
	{
		//curTemp.text = myTempBot.CurrentTemp.ToString();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (MyTarget == null)
		{
			// if I dont have a target, remove me from the scene
			GameObject.Destroy(this.gameObject);
			return;
		}
		// make sure to follow the target
		transform.localPosition = MyTarget.transform.position;
		if (myTempBot == null)
		{	
			Debug.LogError("HeatbarObject: Target host, is not a Bot object");
			return;
		}
		curHeat = myTempBot.Temperature;
		
		if (curHeat > maxHeat) curHeat = maxHeat;
		if (curHeat <= 0 ) curHeat = 0.0009f;
		
		
		Vector3 heatscale = new Vector3(Slider.foreground.localScale.x, curHeat, Slider.foreground.localScale.z);
		
		//Slider.foreground.localScale = heatscale;
		Slider.sliderValue = myTempBot.HeatPercent;
		
		
		//Play the cannon animation if we attached a cannon.
		if(myTempBot.VisibleTemp && !botcannon)
		{
			TheometerWindowPanel.alpha = 1.0f;

			//Set image.
			Animal.spriteName = myTempBot.GetCannon().CannonTextureName;
			
			//Play animation and set bool
			Animal.enabled = true;
			//Animal.alpha = 1.0f;
			AnimalAnimation.Reset();
			AnimalAnimation.Play(true);
			
			AnimalAlphaA.Reset();
			AnimalAlphaA.Play(true);
			
			botcannon  = true;
			
			ThermometerAnimation.Reset();

		}
		
		if(!myTempBot.VisibleTemp)
		{
			botcannon = false;
			Animal.enabled = false;
			// no cannon attached, pulling away the thermometer
			
//			WarningAnimation.Reset();
//			WarningAnimation.Play(false);
			WarningIcon.spriteName = NeedCannonSprite;
			QuestionMark.alpha = 1.0f;

			highTemp.alpha = 0.0f;
			lowTemp.alpha = 0.0f;
			curTemp.alpha = 0.0f;
			Pointer.alpha = 0.0f;
			ThermometerAnimation.Play(false);

			TheometerWindowPanel.alpha = 0.0f;
			//			temp  = false;
			
		}
		else{

			WarningIcon.alpha = 0.0f;
			WarningIcon.spriteName = WarningSprite;
			QuestionMark.alpha = 0.0f;
		}
		

		//Set the labels if we are shielded.
		if(myTempBot.IsShieldActive() || myTempBot.Warning)
		{
			//Turn on
			highTemp.alpha = 1.0f;
			lowTemp.alpha = 1.0f;
			curTemp.alpha = 1.0f;
			Pointer.alpha = 1.0f;
			

			//Update variables.
			curTemp.text = myTempBot.CurrentTemp.ToString();
			
			Vector3 pos = Vector3.zero;
			pos.y = myTempBot.HeatPercent;
			
			//curTemp.transform.localPosition = pos;
			Current.localPosition = pos;	
			
			ThermometerAnimation.Play(true);
			
			SetHeatLampIntensity(8);
			SetHeatLampRange(15);
		}
		else
		{
			highTemp.alpha = 0.0f;
			lowTemp.alpha = 0.0f;
			curTemp.alpha = 0.0f;
			Pointer.alpha = 0.0f;
			ThermometerAnimation.Play(false);
			
			SetHeatLampIntensity(1);
			SetHeatLampRange(10);
		}
		
		
		
		//Handle warning info
		if(!myTempBot.Warning)
		{
			//WarningAnimation.Reset();
			WarningAnimation.Play(false);
			WarningLabel.alpha = 0.0f;
			temp = false;
			//WarningLabel.gameObject.SetActive(false);
			
		}
		
		
		if(!temp && myTempBot.Warning)
		{
			//WarningLabel.gameObject.SetActive(true);
			
			//WarningAnimation.Reset();
			WarningAnimation.Play(true);
			WarningLabel.alpha = 1.0f;
			temp  = true;
			
		}
		
	}
	
	
	void CannonDone()
	{
		Animal.enabled = false;
	}
	
	void SetHeatLampIntensity(int intensity){
		if (HeatLamp == null){
			return;
		}
		
		HeatLamp.intensity = intensity;

	}
	
	void SetHeatLampRange(int range){
		if (HeatLamp == null){
			return;
		}
		
		HeatLamp.range = range;	
	}

	public void PlayCounterResetAnimation(){
		counterResetNotif.PlayCounterReset();
	}

}
