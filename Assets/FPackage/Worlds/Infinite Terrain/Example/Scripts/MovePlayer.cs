using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
			gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 10);
		if (Input.GetKey(KeyCode.RightArrow))
			gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 10);
		if (Input.GetKey(KeyCode.UpArrow))
			gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 10);
		if (Input.GetKey(KeyCode.DownArrow))
			gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 10);
	}
}
