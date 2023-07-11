using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUtility
{
    // Keep track of Async operations
    private Queue<AsyncOperation> operations;
    private HashSet<string> openedScenes;

    private Bootstrap bootstrap;
    private string currentScene;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public SceneUtility(Bootstrap bootstrap)
    {
        operations = new Queue<AsyncOperation>();
        openedScenes = new HashSet<string>();
        InitializeOpenedScenes();

        eventBrokerComponent.Subscribe<SceneEvents.SceneChange>(SceneChangeHandler);
        this.bootstrap = bootstrap;
    }

    ~SceneUtility() 
    {
        eventBrokerComponent.Unsubscribe<SceneEvents.SceneChange>(SceneChangeHandler);
    }

    private void SceneChangeHandler(BrokerEvent<SceneEvents.SceneChange> inEvent)
    {
        bootstrap.StartCoroutine(ChangeScene(inEvent));
    }

    private IEnumerator ChangeScene(BrokerEvent<SceneEvents.SceneChange> inEvent)
    {
        string currentActiveScene = SceneManager.GetActiveScene().name;
        if (inEvent.Payload.UnloadPrevious && currentScene != null)
        {
            eventBrokerComponent.Publish(this, new TransitionEvents.FadeScreen(true, 1f));
            yield return new WaitForSeconds(1f);
            UnloadScene(currentScene);
        }
        LoadScene(inEvent.Payload.SceneName);
        bootstrap.StartCoroutine(WaitForSceneLoad(() =>
        {
            SetActiveScene(inEvent.Payload.SceneName);
            eventBrokerComponent.Publish(this, new TransitionEvents.FadeScreen(false, 1f));
            eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
        }));
    }

    private void LoadScene(string sceneName)
    {
        if (openedScenes.Contains(sceneName))
        {
            Debug.LogError($"{sceneName} is opened already");
			// TODO: Shouldn't return, it should still set the scene as the active scene
            return;
        }

        AsyncOperation addedScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        operations.Enqueue(addedScene);
        openedScenes.Add(sceneName);
        currentScene = sceneName;
    }

    private void UnloadScene(string sceneName)
    {
        if (!openedScenes.Contains(sceneName))
        {
            Debug.LogError($"{sceneName} can not be found");
            return;
        }

        AsyncOperation removedScene = SceneManager.UnloadSceneAsync(sceneName);
        operations.Enqueue(removedScene);
        openedScenes.Remove(sceneName);
    }

    private IEnumerator WaitForSceneLoad(Action onLoadFinished = null)
    {
        while (operations.Count > 0)
        {
            if (operations.Peek().isDone)
            {
                operations.Dequeue();
            }
            yield return null;
        }

        onLoadFinished?.Invoke();
        eventBrokerComponent.Publish(this, new SceneEvents.SceneLoaded());
    }

    private void SetActiveScene(string sceneName)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }


    private void InitializeOpenedScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            openedScenes.Add(SceneManager.GetSceneAt(i).name);
        }
    }
}
