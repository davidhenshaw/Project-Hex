using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : PersistentSingleton<SceneLoader>
{
    Scene gameplayScene;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Additive)
            return;

        if(scene.name.Contains("Level"))
            gameplayScene = scene;
    }

    private void Start()
    {
        gameplayScene = SceneManager.GetActiveScene();

        Scene uiScene = SceneManager.GetSceneByName("Gameplay UI");
        if (!uiScene.isLoaded)
        {
            SceneManager.LoadSceneAsync("Gameplay UI", LoadSceneMode.Additive);
        }
    }

    public void LoadLevel(int buildIndex)
    {
        SceneManager.UnloadSceneAsync(gameplayScene);
        var op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

        gameplayScene = SceneManager.GetSceneByBuildIndex(buildIndex);
    }

    public void LoadLevel(string name)
    {
        var scene = SceneManager.GetSceneByName(name);

        LoadLevel(scene.buildIndex);
    }

    public void LoadNextLevel()
    {
        var currIndex = gameplayScene.buildIndex;
        var nextIndex = currIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            return;

        LoadLevel(nextIndex);
    }

    [ContextMenu("Reload Scene")]
    public void ReloadLevel()
    {
        LoadLevel(gameplayScene.name);
    }

}
