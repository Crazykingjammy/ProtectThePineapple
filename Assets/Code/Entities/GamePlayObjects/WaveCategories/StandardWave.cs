using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandardWave : BaseWave {
	 
	//List of target spawn points for all the contained enemies.
	List<TargetSpawner> EnemySpawnPoints;
	
	
	
	// Use this for initialization
	void Start () {
	
		EnemySpawnPoints = new List<TargetSpawner>();
		
		//FIll up the list.
		foreach(Transform t in transform)
		{
			TargetSpawner tspt = t.GetComponent<TargetSpawner>();
			
			if(tspt)
			EnemySpawnPoints.Add(tspt);
		}
		
			}
	
	// Update is called once per frame
	void Update () {
		
		//We always check if all the waves are finished, every frame.
		foreach(TargetSpawner spawn in EnemySpawnPoints)
		{
			if(!spawn.IsFinished())
			{
				return;
			}
		}
	
		
		//If we are down here then we should have all spawn points finished
		Completed = true;
	}
	
	//Destroy the spawners on destuction of the wave.
	void OnDestroy()
	{
		foreach(TargetSpawner spawn in EnemySpawnPoints)
		{
			Destroy(spawn.gameObject);
		}
	}
}
