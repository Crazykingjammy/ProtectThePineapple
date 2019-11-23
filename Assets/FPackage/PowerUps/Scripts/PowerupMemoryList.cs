using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class PowerupMemoryList {
	
	// this list will know its type
	// so any object in this list is the same type = defaulting to 0
	PowerupFactory.PowerUpsDirectoryType type = PowerupFactory.PowerUpsDirectoryType.LifeAuraCC;

	
	/*
	 * 
	 * This object has a list of objects all of the same type
	 *  For now, this will be used for only powerup Types
	 */
	public List<PowerupBase> listOfRootType = null;
		
	public PowerupMemoryList(){
		Init();
	}
	
	~PowerupMemoryList(){
		DeInit();
	}
	
	public void Init(){
		listOfRootType = new List<PowerupBase>();	
	}
	
	private void DeInit(){
		// Am I all Cleared?
		listOfRootType = null;	
	}
	// Use this for initialization
	void Start () {
		// allocate the list
		Init();
	}
	

	
	// Update is called once per frame
	void Update () {
//		for (int i = 0; i < listOfRootType.Count; i++){
//				PowerupBase newPU = listOfRootType[i];
//			}
	}
	
	public void CreateMemObjects(int count, PowerupBase prefab, Transform parent){
			for (int i = 0; i < count; i++){
				PowerupBase newPU = MonoBehaviour.Instantiate(prefab, Vector3.zero, Quaternion.identity) as PowerupBase;
				
				// makes sure the Powerup Starts in active
				newPU.gameObject.SetActive(false);

				// set its type and parent
				newPU.type = prefab.type;
				
				newPU.transform.parent = parent;
			
			
				listOfRootType.Add(newPU);
				//objectListMemTypes[i].AddMemObject(newPU)/
			}
	}
	
	public void AddMemObject(PowerupBase obj){
		// assuming the new object already has its type set
		// and all other default values
		if (obj){
			listOfRootType.Add(obj);
		}
	    // print ("MemoryList " + listOfRootType.Count);
	}
	
	public PowerupBase GetNext(){
		
		// pop the next one
		// TODO: if we have none we have to make one or tell the factory to.
		PowerupBase nextPU = null;
		for (int i = 0; i < listOfRootType.Count; i++){
			nextPU = listOfRootType[i];
			if (nextPU.IsBackAtSpawner()) return nextPU;
		}
//		if (listOfRootType.Count > 0){
//			System.Predicate<PowerupRoot> puPredicate;
//			//puPredicate = new System.Predicate<PowerupRoot>();
//			nextPU = listOfRootType[0];
//		}
		return null;
	}
	
	new public PowerupFactory.PowerUpsDirectoryType GetType(){
		return type;
	}
	public void SetType(PowerupFactory.PowerUpsDirectoryType newType){
		type = newType;
	}
}
