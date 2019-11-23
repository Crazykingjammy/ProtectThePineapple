using UnityEngine;
using System.Collections;

public class CannonConnector : MonoBehaviour {
	
	public int tool;
	
	public ParticleSystem DetachCannonEffect;
	public ParticleSystem PickupCannonEffect;
	
	public Vector3 DetachTorque;
	
	public Vector3 DetachOffset;
	
	
	public float DetachedHeat = 700.0f;
	
		
	//Store the cannon and what we are attached to.
	Cannon CannonAttachmentSlot;
	GameObject Host;
	
	Transform CannonLookAt;

	// caches the transform for CannonConnectorPos connectorPos
	// cache the full CannonConnectorPos Object, not just its transform
	// Transform connector = null;
	CannonConnectorPos myCannonConnectorPos;
	
	bool HotCannon = false;

	public float DetachForceMin = 900.0f;

	// on detach, where is the cannon going?
	Vector3 forceDir = Vector3.zero;

	public enum DetachMethod{
		DetachRandom, 
		DetachConnectorPosUp
	}

	public DetachMethod MyDetachMethod = DetachMethod.DetachConnectorPosUp;
	
	// Use this for initialization
	void Start () {
			
		if(DetachCannonEffect)
		{
			//Create the detach effect.
			DetachCannonEffect  = Instantiate(DetachCannonEffect) as ParticleSystem;
			DetachCannonEffect.transform.position = DetachOffset;
			DetachCannonEffect.transform.parent = transform;
			//DetachCannonEffect.transform.localPosition = Vector3.zero;
		}
		
		if(PickupCannonEffect)
		{
			PickupCannonEffect = Instantiate(PickupCannonEffect) as ParticleSystem;

			PickupCannonEffect.transform.parent = transform;
			PickupCannonEffect.transform.localPosition = Vector3.zero;

		}

		CannonConnectorPos connectorPos = this.gameObject.GetComponentInChildren<CannonConnectorPos>();
		if (connectorPos){
			myCannonConnectorPos = connectorPos;

			// we have our position, now let the position know of the Gameobject
			// the position will align its self to the Enemies or Objects aim
			// currently only enemies are using this, even though a cannon connector is shared with a bot
			myCannonConnectorPos.CannonConnectorPosHost = Host;


		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//Find the connector from the host and attach it if we have a cannon.		
		if(CannonAttachmentSlot)
		{
			
//			//Just check if we have an attachment panel attached to the child bot.
//			if(!Host.transform.FindChild("CannonConnector") )
//			{
//				//There is no connector to the bot.
//				print("Error, this bot does not contain an connect for a cannon. All Bots should have Cannon Connectors");
//				return;
//			}

			//Default values.
			CannonAttachmentSlot.transform.localPosition.Set(0.0f,0.0f,0.0f);
			//CannonAttachmentSlot.transform.localRotation = Quaternion.identity;
		
			//Attach the cannon to the handle position.
			CannonAttachmentSlot.transform.position = myCannonConnectorPos.transform.position;
			CannonAttachmentSlot.transform.rotation = myCannonConnectorPos.transform.rotation;
			CannonAttachmentSlot.transform.localScale = myCannonConnectorPos.transform.localScale;
			
			
		}
		

		
				
	
	}
	
	
	public Cannon GetCannon()
	{
		return CannonAttachmentSlot;
	}
	public void SetHost(GameObject host)
	{
		Host = host;
	}
	
	public void DetachCannon()
	{
		
		//If there is no cannon then there is nothing to detach!
		if(!CannonAttachmentSlot)
		{
			Debug.LogError("Trying to detach cannon and there is no cannon!");
			return;
		}
		
		//Return back the values and set the attached cannon to null.
		CannonAttachmentSlot.InUse = false;
		
		//Set the heat amount.
		CannonAttachmentSlot.Heat = DetachedHeat;
		
		//Hide the Capsule the cannon came in.
		//CannonAttachmentSlot.transform.Find("Capsule").gameObject.SetActive(true);
		CannonAttachmentSlot.Capsule.SetActive(true);
		CannonAttachmentSlot.GetComponent<Collider>().enabled = true;
		
		//Ensable physics
		CannonAttachmentSlot.GetComponent<Rigidbody>().isKinematic = false;
		CannonAttachmentSlot.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		
		
		//At have to modifiy the mass of the bot as the cannon is dropped.
		//Host.rigidbody.mass -= CannonAttachmentSlot.rigidbody.mass;
		
		
		//Here we can add a detachment force
		Vector3 f = DetachOffset;
		//f.Set(0.0f,7.0f,0.0f);
		f += myCannonConnectorPos.transform.position;
		CannonAttachmentSlot.transform.position = f;
		
		//Play the particle effect at position f. 
		if(DetachCannonEffect)
		{
			//DetachCannonEffect.transform.localPosition = f;
			DetachCannonEffect.Play();
		}
		
		
		//zero the velocity?
		CannonAttachmentSlot.GetComponent<Rigidbody>().velocity = Vector3.zero;
		CannonAttachmentSlot.transform.rotation = Quaternion.identity;
		
		
		//Here we get rid of all the balls that the cannon may have 
		CannonAttachmentSlot.ReleaseBalls();
		

		//If we are a hot cannon invalidate 
		if(HotCannon)
		{
			//Destroy the cannon being detached if we are a hot cannon.
			//Destroy(CannonAttachmentSlot.gameObject,1.0f);
			//CannonAttachmentSlot.SetDirty(true);
			CannonAttachmentSlot.KeepMeArround = false;
		}
		
		//Add the force.

		//f = Random.insideUnitSphere * DetachForceMin;

		switch(MyDetachMethod){
			// switch to make different forceDir's
		case DetachMethod.DetachRandom:{
				forceDir.Set(
					Random.Range(2, DetachForceMin), 
					Random.Range(2, DetachForceMin), 
					Random.Range(2, (DetachForceMin*2))
					);
			break;		
		}
		case DetachMethod.DetachConnectorPosUp:{

			forceDir = myCannonConnectorPos.transform.up;
			forceDir *= DetachForceMin;
			forceDir.z *= 2;

			break;
		}

		}
		//
		f = forceDir;
		CannonAttachmentSlot.GetComponent<Rigidbody>().AddForce(f * CannonAttachmentSlot.GetComponent<Rigidbody>().mass, ForceMode.Force);
		CannonAttachmentSlot.GetComponent<Rigidbody>().AddTorque(DetachTorque * CannonAttachmentSlot.GetComponent<Rigidbody>().mass,ForceMode.Force);
		
		
		//Last but not lease do make sure that we nullify the cannon attachment slot.
		CannonAttachmentSlot = null;
		
		
	}
	
	public bool AttachCannon(Cannon barrel)
	{
		//If we already have a cannon need not do more. We will handle this checking here.
		if(CannonAttachmentSlot)
		{
			//print("Trying to attach a cannon with a cannon already attached");
			
			return false;
			
		}
		
		//Assign the Cannon
		//CannonAttachmentSlot = barrel;


		//barrel.IsDirty

		//We dont pick up dirty cannons.
		//And dont pick up burning cannons.
		if(barrel.IsDirty() || barrel.IsBurning)
		{
			CannonAttachmentSlot = null;
			//DetachCannon();
			return false;
		}
		

		//Assign the Cannon
		CannonAttachmentSlot = barrel;
		
		
		//Now we proceed to pick up the Cannon
		CannonAttachmentSlot.InUse = true;
		
		
		//Hide the Capsule the cannon came in.
		//CannonAttachmentSlot.transform.Find("Capsule").gameObject.SetActive(false);
		CannonAttachmentSlot.Capsule.SetActive(false);
		CannonAttachmentSlot.GetComponent<Collider>().enabled = false;
		
		//Disable physics
		CannonAttachmentSlot.GetComponent<Rigidbody>().isKinematic = true;
		CannonAttachmentSlot.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		
		//Play the effect for picking up
		if(PickupCannonEffect)
		{
			//PickupCannonEffect.transform.position = CannonAttachmentSlot.transform.position;
			PickupCannonEffect.Play();
		}
		
		//At have to modifiy the mass of the bot as the cannon is picked up.
		//Host.rigidbody.mass += CannonAttachmentSlot.rigidbody.mass;
		
		//Zero out the cannon look at since we wont be updating rotation every farme like we will be position.
		AimAt();
		
		return true;
		
	}
	
	public int GetBallsInStorage()
	{
		if(CannonAttachmentSlot)
		{
			return CannonAttachmentSlot.BallInventory.Count;
		}
		
		return 0;
		
	}
	
public void SetHeat(float h)
	{
		if(CannonAttachmentSlot)
		{
			CannonAttachmentSlot.Heat = h;
		}
	}
	
	public void AimAt(GameObject obj = null)
	{
		
		if(!obj)
		{
			//If the game object is null we look at the default forward of the connector. 
			
			//Get the Cannon Connector Transform.
			if(!myCannonConnectorPos.transform)
			{
				//Grab the connector
				// Franks edit.  
				Debug.LogWarning("Cannon Connector: " + 
				                 this.gameObject.name +" ** Has not CannonConnectorPos Transform! **");
				//myCannonConnectorPos.transform = Host.transform.FindChild("CannonConnector").transform;

				//connector = Host.GetComponentInChildren<CannonConnector>().transform;
				//return;
			}
			
			
			if(myCannonConnectorPos.transform)
			{
				//Store the rotation of the connector in a local value.
				// TODO: Quaternion rotation = connector.rotation;
					
				//Assign the rotation of the cannon to the rotation of the connector slot.
				// TODO: CannonAttachmentSlot.transform.rotation = rotation;
		
				//Zero out the angular velocity. The position will be updated every frame so no need to reset the regular veloctiy.
				//CannonAttachmentSlot.rigidbody.angularVelocity = Vector3.zero;

				//TODO: Temp.  disabled above code for cannon rotation because with animation the cannon connector rotates.
				Quaternion rotation = Quaternion.identity;
				CannonAttachmentSlot.transform.rotation = rotation;

			
			}
			
			
			return;
		}

	
		if(CannonAttachmentSlot)
		{
			//Zero out the angular velocity. The position will be updated every frame so no need to reset the regular veloctiy.
			//CannonAttachmentSlot.rigidbody.angularVelocity = Vector3.zero;
			
			CannonAttachmentSlot.transform.LookAt(obj.transform.position);
		}
		
		
	}
	
	
	public void SetHotCannon(bool hot)
	{
		HotCannon = hot;
	}
	
	public bool IsHotCannon()
	{
		return HotCannon;
	}
	
	public bool HasCannon()
	{
		if(CannonAttachmentSlot)
		{
			return true;
		}
	
		return false;
	}
	
	
	/// <summary>
	/// Determines whether this instance is cannon full. Returns true if its full, false otherwise, include if cannon is missing.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is cannon full; otherwise, <c>false</c>.
	/// </returns>
	public bool IsCannonFull()
	{
		if(CannonAttachmentSlot)
		{
			if(CannonAttachmentSlot.IsFull())
			{
				return true;
			}
		}
		
		
		return false;
	}
}
