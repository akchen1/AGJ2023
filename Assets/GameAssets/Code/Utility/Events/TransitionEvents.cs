public class TransitionEvents
{
    public class FadeScreen
    {
        public readonly bool FadeIn;
        public readonly float FadeDurationSeconds;

        public FadeScreen(bool fadeIn, float fadeDurationSeconds)
        {
            FadeIn = fadeIn;
            FadeDurationSeconds = fadeDurationSeconds;
        }
    }
}
