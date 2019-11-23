using UnityEngine;
using System.Collections;

public class BankWindow : MonoBehaviour {
	
	public UILabel GemCount;
	public UILabel TrophyCount;
	
	public UISlider TrophySlider;

	public UILabel LevelCount, CartGemCount;
	
	//Local player data cache.
	PlayerData _data = null;
	
	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(_data == null)
		{
			//Assign player data.
			_data = GameObjectTracker.instance._PlayerData;
		
		}
		if(LevelCount)
			LevelCount.text = _data.MyCurrentLevel.ToString();
		
		//Update the data.
		//Update the gem count
		float gemcount = (float)_data.GemBank/100.0f;
		float cartcount = (float)_data.GemCart/100.0f;

		string format = string.Format("{0:C}",gemcount);
		
		GemCount.text = format;

		//Set the cart amount here.
		format = string.Format("{0:C}",cartcount);
		if(CartGemCount)
		CartGemCount.text = format;

		
		//Set the trophy counts.
		int total = _data.MaxTrophies;
		int count = _data.TrophiesEarned;
		
		TrophyCount.text = string.Concat( count.ToString() + '/' + total.ToString() );
		
		//And set the slider.
		//Calculate ratio
		float ratio = 0;
		if(total > 0)
			ratio = (float)count/(float)total;
		 
		TrophySlider.sliderValue = ratio;
	
	}
	
	
	void OnBanker(bool down)
	{
		if(down)
			GameObjectTracker.instance._PlayerData.GemBank += 700;
	}
}
