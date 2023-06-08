using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityEvents
{
	public class ChangeSanity
	{
		public ChangeSanity(Constants.Sanity.SanityType sanityType)
		{
            SanityType = sanityType;
		}

		public readonly Constants.Sanity.SanityType SanityType;
    }

	public class GetSanity
	{
		public GetSanity(Action<float> processData)
		{
			ProcessData = processData;
		}

		public readonly Action<float> ProcessData;
	}
}
