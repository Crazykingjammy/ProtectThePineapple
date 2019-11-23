using UnityEngine;
using System.Collections;

public class CannonSelectionObject : MonoBehaviour {
	
	//Basic Info
	public UILabel _lblCannonName = null;
	public UISprite _sprIcon = null;
	public UISprite _BG = null;
	public UISprite _closeUpIcon = null;
	
	
	//Unlock button info.
	public UILabel TrophyRequirement;
	public UILabel CostRequirement;
	public TweenScale UnlockButtonWindow;
	public TweenPosition UnlockDenyAnimation;
	
	//Progress information.
	public UISlider TrophyCountSlider;
	public UILabel TrophyCount;
	public FUIWindowToggles SelectedWindow;
	public FUIWindowToggles DisplayWindow;
	
	public UICheckbox _myCheckbox;

	public GameObject NewNotification;

	//Unlocking variables.

	
	//Local card pointer.
	CannonItemCard _cannonItemCard = null;
	
	public CannonItemCard MyCannonCard{
	get{
			return _cannonItemCard;
		}
	set{
			_cannonItemCard = value;
			
			//Do calculations here.
			_cannonItemCard.CalculateTrophies();
			
			//Set the card visual info.
			_lblCannonName.text = _cannonItemCard.Label;
			_sprIcon.spriteName = _cannonItemCard.DisplayInfo.IconName;
		
			_closeUpIcon.spriteName = _cannonItemCard.DisplayInfo.IconName;
			_closeUpIcon.transform.localScale = _cannonItemCard.IconScale;
			_closeUpIcon.transform.localPosition = _cannonItemCard.IconOffset;

			//_BG.color = _cannonItemCard.DisplayInfo.BGColor;
			//_lblCannonName.color = _cannonItemCard.DisplayInfo.BGColor;
			
			//Set the unlock info.
			
			//Set the trophy count string
			TrophyRequirement.text = _cannonItemCard.DisplayInfo.LevelRequirement.ToString();
			
			//Set the gem requirement string.
			float gemcount = (float)_cannonItemCard.DisplayInfo.GemRequirements/100.0f;
			string format = string.Format("{0:C}",gemcount);
			CostRequirement.text = format;
			
		
			//Set the Progress info.
			
			//Set the count text.
			string count = string.Concat(_cannonItemCard.TrophiesEarned.ToString() + "/" + _cannonItemCard.TrophyCount.ToString() );
			TrophyCount.text = count;
			
			//Set the progress.
			float ratio = 0;
			if(_cannonItemCard.TrophyCount > 0)
				ratio = (float)_cannonItemCard.TrophiesEarned/(float)_cannonItemCard.TrophyCount;
			TrophyCountSlider.sliderValue = ratio;
			
			
			//Determine which window to view if the cannon is unlocked.
			if(_cannonItemCard.Unlocked)
			{
				//Display progress window and hide unlock window.
				//TrophyWindow.ToggleWindowOn();
				//UnlockButtonWindow.ToggleWindowOff();

				//TrophyWindow.SetActive(true);
				UnlockButtonWindow.gameObject.SetActive(false);
				NewNotification.gameObject.SetActive(false);
			}
			else
			{
				//Display lock window and hide progress window.
				//TrophyWindow.ToggleWindowOff();
				//UnlockButtonWindow.ToggleWindowOn();
				
				//TrophyWindow.SetActive(false);
				UnlockButtonWindow.gameObject.SetActive(true);

				//Check against current leevel to display new notification if we reached qualifiying level.
				if(GameObjectTracker.instance._PlayerData.MyCurrentLevel >= _cannonItemCard.DisplayInfo.LevelRequirement )
					NewNotification.gameObject.SetActive(true);
				else
					NewNotification.gameObject.SetActive(false);
			}
			
		}
	}
	
	

		
	void Update()
	{
		if(!_cannonItemCard.Unlocked)
			_myCheckbox.isChecked = false;

		if(_myCheckbox.isChecked)
			SelectedWindow.ToggleWindowOn();
		else
			SelectedWindow.ToggleWindowOff();

//		if(!_myCheckbox.isChecked && SelectedWindow._isVisible)
//			SelectedWindow.ToggleWindowOff();
	}

	void OnDisable()
	{
		_myCheckbox.isChecked = false;
	}
	
	/// <summary>
	/// When this selection objects 3d collider hits the 3d selector 
	/// We ar going to update the Center Info Window!
	/// </summary>
	public void HitSelector(){
		Debug.Log("" + _lblCannonName.text + " is Selected");
		
		/*
		 * We have been selected, update the Info in the Info Window
		 * 
		 */
		
		
	
	}
	

	void OnSelectCannon()
	{
		if(_myCheckbox.isChecked)
		{
			//Debug.LogError("Cannon Selected and CHECKED!");
			ActivityManager.Instance.SelectedCard = MyCannonCard;
			ActivityManager.Instance.HighlitedCard = MyCannonCard;

		}
		else
		{
			//Debug.LogError("Deselect Call for NULLLLLLLLLLa");
			//ActivityManager.Instance.SelectedCard = null;
			ActivityManager.Instance.HighlitedCard = null;
		}


		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);

		//Display the info window on selection.
		//SelectedWindow.ToggleWindowOn();
		//DisplayWindow.ToggleWindowOff();

	}

	/// <summary>
	/// Called from one of the buttons that make up the Selection Object
	/// </summary>
	public void ButtonCannonSelectedPressed(){
		
		Debug.Log("Cannon Card Press: " + MyCannonCard.Label);
		
//		//Dont do anything if we are not unlocked.
//		if(!MyCannonCard.Unlocked)
//			return;
//		
//		//Set the current selection and activate mission viewer.
//		ActivityManager.Instance.SelectedCard = MyCannonCard;
			
		ActivityManager.Instance.PushActivity(ActivityManager.ManagerActivities.Missions,true);

		//Play the sound
		AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Select);

		//Deselect the self.
		_myCheckbox.isChecked = false;
			
		}
	
	public void OnUnlockPressed()
	{
		
		
		//Determine which window to view if the cannon is unlocked.
			if(MyCannonCard.UnlockCard())
			{
				//TrophyWindow.SetActive(true);
			//	UnlockButtonWindow.gameObject.SetActive(false);
			
			//Display progress window and hide unlock window.
			UnlockButtonWindow.Play(true);
			
			//Play the sound
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.Unlock);
			}
		else
		{
			UnlockDenyAnimation.Reset();
			UnlockDenyAnimation.Play(true);

			//Play the sound
			AudioPlayer.GetPlayer().PlayMenuSFX(AudioPlayer.MenuSFX.CCWarning);
		}
			
	}
	
	public void OnUnlockAnimationFinish()
	{
		Debug.Log("UNLOCKED CANNON Finish!!");
		
		UnlockButtonWindow.gameObject.SetActive(false);
	}
	
}
