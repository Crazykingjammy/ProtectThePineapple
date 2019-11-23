using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridCannonListScript : MonoBehaviour {
	
	FUIActivity myActivity = null;
	
	// the window is going to populate the grid of slots
	public UIGrid _gridOfCannons = null;
	public CannonSelectionObject _refPrefab = null;
	List<CannonSelectionObject> _CannonList = null;	
	
	public List<Cannon> allCannonsInCatalogue;
	
	// have we populated our list?  when run in the scene, this starts before the Toybox is made.
	bool isPopulated = false;
	
	// Use this for initialization
	void Start () {
		// Make sure we're part of an activity
		if (myActivity == null){
			// try to get one....
			myActivity = this.gameObject.transform.parent.GetComponent<FUIActivity>();
			if (myActivity == null){ // if we sttiiill dont have one.. get out!
				return; 
			}
		}
		
		
		PopulateCannonsList();
		

	}
	
	// Update is called once per frame
	void Update () {
			if (!isPopulated){
			PopulateCannonsList();
		}
	}
	
	public void PopulateCannonsList(){
		// We have the activity.  Populate the grid with Cannons!!		
		EntityFactory ef = EntityFactory.GetEF();
		if (ef == null){
			return;
		}
		
		
		
		Debug.LogError("GRID CANNON LIST SCRIPT CALLED!");
		
		// keeping track of the list of cannon selection objects locally
		_CannonList = new List<CannonSelectionObject>();
		// the next object to allocate
		CannonSelectionObject newCannon = null;
		
		 allCannonsInCatalogue = ef.CannonCatalogue;
		
		int cannonCatalogeIndex = 0;
		foreach(Cannon can in allCannonsInCatalogue){
			newCannon = NGUITools.AddChild(_gridOfCannons.gameObject, _refPrefab.gameObject).GetComponent<CannonSelectionObject>();
			
			newCannon._sprIcon.spriteName = can.CannonTextureName;
			newCannon._lblCannonName.text = can.CannonName;
						
			_gridOfCannons.Reposition();
			
			_CannonList.Add(newCannon);
			
			cannonCatalogeIndex++;
		}
		
		isPopulated = true;
	}
	
}
