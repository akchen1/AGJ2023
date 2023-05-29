using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvents
{
	public class PlayMusic
	{
		public PlayMusic(string musicName)
		{
			MusicName = musicName;
		}

		public readonly string MusicName;
	}

	public class PlaySFX
	{
		public PlaySFX(string sfxName)
		{
			SFXName = sfxName;
		}

		public readonly string SFXName;
	}
}
