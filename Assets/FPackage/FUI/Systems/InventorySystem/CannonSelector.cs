using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Cannon selector.
/// 
/// This is going to be used to updat the center panel with info
/// 
/// </summary>
/// 
public class CannonSelector : MonoBehaviour {
		
	/*
	 * Cannon Selector is going to be staticly available
	 */
	static CannonSelector _instance = null;
	static public CannonSelector GET(){
		return _instance;
	}

	public UILabel _infoTitleCannonName = null;
	
	public UILabel _DisplayInfo = null;
	public FUIStatsTable _displayTable = null;
	
	// we want to check against all the cannon select object and make
	// the one we're colliding with the selected cannon
	public FUICannonListManager CannonListManager = null;
	
	// to do the collision check, we need to know of all the objects
	List<CannonSelectionObject> _listOfCannonSelectObjects = null;
	
	// this is the one that we're currently selecting
	CannonSelectionObject _currentSelectedCannonObject = null;
	
	bool _haveListOfCannonSelectObject = false;
	
	// Use this for initialization
	void Start () {
		_instance = this;
		Init();
		
	}
	
	void Init(){
//		if (CannonListManager.ListOfCannonSelectObjects != null){		
//			_listOfCannonSelectObjects = CannonListManager.ListOfCannonSelectObjects;
//			_haveListOfCannonSelectObject = true;
//		}
//		else{
//			_haveListOfCannonSelectObject = false;
//		}
	}
	
	// Update is called once per frame
	void Update () {
		
//		if (_haveListOfCannonSelectObject){
//			foreach(CannonSelectionObject cobj in _listOfCannonSelectObjects){
//				//if (cobj.mySensor.collider.bounds.Contains(this.collider.transform.position)){
//				if (cobj.mySensor.collider.bounds.Intersects(this.collider.bounds)){					
//					_currentSelectedCannonObject = cobj;
//					break;
//				}
//			}
//			
//			_infoTitleCannonName.text = _currentSelectedCannonObject._lblCannonName.text;
//			
//			//Update teh display info.
////			_DisplayInfo.text = _currentSelectedCannonObject._lblCannonName.text;
////			_displayTable.statData = _currentSelectedCannonObject.MyCannonCard.ItemStats;
//		}
//		else
//		{
//			// try to get it again.
//			Init();
//		}
	}	
	

	public CannonSelectionObject GetSelectedCannonObject(){
		return _currentSelectedCannonObject;
	}
	
	// these ontrigger functions do not work when the simulation time step is 0
	// need to do collision dectection from with in the update
	
//	void OnTriggerEnter(Collider col){
//		Debug.Log("We have collided - cannonSelector");
//		if (col.gameObject.CompareTag("CannonListObject")){
//
//			Debug.Log("Collided with CannonListObject");
//			CannonObjectSensor selSensor = col.GetComponent<CannonObjectSensor>();
//			if(selSensor == null){ return; /* get out! */}
//			
//			CannonSelectionObject selObject;
//			selObject = selSensor.MySelectionObject;
//			
//			_infoTitleCannonName.text =  selObject._lblCannonName.text;
//			
//			selSensor.HitSelector();
//		}
//	}
//	
//	void OnTriggerStay(Collider col){
//		Debug.Log("We have collided - cannonSelector");
//		if (col.gameObject.CompareTag("CannonListObject")){
//
//			Debug.Log("Collided with CannonListObject");
//			CannonObjectSensor selSensor = col.GetComponent<CannonObjectSensor>();
//			if(selSensor == null){ return; /* get out! */}
//			
//			CannonSelectionObject selObject;
//			selObject = selSensor.MySelectionObject;
//			
//			_infoTitleCannonName.text =  selObject._lblCannonName.text;
//			
//			selSensor.HitSelector();
//		}
//	}
}
