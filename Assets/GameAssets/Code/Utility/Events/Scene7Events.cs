using System;
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

	public class GetBloodSanityResult
	{
		public readonly Action<Constants.Sanity.SanityType> SanityType;

		public GetBloodSanityResult(Action<Constants.Sanity.SanityType> sanityType)
		{
			SanityType = sanityType;
		}
	}

	public class HasCombinedItems
	{
		public readonly Action<bool> Result;
		public HasCombinedItems(Action<bool> result)
		{
			Result = result;
		}
	}
}
