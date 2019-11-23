using UnityEngine;
using System.Collections;

public class FUIPhaseBadgeObject : MonoBehaviour {
	
	public UISprite mySprite; 
	public string givenSpriteName = "phaseunknown";
	public bool Used = false;
	
	// Use this for initialization
	void Start () {
		mySprite = GetComponentInChildren<UISprite>();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
