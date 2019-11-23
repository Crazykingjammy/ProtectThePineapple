using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FUI3DManager : MonoBehaviour {
	
	///I'm a singleton
	private static FUI3DManager _manager = null;
	
	public static FUI3DManager GetLM{
		get { return _manager; } 
	}
	static public FUI3DManager GetManager(){
		if (_manager){
			return _manager;}
		else{
			return null;}
	}
	
	//////////////////////////////////////////////////////
	
	private UIRoot _myRoot = null;
	private UIPanel _myPanel = null;
	
	private List<HealthBarObject> _listOfHealthBars = null;
	
	public HealthBarObject DefaultHealthBar = null;
	
	
	
	// Use this for initialization
	void Start () {
		// set the instance of me
		_manager = this;
		
		_myRoot = GetComponentInChildren<UIRoot>();
		_myPanel = GetComponentInChildren<UIPanel>();
		
		Init();
		
	}
	
	bool Init(){
	
		return true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CreateHealthBar(GameObject target, HealthBarObject prefab = null){
		if (target == null) return;
		
		if (prefab == null && DefaultHealthBar != null){
			// we use default healthbar prefab
			// add it to the panel
			HealthBarObject tempHealthBar = NGUITools.AddChild(_myPanel.gameObject, DefaultHealthBar.gameObject).GetComponent<HealthBarObject>();
			tempHealthBar.transform.localScale *= 5;
			tempHealthBar.Init(target);
			
		}
		else{
			return;
		}
	}
}
