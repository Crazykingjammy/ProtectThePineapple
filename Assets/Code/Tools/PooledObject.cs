using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PooledObject<T> where T: class {
	
	
	//Number for the initial instantuated amount
	public int StartingSize = 3;
	
	//Entity to pool.
	//public T original;
	
	//Here we store the list of objects in the pool.
	public List<T> AvailableObjectPool;
	public List<T> UsedObjectPool;
	
	//Constructor
	public PooledObject()
	{
		//Create the two lists.
		AvailableObjectPool = new List<T>();
		UsedObjectPool = new List<T>();
		
	}
	
	
	public void AddItem(T item)
	{
		//Add item to the available pool list.
		AvailableObjectPool.Add(item);
	}
	
	public T GetItem()
	{
		//Go through and see if there is a list of available 
		if(AvailableObjectPool.Count > 0)
		{
			//Grab the item
			T item = AvailableObjectPool[0];
			
			//Remove from the available list.
			AvailableObjectPool.Remove(item);
			
			//Add to the in use list.
			UsedObjectPool.Add(item);
			
			//Grab form the top and return
			return  item;
		}
		
		//Return null if we dont have an item.
		return null;
		
	}
	
	public void ReturnItem(T rItem)
	{
		//We shall check if teh item return is in the used pool. 
		//We will only handle items already in the pool and disguard the rest.
		if(UsedObjectPool.Contains(rItem) )
		{
			//Add to the available pool here.
			AvailableObjectPool.Add(rItem);
			
			//And remove from the used pool list.
			UsedObjectPool.Remove(rItem);
		}
		
		
		//here if the object isnt in the pool, so far we wont do anything at all.
		
	}
	
//	public void ReturnAll()
//	{
//		//Go through and return all items in the list.
//		foreach(T item in UsedObjectPool)
//		{
//			AvailableObjectPool.Add
//		}
//	}
//	
	//Returns the count of total objects both in use and available
	public int TotalObjects
	{
		get 
		{
			return (AvailableObjectPool.Count + UsedObjectPool.Count);
			
		}
	}
}
