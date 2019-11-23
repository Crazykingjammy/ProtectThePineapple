using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class Rewards : MonoBehaviour {


	public float DisplayTime = 2.0f;
	public float FadeRate = 0.02f;
	
	
	//internal color values for fading
	Color currentColor;
	float timer = 0.0f;
	
	int nf = 0;
	
	public GUITexture currentFruit;
	public List<Texture2D> FruitList;
	public List<float> fruitstamps;
	
	

	// Use this for initialization
	void Start () {
		
		//Hide the current fruit.
	 	currentColor = Color.gray;
		currentColor.a = 0;
		
		currentFruit.color = currentColor;
	
		
	}
	
	// Update is called once per frame
	void Update () {
	

		
//		if(Input.GetMouseButtonDown(0))
//		{
//			DisplayFruit(nf,129.0f);
//		}
//		
		
		if(timer > 0.0f)
		{
			//Subtract till we are nothing.
			timer -= Time.deltaTime;
			currentColor.a = timer/DisplayTime;
			
			//Clamp incase we go below 0
			currentColor.a = Mathf.Clamp(currentColor.a,0.0f,1.0f);
			
			//Set the color
			currentFruit.color = currentColor;
		}
		
	}
	
	
	public void DisplayFruit(int index, float time)
	{
	
		//Set the texture according to the index.
		currentFruit.texture = FruitList[index];
		
		//Set the values for on.
		timer = DisplayTime;
		currentColor.a = 1.0f;
		nf++;
		
		fruitstamps.Add(time);
		
		//Play the animation.
		currentFruit.GetComponent<Animation>().Play();
	}
	
	
	public Texture2D GetFruitImage(int index)
	{
		return FruitList[index];
	}
	
	public float GetFruitTime(int index)
	{
		if(fruitstamps.Count == 0)
		{
			return 120.0f;
		}
		
		return fruitstamps[index];
	}
	
	public int GetRewardCount()
	{
		return nf;
	}
	
}
