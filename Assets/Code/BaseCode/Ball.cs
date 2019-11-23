using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	
	//Public varables
	public float DamageAmount = 5.0f;
	public float HotLifeTime = 2.0f;
	public float Mass = 1.0f;
	
	//store the orignal color for retrevial and the body.
	Transform body = null;
	
	public Vector3 PopForce;
	
	public ParticleSystem StandardHitEffect;
	public ParticleSystem TrailEffect;
	
	TrailRenderer trail;
	
	ParticleSystem localtrail;
	ParticleSystem localHit;
	
	bool Active = false;
	float life;
	float originalScale;
	float originalDrag;
	bool shootThrough;
	bool gravityControlled = false;
	
	Material originalMaterial;
	
	Transform trailTransform;
	Transform myTransform;
	
	
	Rigidbody myRigidbody;
	
public	enum BallSourceID
	{
		Bot,
		Enemy,
		Netural,
		Deflect,
		Capture,
		Hot
	}
	
	public BallSourceID ID = BallSourceID.Netural;
	
	// Use this for initialization
	void Start () {
		
		
		localtrail = Instantiate(TrailEffect) as ParticleSystem;
		localtrail.transform.position = transform.position;
		localtrail.transform.parent = transform;
		trailTransform = localtrail.transform;
		
		if(StandardHitEffect)
		{
			StandardHitEffect = Instantiate(StandardHitEffect) as ParticleSystem;
			
			//Set as parent and zero out all positioning.
			StandardHitEffect.transform.parent = transform;
			
			//StandardHitEffect.transform.position = Vector3.zero;
			StandardHitEffect.transform.localPosition =  Vector3.zero;		
			
		}
			
		//Cache rigid body.
		myRigidbody = GetComponent<Rigidbody>();
		myTransform = transform;
		
		
		//Locate the body and store the original color.
		foreach(Transform child in transform)
		{
			if(child.CompareTag("Body"))
			{
				originalMaterial = child.GetComponent<Renderer>().sharedMaterial;	
				body = child;
			}
			
		}
		
		trail = GetComponent<TrailRenderer>();
		
		if(trail)
		{
			trail.enabled = false;
		}
		
		//Store the original scale
		originalScale = transform.localScale.x;
		
		//Store the original drag
		originalDrag = myRigidbody.drag;
	}
	


	void Update()
	{
		if(!Active)
		{
			return;
		}
		
		//Print some output here.
		//print("Ball Speed at :" + rigidbody.velocity);
		
		//Get the current life time.
		life += Time.deltaTime;
		
		if(life >= HotLifeTime)
		{
			//Here is where we pop the ball. and reset the mass.
			Pop();
		}
		
	}
	
	
	public void ResetTimer(float deltashift = 0.0f)
	{
		life = deltashift;
	}
	
	public void Fire(Vector3 force)
	{
		Burn(force,Mass);
	}
	
	public void Fire(Vector3 force, float setmass)
	{
		Burn(force,setmass);
	}
	
	
	/// <summary>
	/// Burn this instance. Everytime a ball is shot it should be burned to know its acitve!
	/// </summary>
	void Burn(Vector3 force, float mass)
	{
		
		//Set the mass to the shooting mass
		myRigidbody.mass = mass;
		
		//Free the ball.
		FreeBall();
		
		//Apply the force.
		myRigidbody.AddForce(force,ForceMode.Force);
		
		
		//Effects for the trail of the ball.
		if(localtrail)
		{
			//Set the position and play.
			trailTransform.position = myTransform.position;
			localtrail.Play();
			
		}
		
		
	}
	
	
	public void FreeBall()
	{
				
		//Activate it, and set the life.
		Active = true;
		life = 0.0f;
	}
	
	public bool IsActive()
	{
		return Active;
	}
	
	/// <summary>
	/// This deactivates the ball so that it is hidden and outside of the scene
	/// </summary>
	public void DeActivate()
	{

		if(StandardHitEffect)
			StandardHitEffect.Stop();
		
		Active = false;
		
		gameObject.SetActive(false);
		
		
	}
	
	public void Activate()
	{
	
		Active = true;
		
		gameObject.SetActive(true);
			
		
	}
	
	/// <summary>
	/// Sets the position. Simple pubic modifier 
	/// </summary>
	/// <param name='position'>
	/// Position.
	/// </param>
	public void SetPosition(Vector3 position)
	{
		//Set the position
		myTransform.position = position;
	}
	
	/// <summary>
	/// Pop returns hte ball to its orignal state.
	/// </summary>
	public void Pop()
	{
		//Restore the Scale
		Vector3 scale = Vector3.zero;
		scale.Set(originalScale,originalScale,originalScale);
		myTransform.localScale = scale;
		
		//Handle the physics numbers and stuff.
		myRigidbody.mass = Mass;
		myRigidbody.useGravity = true;
		myRigidbody.drag = originalDrag;
		myRigidbody.AddForce(PopForce);
		myRigidbody.AddTorque(PopForce);
		
		
		Active = false;
		ID = BallSourceID.Netural;
		shootThrough = false;
		body.GetComponent<Renderer>().sharedMaterial = originalMaterial;
		gravityControlled = false;

		//** Franks Add **
		SetBallSourceID(Ball.BallSourceID.Netural);
		// ** END FRANKS ADD
			
		//Stop the trail effect.
		if(localtrail)
		{
			localtrail.Stop();
		}
		
		if(trail)
		{
			trail.enabled = false;
		}
		
		//Send message to GOT
		GameObjectTracker.instance.BallPop();
	
	}
	
	public void EnableTrail()
	{
		if(trail)
		{
			trail.enabled = true;
		}
		
	
	}
	
	public void SetGravityControl(bool b)
	{
		gravityControlled = b;
	}
	public bool GetGravityControl()
	{
		return gravityControlled;
	}
	
	public void SetScale(float s)
	{
		//Restore the Scale
		Vector3 scale = Vector3.zero;
		scale.Set(s,s,s);
		myTransform.localScale = scale;
		
	}
	
	public void SetMaterial(Material m)
	{
		body.GetComponent<Renderer>().sharedMaterial = m;
	}
	
	public void SetShootThrough(bool b)
	{
		shootThrough = b;
	}
	public bool IsShootThrough()
	{
		return shootThrough;
	}
	public void SetDamageAmount(float damage)
	{
		DamageAmount = damage;
	}
	
	public void SetBallSourceID(BallSourceID id)
	{
		ID = id;
	}
	
	public BallSourceID GetBallSourceID()
	{
		return ID;
	}
	
	public void ZeroVelocity()
	{
		//Set velocity to zero.
		myRigidbody.velocity = Vector3.zero;
	}
	public void SetDrag(float drag)
	{
		myRigidbody.drag = drag;
	}
	public void ResetDrag()
	{
		myRigidbody.drag = originalDrag;
	}
	
	public void HitTarget()
	{
		//Grab the GOT
		GameObjectTracker go = GameObjectTracker.instance;
		
		switch(ID)
		{
		case BallSourceID.Bot:
		{
			go.TargetHit();
			break;
			
		}
		case BallSourceID.Capture:
		{
			go.CaptureHits();
			break;
		}
		case BallSourceID.Deflect:
		{
			go.DeflectHit();
			break;
		}
		case BallSourceID.Enemy:
		{
			
			break;
		}	
		case BallSourceID.Hot:
		{
			break;
		}
		default:
			break;
			
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		
		//Store the current position of the ball.
		//Vector3 ballposition = transform.position;
		GameObject go = col.gameObject;
		Vector3 goPos = go.transform.position;
		
		// we will just return if there is somthing we dont want to play the effect for.
		if(go.CompareTag("Ball"))
		{
			return;
		}
		
		if(go.CompareTag("Ground"))
		{
			return;
		}
	
		if(go.CompareTag("Cannon"))
		{
			Cannon c = go.GetComponent<Cannon>();
			
			if(c && ID != BallSourceID.Netural)
			{
				//Check if we arent burning before we get hit.
				bool ex = false;
				if(!c.IsBurning)
					ex = true;
					
				//Apply the damage.
				c.Heat += DamageAmount;
				
				//If we wernt burning before and now are, then explode!
				if(c.IsBurning && ex)
				{
					//Play chicken sound when shot to explosion.
					AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Chicken);

					c.Explode();

				}
			}
			
			return;
		}
		
//		if(col.gameObject.CompareTag("Bot"))
//		{
//			if(Active)
//			{
//			//	print("Active Collision!!");	
//			}
//					
//		}
		
		if(go.CompareTag("Target"))
		{
			if(StandardHitEffect )
			{
				//StandardHitEffect.transform.position = goPos;
				StandardHitEffect.Play();
			}
		}
		
		//If we hit the wall as the bots ball then we will register a miss event.
		if(go.CompareTag("Wall"))
		{
						
			if(StandardHitEffect )
			{
				//StandardHitEffect.transform.position = myTransform.position;
				StandardHitEffect.Play();
			}
		}
		
		
	}
	
	
	
}
