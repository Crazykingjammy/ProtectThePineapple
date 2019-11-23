using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Grid cannon list script.
/// 
/// This object is created under a parent Activity
/// 
/// Grabs the Cannon Catalog from the Entity factory
/// and locally stores it
/// 
/// Is also responsible for filling up the UIGrid for all the cannon select objects
/// 
/// TODO: This will change to the use ot item Cards that kevaad built
/// 
/// </summary>
public class FUICannonListManager : MonoBehaviour {
	
	// the window is going to populate the grid of slots
	public UIGrid _gridOfCannons = null;
	
	// this is the prefab to populate the grid
	public CannonSelectionObject _refPrefab = null;
	

	
	
	// this is the list contained internally for all the selectable cannons
	List<CannonSelectionObject> _CardSelectors = null;

	
	
	// this is the list returned from the entity factory
	//List<Cannon> allCannonsInCatalogue;
	
	// Going to get the list of item cards from player data.  these are all the cannons to show as item cards
	// instead of a cannon object
	BaseItemCard [] allItemCards;
	// have we populated our list?  when run in the scene, this starts before the Toybox is made.
	bool isPopulated = false;
	
	// Use this for initialization
	void Awake () {
		
		_CardSelectors = new List<CannonSelectionObject>();
		PopulateCannonsList();
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(ActivityManager.Instance == null)
			return;
		
			if (!isPopulated){
			PopulateCannonsList();
		}
		
//		//Check for selected cannon
//		foreach(CannonSelectionObject card in _CardSelectors)
//		{
//			if(SelectorObject.bounds.Intersects(card.Senser.bounds))
//			{
//				//Set the selected cannon.
//				ActivityManager.Instance.SelectedCard = card.MyCannonCard;
//				
//				//Set the title
//				SelectedCannonTitle.text = card.MyCannonCard.Label;
//				
//				if(SelectedCannonTitle.text != previousTitle)
//				{
//					previousTitle = SelectedCannonTitle.text;
//					TitleTween.Reset();
//					TitleTween.Play(true);	
//					
//					//Play the sound
//					AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Tap);
//				}
//				
//				
//				
//			}
//		}
//		
	}
	
	
	public void Refresh()
	{
		//here we will refresh the list of cards.
		
		//All the cards should be created on load so no need to go and recreate. just go and reassign.
		foreach(CannonSelectionObject card in _CardSelectors)
		{
			//Just reassign. It will handle the refresh.
			card.MyCannonCard = card.MyCannonCard;
			
		}
	}
	
	public void PopulateCannonsList(){
		// We have the activity.  Populate the grid with Cannons!!		
		EntityFactory ef = EntityFactory.GetEF();
		if (ef == null){
			return;
		}
		
		// Getting ItemCards from Playerdata to populate list based on item cards not the entity factory calalog
		GameObjectTracker go = GameObjectTracker.GetGOT();
		if (go == null){
			return;
		}
		
		if(_CardSelectors == null)
			return;
		
		// keeping track of the list of cannon selection objects locally
		
		// the next object to temp strore data for the list
		CannonSelectionObject newCannon = null;
		
		
		// Grab the deck of cards from the player data
		allItemCards = go._PlayerData.CardDeck;
		
		int cannonItemCardIndex = 0;
		foreach(CannonItemCard canItem in allItemCards){
			
			
			//Create cannon and assign the reference.
			newCannon = NGUITools.AddChild(_gridOfCannons.gameObject, _refPrefab.gameObject).GetComponent<CannonSelectionObject>();
			newCannon.MyCannonCard = canItem;

			//Set the checkbox root.
			UICheckbox cbox = newCannon.GetComponent<UICheckbox>();
			if(cbox)
			{
				cbox.radioButtonRoot = this.transform;
			}
			
			_gridOfCannons.Reposition();
			
			_CardSelectors.Add(newCannon);
			
			cannonItemCardIndex++;
		}		
		
		//Is populated is true,
		isPopulated = true;
		
		//And list give the list to the activity manager so they can grab it.
		ActivityManager.Instance.CardSelectors = _CardSelectors;
	}
	
	
}
