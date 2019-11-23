using UnityEngine;
using System.Collections;

[System.Serializable]
public class OptionData
{
	float SoundFXLevel = 1.0f;
	float MusicLevel = 1.0f;
	
	Vector2 ShootScale, ShieldScale, ShootPosition, ShieldPosition = Vector2.zero;
	
	public bool ClassicControls = true;
	
	
	public float SoundFX
	{
		get{return SoundFXLevel;}
		
		//Do a write everytime we set?
		set{
			SoundFXLevel = value;
			
			if(SoundFXLevel > 1.0f)
				SoundFXLevel = 1.0f;
			
			//Apply to the audio player.
			AudioPlayer.GetPlayer().EffectsVolume = SoundFXLevel;
		}
	}
	
	public float Music
	{
		get{return MusicLevel;}
		
		//Do a write everytime we set?
		set{
			MusicLevel = value;
			
			if(MusicLevel > 1.0f)
				MusicLevel = 1.0f;
			
			AudioPlayer.GetPlayer().MusicVolume = MusicLevel;
		}
	}
	
	public float JoystickDeadzone 
	{
		get
		{
			if(FileFetch.HasLocalKey("OPTIONS_JoystickDeadzone"))
			{
				float v = FileFetch.FetchLocalFloat("OPTIONS_JoystickDeadzone");
				
				return v;
			}
			
			return -1.0f;
		}
		
		set {
			
			FileFetch.SetLocalFloat("OPTIONS_JoystickDeadzone",value);
			
		}
	}
	
	
	public float JoystickRange
	{
		get
		{
			if(FileFetch.HasLocalKey("OPTIONS_JoystickRange"))
			{
				float v = FileFetch.FetchLocalFloat("OPTIONS_JoystickRange");
				
				return v;
			}
			
			return -1.0f;
		}
		
		set {
			
			FileFetch.SetLocalFloat("OPTIONS_JoystickRange",value);
			
		}
	}
	
	
	public Vector2 JoystickPosition
	{
		set
		{
			//Set the control value
			ToyBox.GetPandora().Controls.JoystickPosition = value;
			
			//Retrieve it for saving.
			Vector2 pos = ToyBox.GetPandora().Controls.JoystickScreenPosition;
			
			
			
			//Save the joystick position.
			FileFetch.SetLocalFloat("OPTIONS_JoystickPos_X", pos.x);
			FileFetch.SetLocalFloat("OPTIONS_JoystickPos_Y", pos.y);
			
		}
		
		get
		{
			
			//Fetch the joystick position.
			Vector2 pos = Vector2.zero;
			
			if(FileFetch.HasLocalKey("OPTIONS_JoystickPos_X"))
			{
				//Load the joystick position.
				pos.x = FileFetch.FetchLocalFloat("OPTIONS_JoystickPos_X");
				pos.y = FileFetch.FetchLocalFloat("OPTIONS_JoystickPos_Y");
				
			}
			
			
			return pos;
			
		}
	}
	
	public Transform ShootButton
	{
		get
		{
			return null;
		}
		
		set
		{
			ShootScale = new Vector2(value.localScale.x,value.localScale.y);
			ShootPosition = new Vector2(value.localPosition.x, value.localPosition.y);
			
		}
	}
	
	public Vector3 ShootButtonScale
	{
		get
		{
			Vector3 s = Vector3.zero;
			
			if(FileFetch.HasLocalKey("OPTIONS_ShootScale"))
			{
				s.x = FileFetch.FetchLocalFloat("OPTIONS_ShootScale");
				s.y = s.x;
				s.z = s.x;
				
				//	Debug.LogError("Shoot Scale " + s);
			}
			
			return s;
		}
	}
	
	public Vector3 ShootButtonPosition
	{
		get
		{
			Vector3 s = Vector3.zero;
			
			if(FileFetch.HasLocalKey("OPTIONS_ShootPosition_X"))
			{
				s.x = FileFetch.FetchLocalFloat("OPTIONS_ShootPosition_X");
				s.y = FileFetch.FetchLocalFloat("OPTIONS_ShootPosition_Y");
				s.z = 0.0f;
				
				//	Debug.LogError("Shoot Position " + s);
				
			}
			
			return s;
		}
	}
	
	
	public Transform ShieldButton
	{
		get
		{
			return null;
		}
		
		set
		{
			ShieldScale = new Vector2(value.localScale.x,value.localScale.y);
			ShieldPosition = new Vector2(value.localPosition.x, value.localPosition.y);
			
			SaveControlButtons();
		}
	}
	
	public Vector3 ShieldButtonScale
	{
		get
		{
			Vector3 s = Vector3.zero;
			
			if(FileFetch.HasLocalKey("OPTIONS_ShieldScale"))
			{
				s.x = FileFetch.FetchLocalFloat("OPTIONS_ShieldScale");
				s.y = s.x;
				s.z = s.x;
				
				//	Debug.LogError("Shield Scale " + s);
			}
			
			return s;
		}
	}
	
	public Vector3 ShieldButtonPosition
	{
		get
		{
			Vector3 s = Vector3.zero;
			
			if(FileFetch.HasLocalKey("OPTIONS_ShieldPosiiton_X"))
			{
				s.x = FileFetch.FetchLocalFloat("OPTIONS_ShieldPosiiton_X");
				s.y = FileFetch.FetchLocalFloat("OPTIONS_ShieldPosiiton_Y");
				s.z = 0.0f;
				
				
				//		Debug.LogError("Shield Position " + s);
			}
			
			return s;
		}
	}
	
	void SaveControlButtons()
	{
		
		//Save the offset vectors.
		FileFetch.SetLocalFloat("OPTIONS_ShieldPosiiton_X",ShieldPosition.x);
		FileFetch.SetLocalFloat("OPTIONS_ShieldPosition_Y",ShieldPosition.y);
		
		FileFetch.SetLocalFloat("OPTIONS_ShootPosition_X",ShootPosition.x);
		FileFetch.SetLocalFloat("OPTIONS_ShootPosition_Y",ShootPosition.y);
		
		//Save the scale values.
		FileFetch.SetLocalFloat("OPTIONS_ShieldScale",ShieldScale.x);
		FileFetch.SetLocalFloat("OPTIONS_ShootScale",ShootScale.x);
		
	}
	void LoadControlButtons()
	{
		
	}
	
	public void Save()
	{
		FileFetch.SetFloat("OPTIONS_SFX", SoundFXLevel);
		FileFetch.SetFloat("OPTIONS_MUSIC", MusicLevel);		
		
	}
	public void Load()
	{
		
		//If we dont have one of the keys we dont have both so set manually and return!
		if(!FileFetch.HasKey("OPTIONS_SFX"))
		{
			SoundFXLevel = 0.63f;
			MusicLevel = 1.0f;
			return;
		}
		
		
		SoundFXLevel = FileFetch.FetchFloat("OPTIONS_SFX");
		MusicLevel = FileFetch.FetchFloat("OPTIONS_MUSIC");
		
	}
	
}