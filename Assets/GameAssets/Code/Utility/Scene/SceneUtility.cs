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

    public SceneUtility(Bootstrap bootstrap)
    {
        operations = new Queue<AsyncOperation>();
        openedScenes = new HashSet<string>();
        InitializeOpenedScenes();

        Bootstrap.EventBrokerComponent.Subscribe<Event.SceneChange>(SceneChangeHandler);
        this.bootstrap = bootstrap;
    }

    ~SceneUtility() 
    {
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.SceneChange>(SceneChangeHandler);
    }

    private void SceneChangeHandler(BrokerEvent<Event.SceneChange> inEvent)
    {
        string currentActiveScene = SceneManager.GetActiveScene().name;
        if (inEvent.Payload.UnloadPrevious && currentScene != null)
        {
            UnloadScene(currentScene);
        }
        LoadScene(inEvent.Payload.SceneName);
        bootstrap.StartCoroutine(WaitForSceneLoad(() =>
        {
            SetActiveScene(inEvent.Payload.SceneName);
        }));
    }


    private void LoadScene(string sceneName)
    {
        if (openedScenes.Contains(sceneName))
        {
            Debug.LogError($"{sceneName} is opened already");
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
        Bootstrap.EventBrokerComponent.Publish<Event.SceneLoaded>(this, new Event.SceneLoaded());
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
