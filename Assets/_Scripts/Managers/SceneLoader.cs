using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : PersistentSingleton<SceneLoader>
{
    public int CurrentLevel { get; private set; } = 0;

    public List<LevelInfo> levels = new List<LevelInfo>();

    private void Start()
    {
        GameEvents.Instance.NextLevelLoadTrigger.AddListener( LoadNextLevel );
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

        Instantiate(levels[CurrentLevel].LevelPrefab);

        yield return null;
    }

    public LevelInfo GetLevelInfo()
    {
        if (CurrentLevel <= 0)
            return null;

        return levels[CurrentLevel];
    }

    public void LoadLevel(LevelInfo levelInfo)
    {
        if (!levelInfo)
        { 
            SceneManager.LoadSceneAsync("Main Menu");
            return;
        }

        LoadLevelBase();
        CurrentLevel = levels.IndexOf(levelInfo);
    }

    public void LoadLevel(int levelNum)
    {
        if (levelNum < 0)
        {
            SceneManager.LoadSceneAsync("Main Menu");
            return;
        }

        LoadLevelBase();
        CurrentLevel = levelNum;
    }

    public void LoadLevelBase()
    {
        SceneManager.LoadSceneAsync("LevelBase", LoadSceneMode.Single);
    }

    public void LoadNextLevel()
    {
        var nextLevel = CurrentLevel + 1;

        if (nextLevel > levels.Count)
            return;

        LoadLevel(nextLevel);
    }

    public void LoadPrevLevel()
    {
        var currIndex = CurrentLevel;
        var prevIndex = currIndex - 1;

        if (prevIndex < 0)
            return;

        LoadLevel(prevIndex);
    }

    [ContextMenu("Reload Scene")]
    public void ReloadLevel()
    {
        LoadLevel(CurrentLevel);
    }
}
