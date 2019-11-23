using UnityEngine;
using System.Collections;

public class Money : MonoBehaviour {
	
	bool gactive = false;
	
	public int value = 10;
	
	//Local value for the age of the entity.
	public float age;
	
	
	public bool GemActive
	{
		get{
			return gactive;
		}
		set{
			gactive = value;
			gameObject.SetActive(value);
			age = 0.0f;
		}
	}
	

}
