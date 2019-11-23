using UnityEngine;
using System.Collections;

public class StartLevel : MonoBehaviour {
	
	public float DisplayTime = 0.32f;
	
	public GUIText Text;
	
	public bool load = true;
	public float duration = 0.5f;

	public string[] IntroTexts;


	public bool LoadTutorial = true;
	float time = 0.0f;
	
	// Use this for initialization
	void Start () {
		
		Time.timeScale = 1.0f;
		
		int phrase = Random.Range(0, IntroTexts.Length);



	
		//Set the text according to index of array.
		Text.text = IntroTexts[phrase];

		//Set custom text for intro if we are going into the tutorial.
		if(LoadTutorial)
			Text.text = "Thank you so much for checking out our game!";
	
	}
	
	// Update is called once per frame
	void Update () {
	
		time += Time.deltaTime;
		
		Text.color = Color.Lerp(Color.black, Color.white,time/duration);

		if(time> DisplayTime && LoadTutorial)
		{
			Application.LoadLevel("Tutorial");
			return;
		}

		if(time > DisplayTime && load)
			Application.LoadLevel("Intro");
		
	}
}
