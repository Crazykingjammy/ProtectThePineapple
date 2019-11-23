using UnityEngine;
using System.Collections;

public class PostGameView : MonoBehaviour {
	
	//Configurations
	public float CrunchTimer = 0.2f;
	
	//Textures
	public GUITexture BotzDestroyed;
	public GUITexture GemsCollected;
	public GUITexture Score;
	
	//Text
	public GUIText BotzCount;
	public GUIText GemsCount;
	public GUIText ScoreCount;
	
	public Label Quit;
	public Label Retry;
	
	
	// Use this for initialization
	void Start () {
		
		Quit.SetAction("OnQuit");
		Retry.SetAction("OnRetry");
			
	}
	
	// Update is called once per frame
	void Update () {
	
			
	}
	
	void OnQuit()
	{
		Application.LoadLevel("MainMenu");
	}
	
	void OnRetry()
	{
		Application.LoadLevel("TestLevel");
	}
	public void Activate(int targetsDestroyed, int moneyCollected, int totalScore)
	{
		
		BotzCount.text = targetsDestroyed.ToString();
		GemsCount.text = moneyCollected.ToString();
		ScoreCount.text = totalScore.ToString();
		

	}
}
