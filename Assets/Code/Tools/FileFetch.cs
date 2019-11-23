using UnityEngine;
using System.Collections;

using Prime31;

public static class FileFetch  {
	
	//A wrapper class for prefs.
	
	static string _localkey = "LOCAL_";
	
	public static void SetLocalInt(string key, int val)
	{
		PlayerPrefs.SetInt(_localkey + key,val);
		
		
	}
	public static void SetLocalFloat(string key, float val)
	{
		PlayerPrefs.SetFloat(_localkey + key,val);
	}
	
	public static bool HasLocalKey(string key)
	{
		return PlayerPrefs.HasKey(_localkey + key);
	}
	public static float FetchLocalFloat(string key)
	{
		return PlayerPrefs.GetFloat(_localkey + key);
	}
	
	public static void SetInt(string key, int val)
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		P31Prefs.setInt(key,val);
		return;
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		PlayerPrefs.SetInt(key,val);
		
	}
	
	public static void SetFloat(string key, float val)
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		P31Prefs.setFloat(key,val);
		return;
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		PlayerPrefs.SetFloat(key,val);
	}
	
	public static void SetString(string key, string val)
	{
		#if UNITY_IPHONE
		//Use player prefs to set and return.
		P31Prefs.setString(key,val);
		return;
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		PlayerPrefs.SetString(key,val);
		
	}
	
	public static void SetDictionary(string key, IDictionary val)
	{
		#if UNITY_IPHONE
		//Use player prefs to set and return.
		P31Prefs.setDictionary(key, new Hashtable(val));
		return;
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		//PlayerPrefs.SetString(key,val);
		
		var json = Prime31.Json.jsonEncode(val);
		PlayerPrefs.SetString(key,json);
		
	}
	
	
	public static string FetchString(string key)
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		
		return P31Prefs.getString(key);
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		return PlayerPrefs.GetString(key);
	}
	
	public static int FetchInt(string key)
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		
		return P31Prefs.getInt(key);
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		return PlayerPrefs.GetInt(key);
	}
	
	public static IDictionary FetchDictonary(string key)
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		
		return P31Prefs.getDictionary(key);
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		var json = PlayerPrefs.GetString(key);
		return json.dictionaryFromJson();
		
		
	}
	
	
	public static float FetchFloat(string key)
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		
		return P31Prefs.getFloat(key);
#endif
		
		//If we fall here then we havent saved any other way and we will resport to unity default.
		return PlayerPrefs.GetFloat(key);
	}
	
	public static void ClearAllKeys()
	{
		
#if UNITY_IPHONE
		//Use player prefs to set and return.
		
		P31Prefs.removeAll();
		return;
#endif
		
		PlayerPrefs.DeleteAll();
		
	}
	
	public static bool HasKey(string key)
	{
#if UNITY_IPHONE
	
		return P31Prefs.hasKey(key);
		
#endif
		
		return PlayerPrefs.HasKey(key);
	}
	
	public static void Sync()
	{

#if UNITY_IPHONE
		//Use player prefs to set and return.
		P31Prefs.synchronize();
#endif	
		
	}
	
	
	

}
