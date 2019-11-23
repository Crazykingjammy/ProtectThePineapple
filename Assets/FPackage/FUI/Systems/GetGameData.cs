using UnityEngine;
using System.Collections;

public class GetGameData : MonoBehaviour {
	
	public UILabel label;
	
	public UISprite Animal;
	TweenAlpha animalTween;
	
	public Color originalColor, FillColor, EmptyColor;
	
	// Use this for initialization
	void Start () {
		label = gameObject.GetComponent<UILabel>();
		animalTween = Animal.GetComponent<TweenAlpha>();
		
		originalColor = label.color;
		
		//Animal.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		ToyBox tb = ToyBox.GetPandora();
		if (tb != null){
			
			//Get the data.
			int ballcount =tb.Bot_01.GetBallsInStorage();
			
			//Get the bot.
			Bot b = tb.Bot_01;
			
			if(b.IsCannonAttached() && Animal.alpha == 0.0f)
			{
				//Get the cannon icon and set it.
				Animal.spriteName = b.GetCannon().CannonTextureName;
				
				animalTween.Play(true);
			}
			
			if(!b.IsCannonAttached())
				animalTween.Play(false);
			
			
			//Write to the label.
			label.text = ""+ ballcount + "";
			
			//Set the color to default
			label.color = originalColor;
			

			//If we reached max capacity
			if(tb.Bot_01.IsCannonFull())
			{
				label.color = FillColor;
			}
			
			if(ballcount <= 0)
			{
				label.color = EmptyColor;
				
			}
				
			
		}
	}
}
