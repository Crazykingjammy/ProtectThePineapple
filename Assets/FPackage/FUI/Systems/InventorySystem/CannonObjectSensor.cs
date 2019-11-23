using UnityEngine;
using System.Collections;

public class CannonObjectSensor : MonoBehaviour {
	
	CannonSelectionObject _myObject = null;
	
	
	public CannonSelectionObject MySelectionObject{
	get{
			return _myObject;
		}
	}
	
	public void Start(){}
	
	public void Awake(){
		if (_myObject == null){
			Transform parent = gameObject.transform.parent;
			CannonSelectionObject testCannonSelectionObject = parent.gameObject.GetComponent<CannonSelectionObject>();
			if (testCannonSelectionObject){
			_myObject = testCannonSelectionObject;
			}
		}
	}
	
	public void HitSelector(){
		_myObject.HitSelector();
	}
	
	
}
