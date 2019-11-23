using UnityEngine;
using System.Collections;

public class Shield : BlackHole {
	
	public float BlockingHeat = 2.0f;
	public float ActivationHeat = 500.0f;
	public float ReflectDamage = 125.0f;
	public float VelocityReflectFactor = 1.5f;
	
	//Effect for the gravity.
	public ParticleSystem GravityEffect;
	public ParticleSystem ShieldActivationEffect;
	public ParticleSystem ShieldPushEffect;

	public Material ReflectMaterial;
	
	public bool IsShieldActive = false;
	public float ShieldDrainStartingTransparency = 1.0f;

	public GameObject ShieldCage = null;
	
	//Local value to keep track of playing the animation.
	bool animationPlayed = false;
	bool canDeflect = false;
	bool activationTax = false;
	
	
	public AudioClip ActivationTaxSound;
	
	
	protected ShieldWall shieldWall = null;
	Color32 originalShieldColor;
	
	//Bot we are attached to.
	protected Bot host;
	
	
	// Use this for initialization
	void Start () {
			
		if(ShieldCage)
			ShieldCage.SetActive(false);
	}
	
	public Material GetReflectMaterial()
	{
		//Return the reflection material if there is one set.
		if(ReflectMaterial)
			return ReflectMaterial;
		
		return GetComponent<Renderer>().sharedMaterial;
	}
	public void AnimationStart()
	{
		canDeflect = true;
		
		//Host did not shatter anything as of each start.
		Host.DidShatter = true;
	}
	
	public void AnimationFinish()
	{
		canDeflect = false;
		
		//We see if we are eligable for taxing the bot once the animation is finished.
		if(activationTax)
		{
			//When this function gets called we also add the activation heat tax.
			host.AddHeat(ActivationHeat);
			
			//Play the effect
			if(ShieldActivationEffect)
			{
				//ShieldActivationEffect.transform.position = host.transform.position;
				ShieldActivationEffect.Play();	
			}
			
		if(ActivationTaxSound)
		{
			AudioPlayer.GetPlayer().PlaySound(ActivationTaxSound);
		}
		
		}
		
		
		
		//If we havent shattered anything then we can play the empty shatter animation.
		if(!Host.DidShatter)
		{
			//reset the variable.
			Host.DidShatter = true;

			Host.EmptyShatterEffect.Play();
			GameObjectTracker.instance.EmptyShatter();
		}
		
		
		
	}
	
	public bool CanDeflect
	{
		get { return canDeflect;}
		set {canDeflect = value;}
	}
	

	//Public inferface
	public float GetBlockingHeat()
	{
		return BlockingHeat;
	}
	
	public bool IsActivated()
	{
		return Active;
	}
	
	public void SetHost(Bot h)
	{
		host = h;
	}
	
	public void BallBlocked(Ball b)
	{
		//Call the on block function from the bot.
		host.OnBlockBall(b);
	}
	
	public void Melee(Target t, float speed = 50.0f)
	{
		host.OnMelee(t, speed, t.CanShatter);
	}
	
	// Update is called once per frame
	void Update () {
		
	
		//Update color of shield
		UpdateColor();
		
	}
	
	public void DontTax()
	{
		activationTax = false;
	}
	
	//Overide the Activate
	public void Activate()
	{
		
		if(!shieldWall)
		{
			shieldWall = transform.GetComponentInChildren<ShieldWall>();
			shieldWall.SetHost(this);
			
		}
		
		//Activate the black hole. 
		Active = true;
	
		//Activate the object.
		//shieldWall.gameObject.active = true;
		shieldWall.gameObject.SetActive(true);
		
		//Play Gravity effect
		GravityEffect.Play();
		
		//We set the activation tax bool to true so we are eligiable to get taxed once the animation finishes.
		activationTax = true;
		
		//Handle the shield animation
		if(!animationPlayed)
		{
			GetComponent<Animation>().Play();
			animationPlayed = true;
		}

		// TODO: I belive this is why the shield sound is playing when SFX are off in options
		if(!GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().Play();
		}
	}
	
	//Override the DeActivate
	public void DeActivate()
	{
		if(!shieldWall)
		{
			shieldWall = transform.GetComponentInChildren<ShieldWall>();
			shieldWall.SetHost(this);
			
		}
		
		Active = false;
		
		
		//shieldWall.gameObject.active = false;
		shieldWall.gameObject.SetActive(false);

		//StopCoroutine the gravity.
		GravityEffect.Stop();
		
		//Set the animation tax to false to reset every time we finish using the shield.
		activationTax = false;
		
		//Stop the animation
		GetComponent<Animation>().Stop();
		animationPlayed = false;

		//handle the cage.
		if(ShieldCage)
			ShieldCage.SetActive(false);
		
		GetComponent<AudioSource>().Stop();
		
	}
	
	void UpdateColor()
	{
		if(!Active)
			return;
	
		//Grab the color and edit the alpha channel.
		Color EndColor = shieldWall.gameObject.GetComponent<Renderer>().sharedMaterial.color;
		EndColor.a = 1.0f - (1.0f * host.HeatPercent);
		
	
		//Set the color to a copy of the material.
		shieldWall.gameObject.GetComponent<Renderer>().material.color = EndColor;
		
		
	}
	
	public Bot Host
	{
		get{
			return host;
		}
	}

}
