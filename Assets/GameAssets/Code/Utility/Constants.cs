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
		public const string MainstreetScene6Part1 = "MainStreet - Scene 6 - Part 1";
		public const string BasementScene6Part2 = "Basement - Scene 6 - Part 2";
		public const string SearchScene7MainStreet = "Search - Scene 7 Main Street";
		public const string ClearingScene9 = "Clearing - Scene 9";
		public const string EndingScene = "EndScreen";
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
			SceneNames.MainstreetScene6Part1,
			SceneNames.BasementScene6Part2,
            SceneNames.SearchScene7MainStreet,
			SceneNames.ClearingScene9,
			SceneNames.EndingScene
        };

	public enum Scene7SubScenes
	{
		MainStreet = 0,
		GeneralStore = 1,
		Basement = 2,
		Forest = 3,
		LivingRoom = 4,
		Playground = 5
	}
	#endregion

    #region Audio
    public struct Audio
	{
		public const float MusicFadeSpeed = 2.5f;
		public const float DefaultAudioLevel = 0.25f;

		public const string MusicVolumePP = "MusicVolume";
		public const string SFXVolumePP = "SFXVolume";

		public struct Music
		{
			public const string MainMenu = "MainMenu";
			public const string Clearing = "Clearing";
			public const string EndingTree = "EndingTree";
			public const string Forest = "Forest";
			public const string LivingRoom = "LivingRoom";
			public const string Prologue = "Prologue";
			public const string RecordPlayer = "RecordPlayer";
			public const string Basement = "Basement";
			public const string GeneralStore = "GeneralStore";
			public const string MainStreet = "MainStreet";
			public const string Playtime = "Playtime";
		}

		public struct SFX
		{
			public const string Click = "Click";
			public const string PieceLift = "PieceLift";
			public const string PiecePlace = "PiecePlace";
			public const string PieceRotate = "PieceRotate";
			public const string ScrollClose = "ScrollClose";
			public const string ScrollFlip = "ScrollFlip";
			public const string ScrollOpen = "ScrollOpen";
			public const string DrawerOpen = "DrawerOpen";
			public const string DrawerClose = "DrawerClose";
			public const string BucketCollect = "BucketCollect";
			public const string CandleCollect = "CandleCollect";
			public const string LitCandleCollect = "LitCandleCollect";
            public const string ClayCollect = "ClayCollect";
			public const string PocketKnifeCollect = "PocketKnifeCollect";
			public const string Dig = "Dig";
			public const string DigBarHit = "DigBarHit";
			public const string MatchLight = "MatchLight";
			public const string MatchStrike = "MatchStrike";
			public const string MatchboxCollect = "MatchboxCollect";
			public const string MatchboxOpen = "MatchboxOpen";
			public const string ShovelCollect = "ShovelCollect";
			public const string TwineCollect = "TwineCollect";
			public const string VaseShatter = "VaseShatter";
			public const string VialCollect = "VialCollect";
			public const string BloodVialCollect = "BloodVialCollect";
			public const string BranchBreak = "BranchBreak";
			public const string BranchCollect = "BranchCollect";
			public const string GemCollect = "GemCollect";
			public const string DialTurn = "DialTurn";
			public const string BoxOpen = "boxOpen";
			public const string BottleHit = "BottleHit";
			public const string BroomCrash = "BroomCrash";
			public const string Shears = "Shears";
			public const string ToyCarRoll = "ToyCarRoll";
			public const string BloodDrop = "BloodDrop";
			public const string ItemCombine = "ItemCombine";
		}
	}
	#endregion Audio

	#region Sanity
	public struct Sanity
	{
		public const int DefaultSanityLevel = 0;
		public const int MinimumSanityLevel = -15;
		public const int MaximumSanityLevel = 15;
		public enum SanityType { Neutral = 0 , Negative = -1 , Positive = 1 }
	}
    #endregion Sanity

    #region Interaction
	public struct Interaction
	{
		public enum InteractionType { World, Virtual }
	}
    #endregion

    #region Inventory

	public struct Inventory
	{
		public const int InventoryScrollPadding = 30;
	}
    #endregion
}
