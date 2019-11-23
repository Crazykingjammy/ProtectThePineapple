using UnityEngine;
using System.Collections;

public class IconLocPhaseNotif : IconLocObject {
	
	// the object that spawned me should have a tween transform
	TweenTransform _tweenTransform = null;
	
	// the notification will have the scale
	TweenScale     _tweenScale     = null;
	
	bool _haveTweenTrans = false;
	bool _haveTweenScale = false;
	
	new void Start(){
		// this is a simple Icon, default basically.  so everything is taken care of from the 
		// base
		base.Start();
		
		Init();
		

	}
	
	bool Init(){
		_tweenScale = gameObject.GetComponent<TweenScale>();
		if (IconLocTarget != null){
			_tweenTransform = IconLocTarget.GetComponent<TweenTransform>();
			
			isFullyInit = false;
			return isFullyInit;
		}
		if (_tweenScale != null){
			_haveTweenScale = true;
		}
		if (_tweenTransform != null){
			_haveTweenTrans = true;
			
//			// set the to and from for the tween transform
//			_tweenTransform.Reset();
//			_tweenTransform.Play(true);
		}
		
		isFullyInit = true;
		return isFullyInit;
		
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		base.LateUpdate();
		
		if (!isFullyInit){
			Init ();
		}
		
		if (_haveTweenTrans && Input.GetKeyUp(KeyCode.T)){
			_tweenTransform.Reset();
			_tweenTransform.Play(true);
		}
		if (_haveTweenScale && Input.GetKeyUp(KeyCode.T)){
			_tweenScale.Reset();
			_tweenScale.Play(true);
		}
		
		
	}
	
	public void Restart(){
			_tweenTransform.Reset();
			_tweenTransform.Play(true);
	}
}