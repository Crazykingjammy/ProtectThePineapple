using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

	protected Vector3 InputDirection;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
	
	
	virtual protected void ProcessInput()	
	{
		
	}
	
	public Vector3 GetInputDirection()
	{
		return InputDirection;
	}
	
	public void ClearInputDirection()
	{
		InputDirection = Vector3.zero;
	}
	
}
