using UnityEngine;
using System.Collections;

public class PowerupBase : Crate {
	
	private Target meAsTarget;
	
	/****START Merge From PowerupRoot****/
	// who spawned me
	// public PowerupSpawner powerUpSpawner = null;
	// storing the type helps in the memory manager.  Use the type for a quick lookup 
	// of the available object of the same type
	public PowerupFactory.PowerUpsDirectoryType type;
	
	public bool startReleased = false;
	
	public bool VisualsOnPickup = false;
	
	//bool isPowerupUnlocked = true;  // on release this needs to be false.
	bool isBackAtSpawner = true;	// When the powerup is no longer active in the scene
	
	/****END Taken From PowerupRoot****/
	
	
	// Power Up Description:
	// When the player obtains the powerup it will attach it's self to the player
	// and when the player is near the command center it will heal it
	
	// script is added on the rigid body, when picked up the 
	// rigidbody gets disabled and we move the affected Area Collider around
	
	// has a player picked me up
	bool isPickedup = false;

	// how many seconds can I stay in the playing area before disappearing
	public float idleTime = 10.0f;
	// keep track of when I was created
	float timeCreated = 0;
	
	/*
	 * Currently its all based on the Affect when we disable this powerup
	 * So keeping track of how long we're picked up is dependent on the length of the affect
	 */
	// float timePickedUp = 0;
	
	// this is just to categorize powerups
	public enum PowerUpType {
		PASSIVE, 
		AGGRESSIVE
	}
	public PowerUpType powerUpType = PowerUpType.PASSIVE;
	
	// the bot we get attached to
	GameObject host = null;
	// set the affected Area GameObject Collider to this Powerup if any
	public PowerupAffectBase affectedArea = null;
	// point me to the root Powerup Object, this way I can destroy it.
	// public PowerupRoot powerUpRoot = null;
	
	// Some info on the visual
	public string PowerUpTextureName = "PowerUpDefaultTxt";
	public string PowerUpName		 = "PowerUpDefaultName";
	
	// if true, the affected area will be positioned with the host
	protected bool keepAffectedAreaOnHost = true;
	public bool KeepAffectOnHost{
	set{ keepAffectedAreaOnHost = value;	}
	get{ return keepAffectedAreaOnHost; }
		
	}
	
	// Use this for initialization
	new void Start () {
		/** START Merge from Root **/
		isBackAtSpawner = true;
		PowerupFactory puf = PowerupFactory.GetPUF();
		transform.position = puf.transform.position;
		
		if (startReleased)
		{
			ProcessReleased();
		}
		/** END **/		
		
		timeCreated = Time.time;
		// created with everything off
		DisablePhysicsAndRendering();
		affectedArea.DeActivate();
		
		Target tempMeAsTarget = this.GetComponent<Target>();
		if (tempMeAsTarget != null){
			meAsTarget = tempMeAsTarget;
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckTiming();
		if (!host && affectedArea)
		{
			// set the parent and the affect position to my rigid body, keep us together
			// position back to the factory!
			PowerupFactory puf = PowerupFactory.GetPUF();
			affectedArea.transform.localPosition.Set(puf.transform.position.x, puf.transform.position.y, puf.transform.position.z);
			affectedArea.transform.position = GetComponent<Rigidbody>().position;
		}
		
		if (host && affectedArea){
//			if (host.rigidbody){
			//affectedArea.transform.position = host.rigidbody.position;
			//rigidbody.position = host.rigidbody.position;
			//}
			// if we have a host that means our physics body is deactive and kinetic.  we can update the position
			// updating the position of the affected area, most affective areas will 
			// be locked on the positon of the bot
			if(host.transform && keepAffectedAreaOnHost){
				affectedArea.transform.localPosition.Set(0.0f,0.0f,0.0f);
				affectedArea.transform.position = host.transform.position;
				
				GetComponent<Rigidbody>().position = host.transform.position;			
				//rigidbody.MovePosition(host.transform.position);
			}
		}
		if (!isPickedup){
				affectedArea.DeActivate();
		}
		if (meAsTarget != null){
			//Debug.Log("PowerupBase Has a target!!!!!!!");
			if (this.IsDestroyed()){
				Debug.Log("PowerupBase Is Destroyed!!!!!");
				
				this.DisablePhysicsAndRendering();
				ProcessBackToSpawner();
			}
		}
		
	}
	
	void CheckTiming(){
		// check if we have been idle too long
		//print("checking powerup time");
		if (!isPickedup && (Time.time - timeCreated)>idleTime){
			//KillMe();

			/*powerUpRoot.*/ProcessBackToSpawner();
		}
		
	}
	
	public void KillMe(){
		// print("killing powerup");
		Destroy(gameObject);
		//Destroy(affectedArea.gameObject);
		//Destroy(gameObject);
		
	}
	
	void DisablePhysicsAndRendering(){
		gameObject.GetComponent<Rigidbody>().isKinematic = true;
		gameObject.GetComponent<Rigidbody>().GetComponent<Collider>().isTrigger = true;
		gameObject.GetComponent<Rigidbody>().Sleep();
		gameObject.GetComponent<Renderer>().enabled = false;
		
		if(GetComponent<ParticleSystem>())
		{
			GetComponent<ParticleSystem>().Stop();
		}
	}
	void EnablePhysicsAndRendering(){
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Rigidbody>().GetComponent<Collider>().isTrigger = false;
		gameObject.GetComponent<Rigidbody>().WakeUp();
		gameObject.GetComponent<Renderer>().enabled = true;	
		
		if(GetComponent<ParticleSystem>())
		{
			GetComponent<ParticleSystem>().Play();
		}
	}
	
	public void ProcessPickedUp(GameObject go){
		// set flags
		// collider if we need to.
		host = go;
		//this.timePickedUp = Time.time;			
		this.isPickedup = true;
		affectedArea.Activate();
		// disable all the physics and rendering
		DisablePhysicsAndRendering();
		
		if(GetComponent<ParticleSystem>())
		{
			
			GetComponent<ParticleSystem>().Play();
		
		}
		
	}
	public void ProcessReleased(){
		/** START Merge from Root **/
		
		isBackAtSpawner = false;
		
		// when im released where do I start from, the last position of my transform
		GetComponent<Rigidbody>().position = transform.position;
		
		/** END */
		
		// set flags
		host = null;
		isPickedup = false;

		//timePickedUp = 0;
		timeCreated = Time.time;	// created can also be used for released
		affectedArea.DeActivate();
		
		// enable all the physics and rendering
		EnablePhysicsAndRendering();
	}
	
	public void ProcessBackToSpawner(){
		/** Merge with Root **/
		// print ("ProcessBackToSpawner");
		
		DisablePhysicsAndRendering();
		transform.position = PowerupFactory.GetPUF().transform.position;
		// we are no longer active, setting this flag will tell the factory to put it back to the active list
		isBackAtSpawner = true;
		
		/** END **/
		
		//DisablePhysicsAndRendering();
	}
	
	public void AffectDone(){
		ProcessBackToSpawner();
//		if (powerUpRoot)
//		{
//			// if we have a spwaner go back to him
//			
//			powerUpRoot.ProcessBackToSpawner();			
//		}
	}
	
	
	void OnCollisionEnter(Collision col)
	{	
		
		//Check if we collide with a Player Bot
		if(col.gameObject.CompareTag("Bot"))
		{
			// set its host so that we can update our position 
			// to the bot
			// print("i hit the powerup!");
			
			// we have been picked up by the bot, now we can update out affective area 
			ProcessPickedUp(col.gameObject);
		}
		
		
	
		
	}//All Collisions Check
	
	public bool IsPickedUp(){
		return isPickedup;
	}

	public void SetHost(GameObject go){
		this.host = go;
	}

	/** MERGED from Root **/
	public bool IsBackAtSpawner(){
		return isBackAtSpawner;
	} 
	public void SetIsBackAtSpawner(bool isBack){
		isBackAtSpawner = isBack;
	} 
	/** END from Root **/
}
