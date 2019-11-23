using UnityEngine;
using System.Collections;

/// <summary>
/// FUI fade panel script.
/// A Simple SCript to fade out a panel and then deactive it
/// </summary>
/// 

public class FUIFadePanelScript : MonoBehaviour {
	
	public float duration = 1.0f;
	bool faded = false;
	public bool startFade = false;
	float fadeDirection = -1;	//either positive or negative, scaled by alpha factor
	float alpha = 1f;
	
	//float start = 0.0f;
	
	public GameObject currentPanel = null;
	public GameObject nextPanel = null;
	
	UIWidget [] widgets;
	
	// Use this for initialization
	void Start () {
		//start = Time.realtimeSinceStartup;
		widgets = GetComponentsInChildren<UIWidget>();	
	}
	
	
	// Update is called once per frame
	void Update () {
	
		if (startFade && faded == false){
		
			
			alpha += fadeDirection * (Time.deltaTime * 1f); // (duration > 0f) ? 1f - Mathf.Clamp01((Time.realtimeSinceStartup - start) / duration) : 0f;
	 
	        foreach (UIWidget w in widgets)
	        {
	            Color c = w.color;
	            c.a = alpha;
	            w.color = c;
	        }
			if (alpha < 0.5f || alpha > 1f){
				//fadeDirection = -fadeDirection;
				// faded = true;
				
				//NGUITools.SetActive(nextPanel, true);
				NGUITools.SetActive(currentPanel, false);
				NGUITools.SetActive(nextPanel, true);
				
			}			
		}		
		
	}
	
	public void ToggleFade(){
		//if (!faded
	}
}
