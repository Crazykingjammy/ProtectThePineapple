using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TodemEnemy : BaseEnemy {
	
	public bool SpawnCaptured = true;
	
	List<TargetSpawner> _capturedTargets;


	// Use this for initialization
	protected override void Start () {		
		base.Start();
	
		//First create the list.
		_capturedTargets = new List<TargetSpawner>();
		
		//Grab any spawners we may have.
		foreach(Transform t in transform)
		{
			TargetSpawner tspt = t.GetComponent<TargetSpawner>();
			
			if(tspt)
			{
				_capturedTargets.Add(tspt);	
			}
			
		}
		
		//Set the captured state.
		Captured = SpawnCaptured;
		
		Debug.Log("We have " + _capturedTargets.Count + " spawners in our Todem");
		
	}
	
	protected override void Reset()
	{
		base.Reset();
		
		//Get rid of the cannon.
		
		//Make sure the renderer is enabled.
		GetComponent<Renderer>().enabled = true;
		
		//And set our captured state again.
		Captured = SpawnCaptured;
		
		
	}
	
	
	// Update is called once per frame
	public override void FixedUpdate () {
		
		//The core baisc stuff here on top of the update function.
		base.FixedUpdate();
			
	
		foreach(TargetSpawner ts in _capturedTargets)
		{
			//Here we check if all targets are finished, returning if one isnt finished yet.
			if(!ts.IsFinished())
				return;
			
			//here we should be if all targets finished and lets set outselves to be vurnable again.
			Captured = false;
			
		}
		
		
		if(Destroyed)
		{
			//Hide our mesh
			GetComponent<Renderer>().enabled = false;
		}
		
		
		
		//lets handle aim.
		HandleAim();
	}
	
	
		void OnCollisionEnter(Collision col)
	{
		

				//If we hit some walls
		if(col.gameObject.CompareTag("Wall") && MomentumDamage)
		{
			if(GetComponent<Rigidbody>().velocity.magnitude > 10.0f)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
				//Deal the damage
				Damage(TargetMomentumDamage);
			}
			
		}
		
		//If we hit some walls
		if(col.gameObject.CompareTag("Target"))
		{
		if(GetComponent<Rigidbody>().velocity.magnitude > 10.0f)
			{
				//print("BOOM!");
				
				//Play sound
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
				//Deal the damage
				Damage(SelfMomentumDamage);
				
				//Get the ball we collided with.
				Target t = col.gameObject.GetComponent<Target>();
				t.Damage(TargetMomentumDamage);
				
			}
		
		}
		
		
		///If we hit a ball it should turn red now.
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();
			
		
			//Only look for active balls!
			if(b.IsActive() && b.GetBallSourceID() != Ball.BallSourceID.Netural)
			{
				//Damage
				Damage(b.DamageAmount);
				
				//audio.clip = HitSoundEffect;
				AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
				
				if(b.GetBallSourceID() == Ball.BallSourceID.Bot)
				{
					//Send the message. Its about sending the message
					GameObjectTracker.GetGOT().TargetHit();	
				}
				
				
				//Pop the ball if it isnt shoot through.
				if(!b.IsShootThrough())
				{
					b.Pop();
				}
				
			}
			
		}
		
		
		
		
		
	}//OnCollisionEnter()
}
