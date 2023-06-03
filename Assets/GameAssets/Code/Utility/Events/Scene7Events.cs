using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene7Events
{
    public class ChangeSubscene
	{
		public ChangeSubscene(Constants.Scene7SubScenes subscene)
		{
			Subscene = subscene;
		}

		public readonly Constants.Scene7SubScenes Subscene;
	}
}
