using UnityEngine;
using System.Collections;

/// <summary>
/// FUI panel toggle.
/// A Quick Script to start a UI Panel Deactived
/// 
/// </summary>

public class FUIPanelToggle : MonoBehaviour {
	
	public bool bActive = true;
	
	//bool bFadeIn = false;
	//bool bFadeOut = false;
	//bool bFading = false;
	//bool bFadingDone = true;
	
	// the attached gameobject needs a panel
	UIPanel thisPanel;
	// cached the widgets of this panel
	UIWidget [] widgets;
	
	Collider [] colliders;
	
	float alpha = 1f;
	//float duration = 3f;
	//float startFade = 0;
	
	
	
	
	// Use this for initialization
	void Start () {
		thisPanel = GetComponent<UIPanel>();
		if(thisPanel){
			NGUITools.SetActive(thisPanel.gameObject, bActive);
		}
		
		widgets = GetComponentsInChildren<UIWidget>();
		
		if (bActive)
			alpha = 1.0f;
		else 
			alpha = 0;
		
				foreach (UIWidget w in widgets)
		        {
		            Color c = w.color;
		            c.a = alpha;
		            w.color = c;
		        }
	}
	
	// Update, handling the fading in or fading out
	void Update(){
//		if (bFading){
//			if (bFadeIn){
//				// We're fading in, make the Panel active and fade it in
//				bActive = true; 
//				NGUITools.SetActive(thisPanel.gameObject, bActive);
//				alpha += .01f; //(duration > 0f) ? 1f - Mathf.Clamp01((Time.realtimeSinceStartup - startFade) / duration) : 0f;
//				foreach (UIWidget w in widgets)
//		        {
//		            Color c = w.color;
//		            c.a = alpha;
//		            w.color = c;
//		        }
//				if (alpha >= 1f){
//					alpha = 1f;
//					bFading = false;
//					bFadeIn = false;
//				}
//			}
//			else if (bFadeOut){
//				// We're fading out
//				// Dont make the panel inactive until the fade out is done.
//				alpha -= .01f; //(duration > 0f) ? 1f - Mathf.Clamp01((Time.realtimeSinceStartup - startFade) / duration) : 0f;
//				foreach (UIWidget w in widgets)
//		        {
//		            Color c = w.color;
//		            c.a = alpha;
//		            w.color = c;
//		        }
//				if (alpha <= 0){
//					alpha = 0;
//					bFading = false;
//					bFadeOut = false;
//					bActive = false;
//					NGUITools.SetActive(thisPanel.gameObject, bActive);
//					
//
//				}
//			}
//		}
	}
	
	/// <summary>
	/// Start this instance.
	/// if its on, it goes off, if off, goes on.
	/// </summary>
	/// 
	public bool TogglePanel(){
		bActive = !bActive;
		if (thisPanel){
			NGUITools.SetActive(thisPanel.gameObject, bActive);
		}
		
		return bActive;
	}
	
	public bool TogglePanel(bool isOn){
		bActive = isOn;
		if (thisPanel){
			NGUITools.SetActive(thisPanel.gameObject, bActive);
		}
		
		return bActive;
	}
	
	/// <summary>
	/// Toggles the panel fade.
	/// When trigger, bounces from on to off, but first tweens a fade, in fade out
	/// </summary>
	/// <returns>
	/// the new state of this panel
	/// </returns>
//	public bool TogglePanelFade(){
//		if (bActive)
//			bFadeOut = true;
//		else if (!bFading && !bActive)
//			bFadeIn = true;
//		
//		bFading = true;
//		bFadingDone = false;
//		startFade = Time.time;
//		
//		return bActive;
//	}
	
	public void ToggleAllColliders(bool active){
		
		colliders = GetComponentsInChildren<Collider>();
		
		foreach(Collider col in colliders){
			col.enabled = active;
		}
	}
	
	public void ApplyAlphaToChildren(float a){
		
		widgets = GetComponentsInChildren<UIWidget>();
		
		foreach (UIWidget w in widgets)
	        {
	            Color c = w.color;
	            c.a = a;
	            w.color = c;
	        }
	}
	
}
