public static class Constants
{
    #region Scene Names
    public struct SceneNames
    {
        public const string Bootstrap = "Bootstrap";
        public const string MainMenu = "MainMenu";
    }
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
