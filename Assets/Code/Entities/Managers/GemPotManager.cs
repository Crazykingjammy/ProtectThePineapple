using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemPotManager : MonoBehaviour {
	
	public Vector3 force;
	
	public Material Gem, Dud, Coin;
	
	//List of gems in the scene.
	public List<Gem> GemStack;
	
	public Gem MasterGemCopy;
	
	//Limit the amount of gems that can be cloned. 
	public int GemLimit = 50;
	public int SilverLimit = 15;
	public int GoldLimit = 100;
	public int BlueLimit = 250;
	
	public bool PreAllocate = true;
	
	
	int prevcolor = 0;

	// Use this for initialization
	void Start () {
		
		//Set the master flag so it dosnt die. 
		//MasterGemCopy.gameObject.active = false;
		
		//Go ahead and allocated all the gems if thats what we want to do.
		if(PreAllocate)
		{
			for(int i = 0; i < GemLimit; i++)
			{
				Gem g = Instantiate(MasterGemCopy) as Gem;
				g.transform.parent = transform;
				
				//g.renderer.material.color = RandomColor();
				g.GemActive = false;
				g.Host = this;
				
				GemStack.Add(g);
			}
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//Clear the list
//		if(Input.GetKeyDown(KeyCode.R) )
//		{
//			GemStack.RemoveAll(item => item == null);
//		}
		
		
		int combo = GameObjectTracker.instance.GetMultiplier();
		
		if(combo > 0)
		{
			
			//Default
			Coin.color = Color.white;
			
			
			//Make gold after given amount.
			if(combo > SilverLimit)
			Coin.color = Color.yellow;
			
			//Make gold after given amount.
			if(combo > GoldLimit)
			Coin.color = Color.blue;
			
			//Make gold after given amount.
			if(combo > BlueLimit)
			Coin.color = Color.green;
		}
//		else
//		{
//			MasterGemCopy.renderer.sharedMaterial.color = Color.black;
//		}	
//			
		
		if(combo < 1)
			MasterGemCopy.GetComponent<Renderer>().sharedMaterial.color = Color.black;
		
	}
	
	public void Drop(int count,Vector3 position, int worth)
	{
		//SpawnGems(count, position, worth);
		DropGem(count,position);

	}
	
	void DropGem(int count, Vector3 pos)
	{
		int i = 0; 
		
		foreach(Gem g in GemStack)
		{
			if(!g.GemActive)
			{
				g.GemActive = true;
				g.transform.position = pos;
				
			//	g.renderer.material.color = RandomColor();
				
				MasterGemCopy.GetComponent<Renderer>().sharedMaterial.color = Color.red;
				
				//Create random force. 
				g.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 50.0f, ForceMode.Impulse);
			
				i++;
			}
			
			//Loop breaking condition
			if(i == count)
				return;
		}
		
		
		//MasterGemCopy.renderer.sharedMaterial.color = RandomColor();
		
		
	}
	
	void SpawnGems(int amount, Vector3 position, int worth = 1)
	{
		
		
		if(GemStack.Count > GemLimit)
		{
			CleanUpList();
			return;
		}
		
		
		//Adjust the amount if we are up to the limit.
		int localamount = amount; 
		
		//If we are asking for more than 50 gems we reduce the amount by 10.
		if(amount > GemLimit )
		{
			localamount = amount / 10;
		}
		
		for(int i = 0; i < localamount; i++)
		{
			//Create the object.
			Gem g = Instantiate(MasterGemCopy,position,Quaternion.identity) as Gem;
		
			//Set the random color.
			//g.renderer.material.color = RandomColor();
			g.GetComponent<Renderer>().sharedMaterial.color = RandomColor();
			
			//Set the value.
			g.value = worth;
			
			//If amount is more than 50 we edit the values and color
			if(amount > GemLimit)
			{
				//g.renderer.material.color = Color.black;
				g.GetComponent<Renderer>().sharedMaterial.color = Color.black;
				g.value *= 20;
			}
			
			
			//Create random force. 
			g.GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * 50.0f, ForceMode.Impulse);
			
			//Set Child
			g.transform.parent = transform;
			
			//Add to the stack.
			GemStack.Add(g);
			
		}
		
	}
	
	
	Color RandomColor()
	{
			//Randomize color
		
			int colorindex = Random.Range(0,7);
		
		
		
		
			Color gemcolor;
			switch (colorindex)
			{
				
			case 0:
				gemcolor = Color.cyan;
				break;
				
			case 1:
				gemcolor = Color.red;
				break;
			case 2:
				gemcolor = Color.green;
				break;
			case 3:
				gemcolor = Color.yellow;
				break;
			case 4:
				gemcolor = Color.magenta;
				break;
			case 5:
				gemcolor = Color.blue;
				break;
			default:
				gemcolor = Color.white;
				break;
				
			}
		
		prevcolor = colorindex;
		
		return gemcolor;
	}
	
	
	
	public int CleanUpList()
	{
		//Clear the list.
		GemStack.RemoveAll(item => item == null);
		
		///Return the new count
		return GemStack.Count;
	}
}
