using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEvents
{
	public class SceneChange
	{
		public SceneChange(string sceneName, bool unloadPrevious = true)
		{
			SceneName = sceneName;
			UnloadPrevious = unloadPrevious;
		}

		public readonly string SceneName;
		public readonly bool UnloadPrevious;
	}

	public class SceneLoaded
	{

	}
}
