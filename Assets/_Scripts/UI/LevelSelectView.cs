using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectView : MonoBehaviour
{
    [SerializeField]
    Transform buttonArea;

    [SerializeField]
    GameObject levelButtonPrefab;

    private void Start()
    {
        GenerateButtons();
    }

    private void OnDisable()
    {
        DestroyButtons();
    }

    public void GenerateButtons()
    {
        if (!buttonArea)
            return;

        LevelInfo[] levels = SceneLoader.Instance.levels.ToArray();

        for(int i = 0; i < levels.Length; i++)
        {
            var levelButton = Instantiate(levelButtonPrefab, buttonArea);
            var button = levelButton.GetComponent<LevelSelectButton>();

            button.level = levels[i];
            button.InitButton(i+1);
        }
    }

    public void DestroyButtons()
    {
        var buttons = GetComponentsInChildren<LevelSelectButton>();

        foreach(LevelSelectButton button in buttons)
        {
            Destroy(button.gameObject);
        }
    }
}
