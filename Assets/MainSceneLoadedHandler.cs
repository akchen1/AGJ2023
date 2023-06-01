#if UNITY_EDITOR // this script should not be present in builds
using System.Collections;
using SaG.MainSceneAutoLoading;
using SaG.MainSceneAutoLoading.MainSceneLoadedHandlers;
using SaG.MainSceneAutoLoading.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoadedHandler : MonoBehaviour, IMainSceneLoadedHandler
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void OnMainSceneLoaded(LoadMainSceneArgs args)
    {
        StartCoroutine(LoadDesiredScenes(args));
    }

    IEnumerator LoadDesiredScenes(LoadMainSceneArgs args)
    {
        yield return new WaitForSeconds(.5f);
        foreach (var sceneSetup in args.SceneSetups)
        {
            string sceneName = sceneSetup.path.Split("/")[^1].Split(".")[0];
            eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(sceneName));
        }

        // call this to restore previously selected and expanded GameObjects 
        SceneHierarchyStateUtility.StartRestoreHierarchyStateCoroutine(args);
    }
}
#endif