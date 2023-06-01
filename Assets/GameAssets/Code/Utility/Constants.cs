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
    }

	// Used in SceneChangeMarkerEmitter Editor script to select scenes from dropdown
	public static readonly string[] SceneNamesArray = new string[] 
		{ SceneNames.Bootstrap, SceneNames.MainMenu, SceneNames.PrologueScene1, SceneNames.PrologueScene2 };
	#endregion

	#region Audio
	public struct Audio
	{
		public const float MusicFadeSpeed = 2.5f;
		public const float DefaultAudioLevel = 0.25f;
	}
	#endregion Audio

	#region Sanity
	public struct Sanity
	{
		public const int DefaultSanityLevel = 0;
		public const int MinimumSanityLevel = -10;
		public const int MaximumSanityLevel = 10;
	}
	#endregion Sanity
}
