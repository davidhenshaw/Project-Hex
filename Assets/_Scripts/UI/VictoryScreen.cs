using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : UIScreen
{
    public Button restartButton;
    public Button nextLevelButton;
    public Button mainMenuButton;

    // Start is called before the first frame update
    protected override void Start()
    {
        if (restartButton)
            restartButton.onClick.AddListener(() => { SceneLoader.Instance.ReloadLevel(); });

        if (nextLevelButton)
        {
            if (SceneLoader.Instance.CurrentLevel + 1 > SceneLoader.Instance.levels.Count)
            {
                Destroy(nextLevelButton.gameObject);
            }
            else
            {
                nextLevelButton.onClick.AddListener(() => { SceneLoader.Instance.LoadNextLevel(); });
            }

        }

        if (mainMenuButton)
            mainMenuButton.onClick.AddListener(() => { SceneLoader.Instance.LoadLevel(-1); });
    }

    public override void Open()
    {
        base.Open();
        if (SceneLoader.Instance.CurrentLevel + 1 > SceneLoader.Instance.levels.Count-1)
        {
            Destroy(nextLevelButton.gameObject);
        }
    }
}
