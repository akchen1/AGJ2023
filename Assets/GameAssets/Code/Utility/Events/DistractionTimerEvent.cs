using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DistractionTimerEvent : MonoBehaviour
{
	public class SetDistracitonTime
	{
		public SetDistracitonTime(float distractionTime)
		{
            DistractionTime = distractionTime;
		}
        public readonly float DistractionTime;
    }
}
