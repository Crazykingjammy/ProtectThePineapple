using UnityEngine;
using System.Collections;

public class TargetSpawner : Spawner {
	
	//Attributes
	
	//Type of target to spawn
	public EntityFactory.TargetTypes type;
	
	//Bool to make the target have the captured flag.
	public bool SpawnedCaptured = false;
	
	public bool OverrideAims = false;
	public bool AttackBot = false;
	public bool AttackCC = false;
	
	//The object that we are holding
	Target self = null;
	
	public Target target
	{
		get {return self;}
	}
	public override GameObject containedObject
	{
		get {return self.gameObject;}
	}
	
	void OnEnable()
	{
		//Reset the number of spawns.
		respawnCount = 0;
		
		if(CreateOnStart)
		{
			Respawn();
		}
		
		
		
	}
	
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!self && AutomaticRespawnAtNull)
		{
			Respawn();
			return;
		}
			
		//Check if we have no target available
		if(!self.gameObject.activeSelf && AutomaticRespawnAtNull)
		{
			Respawn();
		}
		
		//What to do if our self does not exist.
		if(self.IsDestroyed())
		{
			//We check if we have reached our respawn max and flag the spawner as finished.
			if(respawnCount >= NumberOfSpawns && (NumberOfSpawns != -1))
			{
				Finished = true;
			}
		
			//Call Respawn if we are destroyed
			Respawn();
			
		}
		
	}
	
	void Respawn()
	{
		//If we are not within the timer limit we return and come back when we are.
		if( (Time.time - respawnWaitTimer) <= RespawnFrequency)
		{
//			print("TIMER RESTRAINT");
			return;
		}
		
		//If our respawn count does not match the number of spawns put in, we recreate an object.
		if(respawnCount < NumberOfSpawns || (NumberOfSpawns == -1) )
		{
			//Assign the self to a new target.
			self = EntityFactory.GetEF().CreateTarget(type,transform.position);
			
			//If the spawner spawns captured tarets
			self.Captured = SpawnedCaptured;
		
			BaseEnemy e = self.GetComponent<BaseEnemy>();
			if(e != null)
			{
			//	Debug.LogError("NOT AN ENEMY! " + self.name);
				if(OverrideAims)
				{
					e.AttackBots = AttackBot;
					e.AttackCommandCenters = AttackCC;
				}
			}
			
			//Set the timer at current spawn
			respawnWaitTimer = Time.time;
					
			//Update the spawn count.
			respawnCount++;
		}
		
		
		
	}
}
