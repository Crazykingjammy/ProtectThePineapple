using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {
	
	static private AudioPlayer _self = null;
	
	public AudioClip[] MenuSounds;
	
	public enum MenuSFX
	{
		Tap,
		Open,
		Close,
		Pause,
		Unpause,
		Unlock,
		SlotPushed,
		SlotSelect,
		Select,
		PhasePresent,
		PPSJoke,
		PPSWait,
		XIP,
		UnXip,
		LevelUp,
		Chicken,
		Yes,
		CrowdSuspense,
		CashRegister,
		CCWarning,
		Count
	}
	
	AudioClip startingClip;
	

	AudioSource Track2;
	AudioSource EffectsTrack;
	
	float _effectsVolume = 0.63f;
	float _musicVolume = 1.0f;
	
	
	bool tweenDone = false;
	
	// Use this for initialization
	void Start () {
		
		//Assign self.
		_self = this;
		
		//Grab any audiotracks.
		Track2 = transform.Find("AudioTrack").GetComponent<AudioSource>();
		
		//Grab the sound effects track.
		EffectsTrack = transform.Find("EffectsTrack").GetComponent<AudioSource>();
		//EffectsTrack.volume = _effectsVolume;
		//Specify that we do not destroy this object.
		DontDestroyOnLoad(gameObject);
		
		//Store the original clip name
		startingClip = GetComponent<AudioSource>().clip;
		
	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	
	//Our singleton access.
	static public AudioPlayer GetPlayer()
	{
		
		if(_self != null)
			return _self;
		
		//Debug.Log("Player Not Created");
		return null;
						

	}
	
	public void PlaySound(AudioClip clip, float volumeModifier = 1.0f)
	{		
		EffectsTrack.PlayOneShot(clip,_effectsVolume);
	}
	
	public void PlayMenuSFX(MenuSFX sound)
	{
		EffectsTrack.PlayOneShot(MenuSounds[(int)sound],_effectsVolume);
	}
	
	//Public play audio clip for smart switching.
	public void PlayAudioClip(AudioClip clip = null, float t = 0.0f)
	{
		//If nothing is passed in then we use the starting clip.
		if(!clip)
		{
			GetComponent<AudioSource>().clip = startingClip;
		}
		
		//If we are requesting the clip already in place dont do anything.
		if(GetComponent<AudioSource>().clip == clip)
		{
			return;
		}
		
		if(t > 0)
			print("Audio Time: " + t + "Track: " + clip.name);
	
		//Stop the current track.
		GetComponent<AudioSource>().Stop();
		
		
		//Set the clip and location
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().time = t;
		
		
		//Set the volume
		//audio.volume  = _musicVolume;
		
		GetComponent<AudioSource>().Play();
		
	}
	
	public bool PushTrack(AudioClip clip)
	{
		if(!GetComponent<AudioSource>().isPlaying)
			return false;
		
		//Pause currently playing track.
		GetComponent<AudioSource>().Pause();
		
		//Play clip on second track.
		Track2.clip = clip;
		Track2.volume = _musicVolume;
		Track2.Play();
		
		return true;
	}
	
	public void PopTrack()
	{
		//If the main track is playing we dont need do anything. 
		//Allows us to call pop as much as we want.
		if(GetComponent<AudioSource>().isPlaying)
		{
			return;
		}
		
		//Stop Second Track, 
		Track2.Stop();
		
		//Resume main track.
		GetComponent<AudioSource>().volume = 0.0f;
		GetComponent<AudioSource>().Play();
		tweenDone = false;
		
		
		
	}
	
	public void StopMusic()
	{
		//Stop the main track.
		GetComponent<AudioSource>().Stop();
		
		//And we will stop any other tracks as well.
		Track2.Stop();
	}
	
	void tweenUpdate()
	{
		//Get the volume amount to set to.
		float rate = 0.3f * Time.deltaTime;
		
		
		if(GetComponent<AudioSource>().volume < MusicVolume)
		{
			GetComponent<AudioSource>().volume += rate;
		}
		else{
			GetComponent<AudioSource>().volume = MusicVolume;
			//let this be our exiting tween condition
			tweenDone = true;
		}
			
		
	}
	
	void Update()
	{
		//If we need to be animating.
		if(!tweenDone)
		{
			tweenUpdate();
		}
	}
	
	
	public float MusicVolume
	{
		get{
			return _musicVolume;
		}
		set
		{
			_musicVolume = value;
			GetComponent<AudioSource>().volume = _musicVolume;
			Track2.volume = _musicVolume;
		}
	}
	
	public float EffectsVolume
	{
		get{
			return _effectsVolume;
		}
		set{
			_effectsVolume = value;
			//EffectsTrack.volume = _effectsVolume;
		}

	}
	
	//Returns the remaining time of the sound file playing in the main audio track.
	public float RemainingTime
	{
		get {
			
			 return GetComponent<AudioSource>().clip.length - GetComponent<AudioSource>().time;
			
		}
	}
	
}