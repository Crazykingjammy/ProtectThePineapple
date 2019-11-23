using UnityEngine;
using System.Collections;

/// <summary>
/// Slot selection object.
/// 
/// When this object is clicked
/// 
/// Disable either the parent Parent object or just disable the colider 
/// 
/// The Slot button opens up the cannon Select List and waits to be closed by the 
/// Cannon list to make a selection
/// 
/// When a selection is made, the SlotSectionObject then needs to tell the Game that a 
/// new Cannon was selected.  Either directly to the game or first to a slot manager
/// </summary>
public class SlotSelectionObject : MonoBehaviour {
	
	//public TweenPosition myanimation;
	
	// Select object is part of the Inventory Activity
	FUIActivity _myActivity = null;
	public FUIActivity MyActivity{
	get {
		return _myActivity;
		}
		set{
			_myActivity = value;
		}
	}
	

	public UILabel _lblSlotName = null;
	public UILabel _CostLabel = null;
	public UISprite _sprIcon = null;
	public TweenTransform SelectTween;

	public FUIWindowToggles BG, SlotInfo, SlotOptions;

	public TweenPosition DeSelectAnimation;

	public string EmptySlotLabel = "Remove Slot";

	//Private internal variable.s
	ItemSlot _myslot = null;

	//Local values.
	int _slotitemCost = 0;
	
	// this represents the index of the game object tracker who maintains the list
	// of cannon slots. atm there are only two slots.
	private int _slotIndex;
	public int SlotIndex{
	get{
			return _slotIndex;
		}
		set{
			_slotIndex = value;

		}
	}

	public ItemSlot MySlot
	{
		get{return _myslot;}
		set{
			_myslot = value;
			UpdateDisplay();


		}
	}
	
	private Cannon _myCannon;  // hold on to the reference of the cannon
	public Cannon MyCannon{
	get{
			return _myCannon;
		}
		set{
			_myCannon = value;
			
		}
	}

	void OnEnable()
	{
		if(_myslot != null)
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		if(_myslot.IsOpen)
		{
			BG.ToggleWindowOn();
			SlotInfo.ToggleWindowOn();
			SlotOptions.ToggleWindowOff();

			//Update the slot visuals.
			_sprIcon.spriteName = _myslot.IconName;
			//_lblSlotName.text = "Tap to Select";
		}
		else
		{
			BG.ToggleWindowOff();
			SlotOptions.ToggleWindowOn();
			SlotInfo.ToggleWindowOff();

			_lblSlotName.text = "Add Slot";
		}

		if(_CostLabel)
		{
			float cost = (float)_myslot.Cost /100.0f;
			
			string format = string.Format("{0:C}",cost);
			_CostLabel.text = format;
		}

	}

	void Update()
	{
		if(_myslot == null)
		{
			_myslot = GameObjectTracker.instance._PlayerData.ItemSlots[_slotIndex];


			Debug.LogError("slot nulllllll");
			return;
		}


	}

	void OnAddSlot()
	{
		_myslot.IsOpen = true;
		_lblSlotName.text = "Tap to Select";

		//Play sound.
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);

		//Update the cost.
		GameObjectTracker.instance._PlayerData.GemCart += _myslot.Cost;

		UpdateDisplay();


	}

	void MinusSlot()
	{
		//Keep active slots so we cant minus them.
		if(_myslot.StartActive)
			return;

		_myslot.Clear();
	
		GameObjectTracker.instance._PlayerData.GemCart -= _myslot.Cost;

		UpdateDisplay();
	}



//	public void ButtonOpenCannonListPressed(){
//		
//		// when the slot is clicked, and we know the slot is available to change.
//		// tell the activity to open up the cannon grid to select the cannon.
//		// The slot is not told that the button to select cannon has been clicked
//		// Two different scripts 
//		if (_myActivity != null){
//			_myActivity.StartCannonSelect(this);
//		}
//	}
	
	public void ButtonCannonSelectedPressed(BaseItemCard ForcedCard = null){
			
		BaseItemCard cannonItemCard = ActivityManager.Instance.HighlitedCard;

		//Lets check if there is no selected item
		if(cannonItemCard == null)
		{
			//Turn off the info window.
			//SlotInfo.ToggleWindowOff();

			//Play sound.
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);

			//If we press the button and we have no cannon then we clear the slot.
			if(_myslot.Type != EntityFactory.CannonTypes.NULL)
			{
	
				//Clear slot if pressed with nothign selected.
				_myslot.Type = EntityFactory.CannonTypes.NULL;

				//Play the clear animation
				DeSelectAnimation.Reset();
				DeSelectAnimation.Play(true);


				//Set to zebra as default.
				if(_myslot.StartActive)
				{
					_myslot.Type = EntityFactory.CannonTypes.Zebra;
					_lblSlotName.text = "Default";
				}


				ToyBox.GetPandora().AssignSlot(_myslot.Type, _slotIndex);

				//Since we cleared the slow, play the cleared slot label.
				_lblSlotName.text = EmptySlotLabel;

				//Update teh costs here.
				GameObjectTracker.instance._PlayerData.GemCart -= _slotitemCost;
				_slotitemCost = 0;

				// then update the slot visuals
				UpdateDisplay();
				return;
			}

			MinusSlot();
//			Debug.LogError("Slot Minused");
			return;
		}
		
	
		
		// the cannon isn't used, assign it 
		ToyBox.GetPandora().AssignSlot(cannonItemCard.ContainedCannonType, _slotIndex);
		
		// then update the slot visuals
		_sprIcon.spriteName = cannonItemCard.DisplayInfo.IconName;
		_lblSlotName.text = cannonItemCard.Label;

		//Check here if slot is 0 then apply cost. or it would apply cost every time we tap. 
		if(_slotitemCost != cannonItemCard.DisplayInfo.GemRequirements)
		{
			//Negate the current item cost count before we update.
			GameObjectTracker.instance._PlayerData.GemCart -= _slotitemCost;

			//Set the cost.
			_slotitemCost = cannonItemCard.DisplayInfo.GemRequirements;
			GameObjectTracker.instance._PlayerData.GemCart += _slotitemCost;

		}
		
		//Play the animation
		if(SelectTween)
		{
			SelectTween.Reset();
			SelectTween.Play(true);
			
			//Play audio sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotPushed);
			
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotSelect);
			
			
			
		}

	}

//	int SlotItemCost
//	{
//		get{return _slotitemCost;}
//		set
//		{
//			_slotitemCost = value;
//
//			//Update the cart in player data.
//		}
//	}

	void OnSelectFinish()
	{
		//Play audio sound
		//AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.SlotSelect);
	}
	

	
	public void ButtonInfoPressed(){
	
	}
	
	public void ButtonAddSlotPressed(){
	
	}
}
