using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : PersistentSingleton<SceneLoader>
{
    int gameplayScene;

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
            gameplayScene = scene.buildIndex;
    }

    private void Start()
    {
        gameplayScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel(int buildIndex)
    {
        var op = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Single);
    }

    public void LoadLevel(string name)
    {
        var scene = SceneManager.GetSceneByName(name);

        LoadLevel(scene.buildIndex);
    }

    public void LoadNextLevel()
    {
        var currIndex = gameplayScene;
        var nextIndex = currIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings - 1)
            return;

        LoadLevel(nextIndex);
    }

    public void LoadPrevLevel()
    {
        var currIndex = gameplayScene;
        var prevIndex = currIndex - 1;

        if (prevIndex < 0)
            return;

        LoadLevel(prevIndex);
    }

    [ContextMenu("Reload Scene")]
    public void ReloadLevel()
    {
        LoadLevel(gameplayScene);
    }
}
