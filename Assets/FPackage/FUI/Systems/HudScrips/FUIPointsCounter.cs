using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FUIPointsCounter : MonoBehaviour {
	
	public UIGrid PointStatCounter;
	public UIDraggablePanel panel;
	
	public List<FUIStatPoint> _pointsList;
	public List<FUIStatPoint> _activeList;
	
	public FUIStatPoint _template, AnimatorSP;

	public UILabel Bank, BankAdd;
	public TweenTransform BankAddAnimation;

	public UILabel PointsAdd, PointsSubtract;
	public TweenTransform PointsAddAnimation, PointsSubtractAnimation;
	public UISprite PointsAddBG;
	public TweenAlpha PointsSubtractAlphaAnimation;


	public List<int> ScoreStack;
	int currentIndex = 0;
	int trackingIndex = 1;
	
	
	public int listSize = 10;
	
	UILabel myLabel;
	string myLabelText;
	int points;
	int prevGemCount = 0;
	int prevPoints = 0;
	
	//Local timer and frequency values.
	float timer = 0.0f;
	float frequency = 1.0f;
	
	//Public variables for speeds of the log.
	public float CasualFrequency, SlowFrequency, FastFrequency;
	public float scrollSpeed = -0.2f;
	
	bool _logVisible = true;
	public bool debuglog = true;
	
	// Use this for initialization
	void Awake() {
		myLabel = gameObject.GetComponent<UILabel>();	
		myLabel.text = "0";
		// this will set our member var to the gameobjects uilabel script
		// now we can edit it.
		if (myLabel == null){
			Debug.LogError("Unable to find label for Point Counter");
		}


		BankAdd.alpha = 0.0f;

		PointsAdd.alpha = 0.0f;
		PointsAddBG.alpha = 0.0f;

		PointsSubtract.alpha = 0.0f;
		
		//Make a list of points to work with.
		for(int i = 0; i < listSize; i++)
		{
			//Create object.
			FUIStatPoint sp = NGUITools.AddChild(PointStatCounter.gameObject,_template.gameObject).GetComponent<FUIStatPoint>();
			
			//Make it a child of the grid.
			sp.transform.parent = PointStatCounter.transform;
			sp.MyParent = PointStatCounter;
			//sp.myPanel = panel;
			
			//Disable it and add it to list.
			sp.gameObject.SetActive(false);
			_pointsList.Add(sp);
		}
		
		
		//Reserve the first for empty testing.
		frequency = SlowFrequency;
		
		
		LogVisible = debuglog;
		
	}
	void Start () {

		
	}
	
	public bool LogVisible 
	{
		get 
		{
			return _logVisible;
		}
		
		set
		{
			_logVisible = value;
			
			//Disable
			//panel.enabled = _logVisible;
			//AnimatorSP.enabled = _logVisible;
		}
	}

	void OnPointsAddFinish()
	{
		PointsAdd.alpha = 0.0f;
		PointsAddBG.alpha = 0.0f;
	}

	void OnPointsSubtractFinish()
	{
		//PointsSubtract.alpha = 0.0f;
	}
	// Update is called once per frame
	void Update () {
		points = GameObjectTracker.GetGOT().GetCurrentPoints();

		//Trigger point adding animation.
		if(points > prevPoints)
		{
			//myLabelText = "" + points + "";
			myLabelText = string.Format("{0:#,#,#}", points);
			myLabel.text = myLabelText;

			//Set the label of the point to add.
			//Lets set the label to the difference between prev point and current... uhh yeah.
			int dif = points - prevPoints;
			PointsAdd.text = "+ " + dif.ToString();
			PointsAdd.alpha = 1.0f;
			PointsAddBG.alpha = 1.0f;

			//Trigger animation.
			PointsAddAnimation.Reset();
			PointsAddAnimation.Play(true);

			prevPoints = points;

		}

		//Trigger point subtraction here.
		if(points < prevPoints)
		{
			//myLabelText = "" + points + "";
			myLabelText = string.Format("{0:#,#,#}", points);
			myLabel.text = myLabelText;

			//Create the label as the difference between the points
			int dif = prevPoints - points;
			PointsSubtract.text = "- " + dif.ToString();
			//PointsSubtract.alpha = 1.0f;

			//Trigger animation.
			PointsSubtractAnimation.Reset();
			PointsSubtractAnimation.Play(true);

			PointsSubtractAlphaAnimation.Reset();
			PointsSubtractAlphaAnimation.Play(true);

			prevPoints = points;
		}

		if(points == 0)
		{
			myLabel.text = "0";
			prevPoints = 0;
		}



		int newGems = GameObjectTracker.instance.GetGemsCollected();

		//Apply gems label here to draw it anyway.

		if(newGems > prevGemCount)
		{
			//Set the bank amount.
			float gemcount = (float)newGems;
			gemcount = gemcount/100.0f;
			string gemFormat = string.Format("{0:C}",gemcount);
			Bank.text = gemFormat;
		

			//Get the multiplier to set as the adding amount
			int multiplier = GameObjectTracker.instance.GetMultiplier();
			float addrate = (float)multiplier;
			addrate = addrate/100.0f;
			string addFormat = string.Format("{0:C}",addrate);
			BankAdd.text = addFormat;

			//Make visible the count label
			BankAdd.alpha = 1.0f;

			//Trigger Animation
			BankAddAnimation.Reset();
			BankAddAnimation.Play(true);

			prevGemCount = newGems;

		}


		if(newGems == 0)
		{
			//Draw gems at 0 here. only called when gems are 0.
			//Set the bank amount.
			float gemcount = (float)newGems;
			gemcount = gemcount/100.0f;
			string gemFormat = string.Format("{0:C}",gemcount);
			Bank.text = gemFormat;

			prevGemCount = 0;

		}


		}


	void OnBankAddFinish()
	{
		//When finish animation hide the label.
		BankAdd.alpha = 0.0f;
	}
		
	void ResetList()
	{
	
		int index = 0;
		
		//Go through and set all the values from the list down.
		foreach(FUIStatPoint sp in _pointsList)
		{
			if(index >= currentIndex)
			{
		//		Debug.LogError("Deactivate");
				
				//Disable the rest.
				sp.gameObject.SetActive(false);
				break;
			}
			
			sp.PointValue = ScoreStack[index];
			index++;
		}
		
		
		//Reposition grid after resetting.
		PointStatCounter.Reposition();
		
		//And remove the top. 
		ScoreStack.RemoveAt(0);
		currentIndex--;
	}
	
	public void AddStatPoint(int val)
	{
		
		//If we are out of range just keep overwriting the last copy.
		//if(currentIndex > 100)
		//	currentIndex = 0;
		
		if(!_logVisible)
			return;
		
		//Add the score to the current index in the score stack
		//ScoreStack[currentIndex] = val;
		ScoreStack.Add(val);
		
		//Increment index.
		currentIndex++;		

	}
}
