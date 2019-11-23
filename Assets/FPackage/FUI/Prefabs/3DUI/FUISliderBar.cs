using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FUISliderBar : MonoBehaviour {
	
	private UISlider mySlider;
	public UISprite mySliderBarBackground;
	
	protected float maxSliderWidth = 1f;
	
	public UISlider Slider {
		get{
			return mySlider;
		}
		set{
			mySlider = value;
		}
	}		
	
	// who is using me
	// temporary myTarget is public.
	// to be private 
	private GameObject _myTarget = null;
	public GameObject MyTarget {
		get{
			return _myTarget;
		}
		set{
			_myTarget = value;
		}
	}
	
	public Transform windowBar;
	
	// Variables for the slider
	// does this slider bar fade away or always stay visible
	public bool isAlwaysVisible = false;
	// whats my current state, am I visible?
	//bool isVisible = true;
	
	public bool isFadeAway = false;
	// takes x seconds to fade
	public float timeToFade = 3.0f;	
	
	protected void Start(){

		
		mySlider = this.gameObject.GetComponentInChildren<UISlider>();
		if (mySlider == null){
			mySlider = null;
			return;
		}
		
		maxSliderWidth = mySlider.foreground.localScale.x;
		
//		Debug.Log("FUISliderBar found its UISlider");
		
				// get all of my widgets, for easy alpha
//		UISprite [] widgets = this.gameObject.GetComponentsInChildren<UISprite>();
		
//		foreach (UISprite widget in widgets){
//			_listWidgets.Add(widget);
//		}
//		
	}
	
	public void SetSliderAlpha(float a){
		UISprite fgSprite = mySlider.foreground.GetComponent<UISprite>();
		
		if (fgSprite == null){
			Debug.LogError("We have no forground Sprite in FUISliderBar");
			return;
		}
		
		fgSprite.alpha = a;
		
		if (mySliderBarBackground == null){
			Debug.LogError("We have no background Sprite in FUISliderBar");			
		}
		mySliderBarBackground.alpha = a;		
	}
	
	public bool Init(GameObject target){
		if (target == null) return false;
		
		_myTarget = target;
		return true;
	}
	
	
}
