using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Target : MonoBehaviour {
	
	//Very target is bound to have a life.
	public float Health = 1000.0f;
	public float MaxRegenHealth = 500.0f;
	public float MaxHealth = 1000.0f;
	public bool CountForCombo = true;
	
	//This is the interesting part, giving the target a number of lives.
	public int Points = 5;
	
	public float ExplosionForceFactor = 20.0f;
	public bool Interpolate = true;
	public bool AffectedByShieldGravity = false;
	
	//Health Regeneration stuff.
	public bool RegenHealth = true;
	public float HealthRegenrationRate = 50.0f;
	public float AutoKillTimer = 3.0f;
	public float CleanUpTime = 1.0f;
	public float IdleExponent = 4.0f;
	
	
	public bool MomentumDamage = false;
	public bool CanShatter = true;
	public float TargetMomentumDamage = 500.0f;
	public float SelfMomentumDamage = 200.0f;

	
	//Effects
	public ParticleSystem Explosion;
	public ParticleSystem StandardHit;
	public ParticleSystem HealEffect;
	
	//Sound Effects
	public List<AudioClip> DestructionSoundEffects;
	
	public AudioClip HitSoundEffect;
	public AudioClip HurtSoundEffect;
	public AudioClip HealSoundEffect;

	//Local bool to keep track if the target is destroyed.
	bool destroyed = false;
	//bool disabled = true;
	bool captured = false;
	
	//Local value for crown position.
	Vector3 _crownPosition = Vector3.zero;
	
	//Timers for stuff to clean up after being destroyed.
	float resettimer;
	float originalHealth;
	
	protected Transform uiHandler;
	
	protected Transform myTransform;
	
	
	//Physics Parts
	public List<BodyPart> PhysicsParts;
	//public Transform PhysicsPartsTransform = null;
	
	//Id to vieww.
	public EntityFactory.TargetTypes ID;
	
	public enum HitTypes
	{
		none,
		deflect,
		capture,
		enemy,
		bot,
		target,
		wall,
		count
	}
	
	HitTypes _lastHit = HitTypes.none;
	
	
	
	[System.Serializable]
	public class BodyPart
	{
		//Store the original rotations and positions/
		Vector3 originalPosition;
		Quaternion originalRotation;
		
		//Hold on to the game object.
		GameObject physicsPart;
		Rigidbody rbdy;
		Collider col;
		Transform myTransform;
		
		public BodyPart(Transform t)
		{
			originalPosition = t.localPosition;
			originalRotation = t.rotation;
			
			myTransform = t;
			rbdy = t.GetComponent<Rigidbody>();
			col = t.GetComponent<Collider>();
			physicsPart = t.gameObject;
		}
		
		public void Reset()
		{
			rbdy.isKinematic = true;
			rbdy.interpolation = RigidbodyInterpolation.None;
			col.isTrigger = true;
			
			myTransform.localPosition = originalPosition;
			myTransform.localRotation = originalRotation;
			
		}
		
		public void Explode(float ExplosionForceFactor, bool Interpolate = true)
		{
			//Blow up
			rbdy.isKinematic = false;
			col.isTrigger = false;
			
			//Add the explosion force.
			Vector3 forcefactor = Vector3.zero;
			forcefactor.Set(ExplosionForceFactor,ExplosionForceFactor,ExplosionForceFactor);
		

			//Create vector to throw to at random direction.
			Vector3 v = Vector3.zero;
			v = Random.insideUnitSphere;
			v.y += 1.0f;

			//Scale the force factor by the random vector to throw the part.
			forcefactor.Scale(v);

			//Scale by the mass of the rigid body... 	
			forcefactor *= (physicsPart.GetComponent<Rigidbody>().mass);

		
		if(Interpolate)
			rbdy.interpolation = RigidbodyInterpolation.Interpolate;

		
			//Add the force.
			rbdy.AddForce( forcefactor, ForceMode.Impulse);
			rbdy.AddTorque( forcefactor * Random.Range(1.0f, 7.0f),ForceMode.Impulse);
		}
	}
	
	
	// Use this for initialization
	protected virtual void Start () {
		
		if(Explosion)
		{
			//Create an explosion to use upon death.
			Explosion = Instantiate(Explosion) as ParticleSystem;
			
			//Explosion.transform.position = Vector3.zero;	
			Explosion.transform.parent = transform;
			Explosion.transform.localPosition = Vector3.zero;
		}
		
		//Create the standard hit if we have one
		if(StandardHit)
		{
			StandardHit = Instantiate(StandardHit) as ParticleSystem;
		
			StandardHit.transform.parent = transform;
			//StandardHit.transform.position = Vector3.zero;
			StandardHit.transform.localPosition = Vector3.zero;
			
		}
		
		if(HealEffect)
		{
			HealEffect = Instantiate(HealEffect) as  ParticleSystem;
			
			HealEffect.transform.parent = transform;
			HealEffect.transform.position = Vector3.zero;
			HealEffect.transform.localPosition = Vector3.zero;
		}
		
		//Search for UIHandler in children.
		uiHandler = transform.Find("UIHandle");
		
		//Get crown position
		_crownPosition = GetComponent<Collider>().bounds.extents;
		
		//Store the original health.
		originalHealth = Health;
		
		myTransform = transform;
		
//		//Add the physcis parts to the list.
//		foreach(Transform t in PhysicsPartsTransform.transform)
//		{
//			if(t.CompareTag("PhysicsPart"))
//			{
//				//Create and add all body parts.
//				BodyPart bp = new BodyPart(t);
//				PhysicsParts.Add(bp);
//				
//			}
//		}

		// some targets will have thier physics parts unders a unique transform
		// this should go into and find all grand children physics parts
		Transform [] allChildren = this.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren){
				if(child.CompareTag("PhysicsPart"))
				{
					//Create and add all body parts.
					BodyPart bp = new BodyPart(child);
					PhysicsParts.Add(bp);
					
				}
		}
	
	}

	public void Update()
	{
//		Debug.LogError("Target Update   " + name + "   **");

		Regenerate();
//		UpdateColor();
		
		//Update animation speed
		// commented out on 5/31/14
//		if(animation)
//		foreach(AnimationState state in animation)
//		{
//			state.speed = (1.0f + IdleExponent) - (GetHealthPercentage() * IdleExponent);
//		}
		
		
		//Check if we are destroyed to start the timer to reset.
		if(destroyed)
		{
//			Debug.LogError("Target destroyed should reset, issue with timer   " + name + "   **");
			// ** Frank Add 12/28/13
			if (HealEffect)
				HealEffect.Stop();
			// ** Frank END

			resettimer += Time.deltaTime;
			
			if(resettimer >= AutoKillTimer && AutoKillTimer != -1.0f)
			{
				Disable();
				
			}
		}
		
		
	}
	
	void OnEnable()
	{
		if(GetComponent<Animation>())
			GetComponent<Animation>().Play();
	}
	
	public void ForceDisable()
	{
		Disable();
	}
	
	void Disable()
	{
//		Debug.LogError("Target Disable()   " + name + "   ");
		//Get rid of particle
		//Destroy(Explosion.gameObject);
				
		//Destroy self.
		//Destroy(gameObject);
	
		//Deactivate.
		gameObject.SetActive(false);


		//Reset self.
		Reset();
		
		//Return to the entity factory.
		EntityFactory.GetEF().ReturnTarget(this);
		
		
	}
	
	protected HitTypes LastHitBy
	{
		set{
			
			_lastHit = value;
		}
		
		get{
			return _lastHit;
		}
	}
	
	protected virtual void Reset()
	{		
		Debug.Log("Target Reset   " + name + "  ** ");
		//Restore the base object 
		GetComponent<Collider>().isTrigger = false;
		GetComponent<Rigidbody>().isKinematic = false;

		//Null the velocity of rigid body.
		GetComponent<Rigidbody>().velocity = Vector3.zero;

		
		
		//Fix back the rotation lock.
		//rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		
		//Reset all body parts.
		foreach(BodyPart bp in PhysicsParts)
		{
			bp.Reset();
		}
		
		
		
		//Set destroyed to false.
		destroyed = false;
		
		//Restore health
		Health = originalHealth;
		
	}
	
	
	void KillMe()
	{
		//If we are already destroyed no need to do this again.
		if(destroyed)
			return;
		
		//Disable the parent and main collider.
		GetComponent<Collider>().isTrigger = true;
		GetComponent<Rigidbody>().isKinematic = true;
	
		if(Explosion)
		{
			//Set the position of the explosion and play it.
			//Explosion.transform.position = transform.position;
			Explosion.Play();
		}
		
		if(GetComponent<Animation>())
			GetComponent<Animation>().Stop();

		
		//Play sound file.
		
		//Get the index as a random number between the list of destruction effects.
		int clipindex = Random.Range(0,DestructionSoundEffects.Count);
		
		//Set the audio clip and play
		AudioPlayer.GetPlayer().PlaySound(DestructionSoundEffects[clipindex]);
		
		foreach(BodyPart bp in PhysicsParts)
		{
			bp.Explode(ExplosionForceFactor);
		}
				
		//Set the destroy flag
		destroyed = true;	
		
		//Stamp the timer.
		resettimer = 0.0f;
		
		
		//Switch statement to send off message on what was killed by.
		switch(LastHitBy)
		{
		case HitTypes.none:
		{
			break;
		}
		case HitTypes.capture:
		{
			GameObjectTracker.instance.CaptureKills();
			break;
		}
		
		case HitTypes.deflect:
		{
			GameObjectTracker.instance.DeflectKills();
			break;
		}
		case HitTypes.enemy:
		{
			GameObjectTracker.instance.PushKills();
			break;
		}
			
		case HitTypes.target:
		{
			GameObjectTracker.instance.PushKills();
			break;
		}
			
		case HitTypes.wall:
		{
			GameObjectTracker.instance.WallKill();
			break;
		}
		
		
			
			
		}
		
		//Send Message
		//gameObject.SendMessageUpwards(DestroyMessage,this,SendMessageOptions.DontRequireReceiver);
		GameObjectTracker.GetGOT().TargetDestroyed(this,CountForCombo);
		
		
		
	}
	
	
	void UpdateColor()
	{
		Color c = Color.white;
		
		//Create a clamped value of the health to color ratio.
		float health2color = (Health/MaxHealth);
		
		c.b = health2color;
		c.g = health2color;
		
		//We look for a peice named body.
		Transform t = transform.Find("Body");
		
		
		//Set the color.
		if(t)
		{
			//t.renderer.material.color = c;
			//t.renderer.materials[1].color = c;
		}
		
	}
	
	
	#region Modifiers / Access
	

	
	public float GetHealthPercentage()
	{
		if(RegenHealth)
		{
			if(Health > MaxRegenHealth)
				return Health/(MaxHealth);
			
			//Give the current health based on the max regen health.
			return Health/ (MaxHealth);
		}
		
		//Else we return the percent of health to the max health 
		return Health / (MaxHealth);
		
	}
	
	public Vector3 GetUIHandlePosition()
	{
		if(uiHandler)
			return uiHandler.position;
		
		return transform.position;
	}
	
	public Vector3 CrownPosition
	{
		get{
			
			Vector3 pos = myTransform.position;
			pos.y += _crownPosition.y;
			
			return pos;
		
		}
		
	}
	
	public int GetPointWorth()
	{
		return Points;
	}
	
	public void Damage(float amount, HitTypes type = HitTypes.none)
	{
		//Dont process anything if we are already destroyed!
		if(destroyed || captured)
			return;
		
		//Set the last hit type.
		LastHitBy = type;
		
		//Subtract the health.
		Health -= amount;
		
		//Checking if the damage is a healing damage. 
		if(amount > 0) 
		{
			//here we take care of hurting. as the damage is greater than 0.
			
			if(HurtSoundEffect)
				AudioPlayer.GetPlayer().PlaySound(HurtSoundEffect);
			
			//Play the particle effect.
			if(StandardHit)
			{
				//StandardHit.transform.position = transform.position;
				StandardHit.Play();
			}
			
		}
		else
		{
			//If there are any healting effects to apply.
			
			if(HealSoundEffect)
				AudioPlayer.GetPlayer().PlaySound(HealSoundEffect);			
			
			if(HealEffect)
				HealEffect.Play();
			
		}
		
		

		
		//Check to see if the target has been destroyed
		if(Health <= 0)
		{
			KillMe();
		}
			
	}
	
	void Regenerate()
	{
		if(UnableToRegenerate())
		{
			return;
		}
		
		
		//Update the health
		Health += HealthRegenrationRate * Time.deltaTime;

		// *** Franks Add
		if(HealEffect){
			if (!HealEffect.isPlaying){
			 	HealEffect.Play();
			}

		}
		// ** FRANK END
		
	}
	
	/// <summary>
	/// Unables to regenerate.
	/// </summary>
	/// <returns>
	/// Returns true if we should not renegerate. False if regeneration should take place.
	/// </returns>
	bool UnableToRegenerate()
	{
		
		//Stop regenerating after full health has been reached.
		if(Health >= MaxHealth )
		{
			Health = MaxHealth;
			return true;
		}
		
		if(Health > MaxRegenHealth)
		{
			return true;
		}
		
		if(!RegenHealth)
		{
			return true;
		}
		
		
		return false;
		
	}
	
	
	public bool IsDestroyed()
	{		
//		if(!gameObject.activeSelf)
//			return true;
		
		return destroyed;
	}
	public bool Destroyed
	{
		get {return destroyed;}
	}
		
	public Vector3 Position
	{
		// Crashed here, adding check
		get{
			if (myTransform){
				return myTransform.position;
			}
			else return Vector3.zero;
		}
	}
	
	public bool Captured
	{
		get{return captured;}
		set{captured = value;}
	}
	

		
	#endregion 
	
	
	#region CollisionChecks
	
	
	
	#endregion
	
}
