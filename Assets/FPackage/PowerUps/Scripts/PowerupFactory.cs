using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class PowerupFactory : MonoBehaviour {
	
	public enum PowerUpsDirectoryType{
		LifeAuraCC = 0,
		LifeAuraBot,
		DeathBeam,
		SoccerBall,
		RANDOM
	}
	//Static object to be accessed project wide.
	static private PowerupFactory PUF;
	
	// a static list of all the potiential types of powerups
	public List<PowerupBase> powerUpRootTypes;
	// when this factory is created, we can set the amount of each object we want to allocate
	// the index is the index of the RootTypes
	public List<int> startPoolWithTypeCount;
	
	
	public bool _allowSpawn = false;
	
	// This is the main list of power ups for the pool
	// when an object requests a powerup, it will get it from this list
	//List<PowerupBase> poolAvailablePowerUps;
	
	// when the requesting object receives a powerup, the power up is removed from the avail list
	// and onto unavailable, until the Powerup becomes disabled and then returns to the available.
	List<PowerupBase> poolUnavailablePowerUps;
	
	// TODO: remove the dictionary
	// Dictionary<int, PowerupRoot> poolHashTableAvailablePowerups;
	
	// this is the main list containing the list of objects.  
	List<PowerupMemoryList> objectListMemTypes = null;
		
	/*
	 * we want a list of lists.  Each item in the list Indexed by the Powerup Directory
	 * Each item contains a list of the available pool of powerups of that type
	 * Allocate the amount of each type index
	 * We can then retrieve a powerup by the index and returning the first object available
	 * if none is available it will create one and return the new one.
	 * 
	 */
	 
	

	//public PowerUpsDirectory puDirectory;
	
	// Use this for initialization
	void Start () {
		// set the instance
		PUF = this;
		
		poolUnavailablePowerUps = new List<PowerupBase>();
		
		// poolHashTableAvailablePowerups = new Dictionary<int, PowerupRoot>();
		
		objectListMemTypes = new List<PowerupMemoryList>();
	
		Transform myTransform = transform; 
		
		/*
		 * Testing with custom 2d Table
		 * For each type of powerup, we create a list that contains a list of powerup objects
		 * 
		 */
		for (int i = 0; i < powerUpRootTypes.Capacity; i++){

			// the memory list
			PowerupMemoryList curr = new PowerupMemoryList(); // Instantiate(powerupMemoryListPrefabRef, Vector3.zero, Quaternion.identity) as PowerupMemoryList;   // when creating
			
			// set the type of list is to carry
			curr.SetType((PowerUpsDirectoryType)i);
			
			// this creates all the default starting allocation and its prefab of that type index
			curr.CreateMemObjects(startPoolWithTypeCount[i], powerUpRootTypes[i],myTransform);
			
			//Parent?
			
			
			// add the new list to the list of lists.
			objectListMemTypes.Add(curr);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckPowerupAvailability();
	}
	
	static public PowerupFactory GetPUF(){
		if (PUF){
			return PUF;}
		else{
			return null;}
	}
	
	/*
	 * Returns the first powerup in the list
	 * 
	 */
	public PowerupBase GetPowerUp(PowerUpsDirectoryType type){
		// the Powerup we're potientially returning
		PowerupBase currPU = null;
		
		if (!_allowSpawn){
			return null;
		}
		
		// testing with custom memory manager
		/*
		 * Using the passed in type, use type as index to the next available type
		 */
		
		PowerupMemoryList currList = null;
		currList = objectListMemTypes[(int)type];
		
		if (currList != null){
			currPU = objectListMemTypes[(int)type].GetNext();
			if (currPU){
				currPU.SetIsBackAtSpawner(false);
				poolUnavailablePowerUps.Add(currPU);			
			}
		}
		
		return currPU;
	}
	
	void CheckPowerupAvailability(){
		// for now, go through the list of unavailable 
		
		for (int i= 0; i< poolUnavailablePowerUps.Count; i++){
			PowerupBase currPu = poolUnavailablePowerUps[i];
			if (currPu.IsBackAtSpawner()){
				// then the PU needs to go back into the available pool
				poolUnavailablePowerUps.Remove(currPu);
				//poolAvailablePowerUps.Add(currPu);
			}
		}
	}
	
	public bool ToggleFactory(){
		_allowSpawn = !_allowSpawn;
		return _allowSpawn;
	}
	
	public bool IsSpawning(){
		return _allowSpawn;
	}
}
