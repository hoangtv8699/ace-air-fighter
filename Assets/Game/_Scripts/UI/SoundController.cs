using UnityEngine;
using System.Collections;
using System;

 
public sealed class SoundController : MonoBehaviour
{

	public AudioClip ClickSound, CoinsCollectSound, DeadSound;
	
	public static SoundController Static;
	public AudioSource[] audioSources;
	//for audio sources
	public AudioClip[] Clips;
	//for audio clips
	public string[] ClipsName;
	//for audio clip names
	public AudioSource bgSound, CollectedCoins, firingBullets, PropellerEngine;
	//for audio sources

 

	void OnEnable ()
	{
		Time.timeScale = 1;
		
		Static = this;
	}

	//for sound playing acording to clip name
	public void playSoundFromName (string clipName)
	{
		//Debug.Log (" passed clip name " + clipName + "index of it is " +  Array.IndexOf(ClipsName, clipName)    );
		swithAudioSources (Clips [Array.IndexOf (ClipsName, clipName)]);//for audio clips
	}

 
	
	 
	//for click sound
	public void PlayClickSound ()
	{
		
		swithAudioSources (ClickSound);
		
	}
	//for player dead sound
	public void PlayDeadSound ()
	{
		
		swithAudioSources (DeadSound);
		
	}

	public void StopSounds ()
	{
		GetComponent<AudioSource> ().Stop ();
	}

	AudioClip lastclip;
	float lasttime;

	void swithAudioSources (AudioClip clip)
	{
		if (lastclip == clip && Time.timeSinceLevelLoad - lasttime < 1)
			return;

		lastclip = clip;
		lasttime = Time.timeSinceLevelLoad;
		if (audioSources [0].isPlaying) {
			audioSources [1].pitch = UnityEngine.Random.Range (0.9f, 1.1f);
			audioSources [1].PlayOneShot (clip);
		} else {
			
			audioSources [0].pitch = UnityEngine.Random.Range (0.9f, 1.1f);
			audioSources [0].PlayOneShot (clip);
		}
		
	}
	//for bg sound volume
	void StopBG ()
	{
		bgSound.volume = 0;
	}

	 
}


