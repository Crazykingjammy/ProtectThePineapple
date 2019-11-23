using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallStack: MonoBehaviour {
	
	//Force amount to draw in the balls with.
//	public BlackHole GatherPoint;
	
	public Ball ball;
	public int NumberBalls = 30;
	
	//The array of balls in a ball stack
	public List<Ball> Balls;
	public float Wait = 0.2f;

	public float WaitAfterInitial = 5.0f;

	// how hard and angle do we spawn the ball
	public Vector3 SpawnForce = Vector3.zero;

	// how many do we spawn in the beginning
	public int SpawnInitialCount = 5;

	public bool AttachToCC = false;
	public bool SpawnRandom = false;

	
	//Misc Values to apply to the ball.
	//public bool UseGravity = false;
	
	float counter = 1.0f;

	int spawnCounter = 0;

	// Use this for initialization
	void Start () {

//		if (AttachToCC)
//		{
//			// get the position of the CC
//			this.transform.position = ToyBox.GetPandora().CommandCenter_01.transform.position;
//		}
			
	}

	public bool SpawnBall(){

//		Debug.LogError("bawl spwn");
		
		if(Balls.Count < NumberBalls)
		{
			if(counter > Wait)
			{
				
				Ball b = Instantiate(ball,transform.position,Quaternion.identity) as Ball;	
				b.transform.parent = transform;
				Balls.Add(b);
				Vector3 finalSpawnForce;
				if (SpawnRandom){
					Vector3 randomSpawn = Vector3.zero;
					randomSpawn.Set(Random.Range(-SpawnForce.x,SpawnForce.x), 
					                Random.Range(-SpawnForce.y,SpawnForce.y),
					                Random.Range(-SpawnForce.z,SpawnForce.z));
					finalSpawnForce = Vector3.zero;
					//randomSpawn.Normalize();
					finalSpawnForce = randomSpawn;
					
				}
				else{
					finalSpawnForce = SpawnForce;
				}
				
				b.GetComponent<Rigidbody>().AddForce(finalSpawnForce, ForceMode.Force);
				
				counter = 0.0f;
				spawnCounter++;
				if (spawnCounter > SpawnInitialCount){
					Wait = WaitAfterInitial;
				}
			}
			
			counter += Time.deltaTime;

			return true;
		}

		else{
			return false;
		}
		

	}
}
