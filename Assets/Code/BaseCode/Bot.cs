using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Bot : MonoBehaviour {
	
	//Force to push 
	public float ForcePush;
	public float DashFactor;
	
	public float BasicForcePush;
	public float ForcePushShieldDamp = 0.3f;
	public float PushBackForceFactor = 150.0f;
	public float ShieldDampDrag = 7.0f;
	public float StartingDrag = 2.56f;
	public float originalDrag;
	
	
	//Value for the amount of heat the bot has.
	public float Temperature = 100;
	int currentTemp = 0;
	
	public float MaxTemperature = 200;
	public float MinTemperature = 75;
	public float LowestTemperature = 0;
	public float CoolingRate = 3.0f;
	public float OverHeatPickupWaitTime = 1.0f;
	public float DashBufferTime = 0.25f;
	
	
	public float BlockingCoolDownFactor = 7.0f;
	public float HeatWarningPercentage = 0.75f;
	public float DeflectHoldingTime = -5.0f;
	public float DeflectForceFactor = 1500f;
	public float DeflectCCScale = 0.5f;
	public float MeleeDamage = 400.0f;

	public string PickupBallAnimation = "BotPickUpBall";
	public string PickupCannonAnimation = "BotPickUpCannon";

	float InputBackBuffer = -0.9f;
	float pickuptimestamp = 0.0f;
	bool dashed = true;
	bool deadInputOnShield = false;
	bool didshattered = false;

	//Bool for if we shoudl display the temp guage.
	bool isTempVisible = false;
	
	Vector3 moveForce = Vector3.zero;
	Vector3 reflectForce = Vector3.zero;
	
	TrailRenderer trail;
	Magnet myMagnet;
	Transform myTransform;
	Rigidbody Rdbdy = null;
	
	//Cannon to shoot and stuff with.
	//public Cannon CannonAttachmentSlot;
	
	//Shield which is part of the bot.
	protected Shield DefenseUnitSlot;
	
	//The Command Center that will be the lifeline of the bot!
	public CommandCenter Zion;
	
	//Public Particle system for the collection image
	public ParticleSystem Collection;
	
	//Public particle system for collection of balls.
	public ParticleSystem BallCollection;
	public ParticleSystem OverheatEffect;
	public ParticleSystem CaptureBallEffect;
	public ParticleSystem BlockEffect;
	public ParticleSystem ShieldPushEffect;
	public ParticleSystem DeflectEffect;
	public ParticleSystem DamageEffect;
	public ParticleSystem ShatterEffect, EmptyShatterEffect;
	public ParticleSystem OverheatWarningEffect;

	// Particle System for colliding with a wall boundary
	// this is a prefab reference, bot instantiates from prefab  ** Frank, 5/4/14
	public ParticleSystem WallCollideParticleEffectPreb = null;
	ParticleSystem WallCollideParticleEffect = null;
	
	private CannonConnector connectorcontroller;
	private CannonConnectorPos cannonConnectorPos;	// the bot can keep track where the cannon is
	public Vector3 CannonPosition {
		get{ return cannonConnectorPos.transform.position; }
	}
	
		
	//Value for if we are over heated or not.
	bool IsOverHeated;
	bool WarnedPlayer = false;

	Vector2 materialOffset = Vector2.zero;
	
	//Direction
	public bool Forward = true;
	
	
	//Sound files
	public AudioClip HurtSound;
	public AudioClip PickUpCannonSound;
	public AudioClip PickUpAmmo;
	public AudioClip CollectSound;
	
	public AudioClip CannonFilled;
	public AudioClip OverheatDanger;
	public AudioClip ReflectSound;
	public AudioClip ShatterSound;
	public AudioClip BlockSound;
	public AudioClip CaptureSound;
	public AudioClip MeleeSound;

	// added 042014
	// the bot needed access for the heat bar (which is now a notification HUB)
	private HeatBarObject myHeatBar = null;

	// a flag to check wheater we are in cannon pickup animation state
	protected bool isAnimatingCannonPickup = false;
	public bool IsAnimatingCannonPickup{
		get{ return isAnimatingCannonPickup;}
	}
	// handy to fix the movement animations
	// 

	public HeatBarObject BotNotificationHUB{
	get{ return myHeatBar;		}
	set{
			HeatBarObject test = value.GetComponent<HeatBarObject>();
			if (test != null){
			myHeatBar = value;
			}
		}
	}

	// we're caching the Notification HUB to support showing outside info
	// currently adding support to pop up the reset counter notification


	// end added 042014
	
	
	protected void BaseBotStart()
	{
	}
	
	public bool DidShatter
	{
		get{
			return didshattered;
		}
		set {
			didshattered = value;
		}
	}
	// Use this for initialization
	void Start () {
		originalDrag = StartingDrag;
	}
	
	#region Controls
	
	
	public void Move (Vector3 force)
	{
		
		reflectForce = force;
		
		if(!Rdbdy)
			Rdbdy = GetComponent<Rigidbody>();

		//Revert to original drag.
		Rdbdy.drag = originalDrag;
		
		//Dont move if the shield is on!
		if(DefenseUnitSlot.IsActivated())
		{
			//rigidbody.AddForce(force,ForceMode.Acceleration);
			//DefenseUnitSlot.DeActivate();
			//Dash(force);
			
			//Apply the drag here.
			if(DefenseUnitSlot.CanDeflect)
			{
				Rdbdy.drag = ShieldDampDrag;
				
				//Clamp the move force so we dont go anywhere when we push back on shield.
				if(moveForce.z < 0.0f)
				{
					moveForce.z = 0.0f;
					
					//If we didnt shatter already, mark it false.
					if(DidShatter)
					DidShatter = false;
				}
				
				
			}
			
			//Zero out the move force of shield is activated. 
			moveForce = force * ForcePushShieldDamp;
			
			return;
		}
		
		moveForce = force;
		//Move the body according to the given force for move.
		//rigidbody.AddForce(force,ForceMode.Force);
	}
	
	
	void Dash()
	{
		if(!dashed)
		{
			
			//Trail rendere
			if(trail)
			{
				trail.enabled = true;
			}
	
			Vector3 dashForce = Vector3.zero;
			
			if(moveForce.x > 0.0f )
				dashForce += (Vector3.right);
			
			if(moveForce.x < 0.0f)
				dashForce += -(Vector3.right);
			
			if(moveForce.z > 0.0f)
				dashForce += (Vector3.forward);
			
			if(moveForce.z < 0.0f)
				dashForce += -(Vector3.forward);
			
			
			//Dash vector.
			Rdbdy.AddForce(dashForce * DashFactor,ForceMode.Impulse);
				
			//No longer dashing or dashable.
			dashed = true;	
			deadInputOnShield = false;
				
		}
		
		
	}
	
	public void ActivateDefense()
	{
		//If we dont have a cannon we cannot shield!
//		if(!connectorcontroller.GetCannon())
//		{
//			return;
//		}
		
		if(moveForce == Vector3.zero)
		{
			//print("Zero on Shield");
			deadInputOnShield = true;
		}
		else
			deadInputOnShield = false;


		//Play the attack animation
		//animation.Play("Attack");

		//Here is where we turn on the shield.
		DefenseUnitSlot.Activate();
		
		
		

		
	}
	
	public void Shoot()
	{
		//Have to make sure we have a cannon
		if(!connectorcontroller.GetCannon())
		{
			return;
		}

		if(IsShieldActive())
		{
			return;
		}
		
		//Add a pushback force.
		float f = connectorcontroller.GetCannon().GetPushBackFactor() * PushBackForceFactor;
		Rdbdy.AddForce(myTransform.forward * -f);
		
		//And we call the cannon's shoot inside the add heat function.
		AddHeat( connectorcontroller.GetCannon().Shoot() );
		
		//Send the message
		GameObjectTracker.GetGOT().PlayerShoot();
	}
	
	
	//Gets the bots cannon, returns null if there is no cannon.
	public Cannon GetCannon()
	{
		return connectorcontroller.GetCannon();
	}
	
	public void DeactivateDefense()
	{
		//Dashing variables
		dashed = false;
		deadInputOnShield = false;
		
		//Trail rendere
//		if(trail)
//		{
//			trail.enabled = false;
//		}
//	
		//Stop the Sound 
		//audio.Stop();
		
		DefenseUnitSlot.DeActivate();
	}
	
	public void OnMeleeCannon(Cannon c, float speed)
	{
		//We must be in deflect timing!
		if(!DefenseUnitSlot.CanDeflect)
		{
			//Whatever we want to register when the shield touches a target outside of the deflect window.
		
			//And return.
			return;
		};
		
		if(c == null)
			return;
		
		Rigidbody rbody = c.GetComponent<Rigidbody>();
		
		if(!rbody || rbody.isKinematic == true)
			return;
		
		//Force setting part.

		//We get the forward vector of the self.
		Vector3 baseDirection = reflectForce;
		baseDirection += myTransform.forward;
		baseDirection += myTransform.forward;
		
		
		//Scale the force by the mass and a set amount for now.
		baseDirection *= (rbody.mass * DeflectForceFactor);
		
		//Zero out the ball velocity.
		rbody.velocity = Vector3.zero;
		

			
		//Almost last but not least we call dont tax so you dont get the activation fee when you deflect a ball
		DefenseUnitSlot.DontTax();

		//Grab the x magnitude and test
		float xdir = Mathf.Abs(reflectForce.x);
		
			//If we have any direction but holdingback we add force to the ball and play sound.
		//if(reflectForce.z >= 0.0f)
		if(reflectForce.z >= InputBackBuffer)
		{
			
			//Play the sound file.
			if(MeleeSound)
			{
				AudioPlayer.GetPlayer().PlaySound(MeleeSound);
			}
		
			
			//Add the throwing force.
			rbody.AddForce(baseDirection,ForceMode.Force);	

			if(ShieldPushEffect)
			{
				//Apply the effect on the target.
				Quaternion r = Quaternion.LookRotation(baseDirection);
				ShieldPushEffect.transform.rotation = r;	

				ShieldPushEffect.Play();
				
			
			}
				
				
				//Send the message.
				GameObjectTracker.GetGOT().MeleeCannon();
		}
		else{

			//First we check if we have the right input to pick up the cannon.
			//Check if we dont have a cannon equipt then we equipt it!
			//if(xdir < InputBackBuffer)
			if(connectorcontroller.AttachCannon(c))
			{
				AudioPlayer.GetPlayer().PlaySound(PickUpCannonSound);
					
				//Send to the object tracking system.
				GameObjectTracker.GetGOT().CannonPickedUp();
				//DeactivateDefense

				//animation.Stop();
				//animation.Play(PickupCannonAnimation);
				GetComponent<Animation>().Play(PickupCannonAnimation,PlayMode.StopAll);

				//Set the temp guage to view.
				isTempVisible = true;

			}
			
		}
			
				
		
		//If we are performing a move, set can deflect to false to only do the move once per animation.
	//	DefenseUnitSlot.CanDeflect = false;

		
		
		
	}
	
	public void OnMelee(Target t, float speed, bool canShatter)
	{
		//We must be in deflect timing!
		if(!DefenseUnitSlot.CanDeflect)
		{
			//Whatever we want to register when the shield touches a target outside of the deflect window.
		
			//And return.
			return;
		};



		//Lets store the position here in case we need it. quick access
		Vector3 Tposition = t.transform.position;
		Vector3 forward = transform.forward;
		Rigidbody trigid = t.GetComponent<Rigidbody>();
		
		//Force setting part.
		
		//We get the forward vector of the self.
		Vector3 baseDirection = reflectForce;
		baseDirection += forward;
		baseDirection += forward;
		
		
		//Scale the force by the mass and a set amount for now.
		float pf = DefenseUnitSlot.VelocityReflectFactor;

		//If our mass is below the push force, then push at the masses base.
		if(t.GetComponent<Rigidbody>().mass < pf)
			pf = t.GetComponent<Rigidbody>().mass;


		baseDirection *= (pf * DeflectForceFactor);
		
		//If we are pushing things we cant shatter then we wont push them nearly as much.
//		if(!canShatter)
//		baseDirection *= DeflectCCScale;
		
		//Zero out the ball velocity.
		trigid.velocity = Vector3.zero;
		

			
		//Almost last but not least we call dont tax so you dont get the activation fee when you deflect a ball
		DefenseUnitSlot.DontTax();
			
		//Grab the x magnitude and test
		float xdir = Mathf.Abs(reflectForce.z);

		//If we have any direction but holdingback we add force to the ball and play sound.
		//if(reflectForce.z >= 0.0f)
		if(reflectForce.z >= InputBackBuffer)
		{
			
			//Play the sound file.
			if(MeleeSound)
			{
				AudioPlayer.GetPlayer().PlaySound(BlockSound);
				AudioPlayer.GetPlayer().PlaySound(MeleeSound);
			}
		
			
				//Add the throwing force.
				trigid.AddForce(baseDirection,ForceMode.Force);	
			
			//Take away the constraints.
//			if(!canShatter)
//			trigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
//
			if(ShieldPushEffect)
			{
				//Apply the effect on the target.			
				Quaternion r = Quaternion.LookRotation(baseDirection);
				ShieldPushEffect.transform.rotation = r;	

				ShieldPushEffect.Play();
			}
				
				
				//Send the message.
				GameObjectTracker.GetGOT().MeleeHits();
		}
			else{

			//if(xdir < InputBackBuffer)
			if(DefenseUnitSlot.CanDeflect && canShatter)
			{
				//Damage the target.
				t.Damage(MeleeDamage);
				
				//DefenseUnitSlot.ShieldShatterEffect.transform.localPosition = Tposition;
				//DefenseUnitSlot.ShieldShatterEffect.Play();
				ShatterEffect.transform.localPosition = Tposition;
				ShatterEffect.Play();
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(ShatterSound);
					
				DidShatter = true;

				//Send the message.
				GameObjectTracker.GetGOT().MeleeShatter();
				
			
			}
				
				
				
		}

		//If we are performing a move, set can deflect to false to only do the move once per animation.
		//DefenseUnitSlot.CanDeflect = false;

		
	}

	void PushBall(Ball b)
	{

	if(DefenseUnitSlot.CanDeflect)
		{
			//We get the forward vector of the self.
			Vector3 baseDirection = reflectForce;
			baseDirection += myTransform.forward;
			baseDirection += myTransform.forward;
			
			b.GetComponent<Rigidbody>().AddForce(baseDirection * 400.0f, ForceMode.Force);	

		}
	}
	
	public void OnBlockBall(Ball b)
	{
		if(DefenseUnitSlot.CanDeflect)
		{
			//Change the properties of the ball.
			//b.SetBallSourceID(Ball.BallSourceID.Deflect);

			
			//b.SetDamageAmount(DefenseUnitSlot.ReflectDamage);
			b.SetMaterial(DefenseUnitSlot.GetReflectMaterial());
			b.EnableTrail();
			
			//Cap the damage amount
			if(b.DamageAmount < DefenseUnitSlot.ReflectDamage)
				b.SetDamageAmount(DefenseUnitSlot.ReflectDamage);
			
			//Force setting part.	
			
			//We get the forward vector of the self.
			Vector3 baseDirection = reflectForce;
			baseDirection += myTransform.forward;
			baseDirection += myTransform.forward;
			
			Vector3 bposition = b.transform.position;
						
			//Scale the force by the mass and a set amount for now.
			baseDirection *= (b.GetComponent<Rigidbody>().mass * DeflectForceFactor);
			
			//Zero out the ball velocity.
			b.GetComponent<Rigidbody>().velocity = Vector3.zero;
						
			//Almost last but not least we call dont tax so you dont get the activation fee when you deflect a ball
			DefenseUnitSlot.DontTax();

			//Grab the x magnitude and test
			float xdir = Mathf.Abs(reflectForce.x);

		
			//If we have any direction but holdingback we add force to the ball and play sound.
			if(reflectForce.z >= InputBackBuffer)
			{
				
				//Ensure drag is set back to original.
				b.ResetDrag();
				
				//Add the throwing force.
				b.GetComponent<Rigidbody>().AddForce(baseDirection, ForceMode.Force);	
				
				//Set the reset timer to die shortly after.
				b.HotLifeTime = 2.0f;
				b.ResetTimer(0.5f);
				
				if(DeflectEffect)
				{
					//Apply the effect on the target.			
					Quaternion r = Quaternion.LookRotation(baseDirection);
					DeflectEffect.transform.rotation = r;	

					DeflectEffect.Play();
				}
				
				
				if(b.GetBallSourceID() != Ball.BallSourceID.Capture)
				{
					//Set the source ID.
					b.SetBallSourceID(Ball.BallSourceID.Deflect);
				
				}
				
				//Send the message.
				GameObjectTracker.GetGOT().DeflectedBall();
			
				//Play the sound file.
				if(ReflectSound)
				{
					AudioPlayer.GetPlayer().PlaySound(ReflectSound);
				}
			
				//Cool when we deflect.
				Cool(BlockingCoolDownFactor);
				
				
			}
			else{
			
				if(IsCannonAttached())
				{
					//Zero out the velocity here.
					//b.rigidbody.velocity = Vector3.zero;

					b.ResetTimer(DeflectHoldingTime);
					b.SetDrag(10.0f);
					
					//Send the message.
					GameObjectTracker.GetGOT().CaptureBall();
					
					//Set the source ID.
					b.SetBallSourceID(Ball.BallSourceID.Capture);
				
					
					//Play the sound file.
					if(CaptureSound)
					{
						AudioPlayer.GetPlayer().PlaySound(CaptureSound);
					}
					
					//Play particle effect
					if(CaptureBallEffect)
					{
						CaptureBallEffect.transform.localPosition = bposition;
						CaptureBallEffect.Play();
					}
					
					//Cool down the bot when we block.
					Cool(BlockingCoolDownFactor);
					//AddHeat(b.DamageAmount * 0.5f);
		
				
				}
				
				
				
			}
			
			
			
			
		}
		else
		{
			b.Pop();
			
			//Play the sound file.
			if(BlockSound)
			{
				AudioPlayer.GetPlayer().PlaySound(BlockSound);
			}
	
			
			//play the blocking effect here. 
			if(BlockEffect)
				BlockEffect.Play();

			//Cool down the bot when we block.
			//Cool(BlockingCoolDownFactor * 0.32f);
			AddHeat(b.DamageAmount * 0.5f);
			
			GameObjectTracker.GetGOT().BlockedBall();
			
		}


		//If we are performing a move, set can deflect to false to only do the move once per animation.
		//DefenseUnitSlot.CanDeflect = false;
		
		
	}
	
	#endregion
	
	#region Functions
	
	
	/// <summary>
	/// Checks the temperature to see if the temprature is past the maximum capability or not..
	/// </summary>
	/// <returns> 
	/// Returns true of the bot is overheated.
	/// </returns>
	bool CheckTemperature()
	{
		//Cooling over time will process here. This means that to over heat we must surpass the amount of heat after cooling for a time.
		//This will allow us to cool in the same function as we check the temperature.
		CoolOverTime();
		
		
		//Apply the color to the bot based on the heat.
		UpdateColor();
		
		//Here is where we cap the coldest you can get.
		if(Temperature < LowestTemperature)
		{
			Temperature = LowestTemperature;
		}
		
		//If we are on danger
		if(Temperature >= (MaxTemperature) * HeatWarningPercentage)
		{
			if(!WarnedPlayer)
			{
				AudioPlayer.GetPlayer().PlaySound(OverheatDanger);
				WarnedPlayer = true;
				
				//Set the tail length.
				if(OverheatWarningEffect)
					OverheatWarningEffect.Play();

				if(HurtSound)
				{
					AudioPlayer.GetPlayer().PlaySound(HurtSound);
				}
			}
			
		}
		
		//This is to say that if we reach our  low level we need to w arn a brother again
		if(Temperature <= (    MinTemperature +  ( (MaxTemperature - MinTemperature) * 0.25f )   ) )
		{
			WarnedPlayer = false;
		}
		
		if(Temperature >= MaxTemperature)
		{
			//Set the temp to the max.
			Temperature = MaxTemperature;
			
			//We have overheated my friend! 
			OverHeat();
			
			return true;
		}
		
		
		
		
		return false;
	}
	
	public bool Warning
	{
		get {
			if(!IsCannonAttached())
				return true;
			
			return WarnedPlayer;
		}
	}
	
	/// <summary>
	/// Cools the over time. Our little private function for the bot to cool over time.
	/// </summary>
	void CoolOverTime()
	{
		//We check if we are below our coolest rate
		//Cool over time will not be able to cool after a certian temperature is reached
		if(Temperature <= MinTemperature)
		{
			//We do not cool and we just return.
			return;
		}
		
		//Scale the amount to cool by delta time.
		Cool(CoolingRate);
		
	}
	
	/// <summary>
	/// Over heat function that is called when the current temperature exceeeds the maximum temperature capability.
	/// This function will handle all the code for when a bot over heats.
	/// </summary>
	void OverHeat()
	{
		//Set the timestamp for overheating
//		pickuptimestamp = Time.time;
//		
//		//Play the over heated effect.
//		if(OverheatEffect)
//		{
//			OverheatEffect.Play();
//		}
//		
//		
//		//First things first is we detach teh cannon now dont we?
//		connectorcontroller.DetachCannon();
		
		//Send the message
		GameObjectTracker.GetGOT().PlayerOverHeated();

		//Hide the temp guage.
		isTempVisible = false;

		
	}

	public bool VisibleTemp
	{
		get{return isTempVisible;}
	}

	public void TriggerEjectCannon()
	{
		pickuptimestamp = Time.time;
		
		//Play the over heated effect.
		if(OverheatEffect)
		{
			OverheatEffect.Play();
		}
		
		
		//First things first is we detach teh cannon now dont we?
		connectorcontroller.DetachCannon();
	}
	
	public void SetDefenseUnit(Shield unit)
	{
		//Set the defense unit.
		DefenseUnitSlot = unit;
		
		DefenseUnitSlot.SetHost(this);
	}
	
		
	public void SetTemperature(float deg)
	{
		//Do the basic setting.
		Temperature = deg;
	}
	
	public void Damage(float deg)
	{
		if (DefenseUnitSlot==null){
			Debug.LogWarning("No DefensiveSlot in BOT Damage");
			return;
		}
		if(!DefenseUnitSlot.IsActivated())
		{
			AddHeat(deg);

			if(HurtSound)
			{
				AudioPlayer.GetPlayer().PlaySound(HurtSound);
			}

			if(DamageEffect)
			DamageEffect.Play();

		}

		//Register with GOT.
		GameObjectTracker.GetGOT().PlayerHit(deg);

	}
	
	
	public void TriggerOverheat()
	{
		AddHeat(MaxTemperature);
	}
	
	public void AddHeat(float deg)
	{
		//We can only add heat if there is a cannon attached!
		if(!connectorcontroller.GetCannon())
		{
			//print("There is no attached cannon and you are trying to add heat!");
			return;
		}
		
		//Time the heat being added.
		Temperature += (deg) ;
	}
	
	public void Cool(float deg)
	{
		if(!connectorcontroller.GetCannon())
		{
			//Specific cooling if there is no cannon attached. 
			//return;
		}
		
		Temperature -= (deg) ;
	}
	

	
	void UpdateColor()
	{
		
		//Create a clamped value of the health to color ratio.
		//float health2color = (Temperature /MaxTemperature);
		float health2color = ( (Temperature ) / (MaxTemperature) );
		

		Color c =  Color.Lerp(Color.white,Color.red,health2color);
		
		//Set the color.
		gameObject.GetComponent<Renderer>().material.color = c;
		
		//light
		if(GetComponent<Light>())
		{
			GetComponent<Light>().color = c;
		}
	
		
	}
	
	
	public bool IsCannonAttached()
	{
		if(connectorcontroller)
		{
			return connectorcontroller.GetCannon();
		}
		
		return false;
		
	}
	
	#endregion
	
	
	/// <summary>
	/// This funtion does the core checks for objects that all base bots shoudl check against.
	/// Things like always searching for cannons and picking them up if there is a free attachment slot and etc.
	/// </summary>
	void OnCollisionEnter(Collision col)
	{
				


		//Dont pull targets
		if(col.gameObject.CompareTag("CommandCenter") )
		{
			//This is test against shield.
			//Dont process this if shield isnt on.
			//Only melee CC with shield.
			if(!IsShieldActive())
				return;
			//Grab the target.
			Target t = col.gameObject.GetComponent<Target>();

			OnMelee(t,18.0f,t.CanShatter);
		}
		
		
		
		
		if(col.gameObject.CompareTag("Target"))
		{

			//This is test against shield.
			//Dont process this if shield isnt on.
			//Only melee CC with shield.
			if(!IsShieldActive())
				return;

			Target t = col.gameObject.GetComponent<Target>();
			
			if(!t.IsDestroyed())
				OnMelee(t,50.0f,t.CanShatter);
		}



		//Check if we collide with a cannon.
		if(col.gameObject.CompareTag("Cannon") )
		{
			Cannon c = col.gameObject.GetComponent<Cannon>();

			//check if we are holding shield.
			bool blocking = IsShieldActive();


			if(!IsOverHeated)
			{
				
				//Call attach for the cannon that we collide with.
				//Check first if we arent blocking then process the picking up of cannon code.
				if(!blocking)
				if(connectorcontroller.AttachCannon(c) )
				{	
						AudioPlayer.GetPlayer().PlaySound(PickUpCannonSound);
					
						//Send to the object tracking system.
						GameObjectTracker.GetGOT().CannonPickedUp();

					GetComponent<Animation>().Play(PickupCannonAnimation,PlayMode.StopAll);


						// when we attach we set the temperature, not just on collide
						SetTemperature(MinTemperature);

					//Set the temp guage to view.
					isTempVisible = true;
					
				}
				
			}

			//Check for melee.
			if(blocking && DefenseUnitSlot.CanDeflect)
				OnMeleeCannon(c,45.0f);
		}
		
		//Check if we collide with balls.
		if(col.gameObject.CompareTag("Ball") )
		{
			Ball b = col.gameObject.GetComponent<Ball>();


			if(IsShieldActive())
			{
				if(b.ID != Ball.BallSourceID.Netural && b.ID != Ball.BallSourceID.Hot && b.IsActive() )
				{
					OnBlockBall(b);
					return;
				}

//				if(!b.IsActive())
//				{
//					PushBall(b);
//					return;
//
//				}
			}

			//We have to make sure we have a cannon attached to pick up a ball and we make sure sheild is not on.
			if(connectorcontroller.GetCannon())
			{
				
				if(b.GetBallSourceID() == Ball.BallSourceID.Enemy && !DefenseUnitSlot.IsActivated())
				{
//					AddHeat(b.DamageAmount);
////					print("Bot HIT!");
//					

					Damage(b.DamageAmount);

				}
				
				//Call attach for the cannon that we collide with.
				if(!IsShieldActive())
				if(connectorcontroller.GetCannon().PickupBall(b) )
				{
				
					//Play the audio file.
					AudioPlayer.GetPlayer().PlaySound(PickUpAmmo);
					
					//Set teh ball source.
					b.SetBallSourceID(Ball.BallSourceID.Bot);
					
					//Play ball pick up effect.
					if(BallCollection)
					{
						BallCollection.transform.position = b.transform.position;
						BallCollection.Play();
					}

					if (!isAnimatingCannonPickup){
						GetComponent<Animation>().Play(PickupBallAnimation);
					}
				}
				
				if(connectorcontroller.IsCannonFull())
				{
					AudioPlayer.GetPlayer().PlaySound(CannonFilled);
				}

				
			}
		}
		
		if(col.gameObject.CompareTag("Money") )
		{
		
			Money m = col.gameObject.GetComponent<Money>();
			
			//We pick up money.
			//Play the effect for picking up a ball.
			if(Collection && GameObjectTracker.instance.multiplierCount > 0)
			{
				//Play the collection animation.
				Collection.Play();
				AudioPlayer.GetPlayer().PlaySound(CollectSound);
			}
			
			
			//Do money adding logic here.
			GameObjectTracker.GetGOT().AddMoney(m);
			
		}

		if(col.gameObject.CompareTag("Wall") )
		{
			//WallCollideParticleEffect.gameObject.SetActive(true);
			WallCollideParticleEffect.Play();
			foreach (ContactPoint point in col.contacts){
				WallCollideParticleEffect.transform.position = point.point;
				WallCollideParticleEffect.transform.forward = point.normal;
				//Debug.DrawRay(point.point, point.normal);
			}
			//			WallCollideParticleEffect.transform.forward = col.transform.forward;
			//			WallCollideParticleEffect.transform.position = col.transform.position;
		}

		
	}

	/// <summary>
	// when the objects exits the collision
	/// </summary>
//	void OnCollisionExit(Collision col)
//	{
//		if(col.gameObject.CompareTag("Wall") )
//		{
//		//	WallCollideParticleEffect.Stop();
//			//WallCollideParticleEffect.Clear();
//			//WallCollideParticleEffect.gameObject.SetActive(false);
//			//			WallCollideParticleEffect.transform.position = ToyBox.GetPandora().Bot_01.transform.position;
//		}
//	}
//	
	public void StartAnimatingCannonPickup(){
		isAnimatingCannonPickup = true;
	}

	public void FinishAnimatingCannonPickup(){
		isAnimatingCannonPickup = false;
	}

	protected void BaseBotUpdate()
	{
		
		if (DefenseUnitSlot==null){
			Debug.LogWarning("No DefensiveSlot in BOT BaseBotUpdate");
		}
		else if(DefenseUnitSlot.IsActivated())
		{
			AddHeat(DefenseUnitSlot.BlockingHeat);	
			
			if(DefenseUnitSlot.CanDeflect)
			{
				moveForce.z = 0.0f;
			}
				
		}

		//Handle heat checking
		IsOverHeated = CheckTemperature();
		
		
//		//Check for dasking
//		if(deadInputOnShield && DefenseUnitSlot.CanDeflect() && moveForce != Vector3.zero)
//		{
//			
//			//deadInputOnShield = false;
//			
//			
//			Dash();
//			
//			//Tell the object tracker we dashed.
//			GameObjectTracker.GetGOT().Dash();
//			
//			return;
//		}

//Do the dashing force here.

		if(connectorcontroller.HasCannon())
		{
			//Apply the force to the bot.
			 Rdbdy.AddForce(moveForce * ForcePush,ForceMode.Force);	
			
		}
		else{
			//Apply the force to the bot.
			Rdbdy.AddForce(moveForce * BasicForcePush,ForceMode.Force);	
		}
		
		

		//UPDATE your captued material animation.
		//materialOffset.y += 0.04f;
		//DefenseUnitSlot.GetReflectMaterial().SetTextureOffset("_MainTex",materialOffset);

		AnimateMovement(moveForce);
		
		
			
	}
	
	
	// Update is called once per frame
	protected void Update () {	
	
		//At the end of the update we update the local value of current temp.
		currentTemp = (int)Temperature;

		
	}

	void AnimateMovement(Vector3 movementDir){
		
		// we dont want to animate the movement if we're animating the cannon pick up
		if (IsAnimatingCannonPickup){
			
			return;
		}
		
		bool moving = false;
		
		// do moving animations forward back left right
		if (movementDir.x > 0){
			GetComponent<Animation>().CrossFade("MovingRight");
			
			moving = true;				
			
		}
		
		if (movementDir.x < 0){
			//Bot_01.animation.Play("MovingLeft");
			GetComponent<Animation>().CrossFade("MovingLeft");
			moving = true;
			
		}
		
		if (movementDir.x == 0 && movementDir.z < 0){
			//Bot_01.animation.Blend("MovingBackwards");
			GetComponent<Animation>().CrossFade("MovingBackwards");
			moving = true;
		}
		if (movementDir.x == 0 && movementDir.z > 0){
			//Bot_01.animation.Blend("MovingForward");
			GetComponent<Animation>().CrossFade("MovingForward");
			
			moving = true;
		}

		if(IsShieldActive())
			GetComponent<Animation>().CrossFade("Attack");


				if (!moving){
					GetComponent<Animation>().CrossFade("Idle");
				}
		
	}


	
	protected void Initialize()
	{
		//Create the collection particle system.
		Collection = Instantiate(Collection) as ParticleSystem;
		
		//Cache values
		myTransform = transform;
		Rdbdy = GetComponent<Rigidbody>();
		
		//Set things in place for the first time.
		connectorcontroller = GetComponent<CannonConnector>();
		connectorcontroller.SetHost(gameObject);

		// set the reference to the cannon position
		CannonConnectorPos connectorPos = this.gameObject.GetComponentInChildren<CannonConnectorPos>();
		if (connectorPos){
			this.cannonConnectorPos = connectorPos;
		}

		//Get the trail renderer.
		trail = GetComponent<TrailRenderer>();
		//trail.enabled = false;
		
		//Store the original drag 
		originalDrag = StartingDrag;
		
		
		
		//Update the position of the shield.
		if(DefenseUnitSlot)
		{
			DefenseUnitSlot.transform.position = transform.position;
			DefenseUnitSlot.transform.parent = transform;
		}
		
		if(Collection)
		{
			Collection.transform.position = transform.position;
			Collection.transform.parent = transform;
		}
		if(BallCollection)
		{
			BallCollection = Instantiate(BallCollection) as ParticleSystem;
		}
		if(OverheatEffect)
		{
			OverheatEffect = Instantiate(OverheatEffect) as ParticleSystem;
			
			//OverheatEffect.transform.localPosition = Vector3.zero;
			OverheatEffect.transform.position = transform.position;
			OverheatEffect.transform.parent = transform;
		}
		
		if(CaptureBallEffect)
		{
			CaptureBallEffect = Instantiate(CaptureBallEffect) as ParticleSystem;
			CaptureBallEffect.transform.localPosition = Vector3.zero;
		}
		
		if(BlockEffect)
		{
			BlockEffect = Instantiate(BlockEffect) as ParticleSystem;
			BlockEffect.transform.position = transform.position;
			BlockEffect.transform.parent = transform;	
		}
		
		
		if(ShieldPushEffect)
		{
			
			ShieldPushEffect = Instantiate(ShieldPushEffect) as ParticleSystem;
			ShieldPushEffect.transform.parent = transform;
			ShieldPushEffect.transform.localPosition = Vector3.zero;
			
		}

		if(DeflectEffect)
		{
			DeflectEffect = Instantiate(DeflectEffect) as ParticleSystem;
			DeflectEffect.transform.parent = transform;
			DeflectEffect.transform.localPosition = Vector3.zero;
		}

		if(EmptyShatterEffect)
		{
			EmptyShatterEffect = Instantiate(EmptyShatterEffect) as ParticleSystem;
			EmptyShatterEffect.transform.parent = transform;
			EmptyShatterEffect.transform.localPosition = Vector3.zero;

		}

		if(OverheatWarningEffect)
		{
			OverheatWarningEffect = Instantiate(OverheatWarningEffect) as ParticleSystem;
			OverheatWarningEffect.transform.parent = transform;
			OverheatWarningEffect.transform.localPosition = Vector3.zero;
			
		}

		if(ShatterEffect)
		{
			ShatterEffect = Instantiate(ShatterEffect) as ParticleSystem;
			ShatterEffect.transform.localPosition = Vector3.zero;

		}


		if (WallCollideParticleEffectPreb){
			WallCollideParticleEffect = Instantiate(WallCollideParticleEffectPreb) as ParticleSystem;
			WallCollideParticleEffect.transform.parent = transform;
			WallCollideParticleEffect.transform.localPosition = Vector3.zero;
			WallCollideParticleEffect.transform.rotation = transform.rotation;
			WallCollideParticleEffect.transform.localRotation = Quaternion.identity;
		//	WallCollideParticleEffect.gameObject.SetActive(false);
			
		}

		if(DamageEffect)
		{
			DamageEffect = Instantiate(DamageEffect) as ParticleSystem;
			
			DamageEffect.transform.parent = transform;
			DamageEffect.transform.localPosition = Vector3.zero;
			
		}

	}
	
	public int CurrentTemp
	{
		get{
			return currentTemp;
		}
	}


	public float HeatPercent
	{
		get
		{
			float range = MaxTemperature - LowestTemperature;
			return Temperature/range;
		}
	}
	
	public int GetBallsInStorage()
	{
		if(connectorcontroller)
		{
			return connectorcontroller.GetBallsInStorage();
		}
		
		return 0;
	}
	
	public int GetMaxBallCapacity()
	{
		if(connectorcontroller)
		{
			return connectorcontroller.GetCannon().MaxBallCount;
		}
		
		return 0;
	}
	
	public bool IsCannonFull()
	{
		if(connectorcontroller)
		{
			return connectorcontroller.IsCannonFull();
		}
		
		return false;
	}
	
	
	
	public bool IsShieldActive()
	{
		if (DefenseUnitSlot==null){
			Debug.LogWarning("No DefensiveSlot in BOT IsShieldActive");
			return false;
		}

		return DefenseUnitSlot.Active;
	}
	

}
