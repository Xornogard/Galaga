using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

	#region MEMBERS

	[SerializeField]
	private AudioSource musicSource;

	[SerializeField]
	private AudioClip startMenuMusic;

	[SerializeField]
	private AudioClip inGameMusic;

	[SerializeField]
	private AudioClip gameOverMusic;

	[SerializeField]
	private AudioClip gameWonMusic;

	#endregion

	#region PROPERTIES

	private AudioSource MusicSource => musicSource;
	private AudioClip StartMenuMusic => startMenuMusic;
	private AudioClip InGameMusic => inGameMusic;
	private AudioClip GameOverMusic => gameOverMusic;
	private AudioClip GameWonMusic => gameWonMusic;

	#endregion

	#region FUNCTIONS

	public void PlayStartMenuMusic()
	{
		SetMusicClip(StartMenuMusic);
	}

	public void PlayInGameMusic()
	{
		SetMusicClip(InGameMusic);
	}

	public void PlayGameOverMusic()
	{
		SetMusicClip(GameOverMusic);
	}

	public void PlayGameWonMusic()
	{
		SetMusicClip(GameWonMusic);
	}


	private void SetMusicClip(AudioClip musicClip)
	{
		MusicSource.clip = musicClip;
		MusicSource.Play();
	}

	#endregion

	#region CLASS_ENUMS

	#endregion
}
