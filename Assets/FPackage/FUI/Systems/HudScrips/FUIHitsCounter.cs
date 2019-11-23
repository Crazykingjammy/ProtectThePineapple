using UnityEngine;
using System.Collections;

public class FUIHitsCounter : MonoBehaviour {
	
	public float CloseBuffer = 0.2f;

	public UILabel Counter, Multiplier, Bank, CollectCount, PrevCounter;
	
	public UISprite Outline;
	public UISprite CurrentPhaseIcon;
	public UISprite MultiplierStar;
	
	public TweenRotation CounterPop;
	public TweenPosition MyAnimation;
	public TweenTransform CollectAnimation, BankAnimation;
	public TweenAlpha AlphaAni;

	public TweenRotation ClockBigHand;
	private TweenRotation CurrentPhaseIconRotation;
	
	//locals
	int prevMultiplier = 0;
	int prevGems = 0;
	bool visible = false;
	void Awake(){

		
	}
	
	// Use this for initialization
	void Start () {
		
		prevMultiplier = 0;
		
		//CollectCount.alpha = 0.0f;

		// see if the phase icon has a tween rotation
		TweenRotation temp = CurrentPhaseIcon.gameObject.GetComponent<TweenRotation>();
		if (temp != null){
			CurrentPhaseIconRotation = temp;
			CurrentPhaseIconRotation.Play(true);
		}

		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		//Grab data from GOT
		int newmultiplier = GameObjectTracker.GetGOT().GetMultiplier();
		int newGems = GameObjectTracker.instance.GetGemsCollected();
		float alpha = GameObjectTracker.GetGOT().GetComboAlpha();
		
		//Set the bank amount.
		float gemcount = (float)newGems;
		gemcount = gemcount/100.0f;
		string gemFormat = string.Format("{0:C}",gemcount);
		Bank.text = gemFormat;

		//Set the phase icon.
		//CurrentPhaseIcon.spriteName = GameObjectTracker.GetGOT().Game
		
		if(alpha < CloseBuffer && visible)
		{
//			Debug.LogError("alpha: " + alpha);
			
			MyAnimation.Play(false);
			visible = false;
		}
		
		if(newmultiplier == 0)
		{
			//Set previous multiplier.
			prevMultiplier = 0;
			
			//If we are visible play our animation
			//Dont process any more.
			return;
		}
		
		//Turn out counter if we go past 0 and is not visible.
		if(alpha > CloseBuffer && !visible)
		{
			//MyAnimation.Reset();
			MyAnimation.Play(true);
			visible = true;
		}
		
		
		//If we are changed then trigger counter pop.
		if(newmultiplier > prevMultiplier)
		{
			Counter.text = newmultiplier.ToString();
			
			//Set the shadow
			PrevCounter.text = Counter.text;
		
			
			CounterPop.Reset();
			CounterPop.Play(true);


		}
				
		
		
		//Update gem colleciton
		if(newGems != prevGems)
		{
			//Format collect amount text to money.
			float collect = (float)newmultiplier;
			collect = collect/100.0f;
			string collectCount = string.Format("+{0:C}", collect);
			
			//Set the text
			CollectCount.text = collectCount;
				
			//CollectCount.alpha = 1.0f;
			CollectAnimation.Reset();
			CollectAnimation.Play(true);
			
			//AlphaAni.Reset();
			AlphaAni.Play(true);
			
			
			BankAnimation.Reset();
			BankAnimation.Play(true);
			
			prevGems = newGems;
		}
		
		//Fill up the outline!
		Outline.fillAmount = alpha;
		Outline.color = Color.Lerp(Color.red,Color.white,alpha);



		Quaternion qrot = Quaternion.identity;

		float ClockAngle =  Mathf.Lerp(359, 0, alpha);
		qrot = Quaternion.AngleAxis(ClockAngle,Vector3.forward);

		//ClockBigHand.
		 

		ClockBigHand.transform.localRotation = qrot;

		
		//Update the muliplier.
		int currentMultiplierModifier = GameObjectTracker.instance.Game.GetCurrentMultiplierModifier();
		Multiplier.text = "x" + currentMultiplierModifier;

		// we dont want to show a x0 for gameover
		if (currentMultiplierModifier == 0){

			
			//MyAnimation.Reset();
			//MyAnimation.Play(false);
		}	
		
		//Set prev multiplier
		prevMultiplier = newmultiplier;
	}
	
	
	
	void CollectFinish()
	{
		AlphaAni.Reset();
	}
	

}
