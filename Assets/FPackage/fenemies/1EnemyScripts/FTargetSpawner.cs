using UnityEngine;
using System.Collections;

public class FTargetSpawner : MonoBehaviour {

	//Attributes

	// a Target Spawner can be used automatically or controllered by another target
	// when has Controller is enabled the spawners spawn freq and timer clock have no effect
	// if target spawner has no controller then spawning is automated by using spawnfreq and clock
	public bool HasController = false;
	public bool SpawnOnStart = false;


	//Vector for spawning force if any.
	public float SpawnForce = 0.0f;
	
	// how many times has this spawner spawned
	public int SpawnCount = 0;
	
	//The respwan wait time.
	public float SpawnFreq = 3.0f;
	protected float SpawnTimerClock = 0.0f;


	//Type of target to spawn
	public EntityFactory.TargetTypes type;

	public bool OverrideAims = false;
	public bool AttackBot = false;
	public bool AttackCC = false;
	
	// Use this for initialization
	void Start () {
		if (SpawnOnStart){
			SpawnATarget();
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!HasController){
			// we dont have a controller so we can spawn automatically
			ProcessAutoSpawning();
		}
	
	}

	public void SpawnATarget(){

		Target newTarget = EntityFactory.GetEF().CreateTarget(type,transform.position);

		if (newTarget == null){
			return;
		}

		BaseEnemy e = newTarget.GetComponent<BaseEnemy>();



		if (e != null){
			// its a baseenemy
			if(OverrideAims)
			{
				e.AttackBots = AttackBot;
				e.AttackCommandCenters = AttackCC;
			}
			

		}

		if (newTarget != null){
			if (newTarget.GetComponent<Rigidbody>()){
				Vector3 forwardForce = transform.forward;
				
				forwardForce = -forwardForce * SpawnForce;
				// forwardForce.Scale(SpawnForce);
				
				newTarget.GetComponent<Rigidbody>().AddForce(forwardForce * newTarget.GetComponent<Rigidbody>().mass);
			}
		}

		SpawnCount++;

		// reset the spawn clock.
		SpawnTimerClock = Time.time;
	}

	public void ProcessAutoSpawning(){
		// we have no controller so when a spawner is out have it spawn.
		float currTime = Time.time;
		if ((currTime - SpawnTimerClock) > SpawnFreq){

			SpawnATarget();

			// reset clock always after a targetspawn.  reset moved to spawn function
			//SpawnTimerClock = currTime;
		}
		
	}
}

