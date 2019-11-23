using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridSlotsListScript : MonoBehaviour {
	
	/// <summary>
	/// My activity. Always Assuming that the parent will be the activity
	/// </summary>
	
	// the window is going to populate the grid of slots
	public UIGrid _gridOfSlots = null;
	public SlotSelectionObject _refPrefab = null;
	List<SlotSelectionObject> _slotsList = null;

	public bool UseGameSlots = false;
	
	// we have the list of slots in the command center
	// cache them here
	private CannonSpawner[] _CannonSlots;
	
	// have we populated our list?  when run in the scene, this starts before the Toybox is made.
	bool isPopulated = false;
	
	
	// Use this for initialization
	void Start () {
				
		PopulateSlotsList();
		
		
	}

	void OnEnable()
	{
		//foreach(SlotSelectionObject slot in _slotsList)
			//slot.
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPopulated){
			PopulateSlotsList();
		}
		
	}
	
	// when a cannon changes
	public void CheckForCannonChanges(){
	}
	
	public void PopulateSlotsList(){
		
		if (isPopulated){
			return;
		}
		
		// get the Toybox to get info

		
		// we now have the command center with a list of slots, we should
		// iterate through all the slots set the visual data and add to the grid
		int cannonSlotIndex = 0;
		SlotSelectionObject newSlotObject = null;
		_slotsList = new List<SlotSelectionObject>();
		
		ItemSlot[] list = GameObjectTracker.instance._PlayerData.ItemSlots;

		if(UseGameSlots)
			list = GameObjectTracker.instance._PlayerData.GameSlots;

		foreach(ItemSlot iSlot in list){

			//if(iSlot.StartActive)
			//	iSlot.IsOpen = true;

			Debug.Log("Iterating through CannonSlots Spawners");
			newSlotObject = NGUITools.AddChild(_gridOfSlots.gameObject, _refPrefab.gameObject).GetComponent<SlotSelectionObject>();

			newSlotObject.SlotIndex = cannonSlotIndex;
			newSlotObject.MySlot = iSlot;

			cannonSlotIndex++;

			_gridOfSlots.Reposition();
			_slotsList.Add(newSlotObject);
			
		
		}
		
	
		
//		foreach(SlotSelectionObject slot in _slotsList)
//		{
//			slot.StoreTablePosition();
//		}
//		
		
		isPopulated = true;
		
		
		
		
		/*
		 * We're using the slots list in the command center
		 * 
		 * no more hard two slots from the toybox
		 * 
		 */
		// for now the Toybox is set to 2 cannon slots
//		Cannon slot1, slot2 = null;
//		slot1 = tb.CannonSlotA;
//		slot2 = tb.CannonSlotB;
//			
//		SlotSelectionObject newSlot = null;
//			
//		_slotsList = new List<SlotSelectionObject>();
//		
//		bool slot1done = false, slot2done = false;
//		
//		if (slot1 != null){
//			Debug.Log("We have slot 1");
//			newSlot = NGUITools.AddChild(_gridOfSlots.gameObject, _refPrefab.gameObject).GetComponent<SlotSelectionObject>();
//			newSlot._sprIcon.spriteName = slot1.CannonTextureName;
//			newSlot._lblSlotName.text = slot1.CannonName;
//			newSlot.MyActivity = myActivity;
//			newSlot.MyCannon = slot1;
//			newSlot.SlotIndex = 0;
//			
//			_gridOfSlots.Reposition();
//			
//			_slotsList.Add(newSlot);
//			slot1done = true;
//		}
//		
//		if (slot2 != null){
//			Debug.Log("We have slot 2");
//			newSlot = NGUITools.AddChild(_gridOfSlots.gameObject, _refPrefab.gameObject).GetComponent<SlotSelectionObject>();
//			newSlot._sprIcon.spriteName = slot2.CannonTextureName;
//			newSlot._lblSlotName.text = slot2.CannonName;
//			newSlot.MyActivity = myActivity;
//			newSlot.MyCannon = slot2;
//			newSlot.SlotIndex = 1;
//			
//			_gridOfSlots.Reposition();
//			
//			_slotsList.Add(newSlot);
//			
//			slot2done = true;
//		}		
//	
//		if (slot1done && slot2done){
//			isPopulated = true;
//		}
//		else{
//			Debug.LogError("NOOOOOO SLOTS FOUND!!!!");
//		}
	}
	
	
	
}
