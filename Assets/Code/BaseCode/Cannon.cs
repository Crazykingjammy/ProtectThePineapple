using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour {
	
	//List for picking up balls.
	public List<Ball> BallInventory;
	
	//Information variables
	public string CannonName = "Cannon";
	public string CannonDesciption = "Defaul Cannon Description. The developers was too lazy to replace this text.";
	public string CannonTextureName = "CannonTexture";
	
	//Stuff for Balls
	public int MaxBallCount = 5;
	public float BallMass = 1.0f;
	public float DamageAmount = 100.0f;
	public float CannonPushBackFactor = 1.0f;
		
	//Variables for the shooting
	public float ForceMagnitude = 150.0f;
	public float ShootingRate = 1.0f;
	public float FiringEnergyHeat = 25.0f;
	public float BallScale = 0.3f;
	public bool ShootThrough = false;
	public float ActiveTime = 2.0f;
	
	
	public float MaxIdleTime = 5;
	
	public Material BallMaterial;	
	
	public bool TurnOffGravityToShoot = true;
	public bool KeepMeArround = false;
	public float BallDrag = 0.0f;
	public bool GravityControl = false;
	public bool InUse = false;
	
	public bool MassPushFactor = false;
	public float MassPushVelocity = 40.0f;
	
	
	//Effect for shooting.
	public ParticleSystem ShootingEffect;
	public ParticleSystem ExplodeEffect;
	public ParticleSystem BurningEffect;
	
	//Sound Effect
	public AudioClip ShootSound;
	public AudioClip EmptyShotSound;
	public AudioClip OverHeatExplosionSound;
	public AudioClip ChickenSound;
	public AudioClip ExplosionSound;
	
	
	public float ExplosionHeat = 500.0f;
	public float RecoveryRate = 10.0f;
	public float ExplosionForce = 20.0f;
	public float HeatCap = 2000.0f;
	public float TrailMagnitude = 7.0f;


	Rigidbody myRbody = null;
	TrailRenderer myTrail = null;
	
	
	//Private and hidden variables for inner calculations.
	
	float timer;
	Transform mouth;
	bool dirty = false;
	
	float heat = 0.0f;
	
	
	Transform myTransform;
	GameObject capsule;
	
	//Holy what ype of cannon i am.
	EntityFactory.CannonTypes myType  = EntityFactory.CannonTypes.NULL;
	
	float idleTimer = 0.0f;
	
	
	public float Heat
	{
		get{
			return heat;
		}
		
		set
		{	
			heat = value;
					
			//Limit the heat.
			if(heat > HeatCap)
				heat = HeatCap;
		}
	}
	
	public bool IsBurning
	{
		get
		{	
			//Never burn when we are in in use;
			if(InUse)
				return false;
			
			
			return (Heat >= ExplosionHeat);
		}
	}
	
	public void Explode()
	{
		//Random check here just incase the explode function is callled whhile in use?
		if(InUse)
			return;

		//Reset the idle timer.
		idleTimer = 0.0f;

		//Play sound, particle effect, and add force here.
		AudioPlayer.GetPlayer().PlaySound(ExplosionSound);
		
		if(ExplodeEffect)
			ExplodeEffect.Play();
		
		//Set the heat since we exploded!
		Heat = 1200.0f;
		
		//Create the explosion force.
		//Base it on random and limit the Y
		Vector3 p = Random.onUnitSphere;
		p.y = 0.3f;
		
		p *= (ExplosionForce * GetComponent<Rigidbody>().mass);
		
		GetComponent<Rigidbody>().AddForce(p,ForceMode.Impulse);
		
		 
		
	}
		
	
	// Use this for initialization
	void Start () {
		
		if(!BallMaterial)
		{
			BallMaterial = GetComponent<Renderer>().sharedMaterial;
		}
		
		if(!mouth)
		{
			mouth = transform.Find("Mouth").transform;
		}
		
		//Create the shooting effect
		if(ShootingEffect)
		{
			ShootingEffect = Instantiate(ShootingEffect) as ParticleSystem;

			ShootingEffect.transform.parent = transform;

		}
		
		if(BurningEffect)
		{
			BurningEffect = Instantiate(BurningEffect) as ParticleSystem;
			
			//Adjust the burning emmision size.
			//BurningEffect.
			
			BurningEffect.transform.parent = transform;
			BurningEffect.transform.localPosition = Vector3.zero;
			
		}
		
		if(ExplodeEffect)
		{
			ExplodeEffect = Instantiate(ExplodeEffect) as ParticleSystem;
			ExplodeEffect.transform.parent = transform;
			ExplodeEffect.transform.localPosition = Vector3.zero;
			
		}


		//Grab the locals.
		myTransform = transform;
		myRbody = GetComponent<Rigidbody>();
		myTrail = gameObject.GetComponent<TrailRenderer>();

		capsule = transform.Find("Capsule").gameObject;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		
//		//Keep track of the idle timer if we are not in use.
//		if(!InUse)
//		{
//			idleTimer += Time.deltaTime;
//		}
//		else
//		{
//			idleTimer = 0.0f;
//		}
//		
//		
//		//Destroy outselves if we are idle for too long.
//		if(idleTimer > MaxIdleTime && dirty)
//		{
//			ReturnToEF();
//			
//		}
//		
		//Always cool down if we above 0.
		if(Heat > 1.0f)
			Heat -= RecoveryRate;
		
		//If we are higher then 1 then we go throgh colling down process.
		if(IsBurning)
		{
			idleTimer = 0.0f;
			
			//If the burning effect is deactivated then we activate it.
			if(BurningEffect)
			if(BurningEffect.isStopped)
			{
				BurningEffect.Play();
				//Play chicken sound if we explode and we not already burning.
				//AudioPlayer.GetPlayer().PlaySound(ChickenSound);
			}
		}
		else
		{
			if(BurningEffect)
			if(BurningEffect.isPlaying)
				BurningEffect.Stop();
		}



		//Turn on the trail render of we are moving beyond a speed.
		if(myTrail)
		{

			myTrail.enabled = (myRbody.velocity.magnitude > TrailMagnitude);

//			if(myRbody.velocity.magnitude > TrailMagnitude)
//				myTrail.enabled = true;
//			else
//				myTrail.enabled = false;
		}



	}
	

	
	/// <summary>
	/// Releases the balls.
	/// </summary>
	/// <returns>
	/// Returns the count of number balls that were in the stuck upon the release call.
	/// </returns>
	public int ReleaseBalls()
	{
		if(!mouth)
		{
			mouth= transform.Find("Mouth").transform;;
		}
		
		
		foreach (Ball b in BallInventory)
		{
			//Activate, Set Position, Add Force.
			b.Activate();
			b.SetBallSourceID(Ball.BallSourceID.Hot);
			b.SetPosition(myTransform.position);
			b.transform.LookAt(Vector3.forward);
			
			//Set the material
			b.SetMaterial(BallMaterial);
			
			Vector3 explodeforce = Vector3.zero;
		
			explodeforce = Random.onUnitSphere;
			
		//	explodeforce.Set(Random.Range(0.0f,0.0f),Random.Range(0.0f,0.0f),Random.Range(0.0f,100.0f));
			
			
			
			//Also call freeball to allow it back into the scene
			b.Fire(explodeforce);
		}
	
		//Store the count.
		int c = BallInventory.Count;
		
		//Play overheat sound
		AudioPlayer.GetPlayer().PlaySound(OverHeatExplosionSound);
		
		//Clear the list.
		BallInventory.Clear();
	
	
		return c;
		
		
		
	}
	
	public bool PickupBall(Ball b)
	{
		if(BallInventory.Count >= MaxBallCount)
		{
			//We are full and need not pick up any more balls
			return false;
		}
		
		//We cant pick up active balls!
		if(b.IsActive() )
		{
			return false;
		}
		
		
		//Disable the ball and take it away!
		b.DeActivate();
		
		//We have to do some checking... to make sure we dont pick up the same balls twice. 
		BallInventory.Add(b);
	
		
		return true;
	}
	public bool IsFull()
	{
		if(BallInventory.Count >= MaxBallCount)
		{
			//Return true to state we are full of balls.
			return true;
		}
		
		//and false for no.
		return false;
	}
	
	public bool IsDirty()
	{
		return dirty;
	}
	
	public void SetDirty(bool d)
	{
		dirty = d;
		idleTimer = 0.0f;
	}
	
	public float GetPushBackFactor()
	{
		return BallMass * CannonPushBackFactor;
	}
	
	public virtual float Shoot()
	{
		//Dont have balls to shoot. dont shoot any!
		if(UnAbleToFire())
		{
			return 0.0f;
		}
		
		if(!mouth)
		{
			mouth= transform.Find("Mouth").transform;;
		}
		
		//Handle the shooting here.
		Vector3 mouthposition = mouth.position;
		//Vector3 mouthOffset = Vector3.zero;
		
//		print("Mouth Position :" + mouthposition);
		//mouthOffset.Set(mouthposition.x,1.2f,mouthposition.z);
		//mouthposition = mouthOffset;
		
		
		//Grab a hold of the ball to shoot.
		Ball b = BallInventory[0];
		
		
		//Activate it.
		b.Activate();
		
		//Set the balls velocity to zero.
		b.ZeroVelocity();
		
		
		//Set the position of the ball to the mouth position of the cannon.
		b.SetPosition(mouthposition);
	
		//Set the scale
		b.SetScale(BallScale);
		
		//Set shoot through
		b.SetShootThrough(ShootThrough);
		
		//Render Changing
		b.SetMaterial(BallMaterial);
	
		
		//Set the damage amount
		b.SetDamageAmount(DamageAmount);
	
		Rigidbody rbdy = b.GetComponent<Rigidbody>();
		
		//Set the mass amount when shot.
		rbdy.mass = BallMass;
		
		//Set the drag amount
		rbdy.drag = BallDrag;
		
		//Check to see if we need to turn off gravity
		rbdy.useGravity = !TurnOffGravityToShoot;
		
		//Set if shield gravity can control the ball.
		b.SetGravityControl(GravityControl);
		

		
		//Calculate the force.
		Vector3 ForceVector = Vector3.zero;
		float shootvelocity = ForceMagnitude;	
		
		if(MassPushFactor)
		{
			shootvelocity = MassPushVelocity * BallMass;	
		}
		
		
		
		ForceVector.Set(shootvelocity,shootvelocity,shootvelocity);
		
		//Scale the force with the forward vector of the cannon MOUTH to get the direction
		ForceVector.Scale(mouth.forward);
		
		
		//Apply the force and burn the ball.
		b.Fire(ForceVector, BallMass);
		
		//Set the active time.
		b.HotLifeTime = ActiveTime;
		
		
		//Release the ball
		BallInventory.Remove(b);
		
		//Set the timer
		timer = Time.time;
		
		//Play the particle effect
		if(ShootingEffect)
		{	
			ShootingEffect.transform.position = mouthposition;
			ShootingEffect.Play();
		}
		
		
		//PlaySound.
		AudioPlayer.GetPlayer().PlaySound(ShootSound,0.7f);
		
		
		//Return energy used.
		return FiringEnergyHeat;
		
		
	}
	
	bool UnAbleToFire()
	{
		//Calculate delta time
		float t = Time.time - timer;
		
		if(t <= ShootingRate)
		{
			return true;
		}
		
		if(BallInventory.Count == 0)
		{
			//Play Audio FIle.
			AudioPlayer.GetPlayer().PlaySound(EmptyShotSound);
			
			
			return true;
		}
		
		return false;
		
	}
	
	
	public void ForceReturn()
	{
		
		//Return back the values and set the attached cannon to null.
		InUse = false;
		
		//Hide the Capsule the cannon came in.
		//transform.Find("Capsule").gameObject.SetActive(true);
		capsule.SetActive(true);
		GetComponent<Collider>().enabled = true;
		
		
		//Ensable physics
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		
		
		//Get rid of all the balls.
		//ReleaseBalls();
		BallInventory.Clear();
		
		
		ReturnToEF();
	}
	
	public void ReturnToEF()
	{
		
		//Disable self and return;
		gameObject.SetActive(false);
		
		//Set the heat back to 0.
		heat = 0.0f;
		
		if(EntityFactory.GetEF())
		{
//			Debug.LogError("Returning Cannon" + CannonName);
			EntityFactory.GetEF().ReturnCannon(this);
			
		}
		else
		{
			Debug.LogError("Didnt RETURN!?!");
		}
		
	}
	
	public EntityFactory.CannonTypes CannonTypeInfo
	{
		get
		{
			return myType;
		}
		set
		{
			myType = value;
		}
	}
	
	void OnDestroy()
	{
		
//		Debug.LogError("Cannon Destroyed!!!!!" + CannonName);
		
		//Clean up the shooting effect on destroy.
//		if(ShootingEffect)
//			Destroy(ShootingEffect.gameObject);
	}
	
	
	public GameObject Capsule
	{
		get {
			return capsule;
		}
	}
	
	
	
}
