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

        gameplayScene = scene;
    }

    private void Start()
    {
        gameplayScene = SceneManager.GetActiveScene();
    }

    public void LoadLevel(string name)
    {
        SceneManager.UnloadSceneAsync(gameplayScene);
        var op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        gameplayScene = SceneManager.GetSceneByName(name);
    }

    [ContextMenu("Reload Scene")]
    public void ReloadLevel()
    {
        LoadLevel("Gameplay");
    }
}
