using UnityEngine;
using System.Collections;

public class FUIStatPoint : MonoBehaviour {
	
	public UILabel Points;
	
	public TweenTransform exitAnimation;
	
	UIGrid _parent;
	UIDraggablePanel _panel;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}
	
	public void Tally(float speed = 1.0f)
	{
		exitAnimation.duration = speed;
		
		exitAnimation.Reset();
		exitAnimation.Play(true);			
		
	}
	
	public UIGrid MyParent
	{
		get{return _parent;}
		set{_parent = value; }
	}
	
	public UIDraggablePanel myPanel
	{
		get{return _panel;}
		set{_panel = value; }
	}
	
	
	
	public int PointValue 
	{
		get {return 0;}
		set
		{
			string sign = "+";
			
			//Set the text.
			if(value < 0)
				sign = "";
				
			Points.text = sign + value.ToString();
			
			//Activate
			if(!gameObject.activeSelf)
				gameObject.SetActive(true);
			
			
		}
	}
}
