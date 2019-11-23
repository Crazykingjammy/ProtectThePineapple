using UnityEngine;
using System.Collections;

public class CannonSpawner : Spawner {
	
	//Type of cannon to spawn
	public EntityFactory.CannonTypes type;

	//This is what we are holding
	Cannon self; 
	
	// Use this for initialization
	void Start () {
	
		//Instantuate the object on start.
		if(CreateOnStart)
		{
			//Create the object
			self = EntityFactory.GetEF().CreateCannon(type,transform.position);
			
			//Send a spawn message if that is our desire
			if(SendSpawnMessage)
			{
				SendMessageUpwards("SpawnedCannon", self);
			}
			
			//Add the spawning force if any.
			if(self.GetComponent<Rigidbody>())
			{
				//print("Call?");
				self.GetComponent<Rigidbody>().AddForce(SpawnForce * self.GetComponent<Rigidbody>().mass);
			}
			
			
			//Update the spawn count, since this IS the start function, we will set the spawn count exactly to 1.
			respawnCount = 1;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if(AutomaticRespawnAtNull)
		{
			SpawnSelfObject();
		}
		
	}
	
	void ReturnSelf()
	{
		//Return self here.
		self.ReturnToEF();
		
		self = null;
		
	}
	
	void OnDestroy()
	{
		if(!self)
			return;
	
		//Etiher way we mark the cannon as dirty in case its equiped ant wont be destroyed.
		//self.SetDirty(true);
		
		//If we are not in use mark the cannon to be destroyed.
//		if(!self.InUse)
//		{
//			ReturnSelf();
//		}
	}
	
	void SpawnSelfObject()
	{
		if(!self)
		{
			//Create the object
			self = EntityFactory.GetEF().CreateCannon(type,transform.position);
			
			//Send a spawn message if that is our desire
//			if(SendSpawnMessage)
//			{
//				SendMessageUpwards("SpawnedCannon", self);
//			}

			//Add the spawning force if any.
			if(self.GetComponent<Rigidbody>())
			{
				self.GetComponent<Rigidbody>().AddForce(SpawnForce * self.GetComponent<Rigidbody>().mass);
			}
			
			//Update the spawn count
			respawnCount++;
		}
		
		
		
	}
	
	public Cannon TriggerSpawn()
	{
		SpawnSelfObject();
		
		return self;
	}
	
	//Returns true if kill was successful.
	public bool KillSpawn()
	{
		//Check if we have somting spawned. Return true as to our child is dead.
		if(!self)
			return false;
		
		if(!self.InUse)
		{
			//Destroy the self and return true.
			ReturnSelf();
			return true;
		}
		
		//Cant do anything so we return false.
		return false;
		
		
	
	}
	
	
	public override GameObject containedObject
	{
		get {return self.gameObject;}
	}
	
	public Cannon Item
	{
		get {return self;}
	}
}
