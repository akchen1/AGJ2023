using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityEvents
{
	public class ChangeSanity
	{
		public ChangeSanity(int amount)
		{
			Amount = amount;
		}

		public readonly int Amount;
	}

	public class GetSanity
	{
		public GetSanity(Action<int> processData)
		{
			ProcessData = processData;
		}

		public readonly Action<int> ProcessData;
	}
}
