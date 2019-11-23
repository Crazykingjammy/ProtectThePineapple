using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FUIWindowToggles : MonoBehaviour {
	
	// a panel for this window.
	//public UIPanel _panel = null;
	
	// UIWidget [] widgets = null;
	TweenScale _tweenScale = null;
	TweenPosition _tweenPosition = null;
	public bool _isVisible = false;
	
	public float currAlpha;
	
	UIPanel [] _panels;
	
	//bool _isTweenDone = false;
	
	// Use this for initialization
	void Start () {
		
		// get the Tween Scale
		_tweenScale = GetComponent<TweenScale>();
		
		if(_tweenScale)
		{
			_tweenScale.eventReceiver = this.gameObject;
			_tweenScale.callWhenFinished = "TweenDone";	
		}
		
		_tweenPosition  = GetComponent<TweenPosition>();
		
		if(_tweenPosition)
		{
			_tweenScale.eventReceiver = this.gameObject;
			
		}
		
		
		// find all the panels in my window
		_panels = gameObject.GetComponentsInChildren<UIPanel>();
		
		
		if (!_isVisible){
		
			
			// make everything invisible
			SetWindowAlpha(0.0f);
			
		}	
		
		
	}
		
	/// <summary>
	/// Toggles the window.
	/// </summary>
	/// <returns>
	/// The window is visible
	/// </returns>
//	public bool ToggleWindow(){
//		
//		if (_tweenScale != null){
//			_tweenScale.Play(!_isVisible);
//		}
//		
//		_isVisible = !_isVisible;
//		
//		return _isVisible;
//	}
	
	public bool ToggleWindowOn(){
		
		
		//Make sure we are active
//		if(!this.gameObject.activeSelf)
//			this.gameObject.SetActive(true);
//		
		PlayTween(true);
		
		
		SetWindowAlpha(1.0f);
		_isVisible = true;
		
		return _isVisible;
	}

	public bool ToggleWindowOff(){
	
		PlayTween(false);
		
		/*
		 * No Longer setting the window alpha off. here
		 * After the tween is done, then the Window disappears.
		 */
		SetWindowAlpha(0.0f);
		
		
		_isVisible = false;
		
		return _isVisible;
	}
	
	public void SetWindowAlpha(float alpha){
		
		if(_panels == null)
			return;
		
		foreach (UIPanel pan in _panels){
			pan.alpha = alpha;
		}
		currAlpha = alpha;
	}
	
	public float GetWindowAlpha(){
		return currAlpha;
	}
	
	public void TweenDone(){
		//_isTweenDone = true;
		if (!_isVisible){
			SetWindowAlpha(0.0f);
		}
	}
	
	public void PlayTween(bool forward){
		
		if(_tweenScale != null)
		{
			_tweenScale.Play(forward);

		}
		
		
		if(_tweenPosition != null)
		{
			_tweenPosition.Play(forward);
		}

			
		
	}

}
