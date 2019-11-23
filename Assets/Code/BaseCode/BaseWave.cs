using UnityEngine;
using System.Collections;

public class BaseWave : MonoBehaviour {
	
	protected bool Completed = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	public bool IsCompleted()
	{
		 return Completed;
	}
}
