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

	public class TryNextDialogue
	{

	}

	public class SetRaycastTarget
	{
		public readonly bool Active;

		public SetRaycastTarget(bool active)
		{
			Active = active;
		}
	}
}
