using UnityEngine;
using System.Collections;

public class makeworldspin : MonoBehaviour {

	public float rotSpeed = -0.050f;
	public Vector3 rotVec = Vector3.right;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.RotateAround(transform.position, rotVec, rotSpeed);
	}
}
