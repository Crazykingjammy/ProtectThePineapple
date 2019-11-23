using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityFactory : MonoBehaviour {
	
	static private EntityFactory EF;
	
	
	//This Class keeps track of all the different types of Enemies
	public List<Target> TargetCatalogue;
	public List<PooledObject<Target>> TargetPool;
	
	List<Target> ActiveTargetList;
	
	//Keeps Track of all the different types of Cannons
	public List<Cannon> CannonCatalogue;
	public List<PooledObject<Cannon>> CannonPool;
	
	List<Cannon> ActiveCannonList;
	
	//Keeps track of all the Items 
	public List<GameObject> ItemCatalogue;
	
	public PowerupFactory PUF;

	// to show in unity editor
	public TargetTypes type = TargetTypes.StandardShooterBot;

	public enum TargetTypes
	{
		RocketShipS1 = 0,
		RocketShipD2,
		RocketShipD1,
		StandardShooterCC,
		RocketShipO1,
		StandardShooterBot,
		DarkShooterBot,
		MiniRocketShipS1,
		HeavyShooterBot,
		MiniStandardTankBot,
		DarkTankBot,
		ParasiteEnemy,
		StandardTankBot,
		ArmorTank,
		HeavyShooterCC,
		StandardTankCC,
		StandardCrate,
		BigCrate,
		StandardGravityCrate,
		StandardBombCrate,
		GuardedEyeball,
		ZepplinShipS1,
		BallEaterEnemy,
		LauncherEnemy,
		TickEnemyS1,
		MiniStandardTankAttackingCC,
		TickEnemyS2,
		NULL,
		Empty
		
	}
	
	public enum CannonTypes
	{
		Fox = 0,
		SnakeCannon,
		Gorilla,
		Zebra,
		Wolf,
		Eagle,
		PolarBear,
		BumbleBee,
		Unicorn,
		EnemyGorilla,
		Squid,
		Tiger,
		Dolphin,
		Dragon,
		Turtle,
		NULL,
		Empty
	}
	
	public enum ItemTypes
	{
		Crate = 0,
		ItemBox
	}
	
	
	//Count for pre allocations
	public int PreAllocatedTargetCount = 5;
	
	
	static public EntityFactory GetEF()
	{
		if(EF)
		{
			return EF;
		}
		
		return null;
		
	}
	
	// Use this for initialization
	void Awake () {
		
		//print("EF START!!!");
	
		EF = this;
		
		//Dont destroy me.
		DontDestroyOnLoad(this.gameObject);
		
		//Create the target pool.
		TargetPool = new List<PooledObject<Target>>();
		
		//Create the list for active targets
		ActiveTargetList = new List<Target>();
		
		//Create list for active cannons
		ActiveCannonList = new List<Cannon>();
		
		//Create the cannon pool.
		CannonPool = new List<PooledObject<Cannon>>();
		
		//Create a pool for each item in each list we want to pool.
		int TargetTypeIndex = 0;
		
		//Creating the pools for each targets.
		foreach(Target t in TargetCatalogue)
		{
			PooledObject<Target> pT = new PooledObject<Target>();
			
			//Add 3 of each to start off with.
			for(int i = 0; i < PreAllocatedTargetCount; i++)
			{
				Target temp = Instantiate(t,Vector3.zero,Quaternion.identity) as Target;
				
				//Set the target ID
				temp.ID = (TargetTypes)TargetTypeIndex;
				
				temp.transform.parent = transform;
				
				//Add to the pool.
				pT.AddItem(temp);
				
				temp.ForceDisable();
				
			}
			
			//Add the pooled object to the target pool.
			TargetPool.Add(pT);
			
			TargetTypeIndex++;
		}
		
		
		foreach(Cannon c in CannonCatalogue)
		{
			//Create a pool per cannon type.
			PooledObject<Cannon> pC = new PooledObject<Cannon>();
			
			//Add the pooled object to the cannon pool ist.
			CannonPool.Add(pC);
			
			
		}
		
		PUF = Instantiate(PUF) as PowerupFactory;
		PUF.transform.parent = transform;
		
		
	}
	
	//Reset call to clear all active entities.
	public void Reset()
	{
		//temp list so we can hold all the active targets at the time the reset function was called.
		List <Target> rstActiveList = new List<Target>();
		List<Cannon> rstActiveCannonList = new List<Cannon>();
		
		//Go through the lists and deactivate all entities.
		foreach(Target t in ActiveTargetList)
		{
			rstActiveList.Add(t);
		}
		
		//Do the same for cannons
		foreach(Cannon c in ActiveCannonList)
		{
			rstActiveCannonList.Add(c);
		}
		
		//Now that the reset list is filled up, we can go through and disable.
		foreach(Target t in rstActiveList)
		{
			t.ForceDisable();
		}
		
		//And same with cannon.
		foreach(Cannon c in rstActiveCannonList)
		{
			c.ForceReturn();
		}
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ReturnCannon(Cannon rCannon)
	{
		//Remove from the active list
		ActiveCannonList.Remove(rCannon);
		
		//Find out the type and return it to the pool.
		CannonPool[(int)rCannon.CannonTypeInfo].ReturnItem(rCannon);
	}
	
	public void ReturnTarget(Target rTarget)
	{
		
		//Remove from the active list!
		ActiveTargetList.Remove(rTarget);
		
		//Find out the type and return it to the pool.
		TargetPool[(int)rTarget.ID].ReturnItem(rTarget);
		
		//Disable the object.
	//	rTarget.gameObject.SetActive(false);
	}




	public Target CreateTarget(TargetTypes type, Vector3 position)
	{
		//Simply create a target and return it. Let whoever gets it deal with it as they wish.
		//Target t = Instantiate(TargetCatalogue[(int)type], position, Quaternion.identity) as Target;
		
		//If we have a target available in the pool return it.
		Target t = TargetPool[(int)type].GetItem(); 
		
		//If the item didnt exist.
		if(t == null)
		{
			//Add the item and ask for it again.
			t = Instantiate(TargetCatalogue[(int)type], position, Quaternion.identity) as Target;
			
			//Set the target ID
			t.ID = type;
			
			//Make a child?
			t.transform.parent = transform;
			
			TargetPool[(int)type].AddItem(t);
			
			//Should return the item now that we added one.
			t = TargetPool[(int)type].GetItem(); 
		}
		
		//Now we set the position.
		t.transform.position = position;
		t.transform.rotation = Quaternion.identity;
		
		//Activate the object.
		t.gameObject.SetActive(true);
		
		//Add to the active list!
		ActiveTargetList.Add(t);
		
		//Return the temp storage incase we need to make our own modifications.
		return t;
	}

	public string GetCannonIconName(CannonTypes type)
	{
		return CannonCatalogue[(int)type].CannonTextureName;
	}

	public Cannon CreateCannon(CannonTypes type, Vector3 position)
	{
	
		//Simply create a cannon and return it. Let whoever gets it deal with it as they wish.
		//Cannon c = Instantiate(CannonCatalogue[(int)type],position,Quaternion.identity) as Cannon;
		
		Cannon c = CannonPool[(int)type].GetItem();
		
		//If the cannon didnt exist then we create it.
		if(c == null)
		{
			c = Instantiate(CannonCatalogue[(int)type],position,Quaternion.identity) as Cannon;

			//Set the type for the cannon.
			c.CannonTypeInfo = type;
			
			//Make a child?
			c.transform.parent = transform;
			
			//Add to the pool list.
			CannonPool[(int)type].AddItem(c);
			
			//We should return now should we?
			c = CannonPool[(int)type].GetItem();
			
		}
		
		//Now we set the position
		c.transform.position = position;
		c.transform.rotation = Quaternion.identity;
		
		//Activet the object.
		c.gameObject.SetActive(true);

		//Set not dirty
		c.SetDirty(false);
		
		//Add to the active list
		ActiveCannonList.Add(c);
		
		//Return the temp storage incase we need to make our own modificaions
		return c;
	}
	
	public GameObject CreateItem(ItemTypes type, Vector3 position)
	{
		//Create the game object from the list.
		GameObject g = Instantiate(ItemCatalogue[(int)type],position,Quaternion.identity) as GameObject;
		
		//Return it
		return g;
	}
}
