using UnityEngine;
using System.Collections;

public class Intro : MenuSystem {
	
	
	public bool Transit = true;
	public GameObject mesh;
	public GameObject title;
	
	//public GameObject titleEffectPosition;
	
	public ParticleSystem Shine;
	public AudioClip Chime;
	
	public ParticleSystem Bang;
	public ParticleSystem BangBang;
	public AudioClip Boom;
	
	
	public GameObject BG;
	
	public AudioClip[] IntroTracks;
	
	public GUIText load;

	public AudioPlayer SoundAudioPlayer;
	
	//SocialCenter sloader = null;

	bool createdAudio = false;

	void Awake()
	{
		//Create an audioplayer if we dont ahve one.
		if(AudioPlayer.GetPlayer() == null)
		{
			SoundAudioPlayer = Instantiate(SoundAudioPlayer) as AudioPlayer;

			createdAudio = true;
		}
		


		Debug.LogError("AWAKE CALLED IN INTRO");
	}

		// Use this for initialization
	void Start () {
		
		Time.timeScale = 1.0f;
		
		
		//Assign Random Track
		int index = Random.Range(0,IntroTracks.Length);
		
		//print(index);



		//Create an audioplayer if we dont ahve one.
		if(createdAudio)
		{
			SoundAudioPlayer.PlayAudioClip(IntroTracks[index]);

		}
		else
		{
			//If intro is being played and audio wasnt created by us 
			//then we can assume it was the tutorial that created the sound and came before us.
			Time.timeScale = 0.75f;
		}



		
		//Bang = Instantiate(Bang) as ParticleSystem;
		
		//BG.renderer.enabled = false;
		BG.SetActive(false);
		
		
		load.enabled = false;
	}
	
	
	// Update is called once per frame
	void Update () {
		
		
		if(Input.GetKeyDown(KeyCode.K))
		{
			mesh.GetComponent<Renderer>().enabled = true;
			BG.SetActive(false);
			GetComponent<Animation>().Play();
		}

	
	}
	
	public void TriggerShine()
	{
		Shine.Play();
		GetComponent<AudioSource>().PlayOneShot(Chime);
	}
	
	public void TriggerTitleScreen()
	{
		//Display Logo Image.
		mesh.GetComponent<Renderer>().enabled = false;
		BG.SetActive(true);
		
		//Enable the title
		//title.renderer.enabled = true;
		
		//Authenticate here.
		SocialCenter.Instance.Authenticate();
		
		//Bang.transform.position = titleEffectPosition.transform.position;
		
		
		Bang.Play();
		BangBang.Play();
		GetComponent<AudioSource>().PlayOneShot(Boom);
	}
	
	public void EndAnimation()
	{
		
		//Logo.enabled = true;
		
		//GameCenterSingleton.Instance.Initialize();
				
		load.enabled = true;
		//Load Leaderboard
		SocialCenter.Instance.LoadLeaderboard("Highscore_PPS");
		
		if(Transit)
		Application.LoadLevel("TestLevel");
		
	}
}
