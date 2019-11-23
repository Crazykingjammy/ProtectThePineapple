using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupSpawner : MonoBehaviour {
	
	//public PowerupFactory powerUpFactory = null;
	
	// defaults active
	private bool  _active = true;
	float releasedPowerUpTime = 0.0f;
	
	public float releasePowerUpFreq = 2;
	public float spawnForceX = 0;
	public float spawnForceY = 0;
	public float spawnForceZ = 0;
	
	public bool spawnOnStart = false;
	public bool spawnRandom = false;
	
	public int maxSpawnCount = -1; // spawns indefinitely
	public PowerupFactory.PowerUpsDirectoryType spawnType = PowerupFactory.PowerUpsDirectoryType.DeathBeam;
	
	int currSpawnCount = 0;
	
	public bool IsActive(){ return _active; }
	public void SetActive(bool a) {_active = a;}
	public bool Toggle(){ 
		if (_active){ _active = false; }
		else{ _active = true; }
			return _active;
		}
	
	// Use this for initialization
	void Start () {
		
		// it all depends how we want to treat out spawners.  
		// Enemies can spawn powerups, as well as spawners.	
		
		// do we spawn when we're created?
		if (spawnOnStart){
			Spawn(this.transform, spawnType);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if ((Time.time - releasedPowerUpTime) > releasePowerUpFreq)
		{
			Spawn(this.transform, spawnType);
		}
		
	}
	
	public void Spawn(Transform trans = null, PowerupFactory.PowerUpsDirectoryType newSpawnType = PowerupFactory.PowerUpsDirectoryType.RANDOM){
		if (_active && maxSpawnCount < 0 || currSpawnCount <= maxSpawnCount){
			
			if (PowerupFactory.GetPUF() == null){
				return;
			}
			
			int randType = 0;


			if (newSpawnType == PowerupFactory.PowerUpsDirectoryType.RANDOM){//spawnRandom){
				randType = Random.Range(0, PowerupFactory.GetPUF().powerUpRootTypes.Count);
			}
			else{
				randType = (int)newSpawnType;//(int)spawnType;
			}
//


			
			PowerupBase spawnPU =  PowerupFactory.GetPUF().GetPowerUp((PowerupFactory.PowerUpsDirectoryType) randType);
			if (spawnPU){
				// make sure its active
				spawnPU.gameObject.SetActive(true);

				// who ever triggers spawn can set it to a transform
				if (trans != null){
					spawnPU.transform.position = trans.position;
				}
				else{
					spawnPU.transform.position = this.transform.position;
				}
				spawnPU.ProcessReleased();
				if (spawnPU.GetComponent<Rigidbody>()){
					float randX = Random.Range(0, spawnForceX);
					float randY = Random.Range(0, spawnForceY);
					float randZ = Random.Range(0, spawnForceZ);
					spawnPU.GetComponent<Rigidbody>().AddForce(randX, randY, randZ);
				}
				releasedPowerUpTime = Time.time;
					currSpawnCount++;
				}
			else{
				releasedPowerUpTime = 0;
				}	
			}
	}
	
//	public void GivePowerUp(PowerupRoot pu){
//	
//	}
	
}
