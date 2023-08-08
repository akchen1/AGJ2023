using DS.ScriptableObjects;
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

	public class GetCurrentSubScene
	{
		public readonly Action<Constants.Scene7SubScenes> Subscene;

		public GetCurrentSubScene(Action<Constants.Scene7SubScenes> subscene)
		{
			Subscene= subscene;
		}
	}

	public class SetBaduSubsceneDialogue
	{
		public readonly DSDialogueSO Dialogue;

        public SetBaduSubsceneDialogue(DSDialogueSO dialogue)
        {
			Dialogue = dialogue;
        }
    }

    public class SetMaeveSubsceneDialogue
    {
        public readonly DSDialogueSO Dialogue;

        public SetMaeveSubsceneDialogue(DSDialogueSO dialogue)
        {
            Dialogue = dialogue;
        }
    }

	public class SetBaduPosition
	{
		public readonly Vector3 Position;

        public SetBaduPosition(Vector3 position)
        {
            Position = position;
        }
    }

    public class SetMaevePosition
    {
        public readonly Vector3 Position;

        public SetMaevePosition(Vector3 position)
        {
            Position = position;
        }
    }

	public class EnableBaduMovement
	{
		public readonly bool Enable;

		public EnableBaduMovement(bool enable)
		{
			Enable = enable;
		}
	}

	public class EnableMaeveMovement
	{
		public readonly bool Enable;

		public EnableMaeveMovement(bool enable)
		{
			Enable = enable;
		}
	}
}
