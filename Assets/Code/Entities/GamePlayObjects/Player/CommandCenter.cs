using UnityEngine;
using System.Collections;

public class CommandCenter : Target {
	
	public Material HealthMaterial;
	public AudioClip WallHitSound;
	
	public string DeathObjectName;


	float startHealth;
	
	public CannonSpawner[] CannonSlots;

	private BallStack gamesBallStack = null;
	
	
	protected override void Start()
	{
		// Make sure to call parents base.
		base.Start();
		
		//Set the skull object to invisible.
		GameObject skull = transform.Find(DeathObjectName).gameObject;
		skull.GetComponent<Renderer>().enabled = false;

		GameObject ballspawner = transform.Find("BallSpawner").gameObject;

		startHealth = Health;
		gamesBallStack = ToyBox.GetPandora().SceneBallStack;

		if (gamesBallStack == null){
			Debug.LogError("No Ball Stack for CC");
		}
		else
		{
			//gamesBallStack.transform.parent = transform;
			gamesBallStack.transform.position = ballspawner.transform.position;
		}

		//Rset at start
		Reset();

	}

	new public void Update(){

		base.Update();
		UpdateColor();

		gamesBallStack.SpawnBall();
	}

	// Update is called once per frame
	void FixedUpdate () {
	
	
		
		
	}

	new public void Reset()
	{
		Health = startHealth;

		//clear all slots.
		foreach(CannonSpawner spwn in CannonSlots)
		{
			spwn.KillSpawn();
			spwn.type = EntityFactory.CannonTypes.NULL;
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		
		///If we hit a ball it should turn red now.
		if(col.gameObject.CompareTag("Ball"))
		{
			//Get the ball we collided with.
			Ball b = col.gameObject.GetComponent<Ball>();
			
		
			//Only look for active balls!
			if(b.IsActive() && b.GetBallSourceID() == Ball.BallSourceID.Enemy)
			{
				//Damage
				Damage(b.DamageAmount);
				
				if(HitSoundEffect)
					AudioPlayer.GetPlayer().PlaySound(HitSoundEffect);
								
				//Pop the ball if it isnt shoot through.
				if(!b.IsShootThrough())
				{
					b.Pop();
				}
				
			}
			
		}
		
		
		if(col.gameObject.CompareTag("Wall"))
		{

			AudioPlayer.GetPlayer().PlaySound(WallHitSound);
		}
	}
	
	
	
	
	void UpdateColor()
	{
		Color c = Color.white;
		
		//Create a clamped value of the health to color ratio.
		float health2color = (Health/MaxHealth);
		
		c.b = health2color;
		c.g = health2color;
	
		HealthMaterial.color = c;
		
	}
	
	
	public GameObject GetDeathObject()
	{
		GameObject d = transform.Find(DeathObjectName).gameObject;
		
		if(d)
		{
			d.GetComponent<Renderer>().enabled = true;
			return d;
		}
		
		
		return null;
	}
	
	
	
	public bool SetCannonSlot(EntityFactory.CannonTypes type, int index)
	{
		
		//If we are trying to apply the same cannon just return false;
		if(CannonSlots[index].type == type)
			return false;
		
		
		//Assign the type.
		CannonSlots[index].type = type;
		
		//Clear out the slot
		ClearCannonSlot(index);
		
		//Slot is set so return true.
		return true;	
	

	}

	public bool SetItemSlot(ItemSlot pSlot,int index )
	{
		if(pSlot.Type != EntityFactory.CannonTypes.NULL)
		{
			//active and set teh slot type.
			//CannonSlots[index].gameObject.SetActive(true);
			CannonSlots[index].type = pSlot.Type;

			return true;
		}

		//Esle we deactivate slots we wont be using cannons for.
		//CannonSlots[index].gameObject.SetActive(false);
		CannonSlots[index].type = EntityFactory.CannonTypes.NULL;
	
		return false;
	}
	
	
	void SpawnedCannon(Cannon cannon)
	{
		print("You Spawned a: " + cannon.name);
	}


	public void TriggerSlotSpawns()
	{
		//Trigger spawns in all the slots.
		foreach(CannonSpawner spwn in CannonSlots)
		{
			if(spwn.type == EntityFactory.CannonTypes.NULL)
				continue;

			spwn.TriggerSpawn();
		}
	}


	public Cannon TriggerCannonSpawn(int slot = 0)
	{
		//Create 
		Cannon c = null;
	
		//Create the cannon/
		c = CannonSlots[slot].TriggerSpawn();
		
	
		return c;
	}
	
	public bool ClearCannonSlot(int index = 0)
	{
		return CannonSlots[index].KillSpawn();
	}
	
}
