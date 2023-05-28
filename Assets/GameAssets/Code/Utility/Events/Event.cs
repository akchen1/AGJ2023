using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Event
{
    #region Audio Events
    public class PlayMusic
    {
        public readonly string MusicName;
        public PlayMusic(string musicName)
        {
            MusicName = musicName;
        }
    }

    public class PlaySFX 
    {
        public readonly string SFXName;
        public PlaySFX(string sfxName)
        {
            SFXName = sfxName;
        }
    }
    #endregion

    #region Cutscene Events
    public class PlayCutscene
    {
        // TODO: Implement
    }
    #endregion

    #region Dialogue Events
    public class StartDialogue
    {
        // TODO: Implement
    }
    #endregion

    #region Inventory Events
    public class AddItem
    {
        // TODO: Implement
    }
    #endregion

    #region Sanity Events
    public class AddSanity
    {
        // TODO: Implement
    }

    public class SanityChanged
    {
        // TODO: Implement
    }
    #endregion

    #region Scene Events
    public class SceneChange
    {
        public readonly string SceneName;
        public readonly bool UnloadPrevious;

        public SceneChange(string sceneName, bool unloadPrevious = true)
        {
            SceneName = sceneName;
            UnloadPrevious = unloadPrevious;
        }
    }

    public class SceneLoaded
    {
        
    }
    #endregion
}
