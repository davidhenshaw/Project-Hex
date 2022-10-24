using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : PersistentSingleton<SceneLoader>
{
    int currLevel = 0;

    public List<LevelInfo> levels = new List<LevelInfo>();

    private void Start()
    {
        currLevel = SceneManager.GetActiveScene().buildIndex;

        GameEvents.Instance.NextLevelLoadTrigger.AddListener( LoadNextLevel );
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        //GameEvents.Instance.NextLevelLoadTrigger.RemoveListener( LoadNextLevel );
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

        Instantiate(levels[currLevel].LevelPrefab);

        yield return null;
    }

    public void LoadLevel(LevelInfo levelInfo)
    {
        if (!levelInfo)
        { 
            SceneManager.LoadSceneAsync("Main Menu");
            return;
        }

        LoadLevelBase();
        currLevel = levels.IndexOf(levelInfo);
    }

    public void LoadLevel(int levelNum)
    {
        if (levelNum <= 0)
        {
            SceneManager.LoadSceneAsync("Main Menu");
            return;
        }

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
