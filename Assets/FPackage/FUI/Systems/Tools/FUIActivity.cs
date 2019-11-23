using UnityEngine;
using System.Collections;

/// <summary>
/// FUI activity.
/// 
/// This Activity object will be a manager for certain tasks in the UI
/// 
/// Will manage the various panels and windows to get the job done.
/// </summary>

public class FUIActivity : FUIEnablerScript {
	
	// This is a base Activity class
	// the goal is to have other activity objects derive off of this api
	// for now, this base activity will be used to process the activity for selecting a 
	// cannon
	
	public AudioClip ActivityTune = null;
	
	public FUIWindowToggles[] _windows;
	
	
	// the animation of starting and ending the activity
	TweenTransform _myTweenTransform = null;
	
	// we need a way to know what "state" we're in.  Wheter the primary or secondary panel is open
	bool _isActive = false;
	
	/// <summary>
	/// Keeping track of the slots and cannon selection
	/// </summary>
	// when a slot is triggered to select a new cannon.  Its 
	// going to tell the activity who is making the call.
	//SlotSelectionObject _slotObject = null;  
	
	public bool Activated
	{
		get{
			return _isActive;
		}
	}
	
	// Use this for initialization
	new protected void Start () {
		base.Start();
		// just get my tween animation
		_myTweenTransform = GetComponent<TweenTransform>();
	}
	
	/// <summary>
	/// This is called from any button that wants to start the slot and cannon select
	/// Toggle the activity.  first called when a button or trigger wants this activity started
	/// 
	/// This will turn on and start the activity if it is off.
	/// Turn off when its on.
	/// </summary>
	public bool ToggleActivity(){		
		// we maynot be tweened open yet.  activate the primary, and fully open with the tween
		if (_isActive){ 
			this.TogglePanel(true);
			
			foreach(FUIWindowToggles window in _windows)
			{
				window.ToggleWindowOn();
			}
			
		}
		else{
			
			foreach(FUIWindowToggles window in _windows)
			{
				window.ToggleWindowOff();
			}
			
		}
		
		if (_myTweenTransform){
			_myTweenTransform.Play(_isActive);
		}
		
		// not necessarily on yet if on.  has to animate before the flag is set.
		return _isActive;
	}
	
	/// <summary>
	/// Toggles the activity.
	/// </summary>
	/// <returns>
	/// The activity if active
	/// </returns>
	/// Takes in the transform to start the tween from

	public bool ToggleActivity(Transform t){
		// set my from transform to the gameobject that toggled me
		_myTweenTransform.from = t;
		
		// then toggle the activity
		return ToggleActivity();		
	}
	
	/// <summary>
	/// Called when the tween animation is done.
	/// 
	/// Toggles the correct panel to display during the state.
	/// ONLY CALLED BY THE TWEEN TRANSFORM when its done animating
	/// 
	/// </summary>
	public void TweenDone(){
		
		//Debug.LogError("Tween DONE!");
		
		// the issue with the tween.  We want to be active during the animation tween
		// if we're first toggling from off, the ToggleActivity toggles on the Activity
		// when tween is done, just set the state.
		//
		// If toggling off, keep the panel open, do tween animation and when animation is 
		if(_isActive == false)
		{
			this.TogglePanel(false);
		}
	}
	
	/// <summary>
	/// Starts the activity with the primary panel on and any other panel off
	/// </summary>
	public void ActivityOn(){
		// when we want to activate this activity, just enable the gameobject
		_isActive = true;
		ToggleActivity();
		
		OnActivate();
	}
	
	/// <summary>
	/// Shuts down all the panels.
	/// </summary>
	public void ActivityOff(){
		// make sure everything is off
		_isActive = false;
		ToggleActivity();
		
		
		OnDeActivate();
		
	}
	
	public virtual void OnActivate() { Debug.Log("Base Fui Activity Activate for: " + this.name); }
	public virtual void OnDeActivate() { Debug.Log("Base Fui Activity Deactivate"); }
	public virtual void OnReset() { Debug.Log("Base Fui Activity Reset"); }
	
	
	
	
	
	
	
	
	
	
	
//	/// <summary>
//	/// Starts the cannon select. Panel
//	/// </summary>
//	/// <param name='slotObject'>
//	/// Cannon slot Object.
//	/// </param>
//	public void StartCannonSelect(SlotSelectionObject slotObject){
//		
////		// cache the slot object that started the cannon select
////		//_slotObject = slotObject;
////		
////		// toggle off the slots panel and enable the cannon select panel
////		//_windowSlots.ToggleWindowOff();
////		//_windowCannons.transform.position = _slotObject.transform.position;
////		//_windowCannons.ToggleWindowOn();
////		
////		// basically now waiting for the cannon select panel to apply the new cannon for the slot.
//		
//		
//	}
//	
//	public void ApplyCannonSelect(CannonSelectionObject cannonObject){
////		
////		// a cannon was selected 
////		
////		// then tell the toybox the new cannon is selected
////		// EntityFactory ef = EntityFactory.GetEF();
////		// TODO: fix this.  there must be a more elegant way of setting the new cannon slot index.
////		//if (_slotObject.SlotIndex == 0){
////		//	Debug.Log("Setting Slot: " + _slotObject.SlotIndex +" 1: Cannon idex " + cannonObject.CannonTypeIndex + "  !!!***");
//////			GameObjectTracker.GetGOT()._PlayerData.Breathless.CannonSlotA = (EntityFactory.CannonTypes)cannonObject.CannonTypeIndex;
////			
////		//}
////		//if (_slotObject.SlotIndex == 1){
////		//	Debug.Log("Setting Slot: " + _slotObject.SlotIndex +" 1: Cannon idex " + cannonObject.CannonTypeIndex + "  !!!***");
//////			GameObjectTracker.GetGOT()._PlayerData.Breathless.CannonSlotB = (EntityFactory.CannonTypes)cannonObject.CannonTypeIndex;
////		//}
////		
////		ToyBox tb = ToyBox.GetPandora();
////		
////		if (tb)
////		{
////		//	Debug.Log("we have a TB");
////			//tb.ProcessCannonSlots();
////			//tb.AssignCannonSlotTypes();
////			//tb.ClearFreeCannons();
////			//tb.TriggerCannonSpawns();
////		}
////		
////		// the toybox should now update the Cannon Slot A and B Get the info for the UI
////		
////		// change the sprite and label of the slot
//////		if (_slotObject.SlotIndex == 0){
//////			_slotObject._sprIcon.spriteName = tb.CannonSlotA.CannonTextureName;
//////			_slotObject._lblSlotName.text = tb.CannonSlotA.CannonName;
//////			
//////			
//////		}
//////		if (_slotObject.SlotIndex == 1){
//////			_slotObject._sprIcon.spriteName = tb.CannonSlotB.CannonTextureName;
//////			_slotObject._lblSlotName.text = tb.CannonSlotB.CannonName;		
//////		}
//////		
////		// since the Cannon slots is broken, we're just going to set the 
////		// slot icon and text to the selected cannon object sprite and text
////		
////		/***
////		 * The cannon pressing no longer selects the cannon for the slot.
////		 * Instead when the cannon is "Selected" pressing the slot button 
////		 * will equip the cannon.
////		 * 
////		 */
////		
//////		_slotObject._sprIcon.spriteName = cannonObject._sprIcon.spriteName;
//////		_slotObject._lblSlotName.text = cannonObject._lblCannonName.text;
////		
//////		GridSlotsListScript grid =  _windowSlots.GetComponent<GridSlotsListScript>();
//////		if (grid){ grid._gridOfSlots.Reposition();}
////		
////		// then toggle the windows, return to slot select.  New cannon should be visible
////		
////
////		// this is going to come from the cannon select apply button.
////		// deactivate the cannon select panel
////
////		//_windowCannons.ToggleWindowOff();
////		//_windowSlots.ToggleWindowOn();
////		
////		// tell the slot that either a new cannon was selected or no change		
//		
//		
//	}
//	
//	public void Update(){
////		if (!_isActive){
////			// make sure everything is off
////			_windowSlots.ToggleWindowOff();
////			_windowCannons.ToggleWindowOff();
////		}
//	}
}
