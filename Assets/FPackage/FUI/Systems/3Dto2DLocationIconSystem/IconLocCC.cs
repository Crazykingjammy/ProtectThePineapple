using UnityEngine;
using System.Collections;

public class IconLocCC : IconLocObject {
	
	// instead of just a sprite, we need to be an NGUI progress bar
	//	public UISprite CannonIcon;
	// this is the graphic rep of the slider / health bar.  Taken in as a prefab
	public UISlider myHealthBarPrefab;
	private FUISlider _myHealthBar;
	public UISprite CCBG;
	public UILabel  CCLabel;
	
	public UISprite CCHealthBeatSprite;
	private float CCHealthBeatMaxWidth = 0.0f;
	
	
	int _lastFrameHealthPertentage = 0;
	float healthPercentage = 0.0f;
	int healthAsInt = 0;
	
	float _startFadeTime = 0.0f;
	float _TimeSinceStart = 0.0f;
	bool  _startedTiming = false;
	float _curTimeDelta = 0.0f;
	bool _faded = false;
		
	private FUIWindowToggles myWindow;
	
	// the Comman Center we want to track
	// 	BaseEnemy _enemy = null;  needs to get dynamically
	private CommandCenter _myCommandCenter;
	
	// caching an offset to the bottom of the screen.  
	// this way we can keep the CC healthbar along the bottom of the screen
	public Transform bottomOffset;

	
	
	
	new void Start(){
		base.Start();
		isFullyInit = Init();
		
		if (CCHealthBeatSprite != null){
			CCHealthBeatMaxWidth = CCHealthBeatSprite.transform.localScale.x;
		}		
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		base.LateUpdate();
	
		
		if (_myCommandCenter == null){
			return;
		}
		float _healthPercentageDec;
		_healthPercentageDec = _myCommandCenter.GetHealthPercentage();
		
		healthPercentage = _healthPercentageDec * 100;
		healthAsInt = (int)healthPercentage;
		
		if (_lastFrameHealthPertentage != healthAsInt){
			_startedTiming = false;
			_startFadeTime = 0.0f;			
			myWindow.SetWindowAlpha(1.0f);
			_faded = false;
		}
		
		if (_lastFrameHealthPertentage == healthAsInt){
			// start the fade out
			if (!_startedTiming && !_faded){
				_startFadeTime = Time.time;
				_startedTiming = true;
				_TimeSinceStart = 0.0f;
				
			}
			
			_curTimeDelta = Time.deltaTime;
			
			if (_startedTiming && !_faded){
				_TimeSinceStart += _curTimeDelta;
				if (_TimeSinceStart > 3){
					_startedTiming = false;
					_startFadeTime = 0.0f;
					_TimeSinceStart = 0.0f;
					_faded = true;
					myWindow.SetWindowAlpha(0.0f);
					
				}
				else{
					myWindow.SetWindowAlpha(myWindow.GetWindowAlpha() - (0.2f*_curTimeDelta));
				}
				
			}
			
		}
		
		if (CCLabel != null){
			CCLabel.text = "" + healthAsInt + "%";
		}
		
		//CCHealthBeatSprite.transform.localScale = new Vector3(_healthPercentageDec * CCHealthBeatMaxWidth, CCHealthBeatSprite.transform.localScale.y, CCHealthBeatSprite.transform.localScale.z);
		CCBG.fillAmount = _healthPercentageDec;
		
		_lastFrameHealthPertentage = healthAsInt;
		
		
		if (!isFullyInit){ // || _enemy == null){
			Init();
			return;
		}
		
		
		
	}
	
	bool Init(){
		// make sure we have everything we need before this function ends!
//			if (this.IconLocTarget == null){
//				isFullyInit = false;
//				return isFullyInit;
//			}
		CommandCenter tempCC = this.IconLocTarget.GetComponent<CommandCenter>();
		
		if (tempCC == null){
			Debug.Log("" + gameObject.name + " No tempCC attached to IconLocTarget");
			isFullyInit = false;
			return isFullyInit;
		}
		
		// We're attached to an enemy, now we can processes it
		_myCommandCenter = tempCC;	
		
		FUIWindowToggles testWindow = this.GetComponent<FUIWindowToggles>();
		if (testWindow == null){
			Debug.Log("" + gameObject.name + " No window attached to IconLocTarget");
			isFullyInit = false;
			return isFullyInit;
		}
		myWindow = testWindow;		
		
		isFullyInit = true;		
		return isFullyInit;
	}
	
	new public void Deactivate(){

		
		base.Deactivate();
	}
	
	new public void Activate(){
//		Debug.Log("IconLocEnemy - Activating IconLocEnemy");
		base.Activate();
	}
}
