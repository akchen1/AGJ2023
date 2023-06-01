using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneEvents
{
	public class PlayCutscene
	{
		public PlayableAsset Cutscene;
		public PlayCutscene(PlayableAsset cutscene)
		{
			Cutscene = cutscene;
		}
	}
}
