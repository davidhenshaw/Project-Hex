using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : PersistentSingleton<SceneLoader>
{
    int currLevel = 0;

    public List<LevelInfo> levels = new List<LevelInfo>();

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name.Contains("LevelBase"))
        {
            StartCoroutine(InstantiateLevelInScene(scene));
        }
    }

    IEnumerator InstantiateLevelInScene(Scene scene)
    {
        SceneManager.SetActiveScene(scene);

        yield return null;

        Instantiate(levels[currLevel].LevelPrefab);
    }

    private void Start()
    {
        currLevel = SceneManager.GetActiveScene().buildIndex;
    }

    public void LoadLevel(LevelInfo levelInfo)
    {
        LoadLevelBase();
        currLevel = levels.IndexOf(levelInfo);
    }

    public void LoadLevel(int levelNum)
    {
        LoadLevelBase();
        currLevel = levelNum;
    }

    public void LoadLevelBase()
    {
        SceneManager.LoadSceneAsync("LevelBase", LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        var currIndex = currLevel;
        var nextIndex = currIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings - 1)
            return;

        LoadLevel(nextIndex);
    }

    public void LoadPrevLevel()
    {
        var currIndex = currLevel;
        var prevIndex = currIndex - 1;

        if (prevIndex < 0)
            return;

        LoadLevel(prevIndex);
    }

    [ContextMenu("Reload Scene")]
    public void ReloadLevel()
    {
        LoadLevel(currLevel);
    }
}
