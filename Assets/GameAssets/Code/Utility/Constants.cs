using System;
using System.Linq;
using Unity.VisualScripting;

public static class Constants
{
	#region Scene Names
	[System.Serializable]
	public struct SceneNames
	{
		// When adding scene names, add it to the SceneNamesArray as well
		public const string Bootstrap = "Bootstrap";
		public const string MainMenu = "MainMenu";
		public const string PrologueScene1 = "Prologue - Scene 1";
		public const string PrologueScene2 = "Prologue - Scene 2";
		public const string PrologueScene3 = "Prologue - Scene 3";
		public const string PresentScene4Part1 = "Present - Scene 4 - Part 1";
		public const string PresentScene4Part2 = "Present - Scene 4 - Part 2";
		public const string PlaygroundScene5 = "Playground - Scene 5";
		public const string SearchScene7MainStreet = "Search - Scene 7 Main Street";
		public const string SearchScene7Basement = "Search - Scene 7 Basement";
		public const string SearchScene7Forest = "Search - Scene 7 Forest";
		public const string SearchScene7GeneralStore = "Search - Scene 7 General Store";
		public const string SearchScene7LivingRoom = "Search - Scene 7 Living Room";
		public const string SearchScene7Playground = "Search - Scene 7 Playground";
	}

	// Used in SceneChangeMarkerEmitter Editor script to select scenes from dropdown
	public static readonly string[] SceneNamesArray = new string[]
		{ 
			SceneNames.Bootstrap, 
			SceneNames.MainMenu, 
			SceneNames.PrologueScene1,
			SceneNames.PrologueScene2, 
			SceneNames.PrologueScene3,
			SceneNames.PresentScene4Part1, 
			SceneNames.PresentScene4Part2,
			SceneNames.PlaygroundScene5,
            SceneNames.SearchScene7MainStreet,
			SceneNames.SearchScene7Basement,
			SceneNames.SearchScene7Forest,
			SceneNames.SearchScene7GeneralStore,
			SceneNames.SearchScene7LivingRoom,
			SceneNames.SearchScene7Playground,
		};

	public enum Scene7SubScenes
	{
		MainStreet = 0,
		GeneralStore = 1,
		Basement = 2,
		Forest = 3,
		LivingRoom = 4,
		Playground = 5,
	}
	#endregion

    #region Audio
    public struct Audio
	{
		public const float MusicFadeSpeed = 2.5f;
		public const float DefaultAudioLevel = 0.25f;

		public const string MusicVolumePP = "MusicVolume";
		public const string SFXVolumePP = "SFXVolume";
	}
	#endregion Audio

	#region Sanity
	public struct Sanity
	{
		public const int DefaultSanityLevel = 0;
		public const int MinimumSanityLevel = -10;
		public const int MaximumSanityLevel = 10;
		public enum SanityType { Neutral = 0 , Negative = -1 , Positive = 1 }
	}
	#endregion Sanity
}
