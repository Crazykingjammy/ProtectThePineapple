using UnityEngine;
using System.Collections;


public class Spawner : MonoBehaviour {
	
	
	//Attributes
	
	//Vector for spawning force if any.
	public Vector3 SpawnForce;
	
	//Number for the amount of times the spawner should spawn an object. -1 for infinite.
	public int NumberOfSpawns = 1;
	
	//The respwan wait time.
	public float RespawnFrequency = 2.0f;
	protected float respawnWaitTimer = 0.0f;
	
	//Create the object when the spawner is created.
	public bool CreateOnStart = true;
	public bool SendSpawnMessage = false;
	
	public bool AlwaysFinished = false;
	public bool AutomaticRespawnAtNull = true;
	
	
	
	//Data tracking data to track the amount of times the object has spawned.
	protected int respawnCount = 0;
	
	//This will be set to true if the spawner has nothing left to do.
	//Note: this value can be set to true despite being no limit to respawns. 
	protected bool Finished = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool IsFinished ()
	{
		if(AlwaysFinished)
			return true;
		
		return Finished;
	}
	
	public virtual GameObject containedObject
	{
		get {return null;}
	}


}