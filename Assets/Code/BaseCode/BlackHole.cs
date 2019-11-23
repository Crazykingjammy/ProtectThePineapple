using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {
	
	//Magnitude of the force.
	public float Magnitude;
	
	//Activate
	public bool Active = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	
	
	/// <summary>
	/// Pulls to center.
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	public void PullToCenter(GameObject obj)
	{
		gravitate(obj, Magnitude);
	}
	
	/// <summary>
	/// Pulls to center.
	/// This is to overide the standard magnitude of the black hole incase children want to attract differnt objects and different magnitudes..
	/// </summary>
	/// <param name='obj'>
	/// Object.
	/// </param>
	/// <param name='magnitude'>
	/// Magnitude.
	/// </param>
	public void PullToCenter(GameObject obj, float factor)
	{
		gravitate(obj,Magnitude * factor);
	}
	
	void gravitate(GameObject obj, float m)
	{
		//If we are not active just return and do nothing
		if(!Active)
		{
			return;
		}
		
		//We have to check if there is a rigid body.
		if(!obj.GetComponent<Rigidbody>())
		{
			//There is no rigid body.
			Debug.Log("There is no rigidbody and you are trying to pull an object : " + obj.name);
			return;
		}
		
		//Calculate the direction to the center of the object to apply the force.
		
		//Set the vector for the direction to move 
		Vector3 Direction = Vector3.zero;
		Vector3 ForceVector = Vector3.zero;
		
		//Create a vector to temp store the position of self.
		Vector3 Position = transform.position;
		
		//Create a look vector by subtracting the objects position with the position of self. 
		Direction = Position - obj.transform.position;
		Direction.Normalize();
		
		
		//Scale the force vector to the magnitude and then by the direction the force needs to be.
		ForceVector.Set(m,m,m);
		ForceVector.Scale(Direction);
		
		//Move the body.
		obj.GetComponent<Rigidbody>().AddForce(ForceVector,ForceMode.Force);
		
	}
	
//	// Update is called once per frame
//	void FixedUpdate () {
//	
//	}
}
