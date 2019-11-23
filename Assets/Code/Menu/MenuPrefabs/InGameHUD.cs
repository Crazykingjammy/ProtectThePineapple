using UnityEngine;
using System.Collections;


public class InGameHUD : MenuSystem {
	
	public Label ShieldButton;
	public Label ShootButton;
	
	public GUIText BallCountText;
	public GUIText TargetCountText;
	public GUIText MoneyCountText;
	public GUIText MultiplierCountText;
	
	public int BallCount = 0;
	public int TargetCount = 0;
	public int MoneyCount = 0;
	public int MultiplierCount = 0;
	public float Health = 100.0f;
	
	public GUITexture CannonNotification;
	public GUITexture ItemNotification;
	public GUITexture GameOverNotification;
	
	public GUITexture MoneyIcon;
	public GUITexture ScoreIcon;
	
	public GUITexture LifeBar;
	public Texture2D LifeStatus;
	public Rect LifePosition;
	public float HealthOffset = 20.0f;
		
	public Joystick JoystickControls;
	public UnityJoystick xbox;
	
	

	bool shieldPushed = false;
	bool shootPushed = false;
	
	bool gameOver = false;
	
	float multiplierTimer;
	// Use this for initialization
	
	
	void Start () {
		
		ShieldButton.SetAction("OnShield");
		ShootButton.SetAction("OnShoot");
		
		
		//Create joystick from the prefab.
		//JoystickControls = Instantiate(JoystickControls) as Joystick;
		//xbox = Instantiate(xbox) as UnityJoystick;
		
		
		//Get the rect of the life bar.
		LifePosition = LifeBar.pixelInset;
		LifePosition.x = (LifeBar.transform.position.x * Screen.width)  - (LifeBar.pixelInset.width/2.0f);
		LifePosition.y = ( (1.0f - LifeBar.transform.position.y)  * Screen.height) - (LifeBar.pixelInset.height/2.0f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//UpdateText();
		
		if(gameOver)
		{
			SetEnable(false);
			
			//if(JoystickControls)
			//Destroy(JoystickControls.gameObject);
		}
		
	}
	
	void OnGUI()
	{
		//Get the rect of the life bar.
		LifePosition = LifeBar.pixelInset;
		LifePosition.x = (LifeBar.transform.position.x * Screen.width)  - (LifeBar.pixelInset.width/2.0f);
		LifePosition.y = ( (1.0f - LifeBar.transform.position.y)  * Screen.height) - (LifeBar.pixelInset.height/2.0f);
		
		float HealthWidth = LifeBar.pixelInset.width * (Health/100.0f);
		
		//Clamp to not go below 0
		if(HealthWidth < 0.0f)
			HealthWidth = 0.0f;
		
		LifePosition.width = HealthWidth;
		
		
		if(LifeBar.enabled)
		GUI.DrawTexture(LifePosition,LifeStatus);
	}
	
	public void SetEnable(bool b)
	{

		//Set the notifications active accordingly.
		//CannonNotification.gameObject.active = b;
		//ItemNotification.gameObject.active = b;
		
		
		CannonNotification.gameObject.SetActive(b);
		ItemNotification.gameObject.SetActive(b);
		
		MoneyIcon.enabled = b;
		ScoreIcon.enabled = b;
		
		//Set the texts
		BallCountText.enabled = b;
		TargetCountText.enabled = b;
		MoneyCountText.enabled = b;
		MultiplierCountText.enabled = b;
		
		//Set the buttons.
		ShootButton.SetEnable(b);
		ShieldButton.SetEnable(b);
		
		LifeBar.enabled = b;
		
		//JoystickControls.gameObject.active = b;
		
	}
	
	public void OnShield()
	{
		shieldPushed = true;
	}
	
	public void OnShoot()
	{		
		shootPushed = true;
	}
	
	void UpdateText()
	{
		//return;
		
		//Enable the multiplier text by default.
		MultiplierCountText.enabled = true;
		
		//TExt
		BallCountText.text = BallCount.ToString();
		TargetCountText.text = TargetCount.ToString();
		MoneyCountText.text = MoneyCount.ToString();
		
		//We set the combo text
		if(MultiplierCount <= 0)
		{
			MultiplierCountText.enabled = false;
			return;
		}
		
		
		//Set the multiplier text.
		MultiplierCountText.text = "+" + MultiplierCount.ToString() + " Hits";
		
	
		//Calculate the alpha based on the multiplier timer.
		Color c = MultiplierCountText.font.material.color;
		c.a = 1.0f - (multiplierTimer / 3.0f);		
		MultiplierCountText.font.material.color = c; 
		

		
		//Set some fun text if we hit higher numbers
		if(MultiplierCount == 1)
		{
			MultiplierCountText.text = "+" + MultiplierCount.ToString() + " Hit";	
		}
		
		
		//Set some fun text if we hit higher numbers
		if(MultiplierCount > 10)
		{
			MultiplierCountText.text = "+" + MultiplierCount.ToString() + " Hits !";
			
		}
		
		
		//Set some fun text if we hit higher numbers
		if(MultiplierCount > 50)
		{
			MultiplierCountText.text = "+" + MultiplierCount.ToString() + " Hits!!!";	
		}
		
		
		
		
	}
	
	public void SetBasicInformation(int ballcount, int targetcount, int moneycount, int multiplyer = 0, float multipliertimer = 3.0f, float health = 100.0f)
	{
		BallCount = ballcount;
		TargetCount = targetcount;
		MoneyCount = moneycount;
		MultiplierCount = multiplyer;
		multiplierTimer = multipliertimer;
		Health = health;
	}
	
	public bool IsShieldButtonPushed()
	{
		//If the value is true, return true and reset the value.
		if(shieldPushed)
		{
			shieldPushed = false; 
			return true;
		}
		
		return false;
	}
	
	public bool IsShootButtonPushed()
	{
		//If the value is true, we return true and reset the value.
		if(shootPushed)
		{
			shootPushed = false;
			return true;
		}
		
		return false;
	}
	
	public void SetNotifications(Vector3 cannon, Vector3 item, bool visible, Vector3 health)
	{
		//Set the position and settings of the cannon notification for the bot.
		CannonNotification.transform.position = cannon;
		CannonNotification.enabled = visible;
		
		//Create a temp vector and clamp the item notification on the x asis.
		Vector3 itempos = item;
		itempos.x = Mathf.Clamp(item.x,0.07f,1.0f);
		itempos.y = Mathf.Clamp(item.y,0.1f,1.0f);
		
		//Now set the position and settings.
		ItemNotification.transform.position = itempos;
		ItemNotification.enabled = visible;
		
		health.y -= HealthOffset;
		LifeBar.transform.position = health;
	
	}
	
	public void ClearInput()
	{
		if(JoystickControls)
		{
			JoystickControls.ClearInputDirection();
		}
	}
	
	public void ActivateGameOver()
	{
		if(!gameOver)
		{
			GameOverNotification.GetComponent<Animation>().Play("GameOver");
			GameOverNotification.GetComponent<Animation>().PlayQueued("Brethe",QueueMode.CompleteOthers);
			//GameOverNotification.animation.Play("Brethe");
		}
			
		
		gameOver = true;
		
		
		
		
	}
	
	public void KillHealth()
	{
		LifeBar.GetComponent<Animation>().Play();
	}
	
	public void DeactivateGameOver()
	{
		GameOverNotification.enabled = false;
	}
	
	public Vector3 GetMovementDirection()
	{
		
		if(JoystickControls)
		{
			return JoystickControls.GetInputDirection();	
		}
		
		
		return Vector3.zero;
	}
}
